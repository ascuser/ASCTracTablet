using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.COContainer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCOContainerLookup : ContentPage
    {
        string fCntrType = string.Empty;
        string fCntrDesc = "Tote";
        // aCntrType P = Parcel, T=Tote/Containr table, V=Vessel/Trailer
        public pageCOContainerLookup(string aCntrType)
        {
            InitializeComponent();
            fCntrType = aCntrType;
            SetupForm();
        }

        private void SetupForm()
        {
            if (fCntrType.Equals("P"))
                fCntrDesc = "Parcel";
            if (fCntrType.Equals("V"))
                fCntrDesc = "Vessel";
            lblContainerType.Text = fCntrDesc + ":";
            lblContainerGridLabel.Text = fCntrDesc;
            lblContainerStatusGridLabel.Text = fCntrDesc + " Status";
            lblTitle.Text = fCntrDesc + " Lookup";
        }

       async private void GetList()
        {
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;

            string errmsg = string.Empty;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(edCONumber.Text);
                Globals.curBasicMessage.inputDataList.Add(edContainer.Text);
                Globals.curBasicMessage.inputDataList.Add(fCntrType);

                //myClient.GetOrderContainerLookupAsync(edCONumber.Text, edContainer.Text, fCntrType, Globals.curBasicMessage);
                var myReturnData = await App.myRestManager.GetContainerList(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    var myList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.CustOrder.OrderContainerLookupInfo>>(myReturnData.DataMessage);

                    if (myList.Count == 0)
                        errmsg = "No Records Returned";
                    else
                        listContainer.ItemsSource = myList;
                    lblCount.Text = myList.Count.ToString();
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


        private void edCONumber_Completed(object sender, EventArgs e)
        {
            GetList();
        }

        async private void listContainer_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var myRec = (ASCTracFunctionStruct.CustOrder.OrderContainerLookupInfo)e.Item;

                if (fCntrType.Equals("P"))
                    await Navigation.PushAsync(new pageCOParcelInfo(myRec));
                else if (fCntrType.Equals("V"))
                    await Navigation.PushAsync(new pageCOVesselInfo(myRec));
                else
                    await Navigation.PushAsync(new pageCOToteInfo(myRec));
            }
        }

        private void btnRefresh_Clicked(object sender, EventArgs e)
        {
            GetList();
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
