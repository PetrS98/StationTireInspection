using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace StationTireInspection.JDO.DataToServer
{
    public class DataToServerJDO
    {
        public StationInformationJDO StationInformation { get; set; } = new StationInformationJDO();
        public UserInformationJDO UserInformation { get; set; } = new UserInformationJDO();
        public int NonOperation { get; set; } = 0;
    }
}
