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
    public partial class pageProdIssueInventory : ContentPage
    {

        ASCTracFunctionStruct.Production.WOHdrType fWORec;
        DataModel.dataWODet myItem;
        DataModel.dataLocItems mySkid;

        public pageProdIssueInventory(ASCTracFunctionStruct.Production.WOHdrType aWORec, DataModel.dataWODet aItem, DataModel.dataLocItems aSkid)
        {


            fWORec = aWORec;
            Title = "Issue License";
            myItem = aItem;
            mySkid = aSkid;
            InitializeComponent();
            lblWO.Text = fWORec.Workorder_ID;
            lblItemID.Text = myItem.Comp_ItemID + " - " + myItem.Description;
            lblSkidID.Text = mySkid.SkidID;
            lblQtyAvail.Text = mySkid.QtyTotal.ToString();
        }

        async private void onIssue(object sender, EventArgs e)
        {
            double qty = 0;
            if (!String.IsNullOrEmpty(edQty.Text))
                qty = Convert.ToDouble(edQty.Text);
            if (qty > 0)
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(fWORec.Workorder_ID);
                Globals.curBasicMessage.inputDataList.Add(myItem.SeqNum.ToString());
                Globals.curBasicMessage.inputDataList.Add(mySkid.SkidID);
                Globals.curBasicMessage.inputDataList.Add(string.Empty); // FG SkidID
                Globals.curBasicMessage.inputDataList.Add(myItem.Comp_ItemID);
                Globals.curBasicMessage.inputDataList.Add(mySkid.LocationID);
                Globals.curBasicMessage.inputDataList.Add(qty.ToString());


                string errmsg = string.Empty;
                myIndicator.IsEnabled = true;
                myIndicator.IsRunning = true;
                try
                {
                    var myReturnData = await App.myRestManager.WOIssueComponent(Globals.curBasicMessage);
                    myIndicator.IsEnabled = false;
                    myIndicator.IsRunning = false;

                    if (!myReturnData.successful)
                        errmsg = myReturnData.ErrorMessage;
                    else
                    {
                        await DisplayAlert(Globals.AppTitleName, "License " + mySkid.SkidID + " issued", "OK");
                        await Navigation.PopAsync();
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

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
