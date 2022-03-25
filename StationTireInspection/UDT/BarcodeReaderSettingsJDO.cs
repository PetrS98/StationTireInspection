using System;
using System.Collections.Generic;
using System.Text;

namespace StationTireInspection.UDT
{
    public class BarcodeReaderSettingsJDO
    {
        public string IPAddress { get; set; } = "192.168.1.1";
        public int Port { get; set; } = 8080;
    }
}
