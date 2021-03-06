﻿using log4net;
using log4net.Config;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace Crawler
{
    public static class Log4NetHelper
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Log4NetHelper));
        public static String configFilePath = "Settings\\log4net.config";

        public static void Log(this Object log, CfLogLevel level = CfLogLevel.DEBUG)
        {
            if (!logger.Logger.Repository.Configured)
            {
                var assembly = Assembly.GetEntryAssembly();
                var logRepository = LogManager.GetRepository(assembly);
                XmlConfigurator.Configure(logRepository, new FileInfo(configFilePath));
            }

            string json = JsonConvert.SerializeObject(log,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            switch (level)
            {
                case CfLogLevel.DEBUG:
                    logger.Debug(json);
                    break;
                case CfLogLevel.INFO:
                    logger.Info(json);
                    break;
                case CfLogLevel.WARN:
                    logger.Warn(json);
                    break;
                case CfLogLevel.ERROR:
                    logger.Error(json);
                    break;
                case CfLogLevel.FATAL:
                    logger.Fatal(json);
                    break;
                default:
                    break;
            }
        }
    }

    public enum CfLogLevel
    {
        ALL = 1,
        DEBUG = 2,
        INFO = 4,
        WARN = 8,
        ERROR = 16,
        FATAL = 32,
        OFF = 0
    }

}
