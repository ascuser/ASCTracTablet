using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ASCTracTablet.DataModel
{
    public class dataReasonCode
    {
        [PrimaryKey]
        public string ReasonCode { get; set; }
        public string Description { get; set; }
        public string ReasonType { get; set; }
        public string MAFFlag { get; set; }
        public string askCostCenter { get; set; }
        public string defaultCostCenter { get; set; }
        public string askRespSite { get; set; }
        public string askComments { get; set; }
        public string askProjectNumber { get; set; }

    }
}
