using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.Receipt
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pagePOLookup : ContentPage
    {
        public pagePOLookup()
        {
            InitializeComponent();
            FillPickers();
        }

        private void FillPickers()
        {
            pickCustType.Items.Clear();
            pickCustType.Items.Add("Vendor ID");
            pickCustType.Items.Add("Vendor Name");
            pickCustType.SelectedIndex = 0;

            pickFilterType.Items.Clear();
            pickFilterType.Items.Add("Expected Date");
            pickFilterType.Items.Add("Carrier");
            pickFilterType.Items.Add("Dock");
            pickFilterType.Items.Add("Status");
            pickFilterType.Items.Add("Trailer");
            pickFilterType.Items.Add("Order Type");
            pickFilterType.SelectedIndex = 0;

        }


        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
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
                doRefreshOrder();
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


                var myReturnData = await App.myRestManager.GetPOList(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    var mylist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.Receipt.ConfReceiptType>>(myReturnData.DataMessage);
                    if (mylist.Count == 0)
                        errmsg = "No Orders Returned";
                    else
                    {
                        var myList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.Receipt.ConfReceiptType>>(myReturnData.DataMessage);
                        listPOs.ItemsSource = myList;
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
        async private void doRefreshOrder()
        {

            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;

            string errmsg = string.Empty;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(edCONumber.Text.ToUpper());


                var myReturnData = await App.myRestManager.GetPOInfo(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    var myOrderData = Newtonsoft.Json.JsonConvert.DeserializeObject<ASCTracFunctionStruct.Receipt.ConfReceiptType>(myReturnData.DataMessage);
                    await Navigation.PushAsync(new Receipt.pageExpRecvPOInfo(myOrderData)); // CODetail.pageCODetailTab(myOrderData));

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

        async private void listContainer_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new Receipt.pageExpRecvPOInfo((ASCTracFunctionStruct.Receipt.ConfReceiptType)listPOs.SelectedItem));

        }
    }
}