﻿using StationTireInspection.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StationTireInspection.JDO.DataToServer
{
    public class UserInformationJDO
    {
        public Result Status { get; set; } = Result.NoLogged;
        public int PersonalID { get; set; } = 40180000;
        public string FirstName { get; set; } = "First Name";
        public string LastName { get; set; } = "Last Name";
    }
}