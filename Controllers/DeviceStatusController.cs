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
                DeviceStatusResponse.StatusID = Convert.ToInt32(reader["StatusID"]);
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
        int count = 1;
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
            cmd.CommandText = "sentStatusData"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_StatusID", MySqlDbType.Int32).Value = Convert.ToInt32(DateTime.Now.ToString("yyyyMM") + String.Format("{0:0000}", count));
            cmd.Parameters.Add("_Timestamp", MySqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("_DeviceID", MySqlDbType.Int32).Value = DeviceID;
            cmd.Parameters.Add("_Sensor_Status", MySqlDbType.String).Value = Sensor_status;
            
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<DeviceStatusResponse> deviceStatusResponses = new List<DeviceStatusResponse>();
            while(reader.Read()){
                DeviceStatusResponse deviceStatusResponse = new DeviceStatusResponse();
                deviceStatusResponse.StatusID = Convert.ToInt32(reader["StatusID"]);
                deviceStatusResponse.Timestamp = DateTime.Parse(reader["Timestamp"].ToString());
                deviceStatusResponse.DeviceID = Convert.ToInt32(reader["DeviceID"]);
                deviceStatusResponse.Sensor_Status = reader["Sensor_Status"].ToString();

                deviceStatusResponses.Add(deviceStatusResponse);
            }
            await connection.CloseAsync();
            return Ok(deviceStatusResponses);
        }
    }
}