using CWD.Scripts.Modules.Weather.Web;

namespace CWD.Scripts.Modules.Weather.Model
{
    public class WeatherModel
    {
        public int Temperature { get; private set; }
        public string TemperatureUnit { get; private set; }
        public string IconUrl { get; private set; }
        public string Description { get; private set; }

        public void Update(WeatherPeriod period)
        {
            Temperature = period.Temperature;
            TemperatureUnit = period.TemperatureUnit;
            IconUrl = period.Icon;
            Description = period.ShortForecast;
        }
    }
}