using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFTC_LL1Parser
{
    public class Grammar
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

        public Grammar(string filename)
        {
            FromFile(filename);
        }
    
        private void FromFile(string filename)
        {
            Nonterminals = new List<string>();
            Terminals = new List<string>();
            Productions = new List<Production>();

            int count = 1;
            string[] grammar = System.IO.File.ReadAllLines(filename);

            Nonterminals.AddRange(grammar[0].Split(' ').Select(x => x.Trim()).ToList());

            Terminals.AddRange(grammar[1].Split(' ').Select(x => x.Trim()).ToList());

            StartingSymbol = grammar[2];

            for (int i = 3; i < grammar.Length; i++)
            {
                string[] aux = grammar[i].Split('|');

                foreach (string s in aux.Skip(1))
                {
                    Productions.Add(new Production(aux[0].Trim(), s.Trim(), count++));
                }
            }
        }

        public List<Production> GetProductionsWithRHS(string nonterminal)
        {
            return Productions.Where(prod => prod.To.Split(' ').ToList().Contains(nonterminal)).ToList();
        }

        public List<Production> GetProductionsForNonterminal(string nonterminal)
        {
            return Productions.Where(prod => prod.From == nonterminal).ToList();
        }
    }
}
