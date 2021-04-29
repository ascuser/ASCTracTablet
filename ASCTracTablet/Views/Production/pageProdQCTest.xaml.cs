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
    public partial class pageProdQCTest : ContentPage
    {
        static string fDefaultPrinter = String.Empty;

        ASCTracFunctionStruct.Production.WOHdrType fWORec;
        ASCTracFunctionStruct.Production.ProdNewSkidType newskidRec = new ASCTracFunctionStruct.Production.ProdNewSkidType();
        DataModel.overrideType myOverride = new DataModel.overrideType();
        public pageProdQCTest(ASCTracFunctionStruct.Production.WOHdrType aWORec)
        {

            fWORec = aWORec;
            Title = "QC Tests";
            InitializeComponent();

            lblWO.Text = fWORec.Workorder_ID;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetQCTests();
        }

        async private void GetQCTests()
        {
            string errmsg = string.Empty;
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;
            try
            {
                Globals.curBasicMessage.inputDataList = new List<string>();
                Globals.curBasicMessage.inputDataList.Add(fWORec.Workorder_ID);
                var myReturnData = await App.myRestManager.GetQCTests(Globals.curBasicMessage);

                myIndicator.IsRunning = false;
                myIndicator.IsVisible = false;
                if (!myReturnData.successful )
                    errmsg = myReturnData.ErrorMessage;
                else if (myReturnData.DataMessage.StartsWith("OK"))
                        {
                    Globals.myDatabase.ClearQCTests();
                    if (myReturnData.DataMessage.Length > 2)
                    {
                        var xdoc = System.Xml.Linq.XDocument.Parse(myReturnData.DataMessage.Substring(2)).Element("NewDataSet"); //.Element( "Users");


                        foreach (XElement xe in xdoc.Nodes())
                        {
                            //Q1.WORKORDER_ID, Q1.LOTID, Q1.SKIDID, Q1.BATCH_NUM, Q1.TEST_USERID, Q1.TEST_DATETIME, Q1.RECTYPE, Q1.PASSFAIL, Q1.PROMPT";
                            // list of QUESTION_NUM#, PROMPT#, ANSWER#, PASSFAIL#, HOLD_REASON#
                            DataModel.dataQCTest rec = new DataModel.dataQCTest();
                            rec.BatchNum = Convert.ToInt32(ascUtils.XGetString(xe.Element("BATCH_NUM")));
                            rec.LotID = ascUtils.XGetString(xe.Element("LOTID"));
                            rec.SkidID = ascUtils.XGetString(xe.Element("SKIDID"));
                            rec.TestType = ascUtils.XGetString(xe.Element("RECTYPE"));
                            rec.TestDateTime = Convert.ToDateTime(ascUtils.XGetString(xe.Element("TEST_DATETIME")));
                            rec.TestUserID = ascUtils.XGetString(xe.Element("TEST_USERID"));
                            rec._TestResults = string.Empty;
                            for (int i = 1; xe.Element("QUESTION_NUM" + i.ToString()) != null; i++)
                            {
                                string testResult = ascUtils.XGetString(xe.Element("QUESTION_NUM" + i.ToString()));
                                testResult += ";" + ascUtils.XGetString(xe.Element("PROMPT" + i.ToString()));
                                testResult += ";" + ascUtils.XGetString(xe.Element("ABSWER" + i.ToString()));
                                testResult += ";" + ascUtils.XGetString(xe.Element("HOLD_REASON" + i.ToString()));
                                if (!String.IsNullOrEmpty(rec._TestResults))
                                    rec._TestResults += "|";
                                rec._TestResults += testResult;
                            }
                            Globals.myDatabase.AddQCTest(rec);
                        }
                    }
                    var mylist = Globals.myDatabase.GetQCTests().ToList<DataModel.dataQCTest>();
                    listQCTests.ItemsSource = mylist;
                }
                else
                    errmsg = "Invalid Data Returned \r\n" + myReturnData.DataMessage;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Get QC Tests Exception", ex.Message, "OK");
            }
            if (!String.IsNullOrEmpty(errmsg))
                await DisplayAlert("Get QC Tests Error", errmsg, "OK");
        }

        private async void onRunTest(object sender, EventArgs e)
        {
            var ans = await DisplayActionSheet("Run QC Test against", "Cancel", null, "Work Order", "Lot", "License");
            string QCTestType = string.Empty;
            if (ans == "Work Order")
                QCTestType = "W";
            if (ans == "Lot")
                QCTestType = "L";
            if (ans == "License")
                QCTestType = "S";
            if (!String.IsNullOrEmpty(QCTestType))
                await Navigation.PushAsync(new QCHold.pageQCTest(fWORec, QCTestType));
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                if (Parent is pageWOTab)
                    (Parent as pageWOTab).CurrentPage = (Parent as pageWOTab).Children[0];
            }
            else
                Navigation.PopAsync();
        }
    }
}

