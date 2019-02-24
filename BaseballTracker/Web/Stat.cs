using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseballTracker.Web
{
    public class Stat
    {

        public string description = "";
        public string value = "";
        public Stat(string desc, string val)
        {
            description = desc;
            value = val;

        }
    }
}
