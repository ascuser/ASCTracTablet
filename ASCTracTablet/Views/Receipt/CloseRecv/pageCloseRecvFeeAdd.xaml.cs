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
    public partial class pageCloseRecvFeeAdd : ContentPage
    {
        ASCTracFunctionStruct.Fees3PLType myFeeType;
        //ASCTracRestService.DataModel.Fees3PLType myFeeType;
        private bool fAdding = true;
        public pageCloseRecvFeeAdd(ASCTracFunctionStruct.Fees3PLType aFeeType)
        //public pageCloseRecvFeeAdd(ASCTracRestService.DataModel.Fees3PLType aFeeType)
        {
            InitializeComponent();
            myFeeType = aFeeType;
            fAdding = String.IsNullOrEmpty(aFeeType.Code);
            DisplayData();
        }

        private void DisplayData()
        {
            lblPONumber.Text = pageCloseRecv.myConfirmData.PONumber;
            lblVendorInfo.Text = pageCloseRecv.myConfirmData.VendorID + "-" + pageCloseRecv.myConfirmData.VendorName;

            edFee.Text = myFeeType.Fee.ToString();
            edQty.Text = myFeeType.Qty.ToString();
            edNotes.Text = myFeeType.Notes;

            var myList = Globals.myDatabase.Get3PLRates();
            cbCode.Items.Clear();
            foreach (var rec in myList)
            {
                cbCode.Items.Add(rec.ID + "-" + rec.Description);
                if (rec.ID.Equals(myFeeType.Code))
                    cbCode.SelectedIndex = cbCode.Items.Count - 1;
            }
        }

        private void btnNext_Clicked(object sender, EventArgs e)
        {
            if (cbCode.SelectedIndex >= 0)
            {
                string tmp = cbCode.Items[cbCode.SelectedIndex].ToString();
                myFeeType.Code = tmp.Substring(0, tmp.IndexOf("-")).Trim();
                myFeeType.Description = tmp.Substring(tmp.IndexOf("-") + 1).Trim();
                myFeeType.Fee = Convert.ToDouble(edFee.Text);
                myFeeType.Qty = Convert.ToDouble(edQty.Text);
                myFeeType.TotalFee = myFeeType.Fee * myFeeType.Qty;
                myFeeType.Notes = edNotes.Text;
                myFeeType.fChanged = true;
                if (fAdding)
                    pageCloseRecv.myConfirmData.fees3PLList.Add(myFeeType);
                Navigation.PopAsync();
            }
        }

        private void btnClose_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void cbCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmp = cbCode.Items[cbCode.SelectedIndex].ToString();
            string Code = tmp.Substring(0, tmp.IndexOf("-")).Trim();
            var rec = Globals.myDatabase.Get3PLRate(Code);
            if (rec != null)
                edFee.Text = rec.fee.ToString();
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
