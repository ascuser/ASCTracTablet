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
    public partial class pageInvHistory : ContentPage
    {

        DataModel.dataLocItems fSkidRec;
        List<ASCTracFunctionStruct.Inventory.InvHistoryType> fSkidHistList;
        public pageInvHistory(DataModel.dataLocItems aSkidRec, List<ASCTracFunctionStruct.Inventory.InvHistoryType> aSkidHistList)
        {
            InitializeComponent();
            fSkidRec = aSkidRec;
            fSkidHistList = aSkidHistList;
            BindingContext = aSkidRec;

            Title = "History";
            listHistory.ItemsSource = aSkidHistList;
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            //Navigation.PopAsync();
            (Parent as pageInvDetailTab).CurrentPage = (Parent as pageInvDetailTab).Children[0];
        }
    }
}
