using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFTC_LL1Parser
{
    class ParsingAlgorithm
    {
        public Dictionary<String, HashSet<String>> FIRST(Grammar grammar)
        {
            string EPSILON = "EPSILON";
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

            //initialize first
            foreach (Production p in grammar.Productions)
            {
                foreach (string into in p.To)
                {
                    //if the first element "x" in the right hand side of a production "A -> x B" is a terminal, add it to FIRST of the nonterminal "A"
                    string[] rhs = into.Split(' ');
                    if (grammar.Terminals.Contains(rhs[0]) || rhs[0] == EPSILON)
                    {
                        first[p.From].Add(rhs[0]);
                    }
                }
            }

            bool changed = true;

            while(changed)
            {
                changed = false;

                foreach (Production p in grammar.Productions)
                {
                    foreach (string into in p.To)
                    {
                        //skip over epsilon productions (we already added them at the initialization)
                        if(into.Trim() == EPSILON)
                        {
                            continue;
                        }

                        //"rhs" is the right hand side of the production "into"
                        string[] rhs = into.Trim().Split(' ');

                        //epsilon is true when FIRST[current rhs] contains EPSILON
                        bool epsilon = true;

                        //"i" is the counter for the current element of the right hand side of the production
                        int i = 0;

                        //loop through FIRST[rhs[i]] while it contains epsilon
                        while (i < rhs.Count() && epsilon)
                        {
                            //assume FIRST[current element of "rhs"] doesn't contain epsilon
                            epsilon = false;

                            //loop through all the elements of FIRST[current element of "rhs"]
                            foreach (string element in first[rhs[i].Trim()])
                            {
                                //if the current element isn't already in FIRST[left hand side of production] and
                                //if the current element isn't EPSILON, add it to FIRST[left hand side of production]
                                if (!first[p.From].Contains(element) && element != EPSILON)
                                {
                                    first[p.From].Add(element);
                                    changed = true;
                                }

                                //if the current element is EPSILON, make epsilon=true so that the while loop
                                //continues and we analyse FIRST[next element from "rhs"]
                                if (element == EPSILON)
                                {
                                    epsilon = true;
                                }
                            }

                            //move to the next element of "rhs"
                            i++;
                        }

                        //if we reached the end of the elements in "rhs" and epsilon is still true
                        //(so if all the FIRSTs contain EPSILON), we add EPSILON to FIRST[left hand side of production]
                        if (i == rhs.Count() && epsilon)
                        {
                            first[p.From].Add(EPSILON);
                        }
                    }
                }
            }

            return first;
        }
    }
}
