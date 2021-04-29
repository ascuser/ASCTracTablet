using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ASCTracTablet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pageAbout : ContentPage
    {
        public pageAbout()
        {
            InitializeComponent();

            string strBuildDT = DateTime.Now.ToString();
            try
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(pageAbout)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("ascCOSM.BuildDate.txt");
                string text = "";
                using (var reader = new System.IO.StreamReader(stream))
                {
                    strBuildDT = reader.ReadLine().Trim(); //.ReadToEnd();
                }
                //var assembly = Assembly.GetExecutingAssembly();
                //Stream mystream = assembly.GetManifestResourceStream("BuildDate.txt");
                //using (StreamReader sr = new StreamReader( Open("BuildDate.txt")))
                //using (var reader = new System.IO.StreamReader(mystream))
                //{
                //    strBuildDT = reader.ReadToEnd();
                //}
            }
            catch //(Exception ex)
            {
            }
            lblDate.Text = strBuildDT;
        }
    }
}