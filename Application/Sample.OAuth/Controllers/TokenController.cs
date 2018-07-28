using Dragon.Infrastructure.Mvc;
using Dragon.OAuth.Models;
using Dragon.OAuth.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sample.OAuth.Controllers
{

    public class TokenController : ApiController
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public class GetTokenResultViewModel
        {
            [JsonProperty("access_token")]
            [Required, StringLength(200)]
            public string AccessToken { get; set; }

            [JsonProperty("expires_in"), Range(60, int.MaxValue)]
            public int ExpiresIn { get; set; }
        }

        [HttpGet]
        public async Task<ApiResult> Get([FromUri]AuthorizationModel model)
        {
            if (model == null || !ModelState.IsValid)
                return new ApiResult(-10030);

            switch (model.UserName)
            {
                case "dragon":
                    if (model.Password != "dragon1@#$")
                        return new ApiResult("非法授权请求。", -20001);
                    break;
                case "dragonCancel":
                    if (model.Password != "dragon1@#$")
                        return new ApiResult("非法授权请求。", -20001);
                    break;
                default:
                    return new ApiResult("非法授权请求。", -20001);
                    break;
            }


            var token = await _tokenService.GenerateToken(model);
            return new ApiResult(new GetTokenResultViewModel
            {
                AccessToken = token.AccessToken + "$" + model.UserName,
                ExpiresIn = (int)token.ExpiresIn.Subtract(DateTime.Now).TotalSeconds
            });
        }

    }
}
