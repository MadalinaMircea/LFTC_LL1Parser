using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFTC_LL1Parser
{
    public class SymbolTable
    {
        MyHashTable table = new MyHashTable();

        public CodePositionPair Add(Token token, int code)
        {
            return table.Add(token, code);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("Symbol table: {");
            foreach(List<Pair<Token, int>> l in table.table)
            {
                result.Append("\n\t");
                if (l != null)
                {
                    foreach (Pair<Token, int> p in l)
                    {
                        result.Append("(");
                        result.Append(p.x.token);
                        result.Append(", ");
                        result.Append(p.y);
                        result.Append("), ");
                    }
                }
                else
                {
                    result.Append("( )");
                }
                
            }
            return result.ToString();
        }
    }
}
