using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.COEntry
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCOEntry : TabbedPage
    {
        public static ASCTracFunctionStruct.COEntry.COEntryHdr myCOHdr = null; // = new ASCTracFunctionStruct.COEntry.COEntryHdr("");
        public static DataModel.COEntrySetupType setupData = new DataModel.COEntrySetupType();

        public pageCOEntry(string aOrderNumber)
        {
            try
            {
                InitializeComponent();

                Title = "Order Entry";
                Children.Add(new pageCOEntryHdr());
                Children.Add(new pageCOEntryDet());
                myCOHdr = null;
                (Children[0] as pageCOEntryHdr).Init(aOrderNumber);
            }
            catch (Exception ex)
            {
                DisplayAlert("Order Entry Create Exception", ex.Message, "OK");
            }
        }

        async public void GetOrder(string aOrderNumber)
        {
            (Children[0] as pageCOEntryHdr).SetIndicator(true);
            (Children[1] as pageCOEntryDet).SetIndicator(true);
            string OrderNumber = aOrderNumber;
            if (myCOHdr != null)
                OrderNumber = myCOHdr.OrderNumber;

            string errmsg = string.Empty;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(OrderNumber);
                var myReturnData = await App.myRestManager.GetCOEntryInfo(Globals.curBasicMessage);
                (Children[0] as pageCOEntryHdr).SetIndicator(false);
                (Children[1] as pageCOEntryDet).SetIndicator(false);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    var tmpHdr = Newtonsoft.Json.JsonConvert.DeserializeObject<ASCTracFunctionStruct.COEntry.COEntryHdr>(myReturnData.DataMessage);
                    if (myCOHdr != null)
                    {
                        if (tmpHdr.OrderNumber != myCOHdr.OrderNumber)
                            await DisplayAlert("ASCTrac", "Order Number changed on CO Entry Info Call", "OK");
                    }
                    myCOHdr = tmpHdr;
                    (Children[0] as pageCOEntryHdr).SetupHeader();
                    (Children[1] as pageCOEntryDet).SetupDetail();

                    if (!pageCOEntry.setupData.fExist || (Globals.lookupCustList == null) || (Globals.lookupCustList.Count == 0))
                    {
                        GetSetupData(OrderNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                errmsg = "Exception\r\n" + ex.Message;
            }
                (Children[0] as pageCOEntryHdr).SetIndicator(false);
            (Children[1] as pageCOEntryDet).SetIndicator(false);
            if (!String.IsNullOrEmpty(errmsg))
                await DisplayAlert("GetOrder Exception", errmsg, "OK");
        }
        
        public bool checkCompleteOrder()
        {
            bool retval = true;
            if ((myCOHdr == null) || (String.IsNullOrEmpty(myCOHdr.OrderNumber) || myCOHdr.DetailList.Count == 0))
                retval = false;
            else
                if (DisplayAlert("Order " + myCOHdr.OrderNumber, "Complete Order Entry?", "Yes", "No").Result.Equals(false))
                retval = false;
            else
                CompleteOrder();

            return (retval);
        }

        async public void CompleteOrder()
        {
            (Children[0] as pageCOEntryHdr).SetIndicator(true);
            (Children[1] as pageCOEntryDet).SetIndicator(true);

            string errmsg = string.Empty;
            try
            {
                var myReturnData = await App.myRestManager.CompleteCOEntry(Globals.curBasicMessage);
                (Children[0] as pageCOEntryHdr).SetIndicator(false);
                (Children[1] as pageCOEntryDet).SetIndicator(false);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                }
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                errmsg = "Exception\r\n" + ex.Message;
            }
                (Children[0] as pageCOEntryHdr).SetIndicator(false);
            (Children[1] as pageCOEntryDet).SetIndicator(false);
            if (!String.IsNullOrEmpty(errmsg))
                await DisplayAlert("CompleteOrder Exception", errmsg, "OK");
        }

        async public void GetSetupData(string aOrderNumber)
        {
            (Children[0] as pageCOEntryHdr).SetIndicator(true);
            (Children[1] as pageCOEntryDet).SetIndicator(true);
            string OrderNumber = aOrderNumber;
            if (myCOHdr != null)
                OrderNumber = myCOHdr.OrderNumber;

            string errmsg = string.Empty;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(setupData.custType);
                Globals.curBasicMessage.inputDataList.Add(setupData.custTypeData);
                Globals.curBasicMessage.inputDataList.Add(setupData.itemType);
                Globals.curBasicMessage.inputDataList.Add(setupData.itemTypeData);
                var myReturnData = await App.myRestManager.GetUsersCOEntryInfo(Globals.curBasicMessage);
                (Children[0] as pageCOEntryHdr).SetIndicator(false);
                (Children[1] as pageCOEntryDet).SetIndicator(false);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    var tmpHdr = Newtonsoft.Json.JsonConvert.DeserializeObject<ASCTracFunctionStruct.COEntry.COEntryUserInfo>(myReturnData.DataMessage);
                    Globals.lookupCustList = tmpHdr.custList;
                    Globals.lookupItemIDList = tmpHdr.itemIDList;
                }
            }
            catch (Exception ex)
            {
                errmsg = "Exception\r\n" + ex.Message;
            }
                (Children[0] as pageCOEntryHdr).SetIndicator(false);
            (Children[1] as pageCOEntryDet).SetIndicator(false);
            if (!String.IsNullOrEmpty(errmsg))
                await DisplayAlert("GetSetup Exception", errmsg, "OK");
        }

    }
}