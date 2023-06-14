
using PFE.SMSNotification.Library.DTO.Services;
using PFE.SMSNotification.Library.Utility;
using PFE.SMSNotification.Utility;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;


namespace Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PasswordController : ControllerBase
    {
        //lire les requete sql
        NpgsqlConnection npgsqlConnection;
        //faire connection
        TraceManager traceManager;

        public PasswordController()
        {
            traceManager = new TraceManager(HttpContext);
            npgsqlConnection = new NpgsqlConnection(Config.CONNECTION_STRING);
        }
        [HttpPost("addcode")]
        public IActionResult code_confirm_add(AddCode codeget)
        {

            try
            {
                npgsqlConnection.Open();
                string requeteSQL = @"select * from ctl_code_confirm_add( " + "'" + codeget.mobile + "')";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                //read requete
                NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                //n7otouha fi list 
                CodeToReturn AddToReturnDTO = new CodeToReturn();

                //si user doesnt have a row then

                if (!UserReader.HasRows)
                {

                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<CodeToReturn>(false, "User EMPTY", "500", AddToReturnDTO));
                }


                //else 
                while (UserReader.Read())
                {
                    try
                    {

                        AddToReturnDTO.mobile = Convert.ToString(UserReader["mobile"]);
                        AddToReturnDTO.code = Convert.ToString(UserReader["code"]);


                    }
                     

                    catch (Exception ex)
                    {
                        npgsqlConnection.Close();
                        traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                        return BadRequest(new DataResponse<object>(true, "server error", "500", null));
                    }
                    var apiSendCodeController = new ApiSendCodeController();
                    var smsToSend = new CodeToReturn
                    {
                        mobile = AddToReturnDTO.mobile,
                        code = AddToReturnDTO.code
                    };
                    apiSendCodeController.sendsms(smsToSend);
                }
                npgsqlCommand.Dispose();
                npgsqlConnection.Close();

                return Ok(new DataResponse<CodeToReturn>(false, "", "201", AddToReturnDTO));

            }
            catch (Exception ex)
            {
                npgsqlConnection.Close();
                traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return BadRequest(new DataResponse<CodeToReturn>(true, " Mobile number does not exist in the users table", "500", null));
            }
        }

        [HttpPost("confirmcode")]
        public IActionResult ctl_code_confirm(ConfirmCode confirmcode)
        {

            try
            {
                npgsqlConnection.Open();
                string requeteSQL = @"select * from ctl_code_confirm( " + "'" + confirmcode.mobile + "'," + "'" +confirmcode.code + "')";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                //read requete
                NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                //n7otouha fi list 
                ReturnConfirmCode ConfirmToReturnDTO = new ReturnConfirmCode();

                //si user doesnt have a row then

                if (!UserReader.HasRows)
                {

                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<ReturnConfirmCode>(false, "User EMPTY", "500", ConfirmToReturnDTO));
                }


                //else 
                while (UserReader.Read())
                {
                    try
                    {
                        ConfirmToReturnDTO.id_user = Convert.ToInt32(UserReader["id_user"]);
                        ConfirmToReturnDTO.mobile = Convert.ToString(UserReader["mobile"]);
                        ConfirmToReturnDTO.code = Convert.ToString(UserReader["code"]);






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

                return Ok(new DataResponse<ReturnConfirmCode>(false, "", "201", ConfirmToReturnDTO));

            }
            catch (Exception ex)
            {
                npgsqlConnection.Close();
                traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return BadRequest(new DataResponse<ReturnConfirmCode>(true, "server error", "500", null));
            }
        }
        [HttpPost("reset")]
        public IActionResult ctl_reset_password(ResetPassword reset)
        {

            try
            {
                npgsqlConnection.Open();
                string requeteSQL = @"select * from clt_reset_password( " + "'" + reset.pwd + "'," + "'" + reset.id_user + "')";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                //read requete
                NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                //n7otouha fi list 
                ReturnReset resettoreturn = new ReturnReset();

                //si user doesnt have a row then

                if (!UserReader.HasRows)
                {

                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<ReturnReset>(false, "User EMPTY", "500", resettoreturn));
                }


                //else 
                while (UserReader.Read())
                {
                    try
                    {

                        resettoreturn.mobile = Convert.ToString(UserReader["mobile"]);
                        resettoreturn.email = Convert.ToString(UserReader["email"]);





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

                return Ok(new DataResponse<ReturnReset>(false, "", "201", resettoreturn));

            }
            catch (Exception ex)
            {
                npgsqlConnection.Close();
                traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return BadRequest(new DataResponse<ReturnReset>(true, "server error", "500", null));
            }
        }

    }

}


