using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Xamarin.Forms;

namespace ASCTracTablet
{
    public class ascUtils
    {
        public static string XGetString(XElement xe)
        {
            string retval = string.Empty;
            if (xe != null)
                retval = xe.Value;
            return (retval);
        }
        public static Double XGetDouble(XElement xe)
        {
            Double retval = 0;
            if (xe != null)
                retval = Convert.ToDouble(xe.Value);
            return (retval);
        }

        public static string GetIDFromPicker(Picker apicker)
        {
            string retval = string.Empty;

            if (apicker.SelectedIndex >= 0)
            {
                string tmp = apicker.Items[apicker.SelectedIndex];
                string[] sList = tmp.Split('-');
                retval = sList[0].Trim();
            }


            return (retval);
        }


        public static void setupPicker(Picker aPicker, List<ASCTracFunctionStruct.LookupItemType> aList, string aValue)
        {
            aPicker.ItemsSource = aList;
            aPicker.ItemDisplayBinding = new Binding("Description");
            for (int i = 0; i <= aList.Count - 1; i++)
            {
                if (aList[i].ID == aValue)
                {
                    aPicker.SelectedIndex = i;
                    // aPicker.SelectedItem = aList[i];
                    break;
                }
            }
        }

        /*
        public static void setupPicker(Picker aPicker, List<ascTracWCFService.LookupItemType> aList, string aValue)
        {
            aPicker.ItemsSource = aList;
            aPicker.ItemDisplayBinding = new Binding("Description");
            for (int i = 0; i <= aList.Count - 1; i++)
            {
                if (aList[i].ID == aValue)
                {
                    aPicker.SelectedIndex = i;
                    // aPicker.SelectedItem = aList[i];
                    break;
                }
            }
        }
        */

        public static string getPickerValue(Picker aPicker)
        {
            string aDesc = string.Empty;
            return (getPickerValue(aPicker, ref aDesc));
        }
        public static string getPickerValue(Picker aPicker, ref string aDesc)
        {
            string retval = string.Empty;
            if (aPicker.SelectedItem != null)
            {
                //var myrec = (ascTracWCFService.LookupItemType)aPicker.SelectedItem;
                var myrec = (ASCTracFunctionStruct.LookupItemType)aPicker.SelectedItem;
                retval = myrec.ID;
                aDesc = myrec.Description;
            }
            return (retval);

        }
    }
}

