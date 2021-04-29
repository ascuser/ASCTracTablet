using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.InvFunctions.Physical
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pagePhysCountLocList : ContentPage
    {
        List<ASCTracFunctionStruct.Inventory.PhysCountLocType> myLocList;

        public pagePhysCountLocList()
        {
            InitializeComponent();

            Title = "Location List";
            listLocs.ItemSelected += ListLocs_ItemSelected;
        }

        public void ShowList(List<ASCTracFunctionStruct.Inventory.PhysCountLocType> aList)
        {
            myLocList = aList;
            listLocs.ItemsSource = myLocList;
        }

        private void ListLocs_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var myLoc = (e.SelectedItem as ASCTracFunctionStruct.Inventory.PhysCountLocType);
            (Parent as pagePhysCountMain).ShowLoc(myLoc);
            //DisplayAlert("selected", "Location " + myLoc.LocationID, "YUP");
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            (Parent as pagePhysCountMain).CurrentPage = (Parent as pagePhysCountMain).Children[0];
        }
    }
}
