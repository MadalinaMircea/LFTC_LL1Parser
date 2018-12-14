using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFTC_LL1Parser
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

        public Grammar(string filename)
        {
            FromFile(filename);
        }

        private void AddProduction(string from, string to)
        {
            bool found = false;
            for(int i = 0; i < Productions.Count(); i++)
            {
                if(Productions[i].From == from)
                {
                    Productions[i].To.Add(to);
                    found = true;
                    break;
                }
            }

            if(!found)
            {
                List<String> p = new List<String>();
                p.Add(to);
                Productions.Add(new Production(from, p));
            }
        }

        private void FromFile(string filename)
        {
            Nonterminals = new List<string>();
            Terminals = new List<string>();
            Productions = new List<Production>();

            string[] grammar = System.IO.File.ReadAllLines(filename);

            Nonterminals.AddRange(grammar[0].Split(',').Select(x => x.Trim()).ToList());

            Terminals.AddRange(grammar[1].Split(',').Select(x => x.Trim()).ToList());

            StartingSymbol = grammar[2];

            for (int i = 3; i < grammar.Length; i++)
            {
                string[] aux = grammar[i].Split('-');
                if (aux[1].Contains('|'))
                {
                    foreach (string s in aux[1].Split('|'))
                    {
                        AddProduction(aux[0].Trim(), s.Trim());
                    }
                }
                else
                {
                    AddProduction(aux[0].Trim(), aux[1].Trim());
                }
            }
        }
    }
}
