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

    public class ListToGetSMSDTO
    {

    }

    public class ListToGetListSMSDTO
    {
        public string sms { get; set; }
        public string sender { get; set; }
        public string mobile { get; set; }
        public string send_date { get; set; }

        

    }

    public class SMSToGetListDTO
    {

    }
    public class SmsToReturnListDTO
    {
        public string mobile { get; set; }
        public string send_date { get; set; }
        public string sms { get; set; }
        public int statuts { get; set; }

    }

    public class SmsToGetDTO
    {
        public string keyword { get; set; }
        public string entry_date { get; set; }
        public string end_date { get; set; }
    }

    public class SmsToReturnDTO
    {
        public int id_sms_out { get; set; }
        public string mobile { get; set; }
        public string sms { get; set; }
        public string send_date { get; set; }
        public int statuts { get; set; }

    }

    public class SMSDateDTO
    {
        public string date_begin { get; set; }
        public string date_end { get; set; }
    }



}
