using System;
using System.Text;
using DPU_AQD_API.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace DPU_AQD_API;
[ApiController]
[Route("hardware")]
public class ReadingController : ControllerBase
{
    private SQLConection sQLConection = new SQLConection();
    [HttpGet("getReadData")]
    public async Task<IActionResult> GetReadData () 
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
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
    [HttpGet("getReadDataByDeviceID")]
    public async Task<IActionResult> GetReadDataByDeviceID (int DeviceID) 
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getReadDataByDeviceID"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_DeviceID", MySqlDbType.Int32).Value = DeviceID;
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
    [HttpGet("getLatestDataByDeviceID")]
    public async Task<IActionResult> getLatestDataByDeviceID (int DeviceID) 
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getLatestDataByDeviceID"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_DeviceID", MySqlDbType.Int32).Value = DeviceID;
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
    [HttpGet("getHoursData")]
    public async Task<IActionResult> getHoursData (int _deviceID, string _startDate, string _endDate) 
    {
        DateTime startDt = Convert.ToDateTime(_startDate);
        DateTime endDt = Convert.ToDateTime(_endDate);

        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getDataHours"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_DeviceID", MySqlDbType.Int32).Value = _deviceID;
            cmd.Parameters.Add("_startDate", MySqlDbType.Date).Value = startDt;
            cmd.Parameters.Add("_endDate", MySqlDbType.Date).Value = endDt;

            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<ReportDataResponse> reportDataResponses = new List<ReportDataResponse>();
            while(reader.Read()){
                ReportDataResponse reportDataResponse = new ReportDataResponse();
                reportDataResponse.Timestamp = (reader["hours"].ToString());
                reportDataResponse.Temp = Convert.ToInt32(reader["avg(Temp)"]);
                reportDataResponse.Humidity = Convert.ToInt32(reader["avg(Humidity)"]);
                reportDataResponse.VOC = Convert.ToInt32(reader["avg(VOC)"]);
                reportDataResponse.PM2_5 = Convert.ToInt32(reader["avg(PM2_5)"]);
                reportDataResponse.PM_10 = Convert.ToInt32(reader["avg(PM_10)"]);

                reportDataResponses.Add(reportDataResponse);
            }
            await connection.CloseAsync();
            return Ok(reportDataResponses);
        }
    }
    [HttpGet("getWeeklyData")]
    public async Task<IActionResult> getWeeklyData (int _deviceID, string _startDate, string _endDate) 
    {
        DateTime startDt = Convert.ToDateTime(_startDate);
        DateTime endDt = Convert.ToDateTime(_endDate);

        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getDataWeekly"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_DeviceID", MySqlDbType.Int32).Value = _deviceID;
            cmd.Parameters.Add("_startDate", MySqlDbType.Date).Value = startDt;
            cmd.Parameters.Add("_endDate", MySqlDbType.Date).Value = endDt;

            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<ReportDataResponse> reportDataResponses = new List<ReportDataResponse>();
            while(reader.Read()){
                ReportDataResponse reportDataResponse  = new ReportDataResponse();
                reportDataResponse.Timestamp = (reader["days"].ToString());
                reportDataResponse.Temp = Convert.ToInt32(reader["avg(Temp)"]);
                reportDataResponse.Humidity = Convert.ToInt32(reader["avg(Humidity)"]);
                reportDataResponse.VOC = Convert.ToInt32(reader["avg(VOC)"]);
                reportDataResponse.PM2_5 = Convert.ToInt32(reader["avg(PM2_5)"]);
                reportDataResponse.PM_10 = Convert.ToInt32(reader["avg(PM_10)"]);

                reportDataResponses.Add(reportDataResponse);
            }
            await connection.CloseAsync();
            return Ok(reportDataResponses);
        }
    }
    [HttpGet("getMonthlyData")]
    public async Task<IActionResult> getMonthlyData(int _deviceID, string _startDate, string _endDate)
    {
        DateTime startDt = Convert.ToDateTime(_startDate);
        DateTime endDt = Convert.ToDateTime(_endDate);

        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getDataMonthly"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_DeviceID", MySqlDbType.Int32).Value = _deviceID;
            cmd.Parameters.Add("_startDate", MySqlDbType.Date).Value = startDt;
            cmd.Parameters.Add("_endDate", MySqlDbType.Date).Value = endDt;

            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<ReportDataResponse> reportDataResponses = new List<ReportDataResponse>();
            while (reader.Read())
            {
                ReportDataResponse reportDataResponse = new ReportDataResponse();
                reportDataResponse.Timestamp = (reader["days"].ToString());
                reportDataResponse.Temp = Convert.ToInt32(reader["avg(Temp)"]);
                reportDataResponse.Humidity = Convert.ToInt32(reader["avg(Humidity)"]);
                reportDataResponse.VOC = Convert.ToInt32(reader["avg(VOC)"]);
                reportDataResponse.PM2_5 = Convert.ToInt32(reader["avg(PM2_5)"]);
                reportDataResponse.PM_10 = Convert.ToInt32(reader["avg(PM_10)"]);

                reportDataResponses.Add(reportDataResponse);
            }
            await connection.CloseAsync();
            return Ok(reportDataResponses);
        }
    }

    [HttpGet("SentReadData")]
    public async Task<IActionResult> SentReadData (int Temp, int Humidity, int VOC, int PM2_5, int PM_10, int DeviceID) {
        int count = 1;
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
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

        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "sentReadData"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_ReadingID", MySqlDbType.Int32).Value = Convert.ToInt32(DateTime.Now.ToString("yyyyMM") + String.Format("{0:0000}", count));
            cmd.Parameters.Add("_Timestamp", MySqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("_Temp", MySqlDbType.Int32).Value = Temp;
            cmd.Parameters.Add("_Humidity", MySqlDbType.Int32).Value = Humidity;
            cmd.Parameters.Add("_VOC", MySqlDbType.Int32).Value = VOC;
            cmd.Parameters.Add("_PM2_5", MySqlDbType.Int32).Value = PM2_5;
            cmd.Parameters.Add("_PM_10", MySqlDbType.Int32).Value = PM_10;
            cmd.Parameters.Add("_DeviceID", MySqlDbType.Int32).Value = DeviceID;
            
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
    [HttpGet("exportReadData")]
    public async Task<IActionResult> exportReadData (int _deviceID, string _startDate, string _endDate, string requestType) 
    {
        DateTime startDt = Convert.ToDateTime(_startDate);
        DateTime endDt = Convert.ToDateTime(_endDate);
        string timestampType = "days";
        string csv = "";

        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            if(requestType == "Hours")
            {
                cmd.CommandText = "getDataHours";
                timestampType = "hours";
            }
            else if (requestType == "Weekly")
            {
                cmd.CommandText = "getDataWeekly";
            }
            else if (requestType == "Monthly")
            {
                cmd.CommandText = "getDataHours";
            }
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_DeviceID", MySqlDbType.Int32).Value = _deviceID;
            cmd.Parameters.Add("_startDate", MySqlDbType.Date).Value = startDt;
            cmd.Parameters.Add("_endDate", MySqlDbType.Date).Value = endDt;

            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<ReportDataResponse> reportDataResponses = new List<ReportDataResponse>();
            csv += timestampType + "," + "avg(Temp)" + "," + "avg(Humidity)" + "," + "avg(VOC)" + "," + "avg(PM2_5)" + "," + "avg(PM_10)" + "\n";
            while (reader.Read()){
                
                csv += reader[timestampType].ToString() + "," + reader["avg(Temp)"].ToString() + "," + reader["avg(Humidity)"].ToString() + "," + reader["avg(VOC)"].ToString() + "," + reader["avg(PM2_5)"].ToString() + "," + reader["avg(PM_10)"].ToString() + "\n";

            }
            await connection.CloseAsync();

            byte[] fileBytes = Encoding.UTF8.GetBytes(csv);
            string fileName = _deviceID.ToString() + "_" + _startDate.ToString() + "_" + _endDate.ToString();

            return File(fileBytes, "text/csv", fileName);
        }
    }
}