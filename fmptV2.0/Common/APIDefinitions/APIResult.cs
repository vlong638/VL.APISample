using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fmptV2.Common
{
    /// <summary>
    /// Controller层返回结构
    /// </summary>
    public class APIResult
    {
        public const int SuccessCode = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messages"></param>
        public APIResult(params string[] messages)
        {
            Code = SuccessCode;
            if (messages != null & messages.Length != 0)
                Message = string.Join(",", messages);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="messages"></param>
        public APIResult(int code, params string[] messages)
        {
            Code = code;
            if (messages != null & messages.Length != 0)
                Message = string.Join(",", messages);
        }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { set; get; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { set; get; }
    }
    /// <summary>
    /// Controller层返回结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class APIResult<T> : APIResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="code"></param>
        /// <param name="messages"></param>
        public APIResult(T data, int code, params string[] messages) : base(code, messages)
        {
            Data = data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="messages"></param>
        public APIResult(T data, params string[] messages) : base(messages)
        {
            Data = data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceResult"></param>
        public APIResult(ServiceResult<T> serviceResult)
        {
            Data = serviceResult.Data;
            Code = serviceResult.Code;
            Message = serviceResult.Message;
        }
        /// <summary>
        /// 
        /// </summary>
        public APIResult()
        {
        }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { set; get; }
    }
}
