using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.Receipt.CloseRecv
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCloseRecvPalletLog : ContentPage
    {
        public pageCloseRecvPalletLog()
        {
            InitializeComponent();
            Title = "Pallet Log";
            btnClose.IsEnabled = false;
        }

        public void DisplayData()
        {
            lblPONumber.Text = pageCloseRecv.myConfirmData.PONumber;
            lblVendorInfo.Text = pageCloseRecv.myConfirmData.VendorID + "-" + pageCloseRecv.myConfirmData.VendorName;
            btnClose.IsEnabled = true;

            fillAcctType(edWoodInQty, cbWoodInAcctType, edWoodInAcct, pageCloseRecv.myConfirmData.palletLogInfo, "WOODIN");
            fillAcctType(edWoodOutQty, cbWoodOutAcctType, edWoodOutAcct, pageCloseRecv.myConfirmData.palletLogInfo, "WOODOUT");
            fillAcctType(edChepInQty, cbChepInAcctType, edChepInAcct, pageCloseRecv.myConfirmData.palletLogInfo, "CHEPIN");
            fillAcctType(edChepOutQty, cbChepOutAcctType, edChepOutAcct, pageCloseRecv.myConfirmData.palletLogInfo, "CHEPOUT");
            fillAcctType(edIGPSInQty, cbIGPSInAcctType, edIGPSInAcct, pageCloseRecv.myConfirmData.palletLogInfo, "IGPSIN");
            fillAcctType(edIGPSOutQty, cbIGPSOutAcctType, edIGPSOutAcct, pageCloseRecv.myConfirmData.palletLogInfo, "IGPSOUT");
            fillAcctType(edMiscInQty, cbMiscInAcctType, edMiscInAcct, pageCloseRecv.myConfirmData.palletLogInfo, "MISCIN");
            fillAcctType(edMiscOutQty, cbMiscOutAcctType, edMiscOutAcct, pageCloseRecv.myConfirmData.palletLogInfo, "MISCOUT");

            if (pageCloseRecv.myConfirmData.palletLogInfo != null)
                edWhse.Text = pageCloseRecv.myConfirmData.palletLogInfo.WhseID;
        }

        private void fillAcctType(Entry aEntryQty, Picker aPickerType, Entry aEntryAcct, ASCTracFunctionStruct.PalletLogInfoType aPalletLogInfo, string aAcctType)
        {
            aPickerType.Items.Clear();
            aPickerType.Items.Add("C-Customer");
            aPickerType.Items.Add("V-Vendor");
            aPickerType.Items.Add("A-Carrier");
            aPickerType.Items.Add("O-Consignee");
            aPickerType.Items.Add("I-Internal");
            aPickerType.Items.Add("U-Unknown");
            /*
            if ((aPalletLogInfo != null) && aPalletLogInfo.palletTypeList.ContainsKey(aAcctType))
            {
                var rec = aPalletLogInfo.palletTypeList[aAcctType];
                if (rec.AcctType == "C")
                    aPickerType.SelectedIndex = 0;
                if (rec.AcctType == "V")
                    aPickerType.SelectedIndex = 1;
                if (rec.AcctType == "A")
                    aPickerType.SelectedIndex = 2;
                if (rec.AcctType == "O")
                    aPickerType.SelectedIndex = 3;
                if (rec.AcctType == "I")
                    aPickerType.SelectedIndex = 4;
                if (rec.AcctType == "U")
                    aPickerType.SelectedIndex = 5;
                aEntryQty.Text = rec.Qty.ToString();
                aEntryAcct.Text = rec.AcctNum;
            }
            */
        }
        /*
        private void fillAcctType(Entry aEntryQty, Picker aPickerType, Entry aEntryAcct, ASCTracFunctionStruct.PalletLogInfoType aPalletLogInfo, string aAcctType)
        {
            aPickerType.Items.Clear();
            aPickerType.Items.Add("C-Customer");
            aPickerType.Items.Add("V-Vendor");
            aPickerType.Items.Add("A-Carrier");
            aPickerType.Items.Add("O-Consignee");
            aPickerType.Items.Add("I-Internal");
            aPickerType.Items.Add("U-Unknown");
            if ((aPalletLogInfo != null) && aPalletLogInfo.palletTypeList.ContainsKey(aAcctType))
            {
                var rec = aPalletLogInfo.palletTypeList[aAcctType];
                if (rec.AcctType == "C")
                    aPickerType.SelectedIndex = 0;
                if (rec.AcctType == "V")
                    aPickerType.SelectedIndex = 1;
                if (rec.AcctType == "A")
                    aPickerType.SelectedIndex = 2;
                if (rec.AcctType == "O")
                    aPickerType.SelectedIndex = 3;
                if (rec.AcctType == "I")
                    aPickerType.SelectedIndex = 4;
                if (rec.AcctType == "U")
                    aPickerType.SelectedIndex = 5;
                aEntryQty.Text = rec.Qty.ToString();
                aEntryAcct.Text = rec.AcctNum;
            }

        }
        */
        public void ClearData()
        {
            lblPONumber.Text = "N/A";
            lblVendorInfo.Text = "N/A";
            btnClose.IsEnabled = false;
        }

        public void SaveData()
        {
            SaveAcctType(edWoodInQty, cbWoodInAcctType, edWoodInAcct, "WOODIN");
            SaveAcctType(edWoodOutQty, cbWoodOutAcctType, edWoodOutAcct, "WOODOUT");
            SaveAcctType(edChepInQty, cbChepInAcctType, edChepInAcct, "CHEPIN");
            SaveAcctType(edChepOutQty, cbChepOutAcctType, edChepOutAcct, "CHEPOUT");
            SaveAcctType(edIGPSInQty, cbIGPSInAcctType, edIGPSInAcct, "IGPSIN");
            SaveAcctType(edIGPSOutQty, cbIGPSOutAcctType, edIGPSOutAcct, "IGPSOUT");
            SaveAcctType(edMiscInQty, cbMiscInAcctType, edMiscInAcct, "MISCIN");
            SaveAcctType(edMiscOutQty, cbMiscOutAcctType, edMiscOutAcct, "MISCOUT");

            if (pageCloseRecv.myConfirmData.palletLogInfo != null)
                pageCloseRecv.myConfirmData.palletLogInfo.WhseID = edWhse.Text;
        }

        public void SaveAcctType(Entry aEntryQty, Picker aPickerType, Entry aEntryAcct, string aAcctType)
        {
            int myQty = 0;
            if (!string.IsNullOrEmpty(aEntryQty.Text))
                myQty = Convert.ToInt32(aEntryQty.Text);
            if (aAcctType.Equals("WOODIN"))
                pageCloseRecv.myConfirmData.numWoodPallets = myQty;
            if (aAcctType.Equals("CHEPIN"))
                pageCloseRecv.myConfirmData.numChepPallets = myQty;
            if (aAcctType.Equals("IGPSIN"))
                pageCloseRecv.myConfirmData.numIGPSPallets = myQty;
            if (aAcctType.Equals("MISCIN"))
                pageCloseRecv.myConfirmData.numMiscPallets = myQty;

            if (myQty > 0)
            {
                if (pageCloseRecv.myConfirmData.palletLogInfo == null)
                    //pageCloseRecv.myConfirmData.palletLogInfo = new ASCTracRestService.DataModel.PalletLogInfoType();// new ascTracWCFService.PalletLogInfoType();
                    pageCloseRecv.myConfirmData.palletLogInfo = new ASCTracFunctionStruct.PalletLogInfoType();
                if (pageCloseRecv.myConfirmData.palletLogInfo.palletTypeList == null)
                    //pageCloseRecv.myConfirmData.palletLogInfo.palletTypeList = new Dictionary<string, ASCTracRestService.DataModel.PalletLogFieldTypeInfo>();
                    pageCloseRecv.myConfirmData.palletLogInfo.palletTypeList = new List<ASCTracFunctionStruct.PalletLogFieldTypeInfo>();
                string myEntryType = "U";
                if (aPickerType.SelectedIndex >= 0)
                    myEntryType = aPickerType.Items[aPickerType.SelectedIndex].Substring(0, 1);
                /*
                if (pageCloseRecv.myConfirmData.palletLogInfo.palletTypeList.ContainsKey(aAcctType))
                {
                    pageCloseRecv.myConfirmData.palletLogInfo.palletTypeList[aAcctType].Qty = myQty;
                    pageCloseRecv.myConfirmData.palletLogInfo.palletTypeList[aAcctType].AcctType = myEntryType;
                    pageCloseRecv.myConfirmData.palletLogInfo.palletTypeList[aAcctType].AcctNum = aEntryAcct.Text;
                    pageCloseRecv.myConfirmData.palletLogInfo.palletTypeList[aAcctType].Changed = true;
                }
                else
                {
                    //var rec = new ASCTracRestService.DataModel.PalletLogFieldTypeInfo( myEntryType, aEntryAcct.Text, myQty); // new ascTracWCFService.PalletLogFieldTypeInfo();
                    var rec = new ASCTracFunctionStruct.PalletLogFieldTypeInfo(myEntryType, aEntryAcct.Text, myQty); // myEntryType, aEntryAcct.Text, myQty); // new ascTracWCFService.PalletLogFieldTypeInfo();
                    pageCloseRecv.myConfirmData.palletLogInfo.palletTypeList.Add(aAcctType, rec);
                }
                */
            }
            else if (pageCloseRecv.myConfirmData.palletLogInfo != null)
            {
                /*
                if (pageCloseRecv.myConfirmData.palletLogInfo.palletTypeList.ContainsKey(aAcctType))
                {
                    //pageCloseRecv.myConfirmData.palletLogInfo.palletTypeList.Remove(aAcctType);
                    pageCloseRecv.myConfirmData.palletLogInfo.palletTypeList[aAcctType].Qty = -1;
                    pageCloseRecv.myConfirmData.palletLogInfo.palletTypeList[aAcctType].Changed = true;

                }
                */
            }
        }

        private void btnNext_Clicked(object sender, EventArgs e)
        {
            (Parent as pageCloseRecvTab).CurrentPage = (Parent as pageCloseRecvTab).Children[2];
        }

        private void btnClose_Clicked(object sender, EventArgs e)
        {
            (Parent as pageCloseRecvTab).CompleteReceipt();
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            //Navigation.PopAsync();
            (Parent as pageCloseRecvTab).CurrentPage = (Parent as pageCloseRecvTab).Children[0];
        }
    }
}
