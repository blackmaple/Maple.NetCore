using System;
using System.Collections.Generic;
using System.Text;

namespace Maple.NetCore
{
    public class CCommonHttpDTO : CCommonGenericHttpDTO<string>
    {
        public const string HeaderName = nameof(Maple.NetCore);
        public static CCommonHttpDTO GetBadSession()
        {
            return new CCommonHttpDTO()
            {
                Code = (int)EnumCommonHttpStatus.BADSESSION,
                Msg = "请重新登录"
            };
        }

        public static CCommonHttpDTO GetError(int code, string msg)
        {
            return new CCommonHttpDTO()
            {
                Code = code,
                Msg = msg,
            };
        }

        public static CCommonHttpDTO GetError(CCommonException commonException)
        {
            return new CCommonHttpDTO()
            {
                Code = commonException.Code,
                Msg = commonException.Message,
            };
        }

        public static CCommonHttpDTO GetOk(string data)
        {
            return new CCommonHttpDTO()
            {
                Code = (int)EnumCommonHttpStatus.OK,
                Data = data,
                Msg = "成功"
            };
        }

    }

}
