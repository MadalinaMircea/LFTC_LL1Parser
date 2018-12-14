using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFTC
{
    class ParsingAlgorithm
    {
        public Dictionary<String, List<String>> FIRST(Grammar grammar)
        {
            Dictionary<String, List<String>> first = new Dictionary<String, List<String>>();

            //create one entry for each terminal
            //initialize it with the terminal
            foreach (string terminal in grammar.Terminals)
            {
                first[terminal] = new List<String>();
                first[terminal].Add(terminal);
            }

            //create one entry for each nonterminal
            foreach (string nonterminal in grammar.Nonterminals)
            {
                first[nonterminal] = new List<String>();
            }

            ////initialize first
            //foreach (Production p in grammar.Productions)
            //{
            //    foreach (string into in p.To)
            //    {
            //        //if the first element "x" in the right hand side of a production "A -> x B" is a terminal, add it to FIRST of the nonterminal "A"
            //        string[] rhs = into.Split(' ');
            //        if(grammar.Terminals.Contains(rhs[0]))
            //        {
            //            first[p.From].Add(rhs[0]);
            //        }
            //    }
            //}

            return first;
        }
    }
}
