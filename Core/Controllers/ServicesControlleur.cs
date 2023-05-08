
using PFE.SMSNotification.Library.DTO.Services;
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
    public class ServiceController : ControllerBase
    {
        //lire les requete sql
        NpgsqlConnection npgsqlConnection;
        //faire connection
        TraceManager traceManager;

        public ServiceController()
        {
            traceManager = new TraceManager(HttpContext);
            npgsqlConnection = new NpgsqlConnection(Config.CONNECTION_STRING);
        }
        [HttpPost("getservice")]
        public IActionResult get_service(ServiceToGetListDTO serviceget)
        {
            
            try
            {
                npgsqlConnection.Open();
                string requeteSQL = @"select * from get_service( )";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                //read requete
                NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                //n7otouha fi list 
                List<ServiceToReturnListDTO> results = new List<ServiceToReturnListDTO>();

                //si user doesnt have a row then

                if (!UserReader.HasRows)
                {

                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<ServiceToReturnListDTO>(false, "User EMPTY", "500", results));
                }
              

                //else 
                while (UserReader.Read())
                {
                    try
                    {
                        ServiceToReturnListDTO ServiceToReturnDTO = new ServiceToReturnListDTO();

                        ServiceToReturnDTO.id_service = Convert.ToInt32(UserReader["id_service"]);
                        ServiceToReturnDTO.shortcode = Convert.ToString(UserReader["shortcode"]);
                        ServiceToReturnDTO.libelle = Convert.ToString(UserReader["libelle"]);
                        ServiceToReturnDTO.number_gamers = Convert.ToInt32(UserReader["number_gamers"]);

                        results.Add(ServiceToReturnDTO);



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

                return Ok(new DataResponse<ServiceToReturnListDTO>(false, "", "201", results));

            }
            catch (Exception ex)
            {
                npgsqlConnection.Close();
                traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return BadRequest(new DataResponse<ServiceToReturnListDTO>(true, "server error", "500", null));
            }
        }
        [HttpPost("getlist")]
        public IActionResult get_service_detail(ServiceToGetDTO serviceget)
        {
            try
            {
                npgsqlConnection.Open();
                string requeteSQL = @"select * from ctl_service_detail(" + "'" + serviceget.shortcode + "'," + "'" + serviceget.id_service +  "')";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                //read requete
                NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                List<ServiceToReturnDTO> results = new List<ServiceToReturnDTO>();

                //n7otouha fi list 

                //si user doesnt have a row then
                if (!UserReader.HasRows)
                {

                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<ServiceToReturnDTO>(false, "User EMPTY", "201", results));
                }

                //else 
                while (UserReader.Read())
                {
                    try
                    {
                        ServiceToReturnDTO ServiceToReturnserviceDTO = new ServiceToReturnDTO();

                        ServiceToReturnserviceDTO.id_service = Convert.ToInt32(UserReader["id_service"]);
                        ServiceToReturnserviceDTO.shortcode = Convert.ToString(UserReader["shortcode"]);
                        ServiceToReturnserviceDTO.number_gamers = Convert.ToInt32(UserReader["number_gamers"]);
                        ServiceToReturnserviceDTO.libelle = Convert.ToString(UserReader["libelle"]);

                        results.Add(ServiceToReturnserviceDTO);


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

                return Ok(new DataResponse<ServiceToReturnDTO>(false, "", "201", results));

            }
            catch (Exception ex)
            {
                npgsqlConnection.Close();
                traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return BadRequest(new DataResponse<ServiceToReturnDTO>(true, "server error", "500", null));
            }
        }

        [HttpPost("numberservice")]
        public IActionResult Get_number_service()
        {

            {
                try
                {
                    npgsqlConnection.Open();
                    string requeteSQL = "SELECT clt_number_services()";
                    NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);
                    int number_of_services = (int)npgsqlCommand.ExecuteScalar();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<int>(false, "", "201", number_of_services));
                }
                catch (Exception ex)
                {
                    npgsqlConnection.Close();
                    // Handle the exception here
                    throw ex;
                }
            }

        }
        [HttpPost("top10")]

        public IActionResult Get_top10_service() { 
         try
            {

                npgsqlConnection.Open();
                string requeteSQL = @"select * from clt_top10_service( )";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();

                 List<ServiceToReturnListtopDTO> results = new List<ServiceToReturnListtopDTO>();


                   if (!UserReader.HasRows)
                    {

                      npgsqlCommand.Dispose();
                      npgsqlConnection.Close();
                      return Ok(new DataResponse<ServiceToReturnListtopDTO>(false, "User EMPTY", "500", results));
                     }
              

                                    //else 
                   while (UserReader.Read())
                   {
                      try
                      {
                        ServiceToReturnListtopDTO ServiceToReturnTopDTO = new ServiceToReturnListtopDTO();

                      ServiceToReturnTopDTO.libelle = Convert.ToString(UserReader["libelle"]);
                       ServiceToReturnTopDTO.number_gamers = Convert.ToInt32(UserReader["number_gamers"]);

                      results.Add(ServiceToReturnTopDTO);



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

                    return Ok(new DataResponse<ServiceToReturnListDTO>(false, "", "201", results));

         }
                   catch (Exception ex)
                    {
                        npgsqlConnection.Close();
                        traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                        return BadRequest(new DataResponse<ServiceToReturnListDTO>(true, "server error", "500", null));
                    }
        }
        [HttpPost("joueurperservice")]

        public IActionResult Get_joueur_service()
        {
            try
            {

                npgsqlConnection.Open();
                string requeteSQL = @"select * from clt_number_gamers_perservice( )";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();

                List<ServiceToReturnListJoueurDTO> results = new List<ServiceToReturnListJoueurDTO>();


                if (!UserReader.HasRows)
                {

                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<ServiceToReturnListJoueurDTO>(false, "User EMPTY", "500", results));
                }


                //else 
                while (UserReader.Read())
                {
                    try
                    {
                        ServiceToReturnListJoueurDTO ServiceToReturnjoueurDTO = new ServiceToReturnListJoueurDTO();

                        ServiceToReturnjoueurDTO.libelle = Convert.ToString(UserReader["libelle"]);
                        ServiceToReturnjoueurDTO.number_gamers = Convert.ToInt32(UserReader["number_gamers"]);

                        results.Add(ServiceToReturnjoueurDTO);



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

                return Ok(new DataResponse<ServiceToReturnListJoueurDTO>(false, "", "201", results));

            }
            catch (Exception ex)
            {
                npgsqlConnection.Close();
                traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return BadRequest(new DataResponse<ServiceToReturnListJoueurDTO>(true, "server error", "500", null));
            }
        }
    }
}

    
