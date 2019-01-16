using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SendMail.Utils
{
    class Configuration
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IConfigurationRoot config;

        //load the configuration file
        public Configuration()
        {

            try
            {
                Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

                string path = Directory.GetCurrentDirectory();

                IConfigurationBuilder builder = new ConfigurationBuilder()
                    .SetBasePath(path)
                    .AddJsonFile("appsetting.json", optional: true, reloadOnChange: true);

                config = builder.Build();
            }
            catch(Exception e)
            {
                log.Error($"Error in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\n" + e.Message);
            }
            
        }

        //takes an item from the configuration file
        public string GetField(string name)
        {
            return config[name];
        }

    }
}
