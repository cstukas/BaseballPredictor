using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseballTracker.DB
{
    public class Property
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public object OldValue { get; set; }
    }
}
