using System;
namespace DPU_AQD_API.Models;

public class SurveyResponse
{
    public string surveyID { get; set; }
    public DateTime Timestamp { get; set; }
    public string SubmitData { get; set; }
    public int BuildingID { get; set; }
    public string RoomName { get; set; }
}