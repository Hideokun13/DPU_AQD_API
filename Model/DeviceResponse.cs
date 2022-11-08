using System;
namespace DPU_AQD_API.Models;

public class DeviceResponse
{
    public int DeviceID {get; set;}
    public string DeviceName {get; set;}
    public char Isinstalled {get; set;}
    public DateTime RegisterDate {get; set;}
}