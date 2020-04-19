using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Proyecto
{
    class TEST
    {
       
        public int ObtenerToken(int Estado)
        {
            int NumToken = 9;
            switch (Estado)
            {
                case 2: NumToken = 1; break;
                case 3: NumToken = 2; break;
                case 5: NumToken = 4; break;
                case 6: NumToken = 3; break;
            }
            return NumToken;
        }
    }
}

