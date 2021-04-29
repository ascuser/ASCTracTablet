using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.COContainer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCOVesselInfo : ContentPage
    {
        ASCTracFunctionStruct.CustOrder.OrderContainerLookupInfo myRec;
        public pageCOVesselInfo(ASCTracFunctionStruct.CustOrder.OrderContainerLookupInfo aRec)
        {
            InitializeComponent();
            myRec = aRec;
            BindingContext = myRec;
        }

        private void btnSched_Clicked(object sender, EventArgs e)
        {

        }

        private void btnShip_Clicked(object sender, EventArgs e)
        {

        }

        private void btnInspect_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
