using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace DPU_AQD_API
{
    [ApiController]
    [Route("[controller]")]
    public class DeviceController : ControllerBase
    {
        [HttpGet("getDevice")]
        public async Task<IActionResult> GetDevice() {
            using (MySqlConnection connection = new MySqlConnection("server=dpu-aqd-db.cea8uizk3jzd.ap-southeast-1.rds.amazonaws.com;port=3306;user=admin;password=admin1234!;database=DPU_AQD_DB;Convert Zero Datetime=True"))
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "getDevice";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                await connection.OpenAsync();

                MySqlDataReader reader = cmd.ExecuteReader();
                List<DeviceResponse> deviceResponses = new List<DeviceResponse>();
                while (reader.Read())
                {
                    DeviceResponse deviceResponse = new DeviceResponse();
                    deviceResponse.DeviceID = reader["DeviceID"].ToString();
                    deviceResponse.DeviceName = reader["DeviceName"].ToString();
                    deviceResponse.Isinstalled = reader["Isinstalled"].ToString();
                    deviceResponses.Add(deviceResponse);
                }
                await connection.CloseAsync();
                return Ok(deviceResponses);
            }
        }

        [HttpGet("getDeviceIDInfo")]
        public async Task<IActionResult> GetDeviceIDInfo(string _deviceID) {
            using (MySqlConnection connection = new MySqlConnection("server=dpu-aqd-db.cea8uizk3jzd.ap-southeast-1.rds.amazonaws.com;port=3306;user=admin;password=admin1234!;database=DPU_AQD_DB;Convert Zero Datetime=True"))
            {
                using (MySqlCommand cmd = connection.CreateCommand()){
                    cmd.Connection = connection;
                    cmd.CommandText = "getDeviceIDInfo";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("_DeviceID", MySqlDbType.VarChar).Value = _deviceID;

                    await connection.OpenAsync();
                
                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<DeviceResponse> deviceResponses = new List<DeviceResponse>();
                    while (reader.Read())
                    {
                        DeviceResponse deviceResponse = new DeviceResponse();
                        deviceResponse.DeviceID = reader["DeviceID"].ToString();
                        deviceResponse.DeviceName = reader["DeviceName"].ToString();
                        deviceResponse.Isinstalled = reader["Isinstalled"].ToString();
                        deviceResponses.Add(deviceResponse);
                    }
                    await connection.CloseAsync();
                    return Ok(deviceResponses);
                }
            }
        }

        [HttpPost("createDevice")]
        public async Task<IActionResult> createDevice(string _deviceName) {
            int count = 1;
            using (MySqlConnection connection = new MySqlConnection("server=dpu-aqd-db.cea8uizk3jzd.ap-southeast-1.rds.amazonaws.com;port=3306;user=admin;password=admin1234!;database=DPU_AQD_DB;Convert Zero Datetime=True"))
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "getDevice";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                await connection.OpenAsync();

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    count++;
                }
                await connection.CloseAsync();
            }

            using (MySqlConnection connection = new MySqlConnection("server=dpu-aqd-db.cea8uizk3jzd.ap-southeast-1.rds.amazonaws.com;port=3306;user=admin;password=admin1234!;database=DPU_AQD_DB;Convert Zero Datetime=True"))
            {
                using (MySqlCommand cmd = connection.CreateCommand()){
                    cmd.Connection = connection;
                    cmd.CommandText = "createDevice";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("_DeviceID", MySqlDbType.VarChar).Value = DateTime.Now.ToString("yyyyMM") + String.Format("{0:0000}", count);
                    cmd.Parameters.Add("_DeviceName", MySqlDbType.VarChar).Value = _deviceName;
                    cmd.Parameters.Add("_Isinstalled", MySqlDbType.VarChar).Value = "true";

                    await connection.OpenAsync();
                
                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<DeviceResponse> deviceResponses = new List<DeviceResponse>();
                    while (reader.Read())
                    {
                        DeviceResponse deviceResponse = new DeviceResponse();
                        deviceResponse.DeviceID = reader["DeviceID"].ToString();
                        deviceResponse.DeviceName = reader["DeviceName"].ToString();
                        deviceResponse.Isinstalled = reader["Isinstalled"].ToString();
                        deviceResponses.Add(deviceResponse);
                    }
                    await connection.CloseAsync();
                    return Ok(deviceResponses);
                }
            }
        }

        [HttpPut("updateIsinstalled")]
        public async Task<IActionResult> updateIsinstalled (string _deviceID, string _newIsinstall) {
            using (MySqlConnection connection = new MySqlConnection("server=dpu-aqd-db.cea8uizk3jzd.ap-southeast-1.rds.amazonaws.com;port=3306;user=admin;password=admin1234!;database=DPU_AQD_DB;Convert Zero Datetime=True"))
            {
                using (MySqlCommand cmd = connection.CreateCommand()){
                    cmd.Connection = connection;
                    cmd.CommandText = "updateIsinstalled";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("_DeviceID", MySqlDbType.VarChar).Value = _deviceID;
                    cmd.Parameters.Add("_Isinstalled", MySqlDbType.VarChar).Value = _newIsinstall;

                    await connection.OpenAsync();
                
                    MySqlDataReader reader = cmd.ExecuteReader();
                    List<DeviceResponse> deviceResponses = new List<DeviceResponse>();
                    while (reader.Read())
                    {
                        DeviceResponse deviceResponse = new DeviceResponse();
                        deviceResponse.DeviceID = reader["DeviceID"].ToString();
                        deviceResponse.DeviceName = reader["DeviceName"].ToString();
                        deviceResponse.Isinstalled = reader["Isinstalled"].ToString();
                        deviceResponses.Add(deviceResponse);
                    }
                    await connection.CloseAsync();
                    return Ok(deviceResponses);
                }
            }
        }
    }
}