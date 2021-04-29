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
    public partial class pageProdLicenses : ContentPage
    {
        static string fDefaultPrinter = String.Empty;
        ASCTracFunctionStruct.Production.WOHdrType fWORec;
        ASCTracFunctionStruct.Production.ProdNewSkidType newskidRec = new ASCTracFunctionStruct.Production.ProdNewSkidType();
        DataModel.overrideType myOverride = new DataModel.overrideType();
        public pageProdLicenses(ASCTracFunctionStruct.Production.WOHdrType aWORec)
        {
            fWORec = aWORec;
            InitializeComponent();
            Title = "Produced Inventory";
            lblWO.Text = aWORec.Workorder_ID;

            listLicenses.ItemTapped += ListLicenses_ItemTapped;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetLicenses();
        }

        async private void ListLicenses_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                DataModel.dataLocItems myRec = (DataModel.dataLocItems)e.Item;
                var ans = await DisplayActionSheet("Action for license " + myRec.SkidID, null, "Close", "QC Hold", "Print Label");
                if (ans == "QC Hold")
                    await Navigation.PushAsync(new QCHold.pageQCHold(myRec.SkidID));
                else if (ans.Contains( "Label"))
                    await Navigation.PushAsync(new InvFunctions.pageInvCount(myRec.SkidID, "Label"));
            }

        }

        async private void GetLicenses()
        {
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            string errmsg = string.Empty;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(fWORec.Workorder_ID);

                var myReturnData = await App.myRestManager.GetProdSkidList(Globals.curBasicMessage);

                myIndicator.IsRunning = false;
                myIndicator.IsVisible = false;
                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    var xmlResult = myReturnData.DataMessage;
                    if (xmlResult.StartsWith("ER"))
                    {
                        await DisplayAlert("Error", xmlResult.Substring(2), "OK");
                        //lblQty.Text = xmlResult.Substring(2);
                    }
                    else if (xmlResult.StartsWith("OK"))
                    {
                        var xdoc = System.Xml.Linq.XDocument.Parse(xmlResult.Substring(2)).Element("NewDataSet"); //.Element( "Users");

                        Globals.myDatabase.ClearLocItems();
                        foreach (XElement xe in xdoc.Nodes())
                        {
                            bool fValidated = true;
                            string id = xe.Element("SKIDID").Value;
                            double qty = Convert.ToDouble(xe.Element("QTYTOTAL").Value);
                            if ((qty == 0) && (xe.Element("QTY_NOT_VALID") != null))
                            {
                                qty = Convert.ToDouble(xe.Element("QTY_NOT_VALID").Value);
                                fValidated = (qty == 0);
                            }
                            string qahold = xe.Element("QAHOLD").Value;
                            string reason = ascUtils.XGetString(xe.Element("REASONFORHOLD"));

                            //DATETIMEPROD, LOCATIONID, LOTID, EXPDATE, QTY_NOT_VALID

                            DataModel.dataLocItems rec = new DataModel.dataLocItems();
                            rec.SkidID = id;
                            rec.QtyTotal = qty;
                            rec.QAHold = qahold;
                            rec.ReasonForHold = reason;
                            if (xe.Element("DATETIMEPROD") != null)
                                rec.DateTimeProd = Convert.ToDateTime(ascUtils.XGetString(xe.Element("DATETIMEPROD")));
                            rec.LocationID = ascUtils.XGetString(xe.Element("LOCATIONID"));
                            rec.LotID = ascUtils.XGetString(xe.Element("LOTID"));
                            if (xe.Element("EXPDATE") != null)
                                rec.ExpDate = Convert.ToDateTime(ascUtils.XGetString(xe.Element("EXPDATE")));
                            rec.Validated = fValidated;

                            Globals.myDatabase.AddLocItems(rec);
                        }

                        var mylist = Globals.myDatabase.GetLocItems().ToList<DataModel.dataLocItems>();
                        listLicenses.ItemsSource = mylist;
                        lblCount.Text = mylist.Count.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Get License List Exception", ex.Message, "OK");
            }
            if (!String.IsNullOrEmpty(errmsg))
                await DisplayAlert("Get Licences Error", errmsg, "OK");
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            //Navigation.PopAsync();
            (Parent as pageWOTab).CurrentPage = (Parent as pageWOTab).Children[0];
        }

        async private void onLabel(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            var myRec = item.BindingContext as DataModel.dataLocItems;
            await Navigation.PushAsync(new InvFunctions.pageInvCount(myRec.SkidID, "Label"));
        }

        async private void onHold(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            var myRec = item.BindingContext as DataModel.dataLocItems;

            await Navigation.PushAsync(new QCHold.pageQCHold(myRec.SkidID));

        }
    }
}

