using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.Production
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageProdWO : ContentPage
    {
        static string fDefaultPrinter = String.Empty;
        ASCTracFunctionStruct.Production.WOHdrType fWORec;
        ASCTracFunctionStruct.Production.ProdNewSkidType newskidRec = new ASCTracFunctionStruct.Production.ProdNewSkidType();
        DataModel.overrideType myOverride = new DataModel.overrideType();
        bool fWeightIsCustom = false;
        public pageProdWO(ASCTracFunctionStruct.Production.WOHdrType aWORec)
        {
            fWORec = aWORec;
            Title = "Manufacturing Entry";
            InitializeComponent();

            lblWO.Text = fWORec.Workorder_ID;
            lblProdItemDesc.Text = fWORec.Prod_ItemID + " - " + fWORec.Description;

            lblQty.Text = fWORec.QtyMade.ToString() + " of " + fWORec.QtyToMake.ToString();
            lblNumLPNMade.Text = fWORec.numSkidsMade.ToString();
            //edLotID.Text = fWORec.LotID;
            newskidRec.Workorder_ID = fWORec.Workorder_ID;

            ascUtils.setupPicker(pickPrinter, Globals.PrinterLBLList, fDefaultPrinter);

            edNumSkid.Text = "1";
            edQtyLabels.Text = "1";

            setDefaults();
        }

        private void setDefaults()
        {
            SetProdEntry("SKIDID", edSkidID, null);
            SetProdEntry("LOTID", edLotID, lblLotID);
            SetProdEntry("QTY", edQtySkid, null);
            SetProdEntry("WEIGHT", edWgtSkid, lblWgtSkid);
            SetProdEntry("PRODDATE", edProdDate, lblProdDate);
            SetProdEntry("PRODDATE", edProdtime, null);
            fWeightIsCustom = false;
            if (!edWgtSkid.IsVisible)
            {
                fWeightIsCustom = true;
                SetProdEntry("CUSTOM", edWgtSkid, lblWgtSkid);
            }
            SetProdEntry("PRINTERID", pickPrinter, null);
            SetProdEntry("PACKPRINTERID", pickPackPrinter, lblPackPrinter);
        }

        private void SetProdEntry(string aFieldname, Entry aEntry, Label aLabel)
        {
            if (fWORec.entryList.ContainsKey(aFieldname))
            {
                var rec = fWORec.entryList[aFieldname];
                if (rec.EntryFlag == "F")
                {
                    aEntry.IsVisible = false;
                    if (aLabel != null)
                        aLabel.IsVisible = false;
                }
                else
                {
                    if (aLabel != null)
                    {
                        if (!string.IsNullOrEmpty(rec.PromptStr))
                            aLabel.Text = rec.PromptStr;
                    }
                    if (rec.EntryType == "N")
                        aEntry.Keyboard = Keyboard.Numeric;
                    if (!String.IsNullOrEmpty(rec.DefaultValue))
                        aEntry.Text = rec.DefaultValue;
                }
            }
        }

        private void SetProdEntry(string aFieldname, View aEntry, Label aLabel)
        {
            if (fWORec.entryList.ContainsKey(aFieldname))
            {
                var rec = fWORec.entryList[aFieldname];
                if (rec.EntryFlag == "F")
                {
                    aEntry.IsVisible = false;
                    if (aLabel != null)
                        aLabel.IsVisible = false;
                }
                else
                {
                    if (aLabel != null)
                    {
                        if (!string.IsNullOrEmpty(rec.PromptStr))
                            aLabel.Text = rec.PromptStr;
                    }
                }
            }
        }

        private void SetProdEntry(string aFieldname, Picker aPicker, Label aLabel)
        {
            if (fWORec.entryList.ContainsKey(aFieldname))
            {
                var rec = fWORec.entryList[aFieldname];
                if (rec.EntryFlag == "F")
                {
                    aPicker.IsVisible = false;
                    if (aLabel != null)
                        aLabel.IsVisible = false;
                }
                else
                {
                    if (aLabel != null)
                    {
                        if (!string.IsNullOrEmpty(rec.PromptStr))
                            aLabel.Text = rec.PromptStr;
                    }
                    if (!String.IsNullOrEmpty(rec.DefaultValue))
                    {
                        for (int i = 0; i < aPicker.Items.Count; i++)
                        {
                            if (aPicker.Items[i].StartsWith(rec.DefaultValue + " - "))
                            {
                                aPicker.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (myOverride.fProcess && myOverride.fAnswered)
            {
                newskidRec.overridePassword = myOverride.foverridePassword;

                doProdNewskid();
            }
            myOverride.fProcess = false;
            myOverride.fAnswered = false;
        }

        async public void onProduce(object sender, EventArgs e)
        {
            string errmsg = string.Empty;
            if (string.IsNullOrEmpty(edQtySkid.Text))
                errmsg = "License Qty is required";
            else if (Convert.ToDouble(edQtySkid.Text) <= 0)
                errmsg = "License Qty must be a positive value";
            else if (await DisplayAlert("Production", "Produce new inventory for Work Order " + fWORec.Workorder_ID, "Yes", "No"))
            {
                newskidRec.AssetID = string.Empty;
                newskidRec.ExpDate = string.Empty;
                newskidRec.LotID = edLotID.Text;
                newskidRec.numSkids = Convert.ToInt64(edNumSkid.Text);
                newskidRec.overridePassword = string.Empty;

                fDefaultPrinter = ascUtils.getPickerValue(pickPrinter);
                newskidRec.PrinterID = fDefaultPrinter;
                newskidRec.ProdDate = string.Empty;
                newskidRec.ProdTime = string.Empty;
                if (edProdDate.Date != null)
                    newskidRec.ProdDate = edProdDate.Date.ToString();
                if (edProdtime.Time != null)
                    newskidRec.ProdTime = edProdtime.Time.ToString();

                newskidRec.QtyLabels = Convert.ToInt64(edQtyLabels.Text);
                newskidRec.QtyMade = Convert.ToDouble(edQtySkid.Text);
                if (String.IsNullOrEmpty(edSkidID.Text))
                    newskidRec.SkidID = string.Empty;
                else
                    newskidRec.SkidID = edSkidID.Text;
                if (fWeightIsCustom)
                    newskidRec.Custom = edWgtSkid.Text;
                else if (!String.IsNullOrEmpty(edWgtSkid.Text))
                    newskidRec.Weight = Convert.ToDouble(edWgtSkid.Text);
                doProdNewskid();
            }
            if (!String.IsNullOrEmpty(errmsg))
                await DisplayAlert("Production", errmsg, "OK");
        }


        async private void doProdNewskid()
        {
            Globals.curBasicMessage.DataMessage = Newtonsoft.Json.JsonConvert.SerializeObject(newskidRec);

            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            string errmsg = string.Empty;
            try
            {
                var myReturnData = await App.myRestManager.ProdNewSkid(Globals.curBasicMessage);

                myIndicator.IsRunning = false;
                myIndicator.IsVisible = false;
                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else if (myReturnData.DataMessage.StartsWith("QY"))
                {
                    // get override 
                    myOverride.fMessage = myReturnData.DataMessage.Substring(2);
                    await Navigation.PushAsync(new Maintenances.pageOverride(myOverride));
                }
                else if (myReturnData.DataMessage.StartsWith("OK"))
                {
                    string[] retlist = myReturnData.DataMessage.Substring(2).Split('|'); // skid, lot, lotqty, container, serial flag
                    fWORec.QtyMade += newskidRec.QtyMade;
                    lblQty.Text = fWORec.QtyMade.ToString() + " of " + fWORec.QtyToMake.ToString();
                    edSkidID.Text = "";
                    //errmsg = "License " + retlist[0] + " has been produced";
                    lblPrevSkidID.Text = retlist[0];
                }
                else
                    errmsg = myReturnData.DataMessage;
            }
            catch (Exception ex)
            {
                errmsg = "Exception :" + ex.Message;
            }
            myIndicator.IsRunning = false;
            myIndicator.IsVisible = false;
            if (!string.IsNullOrEmpty(errmsg))
                await DisplayAlert("Production Exception", errmsg, "OK");
        }


        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            (Parent as pageWOTab).CurrentPage = (Parent as pageWOTab).Children[0];
            //Navigation.PopAsync();
        }
    }
}

