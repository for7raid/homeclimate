using Iot.Device.Bmp180;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.PowerMode;
using Iot.Device.Si7021;
using System;
using System.Device.I2c;
using System.IO.Ports;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World 1!");

            I2cConnectionSettings settings = new I2cConnectionSettings(1, Si7021.DefaultI2cAddress);
            I2cDevice device = I2cDevice.Create(settings);

            using (device)
            using (Si7021 sensor = new Si7021(device, Resolution.Resolution1))
            {
                // opne heater
                sensor.Heater = true;
                // read revision
                byte revision = sensor.Revision;
                // read temperature
                double temperature = sensor.Temperature.Celsius;
                // read humidity
                double humidity = sensor.Humidity;

                Console.WriteLine("rev {0}, temp: {1}, humidity: {2}", revision, temperature, humidity);
            }



            settings = new I2cConnectionSettings(1, Bmp180.DefaultI2cAddress);
            device = I2cDevice.Create(settings);
           
            using(device)
            using (var i2CBmpe80 = new Bmp180(device))
            {


                var preValue = i2CBmpe80.ReadPressure() / 100.0F * 0.75006375541921;
                Console.WriteLine("Pressure: {0}", preValue);


            }

            SerialPort sp = new SerialPort("/dev/ttyS0", 9600);// /dev/ttyAMA0
            sp.DataReceived += Sp_DataReceived;
            var cmd = new byte[9] { 0xFF, 0x01, 0x86, 0x00, 0x00, 0x00, 0x00, 0x00, 0x79 };
            sp.Open();
            sp.Write(cmd, 0, cmd.Length);

            System.Threading.Thread.Sleep(300);

           {
                Console.WriteLine("Bytes to read: {0}", sp.BytesToRead);
                while (sp.BytesToRead > 0)
                {
                    Console.Write(sp.ReadByte());
                    Console.Write(":");
                }
            }

            Console.WriteLine("Bytes to read: {0}", sp.BytesToRead);

            var response = new byte[9];

            sp.Read(response, 0, 9);

            

            Console.WriteLine("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}", response[0], response[1], response[2], response[3], response[4], response[5], response[6], response[7], response[8]);

            int responseHigh = (int) response[2];
            int responseLow = (int) response[3];
            int ppm = (256 * responseHigh) + responseLow;

            Console.WriteLine("CO2: {0}", ppm);
        }

        private static void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
           
        }
    }
}
