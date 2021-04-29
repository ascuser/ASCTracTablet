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
    public partial class pagePhysCountLocInfo : ContentPage
    {
        public ASCTracFunctionStruct.Inventory.PhysCountLocType myLoc;

        public pagePhysCountLocInfo()
        {
            InitializeComponent();
            Title = "Location Review";
        }

        async public void ShowLoc(ASCTracFunctionStruct.Inventory.PhysCountLocType aLoc)
        {
            myLoc = aLoc;
            BindingContext = myLoc;

            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;

            string errmsg = string.Empty;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(pagePhysCountMain.countID);
                Globals.curBasicMessage.inputDataList.Add( myLoc.LocationID);

                var myReturnData = await App.myRestManager.GetPhysLocitems(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    var mylist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.Inventory.InvType>>( myReturnData.DataMessage);
                    if (mylist.Count == 0)
                    {
                        errmsg = "No Inventory Returned";
                        listInv.ItemsSource = null;
                    }
                    else
                    {
                        listInv.ItemsSource = mylist;
                    }
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

        private async void btnRecount_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Recount Location", "This will reset all counts on location " + myLoc.LocationID, "OK", "Cancel"))
            {
                myIndicator.IsRunning = true;
                myIndicator.IsVisible = true;

                string errmsg = string.Empty;
                try
                {
                    Globals.curBasicMessage.inputDataList = new List<string>();
                    Globals.curBasicMessage.inputDataList.Add(pagePhysCountMain.countID);
                    Globals.curBasicMessage.inputDataList.Add(myLoc.LocationID);
                    Globals.curBasicMessage.inputDataList.Add(""); // item
                    Globals.curBasicMessage.inputDataList.Add(""); // license

                    var myReturnData = await App.myRestManager.RecountPhys(Globals.curBasicMessage);

                    if (!myReturnData.successful)
                        errmsg = myReturnData.ErrorMessage;
                    else
                    {
                        (Parent as pagePhysCountMain).RefreshList();
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

        private async void btncount_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Count Location", "This will mark all inventory as counted with current qty in location " + myLoc.LocationID, "OK", "Cancel"))
            {
                myIndicator.IsRunning = true;
                myIndicator.IsVisible = true;

                string errmsg = string.Empty;
                try
                {
                    Globals.curBasicMessage.inputDataList = new List<string>();
                    Globals.curBasicMessage.inputDataList.Add(pagePhysCountMain.countID);
                    Globals.curBasicMessage.inputDataList.Add(myLoc.LocationID);
                    Globals.curBasicMessage.inputDataList.Add(""); // item
                    Globals.curBasicMessage.inputDataList.Add(""); // license
                    Globals.curBasicMessage.inputDataList.Add("F"); // review only
                    Globals.curBasicMessage.inputDataList.Add("0"); // newqty
                    Globals.curBasicMessage.inputDataList.Add("0"); // newqtyDual


                    var myReturnData = await App.myRestManager.PhysCount(Globals.curBasicMessage);

                    if (!myReturnData.successful)
                        errmsg = myReturnData.ErrorMessage;
                    else
                    {
                        (Parent as pagePhysCountMain).RefreshList();
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


        private void listInv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var myInv = (e.SelectedItem as ASCTracFunctionStruct.Inventory.InvType);
            (Parent as pagePhysCountMain).ShowInv(myInv);
        }

        private async void btnReview_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Flag Location", "Flag location " + myLoc.LocationID + " as reviewed", "OK", "Cancel"))
            {
                myIndicator.IsRunning = true;
                myIndicator.IsVisible = true;

                string errmsg = string.Empty;
                try
                {
                    Globals.curBasicMessage.inputDataList = new List<string>();
                    Globals.curBasicMessage.inputDataList.Add(pagePhysCountMain.countID);
                    Globals.curBasicMessage.inputDataList.Add(myLoc.LocationID);
                    Globals.curBasicMessage.inputDataList.Add(""); // item
                    Globals.curBasicMessage.inputDataList.Add(""); // license
                    Globals.curBasicMessage.inputDataList.Add("T"); // review only
                    Globals.curBasicMessage.inputDataList.Add("0"); // newqty
                    Globals.curBasicMessage.inputDataList.Add("0"); // newqtyDual


                    var myReturnData = await App.myRestManager.PhysCount(Globals.curBasicMessage);

                    if (!myReturnData.successful)
                        errmsg = myReturnData.ErrorMessage;
                    else
                    {
                        (Parent as pagePhysCountMain).RefreshList();
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
