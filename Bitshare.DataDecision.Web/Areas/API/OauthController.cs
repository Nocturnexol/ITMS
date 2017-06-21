using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Bitshare.DataDecision.Service.Enum;
using Newtonsoft.Json;
using Bitshare.Common;
using Bitshare.DataDecision.Common;
using Bitshare.DataDecision.Model;
using Bitshare.DataDecision.Controllers.Filter;
namespace Bitshare.DataDecision.Web.Areas.API.Controllers
{
    public class OauthController : ApiController
    {
        /// <summary>
        /// 根据用户名获取token
        /// </summary>
        /// <param name="appkey"></param>
        /// <returns></returns>

        public HttpResponseMessage GetToken(string appkey, string appsecret)
        {
            ResultMsg resultMsg = null;
          
            //判断参数是否合法
            if (string.IsNullOrEmpty(appkey) )
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int)StatusCodeEnum.ParameterError;
                resultMsg.Message = StatusCodeEnum.ParameterError.GetEnumText();
                resultMsg.Data = "";
                return HttpResponseExtension.ToJson(resultMsg);
            }

            //插入缓存
            Token token = (Token)HttpRuntime.Cache.Get(appkey);
      
            if (token == null)
            {
                token = new Token();
                token.AppKey = appkey;
                token.SignToken = Guid.NewGuid();
                token.ExpireTime = DateTime.Now.AddDays(1);
               
                HttpRuntime.Cache.Insert(token.AppKey.ToString(), token, null, token.ExpireTime, TimeSpan.Zero);
            }

            //返回token信息
            resultMsg = new ResultMsg();
            resultMsg.StatusCode = (int)StatusCodeEnum.Success;
            resultMsg.Message = StatusCodeEnum.Success.GetEnumText();
            resultMsg.Data = token;

            return HttpResponseExtension.ToJson(resultMsg);
        }
    }
}
