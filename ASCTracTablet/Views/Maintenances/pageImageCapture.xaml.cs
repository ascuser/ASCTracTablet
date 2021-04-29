using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.Maintenances
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageImageCapture : ContentPage
    {
        string fRecType;
        string fRecID;
        private static string DefDocType = "1";
        private MemoryStream msPhoto = null;

        public pageImageCapture(string aRecType, string aRecID)
        {
            InitializeComponent();
            fRecID = aRecID.ToUpper();
            fRecType = aRecType;

            if (fRecType.Equals("O"))
                lblOrderLabel.Text = "Customer Order: ";
            if (fRecType.Equals("R"))
                lblOrderLabel.Text = "Purchase Order: ";
            lblOrderNumber.Text = fRecID;
            ascUtils.setupPicker(pickDocType, Globals.imageCaptureDocTypeList, DefDocType);
        }

        async private void BtnSave_Clicked(object sender, EventArgs e)
        {
            //myImage
            DefDocType = ascUtils.getPickerValue(pickDocType);
            myIndicator.IsRunning = true;
            myIndicator.IsVisible = true;

            string errmsg = string.Empty;
            try
            {
                ASCTracFunctionStruct.MaintType myMaintType = new ASCTracFunctionStruct.MaintType(fRecType, fRecID, DefDocType, "");
                myMaintType.anImage = msPhoto.ToArray();

                Globals.curBasicMessage.DataMessage = Newtonsoft.Json.JsonConvert.SerializeObject(myMaintType);
                var myReturnData = await App.myRestManager.ImageCapture(Globals.curBasicMessage);

                if (!myReturnData.successful)
                    errmsg = myReturnData.ErrorMessage;
                else
                {
                    myImage.Source = null;
                    btnSave.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                errmsg = "Exception\r\n" + ex.Message;
            }
            myIndicator.IsRunning = false;
            myIndicator.IsVisible = false;
            if (!String.IsNullOrEmpty(errmsg))
                await DisplayAlert("Exception", errmsg, "OK");
        }

        async private void BtnImagee_Clicked(object sender, EventArgs e)
        {
            string status = "A";
            try
            {
                var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });
                status = "b";
                if (photo != null)
                {
                    status = "c";
                    myImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
                    status = "d";
                    msPhoto = new MemoryStream();
                    photo.GetStream().CopyTo(msPhoto);
                    status = "e";
                    btnSave.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(Globals.AppTitleName + "(" + status + ")", ex.Message, "OK");
            }
        }

        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
