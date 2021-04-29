using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ASCTracTablet.DataModel
{
    public class dataWODet
    {
        [PrimaryKey]
        public long SeqNum { get; set; }
        public string Comp_ItemID { get; set; }
        public string Description { get; set; }
        public double Qty { get; set; }
        public double Qty_Picked { get; set; }
        public double Qty_Used { get; set; }
        public string kanban_location { get; set; }

        public Color bgColor
        {
            get { return (Qty <= (Qty_Picked + Qty_Used)) ? Color.FromHex("F0E68C") : !string.IsNullOrEmpty(kanban_location) ? Color.Gray : Color.White; }
            //get { return !Validated ? Color.FromHex("FF99FF") : QAHold == "T" ? Color.FromHex("B22222") : Color.White; }
        }

    }
}
