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
public class Air4ThaiController : ControllerBase
{

    [HttpGet("GetWeatherAPI")]
    public async Task<IActionResult> GetWeatherAPI()
    {

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
    [HttpGet("GetWeatherAPIByID")]
    public async Task<IActionResult> GetWeatherByID(string _StationID)
    {

        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("User-Agent", "DPU-AQD-API");

        var endpoint = new Uri("http://air4thai.pcd.go.th/services/getNewAQI_JSON.php");
        var result = client.GetAsync(endpoint).Result;
        var json = result.Content.ReadAsStringAsync().Result;

        // Air4ThaiResponse res = JsonSerializer.Deserialize<Air4ThaiResponse>(json);
        // List<IEnumerator> res = JsonSerializer.Deserialize<Air4ThaiResponse>(json);
        // List<Air4ThaiResponse> res = new List<Air4ThaiResponse>();
        var air4thaiCollection = JsonConvert.DeserializeObject<Air4ThaiResponse>(json);

        List<Station> weatherList = new List<Station>();
        foreach (var item in air4thaiCollection.Stations)
        {
            weatherList.Add(item);
        }
        WeatherAPIResponse response = new WeatherAPIResponse();

        foreach (var item in weatherList)
        {
            if (item.StationID == _StationID)
            {
                //Console.WriteLine($"StationID: {item.StationID}\nStationName: {item.NameTH}\nAQI: {item.LastUpdate.AQI.Aqi} ug/m3");
                response.StationID = item.StationID;
                response.StationName = item.NameTH;
                response.AreaTH = item.AreaTH;
                response.Lat = item.Lat;
                response.Long = item.Long;
                response.LastUpdateDate = item.LastUpdate.Date;
                response.LastUpdateTime = item.LastUpdate.Time;
                response.AQI = item.LastUpdate.AQI.Aqi;
                response.PM2_5 = item.LastUpdate.PM25.Value;
                response.PM10 = item.LastUpdate.PM10.Value;

            }
        }


        //foreach (var item in air4thaiCollection)
        //{
        //    Console.WriteLine($"{item.Stations}");
        //}
        return Ok(response);
    }
}