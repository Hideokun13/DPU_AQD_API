using System;
using DPU_AQD_API.Models;

public class IAQResponse
{
    public string IAQ_ID {get; set;}
    public DateTime timestamp {get; set;}
    public int Value {get; set;}
    public string DeviceID { get; set;}
}