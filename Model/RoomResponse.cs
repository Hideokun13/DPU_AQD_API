using System;
namespace DPU_AQD_API.Models;

public class RoomResponse
{
    public int RoomID {get; set;}
    public string RoomName {get; set;}
    public char RoomStatus {get; set;}
    public int BuildingID {get; set;}
    public DateTime CreateDate {get; set;}
    public int AdminID {get; set;}
}