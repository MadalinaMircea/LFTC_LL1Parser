using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFTC_LL1Parser
{
    public class ProgramInternalForm
    {
        public List<CodePositionPair> program = new List<CodePositionPair>();

        public void Add(int code, int posI, int posJ)
        {
            program.Add(new CodePositionPair(code, posI, posJ));
        }

        public void Add(CodePositionPair p)
        {
            program.Add(p);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("Program Internal Form:");
            foreach (CodePositionPair c in program)
            {
                result.Append("\n\t(");
                result.Append(c.Code);
                result.Append(", ");
                result.Append(c.Position.x);
                result.Append(", ");
                result.Append(c.Position.y);
                result.Append(")");
            }
            return result.ToString();
        }
    }
}
