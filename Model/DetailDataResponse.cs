using System;
namespace DPU_AQD_API.Models;

public class DetailDataResponse
{
    public string DeviceID {get; set;}
    public string DeviceName {get; set;}
    public string Isinstalled {get; set;}
    public int RoomID {get; set;}
    public int BuildingID {get; set;}
    public ReadingResponse LatestReadingData {get; set;}
    public DeviceStatusResponse LatestStatus {get; set;}

}