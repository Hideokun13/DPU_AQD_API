using System;
using DPU_AQD_API.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace DPU_AQD_API;
[ApiController]
[Route("hardware")]
public class DeviceStatusController : ControllerBase
{
    private SQLConection sQLConection = new SQLConection();
    [HttpGet("getStatusData")]
    public async Task<IActionResult> GetStatusData()
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getStatusData"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<DeviceStatusResponse> DeviceStatusResponses = new List<DeviceStatusResponse>();
            while (reader.Read())
            {
                DeviceStatusResponse DeviceStatusResponse = new DeviceStatusResponse();
                DeviceStatusResponse.StatusID = Convert.ToString(reader["StatusID"]);
                DeviceStatusResponse.Timestamp = DateTime.Parse(reader["Timestamp"].ToString());
                DeviceStatusResponse.DeviceID = Convert.ToInt32(reader["DeviceID"]);
                DeviceStatusResponse.Sensor_Status = reader["Sensor_Status"].ToString();

                DeviceStatusResponses.Add(DeviceStatusResponse);
            }
            await connection.CloseAsync();
            return Ok(DeviceStatusResponses);
        }
    }
    [HttpGet("getStatusDataByID")]
    public async Task<IActionResult> GetStatusDataByID(int deviceID)
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getStatusDataByID"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_DeviceID", MySqlDbType.Int32).Value = deviceID;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<DeviceStatusResponse> DeviceStatusResponses = new List<DeviceStatusResponse>();
            while (reader.Read())
            {
                DeviceStatusResponse DeviceStatusResponse = new DeviceStatusResponse();
                DeviceStatusResponse.StatusID = Convert.ToString(reader["StatusID"]);
                DeviceStatusResponse.Timestamp = DateTime.Parse(reader["Timestamp"].ToString());
                DeviceStatusResponse.DeviceID = Convert.ToInt32(reader["DeviceID"]);
                DeviceStatusResponse.Sensor_Status = reader["Sensor_Status"].ToString();

                DeviceStatusResponses.Add(DeviceStatusResponse);
            }
            await connection.CloseAsync();
            return Ok(DeviceStatusResponses);
        }
    }
    [HttpGet("SentStatusData")]
    public async Task<IActionResult> SentStatusData (int DeviceID, string Sensor_status) {
        string latestID = "";
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getStatusData";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            try{
                while (reader.Read())
                {
                    latestID = Convert.ToString(reader["StatusID"]);
                }
            } catch (MySqlException ex){
                return BadRequest(ex);
            }
            await connection.CloseAsync();
        }

        string readingID_date = latestID.Substring(0,6);
        string readingID = latestID.Substring(6);

        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "sentStatusData"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            if(DateTime.Now.ToString("yyyyMM") == readingID_date){
                cmd.Parameters.Add("_StatusID", MySqlDbType.VarChar).Value = (DateTime.Now.ToString("yyyyMM") + String.Format("{0:00000}", (Convert.ToInt64(readingID)) + 1));
            }
            else{
                cmd.Parameters.Add("_StatusID", MySqlDbType.VarChar).Value = (DateTime.Now.ToString("yyyyMM") + String.Format("{0:00000}", 1));
            }

            cmd.Parameters.Add("_Timestamp", MySqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("_DeviceID", MySqlDbType.Int32).Value = DeviceID;
            cmd.Parameters.Add("_Sensor_Status", MySqlDbType.String).Value = Sensor_status;
            
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<DeviceStatusResponse> deviceStatusResponses = new List<DeviceStatusResponse>();
            while(reader.Read()){
                DeviceStatusResponse deviceStatusResponse = new DeviceStatusResponse();
                deviceStatusResponse.StatusID = Convert.ToString(reader["StatusID"]);
                deviceStatusResponse.Timestamp = DateTime.Parse(reader["Timestamp"].ToString());
                deviceStatusResponse.DeviceID = Convert.ToInt32(reader["DeviceID"]);
                deviceStatusResponse.Sensor_Status = reader["Sensor_Status"].ToString();

                deviceStatusResponses.Add(deviceStatusResponse);
            }
            await connection.CloseAsync();
            return Ok(deviceStatusResponses);
        }
    }
    [HttpGet("getLatestStatusEachDevice")]
    public async Task<IActionResult> GetLatestStatusEachDevice()
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getLatestStatusEachDevice"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<DeviceStatusEachDevice> deviceStatusEachDevices = new List<DeviceStatusEachDevice>();
            while (reader.Read())
            {
                DeviceStatusEachDevice deviceStatusEachDevice = new DeviceStatusEachDevice();
                deviceStatusEachDevice.BuildingID = Convert.ToInt32(reader["BuildingID"]);
                deviceStatusEachDevice.BuildingName = reader["BuildingName"].ToString();
                deviceStatusEachDevice.RoomID = Convert.ToInt32(reader["RoomID"]);
                deviceStatusEachDevice.RoomName = reader["RoomName"].ToString();
                deviceStatusEachDevice.StatusID = (reader.IsDBNull(4)) ? "-" : (reader["StatusID"].ToString());
                deviceStatusEachDevice.Timestamp = (reader.IsDBNull(5)) ? DateTime.Parse("1970-01-01 00:00:00") : DateTime.Parse(reader["Timestamp"].ToString());
                deviceStatusEachDevice.DeviceID = (reader.IsDBNull(6)) ? 0 : Convert.ToInt32(reader["DeviceID"]);
                deviceStatusEachDevice.Sensor_Status = (reader.IsDBNull(7)) ? "-" : reader["Sensor_Status"].ToString();

                deviceStatusEachDevices.Add(deviceStatusEachDevice);
            }
            await connection.CloseAsync();

            return Ok(deviceStatusEachDevices);
        }
    }
}