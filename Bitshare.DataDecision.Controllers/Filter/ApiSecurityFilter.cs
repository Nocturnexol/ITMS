using Bitshare.Common;
using Bitshare.DataDecision.Model;
using Bitshare.DataDecision.Service.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
namespace Bitshare.DataDecision.Controllers.Filter
{
    public class HttpResponseExtension
    {
        public static HttpResponseMessage ToJson(Object obj)
        {
            String str;
            if (obj is String || obj is Char)
            {
                str = obj.ToString();
            }
            else
            {
                var iso = new IsoDateTimeConverter();
                iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                str = JsonConvert.SerializeObject(obj, iso);
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
    }
    public class ApiSecurityFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            ResultMsg resultMsg = null;
            var request = actionContext.Request;
            string method = request.Method.Method;
            string appkey = String.Empty, timestamp = string.Empty, nonce = string.Empty, sign = string.Empty;
            string appsecret = String.Empty;
            Dictionary<string, object> args = actionContext.ActionArguments;
            if (request.Headers.Contains("appkey"))
            {
                appkey = HttpUtility.UrlDecode(request.Headers.GetValues("appkey").FirstOrDefault());
            }
            else if (args.ContainsKey("appkey"))
            {
                appkey = Convert.ToString(args["appkey"]);
            }
            if (request.Headers.Contains("appsecret"))
            {
                appsecret = HttpUtility.UrlDecode(request.Headers.GetValues("appsecret").FirstOrDefault());
            }
            else if (args.ContainsKey("appsecret"))
            {
                appsecret = Convert.ToString(args["appsecret"]);
            }
            if (request.Headers.Contains("timestamp"))
            {
                timestamp = HttpUtility.UrlDecode(request.Headers.GetValues("timestamp").FirstOrDefault());
            }
            else if (args.ContainsKey("timestamp"))
            {
                timestamp = Convert.ToString(args["timestamp"]);
            }
            if (request.Headers.Contains("nonce"))
            {
                nonce = HttpUtility.UrlDecode(request.Headers.GetValues("nonce").FirstOrDefault());
            }
            else if (args.ContainsKey("nonce"))
            {
                nonce = Convert.ToString(args["nonce"]);
            }
            if (request.Headers.Contains("sign"))
            {
                sign = HttpUtility.UrlDecode(request.Headers.GetValues("sign").FirstOrDefault());
            }
            else if (args.ContainsKey("sign"))
            {
                sign = Convert.ToString(args["sign"]);
            }
            //GetToken方法不需要进行签名验证
            if (actionContext.ActionDescriptor.ActionName.ToLower() == "gettoken")
            {
                if (string.IsNullOrEmpty(appkey) || string.IsNullOrEmpty(appsecret))
                {
                    resultMsg = new ResultMsg();
                    resultMsg.StatusCode = (int)StatusCodeEnum.ParameterError;
                    resultMsg.Message = StatusCodeEnum.ParameterError.GetEnumText();
                    resultMsg.Data = "";
                    actionContext.Response = HttpResponseExtension.ToJson(JsonConvert.SerializeObject(resultMsg));
                    base.OnActionExecuting(actionContext);
                    return;
                }
                else
                {
                    base.OnActionExecuting(actionContext);
                    return;
                }
            }
            //判断请求头是否包含以下参数
            if (string.IsNullOrEmpty(appkey) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(nonce) || string.IsNullOrEmpty(sign))
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int)StatusCodeEnum.ParameterError;
                resultMsg.Message = StatusCodeEnum.ParameterError.GetEnumText();
                resultMsg.Data = "";
                actionContext.Response = HttpResponseExtension.ToJson(JsonConvert.SerializeObject(resultMsg));
                base.OnActionExecuting(actionContext);
                return;
            }
            //判断timespan是否有效
            double ts1 = 0;
            double ts2 = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds;
            bool timespanvalidate = double.TryParse(timestamp, out ts1);
            double ts = ts2 - ts1;
            bool falg = ts > 120 * 1000 || ts < -120 * 1000;//两分钟有效期
            if (falg || (!timespanvalidate))
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int)StatusCodeEnum.URLExpireError;
                resultMsg.Message = StatusCodeEnum.URLExpireError.GetEnumText();
                resultMsg.Data = "";
                actionContext.Response = HttpResponseExtension.ToJson(resultMsg);
                base.OnActionExecuting(actionContext);
                return;
            }


            //判断token是否有效
            Token token = (Token)HttpRuntime.Cache.Get(appkey);

            string signtoken = string.Empty;
            if (token == null)
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int)StatusCodeEnum.TokenInvalid;
                resultMsg.Message = StatusCodeEnum.TokenInvalid.GetEnumText();
                resultMsg.Data = "";
                actionContext.Response = HttpResponseExtension.ToJson(resultMsg);
                base.OnActionExecuting(actionContext);
                return;
            }
            else
            {
                signtoken = token.SignToken.ToString();
            }




            //根据请求类型拼接参数
            NameValueCollection form = HttpContext.Current.Request.QueryString;
            string data = string.Empty;
            switch (method)
            {
                case "POST":
                    Stream stream = HttpContext.Current.Request.InputStream;
                    string responseJson = string.Empty;
                    StreamReader streamReader = new StreamReader(stream);
                    data = streamReader.ReadToEnd();
                    break;
                case "GET":
                #region 安全需要屏蔽GET请求
                /* //第一步：取出所有get参数
                    IDictionary<string, string> parameters = new Dictionary<string, string>();
                    for (int f = 0; f < form.Count; f++)
                    {
                        string key = form.Keys[f];
                        parameters.Add(key, form[key]);
                    }

                    // 第二步：把字典按Key的字母顺序排序
                    IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
                    IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

                    // 第三步：把所有参数名和参数值串在一起
                    StringBuilder query = new StringBuilder();
                    while (dem.MoveNext())
                    {
                        string key = dem.Current.Key;
                        string value = dem.Current.Value;
                        if (!string.IsNullOrEmpty(key))
                        {
                            query.Append(key).Append(value);
                        }
                    }
                    data = query.ToString();

                    break;*/
                #endregion
                default:
                    resultMsg = new ResultMsg();
                    resultMsg.StatusCode = (int)StatusCodeEnum.HttpMehtodError;
                    resultMsg.Message = StatusCodeEnum.HttpMehtodError.GetEnumText();
                    resultMsg.Data = "";
                    actionContext.Response = HttpResponseExtension.ToJson(resultMsg);
                    base.OnActionExecuting(actionContext);
                    return;
            }

            bool result = Md5.Validate(timestamp, nonce, appkey, signtoken, data, sign);
            if (!result)
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int)StatusCodeEnum.HttpRequestError;
                resultMsg.Message = StatusCodeEnum.HttpRequestError.GetEnumText();
                resultMsg.Data = "";
                actionContext.Response = HttpResponseExtension.ToJson(resultMsg);
                base.OnActionExecuting(actionContext);
                return;
            }
            else
            {
                base.OnActionExecuting(actionContext);
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
