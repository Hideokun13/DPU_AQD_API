using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace DPU_AQD_API;
[ApiController]
[Route("hardware")]
public class ReadingController : ControllerBase
{
    [HttpGet("getReadData")]
    public async Task<IActionResult> GetReadData () 
    {
        using (MySqlConnection connection = new MySqlConnection("server=dpu-aqd-db.cea8uizk3jzd.ap-southeast-1.rds.amazonaws.com;port=3306;user=admin;password=admin1234!;database=DPU_AQD_DB;Convert Zero Datetime=True")){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getReadData"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<ReadingResponse> readingResponses = new List<ReadingResponse>();
            while(reader.Read()){
                ReadingResponse readingResponse = new ReadingResponse();
                readingResponse.ReadingID = Convert.ToInt32(reader["ReadingID"]);
                readingResponse.Timestamp = DateTime.Parse(reader["Timestamp"].ToString());
                readingResponse.Temp = Convert.ToInt32(reader["Temp"]);
                readingResponse.Humidity = Convert.ToInt32(reader["Humidity"]);
                readingResponse.VOC = Convert.ToInt32(reader["VOC"]);
                readingResponse.PM2_5 = Convert.ToInt32(reader["PM2_5"]);
                readingResponse.PM_10 = Convert.ToInt32(reader["PM_10"]);
                readingResponse.DeviceID = Convert.ToInt32(reader["DeviceID"]);

                readingResponses.Add(readingResponse);
            }
            await connection.CloseAsync();
            return Ok(readingResponses);
        }
    }

    [HttpPost("SentReadData")]
    public async Task<IActionResult> RegisterDevice (int _Temp, int _Humidity, int _VOC, int _PM2_5, int _PM_10, string _DeviceID) {
        int count = 1;
        using (MySqlConnection connection = new MySqlConnection("server=dpu-aqd-db.cea8uizk3jzd.ap-southeast-1.rds.amazonaws.com;port=3306;user=admin;password=admin1234!;database=DPU_AQD_DB;Convert Zero Datetime=True"))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getReadData";
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

        using (MySqlConnection connection = new MySqlConnection("server=dpu-aqd-db.cea8uizk3jzd.ap-southeast-1.rds.amazonaws.com;port=3306;user=admin;password=admin1234!;database=DPU_AQD_DB;Convert Zero Datetime=True")){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "sentReadData"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_ReadingID", MySqlDbType.Int32).Value = Convert.ToInt32(DateTime.Now.ToString("yyyyMM") + String.Format("{0:0000}", count));
            cmd.Parameters.Add("_Timestamp", MySqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("_Temp", MySqlDbType.Int32).Value = _Temp;
            cmd.Parameters.Add("_Humidity", MySqlDbType.Int32).Value = _Humidity;
            cmd.Parameters.Add("_VOC", MySqlDbType.Int32).Value = _VOC;
            cmd.Parameters.Add("_PM2_5", MySqlDbType.Int32).Value = _PM2_5;
            cmd.Parameters.Add("_PM_10", MySqlDbType.Int32).Value = _PM_10;
            cmd.Parameters.Add("_DeviceID", MySqlDbType.Int32).Value = _DeviceID;
            
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<ReadingResponse> readingResponses = new List<ReadingResponse>();
            while(reader.Read()){
                ReadingResponse readingResponse = new ReadingResponse();
                readingResponse.ReadingID = Convert.ToInt32(reader["ReadingID"]);
                readingResponse.Timestamp = DateTime.Parse(reader["Timestamp"].ToString());
                readingResponse.Temp = Convert.ToInt32(reader["Temp"]);
                readingResponse.Humidity = Convert.ToInt32(reader["Humidity"]);
                readingResponse.VOC = Convert.ToInt32(reader["VOC"]);
                readingResponse.PM2_5 = Convert.ToInt32(reader["PM2_5"]);
                readingResponse.PM_10 = Convert.ToInt32(reader["PM_10"]);
                readingResponse.DeviceID = Convert.ToInt32(reader["DeviceID"]);
                
                readingResponses.Add(readingResponse);
            }
            await connection.CloseAsync();
            return Ok(readingResponses);
        }
    }
}