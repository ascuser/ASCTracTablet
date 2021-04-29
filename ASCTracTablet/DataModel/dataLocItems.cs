using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ASCTracTablet.DataModel
{
    public class dataLocItems
    {
        [PrimaryKey]
        public string SkidID { get; set; }
        public string ItemID { get; set; }
        public string ItemDescription { get; set; }
        public double QtyTotal { get; set; }
        public string QAHold { get; set; }
        public string ReasonForHold { get; set; }
        public DateTime DateTimeProd { get; set; }
        public string LocationID { get; set; }
        public string LotID { get; set; }
        public DateTime ExpDate { get; set; }
        public DateTime PutdownDateTime { get; set; }
        public bool Validated { get; set; }

        public string preallocOrderNum { get; set; }
        public string preallocWorkorderID { get; set; }
        public string promoCode { get; set; }
        public string reblendFlag { get; set; }
        public string loctype { get; set; }
        public string locPickableFlag { get; set; }
        public string locPickType { get; set; }


        public Color bgColor
        {
            get { return !Validated ? Color.FromHex("FF99FF") : QAHold == "T" ? Color.FromHex("B22222") : Color.White; }
        }

        public string strExpDate
        {
            get

            {
                if (ExpDate.Equals(DateTime.MinValue) || (ExpDate.CompareTo(DateTime.Parse("1/1/2000")) < 0))
                    return (string.Empty);
                else
                    return (ExpDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture));
            }
        }
        public string strDateTimeProd
        {
            get
            {
                if (DateTimeProd.Equals(DateTime.MinValue) || (DateTimeProd.CompareTo(DateTime.Parse("1/1/2000")) < 0))
                    return (string.Empty);
                else
                    return (DateTimeProd.ToString());
            }
        }

        public string pickableStatus
        {
            get
            {
                if (!string.IsNullOrEmpty(preallocOrderNum))
                    return ("Preallocated to CO " + preallocOrderNum);
                if (!string.IsNullOrEmpty(preallocWorkorderID))
                    return ("Preallocated to WO " + preallocWorkorderID);
                if (!string.IsNullOrEmpty(promoCode))
                    return ("Preallocated to Promo " + promoCode);
                if (!string.IsNullOrEmpty(reblendFlag))
                    return ("Reblend Flag " + reblendFlag);
                if (locPickableFlag.Equals("F"))
                    return ("Location not Pickable");
                if (locPickType.Equals("V"))
                    return ("Voice Location");
                if (locPickType.Equals("C"))
                    return ("Carousel Location");
                if (locPickType.Equals("L"))
                    return ("Light Location");
                return ("Loc Type " + loctype);
            }
        }
    }
}

