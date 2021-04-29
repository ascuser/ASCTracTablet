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
    public partial class pageInvLookupGrid : ContentPage
    {
        string fItemID;
        string fFieldType;
        int iFieldType;
        string fFieldData;
        bool fQC;
        bool fExpire;
        bool fPicked;

        public pageInvLookupGrid(string aItemID, bool aQC, bool aExpire, bool aPicked, string aFieldType, int aiFieldType, string aFieldData)
        {
            InitializeComponent();
            fItemID = aItemID;
            fQC = aQC;
            fExpire = aExpire;
            fPicked = aPicked;
            fFieldType = aFieldType;
            iFieldType = aiFieldType;
            fFieldData = aFieldData;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ShowData();

            lblItemID.Text = fItemID;
            if (String.IsNullOrEmpty(fFieldData))
            {
                lblFieldType.Text = string.Empty;
                lblFieldData.Text = string.Empty;
            }
            else
            {
                lblFieldType.Text = fFieldType;
                lblFieldData.Text = fFieldData;
            }
        }

        private void ShowData()
        {
            var mylist = Globals.myDatabase.GetLocItems().ToList<DataModel.dataLocItems>();
            if (mylist.Count == 0)
                DisplayAlert("ASCTrac", "No Available Inventory", "OK");
            else
            {
                lblItemDescription.Text = mylist[0].ItemDescription;
                listAvail.ItemsSource = mylist;
                lblCount.Text = mylist.Count.ToString();
            }
        }

        async private void btnRefresh_Clicked(object sender, EventArgs e)
        {
            myIndicator.IsEnabled = true;
            myIndicator.IsVisible = true;
            myIndicator.IsRunning = true;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(fItemID);
                Globals.curBasicMessage.inputDataList.Add(fQC.ToString());
                    Globals.curBasicMessage.inputDataList.Add(fExpire.ToString());
                    Globals.curBasicMessage.inputDataList.Add(fPicked.ToString());
                    Globals.curBasicMessage.inputDataList.Add(iFieldType.ToString());
                    Globals.curBasicMessage.inputDataList.Add(fFieldData);



                var myReturnData = await App.myRestManager.GetInvList(Globals.curBasicMessage);
                myIndicator.IsVisible = false;
                myIndicator.IsRunning = false;

                if (!myReturnData.successful)
                    await DisplayAlert(Globals.AppTitleName, myReturnData.ErrorMessage, "OK");
                else
                {
                    Globals.myDatabase.fillInventory(myReturnData.DataMessage.Substring(2));
                    var mylist = Globals.myDatabase.GetLocItems().ToList<DataModel.dataLocItems>();
                    if (mylist.Count == 0)
                        await DisplayAlert("ASCTrac", "No Available Inventory", "OK");
                    else
                    {
                        ShowData();
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(Globals.AppTitleName, "Exception :" + ex.ToString(), "OK");
            }
            myIndicator.IsVisible = false;
            myIndicator.IsRunning = false;
        }


        async private void listAvail_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            DataModel.dataLocItems rec = (DataModel.dataLocItems)listAvail.SelectedItem;
            myIndicator.IsEnabled = true;
            myIndicator.IsVisible = true;
            myIndicator.IsRunning = true;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(rec.SkidID);
                Globals.curBasicMessage.inputDataList.Add(rec.ItemID);
                Globals.curBasicMessage.inputDataList.Add(rec.LocationID);



                var myReturnData = await App.myRestManager.GetSkidHistory(Globals.curBasicMessage);
                myIndicator.IsVisible = false;
                myIndicator.IsRunning = false;

                if (!myReturnData.successful)
                    await DisplayAlert(Globals.AppTitleName, myReturnData.ErrorMessage, "OK");
                else
                {
                    var myList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.Inventory.InvHistoryType>>(myReturnData.DataMessage);
                    if (myList.Count == 0)
                        await DisplayAlert("ASCTrac", "No Records Found", "OK");
                    else
                    {
                        await Navigation.PushAsync(new invLookup.pageInvDetailTab(rec, myList));
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(Globals.AppTitleName, "Exception :" + ex.ToString(), "OK");
            }
            myIndicator.IsVisible = false;
            myIndicator.IsRunning = false;
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        async private void onCount(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            var myRec = item.BindingContext as DataModel.dataLocItems;
            await Navigation.PushAsync(new InvFunctions.pageInvCount(myRec.SkidID, "Cycle Count"));
        }

        async private void onLabel(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            var myRec = item.BindingContext as DataModel.dataLocItems;
            await Navigation.PushAsync(new InvFunctions.pageInvCount(myRec.SkidID, "Label"));

        }

        async private void onHold(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            var myRec = item.BindingContext as DataModel.dataLocItems;
            await Navigation.PushAsync(new Views.QCHold.pageQCHold(myRec.SkidID));

        }
    }
}
