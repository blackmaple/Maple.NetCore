using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Maple.NetCore
{

    //[ServiceDescriptor(typeof(ILog4netFactory), ServiceLifetime.Singleton)]
    [CDISingleton(typeof(ILog4netFactory))]
    public class CLog4netFactory : ILog4netFactory
    {
        protected virtual PatternLayout PatternLayout { get; }
        protected virtual TraceAppender TraceAppender { get; }
        protected virtual ConsoleAppender ConsoleAppender { get; }
        protected virtual ConcurrentDictionary<string, ILog> DictionaryLog { get; }

        public CLog4netFactory()
        {
            PatternLayout = new PatternLayout()
            {
                //ConversionPattern = "%date{yyyy.MM.dd HH:mm:ss ffff} [%5thread] %-5level %logger - %message%newline"
                ConversionPattern = "%date{yyyy.MM.dd HH:mm:ss ffff} [%5thread] %-5level:%message%newline"
            };
            PatternLayout.ActivateOptions();

            ConsoleAppender = new ConsoleAppender()
            {
                Layout = PatternLayout
            };
            ConsoleAppender.ActivateOptions();

            TraceAppender = new TraceAppender()
            {
                Layout = PatternLayout
            };
            TraceAppender.ActivateOptions();

            DictionaryLog = new ConcurrentDictionary<string, ILog>(8, 32);
        }

        protected virtual ILog AddLog(string typeClassName)
        {
            var rollingFileAppender = new RollingFileAppender
            {
                Layout = PatternLayout
            };

            rollingFileAppender.File = System.IO.Path.Combine(AppContext.BaseDirectory, "LogFiles");
            rollingFileAppender.AppendToFile = true;
            rollingFileAppender.RollingStyle = RollingFileAppender.RollingMode.Composite;
            rollingFileAppender.StaticLogFileName = false;
            rollingFileAppender.DatePattern = $@"/yyyyMM/yyyyMMdd_HH_'{typeClassName}.log'";
            rollingFileAppender.MaxSizeRollBackups = 10;
            rollingFileAppender.MaximumFileSize = "16MB";
            rollingFileAppender.ActivateOptions();

            var repository = LogManager.CreateRepository(typeClassName);
            if (repository is Hierarchy hierarchy)
            {
                hierarchy.Root.AddAppender(rollingFileAppender);
#if DEBUG
                hierarchy.Root.AddAppender(TraceAppender);
#endif
                if (Console.In != System.IO.StreamReader.Null)
                {
                    hierarchy.Root.AddAppender(ConsoleAppender);
                }

                hierarchy.Root.Level = log4net.Core.Level.All;
                hierarchy.Configured = true;
            }
            return LogManager.GetLogger(repository.Name, typeClassName);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public ILog GetOrAddLog(string typeClassName)
        {
            return DictionaryLog.GetOrAdd(typeClassName, (p) =>
             {
                 return AddLog(p);
             });
        }

        public ILog GetLogger(string typeClassName)
        {
            return GetOrAddLog(typeClassName);
        }

    }

}
