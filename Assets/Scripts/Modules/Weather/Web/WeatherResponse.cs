using System;
using Newtonsoft.Json;

namespace CWD.Scripts.Modules.Weather.Web
{
    [Serializable]
    public class WeatherResponse
    {
        [JsonProperty("properties")] public WeatherProperties Properties;
    }

    [Serializable]
    public class WeatherProperties
    {
        [JsonProperty("periods")] public WeatherPeriod[] Periods;
    }

    [Serializable]
    public class WeatherPeriod
    {
        [JsonProperty("name")] public string Name;

        [JsonProperty("temperature")] public int Temperature;

        [JsonProperty("temperatureUnit")] public string TemperatureUnit;

        [JsonProperty("icon")] public string Icon;

        [JsonProperty("shortForecast")] public string ShortForecast;

        [JsonProperty("isDaytime")] public bool IsDaytime;
    }
}