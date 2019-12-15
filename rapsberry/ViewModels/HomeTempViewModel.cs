using Iot.Device.Si7021;
using ReactiveUI;
using System;
using System.Device.I2c;
using System.IO.Ports;
using System.Threading.Tasks;

namespace homeclimate.ViewModels
{
    public class HomeTempViewModel : ViewModelBase
    {
        private int _temp = 24;
        public int Temp
        {
            get => _temp;
            set => this.RaiseAndSetIfChanged(ref _temp, value);
        }

        private int _hum = 45;
        public int Humidity
        {
            get => _hum;
            set => this.RaiseAndSetIfChanged(ref _hum, value);
        }

        private int _pleasure = 477;
        public int Preasure
        {
            get => _pleasure;
            set => this.RaiseAndSetIfChanged(ref _pleasure, value);
        }

        private int _co2 = 1400;
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
                return 800 - map(_co2, 500, 2000, 0, 800);
            }
        }

        public void UpdateHome()
        {
            try
            {
                I2cDevice device = I2cDevice.Create(new I2cConnectionSettings(1, Si7021.DefaultI2cAddress));

                using (device)
                using (Si7021 sensor = new Si7021(device, Resolution.Resolution1))
                {
                    Humidity = (int)sensor.Humidity;
                    Temp = (int)Math.Round(sensor.Temperature.Celsius, MidpointRounding.AwayFromZero);
                }
                
                Task.Run(() =>
                {
                    using (SerialPort sp = new SerialPort("/dev/ttyS0", 9600, Parity.None, 8, StopBits.One))
                    {

                        var cmd = new byte[9] { 0xFF, 0x01, 0x86, 0x00, 0x00, 0x00, 0x00, 0x00, 0x79 };
                        sp.Open();
                        sp.Write(cmd, 0, cmd.Length);
                        System.Threading.Thread.Sleep(300);

                        var response = new byte[9];
                        sp.Read(response, 0, 9);
                        int i;
                        byte crc = 0;
                        for (i = 1; i < 8; i++) crc += response[i];
                        crc = (byte)((byte)255 - crc);
                        crc++;

                        if (response[0] == 0xFF && response[1] == 0x86 && response[8] == crc)
                        {
                            int responseHigh = (int)response[2];
                            int responseLow = (int)response[3];
                            int ppm = (256 * responseHigh) + responseLow;
                            CO2 = ppm;
                        }
                        else
                        {
                            Console.WriteLine("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}; crc:{9}", response[0], response[1], response[2], response[3], response[4], response[5], response[6], response[7], response[8], crc);
                        }
                        sp.Close();
                    }

                }).Wait(600);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private int map(int x, int in_min, int in_max, int out_min, int out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
    }
}
