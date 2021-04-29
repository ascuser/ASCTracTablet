using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.DockSchd
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageNewDockSchd : ContentPage
    {
        string fDock;
        DateTime fDate;
        bool fSchedluing = false;
        public pageNewDockSchd(string aDock, DateTime aDate)
        {
            InitializeComponent();

            fDock = aDock;
            fDate = aDate;

            Globals.InitOrderTypeList();
            ascUtils.setupPicker(cbOrderType, Globals.OrderTypeList, "C");

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (fSchedluing)
                Navigation.PopAsync();
        }

        async private void btnSched_Clicked(object sender, EventArgs e)
        {
            if ((cbOrderType.SelectedIndex >= 0) && !String.IsNullOrEmpty(edOrderNumber.Text))
            {
                var ordertype = ascUtils.getPickerValue(cbOrderType);


                myIndicator.IsRunning = false;
                myIndicator.IsVisible = false;
                string errmsg = string.Empty;
                try
                {
                    Globals.curBasicMessage.DataMessage = ordertype + "|" + edOrderNumber.Text + "|" + fDock + "|" + fDate.ToString();
                    var myReturnData = await App.myRestManager.doNewDockSchd(Globals.curBasicMessage);

                    if (!myReturnData.successful)
                        errmsg = myReturnData.ErrorMessage;
                    else
                    {
                        var myRec = Newtonsoft.Json.JsonConvert.DeserializeObject<ASCTracFunctionStruct.CustOrder.DockType>(myReturnData.DataMessage);

                        bool fok = true;
                        if (!String.IsNullOrEmpty(myRec.ReturnMessage))
                        {
                            fok = await DisplayAlert("Schedule New Order", myRec.ReturnMessage + "\r\nContinue", "Yes", "No");
                        }
                        if (fok)
                        {
                            fSchedluing = true;
                            if (myRec.OrderType.Equals("C"))
                            {
                                //await Navigation.PushAsync(new COSM.pageCODetailTab(myRec.OrderNumber));
                            }
                            else //if (rec.OrderType.Equals("P") || rec.OrderType.Equals("R"))
                                await Navigation.PushAsync(new pageSched(myRec));
                        }
                    }

                }
                catch (Exception ex)
                {
                    errmsg = "Exception :" + ex.Message;
                }
                myIndicator.IsRunning = false;
                myIndicator.IsVisible = false;
                if (!string.IsNullOrEmpty(errmsg))
                    await DisplayAlert("Get Dock Events", errmsg, "OK");
            }
        }

        async private void btnClose_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void cbOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string oTypeDesc = string.Empty;
            string oType = ascUtils.getPickerValue(cbOrderType, ref oTypeDesc);

            if (oType.Equals("V"))
                lblOrderNumber.Text = "CustomerID:";
            else if (oType.Equals("U"))
                lblOrderNumber.Text = "Trailer:";
            else if (oType.Equals("B"))
                lblOrderNumber.Text = "Reason:";
            else
                lblOrderNumber.Text = oTypeDesc; // cbOrderType.SelectedItem..SelectedItem.ToString();
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
