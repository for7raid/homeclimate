using System;
using System.Collections.Generic;
using System.Text;

namespace homeclimate.Models
{
    public class YandexMeteo
    {
        public Fact fact { get; set; }
        public Forecast forecast { get; set; }
    }

    public class Fact
    {
        public int temp { get; set; }
        public int feels_like { get; set; }
        public string icon { get; set; }
        public string condition { get; set; }
        public double wind_speed { get; set; }
        public double wind_gust { get; set; }
        public string wind_dir { get; set; }

        public int pressure_mm { get; set; }

    }

    public class Forecast
    {
        public long date_ts { get; set; }
        public string sunrise { get; set; }
        public string sunset { get; set; }
        public string moon_text { get; set; }
        public int moon_code { get; set; }
        public List<ForecastPart> parts { get; set; }

        
    }

    public class ForecastPart
    {
        public string part_name { get; set; }
        public int temp_avg { get; set; }
        public int feels_like { get; set; }
        public string icon { get; set; }
        public string condition { get; set; }
        public double wind_speed { get; set; }
        public double wind_gust { get; set; }
        public string wind_dir { get; set; }

        public int prec_prob { get; set; }
    }
}
