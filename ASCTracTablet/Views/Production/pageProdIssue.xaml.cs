using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.Production
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageProdIssue : ContentPage
    {
        ASCTracFunctionStruct.Production.WOHdrType fWORec;
        DataModel.dataWODet myItem;
        List<DataModel.dataLocItems> mySkidList;

        public pageProdIssue(ASCTracFunctionStruct.Production.WOHdrType aWORec, DataModel.dataWODet aItem, List<DataModel.dataLocItems> aSkidList)
        {
            fWORec = aWORec;
            Title = "Issue Component";
            myItem = aItem;
            mySkidList = aSkidList;
            InitializeComponent();
            lblWO.Text = fWORec.Workorder_ID;
            lblItemID.Text = myItem.Comp_ItemID + " - " + myItem.Description;

            listLicenses.ItemsSource = mySkidList;
            listLicenses.ItemTapped += ListLicenses_ItemTapped;

            btnRefresh.IsVisible = false;
        }

        private void ListLicenses_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            DataModel.dataLocItems myskid = (DataModel.dataLocItems)e.Item;
            //DisplayAlert("Issue", myskid.SkidID, "OK");
            Navigation.PushAsync(new pageProdIssueInventory(fWORec, myItem, myskid));
        }

        private void OnSkidIDCompleted(object sender, EventArgs e)
        {
            var myskid = Globals.myDatabase.GetSkid(edSkidID.Text);
            if (myskid == null)
            {
                DisplayAlert("ASCTrac", "License not found", "OK");
            }
            else
            {
                Navigation.PushAsync(new pageProdIssueInventory(fWORec, myItem, myskid));
            }
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void btnRefresh_Clicked(object sender, EventArgs e)
        {

        }
    }
}

