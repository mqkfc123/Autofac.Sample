using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.OAuth.Models
{

    public class TokenModel
    {
        public AuthorizationModel Authorization { get; set; }

        [Required, StringLength(200)]
        public string AccessToken { get; set; }

        public DateTime ExpiresIn { get; set; }

        public bool IsExpires()
        {
            return ExpiresIn >= DateTime.Now;
        }

        public bool Check(string accessToken)
        {
            return IsExpires() && AccessToken == accessToken;
        }

        public static TokenModel Create(AuthorizationModel authorization)
        {
            return new TokenModel
            {
                Authorization = authorization,
                AccessToken = GenerateTokenByAuthorization(authorization, "access"),
                ExpiresIn = DateTime.Now.AddHours(2)
            };
        }

        private static string GenerateTokenByAuthorization(AuthorizationModel model, string type)
        {
            var key = $"{type}_{model.UserName}_{model.Password}_{DateTime.Now:yyyyMMddHHmmssff}";
            key = string.Join("", key.Reverse());
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(key));
        }

        public static string GetUserNameByToken(string token)
        {
            token = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            token = string.Join("", token.Reverse());
            var temp = token.Split('_');
            return temp[1];
        }
    }
}
