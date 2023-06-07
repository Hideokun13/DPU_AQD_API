using System;
using DPU_AQD_API.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace DPU_AQD_API;
[ApiController]
[Route("webapi")]
public class DeviceController : ControllerBase
{
    private SQLConection sQLConection = new SQLConection();
    [HttpGet("getDevice")]
    public async Task<IActionResult> GetDevice () 
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getDevice"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<DeviceResponse> deviceResponses = new List<DeviceResponse>();
            while(reader.Read()){
                DeviceResponse deviceResponse = new DeviceResponse();
                deviceResponse.DeviceID = Convert.ToInt32(reader["deviceID"]);
                deviceResponse.DeviceName = reader["deviceName"].ToString();
                deviceResponse.Isinstalled = Convert.ToChar(reader["Isinstalled"]);
                deviceResponse.RegisterDate = DateTime.Parse(reader["RegisterDate"].ToString());
                deviceResponse.BuildingID = Convert.ToInt32(reader["buildingID"]);
                deviceResponse.RoomID = Convert.ToInt32(reader["roomID"]);
                deviceResponses.Add(deviceResponse);
            }
            await connection.CloseAsync();
            return Ok(deviceResponses);
        }
    }

    [HttpGet("getDeviceByID")]
    public async Task<IActionResult> GetDeviceByID (int DeviceID) 
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getDeviceByID"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_DeviceID", MySqlDbType.Int32).Value = DeviceID;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<DeviceResponse> deviceResponses = new List<DeviceResponse>();
            while(reader.Read()){
                DeviceResponse deviceResponse = new DeviceResponse();
                deviceResponse.DeviceID = Convert.ToInt32(reader["deviceID"]);
                deviceResponse.DeviceName = reader["deviceName"].ToString();
                deviceResponse.Isinstalled = Convert.ToChar(reader["Isinstalled"]);
                deviceResponse.RegisterDate = DateTime.Parse(reader["RegisterDate"].ToString());
                deviceResponse.BuildingID = Convert.ToInt32(reader["buildingID"]);
                deviceResponse.RoomID = Convert.ToInt32(reader["roomID"]);
                deviceResponses.Add(deviceResponse);
            }
            await connection.CloseAsync();
            return Ok(deviceResponses);
        }
    }

    [HttpPost("registerDevice")]
    public async Task<IActionResult> RegisterDevice (string deviceName, int buildingID, int roomID) {
        int count = 1;
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getDevice";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            try{
                while (reader.Read())
                {
                    DeviceResponse deviceResponse = new DeviceResponse();
                    deviceResponse.DeviceName = reader["deviceName"].ToString();
                    if(deviceResponse.DeviceName == deviceName){
                        await connection.CloseAsync();
                        return BadRequest($"Device ID: '{deviceName}' is already exist");
                    }
                    else
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
            cmd.CommandText = "registerDevice"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_DeviceID", MySqlDbType.Int32).Value = Convert.ToInt32(DateTime.Now.ToString("yyyyMM") + String.Format("{0:0000}", count));
            cmd.Parameters.Add("_DeviceName", MySqlDbType.VarChar).Value = deviceName;
            cmd.Parameters.Add("_Isinstalled", MySqlDbType.VarChar).Value = "T";
            cmd.Parameters.Add("_RegisterDate", MySqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("_BuildingID", MySqlDbType.Int32).Value = buildingID;
            cmd.Parameters.Add("_RoomID", MySqlDbType.Int32).Value = roomID;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<DeviceResponse> deviceResponses = new List<DeviceResponse>();
            while(reader.Read()){
                DeviceResponse deviceResponse = new DeviceResponse();
                deviceResponse.DeviceID = Convert.ToInt32(reader["deviceID"]);
                deviceResponse.DeviceName = reader["deviceName"].ToString();
                deviceResponse.Isinstalled = Convert.ToChar(reader["Isinstalled"]);
                deviceResponse.RegisterDate = DateTime.Parse(reader["RegisterDate"].ToString());
                deviceResponse.BuildingID = Convert.ToInt32(reader["buildingID"]);
                deviceResponse.RoomID = Convert.ToInt32(reader["roomID"]);
                deviceResponses.Add(deviceResponse);
            }

            await connection.CloseAsync();
            return Ok(deviceResponses);
        }
    }
    [HttpPost("isInstalled")]
    public async Task<IActionResult> SetIsinstalled (int DeviceID, string Isinstalled) 
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "updateIsinstalled"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_DeviceID", MySqlDbType.Int32).Value = DeviceID;
            cmd.Parameters.Add("_Isinstalled", MySqlDbType.VarChar).Value = Isinstalled;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<DeviceResponse> deviceResponses = new List<DeviceResponse>();
            while(reader.Read()){
                DeviceResponse deviceResponse = new DeviceResponse();
                deviceResponse.DeviceID = Convert.ToInt32(reader["deviceID"]);
                deviceResponse.DeviceName = reader["deviceName"].ToString();
                deviceResponse.Isinstalled = Convert.ToChar(reader["Isinstalled"]);
                deviceResponse.RegisterDate = DateTime.Parse(reader["RegisterDate"].ToString());
                deviceResponse.BuildingID = Convert.ToInt32(reader["buildingID"]);
                deviceResponse.RoomID = Convert.ToInt32(reader["roomID"]);
                deviceResponses.Add(deviceResponse);
            }
            await connection.CloseAsync();
            return Ok(deviceResponses);
        }
    }
}