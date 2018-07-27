using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sample.Management.Web.Models
{
    public class ModifyPwdModel
    {

        [DisplayName("旧密码"), Required(ErrorMessage = "旧密码！")]
        public string OldPwd { get; set; }

        [DisplayName("新密码"), Required(ErrorMessage = "新密码！"), StringLength(20, MinimumLength = 6, ErrorMessage = "密码应该在6~20位之间！")]
        public string NewPwd { get; set; }

    }
}
