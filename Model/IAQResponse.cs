using System;
using DPU_AQD_API.Models;

public class IAQResponse
{
    public int IAQ_ID {get; set;}
    public DateTime timestamp {get; set;}
    public int Value {get; set;}
    public int CO2_Value {get; set;}
}