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
            try
            {
                while (reader.Read())
                {
                    BuildingResponse buildingResponse = new BuildingResponse();
                    buildingResponse.BuildingID = Convert.ToInt32(reader["BuildingID"]);
                    buildingResponse.BuildingName = reader["BuildingName"].ToString();
                    buildingResponse.BuildingStatus = Convert.ToChar(reader["BuildingStatus"]);
                    buildingResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                    buildingResponse.AdminID = Convert.ToInt32(reader["AdminID"]);
                    buildingResponse.LastUpdateDate = DateTime.Parse(reader["LastUpdateDate"].ToString());
                    buildingResponse.LastUpdateAdminID = Convert.ToInt32(reader["LastUpdateAdminID"]);
                    buildingResponses.Add(buildingResponse);
                }
                await connection.CloseAsync();
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
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
            cmd.Parameters.Add("_buildingID", MySqlDbType.Int32).Value = _buildingID;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<BuildingResponse> buildingResponses = new List<BuildingResponse>();
            try
            {
                while (reader.Read())
                {
                    BuildingResponse buildingResponse = new BuildingResponse();
                    buildingResponse.BuildingID = Convert.ToInt32(reader["BuildingID"]);
                    buildingResponse.BuildingName = reader["BuildingName"].ToString();
                    buildingResponse.BuildingStatus = Convert.ToChar(reader["BuildingStatus"]);
                    buildingResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                    buildingResponse.AdminID = Convert.ToInt32(reader["AdminID"]);
                    buildingResponse.LastUpdateDate = DateTime.Parse(reader["LastUpdateDate"].ToString());
                    buildingResponse.LastUpdateAdminID = Convert.ToInt32(reader["LastUpdateAdminID"]);
                    buildingResponses.Add(buildingResponse);
                }
                await connection.CloseAsync();
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            return Ok(buildingResponses);
        }
    }
    [HttpPost("registerBuilding")]
    public async Task<IActionResult> RegisterBuilding (string _buildingName, int _adminID) {
        int count = 1;
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getBuilding";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            try{
                while (reader.Read())
                {
                    count++;
                }
            } catch (MySqlException ex){
                throw;
            }
            await connection.CloseAsync();
        }

        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "registerBuilding"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_buildingID", MySqlDbType.Int32).Value = Convert.ToInt32(DateTime.Now.ToString("yyyyMM") + String.Format("{0:0000}", count));
            cmd.Parameters.Add("_buildingName", MySqlDbType.VarChar).Value = _buildingName;
            cmd.Parameters.Add("_createDate", MySqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("_buildingStatus", MySqlDbType.VarChar).Value = 'T';
            cmd.Parameters.Add("_adminID", MySqlDbType.Int32).Value = _adminID;
            cmd.Parameters.Add("_lastUpdateDate", MySqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("_lastUpdateAdminID", MySqlDbType.Int32).Value = _adminID;
            
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<BuildingResponse> buildingResponses = new List<BuildingResponse>();
            try
            {
                while (reader.Read())
                {
                    BuildingResponse buildingResponse = new BuildingResponse();
                    buildingResponse.BuildingID = Convert.ToInt32(reader["BuildingID"]);
                    buildingResponse.BuildingName = reader["BuildingName"].ToString();
                    buildingResponse.BuildingStatus = Convert.ToChar(reader["BuildingStatus"]);
                    buildingResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                    buildingResponse.AdminID = Convert.ToInt32(reader["AdminID"]);
                    buildingResponse.LastUpdateDate = DateTime.Parse(reader["LastUpdateDate"].ToString());
                    buildingResponse.LastUpdateAdminID = Convert.ToInt32(reader["LastUpdateAdminID"]);
                    buildingResponses.Add(buildingResponse);
                }
                await connection.CloseAsync();
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            return Ok(buildingResponses);
        }
    }
    [HttpGet("updateBuildingName")]
    public async Task<IActionResult> EditBuilding (int _buildingID, string _buildingName, int _adminID) {

        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "updateBuildingName"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_buildingID", MySqlDbType.Int32).Value = _buildingID;
            cmd.Parameters.Add("_buildingName", MySqlDbType.VarChar).Value = _buildingName;
            cmd.Parameters.Add("_lastUpdateDate", MySqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("_lastUpdateAdminID", MySqlDbType.Int32).Value = _adminID;
            
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<BuildingResponse> buildingResponses = new List<BuildingResponse>();
            try
            {
                while (reader.Read())
                {
                    BuildingResponse buildingResponse = new BuildingResponse();
                    buildingResponse.BuildingID = Convert.ToInt32(reader["BuildingID"]);
                    buildingResponse.BuildingName = reader["BuildingName"].ToString();
                    buildingResponse.BuildingStatus = Convert.ToChar(reader["BuildingStatus"]);
                    buildingResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                    buildingResponse.AdminID = Convert.ToInt32(reader["AdminID"]);
                    buildingResponse.LastUpdateDate = DateTime.Parse(reader["LastUpdateDate"].ToString());
                    buildingResponse.LastUpdateAdminID = Convert.ToInt32(reader["LastUpdateAdminID"]);
                    buildingResponses.Add(buildingResponse);
                }
                await connection.CloseAsync();
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            return Ok(buildingResponses);
        }
    }
    [HttpGet("deleteBuilding")]
    public async Task<IActionResult> DeleteBuilding (int _buildingID, int _adminID) {

        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "deleteBuilding"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_buildingID", MySqlDbType.Int32).Value = _buildingID;
            cmd.Parameters.Add("_buildingStatus", MySqlDbType.VarChar).Value = 'F';
            cmd.Parameters.Add("_lastUpdateDate", MySqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("_lastUpdateAdminID", MySqlDbType.Int32).Value = _adminID;
            
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<BuildingResponse> buildingResponses = new List<BuildingResponse>();
            try
            {
                while (reader.Read())
                {
                    BuildingResponse buildingResponse = new BuildingResponse();
                    buildingResponse.BuildingID = Convert.ToInt32(reader["BuildingID"]);
                    buildingResponse.BuildingName = reader["BuildingName"].ToString();
                    buildingResponse.BuildingStatus = Convert.ToChar(reader["BuildingStatus"]);
                    buildingResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                    buildingResponse.AdminID = Convert.ToInt32(reader["AdminID"]);
                    buildingResponse.LastUpdateDate = DateTime.Parse(reader["LastUpdateDate"].ToString());
                    buildingResponse.LastUpdateAdminID = Convert.ToInt32(reader["LastUpdateAdminID"]);
                    buildingResponses.Add(buildingResponse);
                }
                await connection.CloseAsync();
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            return Ok(buildingResponses);
        }
    }
}