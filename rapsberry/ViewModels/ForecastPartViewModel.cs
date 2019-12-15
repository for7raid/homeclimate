using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace homeclimate.ViewModels
{

    public class ForecastPartViewModel : ViewModelBase
    {
        private static Dictionary<string, string> WindDirs = new Dictionary<string, string>() {
            { "nw","СЗ"},
            { "n","С"},
            { "ne","СВ"},
            { "e","В"},
            { "se","ЮВ"},
            { "s","Ю"},
            { "sw","ЮЗ"},
            { "w","З"},
            { "с","штиль"}
        };

        private static Dictionary<string, string> DayParts = new Dictionary<string, string>() {
            { "Сейчас","Сейчас"},
            { "night","Ночь"},
            { "morning","Утро"},
            { "day","День"},
            { "evening","Вечер"}
        };

        private static Dictionary<string, string> Conditions = new Dictionary<string, string>() {
            { "clear", "ясно" },
            { "partly-cloudy", "малооблачно" },
            { "cloudy", "облачно с прояснениями" },
            { "overcast", "пасмурно" },
            { "partly-cloudy-and-light-rain", "небольшой дождь" },
            { "partly-cloudy-and-rain", "дождь" },
            { "overcast-and-rain", "сильный дождь" },
            { "overcast-thunderstorms-with-rain", "сильный дождь, гроза" },
            { "cloudy-and-light-rain", "небольшой дождь" },
            { "overcast-and-light-rain", "небольшой дождь" },
            { "cloudy-and-rain", "дождь" },
            { "overcast-and-wet-snow", "дождь со снегом" },
            { "partly-cloudy-and-light-snow", "небольшой снег" },
            { "partly-cloudy-and-snow", "снег" },
            { "overcast-and-snow", "снегопад" },
            { "cloudy-and-light-snow", "небольшой снег" },
            { "overcast-and-light-snow", "небольшой снег" },
            { "cloudy-and-snow", "снег" }
        };

        private string part_name;
        private int temp_avg;
        private int feels_like;
        private string icon;
        private string condition;
        private double wind_speed;
        private double wind_gust;
        private string wind_dir;
        private int prec_prob;

        IAssetLoader assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        static readonly HttpClient client = new HttpClient();
        public string Name
        {
            get => DayParts.ContainsKey(part_name) ? DayParts[part_name] : part_name;
            set => this.RaiseAndSetIfChanged(ref part_name, value);
        }
        public int Temp
        {
            get => temp_avg;
            set => this.RaiseAndSetIfChanged(ref temp_avg, value);
        }

        public int TempFeels
        {
            get => feels_like;
            set => this.RaiseAndSetIfChanged(ref feels_like, value);
        }

        public IBitmap IconBitmap
        {
            get
            {
                var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                //var stream = assets.Open(new Uri($@"avares://homeclimate/Assets/Dark/{icon}.png"));
                var stream = assets.Open(new Uri($@"avares://homeclimate/Assets/{icon.Replace("-","minus_").Replace("+","plus_")}_light.png"));
                var bitmap = new Bitmap(stream);
                return bitmap;
            }
        }

        public string Icon
        {
            get => icon;
            set { this.RaiseAndSetIfChanged(ref icon, value); this.RaisePropertyChanged(nameof(IconBitmap)); }
        }

        public string Condition
        {
            get => Conditions[condition];
            set => this.RaiseAndSetIfChanged(ref condition, value);
        }

        public double WindSpeed
        {
            get => wind_speed;
            set => this.RaiseAndSetIfChanged(ref wind_speed, value);
        }
        public double WindGust
        {
            get => wind_gust;
            set => this.RaiseAndSetIfChanged(ref wind_gust, value);
        }

        public string WindDir
        {
            get => WindDirs[wind_dir];
            set => this.RaiseAndSetIfChanged(ref wind_dir, value);
        }

        public int PrecProb
        {
            get => prec_prob;
            set => this.RaiseAndSetIfChanged(ref prec_prob, value);
        }
    }
}
