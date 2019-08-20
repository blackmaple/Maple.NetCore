using log4net;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Maple.NetCore
{
    public sealed class CCommonRunning : Stopwatch, IDisposable
    {
        string PrintCodeName { get; }
        ILog Log { get; }
        public CCommonRunning([CallerMemberName]string codeName = null) : this(default, codeName)
        {

        }

        public CCommonRunning(ILog log, [CallerMemberName]string codeName = null)
        {
            this.PrintCodeName = codeName;
            this.Log = log;

            this.DebugPrintf($@"{this.PrintCodeName} Start...");
            this.Start();
        }

        public void Dispose()
        {
            this.Stop();
            this.DebugPrintf($@"{this.PrintCodeName} Exit {this.ElapsedMilliseconds.ToString()}ms");
        }

        private void DebugPrintf(string msg)
        {
            if (this.Log == null)
            {
                msg.DebugPrintf();
            }
            else
            {
                this.Log.Info(msg);
            }
        }
    }


}
