using Dragon.Core.Log4net;
using Dragon.Infrastructure.Mvc;
using Dragon.OAuth;
using Newtonsoft.Json; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace Sample.WebApi.Areas.Mutual.Controllers.Scene
{
    /// <summary>
    /// 
    /// </summary> 
    [OAuth]
    [Route("api/Mutual/Scene")]
    public class TestController : ApiController
    {
        private readonly ILog _Logger = LogHelper.GetLogger(typeof(TestController));

        /// <summary>
        /// 
        /// </summary>
        public TestController()
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns> 
        [HttpGet]
        public ApiResult Get()
        {
            try
            {
                return new ApiResult(new { Data = "10000", PowerData = "dragonget" });
            }
            catch (Exception ex)
            {
                _Logger.Error($"SignInAccount异常：param");
                return new ApiResult("查询异常:" + ex.Message, -10001);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns> 
        [HttpPost]
        public ApiResult Post()
        {
            try
            {
                return new ApiResult(new { Data = "10000", PowerData = "dragonpsot" });
            }
            catch (Exception ex)
            {
                _Logger.Error($"SignInAccount异常：param");
                return new ApiResult("查询异常:" + ex.Message, -10001);
            }
        }

    }
}
