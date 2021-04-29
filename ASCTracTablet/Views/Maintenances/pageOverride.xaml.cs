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
    public partial class pageOverride : ContentPage
    {
        //ascTracWCFService.ProdNewSkidType newskidRec;
        DataModel.overrideType myOverrideType;
        public pageOverride(DataModel.overrideType aOverride)
        {
            InitializeComponent();
            Title = "Production Override";
            myOverrideType = aOverride;
            myOverrideType.fProcess = true;
            myOverrideType.fAnswered = false;
            lblPrompt.Text = myOverrideType.fMessage;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            edOverride.Focus();
        }

        async public void OnOverride(object sender, EventArgs e)
        {
            myOverrideType.fAnswered = true;
            myOverrideType.foverridePassword = edOverride.Text;
            //fWORec.Override = edOverride.Text;
            await Navigation.PopAsync();
        }
    }
}

