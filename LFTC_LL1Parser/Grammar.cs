using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFTC
{
    class Grammar
    {
        public List<String> Terminals { get; set; }
        public List<String> Nonterminals { get; set; }
        public string StartingSymbol { get; set; }
        public List<Production> Productions { get; set; }

        public Grammar()
        {
            Terminals = new List<String>();
            Nonterminals = new List<String>();
            StartingSymbol = "";
            Productions = new List<Production>();
        }

        public Grammar(List<String> terminals, List<String> nonterminals, string startingSymbol, List<Production> productions)
        {
            Terminals = terminals;
            Nonterminals = nonterminals;
            StartingSymbol = startingSymbol;
            Productions = productions;
        }
    }
}
