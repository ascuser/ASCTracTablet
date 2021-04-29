using ASCTracTablet.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASCTracTablet
{
    class Globals
    {
        public static string curSiteID = string.Empty;
        public static string curUserID = string.Empty;
        public static string HHCID = string.Empty;
        public static string UnitID = String.Empty;
        public static string ConnectionID = String.Empty;

        public static string hostURL = "https://localhost:44332/"; // http://10.24.0.6:700/ api/coinfo/?aOrderNumber=CX002442&aUserID=admin&aSiteID=1
        public static ASCTracDB myDatabase; // = new ASCTracDB();

        public const string AppTitleName = "ASCTrac Tablet";
        public const string VersionStr = "10.0.6.Dev";
        public static ASCTracFunctionStruct.ascBasicInboundMessageType curBasicMessage;


        public static List<ASCTracFunctionStruct.LookupItemType> pickStatusList;
        public static List<ASCTracFunctionStruct.LookupItemType> dockList;
        public static List<ASCTracFunctionStruct.LookupItemType> LoadLocList;
        public static List<ASCTracFunctionStruct.LookupItemType> TruckLocList;
        public static List<ASCTracFunctionStruct.LookupItemType> UserList;
        public static List<ASCTracFunctionStruct.LookupItemType> PrinterLBLList;
        public static List<ASCTracFunctionStruct.LookupItemType> PrinterBOLList;
        public static List<ASCTracFunctionStruct.LookupItemType> imageCaptureDocTypeList;

        public static List<ASCTracFunctionStruct.LookupItemType> lookupProdlineList;
        public static List<ASCTracFunctionStruct.LookupItemType> lookupCarrierList;
        public static List<ASCTracFunctionStruct.LookupItemType> lookupShipVIAList;
        public static List<ASCTracFunctionStruct.LookupItemType> lookupMAFReaosnList;
        public static List<ASCTracFunctionStruct.LookupItemType> lookupVendorList;
        public static List<ASCTracFunctionStruct.LookupItemType> lookupWhseList;

        public static List<ASCTracFunctionStruct.LookupCustType> lookupCustList;
        public static List<ASCTracFunctionStruct.LookupItemIDType> lookupItemIDList;


        // Lists generated in code
        public static List<ASCTracFunctionStruct.LookupItemType> PicktoList = new List<ASCTracFunctionStruct.LookupItemType>();
        public static List<ASCTracFunctionStruct.LookupItemType> DockStatusList = new List<ASCTracFunctionStruct.LookupItemType>();
        public static List<ASCTracFunctionStruct.LookupItemType> OrderTypeList = new List<ASCTracFunctionStruct.LookupItemType>();
        public static List<ASCTracFunctionStruct.LookupItemType> COrderTypeList = new List<ASCTracFunctionStruct.LookupItemType>();
        public static List<ASCTracFunctionStruct.LookupItemType> WOStatusList = new List<ASCTracFunctionStruct.LookupItemType>();


        public static void InitPickToList()
        {
            if (Globals.PicktoList.Count == 0)
            {
                Globals.PicktoList.Add(new ASCTracFunctionStruct.LookupItemType("S", "Select at Pick")); // "S", "Select at Pick"));
                Globals.PicktoList.Add(new ASCTracFunctionStruct.LookupItemType("T", "Pick to Truck")); // "T", "Pick to Truck"));
                Globals.PicktoList.Add(new ASCTracFunctionStruct.LookupItemType("F", "Pick to Stage")); // "F", "Pick to Stage"));
            }
        }

        public static void InitDockStatusList()
        {
            if (Globals.DockStatusList.Count == 0)
            {
                Globals.DockStatusList.Add(new ASCTracFunctionStruct.LookupItemType("N", "Not Scheduled"));
                Globals.DockStatusList.Add(new ASCTracFunctionStruct.LookupItemType("S", "Scheduled"));
                Globals.DockStatusList.Add(new ASCTracFunctionStruct.LookupItemType("C", "Completed"));
                Globals.DockStatusList.Add(new ASCTracFunctionStruct.LookupItemType("A", "Appointment"));
            }
        }
        public static void InitOrderTypeList()
        {
            if (Globals.OrderTypeList.Count == 0)
            {
                Globals.OrderTypeList.Add(new ASCTracFunctionStruct.LookupItemType("C", "Customer Order"));
                Globals.OrderTypeList.Add(new ASCTracFunctionStruct.LookupItemType("L", "Load Plan"));
                Globals.OrderTypeList.Add(new ASCTracFunctionStruct.LookupItemType("H", "Shipment"));
                Globals.OrderTypeList.Add(new ASCTracFunctionStruct.LookupItemType("P", "Purchase Order"));
                Globals.OrderTypeList.Add(new ASCTracFunctionStruct.LookupItemType("R", "Receiver"));
                Globals.OrderTypeList.Add(new ASCTracFunctionStruct.LookupItemType("F", "CFT (Inbound)"));
                Globals.OrderTypeList.Add(new ASCTracFunctionStruct.LookupItemType("A", "ASN (Inbound)"));
                Globals.OrderTypeList.Add(new ASCTracFunctionStruct.LookupItemType("M", "RMA (Inbound)"));
                Globals.OrderTypeList.Add(new ASCTracFunctionStruct.LookupItemType("U", "Unload Truck"));
                Globals.OrderTypeList.Add(new ASCTracFunctionStruct.LookupItemType("S", "Customer Appointment"));
                Globals.OrderTypeList.Add(new ASCTracFunctionStruct.LookupItemType("V", "Vendor Appointment"));
                Globals.OrderTypeList.Add(new ASCTracFunctionStruct.LookupItemType("B", "Blocked (Maintenance)"));
            }
        }

        public static void InitCOrderTypeList()
        {
            if (Globals.COrderTypeList.Count == 0)
            {
                Globals.COrderTypeList.Add(new ASCTracFunctionStruct.LookupItemType("S", "Standard"));
                Globals.COrderTypeList.Add(new ASCTracFunctionStruct.LookupItemType("T", "Transfer"));
            }
        }

        public static void InitWOStatusList()
        {
            if (Globals.WOStatusList.Count == 0)
            {
                Globals.WOStatusList.Add(new ASCTracFunctionStruct.LookupItemType("A", "Active"));
                Globals.WOStatusList.Add(new ASCTracFunctionStruct.LookupItemType("D", "Pending"));
                Globals.WOStatusList.Add(new ASCTracFunctionStruct.LookupItemType("N", "Not Scheduled"));
                Globals.WOStatusList.Add(new ASCTracFunctionStruct.LookupItemType("P", "Preparing"));
                Globals.WOStatusList.Add(new ASCTracFunctionStruct.LookupItemType("S", "Scheduled"));

            }
        }

        public static void ReadSettings()
        {
            HHCID = myDatabase.GetConfigValue("HHCID");
            UnitID = myDatabase.GetConfigValue("UNITID");
            var tmp = myDatabase.GetConfigValue("HOST_URL");
            if (!string.IsNullOrEmpty(tmp))
                hostURL = tmp;
        }

        public static void SaveSettings()
        {
            myDatabase.SaveConfig( "HHCID", HHCID);
            myDatabase.SaveConfig("UNITID", UnitID);
            myDatabase.SaveConfig("HOST_URL", hostURL);
        }
    }
}
