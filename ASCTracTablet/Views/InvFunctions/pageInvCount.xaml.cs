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
    public partial class pageInvCount : ContentPage
    {
        string fFunction;
        //bool fOneSkid = false;
        ASCTracFunctionStruct.Inventory.InvCountType myInvRecord;
        private static string fDefaultPrinterID = string.Empty;
        public pageInvCount(string aSkidID, string aFunction)
        {
            InitializeComponent();
                fFunction = aFunction;
                Title = fFunction;
                lblTitle.Text = fFunction;

                GetSkidInfo(aSkidID, "", "");
        }
        public pageInvCount(string aItemID, string aLocationID, string aFunction)
        {
            InitializeComponent();
            fFunction = aFunction;
            Title = fFunction;

            GetSkidInfo("", aItemID, aLocationID);
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
            myInvRecord.NewQtyTotal = myInvRecord.invRecord.QtyTotal;
            myInvRecord.NewQtyDualUnit = myInvRecord.invRecord.QtyDualUnit;

            BindingContext = myInvRecord;

            ascUtils.setupPicker(pickLBLPrinter, Globals.PrinterLBLList, fDefaultPrinterID);

            if (fFunction.Contains("Label"))
            {
                myInvRecord.qtyLbls = 1;
                edQtyTotal.IsVisible = false;
                lblEdQtyTotal.IsVisible = false;
                edQtyDualUnit.IsVisible = false;
                lbledQtyDualUnit.IsVisible = false;
                pickReason.IsVisible = false;
                lblPickReason.IsVisible = false;

                edCostCenter.IsVisible = false;
                edRespSite.IsVisible = false;
                lbledCostCenter.IsVisible = false;
                lbledRespSite.IsVisible = false;

                lbledProjNum.IsVisible = false;
                edProjNum.IsVisible = false;

                edComments.IsVisible = true;

                btnCount.Text = "Print";
            }
            else
            {
                myInvRecord.qtyLbls = 0;
                edQtyDualUnit.IsVisible = (myInvRecord.invRecord.QtyDualUnit >= 0);
                lbledQtyDualUnit.IsVisible = edQtyDualUnit.IsVisible;

                var reasonList = Globals.myDatabase.GetReasonCodes();
                pickReason.Items.Clear();
                foreach (var rec in reasonList)
                {
                    pickReason.Items.Add(rec.ReasonCode + " - " + rec.Description);
                }
                edComments.IsEnabled = false;
                edCostCenter.IsEnabled = false;
                edRespSite.IsEnabled = false;

                lbledProjNum.IsVisible = false;
                edProjNum.IsVisible = false;

            }
        }

        private void btnCount_Clicked(object sender, EventArgs e)
        {
            if (fFunction.Contains("Label"))
            {

                fDefaultPrinterID = ascUtils.getPickerValue(pickLBLPrinter);
                myInvRecord.lblPrinterID = fDefaultPrinterID;
            }
            else
            {
                myInvRecord.lblPrinterID = ascUtils.GetIDFromPicker(pickLBLPrinter);
            }

            UpdateSkid();
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
        private void pickReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            string reasonCode = ascUtils.GetIDFromPicker(pickReason);
            var rec = Globals.myDatabase.GetReason(reasonCode);

            if (rec.askCostCenter.Equals("T"))
            {
                edCostCenter.IsEnabled = true;
                edCostCenter.Text = rec.defaultCostCenter;
            }
            else
            {
                edCostCenter.IsEnabled = false;
                edCostCenter.Text = string.Empty;
            }

            edComments.IsEnabled = rec.askComments.Equals("T");
            edRespSite.IsEnabled = rec.askRespSite.Equals("T");
            edProjNum.IsEnabled = rec.askProjectNumber.Equals("T");
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            if (fFunction.StartsWith("Cycle") || fFunction.Contains("Label"))
                Navigation.PopAsync();
            else
                (Parent as Physical.pagePhysCountMain).CurrentPage = (Parent as Physical.pagePhysCountMain).Children[0];
        }
    }
}
