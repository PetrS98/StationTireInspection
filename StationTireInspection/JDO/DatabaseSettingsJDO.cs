using System;
using System.Collections.Generic;
using System.Text;

namespace StationTireInspection.UDT
{
    public class DatabaseSettingsJDO
    {
        public string IPAddress { get; set; } = "192.168.1.1";
        public string DatabaseName { get; set; } = "Database";
        public string UsersTableName { get; set; } = "Table";
        public string NonOPsDataTableName { get; set; } = "Table";
        public string DatabaseUserName { get; set; } = "User";
        public string DatabasePassword { get; set; } = "1234";
    }
}
