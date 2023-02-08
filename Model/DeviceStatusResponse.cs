using System;

namespace DPU_AQD_API.Models;

public class DeviceStatusResponse
{
    public int StatusID { get; set; }
    public DateTime Timestamp { get; set; }
    public int DeviceID { get; set; }
    public string PMS_Status { get; set; }
    public string BME_Status { get; set; }
}