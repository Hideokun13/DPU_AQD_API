using System;
namespace DPU_AQD_API;

public class ReadingResponse
{
    public int ReadingID {get; set;}
    public DateTime Timestamp {get; set;}
    public int Temp {get; set;}
    public int Humidity {get; set;}
    public int VOC {get; set;}
    public int PM2_5 {get; set;}
    public int PM_10 {get; set;}
    public int DeviceID {get; set;}    
}