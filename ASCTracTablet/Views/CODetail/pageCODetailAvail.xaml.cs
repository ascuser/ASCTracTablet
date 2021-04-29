using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.CODetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCODetailAvail : ContentPage
    {
        public pageCODetailAvail()
        {
            InitializeComponent();
            Title = "Available";
        }

        public void RefreshData()
        {
            if (pageCODetailTab.myCO != null)
            {
                pageCODetailTab.myOrdrDet.OrderNumber = pageCODetailTab.myCO.OrderNumber;
                BindingContext = pageCODetailTab.myOrdrDet;
                //lblOrderNumber.Text = pageCODetailTab.myCO.OrderNumber;
                //lblLineNum.Text = pageCODetailTab.myOrdrDet.LineNumber.ToString();
                //lblItemID.Text = pageCODetailTab.myOrdrDet.ItemID;
                //lblItemDescription.Text = pageCODetailTab.myOrdrDet.Description;
            }
            listAvail.ItemsSource = null;
        }

        async private void btnRefresh_Clicked(object sender, EventArgs e)
        {
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            string errmsg = string.Empty;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(pageCODetailTab.myOrderNumber);
                Globals.curBasicMessage.inputDataList.Add(pageCODetailTab.myOrdrDet.LineNumber.ToString());
                Globals.curBasicMessage.inputDataList.Add(chbIncludeQC.IsToggled.ToString());
                Globals.curBasicMessage.inputDataList.Add(chbIncludeExpired.IsToggled.ToString());
                
                var myReturnData = await App.myRestManager.GetInvAvail(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    Globals.myDatabase.fillInventory(myReturnData.DataMessage.Substring(2));
                    var mylist = Globals.myDatabase.GetLocItems().ToList<DataModel.dataLocItems>(); //
                    if (mylist.Count == 0)
                        await DisplayAlert("ASCTrac", "No Available Inventory", "OK");
                    listAvail.ItemsSource = mylist;
                    lblCount.Text = mylist.Count.ToString();
                }
                
            }
            catch (Exception ex)
            {
                errmsg = "Exception\r\n" + ex.Message;
            }
            if (!String.IsNullOrEmpty(errmsg))
                await DisplayAlert("Refresh Data", errmsg, "OK");
            myIndicator.IsRunning = false;
            myIndicator.IsVisible = false;
        }

        private void listAvail_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            // Navigation.PopAsync();
            (Parent as pageCODetailTab).CurrentPage = (Parent as pageCODetailTab).Children[0];
        }
    }
}
