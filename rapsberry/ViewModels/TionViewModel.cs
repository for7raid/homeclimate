
using InTheHand.Devices.Enumeration;
using System;
using System.Collections.Generic;
using System.Text;


namespace homeclimate.ViewModels
{
    
    class TionViewModel
    {
        public TionViewModel()
        {
            var picker = new DevicePicker();
            picker.PickSingleDeviceAsync();
        }
    }
}
