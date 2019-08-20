using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maple.NetCore
{
    public interface ILog4netFactory
    {
        ILog GetLogger(string typeClassName);
    }
}
