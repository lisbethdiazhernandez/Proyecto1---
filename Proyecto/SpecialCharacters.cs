using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto
{
    class SpecialCharacters
    {
        public List<string> SpecialOnSets = new List<string>() { "=", "'", "..", "+", "chr" };
        public List<string> SpecialOnTokens = new List<string>() { "*", "|", "+", "?", "(", ")" };
        public List<string> SpecialOnActions = new List<string>() { "{", "reservadas", "()", "'" };
    }
}
