using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.Receipt
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageExpRecvStatusTab : TabbedPage
    {
        public pageExpRecvStatusTab()
        {
            ClassExpRecvInput.myExpRecvStatusInput.fInit = false;

            InitializeComponent();
            Title = "Expected Receipt Status Metrics";
            Children.Add(new pageExpRecvStatus());
            Children.Add(new pageExpRecvStatusPOList());
        }
    }
}