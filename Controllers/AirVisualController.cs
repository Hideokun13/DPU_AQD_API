using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Net.Http.Headers;
using System.Text.Json;

namespace DPU_AQD_API;
[ApiController]
[Route("webapi")]
public class AirVisualController : ControllerBase
{
    [HttpGet("GetAirVisualAPI")]
    public async Task<IActionResult> GetAirVisualAPI(string city, string state, string country){

        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("User-Agent", "DPU-AQD-API"); 

        const string _key = "61bba741-27a3-451d-9148-af80212b5fe6"; //test-callapi-airvisual
        var endpoint = new Uri($"https://api.airvisual.com/v2/city?city={city}&state={state}&country={country}&key={_key}");
        var result = client.GetAsync(endpoint).Result;
        var json = result.Content.ReadAsStringAsync().Result;

        return Ok(json);
    }
}