using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.Replen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageReplenLocList : ContentPage
    {
        ASCTracFunctionStruct.Inventory.ReplenSummType myZone;
        public pageReplenLocList(ASCTracFunctionStruct.Inventory.ReplenSummType aZone)
        {
            InitializeComponent();
            try
            {
                myZone = aZone;

                myIndicator.IsVisible = true;
                myIndicator.IsEnabled = true;

                Title = "Zone " + myZone.ZoneID; ;

                BindingContext = myZone;
                GetLocations();
            }
            catch (Exception ex)
            {
                DisplayAlert(Globals.AppTitleName, "Page Create\r\n" + ex.Message, "OK");
            }
        }

        private void BtnRefresh_Clicked(object sender, EventArgs e)
        {
            GetLocations();
        }

        async private void GetLocations()
        {
            string errmsg = string.Empty;
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(myZone.ZoneID);
                Globals.curBasicMessage.inputDataList.Add(pageReplenSummary.fReplenFilterType);
                var myReturnData = await App.myRestManager.GetReplenInfoForZone(Globals.curBasicMessage);

                myIndicator.IsRunning = false;
                myIndicator.IsVisible = false;
                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    var myList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.Inventory.ReplenLocType>>(myReturnData.DataMessage);
                    listReplenLocs.ItemsSource = myList;
                    lblCount.Text = myList.Count.ToString();

                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            if (!String.IsNullOrEmpty(errmsg))
            {
                await DisplayAlert("GetReplen Exception", errmsg, "OK");
                await Navigation.PopAsync();
            }

        }


        private void ListReplenLocs_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            ((pageReplenTab)this.Parent).CurrentPage = ((pageReplenTab)this.Parent).Children[0];
        }
    }
}
