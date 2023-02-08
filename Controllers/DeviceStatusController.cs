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
    public async Task<IActionResult> GetStatusData () 
    {
        using (MySqlConnection connection = new MySqlConnection(sQLConection.strConnection)){
            
            return Ok();
        }
    }
}