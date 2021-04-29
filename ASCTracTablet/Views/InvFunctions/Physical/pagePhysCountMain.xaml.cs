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
    public partial class pagePhysCountMain : TabbedPage
    {

        public static string countID = string.Empty;
        public pagePhysCountMain()
        {
            InitializeComponent();

            Title = "Physical Count Review";
            Children.Add(new pagePhysCountFilter());

        }

        public void ShowLocList(List<ASCTracFunctionStruct.Inventory.PhysCountLocType> aList)
        {
            if (Children.Count <= 1)
                Children.Add(new pagePhysCountLocList());
            (Children[1] as pagePhysCountLocList).ShowList(aList);
            CurrentPage = Children[1];
        }

        public void RefreshList()
        {
            CurrentPage = Children[1];

            int idx = Children.Count - 1;
            while (idx > 1)
            {
                Children.RemoveAt(idx);
                idx -= 1;
            }
            (Children[0] as pagePhysCountFilter).doRefresh();
        }

        public void RefreshInvList(string aSkidID)
        {
            CurrentPage = Children[2];
            int idx = Children.Count - 1;
            while (idx > 2)
            {
                if ((Children[idx] as pagePhysCountInv).myInv.SkidID.Equals(aSkidID))
                {
                    Children.RemoveAt(idx);
                    break;
                }
                idx -= 1;
            }
            ShowLoc((Children[2] as pagePhysCountLocInfo).myLoc);
        }

        public void ShowLoc(ASCTracFunctionStruct.Inventory.PhysCountLocType aLoc)
        {
            var oldLoc = string.Empty;
            if (Children.Count <= 2)
                Children.Add(new pagePhysCountLocInfo());
            else
            {
                oldLoc = (Children[2] as pagePhysCountLocInfo).myLoc.LocationID;
            }
            (Children[2] as pagePhysCountLocInfo).ShowLoc(aLoc);
            CurrentPage = Children[2];

            if (!String.IsNullOrEmpty(oldLoc) && !aLoc.LocationID.Equals(oldLoc, StringComparison.OrdinalIgnoreCase))
            {
                int idx = Children.Count - 1;
                while (idx > 2)
                {
                    Children.RemoveAt(idx);
                    idx -= 1;
                }
            }
        }

        public void ShowInv(ASCTracFunctionStruct.Inventory.InvType aInv)
        {
            int idx = Children.Count - 1;
            while (idx > 2)
            {
                if ((Children[idx] as pagePhysCountInv).myInv.SkidID.Equals(aInv.SkidID))
                {
                    CurrentPage = Children[idx];
                    break;
                }
                idx -= 1;
            }
            if (idx <= 2)
            {
                idx = Children.Count;
                Children.Add(new pagePhysCountInv());
                (Children[idx] as pagePhysCountInv).ShowInv((Children[2] as pagePhysCountLocInfo).myLoc.LocationID, aInv);
                CurrentPage = Children[idx];
            }
        }

        public void CancelInv(ASCTracFunctionStruct.Inventory.InvType aInv)
        {
            CurrentPage = Children[2];
            int idx = Children.Count - 1;
            while (idx > 2)
            {
                if ((Children[idx] as pagePhysCountInv).myInv.SkidID.Equals(aInv.SkidID))
                {
                    Children.RemoveAt(idx);
                    break;
                }
                idx -= 1;
            }

        }
    }
}
