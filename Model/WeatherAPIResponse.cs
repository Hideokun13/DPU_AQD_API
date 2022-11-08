using System;
namespace DPU_AQD_API.Models
{
    public class WeatherAPIResponse
    {
        public string StationID { get; set; }
        public string StationName { get; set; }
        public string AreaTH { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string LastUpdateDate { get; set; }
        public string LastUpdateTime { get; set; }
        public string AQI { get; set; }
        public string PM2_5 { get; set; }
        public string PM10 { get; set; }
    }
}
