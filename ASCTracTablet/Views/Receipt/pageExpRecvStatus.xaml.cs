using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.Receipt
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageExpRecvStatus : ContentPage
    {
        public pageExpRecvStatus()
        {
            InitializeComponent();
            try
            {
                Title = "Summary Metrics";

                pickDateField.Items.Clear();
                pickDateField.Items.Add("Expected Date (Header)");
                pickDateField.Items.Add("Expected Date (Detail)");
                pickDateField.SelectedIndex = Globals.myDatabase.GetConfigIntValue("EROSM_datetype");

                pickDateRange.Items.Clear();
                pickDateRange.Items.Add("All Dates");
                pickDateRange.Items.Add("Today");
                pickDateRange.Items.Add("Yesterday");
                pickDateRange.Items.Add("Through Today");
                pickDateRange.Items.Add("Tomorrow");
                pickDateRange.Items.Add("Today an On");
                pickDateRange.SelectedIndex = Globals.myDatabase.GetConfigIntValue("EROSM_datevalue");

                chbOpen.IsToggled = (Globals.myDatabase.GetConfigValue("EROSM_isopen").Equals("T"));
                chbReceived.IsToggled = (Globals.myDatabase.GetConfigValue("EROSM_isreceived").Equals("T"));
                btnRefresh.IsVisible = true;

                GridHeader();

            }
            catch (Exception ex)
            {
                DisplayAlert("ASCTrac-Recv Metrics", ex.Message, "OK");
            }
        }


        private void GridHeader()
        {
            GridPOStat.Children.Clear();
            //var mylabel = new Label { Text = "Status" };
            //GridPOStat.Children.Add(mylabel, 1, 0);
            var mylabel = new Label { Text = "Description", TextColor = Color.Black };
            GridPOStat.Children.Add(mylabel, 1, 0);
            mylabel = new Label { Text = "# PO", TextColor = Color.Black };
            GridPOStat.Children.Add(mylabel, 2, 0);
            mylabel = new Label { Text = "Qty to Receive", TextColor = Color.Black };
            GridPOStat.Children.Add(mylabel, 3, 0);
            mylabel = new Label { Text = "Qty Received", TextColor = Color.Black };
            GridPOStat.Children.Add(mylabel, 4, 0);
            mylabel = new Label { Text = "Qty Left", TextColor = Color.Black };
            GridPOStat.Children.Add(mylabel, 5, 0);
        }

        async private void btnRefresh_Clicked(object sender, EventArgs e)
        {
            string statusList = string.Empty;
            if (chbOpen.IsToggled && chbReceived.IsToggled)
                statusList += "";
            if (chbOpen.IsToggled)
                statusList += "O";
            if (chbReceived.IsToggled)
                statusList += "R";

            Globals.myDatabase.SaveConfig("EROSM_datetype", pickDateField.SelectedIndex.ToString());
            Globals.myDatabase.SaveConfig("EROSM_datevalue", pickDateRange.SelectedIndex.ToString());

            Globals.myDatabase.SaveBoolConfig("EROSM_isopen", chbOpen.IsToggled);
            Globals.myDatabase.SaveBoolConfig("EROSM_isreceived", chbReceived.IsToggled);


            ClassExpRecvInput.myExpRecvStatusInput.statusList = statusList;
            ClassExpRecvInput.myExpRecvStatusInput.dateFieldIndex = pickDateField.SelectedIndex;
            ClassExpRecvInput.myExpRecvStatusInput.dateRangeIndex = pickDateRange.SelectedIndex;
            ClassExpRecvInput.myExpRecvStatusInput.fInit = true;

            myIndicator.IsEnabled = true;
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;

            try
            {
                Globals.curBasicMessage.inputDataList.Clear();
                Globals.curBasicMessage.inputDataList.Add(ClassExpRecvInput.myExpRecvStatusInput.statusList);
                Globals.curBasicMessage.inputDataList.Add(ClassExpRecvInput.myExpRecvStatusInput.dateFieldIndex.ToString());
                Globals.curBasicMessage.inputDataList.Add(ClassExpRecvInput.myExpRecvStatusInput.dateRangeIndex.ToString());
                var myReturnData = await App.myRestManager.GetPOStatusSummary(Globals.curBasicMessage);
                myIndicator.IsEnabled = false;
                myIndicator.IsRunning = false;
                myIndicator.IsVisible = false;

                if (myReturnData.successful)
                {
                    processGetPOStatusSummary(myReturnData.DataMessage);

                }
                else
                    await DisplayAlert("ASCTrac-Recv Metrics", "Error :" + myReturnData.ErrorMessage, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("ASCTrac-Recv Metrics", "Exception :" + ex.Message, "OK");
            }
            myIndicator.IsEnabled = false;
            myIndicator.IsRunning = false;
            myIndicator.IsVisible = false;
        }

        async private void processGetPOStatusSummary(string aDataMessage)
        {
            try
            {
                var myList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.CustOrder.COSMType>>(aDataMessage);

                int i = 1;
                GridHeader();
                foreach (var rec in myList)
                {
                    var myButton = new Button { Text = "Details", Style = (Style)Application.Current.Resources["stdButtonStyle16"] };
                    //myButton.HeightRequest = GridPOStat.RowDefinitions[0].Height.Value;
                    myButton.Clicked += (s, ev) =>
                    {
                        Receipt.ClassExpRecvInput.myExpRecvStatusInput.statusList = rec.StatusID;
                        SeePOForStatus();
                    };
                    GridPOStat.Children.Add(myButton, 0, i);

                    //var mylabel = new Label { Text = rec.StatusID };
                    //GridPOStat.Children.Add(mylabel, 1, i);
                    var mylabel = new Label { Text = rec.StatusDesc, Style = (Style)Application.Current.Resources["stdGridLabel"] };
                    GridPOStat.Children.Add(mylabel, 1, i);
                    mylabel = new Label { Text = rec.numOrders.ToString(), Style = (Style)Application.Current.Resources["stdGridLabel"] };
                    GridPOStat.Children.Add(mylabel, 2, i);
                    mylabel = new Label { Text = rec.QtyTotal.ToString("F4"), Style = (Style)Application.Current.Resources["stdGridLabel"] };
                    GridPOStat.Children.Add(mylabel, 3, i);
                    mylabel = new Label { Text = rec.QtyCompleted.ToString("F4"), Style = (Style)Application.Current.Resources["stdGridLabel"] };
                    GridPOStat.Children.Add(mylabel, 4, i);
                    mylabel = new Label { Text = rec.QtyLeft.ToString("F4"), Style = (Style)Application.Current.Resources["stdGridLabel"] };
                    GridPOStat.Children.Add(mylabel, 5, i);
                    i += 1;

                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("ASCTrac-Recv Metrics", "Processing :" + ex.Message, "OK");
            }
        }

        private void SeePOForStatus()
        {
            ClassExpRecvInput.myExpRecvStatusInput.fInit = true;
            (Parent as pageExpRecvStatusTab).CurrentPage = (Parent as pageExpRecvStatusTab).Children[1];
            ((Parent as pageExpRecvStatusTab).CurrentPage as pageExpRecvStatusPOList).GetPOList();
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}