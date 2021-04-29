using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.COContainer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCOToteInfo : ContentPage
    {
        ASCTracFunctionStruct.CustOrder.OrderContainerLookupInfo myRec;
        private static string fDefaultPrinter = string.Empty;
        private static string fDefaultLBLPrinter = string.Empty;

        public pageCOToteInfo(ASCTracFunctionStruct.CustOrder.OrderContainerLookupInfo aRec)
        {
            InitializeComponent();
            myRec = aRec;
            BindingContext = myRec;

            ascUtils.setupPicker(pickPrinter, Globals.PrinterBOLList, fDefaultPrinter);
            ascUtils.setupPicker(pickLBLPrinter, Globals.PrinterLBLList, fDefaultLBLPrinter);
        }

        async private void doPrint( string aType, string aPrinterID)
        {
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;

            string errmsg = string.Empty;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(myRec.OrderNumber);
                Globals.curBasicMessage.inputDataList.Add(myRec.ContainerID);
                Globals.curBasicMessage.inputDataList.Add("T"); // rec type
                Globals.curBasicMessage.inputDataList.Add(aType); // label / report type
                Globals.curBasicMessage.inputDataList.Add(aPrinterID);


                var myReturnData = await App.myRestManager.PrintOrderContainer(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    await DisplayAlert("ASCTrac", myReturnData.DataMessage, "OK");
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

        async private void btnToteList_Clicked(object sender, EventArgs e)
        {
            string myPrinter = string.Empty;
            string errmsg = string.Empty;
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
                await DisplayAlert("Print Packlist Repoort", errmsg, "OK");
            else
            {
                fDefaultPrinter = myPrinter;


                if (await DisplayAlert("Tote " + myRec.ContainerID, "Print Tote Packlist?", "Yes", "No"))
                {
                    doPrint("P", myPrinter);
                }
            }
        }


        async private void btnLabel_Clicked(object sender, EventArgs e)
        {
            string myPrinter = string.Empty;
            string errmsg = string.Empty;
            if (pickLBLPrinter.Items.Count == 0)
                errmsg = "No Printers Available";
            else if (pickLBLPrinter.SelectedIndex < 0)
                errmsg = "No Printer Selected";
            else
            {
                myPrinter = ascUtils.getPickerValue(pickLBLPrinter);
                if (String.IsNullOrEmpty(myPrinter))
                    errmsg = "No Printer Selected";
            }

            if (!String.IsNullOrEmpty(errmsg))
                await DisplayAlert("Reprint Tote Label", errmsg, "OK");
            else
            {
                fDefaultLBLPrinter = myPrinter;
                if (await DisplayAlert("Tote " + myRec.ContainerID, "Reprint Tote Label?", "Yes", "No"))
                {
                    doPrint("L", myPrinter);

                }
            }
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
