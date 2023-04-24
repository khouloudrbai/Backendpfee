using System;
using System.Collections.Generic;
using System.Text;

namespace PFE.SMSNotification.Library.DTO.User

{
    public class UserToAddDTO
    {
        public string lastname { get; set; }
        public string firstname { get; set; }

        public string mobile { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string pwd { get; set; }
        public int statuts { get; set; }
        public string entry_date { get; set; }
        public string picture { get; set; }




    }

    public class UserListToReturnDTO
    {
        public int id_user { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string picture { get; set; }
        public int statuts { get; set; }
        public string entry_date { get; set; }





    }
    public class UserToUpdateDTO
    {
        public int id_user { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string pwd { get; set; }
    }
    public class UserListToUpdateReturnDTO
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string pwd { get; set; }
        public string picture { get; set; }





    }
    public class UserToauthDTO
    {

        public string email { get; set; }
        public string pwd { get; set; }


    }
    public class UserListToAuthReturnDTO
    {
        public int id_user { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string mobile { get; set; }
        public string mail { get; set; }
        public string address { get; set; }

        public string pwd { get; set; }
        public string picture { get; set; }



    }
 
    public class UserGetListDTO
    {
        public int id_user { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string mobile { get; set; }
        public string mail { get; set; }
        public string address { get; set; }

        public string picture { get; set; }
    }
    public class UserTodeleteDTO
    {
        public int id_user { get; set; }

    }
    public class UserListTodeleteReturnDTO
    {
        public int id_user { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string mobile { get; set; }
        public string mail { get; set; }
        public string picture { get; set; }




    }
    public class UserGetOneDTO
    {
        public int id_user { get; set; }

    }

    public class UserToGetDTO
    {
        public string id_user { get; set; }

    }
    public class UserReturnOneDTO
    {
        public int id_user { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string mobile { get; set; }
        public string mail { get; set; }
        public string picture { get; set; }
        public int statuts { get; set; }
        public string address { get; set; }

    }
    public class UserGetimgDTO
    {
        public string id_user { get; set; }

    }
    public class UserReturnimgDTO
    {
        public string picture { get; set; }

    }
}
