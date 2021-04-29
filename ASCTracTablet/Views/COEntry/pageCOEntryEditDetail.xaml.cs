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
    public partial class pageCOEntryEditDetail : ContentPage
    {
        private string fResumeFlag = string.Empty;
        ASCTracFunctionStruct.COEntry.COEntryDet fMyDetail;
        public pageCOEntryEditDetail()
        {
            InitializeComponent();
            Title = "Order Entry";
        }


        public pageCOEntryEditDetail(ASCTracFunctionStruct.COEntry.COEntryDet aDetail)
        {
            InitializeComponent();
            Title = "Order Entry";
            fMyDetail = aDetail;
            lblOrderNumber.Text = pageCOEntry.myCOHdr.OrderNumber;
            BindingContext = fMyDetail;

            if (!String.IsNullOrEmpty(fMyDetail.ItemID))
            {
                btnSave.Text = "Save";
                DisplayItemInfo();
            }
        }

        public void SetIndicator(bool aOnFlag)
        {
            myIndicator.IsRunning = aOnFlag;
            myIndicator.IsVisible = aOnFlag;
        }

        private void DisplayItemInfo()
        {
            if (!String.IsNullOrEmpty(edItemID.Text))
            {
                var myItemList = Globals.lookupItemIDList.Where(o => o.ItemID.Equals(edItemID.Text)).ToList();
                if (myItemList.Count == 0)
                    DisplayAlert("No Item", "Item " + edItemID.Text + " not in Item List", "OK");
                else
                {
                    fMyDetail.Description = myItemList[0].Description;
                    fMyDetail.UOM = myItemList[0].StockUOM;
                    lblDescription.Text = fMyDetail.Description;
                    lblUOM.Text = fMyDetail.UOM;
                }
            }
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            // update or add to detaillist
            SaveDetail();
        }

        
        async private void SaveDetail()
        {
            Globals.curBasicMessage.hdrDataMessage = Newtonsoft.Json.JsonConvert.SerializeObject(pageCOEntry.myCOHdr);
            Globals.curBasicMessage.DataMessage = Newtonsoft.Json.JsonConvert.SerializeObject(fMyDetail);

            SetIndicator(true);
            string errmsg = string.Empty;
            try
            {
                var myReturnData = await App.myRestManager.COEntrySaveDetail(Globals.curBasicMessage);
                SetIndicator(false);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    await Navigation.PopAsync();
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



        async private void BtnLookupItemID_Clicked(object sender, EventArgs e)
        {
            fResumeFlag = "ItemID";
            Entry myEdit = edItemID;
            // lookup Customer
            await Navigation.PushAsync(new Views.Maintenances.pageLookupRecord("I", myEdit.Text, "Order Number", pageCOEntry.myCOHdr.OrderNumber));

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (fResumeFlag.Equals("ItemID"))
            {
                fMyDetail.ItemID = Views.Maintenances.pageLookupRecord.myResult;
                edItemID.Text = Views.Maintenances.pageLookupRecord.myResult;
                DisplayItemInfo();
            }
            fResumeFlag = string.Empty;
        }

        private void EdItemID_Completed(object sender, EventArgs e)
        {
            DisplayItemInfo();
        }

        private void EdItemID_Unfocused(object sender, FocusEventArgs e)
        {
            DisplayItemInfo();
        }

        async private void btnScanItemID_Clicked(object sender, EventArgs e)
        {
            try
            {/*
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                scanner.UseCustomOverlay = false;
                scanner.TopText = "Hold the camera up to the barcode\nAbout 6 inches away";
                scanner.BottomText = "Wait for the barcode to automatically scan!";

                ZXing.Result result = await scanner.Scan();
                if (result != null)
                {
                    edItemID.Text = result.Text;
                    DisplayItemInfo();
                }
                */
            }
            catch (Exception ex)
            {
                await DisplayAlert("Scanning", ex.Message, "OK");
            }

        }
    }
}