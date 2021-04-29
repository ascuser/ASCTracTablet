using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;

namespace ASCTracTablet.DataModel
{
    public class dataOrdrDet
    {
        //SELECT D.LINENUMBER, D.ITEMID, I.DESCRIPTION, D.QTYORDERED, D.QTYPICKED, D.QTYLOADED, 
        // D.ORDERFILLED, D.PICK_LOCATION, NULL AS PCE_TYPE
        [PrimaryKey]
        public long LineNumber { get; set; }
        public string PCEType { get; set; }
        public String OrderNumber { get; set; }
        public string ItemID { get; set; }
        public string Description { get; set; }
        public double QtyOrdered { get; set; }
        public double QtyPicked { get; set; }
        public double QtyLoaded { get; set; }
        public string OrderFilled { get; set; }
        public string PickLocation { get; set; }

        public Color bgColor
        {
            get { return OrderFilled.Equals("O") ? Color.White : Color.LightGray; }
            //get { return !Validated ? Color.FromHex("FF99FF") : QAHold == "T" ? Color.FromHex("B22222") : Color.White; }
        }

        public string Status
        {
            get { return OrderFilled.Equals("O") ? "Open" : OrderFilled.Equals("T") ? "Filled" : OrderFilled.Equals("S") ? "Scratched" : OrderFilled.Equals("X") ? "Cancelled" : OrderFilled.Equals("H") ? "Pick Hold" : "Status " + OrderFilled; }
        }

        public string ItemAndDescription
        {
            get { return ItemID + " - " + Description; }
        }

        public string PCETypeDesc
        {
            get
            {
                if (PCEType.Equals("E"))
                    return ("Eaches");
                if (PCEType.Equals("C"))
                    return ("Cases");
                if (PCEType.Equals("P"))
                    return ("Pallets");
                if (PCEType.Equals("K"))
                    return ("Packs");
                return ("N/A");
            }
        }

    }
}

