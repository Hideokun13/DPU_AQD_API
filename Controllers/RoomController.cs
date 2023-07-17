using System;
using DPU_AQD_API.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace DPU_AQD_API;
[ApiController]
[Route("webapi")]
public class RoomController : ControllerBase
{
    private SQLConection sQLConection = new SQLConection();
    [HttpGet("getRoom")]
    public async Task<IActionResult> GetRoom () 
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getRoom"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<RoomResponse> roomResponses = new List<RoomResponse>();
            try
            {
                while (reader.Read())
                {
                    RoomResponse roomResponse = new RoomResponse();
                    roomResponse.RoomID = Convert.ToInt32(reader["RoomID"]);
                    roomResponse.RoomName = reader["RoomName"].ToString();
                    roomResponse.RoomStatus = Convert.ToChar(reader["RoomStatus"]);
                    roomResponse.BuildingID = Convert.ToInt32(reader["BuildingID"]);
                    roomResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                    roomResponse.AdminID = Convert.ToInt32(reader["AdminID"]);
                    roomResponse.LastUpdateDate = DateTime.Parse(reader["LastUpdateDate"].ToString());
                    roomResponse.LastUpdateAdminID = Convert.ToInt32(reader["LastUpdateAdminID"]);
                    roomResponse.HasDeviceInstalled = Convert.ToChar(reader["HasDeviceInstalled"]);
                    roomResponses.Add(roomResponse);
                }
                await connection.CloseAsync();
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            return Ok(roomResponses);
        }
    }
    [HttpGet("getRoomByID")]
    public async Task<IActionResult> GetRoomByID (int _roomID) 
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getRoomByID"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_roomID", MySqlDbType.Int32).Value = _roomID;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<RoomResponse> roomResponses = new List<RoomResponse>();
            try
            {
                while (reader.Read())
                {
                    RoomResponse roomResponse = new RoomResponse();
                    roomResponse.RoomID = Convert.ToInt32(reader["RoomID"]);
                    roomResponse.RoomName = reader["RoomName"].ToString();
                    roomResponse.RoomStatus = Convert.ToChar(reader["RoomStatus"]);
                    roomResponse.BuildingID = Convert.ToInt32(reader["BuildingID"]);
                    roomResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                    roomResponse.AdminID = Convert.ToInt32(reader["AdminID"]);
                    roomResponse.LastUpdateDate = DateTime.Parse(reader["LastUpdateDate"].ToString());
                    roomResponse.LastUpdateAdminID = Convert.ToInt32(reader["LastUpdateAdminID"]);
                    roomResponse.HasDeviceInstalled = Convert.ToChar(reader["HasDeviceInstalled"]);
                    roomResponses.Add(roomResponse);
                }
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            await connection.CloseAsync();
            return Ok(roomResponses);
        }
    }
    [HttpGet("getRoomByBuildingID")]
    public async Task<IActionResult> GetRoomByBuildingID(int _buildingID)
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getRoomByBuildingID"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_buildingID", MySqlDbType.Int32).Value = _buildingID;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<RoomResponse> roomResponses = new List<RoomResponse>();
            try
            {
                while (reader.Read())
                {
                    RoomResponse roomResponse = new RoomResponse();
                    roomResponse.RoomID = Convert.ToInt32(reader["RoomID"]);
                    roomResponse.RoomName = reader["RoomName"].ToString();
                    roomResponse.RoomStatus = Convert.ToChar(reader["RoomStatus"]);
                    roomResponse.BuildingID = Convert.ToInt32(reader["BuildingID"]);
                    roomResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                    roomResponse.AdminID = Convert.ToInt32(reader["AdminID"]);
                    roomResponse.LastUpdateDate = DateTime.Parse(reader["LastUpdateDate"].ToString());
                    roomResponse.LastUpdateAdminID = Convert.ToInt32(reader["LastUpdateAdminID"]);
                    roomResponse.HasDeviceInstalled = Convert.ToChar(reader["HasDeviceInstalled"]);
                    roomResponses.Add(roomResponse);
                }
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            await connection.CloseAsync();
            return Ok(roomResponses);
        }
    }
    [HttpPost("registerRoom")]
    public async Task<IActionResult> RegisterRoom (string _roomName, int _buildingID, int _adminID) {
        string latestID = "";
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getLatestRoom";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    latestID = Convert.ToString(reader["RoomID"]);
                }
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            await connection.CloseAsync();
        }

        string roomID_date = latestID.Substring(0, 6);
        string roomID = latestID.Substring(6);

        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "registerRoom"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            if (DateTime.Now.ToString("yyyyMM") == roomID_date)
            {
                cmd.Parameters.Add("_roomID", MySqlDbType.Int32).Value = Convert.ToInt32((DateTime.Now.ToString("yyyyMM") + String.Format("{0:0000}", (Convert.ToInt64(roomID)) + 1)));
            }
            else
            {
                cmd.Parameters.Add("_roomID", MySqlDbType.Int32).Value = Convert.ToInt32((DateTime.Now.ToString("yyyyMM") + String.Format("{0:0000}", 1)));
            }

            cmd.Parameters.Add("_roomName", MySqlDbType.VarChar).Value = _roomName;
            cmd.Parameters.Add("_createDate", MySqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("_roomStatus", MySqlDbType.VarChar).Value = 'T';
            cmd.Parameters.Add("_adminID", MySqlDbType.Int32).Value = _adminID;
            cmd.Parameters.Add("_buildingID", MySqlDbType.Int32).Value = _buildingID;
            cmd.Parameters.Add("_lastUpdateDate", MySqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("_lastUpdateAdminID", MySqlDbType.Int32).Value = _adminID;
            cmd.Parameters.Add("_hasDeviceInstalled", MySqlDbType.VarChar).Value = 'F';

            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<RoomResponse> roomResponses = new List<RoomResponse>();
            try
            {
                while (reader.Read())
                {
                    RoomResponse roomResponse = new RoomResponse();
                    roomResponse.RoomID = Convert.ToInt32(reader["RoomID"]);
                    roomResponse.RoomName = reader["RoomName"].ToString();
                    roomResponse.RoomStatus = Convert.ToChar(reader["RoomStatus"]);
                    roomResponse.BuildingID = Convert.ToInt32(reader["BuildingID"]);
                    roomResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                    roomResponse.AdminID = Convert.ToInt32(reader["AdminID"]);
                    roomResponse.LastUpdateDate = DateTime.Parse(reader["LastUpdateDate"].ToString());
                    roomResponse.LastUpdateAdminID = Convert.ToInt32(reader["LastUpdateAdminID"]);
                    roomResponse.HasDeviceInstalled = Convert.ToChar(reader["HasDeviceInstalled"]);
                    roomResponses.Add(roomResponse);
                }
                await connection.CloseAsync();
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            return Ok(roomResponses);
        }
    }
    [HttpGet("updateRoomName")]
    public async Task<IActionResult> EditRoomName (int _roomID, string _roomName, int _adminID) {

        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "updateRoomName"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_roomID", MySqlDbType.Int32).Value = _roomID;
            cmd.Parameters.Add("_roomName", MySqlDbType.VarChar).Value = _roomName;
            cmd.Parameters.Add("_lastUpdateDate", MySqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("_lastUpdateAdminID", MySqlDbType.Int32).Value = _adminID;
            
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<RoomResponse> roomResponses = new List<RoomResponse>();
            try
            {
                while (reader.Read())
                {
                    RoomResponse roomResponse = new RoomResponse();
                    roomResponse.RoomID = Convert.ToInt32(reader["RoomID"]);
                    roomResponse.RoomName = reader["RoomName"].ToString();
                    roomResponse.RoomStatus = Convert.ToChar(reader["RoomStatus"]);
                    roomResponse.BuildingID = Convert.ToInt32(reader["BuildingID"]);
                    roomResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                    roomResponse.AdminID = Convert.ToInt32(reader["AdminID"]);
                    roomResponse.LastUpdateDate = DateTime.Parse(reader["LastUpdateDate"].ToString());
                    roomResponse.LastUpdateAdminID = Convert.ToInt32(reader["LastUpdateAdminID"]);
                    roomResponse.HasDeviceInstalled = Convert.ToChar(reader["HasDeviceInstalled"]);
                    roomResponses.Add(roomResponse);
                }
                await connection.CloseAsync();
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            return Ok(roomResponses);
        }
    }
    [HttpGet("updateRoomBuilding")]
    public async Task<IActionResult> EditRoomBuilding (int _roomID, int _buildingID, int _adminID) {

        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "updateRoomBuilding"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_roomID", MySqlDbType.Int32).Value = _roomID;
            cmd.Parameters.Add("_buildingID",  MySqlDbType.Int32).Value = _buildingID;
            cmd.Parameters.Add("_lastUpdateDate", MySqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("_lastUpdateAdminID", MySqlDbType.Int32).Value = _adminID;
            
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<RoomResponse> roomResponses = new List<RoomResponse>();
            try
            {
                while (reader.Read())
                {
                    RoomResponse roomResponse = new RoomResponse();
                    roomResponse.RoomID = Convert.ToInt32(reader["RoomID"]);
                    roomResponse.RoomName = reader["RoomName"].ToString();
                    roomResponse.RoomStatus = Convert.ToChar(reader["RoomStatus"]);
                    roomResponse.BuildingID = Convert.ToInt32(reader["BuildingID"]);
                    roomResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                    roomResponse.AdminID = Convert.ToInt32(reader["AdminID"]);
                    roomResponse.LastUpdateDate = DateTime.Parse(reader["LastUpdateDate"].ToString());
                    roomResponse.LastUpdateAdminID = Convert.ToInt32(reader["LastUpdateAdminID"]);
                    roomResponse.HasDeviceInstalled = Convert.ToChar(reader["HasDeviceInstalled"]);
                    roomResponses.Add(roomResponse);
                }
                await connection.CloseAsync();
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            return Ok(roomResponses);
        }
    }
    [HttpGet("deleteRoom")]
    public async Task<IActionResult> DeleteRoom (string _roomName, int _adminID) {

        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "deleteRoom"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_roomName", MySqlDbType.VarChar).Value = _roomName;
            cmd.Parameters.Add("_roomStatus", MySqlDbType.VarChar).Value = 'F';
            cmd.Parameters.Add("_lastUpdateDate", MySqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("_lastUpdateAdminID", MySqlDbType.Int32).Value = _adminID;
            
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<RoomResponse> roomResponses = new List<RoomResponse>();
            try
            {
                while (reader.Read())
                {
                    RoomResponse roomResponse = new RoomResponse();
                    roomResponse.RoomID = Convert.ToInt32(reader["RoomID"]);
                    roomResponse.RoomName = reader["RoomName"].ToString();
                    roomResponse.RoomStatus = Convert.ToChar(reader["RoomStatus"]);
                    roomResponse.BuildingID = Convert.ToInt32(reader["BuildingID"]);
                    roomResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                    roomResponse.AdminID = Convert.ToInt32(reader["AdminID"]);
                    roomResponse.LastUpdateDate = DateTime.Parse(reader["LastUpdateDate"].ToString());
                    roomResponse.LastUpdateAdminID = Convert.ToInt32(reader["LastUpdateAdminID"]);
                    roomResponse.HasDeviceInstalled = Convert.ToChar(reader["HasDeviceInstalled"]);
                    roomResponses.Add(roomResponse);
                }
                await connection.CloseAsync();
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            return Ok(roomResponses);
        }
    }
    [HttpGet("updateHasDeviceInstalled")]
    public async Task<IActionResult> updateHasDeviceInstalled(int _roomID, int _adminID, char _isHasDevice)
    {

        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "updateHasDeviceInstalled"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_roomID", MySqlDbType.Int32).Value = _roomID;
            cmd.Parameters.Add("_hasDeviceInstalled", MySqlDbType.VarChar).Value = _isHasDevice;
            cmd.Parameters.Add("_lastUpdateDate", MySqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("_lastUpdateAdminID", MySqlDbType.Int32).Value = _adminID;

            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<RoomResponse> roomResponses = new List<RoomResponse>();
            try
            {
                while (reader.Read())
                {
                    RoomResponse roomResponse = new RoomResponse();
                    roomResponse.RoomID = Convert.ToInt32(reader["RoomID"]);
                    roomResponse.RoomName = reader["RoomName"].ToString();
                    roomResponse.RoomStatus = Convert.ToChar(reader["RoomStatus"]);
                    roomResponse.BuildingID = Convert.ToInt32(reader["BuildingID"]);
                    roomResponse.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                    roomResponse.AdminID = Convert.ToInt32(reader["AdminID"]);
                    roomResponse.LastUpdateDate = DateTime.Parse(reader["LastUpdateDate"].ToString());
                    roomResponse.LastUpdateAdminID = Convert.ToInt32(reader["LastUpdateAdminID"]);
                    roomResponse.HasDeviceInstalled = Convert.ToChar(reader["HasDeviceInstalled"]);
                    roomResponses.Add(roomResponse);
                }
                await connection.CloseAsync();
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            return Ok(roomResponses);
        }
    }
    [HttpGet("getRoomNameByDeviceId")]
    public async Task<IActionResult> GetRoomNameByDeviceId(int _DeviceID)
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getRoomNameByDeviceId"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_DeviceID", MySqlDbType.Int32).Value = _DeviceID;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<RoomResponse> roomResponses = new List<RoomResponse>();
            try
            {
                while (reader.Read())
                {
                    RoomResponse roomResponse = new RoomResponse();
                    roomResponse.RoomName = reader["RoomName"].ToString();
                    roomResponses.Add(roomResponse);
                }
                await connection.CloseAsync();
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            return Ok(roomResponses);
        }
    }
    [HttpGet("getDeviceIdByRoomName")]
    public async Task<IActionResult> getDeviceIdByRoomName(string _RoomName)
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getDeviceIdByRoomName"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_RoomName", MySqlDbType.VarChar).Value = _RoomName;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<DeviceResponse> deviceResponses = new List<DeviceResponse>();
            try
            {
                while (reader.Read())
                {
                    DeviceResponse deviceResponse = new DeviceResponse();
                    deviceResponse.DeviceID = Convert.ToInt32(reader["deviceID"]);
                    deviceResponses.Add(deviceResponse);
                }
                await connection.CloseAsync();
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            return Ok(deviceResponses);
        }
    }
}