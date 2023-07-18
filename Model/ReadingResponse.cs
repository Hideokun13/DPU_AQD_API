using System;
namespace DPU_AQD_API.Models;

public class ReadingResponse
{
    public string ReadingID {get; set;}
    public DateTime Timestamp {get; set;}
    public int Temp {get; set;}
    public int Humidity {get; set;}
    public int VOC {get; set;}
    public int PM2_5 {get; set;}
    public int PM_10 {get; set;}
    public int DeviceID {get; set;} 

    public DateTime Hours_Timestamp {get; set;}   
}