using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ASCTracTablet.DataModel
{
    public class dataQCReasonType
    {
        [PrimaryKey]
        public long RefNum { get; set; }
        public bool OnHold { get; set; }
        public string Reason { get; set; }
        public string ReasonDescription { get; set; }
        public DateTime HoldDatetime { get; set; }
        public DateTime ExpectedReleaseDate { get; set; }
        public long MafNum { get; set; }
        public string MafCatID { get; set; }
        public string MafStatus { get; set; }
        public string MafDescription { get; set; }
        public string MafAction { get; set; }
        public bool fNewRec { get; set; }

        public Color bgColor
        {                               // Green                    Red
            get { return !OnHold ? Color.FromHex("8FBC8B") : Color.FromHex("B22222"); }
        }

        public string OnholdDisplay
        {
            get { return !OnHold ? "Released" : "QC Hold: " + HoldDatetime.ToString(); }
        }

    }
}
