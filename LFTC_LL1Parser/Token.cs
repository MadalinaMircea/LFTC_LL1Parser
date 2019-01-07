using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFTC_LL1Parser
{
    public class Token
    {
        public string token { get; set; }

        public Token(string t)
        {
            token = t;
        }

        public override bool Equals(object obj)
        {
            if(obj.GetType().IsInstanceOfType(typeof(Token)))
            {
                return (obj as Token).token == token;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int result = 0;

            foreach (char c in token)
                result = result + c;

            return result;
        }
    }
}
