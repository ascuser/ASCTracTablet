using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.DockSchd
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageDockScheduler : ContentPage
    {
        private static string LoadingBay;
        List<ASCTracFunctionStruct.CustOrder.DockType> myList;
        public pageDockScheduler()
        {
            InitializeComponent();

            pickDate.Date = DateTime.Now.Date;
            btnRefresh.IsEnabled = false;
            btnAdd.IsEnabled = false;
            FillDocks();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (btnRefresh.IsEnabled)
                btnRefresh_Clicked(null, null);
        }

        private void FillDocks()
        {
            ascUtils.setupPicker(pickDock, Globals.dockList, LoadingBay);
        }

        public bool isGridOccupied(int col, int row)
        {
            foreach (View v in gridDocks.Children)
            {
                if ((col == Grid.GetColumn(v)) && (row == Grid.GetRow(v)))
                    return true;
            }
            return false;
        }
        private void pickDock_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRefresh.IsEnabled = true;
        }

        async private void btnRefresh_Clicked(object sender, EventArgs e)
        {
            //LoadingBay = ascUtils.GetIDFromPicker(pickDock);
            LoadingBay = ascUtils.getPickerValue(pickDock);
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;

            string errmsg = string.Empty;
            try
            {
                Globals.curBasicMessage.DataMessage = LoadingBay + "|" + pickDate.Date.ToShortDateString();
                var myReturnData = await App.myRestManager.GetDockSchd(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    myList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.CustOrder.DockType>>(myReturnData.DataMessage);
                    btnAdd.IsEnabled = true;

                    gridDocks.Children.Clear();
                    if (myList.Count == 0)
                        errmsg = "No Active Orders";
                    else
                    {
                        var styleHdr = new Style(typeof(Label))
                        {
                            Setters = {
                                      new Setter { Property = Label.BackgroundColorProperty, Value = Color.Blue },
                                      new Setter { Property = Label.TextColorProperty, Value = Color.White },
                                      new Setter { Property = Label.FontSizeProperty, Value = 16 }//,
                                    }
                        };
                        var styleTime = new Style(typeof(Label))
                        {
                            Setters = {
                                      new Setter { Property = Label.BackgroundColorProperty, Value =  (Color) App.Current.Resources["colorGridBackGround"] },
                                      new Setter { Property = Label.TextColorProperty, Value = Color.Black },
                                      new Setter { Property = Label.FontSizeProperty, Value = 16 },
                                      new Setter { Property = Label.HorizontalTextAlignmentProperty, Value="End"}
                                    }
                        };
                        var styleData = new Style(typeof(Label))
                        {
                            Setters = {
                                      new Setter { Property = Label.BackgroundColorProperty, Value = Color.Green },
                                      new Setter { Property = Label.TextColorProperty, Value = Color.White },
                                      new Setter { Property = Label.FontSizeProperty, Value = 16 },
                                    }
                        };
                        var styleDataButton = new Style(typeof(Button))
                        {
                            Setters = {
                                      new Setter { Property = Button.BackgroundColorProperty, Value = Color.Green },
                                      new Setter { Property = Button.TextColorProperty, Value = Color.White },
                                      new Setter { Property = Button.FontSizeProperty, Value = 14 },
                                    }
                        };

                        Label myLabel = new Label { Style = styleHdr };
                        myLabel.Text = "Time";
                        gridDocks.Children.Add(myLabel, 0, 0);

                        myLabel = new Label { Style = styleHdr };
                        myLabel.Text = "Order";
                        gridDocks.Children.Add(myLabel, 1, 0);


                        for (int i = 0; i < 24; i += 1)
                        {
                            myLabel = new Label { Style = styleTime };
                            myLabel.HorizontalTextAlignment = TextAlignment.End;
                            if (i == 0)
                                myLabel.Text = "12AM";
                            else if (i < 12)
                                myLabel.Text = i.ToString() + "AM";
                            else if (i == 12)
                                myLabel.Text = i.ToString() + "PM";
                            else
                                myLabel.Text = (i - 12).ToString() + "PM";

                            gridDocks.Children.Add(myLabel, 0, i + 1);
                        }

                        // display workorders on grid.
                        foreach (var rec in myList)
                        {
                            int mystarthour = rec.Sched_Datetime.Hour;
                            int myendhour = mystarthour + Convert.ToInt32(rec.Duration / 60) - 1;
                            if ((myendhour < 0) || (myendhour <= mystarthour))
                                myendhour = mystarthour + 1;
                            myendhour += 1;
                            if (myendhour > 24)
                                myendhour = 23;

                            double ratio = rec.Percent_Complete / 100;
                            if (ratio > 1)
                                ratio = 1;

                            string orderStr = string.Empty;
                            orderStr = rec.OrderTypeDesc;
                            if (!String.IsNullOrEmpty(rec.OrderNumber))
                                orderStr += ": " + rec.OrderNumber;
                            /*
                            if (rec.OrderType.Equals("P"))
                                orderStr = "PO: " + rec.OrderNumber;
                            else
                                orderStr = "Order: " + rec.OrderNumber;
                                */
                            string[] strRGB = rec.BGStatusColor.Split(',');
                            Color myStatusColor = Color.FromRgb(Convert.ToInt32(strRGB[2]), Convert.ToInt32(strRGB[1]), Convert.ToInt32(strRGB[0]));
                            strRGB = rec.FGStatusColor.Split(',');
                            Color myTextColor = Color.FromRgb(Convert.ToInt32(strRGB[2]), Convert.ToInt32(strRGB[1]), Convert.ToInt32(strRGB[0])); // Color.Gray;
                            var myHdrButton = new Button { BackgroundColor = myStatusColor, HorizontalOptions = LayoutOptions.FillAndExpand, TextColor = myTextColor, Text = orderStr };
                            //var myHdrButton = new XLabs.Forms.Controls.ExtendedButton { BackgroundColor = myStatusColor, TextColor = myTextColor, Text = orderStr };
                            //myHdrButton.HorizontalContentAlignment = TextAlignment.Start;
                            //myHdrButton.Clicked += MyButton_Clicked;

                            var myProgBar = new ProgressBar { Progress = ratio, BackgroundColor = Color.Black, Margin = new Thickness(4, 0) };

                            strRGB = rec.BGOrderTypeColor.Split(',');
                            myStatusColor = Color.FromRgb(Convert.ToInt32(strRGB[2]), Convert.ToInt32(strRGB[1]), Convert.ToInt32(strRGB[0]));
                            strRGB = rec.FGOrderTypeColor.Split(',');
                            myTextColor = Color.FromRgb(Convert.ToInt32(strRGB[2]), Convert.ToInt32(strRGB[1]), Convert.ToInt32(strRGB[0])); // Color.Gray;

                            int iratio = Convert.ToInt32(Math.Truncate(rec.Percent_Complete));
                            string DisplayData = "Completed " + iratio.ToString() + "%\r\nStatus: " + rec.PickStatus_Description +
                                                "\r\nScheduleID: " + rec.SchedID;

                            var myButton = new Button { Style = styleDataButton };
                            //var myButton = new XLabs.Forms.Controls.ExtendedButton { Style = styleDataButton };
                            //ascButton myButton = new ascButton { Style = styleDataButton };
                            myButton.BackgroundColor = myStatusColor;
                            myButton.TextColor = myTextColor;
                            myButton.Text = DisplayData;

                            //myButton.HorizontalContentAlignment = TextAlignment.Start;
                            myButton.HorizontalOptions = LayoutOptions.FillAndExpand;
                            myButton.VerticalOptions = LayoutOptions.FillAndExpand;

                            myButton.Command = new Command(() => MyButton_Clicked(myButton, null));
                            myHdrButton.Command = new Command(() => MyButton_Clicked(myButton, null));
                            //myButton.Clicked += MyButton_Clicked;

                            Label myDetLabel = new Label { Style = styleData, BackgroundColor = myStatusColor, Text = DisplayData, TextColor = myTextColor };

                            var myLayout = new StackLayout { BackgroundColor = myStatusColor, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Fill, Margin = new Thickness(1) };
                            myLayout.Children.Add(myHdrButton);
                            myLayout.Children.Add(myProgBar);
                            myLayout.Children.Add(myButton);

                            var myBox = new BoxView { BackgroundColor = myStatusColor };

                            int col = 1;
                            for (int i = mystarthour + 1; i <= myendhour + 1; i++)
                            {
                                while (isGridOccupied(col, i))
                                    col++;
                            }
                            //gridProdline

                            gridDocks.Children.Add(myBox, col, col + 1, mystarthour + 1, myendhour + 1);
                            gridDocks.Children.Add(myLayout, col, col + 1, mystarthour + 1, myendhour + 1);
                            /*
                            var myButton = new XLabs.Forms.Controls.ExtendedButton { Style = styleDataButton };
                            //ascButton myButton = new ascButton { Style = styleDataButton };
                            myButton.BackgroundColor = myStatusColor;
                            myButton.TextColor = myTextColor;
                            int iratio = Convert.ToInt32(Math.Truncate((100 * ratio) + 0.5));
                            myButton.Text = "Wo: " + rec.Workorder_ID + "\r\nCompleted " + iratio.ToString() + "%\r\nStatus: " + rec.Status_Description;

                            myButton.HorizontalContentAlignment = TextAlignment.Start;
                            myButton.HorizontalOptions = LayoutOptions.StartAndExpand;
                            myButton.Clicked += MyButton_Clicked;
                            */
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errmsg = "Exception :" + ex.Message;
            }
            myIndicator.IsRunning = false;
            myIndicator.IsVisible = false;
            if (!string.IsNullOrEmpty(errmsg))
                await DisplayAlert(Globals.AppTitleName, errmsg, "OK");

        }

        async private void MyButton_Clicked(object sender, EventArgs e)
        {
            string schedID = (sender as Button).Text;
            int idx = schedID.LastIndexOf(":");
            schedID = schedID.Substring(idx + 1).Trim();
            foreach (var rec in myList)
            {
                if (rec.SchedID.Equals(schedID))
                {
                    try
                    {
                        if (rec.OrderType.Equals("C"))
                        {
                            //await Navigation.PushAsync(new COSM.pageCODetailTab(rec.OrderNumber));
                        }
                        else //if (rec.OrderType.Equals("P") || rec.OrderType.Equals("R"))
                            await Navigation.PushAsync(new pageSched(rec));
                        //await Navigation.PushAsync(new Recv.pageCloseRecvTab(rec.OrderType, rec.OrderNumber));
                        //else //if (rec.OrderType.Equals("P"))
                        //{
                        //Navigation.PushAsync(new Recv.pageCloseRecv
                        //    await DisplayAlert("Not Functional", "Schedule for " + rec.OrderTypeDesc + ": " + rec.OrderNumber, "OK");
                        //}
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Opening Scheduler Exception", ex.ToString(), "OK");
                    }
                    break;
                }
            }
        }

        async private void btnAdd_Clicked(object sender, EventArgs e)
        {
            // Get Order Type
            // Get Order Number
            LoadingBay = ascUtils.getPickerValue(pickDock);
            await Navigation.PushAsync(new pageNewDockSchd(LoadingBay, pickDate.Date));
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
