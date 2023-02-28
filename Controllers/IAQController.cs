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
    [HttpGet("GetIAQ")]
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
                iAQResponse.IAQ_ID = Convert.ToInt32(reader["IAQ_ID"]);
                iAQResponse.timestamp = DateTime.Parse(reader["Timestamp"].ToString());
                iAQResponse.Value = Convert.ToInt32(reader["IAQ_Value"]);

                iAQResponses.Add(iAQResponse);
            }
            await connection.CloseAsync();
            return Ok(iAQResponses);
        }
    }
    private void FindMaxEq(){
        //
    }
    private int FindIAQ_MaxMin(int x){
        return x;
    }
    private int FindX_MaxMin(int x){
        return x;
    }
    private int IAQCalculation(int x, int min_x, int max_x, int i, int min_i, int max_i){
        return ((max_i - min_i)/(max_x - min_x) * (x - min_x)) + min_i;
    }
}