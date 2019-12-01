using Iot.Device.Bmp180;
using Iot.Device.Si7021;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Text;

namespace homeclimate.ViewModels
{
    public class HomeTempViewModel : ViewModelBase
    {
        private int _temp;
        public int Temp
        {
            get => _temp;
            set => this.RaiseAndSetIfChanged(ref _temp, value);
        }

        private int _hum;
        public int Humidity
        {
            get => _hum;
            set => this.RaiseAndSetIfChanged(ref _hum, value);
        }

        private int _pleasure;
        public int Preasure
        {
            get => _pleasure;
            set => this.RaiseAndSetIfChanged(ref _pleasure, value);
        }

        private int _co2 = 1800;
        public int CO2
        {
            get => _co2;
            set
            {
                value = Math.Min(value, 2000);
                value = Math.Max(value, 500);
                this.RaiseAndSetIfChanged(ref _co2, value);
                this.RaisePropertyChanged(nameof(CO2Height));
            }
        }

        public int CO2Height
        {
            get
            {
                return 480 - map(_co2, 500, 2000, 0, 480);
            }
        }

        public void UpdateHome()
        {

            I2cDevice device = I2cDevice.Create(new I2cConnectionSettings(1, Si7021.DefaultI2cAddress));

            using (device)
            using (Si7021 sensor = new Si7021(device, Resolution.Resolution1))
            {
                Humidity = (int)sensor.Humidity;
            }

            device = I2cDevice.Create(new I2cConnectionSettings(1, Bmp180.DefaultI2cAddress));

            using (device)
            using (var i2CBmpe80 = new Bmp180(device))
            {
                var preValue = i2CBmpe80.ReadPressure() / 100.0F * 0.75006375541921;
                Preasure = (int)preValue;
            }
        }

        private int map(int x, int in_min, int in_max, int out_min, int out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
    }
}
