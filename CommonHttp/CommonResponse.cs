using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maple.NetCore
{
    public static class CCommonResponse
    {
        public static Task WriteResponseError(this HttpResponse response, Exception exception)
        {
            string json;
            if (exception is CCommonException commonException)
            {
                json = CCommonHttpDTO.GetError(commonException).CommonToJson();
            }
            else
            {
                var errorMsg = $@"系统错误:{DateTime.Now:yyyyMMddHHmmss}";
                json = CCommonHttpDTO.GetError((int)EnumCommonHttpStatus.ERROR, errorMsg).CommonToJson();
            }
            return response.WriteResponse(json);
        }

        public static Task WriteResponseBabSession(this HttpResponse response)
        {
            var json = CCommonHttpDTO.GetBadSession().CommonToJson();
            return response.WriteResponse(json);
        }

        public static Task WriteResponseStatusError(this HttpResponse response)
        {
            var errorMsg = $@"系统错误:{DateTime.Now:yyyyMMddHHmmss}|错误代码:{response.StatusCode}";
            var json = CCommonHttpDTO.GetError((int)EnumCommonHttpStatus.ERROR, errorMsg).CommonToJson();
            return response.WriteResponse(json);
        }

        public static Task WriteResponse(this HttpResponse response, string json)
        {
            response.StatusCode = StatusCodes.Status200OK;
            response.ContentType = "application/json;charset=utf-8";
            return response.WriteAsync(json);
        }
    }
}
