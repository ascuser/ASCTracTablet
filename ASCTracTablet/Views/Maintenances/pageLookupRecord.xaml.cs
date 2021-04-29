using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.Maintenances
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageLookupRecord : ContentPage
    {
        public static string myResult;
        private string fType;

        private List<ASCTracFunctionStruct.LookupItemType> fieldlist;
        private List<ASCTracFunctionStruct.LookupCustType> myCustList;
        private List<ASCTracFunctionStruct.LookupItemIDType> myItemList;

        public pageLookupRecord(string aType, string aDefault, string aHeaderPrompt, string aHeaderData)
        {
            InitializeComponent();
            myResult = aDefault;
            edField.Text = aDefault;
            fType = aType;
            lblHeaderPrompt.Text = aHeaderPrompt;
            lblHeaderData.Text = aHeaderData;

            InitLookup();
        }

        private void InitLookup()
        {
            fieldlist = new List<ASCTracFunctionStruct.LookupItemType>();
            if (fType.Equals("C"))
            {
                lblHeader1.Text = "CustID";
                lblHeader2.Text = "Cust Name";

                fieldlist.Add(new ASCTracFunctionStruct.LookupItemType("CustID", "Cust ID"));
                fieldlist.Add(new ASCTracFunctionStruct.LookupItemType("CustName", "Name"));
            }
            else
            {
                lblHeader1.Text = "ItemID";
                lblHeader2.Text = "Description";
                fieldlist.Add(new ASCTracFunctionStruct.LookupItemType("ItemID", "Item ID"));
                fieldlist.Add(new ASCTracFunctionStruct.LookupItemType("Description", "Description"));
            }
            ascUtils.setupPicker(pickField, fieldlist, fieldlist[0].ID);
            SetupLookup();
        }

        private void SetupLookup()
        {
            var fieldname = ascUtils.getPickerValue(pickField);
            string aValue = edField.Text;
            if (String.IsNullOrEmpty(aValue))
                aValue = string.Empty;
            if (fType.Equals("C"))
            {
                if (Globals.lookupCustList == null)
                    DisplayAlert("Customer Lookup", "Customer list not initialized", "OK");
                else
                {
                    if (fieldname.Equals("CustName"))
                        myCustList = Globals.lookupCustList.Where(o => o.CustName.StartsWith(aValue)).OrderBy(o => o.CustName).ToList();
                    else
                        myCustList = Globals.lookupCustList.Where(o => o.CustID.StartsWith(aValue)).OrderBy(o => o.CustID).ToList();
                    listSelect.ItemsSource = myCustList;
                }
            }
            else
            {
                if (Globals.lookupItemIDList == null)
                    DisplayAlert("Item Lookup", "Item list not initialized", "OK");
                else
                {
                    if (fieldname.Equals("Description"))
                        myItemList = Globals.lookupItemIDList.Where(o => o.Description.StartsWith(aValue)).OrderBy(o => o.Description).ToList();
                    else
                        myItemList = Globals.lookupItemIDList.Where(o => o.ItemID.StartsWith(aValue)).OrderBy(o => o.ItemID).ToList();
                    listSelect.ItemsSource = myItemList;
                }
            }
        }

        private void ProcessSelect(object aSelectedRecord)
        {
            if (fType.Equals("C"))
            {
                var rec = (ASCTracFunctionStruct.LookupCustType)aSelectedRecord;
                myResult = rec.CustID;
                Navigation.PopAsync();
            }
            else
            {
                var rec = (ASCTracFunctionStruct.LookupItemIDType)aSelectedRecord;
                myResult = rec.ItemID;
                Navigation.PopAsync();
            }

        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void ListSelect_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ProcessSelect(e.SelectedItem);

        }

        private void EdField_Completed(object sender, EventArgs e)
        {
            SetupLookup();
        }

        public void OnMore(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            if (fType.Equals("C"))
            {
                var rec = (ASCTracFunctionStruct.LookupCustType)mi.BindingContext;
                string aInfo = rec.CustName;
                if (!String.IsNullOrEmpty(rec.CustAddress.Address1))
                    aInfo += "\r\n" + rec.CustAddress.Address1;
                if (!String.IsNullOrEmpty(rec.CustAddress.Address2))
                    aInfo += "\r\n" + rec.CustAddress.Address2;
                if (!String.IsNullOrEmpty(rec.CustAddress.Address3))
                    aInfo += "\r\n" + rec.CustAddress.Address3;
                if (!String.IsNullOrEmpty(rec.CustAddress.Address4))
                    aInfo += "\r\n" + rec.CustAddress.Address4;
                if (!String.IsNullOrEmpty(rec.CustAddress.City))
                    aInfo += "\r\n" + rec.CustAddress.City + ", " + rec.CustAddress.State + "  " + rec.CustAddress.ZipCode;

                DisplayAlert("Cust ID: " + rec.CustID, aInfo, "OK");
            }
            else
            {
                var rec = (ASCTracFunctionStruct.LookupItemIDType)mi.BindingContext;
                string aInfo = rec.Description;
                if (!String.IsNullOrEmpty(rec.ExtDescription))
                    aInfo += "\r\n" + rec.ExtDescription;
                DisplayAlert("Item ID: " + rec.ItemID, aInfo, "OK");
            }

        }

        private void OnSelect(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            ProcessSelect(mi.BindingContext);

        }
    }
}