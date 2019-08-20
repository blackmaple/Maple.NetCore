using log4net;
using log4net.Core;
using Scrutor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maple.NetCore
{
    //[ServiceDescriptor(typeof(IGenericLog4net<>), Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton)]
    [CDISingleton(typeof(IGenericLog4net<>))]
    public class CGenericLog4net<T> : IGenericLog4net<T>
    {
        static string TypeClassFullName { get; } = typeof(T).FullName;
        ILog Log { get; }

        public bool IsDebugEnabled => this.Log.IsDebugEnabled;

        public bool IsInfoEnabled => this.Log.IsInfoEnabled;

        public bool IsWarnEnabled => this.Log.IsWarnEnabled;

        public bool IsErrorEnabled => this.Log.IsErrorEnabled;

        public bool IsFatalEnabled => this.Log.IsFatalEnabled;

        public ILogger Logger => this.Log.Logger;

        public CGenericLog4net(ILog4netFactory log4netFactory)
        {
            this.Log = log4netFactory.GetLogger(TypeClassFullName);
        }

        public void Debug(object message)
        {
            this.Log.Debug(message);

        }

        public void Debug(object message, Exception exception)
        {
            this.Log.Debug(message, exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            this.Log.DebugFormat(format, args);
        }

        public void DebugFormat(string format, object arg0)
        {
            this.Log.DebugFormat(format, arg0);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            this.Log.DebugFormat(format, arg0, arg1);
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            this.Log.DebugFormat(format, arg0, arg1, arg2);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.Log.DebugFormat(provider, format, args);
        }

        public void Info(object message)
        {
            this.Log.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            this.Log.Info(message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            this.Log.InfoFormat(format, args);
        }

        public void InfoFormat(string format, object arg0)
        {
            this.Log.InfoFormat(format, arg0);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            this.Log.InfoFormat(format, arg0, arg1);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            this.Log.InfoFormat(format, arg0, arg1, arg2);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.Log.InfoFormat(provider, format, args);
        }

        public void Warn(object message)
        {
            this.Log.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            this.Log.Warn(message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            this.Log.WarnFormat(format, args);
        }

        public void WarnFormat(string format, object arg0)
        {
            this.Log.WarnFormat(format, arg0);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            this.Log.WarnFormat(format, arg0, arg1);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            this.Log.WarnFormat(format, arg0, arg1, arg2);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.Log.WarnFormat(provider, format, args);
        }

        public void Error(object message)
        {
            this.Log.Debug(message);
        }

        public void Error(object message, Exception exception)
        {
            this.Log.Error(message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            this.Log.ErrorFormat(format, args);
        }

        public void ErrorFormat(string format, object arg0)
        {
            this.Log.ErrorFormat(format, arg0);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            this.Log.ErrorFormat(format, arg0, arg1);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            this.Log.ErrorFormat(format, arg0, arg1, arg2);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.Log.ErrorFormat(provider, format, args);
        }

        public void Fatal(object message)
        {
            this.Log.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            this.Log.Fatal(message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            this.Log.FatalFormat(format, args);
        }

        public void FatalFormat(string format, object arg0)
        {
            this.Log.FatalFormat(format, arg0);
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            this.Log.FatalFormat(format, arg0, arg1);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            this.Log.FatalFormat(format, arg0, arg1, arg2);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.Log.FatalFormat(provider, format, args);
        }
    }
}
