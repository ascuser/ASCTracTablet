using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.WOSM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageWOSMTab : TabbedPage
    {
        public pageWOSMTab()
        {
            ClassWOSMInput.myWOSMInput.fInit = false;

            InitializeComponent();
            Title = "WO Status Metrics";
            Children.Add(new pageWOSM1());
            Children.Add(new pageWOSMWO());
        }
    }
}

