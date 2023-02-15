using System;
using DPU_AQD_API.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace DPU_AQD_API;
[ApiController]
[Route("webapi")]
public class BuildingController : ControllerBase
{
    private SQLConection sQLConection = new SQLConection();
    [HttpGet("getBuilding")]
    public async Task<IActionResult> GetBuilding () 
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getBuilding"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<BuildingResponse> buildingResponses = new List<BuildingResponse>();
            while(reader.Read()){
                BuildingResponse buildingResponse = new BuildingResponse();
                buildingResponse.BuildingID = Convert.ToInt32(reader["BuildingID"]);
                buildingResponse.BuildingName = reader["BuildingName"].ToString();
                buildingResponse.BuildingStatus = Convert.ToChar(reader["BuildingStatus"]);
                buildingResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                buildingResponse.AdminID = Convert.ToInt32(reader["AdminID"]);
                buildingResponses.Add(buildingResponse);
            }
            await connection.CloseAsync();
            return Ok(buildingResponses);
        }
    }
    [HttpGet("getBuildingByID")]
    public async Task<IActionResult> GetBuildingByID (int _buildingID) 
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getBuildingByID"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_BuildingID", MySqlDbType.Int32).Value = _buildingID;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<BuildingResponse> buildingResponses = new List<BuildingResponse>();
            while(reader.Read()){
                BuildingResponse buildingResponse = new BuildingResponse();
                buildingResponse.BuildingID = Convert.ToInt32(reader["BuildingID"]);
                buildingResponse.BuildingName = reader["BuildingName"].ToString();
                buildingResponse.BuildingStatus = Convert.ToChar(reader["BuildingStatus"]);
                buildingResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                buildingResponse.AdminID = Convert.ToInt32(reader["AdminID"]);
                buildingResponses.Add(buildingResponse);
            }
            await connection.CloseAsync();
            return Ok(buildingResponses);
        }
    }
}