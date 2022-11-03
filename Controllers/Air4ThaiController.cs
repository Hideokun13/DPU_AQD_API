using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Net.Http.Headers;
using System.Text.Json;

namespace DPU_AQD_API;
[ApiController]
[Route("webapi")]
public class Air4ThaiController : ControllerBase
{
    
    [HttpGet("GetWeatherAPI")]
    public async Task<IActionResult> GetWeatherAPI(){

        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("User-Agent", "DPU-AQD-API");

        var endpoint = new Uri("http://air4thai.pcd.go.th/services/getNewAQI_JSON.php");
        var result = client.GetAsync(endpoint).Result;
        var json = result.Content.ReadAsStringAsync().Result;

        return Ok(json);
    }
}