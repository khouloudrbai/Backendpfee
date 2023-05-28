using PFE.SMSNotification.Library.DTO.ApiSendSMS;
using PFE.SMSNotification.Library.Utility;
using PFE.SMSNotification.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Reflection;
using static System.Net.WebRequestMethods;
using System.Net.Http;
using PFE.SMSNotification.Library.DTO.Players;
using PFE.SMSNotification.Library.DTO.Services;

namespace Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiSendSMSController : ControllerBase
    {
        NpgsqlConnection npgsqlConnection;
        TraceManager traceManager;

        public ApiSendSMSController()
        {
            traceManager = new TraceManager(HttpContext);
            npgsqlConnection = new NpgsqlConnection(Config.CONNECTION_STRING);
        }

        [HttpPost("send-sms")]
        public IActionResult sendsms(SMSToSendDTO sendnotif)
        {
            try
            {
                string myMobile = sendnotif.mobile;
                string mySms = sendnotif.sms;
                string mySender = sendnotif.sender;
                string myDate = sendnotif.date;
                DateTime dateValue;

                if (!DateTime.TryParse(myDate, out dateValue))
                {
                    throw new ArgumentException("Invalid date format.");
                }

                myDate = dateValue.ToString("yyyy-MM-dd HH:mm:ss");

                string myKey = "135rnGf7Olilvgamp8VWWYoyYku7eu0OI7G6QxFqi=cehDSXMz4vYut8eoHJjJj2T0AuzRliRON9dri0rowQiqaOgtjQETUiVgeU9Q55";

                if (string.IsNullOrEmpty(myMobile))
                    throw new ArgumentException("Mobile number is null or empty.");

                var url = "https://mystudents.tunisiesms.tn/api/sms";

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "POST";
                httpRequest.Headers["Authorization"] = "Bearer " + myKey;
                httpRequest.ContentType = "application/json";

                string jsonString = "{\"type\":\"55\",\"sender\":\"%sender%\",\"sms\":[{\"mobile\":\"%mobile%\",\"sms\":\"%sms%\"}],\"date\":\"%date%\"}";
                jsonString = jsonString.Replace("%sender%", mySender);
                jsonString = jsonString.Replace("%mobile%", myMobile);
                jsonString = jsonString.Replace("%sms%", mySms);
                jsonString = jsonString.Replace("%date%", myDate);


                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonString);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                Console.WriteLine(httpResponse.StatusCode);


                npgsqlConnection.Open();
                string requeteSQL = @"INSERT INTO sms_out (mobile, sms, sender,send_date) VALUES (@mobile, @sms, @sender,@send_date)";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);
                npgsqlCommand.Parameters.AddWithValue("@mobile", myMobile);
                npgsqlCommand.Parameters.AddWithValue("@sms", mySms);
                npgsqlCommand.Parameters.AddWithValue("@sender", mySender);
                npgsqlCommand.Parameters.AddWithValue("@send_date", myDate);


                npgsqlCommand.ExecuteNonQuery();
                npgsqlConnection.Close();

                return Ok(new { success = true, message = "SMS sent", statusCode = (int)httpResponse.StatusCode });
            }
            catch (Exception ex)
            {
                // handle exceptions
                return BadRequest(new { success = false, message = "Server error", statusCode = 500 });
            }
        }

        [HttpPost("sms_prog")]
        public IActionResult get_sms_programe(ListToGetSMSDTO smsget)
        {
            try
            {
                npgsqlConnection.Open();
                string requeteSQL = @"select * from sms_programe()";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                //read requete
                NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                List<ListToGetListSMSDTO> results = new List<ListToGetListSMSDTO>();

                //n7otouha fi list 

                //si user doesnt have a row then
                if (!UserReader.HasRows)
                {

                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<ListToGetListSMSDTO>(false, "User EMPTY", "201", results));
                }

                //else 
                while (UserReader.Read())
                {
                    try
                    {
                        ListToGetListSMSDTO smsToReturnDTO = new ListToGetListSMSDTO();

                        smsToReturnDTO.mobile = Convert.ToString(UserReader["mobile"]);
                        smsToReturnDTO.sms = Convert.ToString(UserReader["sms"]);
                        smsToReturnDTO.sender = Convert.ToString(UserReader["sender"]);
                        smsToReturnDTO.send_date = Convert.ToString(UserReader["send_date"]);


                        results.Add(smsToReturnDTO);


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

                return Ok(new DataResponse<ListToGetListSMSDTO>(false, "", "201", results));

            }
            catch (Exception ex)
            {
                npgsqlConnection.Close();
                traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return BadRequest(new DataResponse<ListToGetListSMSDTO>(true, "server error", "500", null));
            }
        }
        [HttpPost("getsms")]
        public IActionResult get_sms(SMSToGetListDTO smsget)
        {

            try
            {
                npgsqlConnection.Open();
                string requeteSQL = @"select * from get_sms( )";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                //read requete
                NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                //n7otouha fi list 
                List<SmsToReturnListDTO> results = new List<SmsToReturnListDTO>();

                //si user doesnt have a row then

                if (!UserReader.HasRows)
                {

                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<SmsToReturnListDTO>(false, "User EMPTY", "500", results));
                }


                //else 
                while (UserReader.Read())
                {
                    try
                    {
                        SmsToReturnListDTO SmsToReturnDTO = new SmsToReturnListDTO();

                        SmsToReturnDTO.mobile = Convert.ToString(UserReader["mobile"]);
                        SmsToReturnDTO.sms = Convert.ToString(UserReader["sms"]);
                        SmsToReturnDTO.statuts = Convert.ToInt32(UserReader["statuts"]);
                        SmsToReturnDTO.send_date = Convert.ToString(UserReader["send_date"]);





                        results.Add(SmsToReturnDTO);



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

                return Ok(new DataResponse<SmsToReturnListDTO>(false, "", "201", results));

            }
            catch (Exception ex)
            {
                npgsqlConnection.Close();
                traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return BadRequest(new DataResponse<SmsToReturnListDTO>(true, "server error", "500", null));
            }
        }

        [HttpPost("smsdetail")]
        public IActionResult get_sms_detail(SmsToGetDTO smsget)
        {
            try
            {
                npgsqlConnection.Open();
                string requeteSQL = @"select * from ctl_sms_detail(" + "'" + smsget.keyword + "'," + "'" + smsget.entry_date + "'," + "'" + smsget.end_date + "')";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                //read requete
                NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                List<SmsToReturnDTO> results = new List<SmsToReturnDTO>();

                //n7otouha fi list 

                //si user doesnt have a row then
                if (!UserReader.HasRows)
                {

                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<SmsToReturnDTO>(false, "User EMPTY", "201", results));
                }

                //else 
                while (UserReader.Read())
                {
                    try
                    {
                        SmsToReturnDTO SmsToReturnDTO = new SmsToReturnDTO();

                        SmsToReturnDTO.id_sms_out = Convert.ToInt32(UserReader["id_sms_out"]);
                        SmsToReturnDTO.mobile = Convert.ToString(UserReader["mobile"]);
                        SmsToReturnDTO.sms = Convert.ToString(UserReader["sms"]);
                        SmsToReturnDTO.send_date = Convert.ToString(UserReader["send_date"]);

                        SmsToReturnDTO.statuts = Convert.ToInt32(UserReader["statuts"]);


                        results.Add(SmsToReturnDTO);


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

                return Ok(new DataResponse<SmsToReturnDTO>(false, "", "201", results));

            }
            catch (Exception ex)
            {
                npgsqlConnection.Close();
                traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return BadRequest(new DataResponse<SmsToReturnDTO>(true, "server error", "500", null));
            }
        }


        [HttpPost("smsperperiode")]
        public IActionResult Get_joueur_date(SMSDateDTO smsdateget)
        {
            try
            {

                npgsqlConnection.Open();
                string requeteSQL = @"select * from clt_number_gamers_perdate(" + "'" + smsdateget.date_begin + "'," + "'" + smsdateget.date_end + "')";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);
                int number_of_sms_perdate = (int)npgsqlCommand.ExecuteScalar();
                npgsqlConnection.Close();
                return Ok(new DataResponse<int>(false, "", "201", number_of_sms_perdate));
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
