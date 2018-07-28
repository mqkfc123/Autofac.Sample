using Auth.Infrastructure.Utility;
using Dragon.Core.Log4net;
using Dragon.Infrastructure.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sample.WebApi.Controllers.Account
{
    /// <summary>
    /// 后台系统登入
    /// </summary>
    [Route("api/Account")]
    public class SignInAccountController : ApiController
    {
        private readonly ILog _Logger = LogHelper.GetLogger(typeof(SignInAccountController));

       
 
        [HttpPost]
        public ApiResult Post()
        {
            try
            {
                return new ApiResult(new { Data = "", PowerData = "" });
            }
            catch (Exception ex)
            {
                _Logger.Error($"SignInAccount异常：param");
                return new ApiResult("查询异常:" + ex.Message, -10001);
            }
        }

    }

}
