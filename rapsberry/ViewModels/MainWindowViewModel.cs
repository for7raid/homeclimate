using Avalonia.Media.Imaging;
using Avalonia.Threading;
using InTheHand.Devices.Enumeration;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http;
using System.Text;


//https://api.weather.yandex.ru/v1/informers?lat=55.75396&lon=37.620393
namespace homeclimate.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private static HttpClient client = new HttpClient();
        public DayInfo DayInfo { get; set; } = new DayInfo();
        private DateTime _now = DateTime.Now;

        public DateTime Now
        {
            get => _now;
            set => this.RaiseAndSetIfChanged(ref _now, value);
        }

        public ObservableCollection<ForecastPartViewModel> Forecasts { get; } = new ObservableCollection<ForecastPartViewModel>();
        public HomeTempViewModel Home { get; } = new HomeTempViewModel();
        public MainWindowViewModel()
        {
            client.DefaultRequestHeaders.Add("X-Yandex-API-Key", "79f5bbb9-cb0b-441a-99cc-315e6c536070");

            var timeTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1), IsEnabled = true };
            timeTimer.Tick += TimeTimer_Tick;
            timeTimer.Start();

            var forecastTimer = new DispatcherTimer() { Interval = TimeSpan.FromMinutes(30), IsEnabled = true };
            forecastTimer.Tick += (s, e) => { UpdateForecast(); };
            forecastTimer.Start();

            var homeTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(1), IsEnabled = true };
            homeTimer.Tick += (s, e) => { Home.UpdateHome(); };

            UpdateForecast();
#if RELEASE
            Home.UpdateHome();
#endif
        }

        private async void UpdateForecast()
        {
            try
            {
                //var picker = new DevicePicker();
                //var d = await picker.PickSingleDeviceAsync();

#if DEBUG
                var text = await System.IO.File.ReadAllTextAsync(@"C:\Users\for7r\Documents\Visual Studio 2017\Projects\homeclimate\rapsberry\forecast.json");
#else
            
            HttpResponseMessage response = await client.GetAsync($@"https://api.weather.yandex.ru/v1/informers?lat=55.75396&lon=37.620393");
            response.EnsureSuccessStatusCode();
            var text = await response.Content.ReadAsStringAsync();
#endif
                Forecasts.Clear();
                var forecast = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.YandexMeteo>(text);
                DayInfo.Sunrise = DateTime.ParseExact(forecast.forecast.sunrise, "HH:mm", CultureInfo.InvariantCulture);
                DayInfo.Sunset = DateTime.ParseExact(forecast.forecast.sunset, "HH:mm", CultureInfo.InvariantCulture);
                DayInfo.Moon = forecast.forecast.moon_text;

                Forecasts.Add(new ForecastPartViewModel
                {
                    Name = "Сейчас (" + DateTime.Now.ToString("HH:mm") + ")",
                    Temp = forecast.fact.temp,
                    TempFeels = forecast.fact.feels_like,
                    WindDir = forecast.fact.wind_dir,
                    WindSpeed = forecast.fact.wind_speed,
                    WindGust = forecast.fact.wind_gust,
                    Icon = forecast.fact.icon,
                    Condition = forecast.fact.condition,
                    PrecProb = 0
                });

                foreach (var item in forecast.forecast.parts)
                {
                    Forecasts.Add(new ForecastPartViewModel
                    {
                        Name = item.part_name,
                        Temp = item.temp_avg,
                        TempFeels = item.feels_like,
                        WindDir = item.wind_dir,
                        WindSpeed = item.wind_speed,
                        WindGust = item.wind_gust,
                        Icon = item.icon,
                        Condition = item.condition,
                        PrecProb = item.prec_prob
                    });
                }

                Home.Preasure = forecast.fact.pressure_mm;
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
             }
        }

        private void TimeTimer_Tick(object sender, EventArgs e)
        {
            Now = DateTime.Now;
            if (Now.Hour == 7 && Now.Minute == 0 && Now.Second == 0)
                UpdateForecast();
        }
    }
}
