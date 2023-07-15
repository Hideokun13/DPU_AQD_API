using System;
namespace DPU_AQD_API.Models;
public class DeviceStatusEachDevice
{
    public int BuildingID { get; set; }
    public string BuildingName { get; set; }
    public int RoomID { get; set; }
    public string RoomName { get; set; }
    public int DeviceID { get; set; }
    public string StatusID { get; set; }
    public DateTime Timestamp { get; set; }
    public string Sensor_Status { get; set; }

}
