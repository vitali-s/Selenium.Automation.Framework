using System;
using System.IO;
using Newtonsoft.Json;

namespace Selenuim.Automation.Framework
{
    public class ModelBuilder
    {
        private readonly Configuration _configuration;

        public ModelBuilder(Configuration configuration)
        {
            _configuration = configuration;
        }

        public TModel ReadModel<TModel>(string caseName)
        {
            string fileName = typeof(TModel).Name;

            if (!string.IsNullOrEmpty(caseName))
            {
                fileName = string.Format("{0}-{1}", fileName, caseName);
            }

            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                _configuration.TestDataPath,
                string.Format("{0}.json", fileName));

            string content = File.ReadAllText(path);

            var model = JsonConvert.DeserializeObject<TModel>(content);

            return model;
        }
    }
}
