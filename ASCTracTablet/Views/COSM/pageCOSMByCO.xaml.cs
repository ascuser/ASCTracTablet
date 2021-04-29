using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.COSM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCOSMByCO : ContentPage
    {
        private ASCTracFunctionStruct.CustOrder.DockType myCO;
        public bool fRefreshFlag { get; set; }

        public pageCOSMByCO()
        {
            InitializeComponent();
            Title = "By Customer Order";
            fRefreshFlag = true;
            //listCOs.ItemSelected += ListCOs_ItemSelected;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetCOList();
        }
        async public void GetCOList()
        {
            if (COSM.ClassCOSMInput.myCOSMInput.fInit)
            {
                if (fRefreshFlag)
                {
                    listCOs.ItemsSource = null;

                    fRefreshFlag = false;
                    string errmsg = string.Empty;
                    COSM.ClassCOSMInput.myCOSMInput.fInit = false;
                    // get COSM stats for desired data.

                    myIndicator.IsRunning = true;
                    myIndicator.IsVisible = true;
                    try
                    {
                        Globals.curBasicMessage.inputDataList = new List<string>();
                        Globals.curBasicMessage.inputDataList.Add(COSM.ClassCOSMInput.myCOSMInput.statusList);
                        Globals.curBasicMessage.inputDataList.Add(COSM.ClassCOSMInput.myCOSMInput.docks);
                        Globals.curBasicMessage.inputDataList.Add(DateTime.MinValue.ToString());
                        Globals.curBasicMessage.inputDataList.Add(COSM.ClassCOSMInput.myCOSMInput.dateFieldIndex.ToString());
                        Globals.curBasicMessage.inputDataList.Add(COSM.ClassCOSMInput.myCOSMInput.dateRangeIndex.ToString());

                        var myReturnData = await App.myRestManager.GetCOStatusByCO(Globals.curBasicMessage);

                        if (!myReturnData.successful)
                            errmsg = myReturnData.ErrorMessage;
                        else
                        {
                            myIndicator.IsRunning = false;
                            myIndicator.IsVisible = false;
                            processGetCOStatusByCO(myReturnData.DataMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        errmsg = "Exception\r\n" + ex.Message;
                    }
                    if (!string.IsNullOrEmpty(errmsg))
                        await DisplayAlert(Globals.AppTitleName, errmsg, "OK");
                }
            }
        }


        async private void processGetCOStatusByCO(string aDataMessage)
        {
            string errmsg = string.Empty;
            try
            {
                var myList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.CustOrder.DockType>>(aDataMessage);

                ObservableCollection<ASCTracFunctionStruct.CustOrder.DockType> myDisplayList = new ObservableCollection<ASCTracFunctionStruct.CustOrder.DockType>();
                foreach (var rec in myList)
                    myDisplayList.Add(rec);

                lblCount.Text = "Count: " + myDisplayList.Count.ToString();
                listCOs.ItemsSource = myDisplayList;
            }
            catch (Exception ex)
            {
                errmsg = "Exception :" + ex.Message;
            }
            if (!string.IsNullOrEmpty(errmsg))
                await DisplayAlert(Globals.AppTitleName, errmsg, "OK");
        }

        async public void OnListItemSelected(object sender, EventArgs e)
        {
            var errmsg = string.Empty;
            myCO = (ASCTracFunctionStruct.CustOrder.DockType)listCOs.SelectedItem;
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(myCO.OrderNumber);
                var myReturnData = await App.myRestManager.GetCOInfo(Globals.curBasicMessage); // GetCOInfo

                if (!myReturnData.successful)
                    await DisplayAlert("ASCTrac", myReturnData.ErrorMessage, "OK");
                else
                {

                    myIndicator.IsRunning = false;
                    myIndicator.IsVisible = false;
                    var myCOInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<ASCTracFunctionStruct.CustOrder.CustOrderInfoType>(myReturnData.DataMessage);
                    if (myCOInfo.DetailXMLInfo.StartsWith("OK"))
                    {
                        try
                        {
                            fRefreshFlag = true;
                            Globals.myDatabase.fillOrdrDet(myCOInfo.DetailXMLInfo.Substring(2));
                            await Navigation.PushAsync(new Views.CODetail.pageCODetailTab(myCOInfo));
                        }
                        catch (Exception ex1)
                        {
                            await DisplayAlert("ASCTrac", ex1.Message + "\r\n" + myCOInfo.DetailXMLInfo, "OK");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("ASCTrac", ex.ToString(), "OK");
            }
        }
        

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            (Parent as COSM.pageCOSMTab).CurrentPage = (Parent as COSM.pageCOSMTab).Children[0];
        }
    }
}

