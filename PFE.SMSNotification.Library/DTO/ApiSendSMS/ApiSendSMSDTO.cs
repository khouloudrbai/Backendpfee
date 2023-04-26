using System;
using System.Collections.Generic;
using System.Text;

namespace PFE.SMSNotification.Library.DTO.ApiSendSMS

{
    public class SMSToSendDTO
    {

        public string mobile { get; set; }
        public string sender { get; set; }
        public string date { get; set; }
        public string sms { get; set; }

        public string time { get; set; }


    }



}
