﻿using System;

namespace PhysisWeather.Core.Domains
{
    [Serializable]
    public class WeatherPeriod
    {
        public int Number { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Name { get; set; }
        public string NameAllCaps { get => Name?.ToUpper(); }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public bool IsDaytime { get; set; }
        public string WindSpeed { get; set; }
        public string WindDirection { get; set; }
        public int Temperature { get; set; }
        public TemperatureTypes TemperatureType { get; set; }
        public string TemperatureTypeShort { get => TemperatureType == TemperatureTypes.Celsius ? "°C" : "°F"; }
        public IconTypes IconType { get; set; }

        public enum TemperatureTypes
        {
            Celsius,
            Fahrenheit
        }

        public enum IconTypes
        {
            AllClear,
            AllClearNight,
            SomeClouds,
            SomeCloudsNight,
            MostlyClouds,
            Snow,
            RainSnow,
            FreezingRain,
            IcePellets,
            Rain,
            Thunderstorm,
            FunnelCloud_Tornado,
            Hurricane_TropicalStorm,
            Windy,
            Dust_Smoke_Haze_Fog,
            Hot,
            Cold,
            Blizzard,
            NotSet
        }
    }
}
