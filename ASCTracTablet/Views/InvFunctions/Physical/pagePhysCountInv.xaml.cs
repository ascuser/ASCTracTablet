using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.InvFunctions.Physical
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pagePhysCountInv : ContentPage
    {
        public string myLocID = string.Empty;
        public ASCTracFunctionStruct.Inventory.InvType myInv;
        public pagePhysCountInv()
        {
            InitializeComponent();
        }

        public void ShowInv(string aLocId, ASCTracFunctionStruct.Inventory.InvType aInv)
        {
            myLocID = aLocId;
            myInv = aInv;
            Title = "License " + myInv.SkidID;
            BindingContext = myInv;
            lblNewQty.Text = (myInv.QtyTotal + myInv.PhysAdj).ToString();

            edQtyTotal.Text = lblNewQty.Text;
            if (myInv.QtyDualUnit < 0)
            {
                edQtyDualUnit.IsVisible = false;
                lbledQtyDualUnit.IsVisible = false;
            }
            else
                edQtyDualUnit.Text = myInv.QtyDualUnit.ToString();
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            (Parent as pagePhysCountMain).CancelInv(myInv);
        }

        private async void btnCount_Clicked(object sender, EventArgs e)
        {
            double newQty = Convert.ToDouble(edQtyTotal.Text);
            double newQtyDual = 0;
            if (myInv.QtyDualUnit >= 0)
                newQtyDual = Convert.ToDouble(edQtyDualUnit.Text);
            if ((newQty != myInv.QtyTotal + myInv.PhysAdj) || (newQtyDual != myInv.QtyDualUnit) || !String.IsNullOrEmpty(myInv.PhysLoc))

                if (await DisplayAlert("Count Inventory", "Update Inventory with new counts?", "OK", "Cancel"))
                {
                    myIndicator.IsRunning = true;
                    myIndicator.IsVisible = true;

                    string errmsg = string.Empty;
                    try
                    {
                        Globals.curBasicMessage.inputDataList = new List<string>();
                        Globals.curBasicMessage.inputDataList.Add(pagePhysCountMain.countID);
                        Globals.curBasicMessage.inputDataList.Add(myLocID);
                        Globals.curBasicMessage.inputDataList.Add(myInv.ItemID); // item
                        Globals.curBasicMessage.inputDataList.Add(myInv.SkidID); // license
                        Globals.curBasicMessage.inputDataList.Add("F"); // review only
                        Globals.curBasicMessage.inputDataList.Add(newQty.ToString()); // newqty
                        Globals.curBasicMessage.inputDataList.Add(newQtyDual.ToString()); // newqtyDual

                        var myReturnData = await App.myRestManager.PhysCount(Globals.curBasicMessage);

                        if (!myReturnData.successful)
                            errmsg = myReturnData.ErrorMessage;
                        else
                        {
                            (Parent as pagePhysCountMain).RefreshInvList(myInv.SkidID);
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


        private async void btnRecount_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Recount Inventory", "This will reset count for this inventory.", "OK", "Cancel"))
            {
                myIndicator.IsRunning = true;
                myIndicator.IsVisible = true;

                string errmsg = string.Empty;
                try
                {
                    Globals.curBasicMessage.inputDataList = new List<string>();
                    Globals.curBasicMessage.inputDataList.Add(pagePhysCountMain.countID);
                    Globals.curBasicMessage.inputDataList.Add(myLocID);
                    Globals.curBasicMessage.inputDataList.Add(myInv.ItemID); // item
                    Globals.curBasicMessage.inputDataList.Add(myInv.SkidID); // license

                    var myReturnData = await App.myRestManager.RecountPhys(Globals.curBasicMessage);

                    if (!myReturnData.successful)
                        errmsg = myReturnData.ErrorMessage;
                    else
                    {
                        (Parent as pagePhysCountMain).RefreshInvList(myInv.SkidID);
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
            (Parent as pagePhysCountMain).CurrentPage = (Parent as pagePhysCountMain).Children[0];

            //Navigation.PopAsync();
        }
    }
}
