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
    public partial class pageProdScheduler : ContentPage
    {

        string prodline;
        List<ASCTracFunctionStruct.Production.WOHdrType> myList;

        public pageProdScheduler()
        {
            InitializeComponent();
            Title = "Production Scheduler";

            FillProdlines();
        }

        private void FillProdlines()
        {
            ascUtils.setupPicker(pickProdline, Globals.lookupProdlineList, prodline);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (pickProdline.SelectedIndex >= 0)
                btnRefresh_Clicked(null, null);
        }

        public bool isGridOccupied(int col, int row)
        {
            foreach (View v in gridProdline.Children)
            {
                if ((col == Grid.GetColumn(v)) && (row == Grid.GetRow(v)))
                    return true;
            }
            return false;
        }

        async private void btnRefresh_Clicked(object sender, EventArgs e)
        {
            string errmsg = string.Empty;
            if (pickProdline.SelectedIndex < 0)
                await DisplayAlert("Error", "No Production Line Selected", "OK");
            else
            {
                prodline = ascUtils.getPickerValue(pickProdline);
                myIndicator.IsRunning = true;
                myIndicator.IsVisible = true;
                try
                {
                    Globals.curBasicMessage.inputDataList = new List<string>();
                    Globals.curBasicMessage.inputDataList.Add(prodline);
                    Globals.curBasicMessage.inputDataList.Add(pickDate.Date.ToString());
                    var myReturnData = await App.myRestManager.GetWOHdrList(Globals.curBasicMessage);

                    myIndicator.IsRunning = false;
                    myIndicator.IsVisible = false;
                    if (!myReturnData.successful)
                        errmsg = myReturnData.ErrorMessage;
                    else
                    {
                        myList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ASCTracFunctionStruct.Production.WOHdrType>>(myReturnData.DataMessage);
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
                                      new Setter { Property = Label.BackgroundColorProperty, Value = Color.Gray },
                                      new Setter { Property = Label.TextColorProperty, Value = Color.White },
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


                        gridProdline.Children.Clear();
                        Label myLabel = new Label { Style = styleHdr };
                        myLabel.Text = "Time";
                        gridProdline.Children.Add(myLabel, 0, 0);

                        myLabel = new Label { Style = styleHdr };
                        myLabel.Text = "Work Order";
                        gridProdline.Children.Add(myLabel, 1, 0);


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

                            gridProdline.Children.Add(myLabel, 0, i + 1);
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

                            int col = 1;
                            for (int i = mystarthour + 1; i <= myendhour + 1; i++)
                            {
                                while (isGridOccupied(col, i))
                                    col++;
                            }

                            if (col <= 8)
                            {

                                double ratio = 0;
                                if ((rec.QtyToMake > 0) && (rec.QtyMade > 0))
                                    ratio = (rec.QtyMade / rec.QtyToMake);
                                if (ratio > 1)
                                    ratio = 1;

                                string[] strRGB = rec.BGStatusColor.Split(',');
                                Color myStatusColor = Color.FromRgb(Convert.ToInt32(strRGB[2]), Convert.ToInt32(strRGB[1]), Convert.ToInt32(strRGB[0]));
                                strRGB = rec.FGStatusColor.Split(',');
                                Color myTextColor = Color.FromRgb(Convert.ToInt32(strRGB[2]), Convert.ToInt32(strRGB[1]), Convert.ToInt32(strRGB[0])); // Color.Gray;

                                var myHdrButton = new Button { BackgroundColor = myStatusColor, HorizontalOptions = LayoutOptions.FillAndExpand, TextColor = myTextColor, Text = "WO: " + rec.Workorder_ID };
                                //var myHdrButton = new XLabs.Forms.Controls.ExtendedButton { BackgroundColor = myStatusColor, TextColor = myTextColor, Text = "WO: " + rec.Workorder_ID };
                                //myHdrButton.HorizontalContentAlignment = TextAlignment.Start;
                                //myHdrButton.HorizontalOptions = LayoutOptions.FillAndExpand;
                                myHdrButton.Command = new Command(() => MyButton_Clicked(myHdrButton, null));

                                var myProgBar = new ProgressBar { Progress = ratio, BackgroundColor = Color.Black, Margin = new Thickness(4, 0) };

                                var myButton = new Button { Style = styleDataButton };
                                //var myButton = new XLabs.Forms.Controls.ExtendedButton { Style = styleDataButton };
                                //ascButton myButton = new ascButton { Style = styleDataButton };
                                myButton.BackgroundColor = myStatusColor;
                                myButton.TextColor = myTextColor;
                                int iratio = Convert.ToInt32(Math.Truncate((100 * ratio) + 0.5));
                                myButton.Text = "Completed " + iratio.ToString() + "%\r\nStatus: " + rec.Status_Description;

                                //myButton.HorizontalContentAlignment = TextAlignment.Start;
                                myButton.HorizontalOptions = LayoutOptions.FillAndExpand;
                                myButton.VerticalOptions = LayoutOptions.FillAndExpand;
                                myButton.Command = new Command(() => MyButton_Clicked(myHdrButton, null));

                                var myLayout = new StackLayout { BackgroundColor = myStatusColor, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Fill, Margin = new Thickness(1) };
                                myLayout.Children.Add(myHdrButton);
                                myLayout.Children.Add(myProgBar);
                                myLayout.Children.Add(myButton);

                                var myBox = new BoxView { BackgroundColor = myStatusColor };

                                //gridProdline

                                gridProdline.Children.Add(myBox, col, col + 1, mystarthour + 1, myendhour + 1);
                                gridProdline.Children.Add(myLayout, col, col + 1, mystarthour + 1, myendhour + 1);
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
                    myIndicator.IsRunning = false;
                    myIndicator.IsVisible = false;
                    errmsg = "Exception :" + ex.Message;
                }
                if (!string.IsNullOrEmpty(errmsg))
                    await DisplayAlert("Get Production Lines Exception", errmsg, "OK");

            }
        }

        private void MyButton_Clicked(object sender, EventArgs e)
        {
            string wo = (sender as Button).Text;
            if (wo.StartsWith("WO:"))
                wo = wo.Replace("WO:", "").Trim(); ;
            int idx = wo.IndexOf("\r\n");
            if (idx > 0)
                wo = wo.Substring(0, idx).Trim();

            foreach (var rec in myList)
            {
                if (rec.Workorder_ID == wo)
                {
                    //Navigation.PushAsync(new pageSchedWO(rec, prodline));
                    Navigation.PushAsync(new pageWOTab(rec, prodline));
                    break;
                }
            }
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}