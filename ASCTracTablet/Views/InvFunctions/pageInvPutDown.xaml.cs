using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.InvFunctions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageInvPutDown : ContentPage
    {
        //bool fOneSkid = false;
        ASCTracFunctionStruct.Inventory.InvCountType myInvRecord;
        public pageInvPutDown(string aSkidID)
        {
            InitializeComponent();
            Title = "Cycle Count";

            lbledPassword.IsVisible = false;
            edPassword.IsVisible = false;
            lblPasswordMsg.IsVisible = false;

            GetSkidInfo(aSkidID, "", "");
        }
        public pageInvPutDown(string aItemID, string aLocationID)
        {
            InitializeComponent();
            Title = "Cycle Count";

            GetSkidInfo( string.Empty, aItemID, aLocationID);
        }

        async private void GetSkidInfo(string aSkidID, string aItemID, string aLocationID)
        {
            myIndicator.IsEnabled = true;
            myIndicator.IsVisible = true;
            myIndicator.IsRunning = true;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(aSkidID);
                Globals.curBasicMessage.inputDataList.Add(aItemID);
                Globals.curBasicMessage.inputDataList.Add(aLocationID);
                var myReturnData = await App.myRestManager.GetSkidInfo(Globals.curBasicMessage);
                myIndicator.IsVisible = false;
                myIndicator.IsRunning = false;

                if (!myReturnData.successful)
                    await DisplayAlert(Globals.AppTitleName, myReturnData.ErrorMessage, "OK");
                else
                {
                    myInvRecord = Newtonsoft.Json.JsonConvert.DeserializeObject<ASCTracFunctionStruct.Inventory.InvCountType>(myReturnData.DataMessage);
                    DisplayData();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(Globals.AppTitleName, "Exception :" + ex.ToString(), "OK");
            }
            myIndicator.IsVisible = false;
            myIndicator.IsRunning = false;
        }


        private void DisplayData()
        {
            BindingContext = myInvRecord;
            edLocation.Focus();
        }


        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void btnPutdown_Clicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(myInvRecord.NewLocationID))
            {

                myInvRecord.NewQtyTotal = myInvRecord.invRecord.QtyTotal;
                UpdateSkid();
            }
        }


        async private void UpdateSkid()
        {

            myIndicator.IsEnabled = true;
            myIndicator.IsVisible = true;
            myIndicator.IsRunning = true;
            try
            {
                Globals.curBasicMessage.DataMessage = Newtonsoft.Json.JsonConvert.SerializeObject(myInvRecord);

                var myReturnData = await App.myRestManager.UpdateSkid(Globals.curBasicMessage);
                myIndicator.IsVisible = false;
                myIndicator.IsRunning = false;

                if (!myReturnData.successful)
                    await DisplayAlert(Globals.AppTitleName, myReturnData.ErrorMessage, "OK");
                else if (myReturnData.ErrorMessage.StartsWith("QY"))
                {
                    lblPasswordMsg.Text = myReturnData.ErrorMessage.Substring(2).Replace("~", " ").Replace("\r\n", " ");
                    lblPasswordMsg.IsVisible = true;
                    lbledPassword.IsVisible = true;
                    edPassword.IsVisible = true;
                    edPassword.Text = string.Empty;
                    edPassword.Focus();
                }

                else
                {
                    if (String.IsNullOrEmpty(myInvRecord.invRecord.SkidID) || myInvRecord.invRecord.SkidID.StartsWith("-"))
                        Globals.myDatabase.UpdateInv(myInvRecord.invRecord.ItemID, myInvRecord.invRecord.LocationID, myInvRecord.NewQtyTotal, myInvRecord.NewLocationID);
                    else
                        Globals.myDatabase.UpdateSkid(myInvRecord.invRecord.SkidID, myInvRecord.NewQtyTotal, myInvRecord.NewLocationID);
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(Globals.AppTitleName, "Exception :" + ex.ToString(), "OK");
            }
            myIndicator.IsVisible = false;
            myIndicator.IsRunning = false;

        }


        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
