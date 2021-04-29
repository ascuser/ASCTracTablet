using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.COSM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCOSM1 : ContentPage
    {
        public pageCOSM1()
        {
            InitializeComponent();
            try
            {
                Title = "Summary";

                ascUtils.setupPicker(pickDock1, Globals.dockList, "");
                ascUtils.setupPicker(pickDock2, Globals.dockList, "");

                pickDateField.Items.Clear();
                pickDateField.Items.Add("Schedule Date");
                pickDateField.Items.Add("Required Ship Date");
                pickDateField.Items.Add("Scheduled Complete Date");
                pickDateField.SelectedIndex = Globals.myDatabase.GetConfigIntValue("COSM_datetype");

                pickDateRange.Items.Clear();
                pickDateRange.Items.Add("All Dates");
                pickDateRange.Items.Add("Today");
                pickDateRange.Items.Add("Yesterday");
                pickDateRange.Items.Add("Through Today");
                pickDateRange.Items.Add("Through Yesterday");
                pickDateRange.Items.Add("Tomorrow");
                pickDateRange.Items.Add("Today and On");
                pickDateRange.Items.Add("Tomorrow and On");
                pickDateRange.SelectedIndex = Globals.myDatabase.GetConfigIntValue("COSM_datevalue");

                chbPicking.IsToggled = (Globals.myDatabase.GetConfigValue("COSM_ispicking").Equals("T"));
                chbCompleted.IsToggled = (Globals.myDatabase.GetConfigValue("COSM_iscompleted").Equals("T"));
                chbNotSched.IsToggled = (Globals.myDatabase.GetConfigValue("COSM_isnotsched").Equals("T"));
                chbLoading.IsToggled = (Globals.myDatabase.GetConfigValue("COSM_isloading").Equals("T"));
                chbNoInv.IsToggled = (Globals.myDatabase.GetConfigValue("COSM_isnoinv").Equals("T"));
                chbScheduled.IsToggled = (Globals.myDatabase.GetConfigValue("COSM_isscheded").Equals("T"));

                //btnRefresh.IsVisible = true;
                //imageRefresh.IsVisible = true;

                GridHeader();

            }
            catch (Exception ex)
            {
                DisplayAlert(Globals.AppTitleName, ex.Message, "OK");
            }
        }


        private void GridHeader()
        {
            GridCOStat.Children.Clear();
            var mylabel = new Label { Text = "Status", Style = (Style)Application.Current.Resources["stdGridHeaderLabel"] };
            GridCOStat.Children.Add(mylabel, 1, 0);
            mylabel = new Label { Text = "Description", Style = (Style)Application.Current.Resources["stdGridHeaderLabel"] };
            GridCOStat.Children.Add(mylabel, 2, 0);
            mylabel = new Label { Text = "# CO", Style = (Style)Application.Current.Resources["stdGridHeaderLabel"] };
            GridCOStat.Children.Add(mylabel, 3, 0);
            mylabel = new Label { Text = "Qty Ordered", Style = (Style)Application.Current.Resources["stdGridHeaderLabel"] };
            GridCOStat.Children.Add(mylabel, 4, 0);
            mylabel = new Label { Text = "Qty Picked", Style = (Style)Application.Current.Resources["stdGridHeaderLabel"] };
            GridCOStat.Children.Add(mylabel, 5, 0);
            mylabel = new Label { Text = "Qty Loaded", Style = (Style)Application.Current.Resources["stdGridHeaderLabel"] };
            GridCOStat.Children.Add(mylabel, 6, 0);
        }

        async private void btnRefresh_Clicked(object sender, EventArgs e)
        {
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            myIndicator.IsEnabled = true;
            try
            {

                string statusList = string.Empty;
                if (chbPicking.IsToggled)
                    statusList += "B,P,";
                if (chbCompleted.IsToggled)
                    statusList += "Q, G, C,";
                if (chbNotSched.IsToggled)
                    statusList += "N,U,";
                if (chbLoading.IsToggled)
                    statusList += "D,L,M,";
                if (chbNoInv.IsToggled)
                    statusList += "I,";
                if (chbScheduled.IsToggled)
                    statusList += "S,";
                if (!String.IsNullOrEmpty(statusList))
                    statusList = statusList.Substring(0, statusList.Length - 1);

                string docks = string.Empty;
                if (chbDocks.IsToggled)
                {
                    docks = ascUtils.getPickerValue(pickDock1);
                    if (!string.IsNullOrEmpty(docks))
                    {
                        var tmp = ascUtils.getPickerValue(pickDock2);
                        if (!String.IsNullOrEmpty(tmp))
                            docks += "," + tmp;
                    }
                }
                Globals.myDatabase.SaveConfig("COSM_datetype", pickDateField.SelectedIndex.ToString());
                Globals.myDatabase.SaveConfig("COSM_datevalue", pickDateRange.SelectedIndex.ToString());

                Globals.myDatabase.SaveBoolConfig("COSM_ispicking", chbPicking.IsToggled);
                Globals.myDatabase.SaveBoolConfig("COSM_iscompleted", chbCompleted.IsToggled);
                Globals.myDatabase.SaveBoolConfig("COSM_isnotsched", chbNotSched.IsToggled);
                Globals.myDatabase.SaveBoolConfig("COSM_isloading", chbLoading.IsToggled);
                Globals.myDatabase.SaveBoolConfig("COSM_isnoinv", chbNoInv.IsToggled);
                Globals.myDatabase.SaveBoolConfig("COSM_isscheded", chbScheduled.IsToggled);

                COSM.ClassCOSMInput.myCOSMInput.docks = docks;
                COSM.ClassCOSMInput.myCOSMInput.statusList = statusList;
                COSM.ClassCOSMInput.myCOSMInput.dateFieldIndex = pickDateField.SelectedIndex;
                COSM.ClassCOSMInput.myCOSMInput.dateRangeIndex = pickDateRange.SelectedIndex;
                COSM.ClassCOSMInput.myCOSMInput.fInit = true;


                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(statusList);
                Globals.curBasicMessage.inputDataList.Add(docks);
                Globals.curBasicMessage.inputDataList.Add(pickDateField.SelectedIndex.ToString());
                Globals.curBasicMessage.inputDataList.Add(pickDateRange.SelectedIndex.ToString());
                var myReturnData = await App.myRestManager.GetCOStatusSummary(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    await DisplayAlert(Globals.AppTitleName, myReturnData.ErrorMessage, "OK");
                else
                {
                    var myList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.CustOrder.COSMType>>(myReturnData.DataMessage);


                    int i = 1;
                    GridHeader();
                    foreach (var rec in myList)
                    {
                        var myButton = new Button { Text = "Details", Style = (Style)Application.Current.Resources["stdButtonStyle16"] };
                        //myButton.HeightRequest = GridCOStat.RowDefinitions[0].Height.Value;
                        myButton.Clicked += (s, ev) =>
                        {
                            COSM.ClassCOSMInput.myCOSMInput.statusList = rec.StatusID;
                            SeeCOForStatus();
                        };
                        GridCOStat.Children.Add(myButton, 0, i);
                        var mylabel = new Label { Text = rec.StatusID, Style = (Style)Application.Current.Resources["stdGridLabel"] };
                        GridCOStat.Children.Add(mylabel, 1, i);
                        mylabel = new Label { Text = rec.StatusDesc, Style = (Style)Application.Current.Resources["stdGridLabel"] };
                        GridCOStat.Children.Add(mylabel, 2, i);
                        mylabel = new Label { Text = rec.numOrders.ToString(), Style = (Style)Application.Current.Resources["stdGridLabel"] };
                        GridCOStat.Children.Add(mylabel, 3, i);
                        mylabel = new Label { Text = rec.QtyTotal.ToString(), Style = (Style)Application.Current.Resources["stdGridLabel"] };
                        GridCOStat.Children.Add(mylabel, 4, i);
                        mylabel = new Label { Text = rec.QtyCompleted.ToString(), Style = (Style)Application.Current.Resources["stdGridLabel"] };
                        GridCOStat.Children.Add(mylabel, 5, i);
                        mylabel = new Label { Text = rec.QtyLeft.ToString(), Style = (Style)Application.Current.Resources["stdGridLabel"] };
                        GridCOStat.Children.Add(mylabel, 6, i);
                        i += 1;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(Globals.AppTitleName, "Exception :" + ex.Message, "OK");
            }
            myIndicator.IsRunning = false;

        }

        private void SeeCOForStatus()
        {
            COSM.ClassCOSMInput.myCOSMInput.fInit = true;
            ((Parent as COSM.pageCOSMTab).Children[1] as COSM.pageCOSMByCO).fRefreshFlag = true;
            ((Parent as COSM.pageCOSMTab).Children[1] as COSM.pageCOSMByCO).GetCOList();
            (Parent as COSM.pageCOSMTab).CurrentPage = (Parent as COSM.pageCOSMTab).Children[1];
        }

        private void chbDocks_Toggled(object sender, ToggledEventArgs e)
        {
            pickDock1.IsEnabled = chbDocks.IsToggled;
            pickDock2.IsEnabled = chbDocks.IsToggled;
            if (!pickDock1.IsEnabled)
            {
                pickDock1.SelectedIndex = -1;
                pickDock2.SelectedIndex = -1;
            }
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}

