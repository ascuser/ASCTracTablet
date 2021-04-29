using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.QCHold
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageQCHold : ContentPage
    {
        ASCTracFunctionStruct.QC.QCInventoryType myInvRecord;
        public pageQCHold(string aSkidID)
        {
            InitializeComponent();
            Title = "Quality Control";


            if (string.IsNullOrEmpty(aSkidID))
                InitSkid();
            else
            {
                //fOneSkid = true;
                edSkidID.Text = aSkidID;
                edSkidID.IsEnabled = false;
                btnGo.IsEnabled = false;
                onGo(null, null);
            }
        }

        private void InitSkid()
        {
            edSkidID.Text = string.Empty;
            btnNewHold.IsEnabled = false;
            btnOnTest.IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                if (String.IsNullOrEmpty(edSkidID.Text))
                    edSkidID.Focus();
                else
                    onGo(null, null);
            }
            catch (Exception e)
            {
                DisplayAlert("ASCTrac-QCHold", e.Message, "OK");
            }
        }


        public void onGo(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (!string.IsNullOrEmpty(edSkidID.Text))
                {
                    edSkidID.Text = edSkidID.Text.ToUpper();
                    Globals.curBasicMessage.inputDataList = new List<string>();
                    Globals.curBasicMessage.inputDataList.Add(edSkidID.Text);
                    string errmsg = string.Empty;
                    myIndicator.IsEnabled = true;
                    myIndicator.IsRunning = true;
                    try
                    {
                        var myReturnData = await App.myRestManager.GetSkidQCInfo(Globals.curBasicMessage);

                        if (!myReturnData.successful)
                            errmsg = myReturnData.ErrorMessage;
                        else
                        {
                            myInvRecord = Newtonsoft.Json.JsonConvert.DeserializeObject<ASCTracFunctionStruct.QC.QCInventoryType>(myReturnData.DataMessage);
                            Globals.myDatabase.SaveQCReasons(myInvRecord.qcHoldList);
                            listHolds.ItemsSource = Globals.myDatabase.GetQCReasons();

                            lblItemID.Text = myInvRecord.invRecord.ItemID + "-" + myInvRecord.invRecord.ItemDescription;
                            lblQty.Text = myInvRecord.invRecord.QtyTotal.ToString();
                            btnNewHold.IsEnabled = true;
                            btnOnTest.IsEnabled = (myInvRecord.qcTestSetupList.Count > 0);
                            //await DisplayAlert("Found license", e.Result.SkidID + " Item ID + " + e.Result.ItemID, "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        errmsg = ex.Message;
                    }
                    myIndicator.IsEnabled = false;
                    myIndicator.IsRunning = false;
                    if (!String.IsNullOrEmpty(errmsg))
                        await DisplayAlert("ASCTrac-QC", errmsg, "OK");
                }

            });
        }

        async public void onTest(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new pageQCTest(myInvRecord));
            }
            catch (Exception ex)
            {
                await DisplayAlert("QC Test Setup Exception", ex.ToString(), "OK");
            }
        }

        async public void onNewHold(object sender, EventArgs e)
        {
            DataModel.dataQCReasonType newHold = new DataModel.dataQCReasonType();
            newHold.RefNum = -1;
            newHold.OnHold = true;
            newHold.HoldDatetime = DateTime.Now;
            newHold.fNewRec = true;

            await Navigation.PushAsync(new pageQCHoldDetail(myInvRecord, newHold));
        }

        public void OnSkidCompleted(object sender, EventArgs e)
        {
            onGo(sender, e);
        }

        async public void OnListItemSelected(object sender, EventArgs e)
        {
            var myQCHold = (DataModel.dataQCReasonType)listHolds.SelectedItem;
            await Navigation.PushAsync(new pageQCHoldDetail(myInvRecord, myQCHold));
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}


