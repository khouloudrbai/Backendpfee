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

namespace Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiSendSMSController : ControllerBase
    {
        //lire les requete sql
        NpgsqlConnection npgsqlConnection;
        //faire connection
        TraceManager traceManager;

        public ApiSendSMSController()
        {
            traceManager = new TraceManager(HttpContext);
            npgsqlConnection = new NpgsqlConnection(Config.CONNECTION_STRING);
        }
        [HttpPost("send-sms")]
        public IActionResult get_player(SMSToSendDTO sendnotif)
        {
            try
            {

                string myMobile = sendnotif.mobile;
                string mySms =sendnotif.sms;
                string mySender = sendnotif.sender;
                string myDate = sendnotif.date;
                string myTime = sendnotif.time;
                string key = "135rnGf7Olilvgamp8VWWYoyYku7eu0OI7G6QxFqi=cehDSXMz4vYut8eoHJjJj2T0AuzRliRON9dri0rowQiqaOgtjQETUiVgeU9Q55";

                var url = "https://mystudents.tunisiesms.tn/api/sms";

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "POST";

                httpRequest.Accept = "application/json";
                httpRequest.Headers["Authorization"] = "Bearer {token}";
                httpRequest.ContentType = "application/json";

                //{'type': '55', 'sender': %sender%,  'sms':[{'mobile':'%mobile%','sms':'%sms%'}]}

                var data = @"{""employee"":{ ""name"":""Emma"", ""age"":28, ""city"":""Boston"" }} ";

                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(data);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }

                Console.WriteLine(httpResponse.StatusCode);




                //si user doesnt have a row then
                //if (response.ContentLength == 0)
                //{

                  
                //    return Ok(new DataResponse<object>(false, "SMS NOT SENT", "201", response));
                //}

                ////else 
                //while (response.ContentLength > 0)
                //{
                   
                //        return BadRequest(new DataResponse<object>(true, "server error", "500", null));
                    
                //}
               

                //return Ok(new DataResponse<object>(false, "", "201", response));

            }
            catch (Exception ex)
            {
                npgsqlConnection.Close();
                traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return BadRequest(new DataResponse<object>(true, "server error", "500", null));
            }
        }

       
 

    }
}
