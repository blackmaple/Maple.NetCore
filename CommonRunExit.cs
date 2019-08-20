using System;
using System.Collections.Generic;
using System.Text;

namespace Maple.NetCore
{
    public class CCommonRunExit : IDisposable
    {
        Action ActionExit { get; }
        public CCommonRunExit(Action begin, Action exit)
        {
            begin.Invoke();
            ActionExit = exit;
        }

        public void Dispose()
        {
            this.Dispose(true);

        }
        protected virtual void Dispose(bool disposing)
        {
            ActionExit.Invoke();
        }
    }



}
