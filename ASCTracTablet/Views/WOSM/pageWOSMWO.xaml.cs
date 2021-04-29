using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.WOSM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageWOSMWO : ContentPage
    {
        public pageWOSMWO()
        {
            InitializeComponent();
            Title = "WO Details";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetWOList();
        }

        async public void GetWOList()
        {
            // send packet to get data.
            if (WOSM.ClassWOSMInput.myWOSMInput.fInit)
            {
                string errmsg = string.Empty;
                myIndicator.IsRunning = true;
                myIndicator.IsVisible = true;
                try
                {
                    Globals.curBasicMessage.inputDataList = new List<string>();
                    Globals.curBasicMessage.inputDataList.Add(WOSM.ClassWOSMInput.myWOSMInput.statusList);
                    Globals.curBasicMessage.inputDataList.Add(WOSM.ClassWOSMInput.myWOSMInput.prodlines);
                    Globals.curBasicMessage.inputDataList.Add(WOSM.ClassWOSMInput.myWOSMInput.dateFieldIndex.ToString());
                    Globals.curBasicMessage.inputDataList.Add(WOSM.ClassWOSMInput.myWOSMInput.dateRangeIndex.ToString());
                    var myReturnData = await App.myRestManager.GetWOStatusByWO(Globals.curBasicMessage);

                    myIndicator.IsRunning = false;
                    myIndicator.IsVisible = false;
                    if (!myReturnData.successful)
                        errmsg = myReturnData.ErrorMessage;
                    else
                    {
                        var myList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.Production.WOHdrType>>(myReturnData.DataMessage);
                        listWOs.ItemsSource = myList;
                    }
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }
                if (!String.IsNullOrEmpty(errmsg))
                {
                    await DisplayAlert("Get WO Summary Exception", errmsg, "OK");
                }
            }
        }
        

        async public void OnListItemSelected(object sender, EventArgs e)
        {
            var myWO = (ASCTracFunctionStruct.Production.WOHdrType)listWOs.SelectedItem;
            await Navigation.PushAsync(new Views.Production.pageWOTab(myWO, myWO.Prodline));
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            (Parent as pageWOSMTab).CurrentPage = (Parent as pageWOSMTab).Children[0];
        }
    }
}

