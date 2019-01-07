using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFTC_LL1Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Start();
        }

        private static void Start()
        {
            LexicalAnalyzer lexicalAnalyzer = new LexicalAnalyzer();

            bool keepAlive = true;

            while (keepAlive)
            {
                Console.Write("Type:\n\t'1'-Character Sequence\n\t'2'-Program\n\t'0'-Exit\nGive command: ");
                string option = Console.ReadLine();

                switch (option.Trim())
                {
                    case "1":
                        ReadSequences();
                        break;
                    case "2":
                        ReadPrograms();
                        break;
                    case "0":
                        keepAlive = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
            }
        }

        private static void ReadPrograms()
        {
            Console.Write("Give path to grammar: ");
            string path = Console.ReadLine();
            Grammar g = new Grammar(path);

            Dictionary<string, Dictionary<string, Rule>> parseTable = ParsingAlgorithm.ParseTable(g);

            bool keepAlive = true;

            while (keepAlive)
            {
                Console.Write("Type:\n\t'1'-Read Program\n\t'0'-Go Back\nGive command: ");
                string option = Console.ReadLine();
                switch (option.Trim())
                {
                    case "1":
                        AnalyseProgram(g, parseTable);
                        break;
                    case "0":
                        keepAlive = false;
                        break;
                    default:
                        Console.WriteLine("Invalid command.");
                        break;
                }
            }
        }

        private static void AnalyseProgram(Grammar g, Dictionary<string, Dictionary<string, Rule>> parseTable)
        {
            Console.Write("Give path to program: ");
            string path = Console.ReadLine();
            string program = File.ReadAllText(path);
            
            SymbolTable sti = new SymbolTable();
            SymbolTable stc = new SymbolTable();
            ProgramInternalForm pif = new ProgramInternalForm();
            LexicalAnalyzer.Analyze(program, sti, stc, pif);

            Console.WriteLine(pif);

            List<int> result = ParsingAlgorithm.LL1(parseTable, g, pif);
            if (result != null)
            {
                Console.WriteLine("Sequence accepted!\n");
                foreach (int index in result)
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

        private static void ReadSequences()
        {
            Console.Write("Give path to grammar: ");
            string path = Console.ReadLine();
            Grammar g = new Grammar(path);

            Dictionary<string, Dictionary<string, Rule>> parseTable = ParsingAlgorithm.ParseTable(g);

            bool keepAlive = true;

            while (keepAlive)
            {
                Console.Write("Type:\n\t'1'-Read Sequence\n\t'0'-Go Back\nGive command: ");
                string option = Console.ReadLine();
                switch(option.Trim())
                {
                    case "1":
                        AnalyseSequence(g, parseTable);
                        break;
                    case "0":
                        keepAlive = false;
                        break;
                    default:
                        Console.WriteLine("Invalid command.");
                        break;
                }
            }
        }

        private static void AnalyseSequence(Grammar g, Dictionary<string, Dictionary<string, Rule>> parseTable)
        {
            Console.Write("Give sequence: ");
            string sequence = Console.ReadLine();
            List<int> result = ParsingAlgorithm.LL1(parseTable, g, sequence);
            if (result != null)
            {
                Console.WriteLine("Sequence accepted!\n");
                foreach (int index in result)
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
    }
}
