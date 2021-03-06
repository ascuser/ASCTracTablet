using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.Maintenances
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageItemMaint : ContentPage
    {
        ASCTracFunctionStruct.MaintType myMaintType;

        public pageItemMaint()
        {
            InitializeComponent();
            myIndicator.IsEnabled = false;
            myIndicator.IsVisible = false;
            myIndicator.IsRunning = false;
            btnFind.IsVisible = true;

            myIndicator.IsRunning = true;

        }

        private void EdItemID_Completed(object sender, EventArgs e)

        {
            GetItemInfo();

        }

        private void BtnFind_Clicked(object sender, EventArgs e)
        {
            GetItemInfo();
        }

        async private void GetItemInfo()
        {
            if (!String.IsNullOrEmpty(edItemID.Text))
            {
                myIndicator.IsEnabled = true;
                myIndicator.IsVisible = true;
                myIndicator.IsRunning = true;
                try
                {
                    Globals.curBasicMessage.DataMessage = "I|" + edItemID.Text;

                    var myReturnData = await App.myRestManager.GetMaintInfo(Globals.curBasicMessage);

                    if (!myReturnData.successful)
                        await DisplayAlert(Globals.AppTitleName, myReturnData.ErrorMessage, "OK");
                    else
                    {
                        myMaintType = Newtonsoft.Json.JsonConvert.DeserializeObject<ASCTracFunctionStruct.MaintType>(myReturnData.DataMessage);
                        BindingContext = myMaintType;

                        listEdits.ItemsSource = myMaintType.myMaintFields;
                        btnUpdate.IsEnabled = true;
                    }
                }
                catch (Exception ex)
                {

                    await DisplayAlert(Globals.AppTitleName, "Exception :" + ex.ToString(), "OK");
                }
                myIndicator.IsVisible = false;
                myIndicator.IsRunning = false;
            }
        }

        async private void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            Globals.curBasicMessage.DataMessage = Newtonsoft.Json.JsonConvert.SerializeObject(myMaintType);

            myIndicator.IsEnabled = true;
            myIndicator.IsVisible = true;
            myIndicator.IsRunning = true;
            try
            {

                var myReturnData = await App.myRestManager.SetMaintInfo(Globals.curBasicMessage);
                if( myReturnData.successful)
                {
                    listEdits.ItemsSource = null;
                    BindingContext = null;
                    myMaintType = null;

                    btnUpdate.IsEnabled = false;
                    edItemID.Text = string.Empty;
                    edItemID.Focus();
                }
                else
                    await DisplayAlert(Globals.AppTitleName, myReturnData.ErrorMessage, "OK");
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
    }

}
