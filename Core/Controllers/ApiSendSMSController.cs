using PFE.SMSNotification.Library.DTO.ApiSendSMS;
using PFE.SMSNotification.Library.Utility;
using PFE.SMSNotification.Utility;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Hangfire;
using NpgsqlTypes;

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
        [HttpPost("schedule-sms")]
        public IActionResult ScheduleSms(SMSToScheduleDTO sendnotif)
        {
            try
            {
                int statuts = 0;

                Random random = new Random();
                int randomNumber = random.Next(0, 1000000000);


                npgsqlConnection.Open();

                string requeteSQL = @"INSERT INTO sms_out (id_sms_out, mobile, sms, sender,statuts,send_date) VALUES (@id_sms_out, @mobile, @sms, @sender,@statuts,@send_date)";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);


                if (string.IsNullOrEmpty(sendnotif.mobile) || (sendnotif.mobile.Length > 8 && !sendnotif.mobile.StartsWith("216")))
                {

                    npgsqlCommand.Parameters.AddWithValue("@id_sms_out", randomNumber);
                    npgsqlCommand.Parameters.AddWithValue("@mobile", sendnotif.mobile);
                    npgsqlCommand.Parameters.AddWithValue("@sms", sendnotif.sms);
                    npgsqlCommand.Parameters.AddWithValue("@sender", sendnotif.sender);
                    npgsqlCommand.Parameters.AddWithValue("@send_date", sendnotif.myDate);
                    npgsqlCommand.Parameters.AddWithValue("@statuts", statuts);
                    npgsqlCommand.ExecuteNonQuery();

                    throw new ArgumentException("Invalid mobile number.");


                }



                BackgroundJob.Schedule(() => SendSms(sendnotif), sendnotif.myDate);

                return Ok(new { success = true, message = "SMS scheduled successfully" });
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException;

                // handle exceptions
                return Ok(new { success = false, message = "Server error", statusCode = 500 });
            }
        }

        public void SendSms(SMSToScheduleDTO sendnotif)
        {
            try
            {

                string mySms = sendnotif.sms;
                string mySender = sendnotif.sender;
                string myMobile = sendnotif.mobile;



                string myKey = "135rnGf7Olilvgamp8VWWYoyYku7eu0OI7G6QxFqi=cehDSXMz4vYut8eoHJjJj2T0AuzRliRON9dri0rowQiqaOgtjQETUiVgeU9Q55";


               

                var url = "https://mystudents.tunisiesms.tn/api/sms";

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "POST";
                httpRequest.Headers["Authorization"] = "Bearer " + myKey;
                httpRequest.ContentType = "application/json";

                string jsonString = "{\"type\":\"55\",\"sender\":\"%sender%\",\"sms\":[{\"mobile\":\"%mobile%\",\"sms\":\"%sms%\"}]}";
                jsonString = jsonString.Replace("%sender%", mySender);
                jsonString = jsonString.Replace("%mobile%", myMobile);
                jsonString = jsonString.Replace("%sms%", mySms);

                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonString);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                Console.WriteLine(httpResponse.StatusCode);
               int  statuts = 1;

                Random random = new Random();
                int randomNumber = random.Next(0, 1000000000);


                npgsqlConnection.Open();

                string requeteSQL = @"INSERT INTO sms_out (id_sms_out, mobile, sms, sender,statuts,send_date) VALUES (@id_sms_out, @mobile, @sms, @sender,@statuts,@send_date)";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);
                npgsqlCommand.Parameters.AddWithValue("@id_sms_out", randomNumber);
                npgsqlCommand.Parameters.AddWithValue("@mobile", myMobile);
                npgsqlCommand.Parameters.AddWithValue("@sms", mySms);
                npgsqlCommand.Parameters.AddWithValue("@sender", mySender);
                npgsqlCommand.Parameters.AddWithValue("@statuts", statuts);
                npgsqlCommand.Parameters.AddWithValue("@send_date", sendnotif.myDate);



                npgsqlCommand.ExecuteNonQuery();
                npgsqlConnection.Close();

                Console.WriteLine("SMS sent");
            }
            catch (Exception ex)
            {
                // handle exceptions


                Console.WriteLine("Failed to send SMS: " + ex.Message);
            }
        }

        [HttpPost("send-sms")]
        public IActionResult sendsms(SMSToSendDTO sendnotif)
        {
            try
            {
                int statuts = 0;
                string myMobile = sendnotif.mobile;
                string mySms = sendnotif.sms;
                string mySender = sendnotif.sender;
                string myDate = sendnotif.date;

                Random random = new Random();
                int randomNumber = random.Next(0, 1000000000);

               
                npgsqlConnection.Open();

                string requeteSQL = @"INSERT INTO sms_out (id_sms_out, mobile, sms, sender,statuts,send_date) VALUES (@id_sms_out, @mobile, @sms, @sender,@statuts,@send_date)";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                string myKey = "135rnGf7Olilvgamp8VWWYoyYku7eu0OI7G6QxFqi=cehDSXMz4vYut8eoHJjJj2T0AuzRliRON9dri0rowQiqaOgtjQETUiVgeU9Q55";

                if (string.IsNullOrEmpty(myMobile) || (myMobile.Length > 8 && !myMobile.StartsWith("216")))
                {

                    npgsqlCommand.Parameters.AddWithValue("@id_sms_out", randomNumber);
                    npgsqlCommand.Parameters.AddWithValue("@mobile", myMobile);
                    npgsqlCommand.Parameters.AddWithValue("@sms", mySms);
                    npgsqlCommand.Parameters.AddWithValue("@sender", mySender);
                    if (myDate == null || myDate is DBNull)
                    {
                        npgsqlCommand.Parameters.AddWithValue("@send_date", DateTime.Now);
                    }
                    else
                    {
                        npgsqlCommand.Parameters.AddWithValue("@send_date", NpgsqlTypes.NpgsqlDbType.Timestamp, DateTime.Parse(myDate.ToString()));
                    }

                    npgsqlCommand.Parameters.AddWithValue("@statuts", statuts);
                    npgsqlCommand.ExecuteNonQuery();

                    throw new ArgumentException("Invalid mobile number.");


                }

                var url = "https://mystudents.tunisiesms.tn/api/sms";

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "POST";
                httpRequest.Headers["Authorization"] = "Bearer " + myKey;
                httpRequest.ContentType = "application/json";

                string jsonString = "{\"type\":\"55\",\"sender\":\"%sender%\",\"sms\":[{\"mobile\":\"%mobile%\",\"sms\":\"%sms%\",\"date\":\"%date%\"}]}";
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
                statuts = 1;


                npgsqlCommand.Parameters.AddWithValue("@id_sms_out", randomNumber);
                npgsqlCommand.Parameters.AddWithValue("@mobile", myMobile);
                npgsqlCommand.Parameters.AddWithValue("@sms", mySms);
                npgsqlCommand.Parameters.AddWithValue("@sender", mySender);
                npgsqlCommand.Parameters.AddWithValue("@sender", myDate);
                npgsqlCommand.Parameters.AddWithValue("@statuts", statuts);




                npgsqlCommand.ExecuteNonQuery();
                npgsqlConnection.Close();

                return Ok(new { success = true, message = "SMS sent", statusCode = (int)httpResponse.StatusCode });
            }
            catch (Exception ex)
            {
                // handle exceptions
                return Ok(new { success = false, message = "Server error", statusCode = 500 });
            }
        }

        [HttpPost("getsms")]
        public IActionResult get_sms( )
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
                        if (UserReader["statuts"] != DBNull.Value)
                        {
                            SmsToReturnDTO.statuts = Convert.ToInt32(UserReader["statuts"]);
                        }
                        if (UserReader["send_date"] != DBNull.Value)
                        {
                            SmsToReturnDTO.send_date = Convert.ToString(UserReader["send_date"]);
                        }


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
        public IActionResult Get_sms_date(SMSDateDTO smsdateget)
        {
            try
            {

                npgsqlConnection.Open();
                string requeteSQL = @"select * from clt_number_sms_perdate(" + "'" + smsdateget.date_begin + "'," + "'" + smsdateget.date_end + "')";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);
                //read requete
                NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                //n7otouha fi list 
                List<SmsToReturnListPeriodDTO> results = new List<SmsToReturnListPeriodDTO>();

                //si user doesnt have a row then

                if (!UserReader.HasRows)
                {

                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<SmsToReturnListPeriodDTO>(false, "User EMPTY", "500", results));
                }


                //else 
                while (UserReader.Read())
                {
                    try
                    {
                        SmsToReturnListPeriodDTO SmsToReturndateDTO = new SmsToReturnListPeriodDTO();

                        SmsToReturndateDTO.nbr_sms = Convert.ToInt32(UserReader["nbr_sms"]);
                        SmsToReturndateDTO.entry_date = Convert.ToDateTime(UserReader["entry_date"]);
                      
                        results.Add(SmsToReturndateDTO);



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

                return Ok(new DataResponse<SmsToReturnListPeriodDTO>(false, "", "201", results));

            }
            catch (Exception ex)
            {
                npgsqlConnection.Close();
                traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return BadRequest(new DataResponse<SmsToReturnListPeriodDTO>(true, "server error", "500", null));
            }
        }
    }
}
