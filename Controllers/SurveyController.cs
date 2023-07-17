using System;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using DPU_AQD_API.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace DPU_AQD_API;
[ApiController]
[Route("webapi")]

public class SurveyController : ControllerBase
{
    private SQLConection sQLConection = new SQLConection();
    [HttpGet("getSurvey")]
    public async Task<IActionResult> GetSurvey()
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getSurvey"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<SurveyResponse> surveyResponses = new List<SurveyResponse>();
            try
            {
                while (reader.Read())
                {
                    SurveyResponse surveyResponse = new SurveyResponse();
                    surveyResponse.surveyID = reader["SurveyID"].ToString();
                    surveyResponse.Timestamp = DateTime.Parse(reader["Timestamp"].ToString());
                    surveyResponse.SubmitData = reader["SurveyData"].ToString();
                    surveyResponse.BuildingID = Convert.ToInt32(reader["BuildingID"].ToString());
                    surveyResponse.RoomName = reader["RoomName"].ToString();

                    surveyResponses.Add(surveyResponse);
                }
                await connection.CloseAsync();
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            return Ok(surveyResponses);
        }
    }
    [HttpPost("submitSurvey")]
    public async Task<IActionResult> SubmitSurvey(string submitData, int buildingID, string roomName)
    {
        
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            string latestID = "";
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getLatestSurvey";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            try
            {
                if (!reader.HasRows)
                {
                    latestID = (DateTime.Now.ToString("yyyyMM") + String.Format("{0:00000}", 1));
                }
                else
                {
                    while (reader.Read())
                    {
                        latestID = Convert.ToString(reader["SurveyID"]);
                    }
                }
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            await connection.CloseAsync();


            MySqlCommand cmd2 = new MySqlCommand();
            cmd2.Connection = connection;
            cmd2.CommandText = "submitSurvey"; //Store Procedure Name
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;

            string survey_date = latestID.Substring(0, 6);
            string surveyID = latestID.Substring(6);

            if (DateTime.Now.ToString("yyyyMM") == survey_date)
            {
                cmd2.Parameters.Add("_surveyID", MySqlDbType.VarChar).Value = (DateTime.Now.ToString("yyyyMM") + String.Format("{0:00000}", (Convert.ToInt64(surveyID)) + 1));
            }
            else
            {
                cmd2.Parameters.Add("_surveyID", MySqlDbType.VarChar).Value = (DateTime.Now.ToString("yyyyMM") + String.Format("{0:00000}", 1));
            }


            cmd2.Parameters.Add("_timestamp", MySqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd2.Parameters.Add("_submitData", MySqlDbType.Text).Value = submitData;
            cmd2.Parameters.Add("_buildingID", MySqlDbType.Int32).Value = buildingID;
            cmd2.Parameters.Add("_roomName", MySqlDbType.VarChar).Value = roomName;
            await connection.OpenAsync();

            MySqlDataReader reader2 = cmd2.ExecuteReader();
            List<SurveyResponse> surveyResponses = new List<SurveyResponse>();
            try
            {
                while (reader2.Read())
                {
                    SurveyResponse surveyResponse = new SurveyResponse();
                    surveyResponse.surveyID = reader2["SurveyID"].ToString();
                    surveyResponse.Timestamp = DateTime.Parse(reader2["Timestamp"].ToString());
                    surveyResponse.SubmitData = reader2["SurveyData"].ToString();
                    surveyResponse.BuildingID = Convert.ToInt32(reader2["BuildingID"].ToString());
                    surveyResponse.RoomName = reader2["RoomName"].ToString();

                    surveyResponses.Add(surveyResponse);
                }

                await connection.CloseAsync();
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            return Ok(surveyResponses);
        }
    }
}
