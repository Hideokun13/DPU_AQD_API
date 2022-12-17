using System;
using Newtonsoft.Json;

namespace DPU_AQD_API.Models
{
    public class Air4ThaiResponse
    {
        [JsonProperty("stations")]
        public List<Station> Stations;
    }
    public class AQI
    {
        [JsonProperty("Level")]
        public string Level;

        [JsonProperty("aqi")]
        public string Aqi;
    }

    public class CO
    {
        [JsonProperty("value")]
        public string Value;

        [JsonProperty("unit")]
        public string Unit;
    }

    public class LastUpdate
    {
        [JsonProperty("date")]
        public string Date;

        [JsonProperty("time")]
        public string Time;

        [JsonProperty("PM25")]
        public PM25 PM25;

        [JsonProperty("PM10")]
        public PM10 PM10;

        [JsonProperty("O3")]
        public O3 O3;

        [JsonProperty("CO")]
        public CO CO;

        [JsonProperty("NO2")]
        public NO2 NO2;

        [JsonProperty("SO2")]
        public SO2 SO2;

        [JsonProperty("AQI")]
        public AQI AQI;
    }

    public class NO2
    {
        [JsonProperty("value")]
        public string Value;
    }

    public class O3
    {
        [JsonProperty("value")]
        public string Value;
    }

    public class PM10
    {
        [JsonProperty("value")]
        public string Value;
    }

    public class PM25
    {
        [JsonProperty("value")]
        public string Value;
    }

    public class SO2
    {
        [JsonProperty("value")]
        public string Value;
    }

    public class Station
    {
        [JsonProperty("stationID")]
        public string StationID;

        [JsonProperty("nameTH")]
        public string NameTH;

        [JsonProperty("nameEN")]
        public string NameEN;

        [JsonProperty("areaTH")]
        public string AreaTH;

        [JsonProperty("areaEN")]
        public string AreaEN;

        [JsonProperty("stationType")]
        public string StationType;

        [JsonProperty("lat")]
        public string Lat;

        [JsonProperty("long")]
        public string Long;

        [JsonProperty("forecast")]
        public List<object> Forecast;

        [JsonProperty("AQILast")]
        public LastUpdate LastUpdate;
    }
}
