using System;
using System.Collections.Generic;
using System.Text;

namespace ASCTracTablet.DataModel
{
    public class COEntrySetupType
    {
        public List<ASCTracFunctionStruct.LookupItemType> CustTypeList = new List<ASCTracFunctionStruct.LookupItemType>();
        public List<ASCTracFunctionStruct.LookupItemType> ItemTypeList = new List<ASCTracFunctionStruct.LookupItemType>();

        public string custType = "-";
        public string itemType = "-";
        public string custTypeData = "";
        public string itemTypeData = "";
        public bool fExist = false;
        public COEntrySetupType()
        {
            InitLists();
            ReadConfig();
        }

        private void InitLists()
        {
            if (CustTypeList.Count == 0)
            {
                CustTypeList.Add(new ASCTracFunctionStruct.LookupItemType("R", "Route (Entered)"));
                CustTypeList.Add(new ASCTracFunctionStruct.LookupItemType("C", "Cust Category (User)"));
                CustTypeList.Add(new ASCTracFunctionStruct.LookupItemType("A", "Assoc CustID (User)"));
                CustTypeList.Add(new ASCTracFunctionStruct.LookupItemType("-", "All"));
            }
            if (ItemTypeList.Count == 0)
            {
                ItemTypeList.Add(new ASCTracFunctionStruct.LookupItemType("V", "VMI Customer (Entered)"));
                ItemTypeList.Add(new ASCTracFunctionStruct.LookupItemType("C", "Cust Item"));
                ItemTypeList.Add(new ASCTracFunctionStruct.LookupItemType("-", "All"));
            }
        }

        private void ReadConfig()
        {
            string tmpType = Globals.myDatabase.GetConfigValue("COENTRY_CUSTTYPE");
            if (!String.IsNullOrEmpty(tmpType))
            {
                fExist = true;
                custType = tmpType;
            }
            tmpType = Globals.myDatabase.GetConfigValue("COENTRY_ITEMTYPE");
            if (!String.IsNullOrEmpty(tmpType))
                itemType = tmpType;
            custTypeData = Globals.myDatabase.GetConfigValue("COENTRY_CUSTTYPE_DATA");
            itemTypeData = Globals.myDatabase.GetConfigValue("COENTRY_ITEMTYPE_DATA");
        }
        public void SaveConfig()
        {
            Globals.myDatabase.SaveConfig("COENTRY_CUSTTYPE", custType);
            Globals.myDatabase.SaveConfig("COENTRY_ITEMTYPE", itemType);
            Globals.myDatabase.SaveConfig("COENTRY_CUSTTYPE_DATA", custTypeData);
            Globals.myDatabase.SaveConfig("COENTRY_ITEMTYPE_DATA", itemTypeData);
            fExist = true;
        }

    }

}