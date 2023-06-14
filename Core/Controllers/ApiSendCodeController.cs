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
    public class ApiSendCodeController : ControllerBase
    {
        NpgsqlConnection npgsqlConnection;
        TraceManager traceManager;

        public ApiSendCodeController()
        {
            traceManager = new TraceManager(HttpContext);
            npgsqlConnection = new NpgsqlConnection(Config.CONNECTION_STRING);
        }

        [HttpPost("send-code")]
        public IActionResult sendsms(CodeToReturn sendnotif)
        {
            try
            {
                string myMobile = sendnotif.mobile;
                string mySms = "Votre code est : " + sendnotif.code;


                string myKey = "135rnGf7Olilvgamp8VWWYoyYku7eu0OI7G6QxFqi=cehDSXMz4vYut8eoHJjJj2T0AuzRliRON9dri0rowQiqaOgtjQETUiVgeU9Q55";

                if (string.IsNullOrEmpty(myMobile))
                    throw new ArgumentException("Mobile number is null or empty.");

                var url = "https://mystudents.tunisiesms.tn/api/sms";

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "POST";
                httpRequest.Headers["Authorization"] = "Bearer " + myKey;
                httpRequest.ContentType = "application/json";

                string jsonString = "{\"type\":\"55\",\"sender\":\"%sender%\",\"sms\":[{\"mobile\":\"%mobile%\",\"sms\":\"%sms%\"}]}";
                jsonString = jsonString.Replace("%sender%", "TunSMS Test");
                jsonString = jsonString.Replace("%mobile%", myMobile);
                jsonString = jsonString.Replace("%sms%", mySms);


                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonString);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                Console.WriteLine(httpResponse.StatusCode);



                return Ok(new { success = true, message = "SMS sent", statusCode = (int)httpResponse.StatusCode });
            }
            catch (Exception ex)
            {
                // handle exceptions
                return BadRequest(new { success = false, message = "Server error", statusCode = 500 });
            }
        }

     
              
    }
}
