using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.CODetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCOPrintReport : ContentPage
    {
        ASCTracFunctionStruct.CustOrder.CustOrderInfoType myCO;
        ASCTracFunctionStruct.COShip.COShipHdr myShipHdr = null;
        string fReporttype;
        public pageCOPrintReport(string aReportType, ASCTracFunctionStruct.CustOrder.CustOrderInfoType aCOInfo)
        {
            InitializeComponent();
            fReporttype = aReportType;

            if (fReporttype.Equals("B"))
                lblTitle.Text = "Print BOL";
            if (fReporttype.Equals("P"))
                lblTitle.Text = "Packlist";
            if (fReporttype.Equals("K"))
                lblTitle.Text = "Picklist";

            myCO = aCOInfo;
            ascUtils.setupPicker(pickPrinterID, Globals.PrinterBOLList, string.Empty);

            GetCOReportInfo();
        }

        async private void GetCOReportInfo()
        {
            if (myShipHdr == null)
            {
                myIndicator.IsRunning = true;
                myIndicator.IsVisible = true;
                string errmsg = string.Empty;
                try
                {
                    Globals.curBasicMessage.inputDataList = new List<string>();
                    Globals.curBasicMessage.inputDataList.Add(myCO.OrderNumber.ToUpper());
                    Globals.curBasicMessage.inputDataList.Add(fReporttype);

                    var myReturnData = await App.myRestManager.GetCOReportInfo(Globals.curBasicMessage);

                    if (!myReturnData.successful)
                        errmsg = myReturnData.ErrorMessage;
                    else
                    {
                        myShipHdr =  Newtonsoft.Json.JsonConvert.DeserializeObject<ASCTracFunctionStruct.COShip.COShipHdr>(myReturnData.DataMessage);
                        BindingContext = myShipHdr;

                        if (myShipHdr.fSignatureCaptured)
                            padSign.PromptText = "Already Captured";
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
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        async private void btnPrint_Clicked(object sender, EventArgs e)
        {
            string errmsg = string.Empty;
            if (myShipHdr.fNeedSignature && padSign.IsBlank)
                errmsg = "Signature Required";
            else
            {
                if (!padSign.IsBlank)
                {
                    var image = await padSign.GetImageStreamAsync(SignaturePad.Forms.SignatureImageFormat.Png);
                    image.Seek(0, SeekOrigin.Begin);
                    using (BinaryReader binaryReader = new BinaryReader(image))
                    {
                        binaryReader.BaseStream.Position = 0;
                        myShipHdr.SignatureByteArray = binaryReader.ReadBytes((int)image.Length);
                    }
                    /*                    
                    var signatureMemoryStream = image as MemoryStream;
                    byte[] data = signatureMemoryStream.ToArray();
                    myShipHdr.SignatureByteArray = data;
                    */
                }

                myIndicator.IsRunning = true;
                myIndicator.IsVisible = true;

                string printerID = string.Empty;
                try
                {
                    printerID = ascUtils.getPickerValue(pickPrinterID);

                    Globals.curBasicMessage.inputDataList = new List<string>();
                    Globals.curBasicMessage.inputDataList.Add(fReporttype);
                    Globals.curBasicMessage.inputDataList.Add(printerID);
                    Globals.curBasicMessage.DataMessage = Newtonsoft.Json.JsonConvert.SerializeObject(myShipHdr);

                    var myReturnData = await App.myRestManager.doCOReport(Globals.curBasicMessage);

                    if (!myReturnData.successful)
                        errmsg = myReturnData.ErrorMessage;
                    else
                    {
                        myIndicator.IsRunning = false;
                        myIndicator.IsVisible = false;
                        if (!String.IsNullOrEmpty(myReturnData.DataMessage))
                            await DisplayAlert("ASCTrac", myReturnData.DataMessage, "OK");
                        await Navigation.PopAsync();
                    }
                }
                catch (Exception ex)
                {
                    errmsg = "Exception\r\n" + ex.Message;
                }

                myIndicator.IsRunning = false;
                myIndicator.IsVisible = false;
            }
            if (!String.IsNullOrEmpty(errmsg))
                await DisplayAlert("Exception", errmsg, "OK");

        }

       
    }
}