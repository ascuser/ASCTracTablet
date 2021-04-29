using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ASCTracTablet
{
    public partial class MainPage : ContentPage
    {
        public MainPage(Data.ASCTracDB aASCTracDB)
        {
            InitializeComponent();
            Globals.myDatabase = aASCTracDB;
            Globals.ReadSettings();
            lblHHID.Text = "HHCID: Tablet " + Globals.HHCID;
            Title = "ASCTrac Tablet";

            btnLogon.Clicked += onLogon;
            mnuAbout.Clicked += onAbout;
            mnuSetup.Clicked += onSetup;
        }

        async public void onLogon(object sender, EventArgs e)
        {
            btnLogon.IsEnabled = false;
            //ImageLogin.IsVisible = false;
            //ImageLogin.IsEnabled = false;
            //string msg = "Login " + edUserID.Text + ", with " + edPassword.Text;
            //string myurl = "http://asc-rds02.asc.local/ascTracWCFService.svc";

            var mydata = new ASCTracFunctionStruct.inputType(); // ascTracWCFService.ascBasicInboundMessageType();
            mydata.UserID = edUserID.Text;

            mydata.inputDataList.Add( edPassword.Text);
            mydata.HHID = Globals.HHCID;
            mydata.UnitID = Globals.UnitID;
            mydata.ConnectionID = Globals.ConnectionID;

            //mydatabase.SaveConfig("UnitID", Globals.UnitID);

            myIndicator.IsEnabled = true;
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;

            try
            {
                var mySignonData = await App.myRestManager.Signon(mydata);

                //if (String.IsNullOrEmpty(mySignonData.ReturnMessage))
                if( mySignonData.ReturnMessage.StartsWith( "OK"))
                {
                    Globals.ConnectionID = mySignonData.ConnectionID;
                    Globals.curUserID = edUserID.Text.ToUpper();
                    Globals.curSiteID = mySignonData.SiteID;

                    Globals.myDatabase.fillReasonCodes(mySignonData.lookupReasonCodeList);
                    Globals.myDatabase.fillMenuList(mySignonData);


                    Globals.pickStatusList = mySignonData.pickStatusList.ToList<ASCTracFunctionStruct.LookupItemType>();
                    Globals.dockList = mySignonData.lookupDockList.ToList<ASCTracFunctionStruct.LookupItemType>();
                    Globals.LoadLocList = mySignonData.lookupLoadLocList.ToList<ASCTracFunctionStruct.LookupItemType>();
                    Globals.TruckLocList = mySignonData.lookupTruckLocList.ToList<ASCTracFunctionStruct.LookupItemType>();
                    Globals.UserList = mySignonData.lookupUserList.ToList<ASCTracFunctionStruct.LookupItemType>();
                    Globals.PrinterLBLList = mySignonData.lookupLblPrinterList.ToList<ASCTracFunctionStruct.LookupItemType>();
                    Globals.PrinterBOLList = mySignonData.lookupBOLPrinterList.ToList<ASCTracFunctionStruct.LookupItemType>();
                    Globals.imageCaptureDocTypeList = mySignonData.lookupDocumentCaptureTypeList.ToList<ASCTracFunctionStruct.LookupItemType>();

                    Globals.lookupProdlineList = mySignonData.lookupProdlineList.ToList<ASCTracFunctionStruct.LookupItemType>();
                    Globals.lookupCarrierList = mySignonData.lookupCarrierList.ToList<ASCTracFunctionStruct.LookupItemType>();
                    Globals.lookupShipVIAList = mySignonData.lookupShipVIAList.ToList<ASCTracFunctionStruct.LookupItemType>();
                    Globals.lookupMAFReaosnList = mySignonData.lookupMAFReaosnList.ToList<ASCTracFunctionStruct.LookupItemType>();
                    Globals.lookupVendorList = mySignonData.lookupVendorList.ToList<ASCTracFunctionStruct.LookupItemType>();
                    Globals.lookupWhseList = mySignonData.lookupWhseList.ToList<ASCTracFunctionStruct.LookupItemType>();

                    Globals.HHCID = mySignonData.HHCid;
                    Globals.UnitID = mySignonData.UnitID;
                    Globals.SaveSettings();

                    lblHHID.Text = "HHCID: Tablet " + Globals.HHCID;

                    Globals.curBasicMessage = new ASCTracFunctionStruct.ascBasicInboundMessageType(); // ascTracWCFService.ascBasicInboundMessageType();
                    Globals.curBasicMessage.SiteID = Globals.curSiteID;
                    Globals.curBasicMessage.UserID = Globals.curUserID;
                    Globals.curBasicMessage.HHID = Globals.HHCID;
                    Globals.curBasicMessage.UnitID = Globals.UnitID;
                    Globals.curBasicMessage.ConnectionID = Globals.ConnectionID;

                    await Navigation.PushAsync(new pageMenu()); // DisplayAlert("Success", "Success", "OK");
                }
                else
                    await DisplayAlert("Exception", mySignonData.ReturnMessage, "OK");
            }
            catch( Exception ex)
            {
                if (ex.InnerException == null)
                    await DisplayAlert("Exception", ex.Message, "OK");
                else
                    await DisplayAlert("Exception", ex.Message + "\r\nInnser: " + ex.InnerException.Message, "OK");
            }

            myIndicator.IsEnabled = false;
            myIndicator.IsRunning = false;
            myIndicator.IsVisible = false;
            btnLogon.IsEnabled = true;

            //myClient.SignonJsonCompleted += MyClient_SignonJsonCompleted;
            //myClient.SignonJsonAsync(mydata);
        }

        private void OnUserCompleted(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(edUserID.Text))
                edPassword.Focus();
        }

        public void OnPasswordCompleted(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(edUserID.Text) && !String.IsNullOrEmpty(edPassword.Text))
                onLogon(sender, e);
        }

        async public void onAbout(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new pageAbout());
        }
        async public void onSetup(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new pageSetup());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            edUserID.Text = string.Empty;
            edPassword.Text = string.Empty;
            edUserID.Focus();
        }

    }
}