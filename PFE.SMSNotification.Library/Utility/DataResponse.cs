using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Text;

namespace PFE.SMSNotification.Utility
{
    public class DataResponse<T>
    {
        public bool isFailed { get; set; }
        public string message { get; set; }
        public string code { get; set; }
        public dynamic data { get; set; }

        public DataResponse(bool isFailed, string message, string code, dynamic data)
        {
            this.isFailed = isFailed;
            this.message = message;
            this.code = code;
            this.data = data;
        }
    }

}
