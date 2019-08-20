using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maple.NetCore
{

    //[CDISingleton(typeof(IOptionsChangeTokenSource<T>))]
    public class CDIOptionsChangeTokenSource<T> : ConfigurationChangeTokenSource<T>, IOptionsChangeTokenSource<T>
    {
        public CDIOptionsChangeTokenSource(IConfiguration config) : base(Options.DefaultName, config)
        {
        }

    }
}
