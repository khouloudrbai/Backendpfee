
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
                string requeteSQL = @"select * from ctl_players_detail('" + playerget.id_player + "','" + playerget.keyword + "','"
                    + playerget.id_service + "','" +
                    playerget.libelle+"','"
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

    }
}
