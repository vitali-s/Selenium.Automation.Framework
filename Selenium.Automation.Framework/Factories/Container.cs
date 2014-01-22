using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Selenium.Automation.Framework.Factories
{
    /// <summary>
    /// Manages types and their implementations.
    /// </summary>
    public class Container
    {
        private static readonly object LockObject = new object();

        private readonly Hashtable _registrations = new Hashtable();
        private readonly Hashtable _instances = new Hashtable();
        private readonly Hashtable _constructors = new Hashtable();

        public Container()
        {
            Register(Assembly.GetExecutingAssembly());
        }

        public void Register(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();

            for (int index = 0; index < types.Length; index++)
            {
                Type type = types[index];

                if (type.IsAssignableFrom(typeof(View)))
                {
                    Register(type, type, LifeTypes.PerDependency);
                }
                else
                {
                    Register(type, type, LifeTypes.Single);
                }
            }
        }

        public void Register<TInterface, TImplementation>(LifeTypes lifeType = LifeTypes.Single)
        {
            Register(typeof(TInterface), typeof(TImplementation), lifeType);
        }

        public void Register<TInterface>(object instance, LifeTypes lifeType = LifeTypes.Single)
        {
            Type type = instance.GetType();

            Register(typeof(TInterface), type, lifeType);

            _instances[type] = instance;
        }

        public void Register(Type service, Type implementation, LifeTypes lifeType = LifeTypes.Single)
        {
            if (service.IsGenericType && service.FullName == null)
            {
                service = service.GetGenericTypeDefinition();
            }

            _registrations[service] = new Registration(implementation, lifeType);
        }

        public TInterface Resolve<TInterface>(params object[] parameters)
        {
            return (TInterface)Resolve(typeof(TInterface), parameters);
        }

        public object Resolve(Type registrationType, params object[] parameters)
        {
            object instance = ResolveInternal(registrationType, parameters);

            // Try to register appropriate assembly
            if (instance == null)
            {
                Register(registrationType.Assembly);

                instance = ResolveInternal(registrationType, parameters);
            }

            return instance;
        }

        protected object ResolveInternal(Type registrationType, params object[] parameters)
        {
            Type typeToGet = null;

            var registration = _registrations[registrationType] as Registration;

            if (registration == null)
            {
                if (registrationType.IsGenericType)
                {
                    registration = _registrations[registrationType.GetGenericTypeDefinition()] as Registration;

                    if (registration == null)
                    {
                        throw new ArgumentNullException();
                    }

                    typeToGet = registration.Type.MakeGenericType(registrationType.GetGenericArguments().Take(registration.Type.GetGenericArguments().Length).ToArray());
                }
                else
                {
                    return null;
                }
            }
            else
            {
                typeToGet = registration.Type;
            }

            switch (registration.LifeType)
            {
                case LifeTypes.Single:
                default:
                    return ResolveSingleInstance(typeToGet, parameters);

                case LifeTypes.PerDependency:
                    return ResolvePerDependencyInstance(typeToGet, parameters);
            }
        }

        protected object ResolveSingleInstance(Type registrationType, object[] parameters)
        {
            object instance = _instances[registrationType];

            if (instance == null)
            {
                instance = CreateInstance(registrationType, parameters);

                lock (LockObject)
                {
                    _instances[registrationType] = instance;
                }
            }

            return instance;
        }

        protected object ResolvePerDependencyInstance(Type registrationType, object[] parameters)
        {
            return CreateInstance(registrationType, parameters);
        }

        protected object CreateInstance(Type type, params object[] additionalParameters)
        {
            var constructor = _constructors[type] as Constructor;

            if (constructor == null)
            {
                ConstructorInfo[] constructors = type.GetConstructors();

                ConstructorInfo currentConstruction = constructors.First();

                if (constructors.Length > 1)
                {
                    ConstructorInfo emptyConstructor = type.GetConstructor(Type.EmptyTypes);

                    if (emptyConstructor != null)
                    {
                        currentConstruction = emptyConstructor;
                    }
                }

                constructor = new Constructor(currentConstruction);

                lock (LockObject)
                {
                    _constructors[type] = constructor;
                }
            }

            var objects = new List<object>();

            for (int index = 0; index < constructor.Parameters.Length; index++)
            {
                object parameter = Resolve(constructor.Parameters[index].ParameterType);

                if (parameter != null)
                {
                    objects.Add(parameter);
                }
            }

            objects.AddRange(additionalParameters);

            return constructor.ConstructorInfo.Invoke(objects.ToArray());
        }
    }
}
