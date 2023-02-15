using System;
namespace DPU_AQD_API.Models;

public class BuildingResponse
{
    public int BuildingID {get; set;}
    public string BuildingName {get; set;}
    public DateTime CreateDate {get; set;}
    public char BuildingStatus {get; set;}
    public int AdminID {get; set;}
}