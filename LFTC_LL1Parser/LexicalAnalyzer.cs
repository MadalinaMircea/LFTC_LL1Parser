using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LFTC_LL1Parser
{
    class LexicalAnalyzer
    {
        //IDictionary<string, int> reservedWords = new Dictionary<string, int>();
        //IDictionary<string, int> operators = new Dictionary<string, int>();
        //IDictionary<string, int> separators = new Dictionary<string, int>();

        static List<string> reservedWords = new List<string>();
        static List<string> operators = new List<string>();
        static List<string> separators = new List<string>();

        public static IDictionary<string, int> codeTable = new Dictionary<string, int>();
        public static IDictionary<int, string> reverseCodeTable = new Dictionary<int, string>();

        static int line = 0;

        public LexicalAnalyzer()
        {
            codeTable.Add("declare", 2); codeTable.Add("execute", 3); codeTable.Add("int", 4);
            codeTable.Add("char", 5); codeTable.Add("string", 6); codeTable.Add("array", 7);
            codeTable.Add("arrayOf", 8); codeTable.Add("if", 9); codeTable.Add("else", 10);
            codeTable.Add("for", 11); codeTable.Add("print", 12);

            codeTable.Add("+", 13); codeTable.Add("-", 14); codeTable.Add("*", 15); codeTable.Add("/", 16);
            codeTable.Add("=", 17); codeTable.Add("==", 18); codeTable.Add("<", 19); codeTable.Add(">", 20);
            codeTable.Add("!=", 21);

            codeTable.Add(";", 22); codeTable.Add(".", 23); codeTable.Add(",", 24); codeTable.Add("(", 25);
            codeTable.Add(")", 26); codeTable.Add("{", 27); codeTable.Add("}", 28); codeTable.Add("\"", 29);
            codeTable.Add("\'", 30);

            foreach(KeyValuePair<string, int> pair in codeTable)
            {
                reverseCodeTable.Add(pair.Value, pair.Key);
            }

            //reservedWords.Add("declare", 2); reservedWords.Add("execute", 3); reservedWords.Add("int", 4);
            //reservedWords.Add("char", 5); reservedWords.Add("string", 6); reservedWords.Add("array", 7);
            //reservedWords.Add("arrayOf", 8); reservedWords.Add("if", 9); reservedWords.Add("else", 10);
            //reservedWords.Add("for", 11); reservedWords.Add("print", 12);

            //operators.Add("+", 13); operators.Add("-", 14); operators.Add("*", 15); operators.Add("/", 16);
            //operators.Add("=", 17); operators.Add("==", 18); operators.Add("<", 19); operators.Add(">", 20);
            //operators.Add("!=", 21);

            //separators.Add(";", 22); separators.Add(".", 23); separators.Add(",", 24); separators.Add("(", 25);
            //separators.Add(")", 26); separators.Add("{", 27); separators.Add("}", 28); separators.Add("\"", 29);
            //separators.Add("\'", 30);

            reservedWords.Add("declare"); reservedWords.Add("execute"); reservedWords.Add("int");
            reservedWords.Add("char"); reservedWords.Add("string"); reservedWords.Add("array");
            reservedWords.Add("arrayOf"); reservedWords.Add("if"); reservedWords.Add("else");
            reservedWords.Add("for"); reservedWords.Add("print");

            operators.Add("+"); operators.Add("-"); operators.Add("*"); operators.Add("/");
            operators.Add("="); operators.Add("=="); operators.Add("<"); operators.Add(">");
            operators.Add("!=");

            separators.Add(";"); separators.Add("."); separators.Add(","); separators.Add("(");
            separators.Add(")"); separators.Add("{"); separators.Add("}"); separators.Add("\"");
            separators.Add("\'");
        }

        public static Boolean isIdentifier(string str)
        {
            Regex rg = new Regex(@"^[a-zA-Z][a-zA-Z0-9]*$");
            return rg.IsMatch(str);
        }

        public static Boolean isPossibleOperator(string str)
        {
            Regex rg = new Regex(@"^(\+|-|\*|\/|=|<|>|!)*$");
            return rg.IsMatch(str);
        }

        public static Boolean isConstant(string str)
        {
            Regex rg = new Regex(@"^([0-9]+|\'[a-zA-Z0-9 ]\'|\" + '"' + "[a-zA-Z0-9]+\\" + '"' + ")$");
            return rg.IsMatch(str);
        }

        public static Boolean isOperator(string str)
        {
            return operators.Contains(str);
        }

        public static Boolean isSeparator(string str)
        {
            return separators.Contains(str);
        }

        public static Boolean isReservedWord(string str)
        {
            return reservedWords.Contains(str);
        }

        public static Boolean isAlphaNumeric(string strToCheck)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9]*$");
            return rg.IsMatch(strToCheck);
        }

        public static void CheckWord(string word, int line, SymbolTable sti, SymbolTable stc, ProgramInternalForm pif)
        {
            if(word != "")
            {
                if(isReservedWord(word))
                {
                    //int code;
                    //reservedWords.TryGetValue(word, out code);

                    pif.Add(codeTable[word], -1, -1);
                }
                else if (isSeparator(word))
                {
                    pif.Add(codeTable[word], -1, -1);
                }
                else if (isOperator(word))
                {
                    pif.Add(codeTable[word], -1, -1);
                }
                else if(isIdentifier(word))
                {
                    CodePositionPair p = sti.Add(new Token(word), 0);
                    pif.Add(p);
                }
                else if(isConstant(word))
                {
                    CodePositionPair p = stc.Add(new Token(word), 1);
                    pif.Add(p);
                }
                else
                {
                    throw new Exception("Error at line " + line + ": unknown word \"" + word + "\"");
                }
            }
        }

        public static void Analyze(string sourceCode, SymbolTable sti, SymbolTable stc, ProgramInternalForm pif)
        {
            bool isQuoteOpen = false;
            string aux = "";
            bool isOp = false;
            string code = "";

            foreach (string l in sourceCode.Split('\n'))
            {
                line++;

                if(isQuoteOpen)
                {
                    throw new Exception("Error at line " + line + ": new line not allowed here.");
                }

                CheckWord(aux, line, sti, stc, pif);
                aux = "";
                isOp = false;

                code = l.Trim();

                for (int i = 0; i < code.Length; i++)
                {
                    if (code[i] == ' ')
                    {
                        if (isQuoteOpen)
                        {
                            throw new Exception("Error at line " + line + ": space not allowed here.");
                        }

                        CheckWord(aux, line, sti, stc, pif);
                        aux = "";
                        isOp = false;
                    }
                    else if (isAlphaNumeric(code[i].ToString()))
                    {
                        if (!isOp)
                        {
                            aux = aux + code[i];
                        }
                        else
                        {
                            CheckWord(aux, line, sti, stc, pif);
                            aux = "" + code[i];
                            isOp = false;
                        }
                    }
                    else if (isPossibleOperator(code[i].ToString()))
                    {
                        if (isQuoteOpen)
                        {
                            throw new Exception("Error at line " + line + ": operator not allowed here.");
                        }

                        if (isOp)
                        {
                            aux = aux + code[i];
                        }
                        else
                        {
                            CheckWord(aux, line, sti, stc, pif);
                            aux = "" + code[i];
                            isOp = true;
                        }
                    }
                    else if (separators.Contains(code[i].ToString()))
                    {
                        if (code[i] == '\'')
                        {
                            if(isQuoteOpen)
                            {
                                throw new Exception("Error at line " + line + ": ' not allowed here.");
                            }

                            CheckWord(aux, line, sti, stc, pif);
                            aux = "";
                            isOp = false;

                            if (i < code.Length - 2)
                            {
                                if (code[i + 1] != '\'' && code[i + 2] == '\'')
                                {
                                    CheckWord("'" + code[i + 1] + "'", line, sti, stc, pif);

                                    i = i + 2;
                                }
                                else
                                {
                                    throw new Exception("Error at line " + line + ": invalid constant.");
                                }
                            }
                            else
                            {
                                throw new Exception("Error at line " + line + ": ' not allowed here.");
                            }
                        }
                        else if (code[i] == '\"')
                        {
                            if (isQuoteOpen)
                            {
                                CheckWord("\"" + aux + "\"", line, sti, stc, pif);
                                aux = "";
                                isQuoteOpen = false;
                            }
                            else
                            {
                                isQuoteOpen = true;
                                CheckWord(aux, line, sti, stc, pif);
                            }
                        }
                        else
                        {
                            if (isQuoteOpen)
                            {
                                throw new Exception("Error at line " + line + ": separator not allowed here.");
                            }

                            CheckWord(aux, line, sti, stc, pif);
                            CheckWord(code[i].ToString(), line, sti, stc, pif);
                            aux = "";
                        }
                    }
                }
            }
        }
    }
}
