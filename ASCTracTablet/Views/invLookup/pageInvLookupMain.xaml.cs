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
    public partial class pageInvLookupMain : ContentPage
    {
        // aLookupTtype, "R"=Receiving Dock, I=Standard Inv
        private string fLookupType;
        // aLookupTtype, "R"=Receiving Dock, I=Standard Inv
        public pageInvLookupMain(string aLookupType)
        {
            InitializeComponent();
            fLookupType = aLookupType;

            if (fLookupType.Equals("R"))
            {
                lblTitle.Text = "Inventory not Putaway";
                pickFieldType.IsVisible = false;
                edFieldValue.IsVisible = false;
                chbExpire.IsEnabled = false;
                chbPicked.IsEnabled = false;
            }
            else
            {
                pickFieldType.Items.Clear();
                pickFieldType.Items.Add("License");
                pickFieldType.Items.Add("Location");
                pickFieldType.Items.Add("LotID");
                pickFieldType.Items.Add("Alt LotID");
                pickFieldType.Items.Add("Expiration Date");
                pickFieldType.Items.Add("Inventory Container");
                pickFieldType.Items.Add("Pick Container");
                pickFieldType.Items.Add("Order Number");
                pickFieldType.Items.Add("Serial Number");

                pickFieldType.SelectedIndex = 0;
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
                Globals.curBasicMessage.inputDataList.Add(edItem.Text);
                Globals.curBasicMessage.inputDataList.Add(chbQC.IsToggled.ToString());
                if (fLookupType.Equals("R"))
                {
                    Globals.curBasicMessage.inputDataList.Add("True");
                    Globals.curBasicMessage.inputDataList.Add("True");
                    Globals.curBasicMessage.inputDataList.Add("100");
                    Globals.curBasicMessage.inputDataList.Add(fLookupType);
                    //myclient.GetInvListAsync(edItem.Text, chbQC.IsToggled, true, true, 100, fLookupType, Globals.curBasicMessage);
                }
                else
                {
                    Globals.curBasicMessage.inputDataList.Add(chbExpire.IsToggled.ToString());
                    Globals.curBasicMessage.inputDataList.Add(chbPicked.IsToggled.ToString());
                    Globals.curBasicMessage.inputDataList.Add(pickFieldType.SelectedIndex.ToString());
                    Globals.curBasicMessage.inputDataList.Add(edFieldValue.Text);

                    //myclient.GetInvListAsync(edItem.Text, chbQC.IsToggled, chbExpire.IsToggled, chbPicked.IsToggled, pickFieldType.SelectedIndex, edFieldValue.Text, Globals.curBasicMessage);
                }


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
                        string fFieldType = string.Empty;
                        if (fLookupType.Equals("R"))
                            fFieldType = "Location Type";
                        else
                            fFieldType = pickFieldType.Items[pickFieldType.SelectedIndex];

                        await Navigation.PushAsync(new invLookup.pageInvLookupGrid(edItem.Text, chbQC.IsToggled, chbExpire.IsToggled, chbPicked.IsToggled, fFieldType, pickFieldType.SelectedIndex, edFieldValue.Text));
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
    }
}
