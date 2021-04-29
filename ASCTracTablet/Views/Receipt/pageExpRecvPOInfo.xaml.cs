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
    public partial class pageExpRecvPOInfo : ContentPage
    {
        static string fDefaultPrinter = String.Empty;
        ASCTracFunctionStruct.Receipt.ConfReceiptType myPO;
        public pageExpRecvPOInfo(ASCTracFunctionStruct.Receipt.ConfReceiptType aPO)
        {
            InitializeComponent();
            myPO = aPO;
            SetupPage();
        }

        private void SetupPage()
        {
            lblPONumber.Text = myPO.PONumber + "-" + myPO.ReleaseNum;
            lblVendorID.Text = myPO.VendorID;
            lblVendorName.Text = myPO.VendorName;
            lblRXStatusDescription.Text = myPO.Status_Description;

            ascUtils.setupPicker(pickPrinter, Globals.PrinterBOLList, fDefaultPrinter);
            btnClose.IsEnabled = !(myPO.Status.Equals("C"));
        }

        private void btnTally_Clicked(object sender, EventArgs e)
        {
            string errmsg = string.Empty;
            string myPrinter = string.Empty;
            if (pickPrinter.Items.Count == 0)
                errmsg = "No Printers Available";
            else if (pickPrinter.SelectedIndex < 0)
                errmsg = "No Printer Selected";
            else
            {
                myPrinter = ascUtils.getPickerValue(pickPrinter);
                if (String.IsNullOrEmpty(myPrinter))
                    errmsg = "No Printer Selected";
            }

            if (!String.IsNullOrEmpty(errmsg))
                DisplayAlert("Print Tally Repoort", errmsg, "OK");
            else
            {
                fDefaultPrinter = myPrinter;

                Globals.curBasicMessage.DataMessage = Newtonsoft.Json.JsonConvert.SerializeObject(myPO);
                Globals.curBasicMessage.inputDataList.Clear();
                Globals.curBasicMessage.inputDataList.Add("R");
                Globals.curBasicMessage.inputDataList.Add(myPrinter);


                /*
                ascTracWCFService.iReceiptClient.EndpointConfiguration myEndpoint = new ascTracWCFService.iReceiptClient.EndpointConfiguration();
                var myClient = new ascTracWCFService.iReceiptClient(myEndpoint, pageLogin.hostURL);

                myIndicator.IsRunning = true;
                myIndicator.IsVisible = true;
                myClient.doPrintPOReportCompleted += MyClient_doPrintPOReportCompleted;
                myClient.doPrintPOReportAsync("R", myPrinter, Globals.curBasicMessage);
                */
            }
        }

        /*
        private void MyClient_doPrintPOReportCompleted(object sender, ascTracWCFService.doPrintPOReportCompletedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    Globals.curBasicMessage.DataMessage = string.Empty;
                    myIndicator.IsRunning = false;
                    myIndicator.IsVisible = false;
                    if (!String.IsNullOrEmpty(e.Result))
                    {
                        DisplayAlert("ASCTrac", e.Result, "OK");
                    }
                    else
                        DisplayAlert("ASCTrac", "Tally Report is being sent to the printer", "OK");
                }
                catch (Exception ex)
                {
                    DisplayAlert("ASCTrac", ex.Message, "OK");
                }

            });
        }
        */

        async private void btnClose_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new pageCloseRecvTab("P", myPO.PONumber)); // + "-" + myPO.ReleaseNum));
        }

        async private void BtnCapture_Clicked(object sender, EventArgs e)
        {
            
            try
            {
                await Navigation.PushAsync(new Maintenances.pageImageCapture("R", myPO.PONumber));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Image Capture Exception", ex.Message, "OK");
            }
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}