using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.Receipt
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageExpRecvStatusPOList : ContentPage
    {
        public pageExpRecvStatusPOList()
        {
            InitializeComponent();
            Title = "Expected Receipt Details";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetPOList();
        }


        async public void GetPOList()
        {
            if (ClassExpRecvInput.myExpRecvStatusInput.fInit)
            {
                myIndicator.IsRunning = true;
                myIndicator.IsVisible = true;
                try
                {
                    Globals.curBasicMessage.inputDataList.Clear();
                    Globals.curBasicMessage.inputDataList.Add(ClassExpRecvInput.myExpRecvStatusInput.statusList);
                    Globals.curBasicMessage.inputDataList.Add(ClassExpRecvInput.myExpRecvStatusInput.dateFieldIndex.ToString());
                    Globals.curBasicMessage.inputDataList.Add(ClassExpRecvInput.myExpRecvStatusInput.dateRangeIndex.ToString());
                    var myReturnData = await App.myRestManager.GetPOStatusByPO(Globals.curBasicMessage);

                    if (myReturnData.successful)
                    {
                        processGetPOStatusByPO(myReturnData.DataMessage);

                    }
                    else
                        await DisplayAlert("ASCTrac-Recv Metrics", "Error :" + myReturnData.ErrorMessage, "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("ASCTrac-Recv Metrics", "Exception :" + ex.Message, "OK");
                }
                myIndicator.IsRunning = false;
                myIndicator.IsVisible = false;
            }
        }

        async private void processGetPOStatusByPO(string aDataMessage)
        {
            string errmsg = string.Empty;
            try
            {
                var myList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.Receipt.ConfReceiptType>>(aDataMessage);

                ObservableCollection<ASCTracFunctionStruct.Receipt.ConfReceiptType> myDisplayList = new ObservableCollection<ASCTracFunctionStruct.Receipt.ConfReceiptType>();
                foreach (var rec in myList)
                    myDisplayList.Add(rec);

                listPOs.ItemsSource = myDisplayList;
                lblCount.Text = "Count: " + myDisplayList.Count.ToString();
            }
            catch (Exception ex)
            {
                errmsg = "Exception :" + ex.Message;
            }
            if (!string.IsNullOrEmpty(errmsg))
                await DisplayAlert(Globals.AppTitleName, errmsg, "OK");

        }

        async public void OnListItemSelected(object sender, EventArgs e)
        {
            var myPO = (ASCTracFunctionStruct.Receipt.ConfReceiptType)listPOs.SelectedItem;
            await Navigation.PushAsync(new pageExpRecvPOInfo(myPO));
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            (Parent as pageExpRecvStatusTab).CurrentPage = (Parent as pageExpRecvStatusTab).Children[0];
        }

    }
}