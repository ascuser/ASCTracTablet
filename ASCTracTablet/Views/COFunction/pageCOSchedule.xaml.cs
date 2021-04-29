using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.COFunction
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCOSchedule : ContentPage
    {
        ASCTracFunctionStruct.CustOrder.COSchedInfoType myCOSchedInfo = null; // new ASCTracFunctionStruct.CustOrder.COSchedInfoType();
                                                                              //ascTracWCFService.COSchedInfoType myCOSchedInfo = new ascTracWCFService.COSchedInfoType();

        public pageCOSchedule()
        {
            InitializeComponent();
            string status = "A";
            try
            {
                GetScheduleInfo(CODetail.pageCODetailTab.myCO.OrderNumber);
            }
            catch (Exception ex)
            {
                DisplayAlert("Send Exception at " + status, ex.Message, "OK");
            }
        }

        async private void GetScheduleInfo(string aOrderNumber)
        {
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;

            string errmsg = string.Empty;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(aOrderNumber);
                var myReturnData = await App.myRestManager.GetScheduleInfo(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    myCOSchedInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<ASCTracFunctionStruct.CustOrder.COSchedInfoType>(myReturnData.DataMessage);

                    SetupOrder();
                }
            }
            catch (Exception ex)
            {
                errmsg = "Exception\r\n" + ex.Message;
            }
            myIndicator.IsRunning = false;
            myIndicator.IsVisible = false;
            if (!String.IsNullOrEmpty(errmsg))
                await DisplayAlert("Exception", errmsg, "OK");
        }

        private void SetupOrder()
        {
            BindingContext = myCOSchedInfo;

            lblCustInfo.Text = CODetail.pageCODetailTab.myCO.CustIDAndName;
            if (CODetail.pageCODetailTab.myCO.requiredShipDate != DateTime.MinValue)
                lblRequiredShipDate.Text = CODetail.pageCODetailTab.myCO.requiredShipDate.Date.ToString();
            else
                lblRequiredShipDate.Text = "N/A";
            Globals.InitPickToList();
            ascUtils.setupPicker(pckPickStatus, Globals.pickStatusList, myCOSchedInfo.PickStatus.OrigValue);
            ascUtils.setupPicker(pckSchedPicker, Globals.UserList, myCOSchedInfo.SchedPickerID.OrigValue);
            //setupPicker2(pckPickTo, Globals.PicktoList, myCOSchedInfo.TruckAvail);
            ascUtils.setupPicker(pckPickTo, Globals.PicktoList, myCOSchedInfo.TruckAvail.OrigValue);
            SetPickToLoc(myCOSchedInfo.TruckAvail.OrigValue);
            ascUtils.setupPicker(pckDock, Globals.dockList, myCOSchedInfo.Dock.OrigValue);

        }
        private void SetPickToLoc(string aPickToValue)
        {
            if (aPickToValue.Equals("T"))
            {
                ascUtils.setupPicker(pckPickToLocID, Globals.TruckLocList, myCOSchedInfo.PickToLocID.newValue);
            }
            else
                ascUtils.setupPicker(pckPickToLocID, Globals.LoadLocList, myCOSchedInfo.PickToLocID.newValue);
            edTrailerID.IsEnabled = !aPickToValue.Equals("F");
        }

        async private void btnSchedule_Clicked(object sender, EventArgs e)
        {
            string status = "A";
            string errmsg = string.Empty;
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            try
            {
                myCOSchedInfo.PickStatus.newValue = ascUtils.getPickerValue(pckPickStatus);
                status = "b";
                myCOSchedInfo.SchedPickerID.newValue = ascUtils.getPickerValue(pckSchedPicker);

                //var myrec2 = (DataModel.dropDownItemType)pckPickTo.SelectedItem;
                //myCOSchedInfo.TruckAvail = myrec2.ID;
                status = "c";
                myCOSchedInfo.TruckAvail.newValue = ascUtils.getPickerValue(pckPickTo);

                status = "d";
                if (pckPickToLocID.IsEnabled)
                    myCOSchedInfo.PickToLocID.newValue = ascUtils.getPickerValue(pckPickToLocID);
                status = "e";
                myCOSchedInfo.Dock.newValue = ascUtils.getPickerValue(pckDock);

                status = "f";


                Globals.curBasicMessage.hdrDataMessage = Newtonsoft.Json.JsonConvert.SerializeObject(CODetail.pageCODetailTab.myCO);
                status = "f2";

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ContractResolver = new DataModel.JsonExceptionResolver();

                Globals.curBasicMessage.DataMessage = Newtonsoft.Json.JsonConvert.SerializeObject(myCOSchedInfo, settings);
                status = "f3";


                var myReturnData = await App.myRestManager.doScheduleInfo(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    myIndicator.IsRunning = false;
                    myIndicator.IsVisible = false;
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                errmsg = "Exception\r\n" + ex.Message;
            }
            myIndicator.IsRunning = false;
            myIndicator.IsVisible = false;
            if (!String.IsNullOrEmpty(errmsg))
                await DisplayAlert("Exception", errmsg, "OK");
        }

        private void pckPickTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPickToLoc(ascUtils.getPickerValue(pckPickTo));
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}