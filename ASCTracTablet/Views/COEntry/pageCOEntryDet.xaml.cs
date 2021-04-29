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
    public partial class pageCOEntryDet : ContentPage
    {
        public pageCOEntryDet()
        {
            InitializeComponent();
            Title = "Detail List";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (Parent as COEntry.pageCOEntry).GetOrder(pageCOEntry.myCOHdr.OrderNumber);
        }

        public void SetupDetail()
        {
            BindingContext = pageCOEntry.myCOHdr;
            listDetails.ItemsSource = pageCOEntry.myCOHdr.DetailList.Where(o => o.fDeleted.Equals(false)).OrderBy(o => o.LineNumber);
        }

        public void SetIndicator(bool aOnFlag)
        {
            myIndicator.IsRunning = aOnFlag;
            myIndicator.IsVisible = aOnFlag;
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            (Parent as pageCOEntry).CurrentPage = (Parent as pageCOEntry).Children[0];
        }

        async private void BtnAdd_Clicked(object sender, EventArgs e)
        {
            // open form to add new Detail record.
            long linenum = 1;
            if (pageCOEntry.myCOHdr.DetailList.Count > 0)
                linenum = pageCOEntry.myCOHdr.DetailList[pageCOEntry.myCOHdr.DetailList.Count - 1].LineNumber + 1;
            var myDetRec = new ASCTracFunctionStruct.COEntry.COEntryDet();
            myDetRec.LineNumber = linenum;
            myDetRec.QtyOrdered = 0;
            await Navigation.PushAsync(new pageCOEntryEditDetail(myDetRec));
        }

        private void BtnCreate_Clicked(object sender, EventArgs e)
        {
            (Parent as pageCOEntry).CompleteOrder();
        }

        async private void ListDetails_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // edit line, with option to delete?
            var rec = (ASCTracFunctionStruct.COEntry.COEntryDet)e.SelectedItem;
            await Navigation.PushAsync(new pageCOEntryEditDetail(rec));
        }

        async private void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var rec = (ASCTracFunctionStruct.COEntry.COEntryDet)mi.BindingContext;

            var retval = await DisplayAlert("Delete Detail", "Delete Line " + rec.LineNumber.ToString(), "Yes", "No");
            if (retval)
            {
                rec.fDeleted = true;
                listDetails.ItemsSource = null;
                listDetails.ItemsSource = pageCOEntry.myCOHdr.DetailList.Where(o => o.fDeleted.Equals(false)).OrderBy(o => o.LineNumber);
            }
        }

        async private void OnEdit(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var rec = (ASCTracFunctionStruct.COEntry.COEntryDet)mi.BindingContext;
            await Navigation.PushAsync(new pageCOEntryEditDetail(rec));
        }
    }
}