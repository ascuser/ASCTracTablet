using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.COFunction
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCOConfirmShip : ContentPage
    {
        public string fRecType, fRecID;
        public pageCOConfirmShip( string aRecType, string aRecID)
        {
            InitializeComponent();
            fRecType = aRecType;
            fRecID = aRecID;

            GetCOInfo();
        }

        private void GetCOInfo()
        {

        }

        private void btnShip_Clicked(object sender, EventArgs e)
        {

        }

        private void btnBOL_Clicked(object sender, EventArgs e)
        {

        }

        private void btnPacklist_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}