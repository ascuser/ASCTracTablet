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
    public partial class pageSched : ContentPage
    {
        ASCTracFunctionStruct.CustOrder.DockType myDockRec;
        public pageSched(ASCTracFunctionStruct.CustOrder.DockType aDockRec)
        {
            InitializeComponent();
            myDockRec = aDockRec;

            myDockRec.newDock = myDockRec.Dock;
            myDockRec.newDuration = myDockRec.Duration;
            myDockRec.newPickStatus = myDockRec.PickStatus;
            myDockRec.newSched_Datetime = myDockRec.Sched_Datetime;

            BindingContext = myDockRec;

            ascUtils.setupPicker(pickDock, Globals.dockList, myDockRec.Dock);
            FillCarriers();

            Globals.InitDockStatusList();
            ascUtils.setupPicker(pickStatus, Globals.DockStatusList, myDockRec.PickStatus);

        }

        private void FillCarriers()
        {
            ascUtils.setupPicker(pickCarrier, Globals.lookupCarrierList, myDockRec.CarrierID);
        }


        async private void btnSched_Clicked(object sender, EventArgs e)
        {
            myDockRec.newPickStatus = ascUtils.getPickerValue(pickStatus);
            if ((myDockRec.OrderType.Equals("P") || myDockRec.OrderType.Equals("R")) && myDockRec.newPickStatus.Equals("C"))
            {

                await Navigation.PushAsync(new Views.Receipt.pageCloseRecvTab(myDockRec.OrderType, myDockRec.OrderNumber));
            }
            else
            {
                myIndicator.IsRunning = true;
                myIndicator.IsVisible = true;
                string errmsg = string.Empty;
                try
                {
                    // call schedule Dockschd record
                    //myDockRec.newCarrierID = myDockRec.CarrierID;
                    //myDockRec.newDock = myDockRec.Dock;
                    //myDockRec.newDuration = myDockRec.Duration;
                    //myDockRec.newPickStatus = myDockRec.PickStatus;
                    //myDockRec.newSched_Datetime = myDockRec.Sched_Datetime;

                    myDockRec.newDock = ascUtils.getPickerValue(pickDock);
                    myDockRec.newCarrierID = ascUtils.getPickerValue(pickCarrier);

                    //myDockRec.newSched_Datetime = myDockRec.newSched_Datetime + cbSchedTime.Time;


                    Globals.curBasicMessage.DataMessage = Newtonsoft.Json.JsonConvert.SerializeObject(myDockRec);
                    var myReturnData = await App.myRestManager.doDockSchd(Globals.curBasicMessage);

                    if (myReturnData.successful)
                        await Navigation.PopAsync();
                    else
                        errmsg = myReturnData.ErrorMessage;
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

        private void btnClose_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
