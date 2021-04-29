using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.invLookup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageBOMAvail : ContentPage
    {
        public pageBOMAvail()
        {
            InitializeComponent();
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        async private void btnRefresh_Clicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(edItem.Text))
            {
                double qty = 0; // edQty.Text
                Double.TryParse(edQty.Text, out qty);
                if (qty > 0)
                {
                    string errmsg = string.Empty;
                    myIndicator.IsRunning = true;
                    myIndicator.IsVisible = true;
                    try
                    {
                        Globals.curBasicMessage.inputDataList = new List<string>();
                        Globals.curBasicMessage.inputDataList.Add(edItem.Text);
                        Globals.curBasicMessage.inputDataList.Add(qty.ToString());
                        var myReturnData = await App.myRestManager.GetBOMAvailList(Globals.curBasicMessage);

                        myIndicator.IsRunning = false;
                        myIndicator.IsVisible = false;
                        if (!myReturnData.successful)
                            errmsg = myReturnData.ErrorMessage;
                        else
                        {
                            var myList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.Inventory.BOMAvailType>>(myReturnData.DataMessage);
                            var myProdRecord = myList[0];
                            myList.Remove(myProdRecord);

                            lblItemDesc.Text = myProdRecord.Description;
                            lblQtyAvail.Text = myProdRecord.QtyTotal.ToString() + " " + myProdRecord.StockUOM;


                            listAvail.ItemsSource = myList;
                        }
                    }
                    catch (Exception ex)
                    {
                        errmsg = ex.Message;
                    }
                    if (!String.IsNullOrEmpty(errmsg))
                    {
                        await DisplayAlert("BOM Lookup Exception", errmsg, "OK");
                    }
                }
            }
        }


        private void listAvail_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {/*
            Data.dataLocItems rec = (Data.dataLocItems)listAvail.SelectedItem;

            ascTracWCFService.iInvLookupClient.EndpointConfiguration myEndpoint = new ascTracWCFService.iInvLookupClient.EndpointConfiguration();
            var myClient = new ascTracWCFService.iInvLookupClient(myEndpoint, pageLogin.hostURL);

            myClient.GetSkidHistoryListCompleted += MyClient_GetSkidHistoryListCompleted;
            myClient.GetSkidHistoryListAsync(rec.SkidID, rec.ItemID, rec.LocationID, Globals.curUserID, Globals.curSiteID);
            */
        }

    }
}
