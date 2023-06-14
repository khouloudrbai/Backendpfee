
using PFE.SMSNotification.Library.DTO.Players;
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
    public class PlayerController : ControllerBase
    {
        //lire les requete sql
        NpgsqlConnection npgsqlConnection;
        //faire connection
        TraceManager traceManager;

        public PlayerController()
        {
            traceManager = new TraceManager(HttpContext);
            npgsqlConnection = new NpgsqlConnection(Config.CONNECTION_STRING);
        }
        [HttpPost("get")]
        public IActionResult get_player(PlayerToGetDTO playerget)
        {
            try
            {
                npgsqlConnection.Open();
                string requeteSQL = @"select * from ctl_players_detail('" + playerget.service_libelle + "','" + playerget.type_service_libelle + "','"
                    + playerget.entry_date + "','" +
                    playerget.date_end + "')";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                //read requete
                NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                List<PlayerToGetListDTO> results = new List<PlayerToGetListDTO>();

                //n7otouha fi list 

                //si user doesnt have a row then
                if (!UserReader.HasRows)
                {

                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<PlayerToGetListDTO>(false, "User EMPTY", "201", results));
                }

                //else 
                while (UserReader.Read())
                {
                    try
                    {
                        PlayerToGetListDTO PlayerToReturnDTO = new PlayerToGetListDTO();

                        PlayerToReturnDTO.id_player = Convert.ToInt32(UserReader["id_player"]);
                        PlayerToReturnDTO.firstname = Convert.ToString(UserReader["firstname"]);
                        PlayerToReturnDTO.lastname = Convert.ToString(UserReader["lastname"]);
                        PlayerToReturnDTO.mobile = Convert.ToString(UserReader["mobile"]);
                        PlayerToReturnDTO.mail = Convert.ToString(UserReader["mail"]);
                        PlayerToReturnDTO.address = Convert.ToString(UserReader["address"]);
                        PlayerToReturnDTO.entry_date = Convert.ToString(UserReader["entry_date"]);
                        PlayerToReturnDTO.service = Convert.ToString(UserReader["service"]);
                        PlayerToReturnDTO.type = Convert.ToString(UserReader["type"]);

                        results.Add(PlayerToReturnDTO);


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

                return Ok(new DataResponse<PlayerToGetListDTO>(false, "", "201", results));

            }
            catch (Exception ex)
            {
                npgsqlConnection.Close();
                traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return BadRequest(new DataResponse<PlayerToGetListDTO>(true, "server error", "500", null));
            }
        }

        [HttpPost("list")]
        public IActionResult get_list_player(ListToGetDTO playerget)
        {
            try
            {
                npgsqlConnection.Open();
                string requeteSQL = @"select * from get_list_player()";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                //read requete
                NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                List<ListToGetListDTO> results = new List<ListToGetListDTO>();

                //n7otouha fi list 

                //si user doesnt have a row then
                if (!UserReader.HasRows)
                {

                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<ListToGetListDTO>(false, "User EMPTY", "201", results));
                }

                //else 
                while (UserReader.Read())
                {
                    try
                    {
                        ListToGetListDTO PlayerToReturnDTO = new ListToGetListDTO();

                        PlayerToReturnDTO.id_player = Convert.ToInt32(UserReader["id_player"]);
                        PlayerToReturnDTO.firstname = Convert.ToString(UserReader["firstname"]);
                        PlayerToReturnDTO.lastname = Convert.ToString(UserReader["lastname"]);
                        PlayerToReturnDTO.mobile = Convert.ToString(UserReader["mobile"]);
                        PlayerToReturnDTO.mail = Convert.ToString(UserReader["mail"]);
                        PlayerToReturnDTO.address = Convert.ToString(UserReader["address"]);
                        PlayerToReturnDTO.entry_date = Convert.ToString(UserReader["entry_date"]);
                        PlayerToReturnDTO.service = Convert.ToString(UserReader["service"]);
                        PlayerToReturnDTO.type = Convert.ToString(UserReader["type"]);

                        results.Add(PlayerToReturnDTO);


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

                return Ok(new DataResponse<ListToGetListDTO>(false, "", "201", results));

            }
            catch (Exception ex)
            {
                npgsqlConnection.Close();
                traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return BadRequest(new DataResponse<ListToGetListDTO>(true, "server error", "500", null));
            }
        }
        [HttpPost("joueur")]
        public IActionResult Get_joueur(JoueurToGetDTO Joueurget)
        {

            {
                try
                {

                    npgsqlConnection.Open();
                    string requeteSQL = @"select * from get_detail_joueur('" + Joueurget.id_player + "')";

                    NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                    //read requete
                    NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                    //n7otouha fi list 
                    List<JoueurGetListDTO> results = new List<JoueurGetListDTO>();

                    //si user doesnt have a row then

                    if (!UserReader.HasRows)
                    {

                        npgsqlCommand.Dispose();
                        npgsqlConnection.Close();
                        return Ok(new DataResponse<JoueurToGetDTO>(false, "User EMPTY", "500", results));
                    }
                    //else 
                    while (UserReader.Read())
                    {
                        try
                        {
                            JoueurGetListDTO JoueurTogetReturnDTO = new JoueurGetListDTO();


                            JoueurTogetReturnDTO.id_player = Convert.ToInt32(UserReader["id_player"]);
                            JoueurTogetReturnDTO.firstname = Convert.ToString(UserReader["firstname"]);
                            JoueurTogetReturnDTO.lastname = Convert.ToString(UserReader["lastname"]);
                            JoueurTogetReturnDTO.mobile = Convert.ToString(UserReader["mobile"]);
                            JoueurTogetReturnDTO.mail = Convert.ToString(UserReader["mail"]);
                            JoueurTogetReturnDTO.num_sms = Convert.ToInt32(UserReader["num_sms"]);
                            JoueurTogetReturnDTO.address = Convert.ToString(UserReader["address"]);
                            JoueurTogetReturnDTO.entry_date = Convert.ToString(UserReader["entry_date"]);

                            results.Add(JoueurTogetReturnDTO);




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

                    return Ok(new DataResponse<JoueurGetListDTO>(false, "", "201", results));

                }
                catch (Exception ex)
                {
                    npgsqlConnection.Close();
                    traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                    return BadRequest(new DataResponse<JoueurGetListDTO>(true, "server error", "500", null));
                }
            }

        }

        [HttpPost("numberjoueur")]
        public IActionResult Get_number_joueur()
        {

            {
                try
                {
                    npgsqlConnection.Open();
                    string requeteSQL = "SELECT clt_number_players()";
                    NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);
                    int number_of_players = (int)npgsqlCommand.ExecuteScalar();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<int>(false, "", "201", number_of_players));
                }
                catch (Exception ex)
                {
                    npgsqlConnection.Close();
                    // Handle the exception here
                    throw ex;
                }
            }

        }
        [HttpPost("top10players")]

        public IActionResult Get_top10_player()
        {
            try
            {

                npgsqlConnection.Open();
                string requeteSQL = @"select * from clt_top10_player( )";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);

                NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();

                List<PlayerToReturnListtopDTO> results = new List<PlayerToReturnListtopDTO>();


                if (!UserReader.HasRows)
                {

                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<PlayerToReturnListtopDTO>(false, "User EMPTY", "500", results));
                }


                //else 
                while (UserReader.Read())
                {
                    try
                    {
                        PlayerToReturnListtopDTO ServiceToReturnTopDTO = new PlayerToReturnListtopDTO();

                        ServiceToReturnTopDTO.mobile = Convert.ToString(UserReader["mobile"]);
                        ServiceToReturnTopDTO.firstname = Convert.ToString(UserReader["firstname"]);
                        ServiceToReturnTopDTO.lastname = Convert.ToString(UserReader["lastname"]);
                        ServiceToReturnTopDTO.num_sms = Convert.ToInt32(UserReader["num_sms"]);

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

                return Ok(new DataResponse<PlayerToReturnListtopDTO>(false, "", "201", results));

            }
            catch (Exception ex)
            {
                npgsqlConnection.Close();
                traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return BadRequest(new DataResponse<PlayerToReturnListtopDTO>(true, "server error", "500", null));
            }
        }

        [HttpPost("numberjoueurajr")]
        public IActionResult Get_number_joueur_ajr()
        {

            {
                try
                {
                    npgsqlConnection.Open();
                    string requeteSQL = "SELECT clt_number_joueur_ajr()";
                    NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);
                    int number_of_players_ajr = (int)npgsqlCommand.ExecuteScalar();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<int>(false, "", "201", number_of_players_ajr));
                }
                catch (Exception ex)
                {
                    npgsqlConnection.Close();
                    // Handle the exception here
                    throw ex;
                }
            }

        }

        [HttpPost("joueurperperiode")]
        public IActionResult Get_joueur_date(ServiceToGetPlayersDateDTO servicenumberdateget)
        {
            try
            {

                npgsqlConnection.Open();
                string requeteSQL = @"select * from clt_number_gamers_perdate(" + "'" + servicenumberdateget.date_begin + "'," + "'" + servicenumberdateget.date_end + "')";

                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(requeteSQL, npgsqlConnection);
                NpgsqlDataReader UserReader = npgsqlCommand.ExecuteReader();
                //n7otouha fi list 
                List<JoueurPerPeriodDTO> results = new List<JoueurPerPeriodDTO>();

                //si user doesnt have a row then

                if (!UserReader.HasRows)
                {

                    npgsqlCommand.Dispose();
                    npgsqlConnection.Close();
                    return Ok(new DataResponse<JoueurPerPeriodDTO>(false, "User EMPTY", "500", results));
                }


                //else 
                while (UserReader.Read())
                {
                    try
                    {
                        JoueurPerPeriodDTO JoueurToReturndateDTO = new JoueurPerPeriodDTO();

                        JoueurToReturndateDTO.nbr_joueur = Convert.ToInt32(UserReader["nbr_joueur"]);
                        JoueurToReturndateDTO.entry_date = Convert.ToDateTime(UserReader["entry_date"]);

                        results.Add(JoueurToReturndateDTO);



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

                return Ok(new DataResponse<JoueurPerPeriodDTO>(false, "", "201", results));

            }
            catch (Exception ex)
            {
                npgsqlConnection.Close();
                traceManager.WriteLog(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return BadRequest(new DataResponse<JoueurPerPeriodDTO>(true, "server error", "500", null));
            }
        }

    }

}
