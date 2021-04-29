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
    public partial class pageCloseRecvFees : ContentPage
    {
        public pageCloseRecvFees()
        {
            InitializeComponent();
            Title = "3PL Fees";
            btnClose.IsEnabled = false;

        }

        public void DisplayData()
        {
            lblPONumber.Text = pageCloseRecv.myConfirmData.PONumber;
            lblVendorInfo.Text = pageCloseRecv.myConfirmData.VendorID + "-" + pageCloseRecv.myConfirmData.VendorName;
            btnClose.IsEnabled = true;

            list3PLFees.ItemsSource = pageCloseRecv.myConfirmData.fees3PLList;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DisplayData();
        }

        public void ClearData()
        {
            lblPONumber.Text = "N/A";
            lblVendorInfo.Text = "N/A";
            btnClose.IsEnabled = false;
            list3PLFees.ItemsSource = null;
        }

        public void SaveData()
        {

        }

        private void btnNext_Clicked(object sender, EventArgs e)
        {
                (Parent as pageCloseRecvTab).CurrentPage = (Parent as pageCloseRecvTab).Children[3];
        }

        private void btnClose_Clicked(object sender, EventArgs e)
        {
                (Parent as pageCloseRecvTab).CompleteReceipt();
        }

        private void btnAdd_Clicked(object sender, EventArgs e)
        {
            //var rec = new ASCTracRestService.DataModel.Fees3PLType();
            var rec = new ASCTracFunctionStruct.Fees3PLType();
            rec.Code = string.Empty;
            rec.Description = string.Empty;
            rec.Qty = 1;
            rec.Fee = 0;
            rec.TotalFee = 0;
            rec.Notes = string.Empty;
            Navigation.PushAsync(new pageCloseRecvFeeAdd(rec));
        }

        private void list3PLFees_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var rec = (ASCTracFunctionStruct.Fees3PLType)list3PLFees.SelectedItem;
            //var rec = (ASCTracRestService.DataModel.Fees3PLType)list3PLFees.SelectedItem;
            Navigation.PushAsync(new pageCloseRecvFeeAdd(rec));
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            //Navigation.PopAsync();
            (Parent as pageCloseRecvTab).CurrentPage = (Parent as pageCloseRecvTab).Children[0];
        }
    }
}
