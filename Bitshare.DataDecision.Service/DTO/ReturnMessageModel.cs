using System;
using System.Collections.Generic;

namespace Bitshare.DataDecision.Service.DTO
{
    /// <summary>
    /// 返回消息类
    /// </summary>
    public class ReturnMessageModel
    {
        private IDictionary<string, object> m_Data = new Dictionary<string, object>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="IsSuccess">默认是true还是false</param>
        public ReturnMessageModel(bool IsSuccess)
        {
            this.IsSuccess = IsSuccess;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReturnMessageModel()
        {

        }

        /// <summary>
        /// 是否保存并继续
        /// </summary>
        public bool IsContinue { set; get; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回单项数据信息
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 返多项值,以字典形式返回
        /// </summary>
        public IDictionary<string, object> ResultData 
        {
            get { return m_Data; }
            set { m_Data = value; }
        }

        public object ReturnData { set; get; }


        /// <summary>
        /// 异常信息
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// ToJSONString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
