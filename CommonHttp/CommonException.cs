using System;
using System.Collections.Generic;
using System.Text;

namespace Maple.NetCore
{
    public class CCommonException : Exception
    {
        public int Code { set; get; }
        public CCommonException(int code, string message) : base(message)
        {
            this.Code = code;
        }
        public CCommonException(string message) : base(message)
        {
            this.Code = (int)EnumCommonHttpStatus.ERROR;
        }


    }
}
