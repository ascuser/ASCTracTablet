using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.COSM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCOSMTab : TabbedPage
    {
        public pageCOSMTab()
        {
            COSM.ClassCOSMInput.myCOSMInput.fInit = false;

            InitializeComponent();
            Title = "Customer Order Status Metrics";
            Children.Add(new pageCOSM1());
            Children.Add(new pageCOSMByCO());
        }
    }
}

