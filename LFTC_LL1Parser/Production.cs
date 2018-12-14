using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFTC_LL1Parser
{
    class Production
    {
        public String From { get; set; }
        public List<String> To { get; set; }

        public Production(string from, List<string> to)
        {
            From = from;
            To = to;
        }
    }
}
