using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFTC_LL1Parser
{
    public class MyHashTable
    {
        int n = 43;
        public List<Pair<Token, int>>[] table = new List<Pair<Token, int>>[43];

        public MyHashTable()
        {
        }

        public CodePositionPair Add(Token token, int code)
        {
            int position = token.GetHashCode() % n;

            if(table[position] == null)
            {
                table[position] = new List<Pair<Token, int>>();
            }

            int pos = -1;
            for(int i = 0; i < table[position].Count; i++)
            {
                if(table[position][i].x.token == token.token && table[position][i].y == code)
                {
                    pos = i;
                    break;
                }
            }

            if (pos == -1)
            {
                table[position].Add(new Pair<Token, int> { x = token, y = code });
                return new CodePositionPair(code, position, table[position].Count - 1);
            }

            return new CodePositionPair(code, position, pos);
        }

        //private void FindFirstFree(ProgramInternalForm pif)
        //{
        //    int i = 0;
        //    for (i = firstFree; i < table.Count(); i++)
        //        if (table[i].Equals(default(T)))
        //            firstFree = i;

        //    if(i == table.Count())
        //    {
        //        Resize(pif);
        //    }
        //}

        //public int Add(T t, ProgramInternalForm pif)
        //{
        //    int i = t.GetHashCode();
        //    if (table[i].Equals(default(T)))
        //    {
        //        table[i] = t;
        //        return i;
        //    }
        //    else
        //    {
        //        table[firstFree] = t;
        //        int prev = i;
        //        i = positions[i];
        //        while (i != -1)
        //        {
        //            prev = i;
        //            i = positions[i];
        //        }
        //        positions[prev] = firstFree;
        //        FindFirstFree(pif);
        //        return positions[prev];
        //    }
        //}
    }
}
