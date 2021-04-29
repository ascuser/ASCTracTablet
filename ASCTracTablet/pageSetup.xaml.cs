using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageSetup : ContentPage
    {
        public pageSetup()
        {
            InitializeComponent();
            edHostURL.Text = Globals.hostURL;
        }

        async public void onSave(object sender, EventArgs e)
        {
            Globals.hostURL = edHostURL.Text;
            //pageLogin.mydatabase.SaveConfig("hosturl", edHostURL.Text);
            await Navigation.PopAsync();
        }

    }
}