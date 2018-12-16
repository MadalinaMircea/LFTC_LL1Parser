using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFTC_LL1Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Grammar g = new Grammar("grammar.txt");

            //Console.WriteLine("FIRST");
            //foreach (KeyValuePair<String, HashSet<String>> pair in ParsingAlgorithm.First(g))
            //{
            //    Console.Write(pair.Key + ": ");
            //    foreach (String s in pair.Value)
            //    {
            //        Console.Write(s + " ");
            //    }
            //    Console.WriteLine();
            //}

            //Console.WriteLine("\nFOLLOW");
            //foreach (KeyValuePair<String, HashSet<String>> pair in ParsingAlgorithm.Follow(g))
            //{
            //    Console.Write(pair.Key + ": ");
            //    foreach (String s in pair.Value)
            //    {
            //        Console.Write(s + " ");
            //    }
            //    Console.WriteLine();
            //}

            Dictionary<string, Dictionary<string, Rule>> parseTable = ParsingAlgorithm.ParseTable(g);
            //foreach(KeyValuePair<string, Dictionary<string, Rule>> pair in parseTable)
            //{
            //    foreach (KeyValuePair<string, Rule> cell in pair.Value)
            //    {
            //        Console.WriteLine(String.Format("{0},{1} -> {2},{3}", pair.Key, cell.Key, cell.Value.ProductionRHS, cell.Value.ProductionIndex));
            //    }
            //}

            while(true)
            {
                Console.Write("Sequence: ");
                string sequence = Console.ReadLine();
                List<int> result = ParsingAlgorithm.LL1(parseTable, g, sequence);
                if(result != null)
                {
                    Console.WriteLine("Sequence accepted!\n");
                    foreach(int index in result)
                    {
                        Console.Write(index + " ");
                    }
                    Console.WriteLine("\n");
                }
                else
                {
                    Console.WriteLine("Sequence NOT accepted!\n");
                }

            }
            Console.WriteLine("\n\n\n\n");
        }
    }
}
