using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.QCHold
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageQCTest : ContentPage
    {
        ASCTracFunctionStruct.QC.QCInventoryType myInvType = null;
        ASCTracFunctionStruct.Production.WOHdrType fWORec = null;
        string fQCPassword = string.Empty;
        string QCTestType = "S"; // S=Licnese, L=Lot, W=WO
        long batchnum = -1;
        int testIndex;
        public pageQCTest(ASCTracFunctionStruct.QC.QCInventoryType aInvType)
        {
            InitializeComponent();
            try
            {
                myInvType = aInvType;

                Title = "QC Tests";
                QCTestType = "S";

                lblSkidID.Text = myInvType.invRecord.SkidID;
                lblItemID.Text = myInvType.invRecord.ItemID + " - " + myInvType.invRecord.ItemDescription;
                lblQty.Text = myInvType.invRecord.QtyTotal.ToString() + " " + myInvType.invRecord.StockUOM;

                testIndex = 0;

                FillReasonCodes("");
            }
            catch (Exception e)
            {
                DisplayAlert("QC Test Setup Exception", e.ToString(), "OK");
            }
        }
        public pageQCTest(ASCTracFunctionStruct.Production.WOHdrType aWORec, string aQCTestType)
        {
            InitializeComponent();
            fWORec = aWORec;

            Title = "QC Tests";
            QCTestType = aQCTestType;
            if (QCTestType == "W")
            {
                lblSkidPrompt.Text = "Work Order ID";
                lblSkidID.Text = fWORec.Workorder_ID;
            }
            else if (QCTestType == "L")
            {
                lblSkidPrompt.Text = "Lot ID";
                lblSkidID.Text = fWORec.LotID;
            }
            else if (QCTestType == "S")
            {
                lblSkidPrompt.Text = "License ID";
                lblSkidID.Text = string.Empty;
            }


            lblItemID.Text = fWORec.Prod_ItemID + " - " + fWORec.Description;
            lblQty.Text = fWORec.QtyToMake.ToString();
            testIndex = 0;

            FillReasonCodes("");

            doGetWOInfo();
        }
        async private void doGetWOInfo()
        {
            string errmsg = string.Empty;
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add("W");
                Globals.curBasicMessage.inputDataList.Add(fWORec.Workorder_ID);
                var myReturnData = await App.myRestManager.GetQCTestInfo(Globals.curBasicMessage);

                myIndicator.IsRunning = false;
                myIndicator.IsVisible = false;
                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    myInvType = Newtonsoft.Json.JsonConvert.DeserializeObject<ASCTracFunctionStruct.QC.QCInventoryType>(myReturnData.DataMessage);
                    //pageLogin.mydatabase.SaveQCReasons(myInvType.qcHoldList);
                    if (myInvType.qcTestSetupList.Count == 0)
                    {
                        errmsg = "No Tests for Item";
                    }
                    else
                        SetupQCTest();
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            if (!String.IsNullOrEmpty(errmsg))
            {
                await DisplayAlert("GetTestInfo Exception", errmsg, "OK");
                await Navigation.PopAsync();
            }
        }

        private void FillReasonCodes(string aDefault)
        {
            int aDefIndex = -1;

            pickerReason.Items.Clear();
            foreach (var rec in Globals.myDatabase.GetReasonCodes())
            {
                if (rec.ReasonCode == aDefault)
                    aDefIndex = pickerReason.Items.Count;
                pickerReason.Items.Add(rec.ReasonCode + "-" + rec.Description);
            }
            if (aDefIndex >= 0)
                pickerReason.SelectedIndex = aDefIndex;
        }

        private void SetupQCTest()
        {
            if ((myInvType.qcTestSetupList != null) && (myInvType.qcTestSetupList.Count > testIndex))
            {
                lblTestNum.Text = myInvType.qcTestSetupList[testIndex].QuestionNum.ToString();
                lblPrompt.Text = myInvType.qcTestSetupList[testIndex].Prompt;

                chbPassFail.IsEnabled = (myInvType.qcTestSetupList[testIndex].PassFail == "T") ||
                    (myInvType.qcTestSetupList[testIndex].PassFail == "H");
                chbPassFail.IsToggled = false;

                pickerReason.IsEnabled = false;
                pickerReason.SelectedIndex = -1;

                edAnswer.Text = string.Empty;
                edAnswer.IsVisible = false;
                btnAnswer1.IsVisible = false;
                btnAnswer2.IsVisible = false;
                btnAnswer3.IsVisible = false;
                btnAnswer4.IsVisible = false;
                if (myInvType.qcTestSetupList[testIndex].AnswerType == "L")
                {
                    if (!String.IsNullOrEmpty(myInvType.qcTestSetupList[testIndex].Answer1))
                    {
                        btnAnswer1.IsVisible = true;
                        btnAnswer1.Text = myInvType.qcTestSetupList[testIndex].Answer1;
                        btnAnswer1.BackgroundColor = Color.Silver; //.Color.FromHex("EEB422");
                    }
                    if (!String.IsNullOrEmpty(myInvType.qcTestSetupList[testIndex].Answer2))
                    {
                        btnAnswer2.IsVisible = true;
                        btnAnswer2.Text = myInvType.qcTestSetupList[testIndex].Answer2;
                        btnAnswer2.BackgroundColor = Color.Silver; //.Color.FromHex("EEB422");
                    }
                    if (!String.IsNullOrEmpty(myInvType.qcTestSetupList[testIndex].Answer3))
                    {
                        btnAnswer3.IsVisible = true;
                        btnAnswer3.Text = myInvType.qcTestSetupList[testIndex].Answer3;
                        btnAnswer3.BackgroundColor = Color.Silver; //.Color.FromHex("EEB422");
                    }
                    if (!String.IsNullOrEmpty(myInvType.qcTestSetupList[testIndex].Answer4))
                    {
                        btnAnswer4.IsVisible = true;
                        btnAnswer4.Text = myInvType.qcTestSetupList[testIndex].Answer4;
                        btnAnswer4.BackgroundColor = Color.Silver; //.Color.FromHex("EEB422");
                    }
                }
                else
                {
                    edAnswer.IsVisible = true;
                    edAnswer.Focus();
                }
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (myInvType != null)
                SetupQCTest();
        }

        async public void onRunTest(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(edAnswer.Text))
                await DisplayAlert("QC Test", "Test Requires an Answer", "OK");
            else if (pickerReason.IsEnabled && (pickerReason.SelectedIndex < 0))
                await DisplayAlert("QC Test", "Failed Test requires a reason code", "OK");
            else
            {
                ASCTracFunctionStruct.QC.QCTestInstance myTest = new ASCTracFunctionStruct.QC.QCTestInstance();
                myTest.BatchNum = batchnum;
                myTest.Answer = edAnswer.Text;
                myTest.PassFail = chbPassFail.IsToggled;
                myTest.QuestionNum = myInvType.qcTestSetupList[testIndex].QuestionNum;
                myTest.Reason = string.Empty;
                if (pickerReason.IsEnabled)
                {
                    string[] tmp = pickerReason.Items[pickerReason.SelectedIndex].Split('-');
                    if (tmp.Length > 1)
                        myTest.Reason = tmp[0].Trim();
                }
                myTest.TestDateTime = DateTime.Now;

                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(QCTestType);
                if (QCTestType == "W")
                    Globals.curBasicMessage.inputDataList.Add(fWORec.Workorder_ID);
                else if (QCTestType == "L")
                    Globals.curBasicMessage.inputDataList.Add(fWORec.Prod_ItemID + "|" + fWORec.LotID);
                else
                    Globals.curBasicMessage.inputDataList.Add(myInvType.invRecord.SkidID);
                Globals.curBasicMessage.DataMessage = Newtonsoft.Json.JsonConvert.SerializeObject(myTest);

                string errmsg = string.Empty;
                myIndicator.IsEnabled = true;
                myIndicator.IsRunning = true;
                try
                {
                    var myReturnData = await App.myRestManager.doQCTest(Globals.curBasicMessage);

                    if (!myReturnData.successful)
                        errmsg = myReturnData.ErrorMessage;
                    else
                    {
                        testIndex++;
                        if (testIndex >= myInvType.qcTestSetupList.Count)
                            await Navigation.PopAsync();
                        else
                            SetupQCTest();
                    }
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }
                myIndicator.IsEnabled = false;
                myIndicator.IsRunning = false;
                if (!String.IsNullOrEmpty(errmsg))
                    await DisplayAlert("Test Exception", errmsg, "OK");
            }
        }


        public void onTestAnswer(object sender, EventArgs e)
        {
            btnAnswer1.BackgroundColor = Color.Silver; //.FromHex("EEB422");
            btnAnswer2.BackgroundColor = Color.Silver; //.FromHex("EEB422");
            btnAnswer3.BackgroundColor = Color.Silver; //.FromHex("EEB422");
            btnAnswer4.BackgroundColor = Color.Silver; //.FromHex("EEB422");

            (sender as Button).BackgroundColor = Color.Green;
            edAnswer.Text = (sender as Button).Text;

        }

        public void onPassFail(object sender, EventArgs e)
        {
            pickerReason.IsEnabled = chbPassFail.IsToggled && (myInvType.qcTestSetupList[testIndex].PassFail == "H");
        }


        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void OnReasonSelected(object sender, EventArgs e)
        {

        }
    }
}
