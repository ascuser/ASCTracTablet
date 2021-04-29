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
    public partial class pageQCHoldDetail : ContentPage
    {
        ASCTracFunctionStruct.QC.QCInventoryType myInvType;
        ASCTracFunctionStruct.QC.QCReasonType myHostQCType;
        DataModel.dataQCReasonType myQCType;
        DataModel.overrideType myOverride = new DataModel.overrideType();
        string fQCPassword = string.Empty;
        public pageQCHoldDetail(ASCTracFunctionStruct.QC.QCInventoryType aInvType, DataModel.dataQCReasonType aQCType)
        {
            InitializeComponent();
            myInvType = aInvType;
            myQCType = aQCType;
            Title = "Quality Control Detail";

            BindingContext = myInvType.invRecord;

            lblHoldDatetime.Text = myQCType.HoldDatetime.ToString();
            if (myQCType.ExpectedReleaseDate > Convert.ToDateTime("1/1/2001"))
                lblExpectedRelease.Text = myQCType.ExpectedReleaseDate.ToString();
            edMAFAction.Text = myQCType.MafAction;
            edMAFDesc.Text = myQCType.MafDescription;

            FillMAFReasonCodes(myQCType.MafStatus);
            FillReasonCodes(myQCType.Reason);

            if (myQCType.fNewRec)
                btnOnHold.Text = "Add";
            else if (myQCType.OnHold)
                btnOnHold.Text = "Release";
            else
                btnOnHold.IsEnabled = false;
        }

        private string GetFromPicker(Picker aPicker)
        {
            string retval = string.Empty;
            if (aPicker.SelectedIndex >= 0)
            {
                string aData = aPicker.Items[aPicker.SelectedIndex];
                string[] sData = aData.Split('-');
                retval = sData[0];
            }
            return (retval.Trim().ToUpper());
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
            pickerReason.IsEnabled = myQCType.OnHold;
            if (aDefIndex >= 0)
                pickerReason.SelectedIndex = aDefIndex;
        }
        private void FillMAFReasonCodes(string aDefault)
        {
            ascUtils.setupPicker(pickerMAFReason, Globals.lookupMAFReaosnList, aDefault);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (myOverride.fProcess && myOverride.fAnswered)
            {
                string overrideVal = string.Empty;
                if (!String.IsNullOrEmpty(fQCPassword))
                {
                    overrideVal = myOverride.foverridePassword;
                }
                else
                {
                    myHostQCType = new ASCTracFunctionStruct.QC.QCReasonType();
                    myHostQCType.MafAction = myQCType.MafAction;
                    myHostQCType.MafCatID = myQCType.MafCatID;
                    myHostQCType.MafDescription = myQCType.MafDescription;
                    myHostQCType.MafNum = myQCType.MafNum;
                    myHostQCType.MafStatus = myQCType.MafStatus;
                    myHostQCType.OnHold = myQCType.OnHold;
                    myHostQCType.Reason = myQCType.Reason;
                    myHostQCType.RefNum = myQCType.RefNum;

                    //fQCPassword = myOverride.foverridePassword;
                }
                doToggleQCSkid(overrideVal);
            }
            else
                fQCPassword = string.Empty;
            myOverride.fProcess = false;
            myOverride.fAnswered = false;

        }

        async private void doToggleQCSkid(string overrideVal)
        {
            myHostQCType.fQCPassword = fQCPassword;
            myHostQCType.fQCOverride = overrideVal;
            Globals.curBasicMessage.hdrDataMessage = Newtonsoft.Json.JsonConvert.SerializeObject(myInvType);
            Globals.curBasicMessage.DataMessage = Newtonsoft.Json.JsonConvert.SerializeObject(myHostQCType);

            string errmsg = string.Empty;
            myIndicator.IsEnabled = true;
            myIndicator.IsRunning = true;
            try
            {
                var myReturnData = await App.myRestManager.ToggleQCSkid(Globals.curBasicMessage);
                myIndicator.IsEnabled = false;
                myIndicator.IsRunning = false;

                if (!myReturnData.successful)
                {
                    if (myReturnData.ErrorMessage.StartsWith("ER"))
                        errmsg = myReturnData.ErrorMessage.Substring(2);
                    else if (myReturnData.ErrorMessage.StartsWith("QY"))
                    {
                        // get override 
                        //errmsg = "Need Override " + e.Result.Substring(2);
                        myOverride.fMessage = myReturnData.ErrorMessage.Substring(2);
                        await Navigation.PushAsync(new Maintenances.pageOverride(myOverride));
                    }
                    else
                        errmsg = myReturnData.ErrorMessage;
                }
                else
                {
                    myOverride.fAnswered = false;
                    myOverride.fProcess = false;
                    fQCPassword = string.Empty;
                    await Navigation.PopAsync();
                }
                }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            myIndicator.IsEnabled = false;
            myIndicator.IsRunning = false;

            if (!string.IsNullOrEmpty(errmsg))
                await DisplayAlert(Globals.AppTitleName, errmsg, "OK");
        }

        async public void onHold(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(edPassword.Text))
                edPassword.Focus();
            else
            {
                bool fdoit = false;
                string qcType = "Hold";
                if (myQCType.fNewRec)
                {
                    fdoit = (await DisplayAlert("ASCTrac QC", "Add QC Hold to License: " + myInvType.invRecord.SkidID, "Yes", "No"));
                }
                else if (myQCType.OnHold)
                {
                    if (await DisplayAlert("ASCTrac QC", "Release QC Hold on License: " + myInvType.invRecord.SkidID, "Yes", "No"))
                    {
                        qcType = "Release";
                        fdoit = true;
                        myQCType.OnHold = false;

                    }
                }
                else
                    await DisplayAlert("ASCtrac", "License no longer on hold", "ok");

                if (fdoit)
                {

                    myQCType.Reason = GetFromPicker(pickerReason);
                    if (pickerMAFReason.IsEnabled)
                        myQCType.MafStatus = ascUtils.getPickerValue(pickerMAFReason);
                    myQCType.MafDescription = edMAFDesc.Text;
                    myQCType.MafAction = edMAFAction.Text;
                    // send QC Hold for License
                    //myOverride.fMessage = "Enter QC " + qcType + " password";
                    //await Navigation.PushAsync(new pageOverride(myOverride));
                    fQCPassword = edPassword.Text;
                    myHostQCType = new ASCTracFunctionStruct.QC.QCReasonType();
                    myHostQCType.MafAction = myQCType.MafAction;
                    myHostQCType.MafCatID = myQCType.MafCatID;
                    myHostQCType.MafDescription = myQCType.MafDescription;
                    myHostQCType.MafNum = myQCType.MafNum;
                    myHostQCType.MafStatus = myQCType.MafStatus;
                    myHostQCType.OnHold = myQCType.OnHold;
                    myHostQCType.Reason = myQCType.Reason;
                    myHostQCType.RefNum = myQCType.RefNum;

                    //fQCPassword = myOverride.foverridePassword;
                    doToggleQCSkid(string.Empty);
                }
            }
        }


        public void OnReasonSelected(object sender, EventArgs e)
        {
            if (pickerReason.SelectedIndex >= 0)
            {
                string[] str = pickerReason.Items[pickerReason.SelectedIndex].Split('-');
                var reasonRec = Globals.myDatabase.GetReason(str[0].Trim());
                bool fMAFEditable = pickerReason.IsEnabled && ((reasonRec != null) && (reasonRec.MAFFlag.Equals("T") || reasonRec.MAFFlag.Equals("R")));
                edMAFAction.IsEnabled = fMAFEditable;
                edMAFDesc.IsEnabled = fMAFEditable;
                pickerMAFReason.IsEnabled = fMAFEditable;
            }
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
