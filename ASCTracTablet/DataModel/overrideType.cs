using System;
using System.Collections.Generic;
using System.Text;

namespace ASCTracTablet.DataModel
{
    public class overrideType
    {
        public string fMessage { get; set; }
        public string foverridePassword { get; set; }
        public bool fProcess { get; set; }
        public bool fAnswered { get; set; }
        public overrideType()
        {
            fProcess = false;
            fAnswered = false;
        }
    }
}
