using PFE.SMSNotification.Library.DTO.User;
using PFE.SMSNotification.Library.Utility;
using PFE.SMSNotification.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Numerics;

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
                string requeteSQL = @"select * from ctl_user_add(" +"'" + Useradd.id_user+ "'," + "'" + Useradd.lastname + "'," + "'" + Useradd.firstname + "'," + "'" + Useradd.mobile + "'," + "'" + Useradd.email + "'," + "'" +
                    Useradd.address + "','" + Useradd.pwd + "'," + "'" + Useradd.statuts + "'," + "'" + Useradd.entry_date + "'," + "'" + Useradd.picture + "')";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                //read requete
                NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                //n7otouha fi list 
                UserListToReturnDTO UserToReturnDTO = new UserListToReturnDTO();
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
                        UserToReturnDTO.statuts = Convert.ToString(UserReader["statuts"]);
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
                    string requeteSQL = @"select * from ctl_user_update(" + "'" + Userupdate.id_user + "','" + Userupdate.mobile + "','" + Userupdate.firstname + "','"
                                                                              + Userupdate.email + "','" + Userupdate.address + "','"+Userupdate.picture
                                                                              + "')";

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
                            UserToUpdateReturnDTO.picture = Convert.ToString(UserReader["picture"]);

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
            try
            {
                npgsqlConnection.Open();
                string requeteSQL = @"select * from ctl_user_authenticate('" + Userauth.email + "','" + Userauth.pwd + "')";
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                // Execute the query
                NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();

                // Check if the user exists
                if (!UserReader.HasRows)
                {
                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<object>(false, "User EMPTY", "500", null));
                }

                UserListToAuthReturnDTO UserToAuthReturnDTO = new UserListToAuthReturnDTO();

                // Read the user information
                while (UserReader.Read())
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

                // Generate JWT token

                var tokenHandler = new JwtSecurityTokenHandler();

                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("KHOULOUDNOUHAPFEkhouloudnouhapfe20222023"));

                
                var tokenDescriptor = new SecurityTokenDescriptor
                {


                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("id_user", UserToAuthReturnDTO.id_user.ToString()),
                        new Claim("firstname", UserToAuthReturnDTO.firstname),
                        new Claim("lastname", UserToAuthReturnDTO.lastname),
                        new Claim("mobile", UserToAuthReturnDTO.mobile),
                        new Claim("mail", UserToAuthReturnDTO.mail),
                        new Claim("address", UserToAuthReturnDTO.address),
                        new Claim("pwd", UserToAuthReturnDTO.pwd),
                        new Claim("picture", UserToAuthReturnDTO.picture),

                    }),


                    Expires = DateTime.UtcNow.AddDays(7), // Set token expiration
                    SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
                };


                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);



                npgsqlCommand.Dispose();
                npgsqlConnection.Close();

                return Ok(new DataResponse<object>(false, "", "201", new { token = jwtToken }));
            }
            catch (Exception ex)
            {
                npgsqlConnection.Close();
                traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return BadRequest(new DataResponse<object>(true, "server error", "500", null));
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
                            UserTogetReturnDTO.entry_date = Convert.ToString(UserReader["entry_date"]);

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
                    string requeteSQL = @"select * from delate_case_user(" + Userdelete.id_user + ")";

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
                    string requeteSQL = @"select * from get_one_user(" + Useroneget.id_user + ")";

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
                            UserTogetReturnDTO.entry_date = Convert.ToString(UserReader["entry_date"]);

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

        [HttpPost("updatepwd")]
        public IActionResult Contact_update_pwd(UserToUpdatePwdDTO Userupdatepwd)
        {
            try
            {
                npgsqlConnection.Open();
                string requeteSQL = @"select * from ctl_user_password_update(" + "'" + Userupdatepwd.id_user + "','" + Userupdatepwd.old_pwd + "','" + Userupdatepwd.new_pwd + "')";
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                var result = npgsqlCommand.ExecuteScalar();
                var data = (object)result;
                JObject json = JObject.Parse((string)result);
                int code = (int)json["code"];
                Console.WriteLine("Code: " + code);

                npgsqlCommand.Dispose();
                npgsqlConnection.Close();
                if (code == 400)
                {
                    return Ok(new DataResponse<object>(true, "", "500", data));
                }
                else
                {
                    return Ok(new DataResponse<object>(false, "", "201", data));

                }
            }
            catch (Exception ex)
            {
                npgsqlConnection.Close();
                traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return BadRequest(new DataResponse<object>(true, "server error", "500", null));
            }
        }
        [HttpPost("verify")]
        public IActionResult verify_user(VerifyDTO verify)
        {

            {
                try
                {
                    npgsqlConnection.Open();
                    string requeteSQL = "SELECT ctl_verify_user(" + "'" + verify.email+ "','"+ verify.pwd + "')";
                    NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);
                    int number = (int)npgsqlCommand.ExecuteScalar();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<int>(false, "", "201", number));
                }
                catch (Exception ex)
                {
                    npgsqlConnection.Close();
                    // Handle the exception here
                    throw ex;
                }
            }

        }
    }
    }