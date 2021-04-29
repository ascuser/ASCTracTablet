using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.Production
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageProdGetWO : ContentPage
    {
        List<ASCTracFunctionStruct.Production.WOHdrType> myList;
        public pageProdGetWO()
        {
            InitializeComponent();
        }

        async private void btnGo_Clicked(object sender, EventArgs e)
        {
            string errmsg = string.Empty;
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(edWO.Text);
                var myReturnData = await App.myRestManager.GetWOHdrInfo(Globals.curBasicMessage);

                myIndicator.IsRunning = false;
                myIndicator.IsVisible = false;
                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    var myrec = Newtonsoft.Json.JsonConvert.DeserializeObject<ASCTracFunctionStruct.Production.WOHdrType>(myReturnData.DataMessage);
                    await Navigation.PushAsync(new pageWOTab(myrec, myrec.Prodline));
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


        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
