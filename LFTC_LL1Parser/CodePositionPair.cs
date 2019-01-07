using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFTC_LL1Parser
{
    public class CodePositionPair
    {
        public int Code { get; set; }
        public Pair<int, int> Position { get; set; }

        public CodePositionPair(int c, int pi, int pj)
        {
            Code = c;
            Position = new Pair<int, int> { x = pi, y = pj };
        }
    }
}
