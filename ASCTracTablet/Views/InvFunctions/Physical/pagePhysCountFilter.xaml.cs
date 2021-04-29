using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.InvFunctions.Physical
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pagePhysCountFilter : ContentPage
    {
        public pagePhysCountFilter()
        {
            InitializeComponent();
            Title = "Filter";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetCounts();
        }
        async public void GetCounts()
        {
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;

            string errmsg = string.Empty;
            try
            {

                var myReturnData = await App.myRestManager.GetCounts(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    var mylist = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(myReturnData.DataMessage);
                    if (mylist.Count == 0)
                        errmsg = "No Counts Returned";
                    else
                    {
                        cbPickCountID.Items.Clear();
                        foreach (var rec in mylist)
                        {
                            cbPickCountID.Items.Add(rec.Key + "-" + rec.Value);
                        }

                        cbPickCountID.SelectedIndex = 0;

                        chbLocCounted.IsToggled = Globals.myDatabase.GetConfigValue("PHYS_LOCCOUNTED", "T").Equals("T");
                        chbLocUncounted.IsToggled = Globals.myDatabase.GetConfigValue("PHYS_LOCUNCOUNTED", "F").Equals("T");
                        chbLocReviewed.IsToggled = Globals.myDatabase.GetConfigValue("PHYS_LOCREVIEWED", "F").Equals("T");
                        chbAllInv.IsToggled = Globals.myDatabase.GetConfigValue("PHYS_ALLINV", "F").Equals("T");
                        chbQtyVariance.IsToggled = Globals.myDatabase.GetConfigValue("PHYS_QTYVARIANCE", "T").Equals("T");
                        chbLocVariance.IsToggled = Globals.myDatabase.GetConfigValue("PHYS_LOCVARIANCE", "T").Equals("T");
                        chbLocEmpty.IsToggled = Globals.myDatabase.GetConfigValue("PHYS_LOCEMPTY", "F").Equals("T");
                        Globals.myDatabase.SaveBoolConfig("PHYS_ALLINV", chbAllInv.IsToggled);
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

        private void cbPickCountID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Clicked(object sender, EventArgs e)
        {
            Globals.myDatabase.SaveBoolConfig("PHYS_LOCCOUNTED", chbLocCounted.IsToggled);
            Globals.myDatabase.SaveBoolConfig("PHYS_LOCUNCOUNTED", chbLocUncounted.IsToggled);
            Globals.myDatabase.SaveBoolConfig("PHYS_LOCREVIEWED", chbLocReviewed.IsToggled);

            Globals.myDatabase.SaveBoolConfig("PHYS_ALLINV", chbAllInv.IsToggled);
            Globals.myDatabase.SaveBoolConfig("PHYS_QTYVARIANCE", chbQtyVariance.IsToggled);
            Globals.myDatabase.SaveBoolConfig("PHYS_LOCVARIANCE", chbLocVariance.IsToggled);
            Globals.myDatabase.SaveBoolConfig("PHYS_LOCEMPTY", chbLocEmpty.IsToggled);
            doRefresh();
        }

        async public void doRefresh()
        {
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            pagePhysCountMain.countID = ascUtils.GetIDFromPicker(cbPickCountID);

            string errmsg = string.Empty;
            try
            {
                /*
                 * pagePhysCountMain.countID, edLocStart.Text, edLocEnd.Text, edItemStart.Text, edItemEnd.Text,
                chbLocCounted.IsToggled, chbLocUncounted.IsToggled, chbLocReviewed.IsToggled,
                chbAllInv.IsToggled,
                chbQtyVariance.IsToggled, chbLocVariance.IsToggled, chbLocEmpty.IsToggled,
                 */
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(pagePhysCountMain.countID);
                Globals.curBasicMessage.inputDataList.Add(edLocStart.Text);
                Globals.curBasicMessage.inputDataList.Add(edLocEnd.Text); // item
                Globals.curBasicMessage.inputDataList.Add(edItemStart.Text); // review only
                Globals.curBasicMessage.inputDataList.Add(edItemEnd.Text);
                Globals.curBasicMessage.inputDataList.Add(chbLocCounted.IsToggled.ToString());
                Globals.curBasicMessage.inputDataList.Add(chbLocUncounted.IsToggled.ToString());
                Globals.curBasicMessage.inputDataList.Add(chbLocReviewed.IsToggled.ToString());
                Globals.curBasicMessage.inputDataList.Add(chbAllInv.IsToggled.ToString());
                Globals.curBasicMessage.inputDataList.Add(chbQtyVariance.IsToggled.ToString());
                Globals.curBasicMessage.inputDataList.Add(chbLocVariance.IsToggled.ToString());
                Globals.curBasicMessage.inputDataList.Add(chbLocEmpty.IsToggled.ToString());


                var myReturnData = await App.myRestManager.GetPhysLocs(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    var mylist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.Inventory.PhysCountLocType>>(myReturnData.DataMessage);
                    if (mylist.Count == 0)
                        errmsg = "No Locations Returned";
                    else
                    {
                        (Parent as pagePhysCountMain).ShowLocList(mylist);
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


        private void chbAllInv_Toggled(object sender, ToggledEventArgs e)
        {
            chbQtyVariance.IsEnabled = !chbAllInv.IsToggled;
            chbLocVariance.IsEnabled = !chbAllInv.IsToggled;
            chbLocEmpty.IsEnabled = !chbAllInv.IsToggled;
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
