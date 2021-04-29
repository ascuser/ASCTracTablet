using System;
using System.Collections.Generic;
using System.Text;
using SQLite;


namespace ASCTracTablet.DataModel
{
    public class data3PLRate
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string Description { get; set; }
        public double fee { get; set; }
    }
}
