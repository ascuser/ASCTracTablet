using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.CODetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCODetailInfo : ContentPage
    {
        public pageCODetailInfo()
        {
            InitializeComponent();
            Title = "Line Info";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // DisplayData();
        }

        public void DisplayData()
        {
            if (pageCODetailTab.myCO != null)
            {
                pageCODetailTab.myOrdrDet.OrderNumber = pageCODetailTab.myCO.OrderNumber;
                BindingContext = pageCODetailTab.myOrdrDet;

                pickStatus.Items.Clear();
                pickStatus.Items.Add("O-Open");
                pickStatus.Items.Add("T-Completed");
                pickStatus.Items.Add("H-Pick Hold");
                pickStatus.Items.Add("S-Scratched");
                pickStatus.Items.Add("X-Cancelled");
                if (pageCODetailTab.myOrdrDet.OrderFilled.Equals("O"))
                    pickStatus.SelectedIndex = 0;
                if (pageCODetailTab.myOrdrDet.OrderFilled.Equals("T"))
                    pickStatus.SelectedIndex = 1;
                if (pageCODetailTab.myOrdrDet.OrderFilled.Equals("H"))
                    pickStatus.SelectedIndex = 2;
                if (pageCODetailTab.myOrdrDet.OrderFilled.Equals("S"))
                    pickStatus.SelectedIndex = 3;
                if (pageCODetailTab.myOrdrDet.OrderFilled.Equals("X"))
                    pickStatus.SelectedIndex = 4;
            }
        }

        async private void btnUpdate_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Update Line " + pageCODetailTab.myOrdrDet.LineNumber.ToString(), "Update Line Item?", "OK", "Cancel"))
            {
                myIndicator.IsRunning = true;
                myIndicator.IsVisible = true;
                string errmsg = string.Empty;
                try
                {
                    Globals.curBasicMessage.inputDataList = new List<string>();
                    Globals.curBasicMessage.inputDataList.Add("C");
                    Globals.curBasicMessage.inputDataList.Add(pageCODetailTab.myCO.OrderNumber);
                    Globals.curBasicMessage.inputDataList.Add(pageCODetailTab.myOrdrDet.LineNumber.ToString());
                    Globals.curBasicMessage.inputDataList.Add(pageCODetailTab.myOrdrDet.PCEType);
                    Globals.curBasicMessage.inputDataList.Add(pickStatus.SelectedItem.ToString().Substring(0, 1));
                    Globals.curBasicMessage.inputDataList.Add(chbClearLoc.IsToggled.ToString());
                    var myReturnData = await App.myRestManager.UpdateOrdrDet(Globals.curBasicMessage);

                    if (!myReturnData.successful)
                        errmsg = myReturnData.ErrorMessage;
                    else
                    {
                        (Parent as CODetail.pageCODetailTab).CurrentPage = (Parent as CODetail.pageCODetailTab).Children[0];
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
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            //Navigation.PopAsync();
            (Parent as pageCODetailTab).CurrentPage = (Parent as pageCODetailTab).Children[0];
        }
    }
}
