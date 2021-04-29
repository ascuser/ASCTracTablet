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
    public partial class pageReplenSummary : ContentPage
    {
        internal static string fReplenFilterType = "N";
        public pageReplenSummary()
        {
            InitializeComponent();
            myIndicator.IsVisible = true;
            myIndicator.IsEnabled = true;

            Title = "Summary";
        }

        private string GetFilterType()
        {
            string filterType = "N";
            if (pickFilter.SelectedIndex == 1)
                filterType = "S";
            if (pickFilter.SelectedIndex == 2)
                filterType = "O";
            return (filterType);
        }

       async private void BtnRefresh_Clicked(object sender, EventArgs e)
        {
            fReplenFilterType = GetFilterType();

            string errmsg = string.Empty;
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(edZone.Text);
                Globals.curBasicMessage.inputDataList.Add(pageReplenSummary.fReplenFilterType);
                var myReturnData = await App.myRestManager.GetReplenSummary(Globals.curBasicMessage);

                myIndicator.IsRunning = false;
                myIndicator.IsVisible = false;
                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    var myList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.Inventory.ReplenSummType>>(myReturnData.DataMessage);
                    if (myList.Count == 0)
                        await DisplayAlert("ASCTrac-Replen Summary", "No Records Returned", "OK");
                    listReplenSummary.ItemsSource = myList;
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


        private void ListReplenSummary_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var rec = (ASCTracFunctionStruct.Inventory.ReplenSummType)listReplenSummary.SelectedItem;

            ((pageReplenTab)this.Parent).SelectZone(rec);
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

    }
}