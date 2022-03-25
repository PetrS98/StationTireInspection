using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace StationTireInspection.UDT
{
    public class SettingsJDO
    {
        public DatabaseSettingsJDO DatabaseSettings { get; set; } = new DatabaseSettingsJDO();
        public BarcodeReaderSettingsJDO BarcodeReaderSettings { get; set; } = new BarcodeReaderSettingsJDO();
        public MainAppConnectionSettingsJDO MainAppConnectionSettings { get; set; } = new MainAppConnectionSettingsJDO();
        public StationSettingsJDO StationSettings { get; set; } = new StationSettingsJDO();

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static SettingsJDO Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<SettingsJDO>(json);
        }
    }
}
