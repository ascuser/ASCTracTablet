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
    public partial class pageCloseRecvTab : TabbedPage
    {
        public pageCloseRecvTab()
        {
            InitializeComponent();
            Title = "Close Receipt";
            Children.Add(new CloseRecv.pageCloseRecv());

        }

        public pageCloseRecvTab(string aRecType, string aRecID)
        {
            InitializeComponent();
            Title = "Close Receipt";
            Children.Add(new CloseRecv.pageCloseRecv());
            (Children[0] as CloseRecv.pageCloseRecv).Init(aRecType, aRecID);

        }

        public void ShowTabs()
        {
            if (Children.Count == 1)
            {
                Children.Add(new CloseRecv.pageCloseRecvPalletLog());
                Children.Add(new CloseRecv.pageCloseRecvFees());
                Children.Add(new CloseRecv.pageCloseRecvCustom());
            }
        }

        async public void CompleteReceipt()
        {
            if (await DisplayAlert("Close Receipt", "Close this Receipt?", "OK", "Cancel"))
            {
                (Children[0] as CloseRecv.pageCloseRecv).SaveData();
                (Children[1] as CloseRecv.pageCloseRecvPalletLog).SaveData();
                (Children[2] as CloseRecv.pageCloseRecvFees).SaveData();
                (Children[3] as CloseRecv.pageCloseRecvCustom).SaveData();
                CurrentPage = Children[0];
                (Children[0] as CloseRecv.pageCloseRecv).ProcessReceipt();
            }
        }
    }
}
