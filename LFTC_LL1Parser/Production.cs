using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFTC_LL1Parser
{
    public class Production
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Index { get; set; }

        public Production(string from, string to, int index)
        {
            From = from;
            To = to;
            Index = index;
        }
    }
}
