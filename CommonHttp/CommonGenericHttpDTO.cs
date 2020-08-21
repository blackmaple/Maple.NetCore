namespace Maple.NetCore
{
    public class CCommonGenericHttpDTO<T>
    {
        /// <summary>
        /// EnumCommonHttpStatus
        /// </summary>
        public int Code { set; get; }
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { set; get; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { set; get; }
    }

}
