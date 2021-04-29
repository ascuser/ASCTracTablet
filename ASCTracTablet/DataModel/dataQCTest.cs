using System;
using System.Collections.Generic;
using SQLite; //.Net.Attributes;
using System.Linq;

using Xamarin.Forms;
namespace ASCTracTablet.DataModel
{
    public class dataQCTest
    {
        [PrimaryKey]
        public long BatchNum { get; set; }
        public string TestType { get; set; }
        public string LotID { get; set; }
        public string SkidID { get; set; }
        public DateTime TestDateTime { get; set; }
        public string TestUserID { get; set; }

        public string _TestResults { get; set; }
        //List<string> testResults { get; set; }
        public List<string> TestResults
        {
            get { return _TestResults.Split('|').ToList<string>(); }
        }

        public string testTypeDesc
        {
            get { return TestType == "W" ? "Work Order" : TestType == "L" ? "License: " + SkidID : "LotID: " + LotID; }
        }

        public Color bgColor
        {
            get { return Color.White; }
        }
    }
}
