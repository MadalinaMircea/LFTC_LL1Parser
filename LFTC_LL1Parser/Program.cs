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
            Grammar g = new Grammar("C:\\Users\\Madalina Mircea\\Documents\\Visual Studio 2015\\Projects\\LFTC_LL1Parser\\LFTC_LL1Parser\\LFTC_LL1Parser\\grammar2.txt");

            ParsingAlgorithm p = new ParsingAlgorithm();

            foreach(KeyValuePair<String, HashSet<String>> pair in p.FIRST(g))
            {
                Console.Write(pair.Key + ": ");
                foreach(String s in pair.Value)
                {
                    Console.Write(s + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
