using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Maple.NetCore
{
    public static class CCommonLogger
    {
        static ILog Logger { get; }
        static CCommonLogger()
        {

            Logger = new CLog4netFactory().GetLogger(nameof(CCommonLogger));
            //var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            //var path = System.IO.Path.Combine(baseDir, "config/log4net.config");
            //var fileInfo = new FileInfo(path);

            //var repository = LogManager.CreateRepository(nameof(CCommonLogger));
            //log4net.Config.XmlConfigurator.Configure(repository, fileInfo);
            //Logger = LogManager.GetLogger(repository.Name, nameof(CCommonLogger));
        }

        public static string GetAllError(this Exception ex)
        {
            StringBuilder sb = new StringBuilder(Environment.NewLine, 1024 * 8);

            do
            {
                var line = $@"Message=>{ex.Message},StackTrace=>{ex.StackTrace}";
                sb.AppendLine(line);
                ex = ex.InnerException;
            } while (ex != null);
            var msg = sb.ToString();
            sb.Clear();
            return msg;
        }

        public static void ErrorPrintf(this Exception ex)
        {
            ex.GetAllError().ErrorPrintf();
        }

        public static void ErrorPrintf(this string msg)
        {
            Logger.Error(msg);
        }

        public static void DebugPrintf(this string msg)
        {
            Logger.Info(msg);
        }


    }
}
