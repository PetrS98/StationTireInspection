using StationTireInspection.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StationTireInspection.JDO.DataToServer
{
    public class UserInformationJDO
    {
        public LoginResult Status { get; set; } = LoginResult.NoLogged;
        public int PersonalID { get; set; } = 40180000;
        public string FirstName { get; set; } = "First Name";
        public string LastName { get; set; } = "Last Name";
    }
}
