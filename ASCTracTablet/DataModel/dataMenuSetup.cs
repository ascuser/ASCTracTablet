using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ASCTracTablet.DataModel
{
    public class dataMenuSetup
    {
        [PrimaryKey]
        public string MenuID { get; set; }
        public string Description { get; set; }
        public string MenuParentID { get; set; }
    }
}
