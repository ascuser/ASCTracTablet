using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
//using ; //.Net.Attributes;

namespace ASCTracTablet.DataModel
{
    public class dataConfig
    {
        [PrimaryKey]
        public string cfgname { get; set; }
        public string cfgValue { get; set; }

    }
}
