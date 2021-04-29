using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.COEntry
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCOEntryHdr : ContentPage
    {
        private string fResumeFlag = string.Empty;
        public pageCOEntryHdr()
        {
            InitializeComponent();
            Title = "Header";
            dtpReqShipDate.MinimumDate = DateTime.Now.Date;
            dtpSchedCmpltDate.MinimumDate = DateTime.Now.Date;
        }

        public void Init(string aOrderNumber)
        {
            try
            {
                // call service GetORderInfo, to intiailize 
                if (pageCOEntry.myCOHdr == null)
                {
                    Globals.InitCOrderTypeList();
                    (Parent as pageCOEntry).GetOrder(aOrderNumber);
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Order Entry init", ex.Message, "OK");
            }

        }

        public void SetupHeader()
        {
            BindingContext = pageCOEntry.myCOHdr;
            ascUtils.setupPicker(cbOrderType, Globals.COrderTypeList, pageCOEntry.myCOHdr.OrderType);
            //ascUtils.setupPicker(pickShipto, Globals.lookupCustList, pageCOEntry.myCOHdr.ShiptoCustID);
            //ascUtils.setupPicker(pickSoldto, Globals.lookupCustList, pageCOEntry.myCOHdr.SoldtoCustID);
            ascUtils.setupPicker(cbCarrier, Globals.lookupCarrierList, pageCOEntry.myCOHdr.CarrierID);
        }

        public void SetIndicator(bool aOnFlag)
        {
            myIndicator.IsRunning = aOnFlag;
            myIndicator.IsVisible = aOnFlag;
        }

        private bool DisplayCustInfo(Entry aCustID, Label aCustName)
        {
            bool retval = false;
            if (!String.IsNullOrEmpty(aCustID.Text))
            {
                var myCustList = Globals.lookupCustList.Where(o => o.CustID.Equals(aCustID.Text)).ToList();
                if (myCustList.Count == 0)
                    DisplayAlert("No Customer", "Customer " + aCustID.Text + " not in Item List", "OK");
                else
                {
                    retval = true;
                    aCustName.Text = myCustList[0].CustName;
                }
            }
            return (retval);
        }

        /*
        private void MyClient_GetCOEntryInfoCompleted(object sender, ascTracWCFService.GetCOEntryInfoCompletedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    Globals.curBasicMessage.DataMessage = string.Empty;
                    myIndicator.IsRunning = false;
                    myIndicator.IsVisible = false;
                    var myReturnData = e.Result;
                    if(myReturnData.successful )
                    {
                        pageCOEntry.myCOHdr =Newtonsoft.Json.JsonConvert.DeserializeObject<ASCTracFunctionStruct.COEntry.COEntryHdr>(myReturnData.DataMessage);
                        SetupHeader();
                    }
                    else
                        DisplayAlert("ASCTrac", myReturnData.ErrorMessage, "OK");
                }
                catch (Exception ex)
                {
                    DisplayAlert("ASCTrac", ex.Message, "OK");
                }
            });
        }
        */

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (fResumeFlag.Equals("SoldTo"))
            {
                pageCOEntry.myCOHdr.SoldtoCustID = Views.Maintenances.pageLookupRecord.myResult;
                edSoldto.Text = Views.Maintenances.pageLookupRecord.myResult;
                EdSoldto_Unfocused(edSoldto, null);
            }
            if (fResumeFlag.Equals("ShipTo"))
            {
                pageCOEntry.myCOHdr.ShiptoCustID = Views.Maintenances.pageLookupRecord.myResult;
                edShipto.Text = Views.Maintenances.pageLookupRecord.myResult;
                DisplayCustInfo(edShipto, lblShiptoName);
            }
            if (fResumeFlag.Equals("Setup"))
            {
                (Parent as pageCOEntry).GetSetupData("");
            }
            fResumeFlag = string.Empty;
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {

            Navigation.PopAsync();
        }

        private void btnNext_Clicked(object sender, EventArgs e)
        {
            //pageCOEntry.myCOHdr.SoldtoCustID = ascUtils.getPickerValue(pickSoldto);
            //pageCOEntry.myCOHdr.ShiptoCustID= ascUtils.getPickerValue(pickShipto);
            pageCOEntry.myCOHdr.OrderType = ascUtils.getPickerValue(cbOrderType);
            pageCOEntry.myCOHdr.CarrierID = ascUtils.getPickerValue(cbCarrier);

            SaveHeader();
        }

        async private void SaveHeader()
        { 
            Globals.curBasicMessage.DataMessage = Newtonsoft.Json.JsonConvert.SerializeObject(pageCOEntry.myCOHdr);

            SetIndicator(true);
            string errmsg = string.Empty;
            try
            {
                var myReturnData = await App.myRestManager.COEntrySaveHeader(Globals.curBasicMessage);
                SetIndicator(false);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    pageCOEntry.myCOHdr = Newtonsoft.Json.JsonConvert.DeserializeObject<ASCTracFunctionStruct.COEntry.COEntryHdr>(myReturnData.DataMessage);
                    SetupHeader();
                    (Parent as pageCOEntry).CurrentPage = (Parent as pageCOEntry).Children[1];
                    ((Parent as pageCOEntry).Children[1] as pageCOEntryDet).SetupDetail();
                }
            }
            catch (Exception ex)
            {
                errmsg = "Exception\r\n" + ex.Message;
            }
            SetIndicator(false);
            if (!String.IsNullOrEmpty(errmsg))
                await DisplayAlert("Exception", errmsg, "OK");
        }

        private void CbOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        async private void BtnLookupSoldto_Clicked(object sender, EventArgs e)
        {
            fResumeFlag = "ShipTo";
            Entry myEdit = edShipto;
            if (sender == btnLookupSoldto)
            {
                myEdit = edSoldto;
                fResumeFlag = "SoldTo";
            }
            // lookup Customer
            await Navigation.PushAsync(new Views.Maintenances.pageLookupRecord("C", myEdit.Text, "Order Number", pageCOEntry.myCOHdr.OrderNumber));
        }

        private void EdSoldto_Unfocused(object sender, FocusEventArgs e)
        {
            if (DisplayCustInfo(edSoldto, lblSoldtoName))
            {
                if (String.IsNullOrEmpty(edShipto.Text))
                {
                    pageCOEntry.myCOHdr.ShiptoCustID = edSoldto.Text;
                    edShipto.Text = edSoldto.Text;
                    DisplayCustInfo(edShipto, lblShiptoName);
                }
            }
        }

        private void EdShipto_Unfocused(object sender, FocusEventArgs e)
        {
            DisplayCustInfo(edShipto, lblShiptoName);
        }

        private void BtnSetup_Clicked(object sender, EventArgs e)
        {
            fResumeFlag = "Setup";
            Navigation.PushAsync(new pageCOEntrySetup());
        }
    }
}