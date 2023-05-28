
using System;


namespace PFE.SMSNotification.Library.DTO.Players


{
    public class PlayerToGetDTO
    {
        public string service_libelle { get; set; }
        public string type_service_libelle { get; set; }
        public string entry_date { get; set; }
        public string date_end { get; set; }


    }
    public class PlayerToGetListDTO
    {
        public int id_player { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string mobile { get; set; }
        public string mail { get; set; }
        public string address { get; set; }
        public string entry_date { get; set; }
        public string service { get; set; }
        public string type { get; set; }

    }
    public class ListToGetDTO { }
    public class ListToGetListDTO
    {
        public int id_player { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string mobile { get; set; }
        public string mail { get; set; }
        public string address { get; set; }
        public string entry_date { get; set; }
        public string service { get; set; }
    }
    public class JoueurToGetDTO
    {
        public string id_player{ get; set; }


    }
    public class JoueurGetListDTO
    {
        public int id_player { get; set; }
        public string mobile { get; set; }
        public string mail { get; set; }

        public int num_sms { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string address { get; set; }
        public string entry_date { get; set; }




    }
  public class PlayerToReturnListtopDTO
    {
        public string mobile { get; set; }

        public string firstname { get; set; }
        public string lastname { get; set; }

        public int num_sms { get; set; }

    }
   

}