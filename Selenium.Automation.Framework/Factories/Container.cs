using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Selenium.Automation.Framework.Factories
{
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

                if (type.IsGenericType && type.FullName == null)
                {
                    type = type.GetGenericTypeDefinition();
                }

                _registrations[type] = type;
            }
        }

        public TDependency Resolve<TDependency>(params object[] parameters)
        {
            return (TDependency)Resolve(typeof(TDependency), parameters);
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

            var registration = _registrations[registrationType] as Type;

            if (registration == null)
            {
                if (registrationType.IsGenericType)
                {
                    registration = _registrations[registrationType.GetGenericTypeDefinition()] as Type;

                    if (registration == null)
                    {
                        throw new ArgumentNullException();
                    }

                    typeToGet = registration.MakeGenericType(registrationType.GetGenericArguments().Take(registration.GetGenericArguments().Length).ToArray());
                }
                else
                {
                    return null;
                }
            }
            else
            {
                typeToGet = registration;
            }

            object instance = ResolveSingleInstance(typeToGet, parameters);

            return instance;
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

        protected object CreateInstance(Type type, params object[] additionalParameters)
        {
            var constructor = _constructors[type] as Constructor;

            if (constructor == null)
            {
                ConstructorInfo[] constructors = type.GetConstructors();

                if (constructors.Length != 0)
                {
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
