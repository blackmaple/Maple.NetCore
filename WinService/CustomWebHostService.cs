using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceProcess;
using System.Text;

namespace Maple.NetCore
{
    public static class CWebHostServiceExtensions
    {
        public static void RunAsCustomService(this IWebHost host)
        {
            var webHostService = new CCustomWebHostService(host);
            ServiceBase.Run(webHostService);
        }

 
    }




    [DesignerCategory("Code")]
    internal class CCustomWebHostService : WebHostService
    {


        public CCustomWebHostService(IWebHost host) : base(host)
        {

        }

        protected override void OnStarting(string[] args)
        {
            "OnStarting method called.".DebugPrintf();
            base.OnStarting(args);
        }

        protected override void OnStarted()
        {
            "OnStarted method called.".DebugPrintf();
            base.OnStarted();
        }

        protected override void OnStopping()
        {
            "OnStopping method called.".DebugPrintf();
            base.OnStopping();
        }
    }

}
