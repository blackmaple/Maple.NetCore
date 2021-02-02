using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maple.NetCore.ServiceException
{
    public class CMapleServiceException : Exception
    {
        public int Code { set; get; }
        public CMapleServiceException(string msg) : base(msg) { }
    }

    public static class CMapleServiceResponseExtensions
    {
        public static Task WriteResponseError(this HttpResponse response, Exception exception)
        {
            string json;
            if (exception is CMapleServiceException commonException)
            {
                json = CMapleServiceHttpDTO.GetError(commonException).Object2Json();
            }
            else
            {
                var errorMsg = $@"系统错误:{DateTime.Now:yyyyMMddHHmmss}";
                json = CMapleServiceHttpDTO.GetError((int)EnumMapleServiceHttpStatus.ERROR, errorMsg).Object2Json();
            }
            return response.WriteResponse(json);
        }

        //public static Task WriteResponseBabSession(this HttpResponse response)
        //{
        //    var json = CMapleServicettpDTO.GetBadSession().CommonToJson();
        //    return response.WriteResponse(json);
        //}

        public static Task WriteResponseStatusError(this HttpResponse response)
        {
            var errorMsg = $@"系统错误:{DateTime.Now:yyyyMMddHHmmss}|错误代码:{response.StatusCode}";
            var json = CMapleServiceHttpDTO.GetError((int)EnumMapleServiceHttpStatus.ERROR, errorMsg).Object2Json();
            return response.WriteResponse(json);
        }

        public static Task WriteResponse(this HttpResponse response, string json)
        {
            response.StatusCode = StatusCodes.Status200OK;
            response.ContentType = "application/json;charset=utf-8";
            return response.WriteAsync(json);
        }
    }

    public class CMapleServiceGenericHttpDTO<T>
    {
        public int Code { set; get; }
        public T Data { set; get; }
        public string Msg { set; get; }




    }

    public class CMapleServiceHttpDTO : CMapleServiceGenericHttpDTO<string>
    {
        public static CMapleServiceHttpDTO GetError(int code, string msg)
        {
            return new CMapleServiceHttpDTO()
            {
                Code = code,
                Msg = msg,
            };
        }

        public static CMapleServiceHttpDTO GetError(CMapleServiceException commonException)
        {
            return new CMapleServiceHttpDTO()
            {
                Code = commonException.Code,
                Msg = commonException.Message,
            };
        }

        public static CMapleServiceHttpDTO GetOk(string data)
        {
            return new CMapleServiceHttpDTO()
            {
                Code = (int)EnumMapleServiceHttpStatus.OK,
                Data = data,
                Msg = "成功"
            };
        }
    }

    public enum EnumMapleServiceHttpStatus
    {
        EXCEPTION = -1,
        ERROR = 0,
        OK = 1
    }
}
