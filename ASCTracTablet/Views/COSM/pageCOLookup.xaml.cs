using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.COSM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCOLookup : ContentPage
    {
        public pageCOLookup()
        {
            InitializeComponent();
            FillPickers();
        }

        private void FillPickers()
        {
            pickCustType.Items.Clear();
            pickCustType.Items.Add("Ship to CustID");
            pickCustType.Items.Add("Sold to CustID");
            pickCustType.Items.Add("Ship to Name");
            pickCustType.Items.Add("Sold to Name");
            pickCustType.SelectedIndex = 0;

            pickFilterType.Items.Clear();
            pickFilterType.Items.Add("Required Ship Date");
            pickFilterType.Items.Add("Carrier");
            pickFilterType.Items.Add("Dock");
            pickFilterType.Items.Add("Pick Status");
            pickFilterType.Items.Add("Trailer");
            pickFilterType.Items.Add("Order Type");
            pickFilterType.SelectedIndex = 0;

        }

        private void edCustInfo_Completed(object sender, EventArgs e)
        {
            btnRefresh_Clicked(sender, e);
        }

        private void btnRefresh_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(edCONumber.Text))
                doRefreshList();
            else
                doRefreshCO();
        }

        //=============================================================================
        async private void doRefreshList()
        {

            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;

            string errmsg = string.Empty;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(pickCustType.SelectedIndex.ToString());
                Globals.curBasicMessage.inputDataList.Add(edCustInfo.Text);
                Globals.curBasicMessage.inputDataList.Add(pickFilterType.SelectedIndex.ToString());
                Globals.curBasicMessage.inputDataList.Add(edFilterInfo.Text);


                var myReturnData = await App.myRestManager.GetCOList(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    var mylist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.CustOrder.CustOrderInfoType>>(myReturnData.DataMessage);
                    if (mylist.Count == 0)
                        errmsg = "No Counts Returned";
                    else
                    {
                        var myList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.CustOrder.CustOrderInfoType>>(myReturnData.DataMessage);
                        //DisplayAlert("ASCTrac Exception","Found " + myList.Count.ToString() + " orders ", "OK");
                        listCOs.ItemsSource = myList;
                        lblCount.Text = myList.Count.ToString();
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

        //=============================================================================
        async private void doRefreshCO()
        {

            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;

            string errmsg = string.Empty;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(edCONumber.Text.ToUpper());


                var myReturnData = await App.myRestManager.GetCOInfo(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    ASCTracFunctionStruct.CustOrder.CustOrderInfoType myOrderData = Newtonsoft.Json.JsonConvert.DeserializeObject<ASCTracFunctionStruct.CustOrder.CustOrderInfoType>(myReturnData.DataMessage);
                    await Navigation.PushAsync(new CODetail.pageCODetailTab(myOrderData));

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

        //=============================================================================
        async private void listContainer_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new CODetail.pageCODetailTab((ASCTracFunctionStruct.CustOrder.CustOrderInfoType)listCOs.SelectedItem));
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
