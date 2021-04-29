using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.invLookup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageInvDetailInfo : ContentPage
    {
        DataModel.dataLocItems fSkidRec;
        public pageInvDetailInfo(DataModel.dataLocItems aSkidRec)

        {
            InitializeComponent();

            fSkidRec = aSkidRec;

            Title = "Detail";
            DisplayData();
        }

        private void DisplayData()
        {
            fSkidRec = Globals.myDatabase.GetSkid(fSkidRec.SkidID);
            BindingContext = fSkidRec;

            btnPutdown.IsEnabled = fSkidRec.LocationID.StartsWith("ZXIT", StringComparison.CurrentCultureIgnoreCase) || fSkidRec.LocationID.StartsWith("ZCYCL", StringComparison.CurrentCultureIgnoreCase);
            btnQC.IsEnabled = !fSkidRec.SkidID.StartsWith("-");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DisplayData();
        }

        private void btnQC_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.QCHold.pageQCHold(fSkidRec.SkidID));
        }

        private void btnCount_Clicked(object sender, EventArgs e)
        {
            try
            {
                
                if (fSkidRec.SkidID.StartsWith("-"))
                    Navigation.PushAsync(new Views.InvFunctions.pageInvCount(fSkidRec.ItemID, fSkidRec.LocationID, "Cycle Count"));
                else
                    Navigation.PushAsync(new Views.InvFunctions.pageInvCount(fSkidRec.SkidID, "Cycle Count"));
                
            }
            catch (Exception ex)
            {
                DisplayAlert("Count Exception", ex.Message, "OK");
            }
        }

        private void btnPutdown_Clicked(object sender, EventArgs e)
        {
            if (fSkidRec.SkidID.StartsWith("-"))
                Navigation.PushAsync(new Views.InvFunctions.pageInvPutDown(fSkidRec.ItemID, fSkidRec.LocationID));
            else
                Navigation.PushAsync(new Views.InvFunctions.pageInvPutDown(fSkidRec.SkidID));
        }

        private void btnLabel_Clicked(object sender, EventArgs e)
        {
            if (fSkidRec.SkidID.StartsWith("-"))
                Navigation.PushAsync(new Views.InvFunctions.pageInvCount(fSkidRec.ItemID, fSkidRec.LocationID, "Print Label"));
            else
                Navigation.PushAsync(new Views.InvFunctions.pageInvCount(fSkidRec.SkidID, "Print Label"));
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
