using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sample.OAuth.Models
{

    public class AuthorizationModel
    {
        [Required, StringLength(20)]
        public string UserName { get; set; }

        [Required, StringLength(20)]
        public string Password { get; set; }
    }
}