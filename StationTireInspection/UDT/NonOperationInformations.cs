using System;
using System.Collections.Generic;
using System.Text;

namespace StationTireInspection.UDT
{
    public class NonOperationInformations
    {
        public DateTime StartNonOPDateTime { get; set; }//done
        public DateTime StopNonOPDateTime { get; set; } //done
        public string NonOperationTime { get; set; }    //done
        public int IDNonOperation { get; set; }         //done
        public int IDUserSelectNonOp { get; set; }      //done
        public int IDUserClearNonOp { get; set; }       //done
        public int IDStation { get; set; }              //done
    }
}
