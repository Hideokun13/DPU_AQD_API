using System;

namespace DPU_AQD_API.Models;

public class DeviceStatusResponse
{
    public string StatusID { get; set; }
    public DateTime Timestamp { get; set; }
    public int DeviceID { get; set; }
    public string Sensor_Status { get; set; }
}