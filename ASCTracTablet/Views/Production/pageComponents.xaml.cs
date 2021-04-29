using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.Production
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageComponents : ContentPage
    {
        static string fDefaultPrinter = String.Empty;
        ASCTracFunctionStruct.Production.WOHdrType fWORec;
        DataModel.dataWODet myItem;
        public pageComponents(ASCTracFunctionStruct.Production.WOHdrType aWORec)
        {
            fWORec = aWORec;
            Title = "Components";
            InitializeComponent();
            BindingContext = fWORec;
            listComponents.ItemTapped += ListComponents_ItemTapped;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetComponents();
        }

        async private void GetComponents()
        {
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            string errmsg = string.Empty;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(fWORec.Workorder_ID);

                var myReturnData = await App.myRestManager.GetWOComponents(Globals.curBasicMessage);

                myIndicator.IsRunning = false;
                myIndicator.IsVisible = false;
                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    var xmlResult = myReturnData.DataMessage;
                    if (xmlResult.StartsWith("OK"))
                    {
                        var xdoc = System.Xml.Linq.XDocument.Parse(xmlResult.Substring(2)).Element("NewDataSet"); //.Element( "Users");

                        Globals.myDatabase.ClearWODet();
                        foreach (XElement xe in xdoc.Nodes())
                        {
                            //SEQ_NUM, D.COMP_ITEMID, I.DESCRIPTION, D.QTY, D.QTY_PICKED, D.QTY_USED
                            long seq = Convert.ToInt64(ascUtils.XGetString(xe.Element("SEQ_NUM")));
                            DataModel.dataWODet rec = new DataModel.dataWODet();
                            rec.SeqNum = seq;
                            rec.Comp_ItemID = ascUtils.XGetString(xe.Element("COMP_ITEMID"));
                            rec.Description = ascUtils.XGetString(xe.Element("DESCRIPTION"));
                            rec.Qty = ascUtils.XGetDouble(xe.Element("QTY"));
                            rec.Qty_Picked = ascUtils.XGetDouble(xe.Element("QTY_PICKED"));
                            rec.Qty_Used = ascUtils.XGetDouble(xe.Element("QTY_USED"));
                            rec.kanban_location = ascUtils.XGetString(xe.Element("KANBAN_LOCATION"));

                            Globals.myDatabase.AddWODet(rec);
                        }
                        var mylist = Globals.myDatabase.GetWODets().ToList<DataModel.dataWODet>();
                        listComponents.ItemsSource = mylist;
                        lblCount.Text = mylist.Count.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Get Components Exception", ex.Message, "OK");
            }
            if (!String.IsNullOrEmpty(errmsg))
                await DisplayAlert("Get Components Error", errmsg, "OK");
        }

        private void ListComponents_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            myItem = (DataModel.dataWODet)e.Item;
            GetComponentLicenses(myItem);
        }

        async private void GetComponentLicenses(DataModel.dataWODet aItem)
        {
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            string errmsg = string.Empty;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(fWORec.Workorder_ID);
                Globals.curBasicMessage.inputDataList.Add(aItem.SeqNum.ToString());
                var myReturnData = await App.myRestManager.GetWOComponentLicenses(Globals.curBasicMessage);

                myIndicator.IsRunning = false;
                myIndicator.IsVisible = false;
                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else if (myReturnData.DataMessage.StartsWith("OK"))

                {
                    var xdoc = System.Xml.Linq.XDocument.Parse(myReturnData.DataMessage.Substring(2)).Element("NewDataSet"); //.Element( "Users");

                    Globals.myDatabase.ClearLocItems();
                    foreach (XElement xe in xdoc.Nodes())
                    {
                        // SKIDID, QTYTOTAL, LOCATIONID, DATETIMEPROD, LOTID, EXPDATE, PUTDOWN_DATETIME
                        DataModel.dataLocItems rec = new DataModel.dataLocItems();
                        rec.SkidID = ascUtils.XGetString(xe.Element("SKIDID"));
                        rec.LotID = ascUtils.XGetString(xe.Element("LOTID"));
                        rec.QtyTotal = ascUtils.XGetDouble(xe.Element("QTYTOTAL"));
                        rec.LocationID = ascUtils.XGetString(xe.Element("LOCATIONID"));
                        rec.DateTimeProd = Convert.ToDateTime(ascUtils.XGetString(xe.Element("DATETIMEPROD")));
                        if (xe.Element("EXPDATE") != null)
                            rec.ExpDate = Convert.ToDateTime(ascUtils.XGetString(xe.Element("EXPDATE")));
                        if (xe.Element("PUTDOWN_DATETIME") != null)
                            rec.PutdownDateTime = Convert.ToDateTime(ascUtils.XGetString(xe.Element("PUTDOWN_DATETIME")));
                        rec.QAHold = "F";

                        Globals.myDatabase.AddLocItems(rec);
                    }

                    var mylist = Globals.myDatabase.GetLocItems().ToList(); //<Data.dataLocItems>();
                    if (mylist.Count == 0)
                        await DisplayAlert("ASCTrac", "Component " + myItem.Comp_ItemID + " has no inventory to issue", "OK");
                    else
                        await Navigation.PushAsync(new pageProdIssue(fWORec, myItem, mylist));
                }
                else
                    errmsg = "Invalid Data Returned \r\n" + myReturnData.DataMessage;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Get Licences Exception", ex.Message, "OK");
            }
            if (!String.IsNullOrEmpty(errmsg))
                await DisplayAlert("Get Licences Error", errmsg, "OK");
        }

    

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            //Navigation.PopAsync();
            (Parent as pageWOTab).CurrentPage = (Parent as pageWOTab).Children[0];
        }
    }
}

