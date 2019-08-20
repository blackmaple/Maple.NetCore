using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maple.NetCore
{
   // [CDISingleton(typeof(IConfigureOptions<T>))]

    public class CDIConfigureOptions<T> : NamedConfigureFromConfigurationOptions<T>, IConfigureOptions<T> where T : class
    {
        public CDIConfigureOptions(IConfiguration config) : base(Options.DefaultName, config, _ => { })
        {
        }
    }
}
