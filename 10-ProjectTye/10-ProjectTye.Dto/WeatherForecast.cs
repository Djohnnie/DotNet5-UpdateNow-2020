using System;

namespace _10_ProjectTye.Dto
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int Temperature { get; set; }

        public string Summary { get; set; }

        public override string ToString()
        {
            return $"{Date:dd:MM:yyyy}: Temperature: {Temperature}, Summary: {Summary}";
        }
    }
}