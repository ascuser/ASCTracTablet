using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageMenu : ContentPage
    {
        string currMenu = "X0";
        string MENU_MAIN = "X0";
        const int BUTTON_COLUMN = 1;

        public pageMenu()
        {
            InitializeComponent();
            SetMenu(MENU_MAIN, "Main");
        }

        private Button FindButton(int ypos, string aText)
        {
            Button retval = null;
            foreach (View v in gridMainMenu.Children)
            {
                if ((BUTTON_COLUMN == Grid.GetColumn(v)) && (ypos == Grid.GetRow(v)))
                {
                    retval = (v as Button);
                    break;
                }
            }
            if (retval == null)
            {
                //retval = new Button { Text = aText, Style = (Xamarin.Forms.Style)Application.Current.Resources["stdMenuButtonStyle"] };
                retval = new Button
                {
                    Text = aText,
                    Style = (Xamarin.Forms.Style)Application.Current.Resources["stdButtonStyle"]
                };
                gridMainMenu.Children.Add(retval, BUTTON_COLUMN, ypos);
            }
            else
                retval.Text = aText;

            return (retval);
        }

        private void SetMenu(string aMenuID, string aMenuDesc)
        {
            currMenu = aMenuID;
            lblTitle.Text = aMenuDesc + " Menu";

            ClearGrid();

            SetMenuButtons(aMenuID);
        }

        private void ClearGrid()
        {
            foreach (var child in gridMainMenu.Children.Reverse())
            {
                if (child is Button)
                {
                    //var myButton = Button();
                    if (child != btnEsc)
                        gridMainMenu.Children.Remove(child);
                }
            }
        }

        private void SetMenuButtons(string aMenuID)
        {
            int ypos = 0;
            var myMenuList = Globals.myDatabase.GetMenuList(aMenuID);
            foreach (var rec in myMenuList)
            {
                var myButton1 = FindButton(ypos, rec.Description);
                if (rec.MenuID.Equals("C4") || rec.MenuID.Equals("I4"))// rec.MenuID.Equals("C5") || || rec.MenuID.Equals("P4")) // scheduling, OE, Quick Count, BOM Avail
                {
                    myButton1.IsEnabled = false;
                    myButton1.Command = null;
                }
                else
                {
                    myButton1.IsEnabled = true;

                    myButton1.Command = new Command(() => btnASCButton_Clicked(myButton1, null));
                }
                ypos++;
            }

            if ((Device.RuntimePlatform == Device.iOS) && !aMenuID.Equals(MENU_MAIN))
            {
                var myButton = new Button { Text = "Main Menu", Style = (Xamarin.Forms.Style)Application.Current.Resources["stdMenuButtonStyle"] };
                gridMainMenu.Children.Add(myButton, BUTTON_COLUMN, 0);
                myButton.Clicked += onMainMenu;
            }
        }

        /*

DECLARE @MENUID VARCHAR(2)
DECLARE @DESC VARCHAR(40)
DECLARE @HDRID NUMERIC(5)

SET @MENUID='R5'
SET @DESC = 'PO Lookup'
SET @HDRID = 1
IF NOT EXISTS ( SELECT * FROM HH_SETUP_FILES WHERE RECTYPE='T' AND PROMPT_ID=@MENUID )
INSERT INTO HH_SETUP_FILES
( RECTYPE, PROMPT_ID, PROMPT_DEFAULT, MIN_LEN, MAX_LEN, TITLE_LEN, EDTYPE, REQ_FLAG, HDRID )
VALUES ( 'T', @MENUID, @DESC, 0, 0, 0, 'M', 'T', @HDRID)

         */

        private async void SetTran(DataModel.dataMenuSetup menurec)
        {
            try
            {
                bool fGood = false;
                switch (menurec.MenuID)
                {
                    case "R1": // Expected Receipts Status
                        fGood = true;
                        await Navigation.PushAsync(new Views.Receipt.pageExpRecvStatusTab());
                        break;

                    case "R2": //   Close Receipt
                        fGood = true;
                        await Navigation.PushAsync(new Views.Receipt.pageCloseRecvTab()); // Recv.pageCloseRecvTab());
                        break;
                    case "R3": //  Dock Schedule
                        fGood = true;
                        await Navigation.PushAsync(new Views.DockSchd.pageDockScheduler());
                        break;
                    case "R4": //   Inventory not Putaway
                        fGood = true;
                        await Navigation.PushAsync(new Views.invLookup.pageInvLookupMain("R"));
                        break;
                    case "R5": //   PO Lookup
                        fGood = true;
                        await Navigation.PushAsync(new Views.Receipt.pagePOLookup());
                        break;
                    case "C1": //   Customer Order Status
                        fGood = true;
                        await Navigation.PushAsync(new Views.COSM.pageCOSMTab());
                        break;
                    case "C2": //   CO Lookup
                        fGood = true;
                        await Navigation.PushAsync(new Views.COSM.pageCOLookup());
                        break;
                    case "C3": //  Dock Scheduling
                        fGood = true;
                        await Navigation.PushAsync(new Views.DockSchd.pageDockScheduler());
                        break;
                    case "C4": //   Customer Order Scheduling
                               //await DisplayAlert("ASCTrac", "Function Not Available", "OK");
                        break;
                    case "C5": //   Order Entry
                        fGood = true;
                        await Navigation.PushAsync(new Views.COEntry.pageCOEntry(string.Empty));
                        break;
                    case "C6": //  Parcel Lookup
                        fGood = true;
                        await Navigation.PushAsync(new Views.COContainer.pageCOContainerLookup("P"));
                        break;
                    case "C7": //   Container(Tote) Lookup
                        fGood = true;
                        await Navigation.PushAsync(new Views.COContainer.pageCOContainerLookup("T"));
                        break;
                    case "C8": //   Vessel Lookup
                        fGood = true;
                        await Navigation.PushAsync(new Views.COContainer.pageCOContainerLookup("V"));
                        break;

                    case "C9": //  Document Capture
                               //await DisplayAlert("ASCTrac", "Function Not Available", "OK");
                        break;
                    case "P1": //   Production Scheduler
                        fGood = true;
                        await Navigation.PushAsync(new Views.Production.pageProdScheduler());
                        break;
                    case "P2": //  Manufacturing Order Status
                        fGood = true;
                        await Navigation.PushAsync(new Views.WOSM.pageWOSMTab());
                        break;

                    case "P3": //  Manufacturing Order Entry
                        fGood = true;
                        await Navigation.PushAsync(new Views.Production.pageProdGetWO());
                        break;

                    case "P4": //  Inventory / BOM Availability
                        fGood = true;
                        await Navigation.PushAsync(new Views.invLookup.pageBOMAvail());
                        break;
                    case "I1": //   Inventory Lookup
                        fGood = true;
                        await Navigation.PushAsync(new Views.invLookup.pageInvLookupMain("I"));
                        break;
                    case "I2": //  QC Hold / Release
                        fGood = true;
                        await Navigation.PushAsync(new Views.QCHold.pageQCHold(string.Empty));
                        break;
                    case "I3": //  Physical Count Review
                        fGood = true;
                        await Navigation.PushAsync(new Views.InvFunctions.Physical.pagePhysCountMain());
                        break;
                    case "I4": //  Quick Count Setup
                               //await DisplayAlert("ASCTrac", "Function Not Available", "OK");
                        break;

                    case "I5": //  Item Maintenance
                        fGood = true;
                        await Navigation.PushAsync(new Views.Maintenances.pageItemMaint());
                        break;
                    case "I6": //   Location Maintenance
                        fGood = true;
                        await Navigation.PushAsync(new Views.Maintenances.pageLocMaint());
                        break;
                    case "I7": //  Replenishment Lookup
                        fGood = true;
                        await Navigation.PushAsync(new Views.Replen.pageReplenTab());
                        break;
                    default:
                        //await DisplayAlert("Exception", "Menu " + menurec.MenuID + " not defined", "OK");
                        fGood = false;
                        break;
                }
                if (!fGood)
                    await DisplayAlert("ASCTrac", "Function for Menu " + menurec.MenuID + " Not Available", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("ASCTrac (SetTran)", ex.Message, "OK");
            }
        }

        async private void doSignoff()
        {
            if (await DisplayAlert("ASCTrac Tablet", "Confirm Logoff", "Yes", "No"))
            {
                var returnData = await App.myRestManager.Signoff(Globals.curBasicMessage);
                await Navigation.PopAsync();
            }
            else
            {
                SetMenu(MENU_MAIN, "Main");
            }
        }

        private void btnASCButton_Clicked(Button myButton1, object p)
        {
            var menuID = Globals.myDatabase.GetMenuByDesc(myButton1.Text);
            if (menuID.MenuID.StartsWith("X"))
                SetMenu(menuID.MenuID, myButton1.Text);
            else
                SetTran(menuID);
        }

        protected override bool OnBackButtonPressed()
        {
            var retval = true;
            Device.BeginInvokeOnMainThread(() =>
           {

               if (currMenu != MENU_MAIN)
               {
                   SetMenu(MENU_MAIN, "Main");
               }
               else
                   doSignoff();
           });
            return (retval);
        }

        private void onMainMenu(object sender, EventArgs e)
        {
            SetMenu(MENU_MAIN, "Main");
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            if (currMenu.Equals(MENU_MAIN))
                doSignoff(); // Navigation.PopAsync();
            else
                SetMenu(MENU_MAIN, "Main");

        }
    }
}