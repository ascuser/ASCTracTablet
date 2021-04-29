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
    public partial class pageInvDetailTab : TabbedPage
    {
        public pageInvDetailTab(DataModel.dataLocItems aSkidRec, List<ASCTracFunctionStruct.Inventory.InvHistoryType> aSkidHistList)
        {
            InitializeComponent();
            if (!String.IsNullOrEmpty(aSkidRec.SkidID) && !aSkidRec.SkidID.StartsWith("-"))
                Title = "License " + aSkidRec.SkidID;
            else
                Title = "Item " + aSkidRec.ItemID;
            Children.Add(new pageInvDetailInfo(aSkidRec));
            if ((aSkidHistList != null) && (aSkidHistList.Count > 0))
                Children.Add(new pageInvHistory(aSkidRec, aSkidHistList));
        }
    }
}
