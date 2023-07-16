using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Net.Http.Headers;
using System.Text.Json;
using Newtonsoft.Json;
using DPU_AQD_API.Models;

namespace DPU_AQD_API;
[ApiController]
[Route("webapi")]

public class IAQController : ControllerBase
{
    private SQLConection sQLConection = new SQLConection();
    [HttpGet("GetIAQData")]
    public async Task<IActionResult> GetIAQData()
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "get_iaq"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<IAQResponse> iAQResponses = new List<IAQResponse>();
            while (reader.Read())
            {
                IAQResponse iAQResponse = new IAQResponse();
                iAQResponse.IAQ_ID = reader["IAQ_ID"].ToString();
                iAQResponse.timestamp = DateTime.Parse(reader["Timestamp"].ToString());
                iAQResponse.Value = Convert.ToInt32(reader["IAQ_Value"]);
                iAQResponse.DeviceID = reader["DeviceID"].ToString();

                iAQResponses.Add(iAQResponse);
            }
            await connection.CloseAsync();
            return Ok(iAQResponses);
        }
    }
    [HttpGet("GetLatestIAQByRoomName")]
    public async Task<IActionResult> GetLatestIAQByRoomName(string _roomName)
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "get_iaqByRoomName"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_RoomName", MySqlDbType.VarChar).Value = _roomName;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<IAQResponse> iAQResponses = new List<IAQResponse>();
            while (reader.Read())
            {
                IAQResponse iAQResponse = new IAQResponse();
                iAQResponse.IAQ_ID = reader["IAQ_ID"].ToString();
                iAQResponse.timestamp = DateTime.Parse(reader["Timestamp"].ToString());
                iAQResponse.Value = Convert.ToInt32(reader["IAQ_Value"]);
                iAQResponse.DeviceID = reader["DeviceID"].ToString();

                iAQResponses.Add(iAQResponse);
            }
            await connection.CloseAsync();
            return Ok(iAQResponses);
        }
    }
    [HttpGet("CalculateIAQ")]
    public async Task<IActionResult> CalculateIAQ(int _deviceID)
    {
        double[] eqArr = new double [2];
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getAVG_Eq"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_DeviceID", MySqlDbType.Int32).Value = _deviceID;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<EqResponse> eqResponses = new List<EqResponse>();
            try
            {
                while (reader.Read())
                {
                    EqResponse eqResponse = new EqResponse();
                    eqResponse.avg_pm2_5 = Convert.ToDouble(reader["avg(PM2_5)"]);
                    eqResponse.avg_pm10 = Convert.ToDouble(reader["avg(PM_10)"]);
                    eqResponse.max_pm2_5 = Convert.ToInt32(reader["max(PM2_5)"]);
                    eqResponse.min_pm2_5 = Convert.ToInt32(reader["min(PM2_5)"]);
                    eqResponse.max_pm10 = Convert.ToInt32(reader["max(PM_10)"]);
                    eqResponse.min_pm10 = Convert.ToInt32(reader["min(PM_10)"]);

                    eqResponses.Add(eqResponse);

                    eqArr[0] = eqResponse.avg_pm2_5;
                    eqArr[1] = eqResponse.avg_pm10;
                }
                await connection.CloseAsync();
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }

            //Calculation
            int[] FindEqResult = FindMaxEq(eqArr);
            int[] FindX_MaxMinResult = FindX_MaxMin(FindEqResult[0], FindEqResult[1]);
            int[] FindIAQ_MaxMinResult = FindIAQ_MaxMin(FindEqResult[0], FindEqResult[1]);

            string latestID = "";
            MySqlCommand cmd2 = new MySqlCommand();
            cmd2.Connection = connection;
            cmd2.CommandText = "getLatestIAQ";
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.Parameters.Add("_deviceID", MySqlDbType.Int32).Value = _deviceID;
            await connection.OpenAsync();

            MySqlDataReader reader2 = cmd2.ExecuteReader();
            try
            {
                if (!reader2.IsDBNull(0))
                {
                    while (reader2.Read())
                    {
                        latestID = Convert.ToString(reader2["IAQ_ID"]);
                    }
                }
                else
                {
                    latestID = (DateTime.Now.ToString("yyyyMM") + String.Format("{0:00000}", 1));
                }
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }
            await connection.CloseAsync();


            MySqlCommand cmd3 = new MySqlCommand();
            cmd3.Connection = connection;
            cmd3.CommandText = "sentIAQCalulation"; //Store Procedure Name
            cmd3.CommandType = System.Data.CommandType.StoredProcedure;

            string iAQID_date = latestID.Substring(0, 6);
            string iAQID = latestID.Substring(6);

            if (DateTime.Now.ToString("yyyyMM") == iAQID_date)
            {
                cmd3.Parameters.Add("_iaqID", MySqlDbType.VarChar).Value = (DateTime.Now.ToString("yyyyMM") + String.Format("{0:00000}", (Convert.ToInt64(iAQID)) + 1));
            }
            else
            {
                cmd3.Parameters.Add("_iaqID", MySqlDbType.VarChar).Value = (DateTime.Now.ToString("yyyyMM") + String.Format("{0:00000}", 1));
            }

            cmd3.Parameters.Add("_timestamp", MySqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd3.Parameters.Add("_iaqValue", MySqlDbType.Int32).Value = IAQCalculation(FindEqResult[0], FindX_MaxMinResult[0], FindX_MaxMinResult[1], FindIAQ_MaxMinResult[0], FindIAQ_MaxMinResult[1]);
            cmd3.Parameters.Add("_deviceID", MySqlDbType.Int32).Value = _deviceID;
            await connection.OpenAsync();

            MySqlDataReader reader3 = cmd3.ExecuteReader();
            List<IAQResponse> iAQResponses = new List<IAQResponse>();
            try
            {
                while (reader3.Read())
                {

                    IAQResponse iAQResponse = new IAQResponse();
                    iAQResponse.IAQ_ID = reader3["IAQ_ID"].ToString();
                    iAQResponse.timestamp = DateTime.Parse(reader3["Timestamp"].ToString());
                    iAQResponse.Value = Convert.ToInt32(reader3["IAQ_Value"]);
                    iAQResponse.DeviceID = reader["DeviceID"].ToString();

                    iAQResponses.Add(iAQResponse);

                }
                await connection.CloseAsync();
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex);
            }

            return Ok(iAQResponses);
        }
    }
    public class CO2
    {
        public int CO2_Value { get; set; }
        public int CO2_Lv { get; set; }
        public string CO2_Lv_Desc { get; set; }
    }
    [HttpGet("CalculateCO2")]
    public async Task<IActionResult> CalculateCO2(int _deviceID)
    {
        List<CO2> co2_list = new List<CO2>();
        CO2 co2_Item = new CO2();
        string Lv_Desc = "";

        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection))
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "getlatest_co2"; //Store Procedure Name
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("_DeviceID", MySqlDbType.Int32).Value = _deviceID;
            await connection.OpenAsync();

            MySqlDataReader reader = cmd.ExecuteReader();
            int co2_input = 0;

            while (reader.Read())
            {
                co2_input = Convert.ToInt32(reader["VOC"]);
            }

            co2_Item.CO2_Value = co2_input;
            co2_Item.CO2_Lv = CO2Calculation(co2_input);

            if (co2_Item.CO2_Lv == 1)
            {
                Lv_Desc = "Normal";
            }
            else if (co2_Item.CO2_Lv == 2)
            {
                Lv_Desc = "Affecting to Concentration";
            }
            else if (co2_Item.CO2_Lv == 3)
            {
                Lv_Desc = "Risk to health";
            }
            else if (co2_Item.CO2_Lv == 4)
            {
                Lv_Desc = "Harmful to health";
            }
            co2_Item.CO2_Lv_Desc = Lv_Desc;

            co2_list.Add(co2_Item);

            return Ok(co2_list);
        }
    }

    private int[] FindMaxEq(double[] input)
    {
        double maxEq = input[0];
        int eqIndex = 0;
        int[] result = new int[2];
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] > maxEq)
            {
                maxEq = input[i];
                eqIndex = i;
            }
        }
        result[0] = Convert.ToInt32(maxEq);
        result[1] = eqIndex;

        return result;
    }
    private int[] FindIAQ_MaxMin(int x, int eqIndex)
    {
        int[] max_min_IAQ = new int[2];
        if (eqIndex == 0)
        { //PM 2.5
            if (x < 26)
            {
                max_min_IAQ[0] = 0; //Min value
                max_min_IAQ[1] = 25; //Max value
            }
            else if (x < 38)
            {
                max_min_IAQ[0] = 26; //Min value
                max_min_IAQ[1] = 50; //Max value
            }
            else if (x < 51)
            {
                max_min_IAQ[0] = 51; //Min value
                max_min_IAQ[1] = 100; //Max value
            }
            else if (x < 91)
            {
                max_min_IAQ[0] = 100; //Min value
                max_min_IAQ[1] = 200; //Max value
            }
            else
            {
                max_min_IAQ[0] = 201; //Min value
                max_min_IAQ[1] = 0; //Max value
            }
        }
        else if (eqIndex == 1)
        { //PM 10
            if (x < 51)
            {
                max_min_IAQ[0] = 0; //Min value
                max_min_IAQ[1] = 25; //Max value
            }
            else if (x < 81)
            {
                max_min_IAQ[0] = 26; //Min value
                max_min_IAQ[1] = 50; //Max value
            }
            else if (x < 121)
            {
                max_min_IAQ[0] = 51; //Min value
                max_min_IAQ[1] = 100; //Max value
            }
            else if (x < 181)
            {
                max_min_IAQ[0] = 101; //Min value
                max_min_IAQ[1] = 200; //Max value
            }
            else
            {
                max_min_IAQ[0] = 201; //Min value
                max_min_IAQ[1] = 0; //Max value
            }
        }
        return max_min_IAQ;
    }
    private int[] FindX_MaxMin(int x, int eqIndex)
    {
        int[] max_min_Eq = new int[2];
        if (eqIndex == 0)
        { //PM 2.5
            if (x < 26)
            {
                max_min_Eq[0] = 0; //Min value
                max_min_Eq[1] = 25; //Max value
            }
            else if (x < 38)
            {
                max_min_Eq[0] = 26; //Min value
                max_min_Eq[1] = 37; //Max value
            }
            else if (x < 51)
            {
                max_min_Eq[0] = 38; //Min value
                max_min_Eq[1] = 50; //Max value
            }
            else if (x < 91)
            {
                max_min_Eq[0] = 51; //Min value
                max_min_Eq[1] = 90; //Max value
            }
            else
            {
                max_min_Eq[0] = 91; //Min value
                max_min_Eq[1] = 0; //Max value
            }
        }
        else if (eqIndex == 1)
        { //PM 10
            if (x < 51)
            {
                max_min_Eq[0] = 0; //Min value
                max_min_Eq[1] = 50; //Max value
            }
            else if (x < 81)
            {
                max_min_Eq[0] = 51; //Min value
                max_min_Eq[1] = 80; //Max value
            }
            else if (x < 121)
            {
                max_min_Eq[0] = 81; //Min value
                max_min_Eq[1] = 120; //Max value
            }
            else if (x < 181)
            {
                max_min_Eq[0] = 121; //Min value
                max_min_Eq[1] = 180; //Max value
            }
            else
            {
                max_min_Eq[0] = 181; //Min value
                max_min_Eq[1] = 0; //Max value
            }
        }
        return max_min_Eq;
    }
    private int IAQCalculation(int x, int min_x, int max_x, int min_i, int max_i)
    {
        return (int)(((float)(max_i - min_i) / (max_x - min_x)) * (x - min_x) + min_i);
    }
    private int CO2Calculation(int co2_ppm)
    {
        int result_Lv;
        if (co2_ppm < 1000)
        {
            result_Lv = 1; //Normal
        }
        else if (co2_ppm < 1500)
        {
            result_Lv = 2; //Affecting to Concentration
        }
        else if (co2_ppm < 5000)
        {
            result_Lv = 3; //Risk to health
        }
        else
        {
            result_Lv = 4; //Harmful to health
        }

        return result_Lv;
    }
}