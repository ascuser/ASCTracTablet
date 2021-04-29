using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.Production
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageSchedWO : ContentPage
    {
        ASCTracFunctionStruct.Production.WOHdrType fWORec;
        public pageSchedWO(ASCTracFunctionStruct.Production.WOHdrType aWORec, string aProdLine)
        {
            fWORec = aWORec;
            InitializeComponent();
            Title = "Scheduling";
            BindingContext = fWORec;

            edSchedDate.Date = fWORec.Sched_Datetime.Date;
            edSchedTime.Time = fWORec.Sched_Datetime.TimeOfDay;

            if (String.IsNullOrEmpty(fWORec.Prodline))
                FillProdlines(aProdLine);
            else
                FillProdlines(fWORec.Prodline);

            Globals.InitWOStatusList();
            ascUtils.setupPicker(pickStatus, Globals.WOStatusList, fWORec.Status);
        }

        private void FillProdlines(string adefault)
        {
            ascUtils.setupPicker(pickProdline, Globals.lookupProdlineList, adefault);
        }


        async public void onSchedule(object sender, EventArgs e)
        {
            if (await DisplayAlert("Schedule Production", "Update Work Order " + fWORec.Workorder_ID, "Yes", "No"))
            {
                fWORec.Status = ascUtils.getPickerValue(pickStatus);
                fWORec.Sched_Datetime = edSchedDate.Date.Add(edSchedTime.Time);
                if (pickProdline.SelectedIndex >= 0)
                    fWORec.Prodline = ascUtils.getPickerValue(pickProdline);

                Globals.curBasicMessage.DataMessage = Newtonsoft.Json.JsonConvert.SerializeObject(fWORec);

                myIndicator.IsRunning = true;
                myIndicator.IsVisible = true;
                string errmsg = string.Empty;
                try
                {
                    var myReturnData = await App.myRestManager.ScheduleWO(Globals.curBasicMessage);

                    myIndicator.IsRunning = false;
                    myIndicator.IsVisible = false;
                    if (!myReturnData.successful)
                        errmsg = myReturnData.ErrorMessage;
                    else
                    {
                        if (fWORec.Status == "A")
                        {
                            (Parent as pageWOTab).startProd();
                            (Parent as pageWOTab).CurrentPage = (Parent as pageWOTab).Children[1];
                        }
                        else
                        {
                            await Navigation.PopAsync();
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
                    await DisplayAlert("Schedule WO Exception", errmsg, "OK");
            }
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}

