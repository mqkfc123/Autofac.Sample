using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sample.Management.Web.Models
{
    public class UserSignInModel
    {
        [DisplayName("用户名"), Required(ErrorMessage = "请输入用户名！")]
        public string UserName { get; set; }

        [DisplayName("密码"), Required(ErrorMessage = "请输入密码！"), StringLength(20, MinimumLength = 6, ErrorMessage = "密码应该在6~20位之间！")]
        public string Password { get; set; }
    }
}