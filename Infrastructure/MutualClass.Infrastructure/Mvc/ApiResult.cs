using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Infrastructure.Mvc
{

    public class ApiResult
    {
        public ApiResult()
        {
            Code = 10000;
        }

        public ApiResult(int code)
        {
            Code = code;
        }

        public ApiResult(string errorMessage)
        {
            Message = errorMessage;
            Code = -10000;
        }

        public ApiResult(string errorMessage, int code)
        {
            Message = errorMessage;
            Code = code;
        }

        public ApiResult(object result)
        {
            Code = 10000;
            Result = result;
        }

        public string Message { get; set; }
        public int Code { get; set; }
        public object Result { get; set; }
    }
}
