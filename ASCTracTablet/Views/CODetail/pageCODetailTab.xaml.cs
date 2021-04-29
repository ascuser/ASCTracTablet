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
    public partial class pageCODetailTab : TabbedPage
    {
        public static string myOrderNumber;
        public static ASCTracFunctionStruct.CustOrder.CustOrderInfoType myCO;
        public static DataModel.dataOrdrDet myOrdrDet;
        private bool fInit = false;

        public pageCODetailTab(string aOrderNumber)
        {
            InitializeComponent();
            myCO = null;
            myOrderNumber = aOrderNumber;
            Title = "Customer Order " + myOrderNumber;

            mnuPicklist.Clicked += MnuPicklist_Clicked;
            mnuPacklist.Clicked += MnuPacklist_Clicked;
            mnuBOL.Clicked += MnuBOL_Clicked;

            Children.Add(new pageCODetailList());
            Children.Add(new pageCODetailInfo());
            Children.Add(new pageCODetailAvail());
            fInit = true;

            (Children[0] as pageCODetailList).RefreshData();
        }


        public pageCODetailTab(ASCTracFunctionStruct.CustOrder.CustOrderInfoType aCO)
        {
            InitializeComponent();
            myCO = aCO;
            myOrderNumber = myCO.OrderNumber;
            Title = "Customer Order " + myOrderNumber;

            mnuPicklist.Clicked += MnuPicklist_Clicked;
            mnuPacklist.Clicked += MnuPacklist_Clicked;
            mnuBOL.Clicked += MnuBOL_Clicked;

            Children.Add(new pageCODetailList());
            Children.Add(new pageCODetailInfo());
            Children.Add(new pageCODetailAvail());
            fInit = true;
        }

        private void MnuBOL_Clicked(object sender, EventArgs e)
        {
            //if (await DisplayAlert("ASCTrac", "Print BOL for Order " + myCO.OrderNumber, "Yes", "No"))
            {
                // print picklist
                PrintReport("B");
            }

        }

        private void MnuPacklist_Clicked(object sender, EventArgs e)
        {
            //if (await DisplayAlert("ASCTrac", "Print Pack List for Order " + myCO.OrderNumber, "Yes", "No"))
            {
                // print picklist
                PrintReport("P");
            }
        }

        private void MnuPicklist_Clicked(object sender, EventArgs e)
        {
            //if (await DisplayAlert("ASCTrac", "Print Picklist for Order " + myCO.OrderNumber, "Yes", "No"))
            {
                // print picklist
                PrintReport("K");
            }
        }

        async private void PrintReport(string aReportType)
        {
            await Navigation.PushAsync(new pageCOPrintReport(aReportType, myCO));
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            if (fInit)
            {
                var i = this.Children.IndexOf(this.CurrentPage);
                if (i == 0)
                    (CurrentPage as pageCODetailList).RefreshData();
                if (i == 1)
                    (CurrentPage as pageCODetailInfo).DisplayData();
                if (i == 2)
                    (CurrentPage as pageCODetailAvail).RefreshData();
            }
        }

    }
}
