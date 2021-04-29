using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.CODetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCODetailList : ContentPage
    {
        public pageCODetailList()
        {
            InitializeComponent();
            Title = "List";
            lblOrderNumber.Text = pageCODetailTab.myOrderNumber;
            if (pageCODetailTab.myCO == null)
            {
                //   RefreshData();
            }
            else
                displayCO();
            //DisplayList();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //RefreshData();
        }

        private void DisplayList()
        {
            var myList = Globals.myDatabase.GetOrdrDets().ToList<DataModel.dataOrdrDet>();
            listOrdrDets.ItemsSource = myList;
            if (myList.Count > 0)
                pageCODetailTab.myOrdrDet = myList[0];
            else
                pageCODetailTab.myOrdrDet = null;
        }

        private void displayCO()
        {
            //DisplayAlert("Debug", pageCODetailTab.myCO.PickStatus + "," + pageCODetailTab.myCO.PickStatus_Description, "ok");
            btnShip.IsEnabled = pageCODetailTab.myCO.PickStatus.Equals("G");
            btnOE.IsEnabled = pageCODetailTab.myCO.PickStatus.Equals("N");

            BindingContext = pageCODetailTab.myCO;

            if (pageCODetailTab.myCO.requiredShipDate == DateTime.MinValue)
                lblRequiredShipDate.IsVisible = false;

            string xmlResult = pageCODetailTab.myCO.DetailXMLInfo;
            if (xmlResult.StartsWith("ER") || xmlResult.StartsWith("EX"))
                DisplayAlert("ASCTrac Return Detail", xmlResult, "OK");
            else if (xmlResult.StartsWith("OK"))
            {

               Globals.myDatabase.fillOrdrDet(xmlResult.Substring(2));
                DisplayList();
            }
        }

        async public void RefreshData()
        {
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            string errmsg = string.Empty;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(pageCODetailTab.myOrderNumber);

                var myReturnData = await App.myRestManager.GetCOInfo(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    pageCODetailTab.myCO = Newtonsoft.Json.JsonConvert.DeserializeObject<ASCTracFunctionStruct.CustOrder.CustOrderInfoType>(myReturnData.DataMessage);
                    displayCO();
                }
            }
            catch (Exception ex)
            {
                errmsg = "Exception\r\n" + ex.Message;
            }
            myIndicator.IsRunning = false;
            myIndicator.IsVisible = false;
            if (!String.IsNullOrEmpty(errmsg))
                await DisplayAlert("Refresh Data", errmsg, "OK");
        }

        async private void btnRecalc_Clicked(object sender, EventArgs e)
        {
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            string errmsg = string.Empty;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add("C");
                Globals.curBasicMessage.inputDataList.Add(pageCODetailTab.myOrderNumber);
                Globals.curBasicMessage.inputDataList.Add(""); // linenum
                
                var myReturnData = await App.myRestManager.CalcPCE(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                        RefreshData();
                        await DisplayAlert(Globals.AppTitleName, "Operation Completed.", "OK");
                }
               
            }
            catch (Exception ex)
            {
                errmsg = "Exception\r\n" + ex.Message;
            }
            myIndicator.IsRunning = false;
            myIndicator.IsVisible = false;
            if (!String.IsNullOrEmpty(errmsg))
                await DisplayAlert("Recalc PCE", errmsg, "OK");
        }

        private void listOrdrDets_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            pageCODetailTab.myOrdrDet = (DataModel.dataOrdrDet)listOrdrDets.SelectedItem;
            (Parent as pageCODetailTab).CurrentPage = (Parent as pageCODetailTab).Children[1];
        }

        async private void btnRecalcPicks_Clicked(object sender, EventArgs e)
        {
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            string errmsg = string.Empty;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(pageCODetailTab.myOrderNumber);
                Globals.curBasicMessage.inputDataList.Add("-1");
                Globals.curBasicMessage.inputDataList.Add(""); // PCE Type
                Globals.curBasicMessage.inputDataList.Add(""); // new status
                Globals.curBasicMessage.inputDataList.Add("T"); // clear pick
                var myReturnData = await App.myRestManager.UpdateOrdrDet(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                        RefreshData();
                        await DisplayAlert(Globals.AppTitleName, "Operation Completed.", "OK");
                }
            }
            catch (Exception ex)
            {
                errmsg = "Exception\r\n" + ex.Message;
            }
            myIndicator.IsRunning = false;
            myIndicator.IsVisible = false;
            if (!String.IsNullOrEmpty(errmsg))
                await DisplayAlert("Recalc Picks", errmsg, "OK");
        }


        private void onCloseDetail(object sender, EventArgs e)
        {
            DataModel.dataOrdrDet record = null;
            if (sender is SwipeItem)
            {
                SwipeItem item = sender as SwipeItem;
                record = item.BindingContext as DataModel.dataOrdrDet;
            }
            else
            {
                var mi = ((MenuItem)sender);
                record = (DataModel.dataOrdrDet)mi.BindingContext;
            }
            if (record != null)
                DisplayAlert(Globals.AppTitleName, "Close Detail " + record.LineNumber.ToString(), "OK");
        }

        private void btnSchedule_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new COFunction.pageCOSchedule());
        }

        private void btnShip_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new COFunction.pageCOConfirmShip("C", pageCODetailTab.myCO.OrderNumber));
        }

        private void btnOE_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new COEntry.pageCOEntry(  pageCODetailTab.myOrderNumber));
        }

        private void BtnCapture_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Maintenances.pageImageCapture("O", pageCODetailTab.myCO.OrderNumber));
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
