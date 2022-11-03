using System;
namespace DPU_AQD_API;

public class Air4ThaiResponse
{
    public string StationID {get;set;}
    public string NameTH {get; set;}
    public string AreaTH {get; set;}
    public string Date {get; set;}
    public string Time {get; set;}
    public int AQI {get; set;}
    public int PM2_5 {get; set;}
    public int PM_10 {get; set;}
    public int O3 {get; set;}
    public int CO {get; set;}
    public int NO2 {get; set;}
    public int SO2 {get; set;}
}