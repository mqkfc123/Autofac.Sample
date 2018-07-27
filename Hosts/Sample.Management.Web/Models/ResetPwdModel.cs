using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sample.Management.Web.Models
{
    public class ResetPwdModel
    {
        [DisplayName("手机号码"), Required(ErrorMessage = "请输入手机号码！")]
        public string Phone { get; set; }

        [DisplayName("验证码"), Required(ErrorMessage = "验证码！")]
        public string ValidCode { get; set; }

    }
}