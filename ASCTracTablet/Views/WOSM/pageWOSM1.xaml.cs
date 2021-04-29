using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.WOSM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageWOSM1 : ContentPage
    {
        public pageWOSM1()
        {
            try
            {
                InitializeComponent();
                Title = "Work Order Status Metrics";

                ascUtils.setupPicker(pickProdline1, Globals.lookupProdlineList, string.Empty);
                ascUtils.setupPicker(pickProdline2, Globals.lookupProdlineList, string.Empty);

                pickDateField.Items.Clear();
                pickDateField.Items.Add("Schedule Date");
                pickDateField.Items.Add("Target Date");
                pickDateField.SelectedIndex = Globals.myDatabase.GetConfigIntValue("WOSM_datetype");

                pickDateRange.Items.Clear();
                pickDateRange.Items.Add("All Dates");
                pickDateRange.Items.Add("Today");
                pickDateRange.Items.Add("Yesterday");
                pickDateRange.Items.Add("Through Today");
                pickDateRange.Items.Add("Tomorrow");
                pickDateRange.Items.Add("Today an On");
                pickDateRange.SelectedIndex = Globals.myDatabase.GetConfigIntValue("WOSM_datevalue");

                chbActive.IsToggled = (Globals.myDatabase.GetConfigValue("WOSM_isactive").Equals("T"));
                chbCompleted.IsToggled = (Globals.myDatabase.GetConfigValue("WOSM_iscompleted").Equals("T"));
                chbNotSched.IsToggled = (Globals.myDatabase.GetConfigValue("WOSM_isnotsched").Equals("T"));
                chbPending.IsToggled = (Globals.myDatabase.GetConfigValue("WOSM_ispending").Equals("T"));
                chbPreparing.IsToggled = (Globals.myDatabase.GetConfigValue("WOSM_ispreparing").Equals("T"));
                chbScheduled.IsToggled = (Globals.myDatabase.GetConfigValue("WOSM_isscheded").Equals("T"));

                btnRefresh.IsVisible = true;

                GridHeader();

            }
            catch (Exception ex)
            {
                DisplayAlert("ASCTrac-WOSM", ex.Message, "OK");
            }
        }

        private void GridHeader()
        {
            GridWOStat.Children.Clear();
            //var mylabel = new Label { Text = "Status" };
            //GridWOStat.Children.Add(mylabel, 1, 0);
            var mylabel = new Label { Text = "Description" };
            GridWOStat.Children.Add(mylabel, 1, 0);
            mylabel = new Label { Text = "# WO" };
            GridWOStat.Children.Add(mylabel, 2, 0);
            mylabel = new Label { Text = "Qty to Make" };
            GridWOStat.Children.Add(mylabel, 3, 0);
            mylabel = new Label { Text = "Qty Made" };
            GridWOStat.Children.Add(mylabel, 4, 0);
            mylabel = new Label { Text = "Qty Left" };
            GridWOStat.Children.Add(mylabel, 5, 0);

        }

        private void btnRefresh_Clicked(object sender, EventArgs e)
        {
            string statusList = string.Empty;
            if (chbActive.IsToggled)
                statusList += "A,";
            if (chbCompleted.IsToggled)
                statusList += "F,";
            if (chbNotSched.IsToggled)
                statusList += "N,";
            if (chbPending.IsToggled)
                statusList += "D,";
            if (chbPreparing.IsToggled)
                statusList += "P,";
            if (chbScheduled.IsToggled)
                statusList += "S,";
            if (!String.IsNullOrEmpty(statusList))
                statusList = statusList.Substring(0, statusList.Length - 1);

            string prodlines = string.Empty;
            if (chbProdLine.IsToggled)
            {
                if (pickProdline1.SelectedIndex >= 0)
                {
                    prodlines = ascUtils.getPickerValue(pickProdline1);
                    if (pickProdline2.SelectedIndex >= 0)
                    {
                        string tmp = ascUtils.getPickerValue(pickProdline2);
                        prodlines = prodlines + "|" + tmp;
                    }
                }
            }
            Globals.myDatabase.SaveConfig("WOSM_datetype", pickDateField.SelectedIndex.ToString());
            Globals.myDatabase.SaveConfig("WOSM_datevalue", pickDateRange.SelectedIndex.ToString());

            Globals.myDatabase.SaveBoolConfig("WOSM_isactive", chbActive.IsToggled);
            Globals.myDatabase.SaveBoolConfig("WOSM_iscompleted", chbCompleted.IsToggled);
            Globals.myDatabase.SaveBoolConfig("WOSM_isnotsched", chbNotSched.IsToggled);
            Globals.myDatabase.SaveBoolConfig("WOSM_ispending", chbPending.IsToggled);
            Globals.myDatabase.SaveBoolConfig("WOSM_ispreparing", chbPreparing.IsToggled);
            Globals.myDatabase.SaveBoolConfig("WOSM_isscheded", chbScheduled.IsToggled);

            WOSM.ClassWOSMInput.myWOSMInput.prodlines = prodlines;
            WOSM.ClassWOSMInput.myWOSMInput.statusList = statusList;
            WOSM.ClassWOSMInput.myWOSMInput.dateFieldIndex = pickDateField.SelectedIndex;
            WOSM.ClassWOSMInput.myWOSMInput.dateRangeIndex = pickDateRange.SelectedIndex;
            WOSM.ClassWOSMInput.myWOSMInput.fInit = true;

            GetWOStatusSummary();
        }

        async private void GetWOStatusSummary()
        {
            string errmsg = string.Empty;
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(WOSM.ClassWOSMInput.myWOSMInput.statusList);
                Globals.curBasicMessage.inputDataList.Add(WOSM.ClassWOSMInput.myWOSMInput.prodlines);
                Globals.curBasicMessage.inputDataList.Add(WOSM.ClassWOSMInput.myWOSMInput.dateFieldIndex.ToString());
                Globals.curBasicMessage.inputDataList.Add(WOSM.ClassWOSMInput.myWOSMInput.dateRangeIndex.ToString());
                var myReturnData = await App.myRestManager.GetWOStatusSummary(Globals.curBasicMessage);

                myIndicator.IsRunning = false;
                myIndicator.IsVisible = false;
                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    var myList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.CustOrder.COSMType>>(myReturnData.DataMessage);

                    int i = 1;
                    GridHeader();
                    foreach (var rec in myList)
                    {
                        var myButton = new Button { Text = "Details", Style = (Style)Application.Current.Resources["stdButtonStyle16"] };
                        //myButton.HeightRequest = GridWOStat.RowDefinitions[0].Height.Value;
                        myButton.Clicked += (s, ev) =>
                        {
                            WOSM.ClassWOSMInput.myWOSMInput.statusList = rec.StatusID;
                            SeeWOForStatus();
                        };
                        GridWOStat.Children.Add(myButton, 0, i);

                        //var mylabel = new Label { Text = rec.StatusID };
                        //GridWOStat.Children.Add(mylabel, 1, i);
                        var mylabel = new Label { Text = rec.StatusDesc, Style = (Style)Application.Current.Resources["stdGridLabel"] };
                        GridWOStat.Children.Add(mylabel, 1, i);
                        mylabel = new Label { Text = rec.numOrders.ToString(), Style = (Style)Application.Current.Resources["stdGridLabel"] };
                        GridWOStat.Children.Add(mylabel, 2, i);
                        mylabel = new Label { Text = rec.QtyTotal.ToString(), Style = (Style)Application.Current.Resources["stdGridLabel"] };
                        GridWOStat.Children.Add(mylabel, 3, i);
                        mylabel = new Label { Text = rec.QtyCompleted.ToString(), Style = (Style)Application.Current.Resources["stdGridLabel"] };
                        GridWOStat.Children.Add(mylabel, 4, i);
                        mylabel = new Label { Text = rec.QtyLeft.ToString(), Style = (Style)Application.Current.Resources["stdGridLabel"] };
                        GridWOStat.Children.Add(mylabel, 5, i);
                        i += 1;
                    }

                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            if (!String.IsNullOrEmpty(errmsg))
            {
                await DisplayAlert("Get WO Summary Exception", errmsg, "OK");
            }
        }
        private void SeeWOForStatus()
        {
            WOSM.ClassWOSMInput.myWOSMInput.fInit = true;
            (Parent as WOSM.pageWOSMTab).CurrentPage = (Parent as WOSM.pageWOSMTab).Children[1];
            ((Parent as WOSM.pageWOSMTab).CurrentPage as WOSM.pageWOSMWO).GetWOList();
        }


        private void chbProdLine_Toggled(object sender, ToggledEventArgs e)
        {
            pickProdline1.IsEnabled = chbProdLine.IsToggled;
            pickProdline2.IsEnabled = chbProdLine.IsToggled;
            if (!pickProdline1.IsEnabled)
            {
                pickProdline1.SelectedIndex = -1;
                pickProdline2.SelectedIndex = -1;
            }

        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}