using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.Replen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageReplenTab : TabbedPage
    {

        Dictionary<string, int> myTabList = new Dictionary<string, int>();

        public pageReplenTab()
        {
            InitializeComponent();
            Title = "Replenishment Status";
            Children.Add(new pageReplenSummary());
        }

        internal void SelectZone(ASCTracFunctionStruct.Inventory.ReplenSummType aZone)
        {
            try
            {
                int aIndex;
                if (myTabList.TryGetValue(aZone.ZoneID, out aIndex))
                {
                    CurrentPage = Children[aIndex];
                }
                else
                {
                    myTabList.Add(aZone.ZoneID, Children.Count);
                    var myPage = new pageReplenLocList(aZone);
                    Children.Add(myPage);
                    CurrentPage = myPage;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert(Globals.AppTitleName, "Opening new Page\r\n" + ex.Message, "OK");
            }
        }
    }
}
