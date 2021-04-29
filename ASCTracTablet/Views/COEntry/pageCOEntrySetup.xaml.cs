using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ASCTracTablet.Views.COEntry
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageCOEntrySetup : ContentPage
    {
        public pageCOEntrySetup()
        {
            InitializeComponent();
            InitScreen();
        }

        public void InitScreen()
        {

            ascUtils.setupPicker(cbCustType, pageCOEntry.setupData.CustTypeList, pageCOEntry.setupData.custType);
            ascUtils.setupPicker(cbItemType, pageCOEntry.setupData.ItemTypeList, pageCOEntry.setupData.itemType);

            edCustTypeData.Text = pageCOEntry.setupData.custTypeData;
            edItemTypeData.Text = pageCOEntry.setupData.itemTypeData;
        }


        private void BtnEsc_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            pageCOEntry.setupData.custType = ascUtils.getPickerValue(cbCustType);
            pageCOEntry.setupData.itemType = ascUtils.getPickerValue(cbItemType);
            pageCOEntry.setupData.custTypeData = edCustTypeData.Text;
            pageCOEntry.setupData.itemTypeData = edItemTypeData.Text;
            pageCOEntry.setupData.SaveConfig();
            Navigation.PopAsync();
        }
    }
}
