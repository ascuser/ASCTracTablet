using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.Receipt.CloseRecv
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCloseRecv : ContentPage
    {
        public static ASCTracFunctionStruct.Receipt.ConfReceiptType myConfirmData = null;
        //public static ASCTracRestService.DataModel.ConfirmReceiptInfo myConfirmData = null;
        public static bool fUseCarouselPage = false;
        public pageCloseRecv()
        {
            InitializeComponent();
            Title = "PO Info";
            btnClose.IsEnabled = false;
            edPONumber.Focus();

            cbReceiptType.Items.Add("P-PO");
            cbReceiptType.Items.Add("R-Receiver");
            cbReceiptType.SelectedIndex = 0;

            FillCarrier(string.Empty);
            FillShipVia(string.Empty);

        }

        public void Init(string aRecType, string aRecID)
        {
            edPONumber.Text = aRecID;
            if (aRecType.Equals("R"))
                cbReceiptType.SelectedIndex = 1;
            else
                cbReceiptType.SelectedIndex = 0;
            edPONumber.IsEnabled = false;
            cbReceiptType.IsEnabled = false;
            edPONumber_Completed(null, null);
        }

        private void FillCarrier(string aCurrentCarrier)
        {
            ascUtils.setupPicker(cbCarrier, Globals.lookupCarrierList, aCurrentCarrier);
        }

        private void FillShipVia(string aCurrentShipVia)
        {
            ascUtils.setupPicker(cbShipVia, Globals.lookupShipVIAList, aCurrentShipVia);
        }

        private void DisplayData()
        {
            edTrailer.Text = myConfirmData.TrailerNum;
            edSeal.Text = myConfirmData.SealNum;
            edWhseNum.Text = myConfirmData.WhseReceiptNumber;
            lblVendorInfo.Text = myConfirmData.VendorID + " - " + myConfirmData.VendorName;

            chbInTact.IsToggled = true;
            chbOntime.IsToggled = true;
            chbBadDocuments.IsToggled = myConfirmData.DocumentsFlag;
            chbDamage.IsToggled = myConfirmData.DamageFlag;

            FillCarrier(myConfirmData.CarrierID);
            FillShipVia(myConfirmData.ShipVia);
            btnClose.IsEnabled = true;
        }

        public void ClearData()
        {
            edPONumber.Text = "";
            lblVendorInfo.Text = "N/A";
            edTrailer.Text = "N/A";
            edWhseNum.Text = "N/A";
            edSeal.Text = "N/A";
            cbCarrier.SelectedIndex = -1;
            cbShipVia.SelectedIndex = -1;
            btnClose.IsEnabled = false;
            edPONumber.Focus();
            //while( irow >= gridCustomData.row
            //gridCustomData.
        }

        public void SaveData()
        {
            myConfirmData.TrailerNum = edTrailer.Text;
            myConfirmData.SealNum = edSeal.Text;
            myConfirmData.WhseReceiptNumber = edWhseNum.Text;
            myConfirmData.DamageFlag = chbDamage.IsToggled;
            myConfirmData.DocumentsFlag = chbBadDocuments.IsToggled;
            myConfirmData.OnTimeFlag = chbOntime.IsToggled;
            myConfirmData.SealIntactFlag = chbInTact.IsToggled;

            if (cbCarrier.SelectedIndex >= 0)
                myConfirmData.CarrierID = ascUtils.getPickerValue(cbCarrier);
            if (cbShipVia.SelectedIndex >= 0)
                myConfirmData.ShipVia = ascUtils.getPickerValue(cbShipVia);

            Globals.myDatabase.fill3PLRate(myConfirmData.lookupRates3PLList);

        }

        async public void ProcessReceipt()
        {
            string myRet = string.Empty;
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            try
            {
                Globals.curBasicMessage.DataMessage = Newtonsoft.Json.JsonConvert.SerializeObject(myConfirmData);
                var myReturnData = await App.myRestManager.doConfirmReceiptInfo(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    myRet = myReturnData.ErrorMessage;
                {
                    ProcessConfirmReceipt(string.Empty);
                }
            }
            catch (Exception ex)
            {
                myRet = ex.Message;
                //await DisplayAlert("Process Exception", ex.Message, "OK");
            }
            ProcessConfirmReceipt(myRet);
            //myIndicator.IsRunning = false;
            //myIndicator.IsVisible = false;

            /*
            try
            {
                myRet = await RestService.restSignon.doConfirmReceipt(myConfirmData);
            }
            catch (Exception ex)
            {
                myRet = ex.Message;
            }
            //finally
            {
                myIndicator.IsRunning = false;
                myIndicator.IsVisible = false;

                if (!String.IsNullOrEmpty(myRet))
                {
                    await DisplayAlert("ASCTrac", myRet, "OK");
                }
                else
                {
                    ClearData();
                    if (fUseCarouselPage)
                    {
                        ((Parent as Recv.pageCloseRecvCarousel).Children[1] as Recv.pageCloseRecvPalletLog).ClearData();
                        ((Parent as Recv.pageCloseRecvCarousel).Children[2] as Recv.pageCloseRecvFees).ClearData();
                        ((Parent as Recv.pageCloseRecvCarousel).Children[3] as Recv.pageCloseRecvCustom).ClearData();
                    }
                    else
                    {
                    }
                }
            }
            

            Globals.curBasicMessage.DataMessage = Newtonsoft.Json.JsonConvert.SerializeObject(myConfirmData);

            ascTracWCFService.iReceiptClient.EndpointConfiguration myEndpoint = new ascTracWCFService.iReceiptClient.EndpointConfiguration();
            var myClient = new ascTracWCFService.iReceiptClient(myEndpoint, pageLogin.hostURL);

            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            myClient.DoConfirmReceiptCompleted += MyClient_DoConfirmReceiptCompleted;
            myClient.DoConfirmReceiptAsync(Globals.curBasicMessage);

            */
        }

        //private void MyClient_DoConfirmReceiptCompleted(object sender, ascTracWCFService.DoConfirmReceiptCompletedEventArgs e)
        async private void ProcessConfirmReceipt(string aErrMsg)
        {
            try
            {
                Globals.curBasicMessage.DataMessage = string.Empty;
                myIndicator.IsRunning = false;
                myIndicator.IsVisible = false;
                if (String.IsNullOrEmpty(aErrMsg))
                {
                    if (!edPONumber.IsEnabled)
                        await Navigation.PopAsync();
                    else
                    {
                        ClearData();
                        ((Parent as pageCloseRecvTab).Children[1] as pageCloseRecvPalletLog).ClearData();
                        ((Parent as pageCloseRecvTab).Children[2] as pageCloseRecvFees).ClearData();
                        ((Parent as pageCloseRecvTab).Children[3] as pageCloseRecvCustom).ClearData();
                    }
                }
                else
                    await DisplayAlert("ASCTrac", aErrMsg, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("ASCTrac", ex.Message, "OK");
            }

        }

        async private void edPONumber_Completed(object sender, EventArgs e)
        {
            string ponum = edPONumber.Text;
            string relnum = string.Empty;
            if (ponum.Contains("-"))
            {
                string[] sList = ponum.Split('-');
                ponum = sList[0];
                for (int i = 1; i < sList.Length - 1; i++)
                    ponum += "-" + sList[i];
                relnum = sList[sList.Length - 1];
            }
            var ordertyp = cbReceiptType.Items[cbReceiptType.SelectedIndex].Substring(0, 1);

            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            try
            {
                Globals.curBasicMessage.inputDataList.Clear();
                Globals.curBasicMessage.inputDataList.Add(ordertyp);
                Globals.curBasicMessage.inputDataList.Add(ponum);
                Globals.curBasicMessage.inputDataList.Add(relnum);
                var myReturnData = await App.myRestManager.GetConfirmReceiptInfo(Globals.curBasicMessage);

                if (myReturnData.successful)
                {
                    myConfirmData = Newtonsoft.Json.JsonConvert.DeserializeObject<ASCTracFunctionStruct.Receipt.ConfReceiptType>(myReturnData.DataMessage);
                    (Parent as pageCloseRecvTab).ShowTabs();
                    DisplayData();
                    ((Parent as pageCloseRecvTab).Children[1] as pageCloseRecvPalletLog).DisplayData();
                    ((Parent as pageCloseRecvTab).Children[2] as pageCloseRecvFees).DisplayData();
                    ((Parent as pageCloseRecvTab).Children[3] as pageCloseRecvCustom).DisplayData();
                }
                else
                    await DisplayAlert("ASCTrac-Recv Metrics", "Error :" + myReturnData.ErrorMessage, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("ASCTrac-Recv Metrics", "Exception :" + ex.Message, "OK");
            }
            myIndicator.IsRunning = false;
            myIndicator.IsVisible = false;
        }

        private void btnNext_Clicked(object sender, EventArgs e)
        {
            if ((myConfirmData == null) || string.IsNullOrEmpty(myConfirmData.PONumber) || !myConfirmData.PONumber.Equals(edPONumber.Text))
            {
                if (myConfirmData == null)
                    DisplayAlert("DEBUG", "NULL data", "ok");
                else
                    DisplayAlert("DEBUG", "data " + myConfirmData.PONumber + "," + edPONumber.Text, "ok");
                edPONumber_Completed(sender, e);
            }
            else
                (Parent as pageCloseRecvTab).CurrentPage = (Parent as pageCloseRecvTab).Children[1];
        }

        private void btnClose_Clicked(object sender, EventArgs e)
        {
            (Parent as pageCloseRecvTab).CompleteReceipt();
        }

        private void cbReceiptType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbReceiptType.SelectedIndex == 1)
                lblOrderType.Text = "Receiver:";
            else
                lblOrderType.Text = "PO Number:";
        }

        private void ProcessGetConfirmReceiptData(ASCTracFunctionStruct.ascBasicReturnMessageType aReturnData)
        {
            try
            {
                myIndicator.IsRunning = false;
                myIndicator.IsVisible = false;
                var myReturnData = aReturnData;
                if (!myReturnData.successful)
                    DisplayAlert(Globals.AppTitleName, myReturnData.ErrorMessage, "OK");
                else
                {
                    myConfirmData = Newtonsoft.Json.JsonConvert.DeserializeObject<ASCTracFunctionStruct.Receipt.ConfReceiptType>(myReturnData.DataMessage);
                    (Parent as pageCloseRecvTab).ShowTabs();
                    DisplayData();
                    ((Parent as pageCloseRecvTab).Children[1] as pageCloseRecvPalletLog).DisplayData();
                    ((Parent as pageCloseRecvTab).Children[2] as pageCloseRecvFees).DisplayData();
                    ((Parent as pageCloseRecvTab).Children[3] as pageCloseRecvCustom).DisplayData();
                }

            }
            catch (Exception ex)
            {
                DisplayAlert(Globals.AppTitleName, ex.Message, "OK");
            }

        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
