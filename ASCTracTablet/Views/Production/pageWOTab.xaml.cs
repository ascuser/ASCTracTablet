using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.Production
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageWOTab : TabbedPage
    {
        ASCTracFunctionStruct.Production.WOHdrType fWORec;

        bool fProdStarted = false;
        public pageWOTab(ASCTracFunctionStruct.Production.WOHdrType aWORec, string aProdLine)
        {
            fWORec = aWORec;

            InitializeComponent();

            Title = "Production";
            //Children.Add(new pageTripLegList(myTrip, myDatabase));

            var navigationPage = new pageSchedWO(aWORec, aProdLine);
            //navigationPage.Icon = "ALARM.png";
            navigationPage.Title = "Schedule";
            Children.Add(navigationPage);

            if (aWORec.Status == "A")
            {
                startProd();
                CurrentPage = Children[1];

            }
            //Children.Add(new pageProdWO(aWORec, aProdLine, aUserID, aSiteID));
            //Children.Add(new pageTripLegExpenseList(myDatabase, myTrip, myTripLeg));

        }

        public void startProd()
        {
            if (!fProdStarted)
            {
                fProdStarted = true;
                var navigationPage = new pageProdWO(fWORec);
                //navigationPage.Icon = "ALARM.png";
                navigationPage.Title = "Manufacturing Entry";
                Children.Add(navigationPage);

                var navigationPage2 = new pageProdLicenses(fWORec);
                //navigationPage2.Icon = "ALARM.png";
                navigationPage2.Title = "Licenses";
                Children.Add(navigationPage2);
                var navigationPage3 = new Production.pageComponents(fWORec);
                //navigationPage3.Icon = "ALARM.png";
                navigationPage3.Title = "Components";
                Children.Add(navigationPage3);
                var navigationPage4 = new Production.pageProdQCTest(fWORec);
                //navigationPage4.Icon = "ALARM.png";
                navigationPage4.Title = "QC Tests";
                Children.Add(navigationPage4);
            }
        }
    }
}

