using PFE.SMSNotification.Library.DTO.User;
using PFE.SMSNotification.Library.Utility;
using PFE.SMSNotification.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;



namespace Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        //lire les requete sql
        NpgsqlConnection npgsqlConnection;
        //faire connection
        TraceManager traceManager;

        public UserController()
        {
            traceManager = new TraceManager(HttpContext);
            npgsqlConnection = new NpgsqlConnection(Config.CONNECTION_STRING);
        }
        [HttpPost("add")]
        public IActionResult Contact_add(UserToAddDTO Useradd)
        {
            try
            {
                npgsqlConnection.Open();
                string requeteSQL = @"select * from ctl_user_add(" + "'" + Useradd.lastname+ "'," + "'"+Useradd.firstname+ "'," + "'"+ Useradd.mobile + "'," + "'" + Useradd.email + "'," + "'" +
                    Useradd.address + "','" + Useradd.pwd+ "'," + "'" + Useradd.statuts+ "'," + "'" + Useradd.entry_date+ "'," + "'" +Useradd.picture+"')";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                //read requete
                NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                //n7otouha fi list 
                UserListToReturnDTO UserToReturnDTO =new UserListToReturnDTO();
                //si user doesnt have a row then
                if (!UserReader.HasRows)
                {

                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<UserListToReturnDTO>(false, "User EMPTY", "201", UserToReturnDTO));
                }

                //else 
                while (UserReader.Read())
                {
                    try
                    {

                        UserToReturnDTO.id_user = Convert.ToInt32(UserReader["id_user"]);
                        UserToReturnDTO.firstname = Convert.ToString(UserReader["firstname"]);
                        UserToReturnDTO.lastname = Convert.ToString(UserReader["lastname"]);
                        UserToReturnDTO.mobile = Convert.ToString(UserReader["mobile"]);
                        UserToReturnDTO.email = Convert.ToString(UserReader["email"]);
                        UserToReturnDTO.picture = Convert.ToString(UserReader["picture"]);
                        UserToReturnDTO.statuts = Convert.ToInt32(UserReader["statuts"]);
                        UserToReturnDTO.entry_date = Convert.ToString(UserReader["entry_date"]);
                      

                    }
                    catch (Exception ex)
                    {
                        npgsqlConnection.Close();
                        traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                        return BadRequest(new DataResponse<object>(true, "server error", "500", null));
                    }
                }
                npgsqlCommand.Dispose();
                npgsqlConnection.Close();

                return Ok(new DataResponse<UserListToReturnDTO>(false, "", "201", UserToReturnDTO));

            }
            catch (Exception ex)
            {
                npgsqlConnection.Close();
                traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return BadRequest(new DataResponse<UserListToReturnDTO>(true, "server error", "500", null));
            }
        }
        [HttpPost("update")]
        public IActionResult Contact_update(UserToUpdateDTO Userupdate)
        {

            {
                try
                {
                    
                    npgsqlConnection.Open();
                    string requeteSQL = @"select * from ctl_user_update(" + "'" +Userupdate.id_user+ "','"+ Userupdate.mobile + "','"
                                                                              + Userupdate.email +"','"+Userupdate.address + "','"+Userupdate.pwd
                                                                              +  "')";

                    NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                    //read requete
                    NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                    List<UserListToUpdateReturnDTO> results = new List<UserListToUpdateReturnDTO>();

                    //n7otouha fi list 
                    //si user doesnt have a row then
                    if (!UserReader.HasRows)
                    {

                        npgsqlCommand.Dispose();
                        npgsqlConnection.Close();
                        return Ok(new DataResponse<UserListToUpdateReturnDTO>(false, "User EMPTY", "201", results));
                    }

                    //else 
                    while (UserReader.Read())
                    {
                        try
                        {
                            UserListToUpdateReturnDTO UserToUpdateReturnDTO = new UserListToUpdateReturnDTO();

                            UserToUpdateReturnDTO.firstname = Convert.ToString(UserReader["firstname"]);
                            UserToUpdateReturnDTO.lastname = Convert.ToString(UserReader["lastname"]);
                            UserToUpdateReturnDTO.mobile = Convert.ToString(UserReader["mobile"]);
                            UserToUpdateReturnDTO.email = Convert.ToString(UserReader["email"]);
                            UserToUpdateReturnDTO.address = Convert.ToString(UserReader["address"]);
                            UserToUpdateReturnDTO.pwd = Convert.ToString(UserReader["pwd"]);
                            results.Add(UserToUpdateReturnDTO);


                        }
                        catch (Exception ex)
                        {
                            npgsqlConnection.Close();
                            traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                            return BadRequest(new DataResponse<object>(true, "server error", "500", null));
                        }
                    }
                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();

                    return Ok(new DataResponse<UserListToUpdateReturnDTO>(false, "", "201", results));

                }
                catch (Exception ex)
                {
                    npgsqlConnection.Close();
                    traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                    return BadRequest(new DataResponse<UserListToUpdateReturnDTO>(true, "server error", "500", null));
                }
            }

        }

        [HttpPost("authentification")]
        public IActionResult Contact_auth(UserToauthDTO Userauth)
        {

            {
                try
                {

                    npgsqlConnection.Open();
                    string requeteSQL = @"select * from auth_user(" + "'" + Userauth.email + "','"
                                                                              + Userauth.pwd
                                                                              + "')";

                    NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                    //read requete
                    NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                    //n7otouha fi list 
                    UserListToAuthReturnDTO UserToAuthReturnDTO = new UserListToAuthReturnDTO();
                    //si user doesnt have a row then

                    if (!UserReader.HasRows)
                    {

                        npgsqlCommand.Dispose();
                        npgsqlConnection.Close();
                        return Ok(new DataResponse<UserListToAuthReturnDTO>(false, "User EMPTY", "500", UserToAuthReturnDTO));
                    }
                    //else 
                    while (UserReader.Read())
                    {
                        try
                        {

                            UserToAuthReturnDTO.id_user = Convert.ToInt32(UserReader["id_user"]);
                            UserToAuthReturnDTO.firstname = Convert.ToString(UserReader["firstname"]);
                            UserToAuthReturnDTO.lastname = Convert.ToString(UserReader["lastname"]);
                            UserToAuthReturnDTO.mobile = Convert.ToString(UserReader["mobile"]);
                            UserToAuthReturnDTO.mail = Convert.ToString(UserReader["mail"]);
                            UserToAuthReturnDTO.address = Convert.ToString(UserReader["address"]);
                            UserToAuthReturnDTO.pwd = Convert.ToString(UserReader["pwd"]);
                            UserToAuthReturnDTO.picture = Convert.ToString(UserReader["picture"]);



                        }
                        catch (Exception ex)
                        {
                            npgsqlConnection.Close();
                            traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                            return BadRequest(new DataResponse<object>(true, "server error", "500", null));
                        }
                    }
                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();

                    return Ok(new DataResponse<UserListToAuthReturnDTO>(false, "", "201", UserToAuthReturnDTO));

                }
                catch (Exception ex)
                {
                    npgsqlConnection.Close();
                    traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                    return BadRequest(new DataResponse<UserListToAuthReturnDTO>(true, "server error", "500", null));
                }
            }

        }

        [HttpPost("list")]
        public IActionResult Get_user(UserToGetDTO Userget)
        {

            {
                try
                {

                    npgsqlConnection.Open();
                    string requeteSQL = @"select * from get_user('" + Userget.id_user + "')";

                    NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                    //read requete
                    NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                    //n7otouha fi list 
                    List<UserGetListDTO> results = new List<UserGetListDTO>();

                    //si user doesnt have a row then

                    if (!UserReader.HasRows)
                    {

                        npgsqlCommand.Dispose();
                        npgsqlConnection.Close();
                        return Ok(new DataResponse<UserGetListDTO>(false, "User EMPTY", "500", results));
                    }
                    //else 
                    while (UserReader.Read())
                    {
                        try
                        {
                            UserGetListDTO UserTogetReturnDTO = new UserGetListDTO();


                            UserTogetReturnDTO.id_user = Convert.ToInt32(UserReader["id_user"]);
                            UserTogetReturnDTO.firstname = Convert.ToString(UserReader["firstname"]);
                            UserTogetReturnDTO.lastname = Convert.ToString(UserReader["lastname"]);
                            UserTogetReturnDTO.mobile = Convert.ToString(UserReader["mobile"]);
                            UserTogetReturnDTO.mail = Convert.ToString(UserReader["mail"]);
                            UserTogetReturnDTO.address = Convert.ToString(UserReader["address"]);
                            UserTogetReturnDTO.picture = Convert.ToString(UserReader["picture"]);

                            results.Add(UserTogetReturnDTO);




                        }
                        catch (Exception ex)
                        {
                            npgsqlConnection.Close();
                            traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                            return BadRequest(new DataResponse<object>(true, "server error", "500", null));
                        }
                    }
                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();

                    return Ok(new DataResponse<UserGetListDTO>(false, "", "201", results));

                }
                catch (Exception ex)
                {
                    npgsqlConnection.Close();
                    traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                    return BadRequest(new DataResponse<UserGetListDTO>(true, "server error", "500", null));
                }
            }

        }

        [HttpPost("delete")]
        public IActionResult delete_user(UserTodeleteDTO Userdelete)
        {

            {
                try
                {

                    npgsqlConnection.Open();
                    string requeteSQL = @"select * from delate_case_user("  + Userdelete.id_user+ ")";

                    NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                    //read requete
                    NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                    //n7otouha fi list 
                    List<UserListTodeleteReturnDTO> results = new List<UserListTodeleteReturnDTO>();

                    //si user doesnt have a row then

                    if (!UserReader.HasRows)
                    {

                        npgsqlCommand.Dispose();
                        npgsqlConnection.Close();
                        return Ok(new DataResponse<UserListTodeleteReturnDTO>(false, "User EMPTY", "500", results));
                    }
                    //else 
                    while (UserReader.Read())
                    {
                        try
                        {
                            UserListTodeleteReturnDTO UserdeleteDTO = new UserListTodeleteReturnDTO();


                            UserdeleteDTO.id_user = Convert.ToInt32(UserReader["id_user"]);
                            UserdeleteDTO.firstname = Convert.ToString(UserReader["firstname"]);
                            UserdeleteDTO.lastname = Convert.ToString(UserReader["lastname"]);
                            UserdeleteDTO.mobile = Convert.ToString(UserReader["mobile"]);
                            UserdeleteDTO.mail = Convert.ToString(UserReader["mail"]);
                            UserdeleteDTO.picture = Convert.ToString(UserReader["picture"]);
                            results.Add(UserdeleteDTO);


                        }
                        catch (Exception ex)
                        {
                            npgsqlConnection.Close();
                            traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                            return BadRequest(new DataResponse<object>(true, "server error", "500", null));
                        }
                    }
                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();

                    return Ok(new DataResponse<UserListTodeleteReturnDTO>(false, "", "201", results));

                }
                catch (Exception ex)
                {
                    npgsqlConnection.Close();
                    traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                    return BadRequest(new DataResponse<UserListTodeleteReturnDTO>(true, "server error", "500", null));
                }
            }

        }

        [HttpPost("getuser")]
        public IActionResult Get_one_user(UserGetOneDTO Useroneget)
        {

            {
                try
                {

                    npgsqlConnection.Open();
                    string requeteSQL = @"select * from get_one_user(" + Useroneget.id_user+ ")";

                    NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                    //read requete
                    NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                    //n7otouha fi list 
                    List<UserReturnOneDTO> results = new List<UserReturnOneDTO>();

                    //si user doesnt have a row then

                    if (!UserReader.HasRows)
                    {

                        npgsqlCommand.Dispose();
                        npgsqlConnection.Close();
                        return Ok(new DataResponse<UserReturnOneDTO>(false, "User EMPTY", "500", results));
                    }
                    //else 
                    while (UserReader.Read())
                    {
                        try
                        {
                            UserReturnOneDTO UserTogetReturnDTO = new UserReturnOneDTO();


                            UserTogetReturnDTO.id_user = Convert.ToInt32(UserReader["id_user"]);
                            UserTogetReturnDTO.firstname = Convert.ToString(UserReader["firstname"]);
                            UserTogetReturnDTO.lastname = Convert.ToString(UserReader["lastname"]);
                            UserTogetReturnDTO.mobile = Convert.ToString(UserReader["mobile"]);
                            UserTogetReturnDTO.mail = Convert.ToString(UserReader["mail"]);
                            UserTogetReturnDTO.address = Convert.ToString(UserReader["address"]);
                            UserTogetReturnDTO.picture = Convert.ToString(UserReader["picture"]);

                            results.Add(UserTogetReturnDTO);




                        }
                        catch (Exception ex)
                        {
                            npgsqlConnection.Close();
                            traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                            return BadRequest(new DataResponse<object>(true, "server error", "500", null));
                        }
                    }
                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();

                    return Ok(new DataResponse<UserReturnOneDTO>(false, "", "201", results));

                }
                catch (Exception ex)
                {
                    npgsqlConnection.Close();
                    traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                    return BadRequest(new DataResponse<UserReturnOneDTO>(true, "server error", "500", null));
                }
            }

        }


    }
}