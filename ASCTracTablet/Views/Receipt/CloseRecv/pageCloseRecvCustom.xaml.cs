using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.Receipt.CloseRecv
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCloseRecvCustom : ContentPage
    {
        public pageCloseRecvCustom()
        {
            InitializeComponent();
            Title = "Custom Data";
            btnNext.IsEnabled = false;
            btnClose.IsEnabled = false;
        }

        public void DisplayData()
        {
            lblPONumber.Text = pageCloseRecv.myConfirmData.PONumber;
            lblVendorInfo.Text = pageCloseRecv.myConfirmData.VendorID + "-" + pageCloseRecv.myConfirmData.VendorName;
            btnClose.IsEnabled = true;

            int irow = 2;
            foreach (var rec in pageCloseRecv.myConfirmData.customFieldList)
            {
                var mydata = rec.Value;
                var myLabel = new Label { Text = rec.Key + ":", Style = (Style)Application.Current.Resources["stdDetailLabel"] };
                gridCustomData.Children.Add(myLabel, 0, irow);

                var myEntry = new Entry { Text = mydata.Value };
                gridCustomData.Children.Add(myEntry, 1, irow);
                irow += 1;
            }
        }

        public void ClearData()
        {
            lblPONumber.Text = "N/A";
            lblVendorInfo.Text = "N/A";
            btnClose.IsEnabled = false;
            //int irow = 2;
            //while( irow >= gridCustomData.row
            //gridCustomData.
        }
        public void SaveData()
        {
            int irow = 2;
            foreach (var rec in pageCloseRecv.myConfirmData.customFieldList)
            {
                var mydata = rec.Value;

                var myView = gridCustomData.Children.FirstOrDefault(v => Grid.GetRow(v) == irow && Grid.GetColumn(v) == 1);
                var myEntry = myView as Entry;
                if (!mydata.Value.Equals(myEntry.Text, StringComparison.OrdinalIgnoreCase))
                {
                    rec.Value.fChanged = true;
                    rec.Value.Value = myEntry.Text;
                }
                irow += 1;
            }
        }

        private void btnNext_Clicked(object sender, EventArgs e)
        {

        }

        private void btnClose_Clicked(object sender, EventArgs e)
        {
            (Parent as pageCloseRecvTab).CompleteReceipt();
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            //Navigation.PopAsync();
            (Parent as pageCloseRecvTab).CurrentPage = (Parent as pageCloseRecvTab).Children[0];
        }
    }
}
