using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace homeclimate.ViewModels
{
    public class DayInfo: ViewModelBase
    {
        public static Dictionary<string, string> MoonNames = new Dictionary<string, string>() {
            { "full-moon", "полнолуние" },
            { "decreasing-moon", "убывающая" },
            { "last-quarter", "посл. чтв." },
            { "new-moon", "новолуние" },
            { "growing-moon", "растущая" },
            { "first-quarter", "перв. чтв." }
        };

        private DateTime _sunrise = DateTime.Now;
        public DateTime Sunrise
        {
            get => _sunrise;
            set { this.RaiseAndSetIfChanged(ref _sunrise, value);}
        }
        private DateTime _sunset = DateTime.Now;
        public DateTime Sunset
        {
            get => _sunset;
            set { this.RaiseAndSetIfChanged(ref _sunset, value); this.RaisePropertyChanged(nameof(DayLong)); this.RaisePropertyChanged(nameof(DayMiddle)); }
        }

        public TimeSpan DayLong
        {
            get => _sunset.Subtract(_sunrise);
        }

        public TimeSpan DayMiddle
        {
            get => (new TimeSpan(_sunrise.Hour, _sunrise.Minute, _sunrise.Second) + new TimeSpan(_sunset.Hour, _sunset.Minute, _sunset.Second)) / 2;
        }

        private string _moon;
        public string Moon
        {
            get => !string.IsNullOrWhiteSpace(_moon) ? MoonNames[ _moon] : "";
            set { this.RaiseAndSetIfChanged(ref _moon, value); }
        }

    }
}
