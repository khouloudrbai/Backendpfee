

using System;
using System.Numerics;

namespace PFE.SMSNotification.Library.DTO.Services
{
    public class ServiceToGetListDTO
    {

    }
    public class ServiceToReturnListDTO

    {
        public int id_service { get; set; }
        public string type_service { get; set; }
        public string libelle { get; set; }
        public string entry_date { get; set; }
        public string date_end { get; set; }

    }
    public class ServiceToGetDTO
    {
        public string keyword { get; set; }
        public string entry_date { get; set; }
        public string end_date { get; set; }


    }
    public class ServiceToReturnDTO
    {
        public int id_service { get; set; }
        public string shortcode { get; set; }
        public string libelle { get; set; }
        public string entry_date { get; set; }
        public string date_end { get; set; }

        public string type_service{ get; set; }


        


    }
  public class ServiceToReturnListtopDTO
    {
        public string libelle { get; set; }
        public string shortcode { get; set; }
        public string entry_date { get; set; }
        public int number_gamers { get; set; }
    }
    public class ServiceToReturnListJoueurDTO
    {
        public string libelle { get; set; }
        public int number_gamers { get; set; }

    }
    public class ServiceToReturnStatDTO
    {
        public int id_service { get; set; }
        public string libelle { get; set; }
        public int number_gamers { get; set; }
    }

    public class ServiceToGetTypeDTO
    {
        
    }
    public class ServiceToReturnTypeListDTO
    {
       public int id_type_service { get; set; }
        public string libelle_type_service { get; set; }

    }


   

    public class ServiceToReturnNBListDTO
    {
        public string libelle { get; set; }
        public int v_number { get; set; }
    }
    public class ServiceToReturnIdListDTO
    {
     public int id_service { get; set; }
             
    }

}