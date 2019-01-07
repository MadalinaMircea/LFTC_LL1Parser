using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFTC_LL1Parser
{
    public static class ParsingAlgorithm
    {
        private const string EPSILON = "EPSILON";
        private const string DOLLAR = "$";

        public static Dictionary<String, HashSet<String>> First(Grammar grammar)
        {           
            Dictionary<String, HashSet<String>> first = new Dictionary<String, HashSet<String>>();

            //create one entry for each terminal
            //initialize it with the terminal
            foreach (string terminal in grammar.Terminals)
            {
                first[terminal] = new HashSet<String>();
                first[terminal].Add(terminal);
            }

            //create one entry for each nonterminal
            foreach (string nonterminal in grammar.Nonterminals)
            {
                first[nonterminal] = new HashSet<String>();
            }

            //initialize First
            foreach (Production p in grammar.Productions)
            {
                //foreach (string into in p.To)
                //{
                    //if the First element "x" in the right hand side of a production "A -> x B" is a terminal, add it to First of the nonterminal "A"
                    string[] rhs = p.To.Split(' ');
                    if (grammar.Terminals.Contains(rhs[0]) || rhs[0] == EPSILON)
                    {
                        first[p.From].Add(rhs[0]);
                    }
                //}
            }

            bool changed = true;

            while(changed)
            {
                changed = false;

                foreach (Production p in grammar.Productions)
                {
                    //foreach (string into in p.To)
                    //{
                        //skip over epsilon productions (we already added them at the initialization)
                        if(p.To.Trim() == EPSILON)
                        {
                            continue;
                        }

                        //"rhs" is the right hand side of the production "into"
                        string[] rhs = p.To.Trim().Split(' ');

                        //epsilon is true when First[current rhs] contains EPSILON
                        bool epsilon = true;

                        //"i" is the counter for the current element of the right hand side of the production
                        int i = 0;

                        //loop through First[rhs[i]] while it contains epsilon
                        while (i < rhs.Count() && epsilon)
                        {
                            //assume First[current element of "rhs"] doesn't contain epsilon
                            epsilon = false;

                            //loop through all the elements of First[current element of "rhs"]
                            foreach (string element in first[rhs[i].Trim()])
                            {
                                //if the current element isn't already in First[left hand side of production] and
                                //if the current element isn't EPSILON, add it to First[left hand side of production]
                                if (!first[p.From].Contains(element) && element != EPSILON)
                                {
                                    first[p.From].Add(element);
                                    changed = true;
                                }

                                //if the current element is EPSILON, make epsilon=true so that the while loop
                                //continues and we analyse First[next element from "rhs"]
                                if (element == EPSILON)
                                {
                                    epsilon = true;
                                }
                            }

                            //move to the next element of "rhs"
                            i++;
                        }

                        //if we reached the end of the elements in "rhs" and epsilon is still true
                        //(so if all the Firsts contain EPSILON), we add EPSILON to First[left hand side of production]
                        if (i == rhs.Count() && epsilon)
                        {
                            first[p.From].Add(EPSILON);
                        }
                    //}
                }
            }

            first[EPSILON] = new HashSet<string>();
            first[EPSILON].Add(EPSILON);
            return first;
        }

        public static Dictionary<String, HashSet<String>> Follow(Grammar grammar)
        {
            Dictionary<String, HashSet<String>> follow = new Dictionary<String, HashSet<String>>();
            Dictionary<String, HashSet<String>> first = First(grammar);
            bool changed = false;

            foreach (string nonterminal in grammar.Nonterminals)
            {           
                follow[nonterminal] = new HashSet<String>();
            }

            follow[grammar.StartingSymbol].Add(EPSILON);
       
            do
            {
                changed = false;
                foreach(string nonterm in grammar.Nonterminals)
                {
                    int initialLength = follow[nonterm].Count();
                    List<Production> productions = grammar.GetProductionsWithRHS(nonterm);
                    foreach(Production prod in productions)
                    {
                        string[] rhs = prod.To.Split(' ');
                        int pos = rhs.ToList().FindIndex(nt => nt == nonterm);
                        if (pos == rhs.Length - 1)
                        {
                            if (follow[prod.From].Count != 0)
                            {
                                follow[prod.From].ToList().ForEach(elem => follow[nonterm].Add(elem));
                            }
                        }
                        else
                        {
                            bool containsEpsilon = false;
                            first[rhs[pos + 1]].ToList().ForEach(elem => {
                                if (elem != EPSILON)
                                    follow[nonterm].Add(elem);
                                else
                                    containsEpsilon = true;
                            });

                            if(containsEpsilon)
                            {
                                follow[prod.From].ToList().ForEach(elem => follow[nonterm].Add(elem));
                            }
                        }
                    }
                    if (follow[nonterm].Count() != initialLength)
                        changed = true;
                }
            }
            while (changed == true);

            return follow;
        }

        public static Dictionary<string, Dictionary<string, Rule>> ParseTable(Grammar grammar)
        {
            Dictionary<String, HashSet<String>> first = First(grammar);
            Dictionary<String, HashSet<String>> follow = Follow(grammar);

            Dictionary<string, Dictionary<string, Rule>> parseTable = new Dictionary<string, Dictionary<string, Rule>>();
            foreach(string nonterminal in grammar.Nonterminals)
            {
                Dictionary<string, Rule> columns = new Dictionary<string, Rule>();
                foreach (string terminal in grammar.Terminals)
                {
                    columns[terminal] = new Rule()
                    {
                        ProductionRHS = "err",
                        ProductionIndex = 0
                    };

                    foreach (Production production in grammar.GetProductionsForNonterminal(nonterminal))
                    {
                        string[] rhs = production.To.Split(' ');
                        if(first[rhs[0]].Contains(terminal))
                        {
                            columns[terminal] = new Rule()
                            {
                                ProductionRHS = production.To,
                                ProductionIndex = production.Index
                            };
                            break;
                        }

                        if (follow[nonterminal].Contains(terminal) && first[rhs[0]].Contains(EPSILON))
                        {
                            columns[terminal] = new Rule()
                            {
                                ProductionRHS = production.To,
                                ProductionIndex = production.Index
                            };
                            break;
                        }                
                    }
                }
                columns[DOLLAR] = new Rule()
                {
                    ProductionRHS = "err",
                    ProductionIndex = 0
                };
                parseTable[nonterminal] = columns;
            }

            foreach (string terminalLeft in grammar.Terminals)
            {
                Dictionary<string, Rule> columns = new Dictionary<string, Rule>();
                foreach (string terminalRight in grammar.Terminals)
                {
                    columns[terminalRight] = new Rule()
                    {
                        ProductionRHS = "err",
                        ProductionIndex = 0
                    };

                    if(terminalLeft == terminalRight)
                    {
                        columns[terminalRight] = new Rule()
                        {
                            ProductionRHS = "pop",
                            ProductionIndex = 0
                        };
                    }
                }
                parseTable[terminalLeft] = columns;
            }

            foreach (Production production in grammar.Productions)
            {
                if(production.To == EPSILON)
                {
                    parseTable[production.From][DOLLAR] = new Rule()
                    {
                        ProductionRHS = EPSILON,
                        ProductionIndex = production.Index
                    };
                }
            }

            Dictionary<string, Rule> dollarColumns = new Dictionary<string, Rule>();
            foreach (string terminal in grammar.Terminals)
            {
                dollarColumns[terminal] = new Rule()
                {
                    ProductionRHS = "err",
                    ProductionIndex = 0
                };
            }
            dollarColumns[DOLLAR] = new Rule()
            {
                ProductionRHS = "acc",
                ProductionIndex = 0
            };
            parseTable[DOLLAR] = dollarColumns;

            return parseTable;
        }

        public static List<int> LL1(Dictionary<string, Dictionary<string, Rule>> parseTable, Grammar grammar, string sequence)
        {
            try
            {
                Stack<string> alfa = new Stack<string>();
                Stack<string> beta = new Stack<string>();
                List<int> pi = new List<int>();

                alfa.Push(DOLLAR);
                for (int i = sequence.Length - 1; i >= 0; i--)
                    alfa.Push(sequence[i].ToString());

                beta.Push(DOLLAR);
                beta.Push(grammar.StartingSymbol);

                bool go = true, isAccepted = false;
                while (go)
                {
                    string row = beta.Peek() == EPSILON ? DOLLAR : beta.Peek();
                    string column = alfa.Peek() == EPSILON ? DOLLAR : alfa.Peek();
                    Rule rule = parseTable[row][column];
                    if (rule.ProductionIndex != 0)
                    {
                        beta.Pop();
                        if (rule.ProductionRHS != EPSILON)
                        {
                            string[] rhs = rule.ProductionRHS.Split(' ');
                            for (int i = rhs.Length - 1; i >= 0; i--)
                                beta.Push(rhs[i].ToString());
                            pi.Add(rule.ProductionIndex);
                        }
                    }
                    else if (rule.ProductionRHS == "pop")
                    {
                        beta.Pop();
                        alfa.Pop();
                    }
                    else if (rule.ProductionRHS == "acc")
                    {
                        go = false;
                        isAccepted = true;
                    }
                    else
                    {
                        go = false;
                        isAccepted = false;
                    }
                }

                if (isAccepted == false)
                    return null;

                return pi;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<int> LL1(Dictionary<string, Dictionary<string, Rule>> parseTable, Grammar grammar, ProgramInternalForm pif)
        {
            //try
            //{
                Stack<string> alfa = new Stack<string>();
                Stack<string> beta = new Stack<string>();
                List<int> pi = new List<int>();
                bool go = true, isAccepted = false;

                alfa.Push(DOLLAR);
                for (int i = pif.program.Count - 1; i >= 0; i--)
                {
                    int code = pif.program[i].Code;

                    if (code == 0)
                        alfa.Push("IDENTIFIER");
                    else if (code == 1)
                        alfa.Push("CONSTANT");
                    else
                        alfa.Push(LexicalAnalyzer.reverseCodeTable[pif.program[i].Code]);
                }

                beta.Push(DOLLAR);
                beta.Push(grammar.StartingSymbol);


                while (go)
                {
                    string row = beta.Peek() == EPSILON ? DOLLAR : beta.Peek();
                    string column = alfa.Peek() == EPSILON ? DOLLAR : alfa.Peek();
                    Rule rule = parseTable[row][column];
                    if (rule.ProductionIndex != 0)
                    {
                        beta.Pop();
                        if (rule.ProductionRHS != EPSILON)
                        {
                            string[] rhs = rule.ProductionRHS.Split(' ');
                            for (int i = rhs.Length - 1; i >= 0; i--)
                                beta.Push(rhs[i].ToString());
                            pi.Add(rule.ProductionIndex);
                        }
                    }
                    else if (rule.ProductionRHS == "pop")
                    {
                        beta.Pop();
                        alfa.Pop();
                    }
                    else if (rule.ProductionRHS == "acc")
                    {
                        go = false;
                        isAccepted = true;
                    }
                    else
                    {
                        go = false;
                        isAccepted = false;
                    }
                }

                if (isAccepted == false)
                    return null;

                return pi;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
        }
    }
}
