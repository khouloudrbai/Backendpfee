

using System;
using System.Numerics;

namespace PFE.SMSNotification.Library.DTO.Services
{
    public class AddCode
    {
        public string mobile { get; set; }
    }
    public class CodeToReturn
    {
        public string code { get; set; }
        public string mobile { get; set; }
    }
    public class ConfirmCode
    {
        public string mobile { get; set; }
        public string code { get; set; }
    }
    public class ReturnConfirmCode
    {
        public string mobile { get; set; }
        public string code { get; set; }
    }
    public class ResetPassword
    {
        public string pwd { get; set; }
        public string mobile { get; set; }


    }
    public class ReturnReset
    {
        public string email { get; set; }
        public string mobile { get; set; }
    }


}