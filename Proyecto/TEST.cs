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
        public void Verificar(string cadena)
        {
            for (int i = 0; i < cadena.Length; i++)
            {
                int Estado = 1; bool Salir = false; bool Error = false;
                switch (Estado)
                {
                    case 1:
                        if ((cadena[i] >= 48 && cadena[i] <= 57))
                        { Estado = 2; }
                        else if (cadena[i] == 61)
                        { Estado = 3; }
                        else if (cadena[i] == 58)
                        { Estado = 4; }
                        else if ((cadena[i] >= 65 && cadena[i] <= 90) || (cadena[i] >= 97 && cadena[i] <= 122))
                        { Estado = 5; }
                        else
                        {
                            Error = true;
                            Salir = true;
                        }
                        break;
                    case 2:
                        if ((cadena[i] >= 48 && cadena[i] <= 57)) { Estado = 2; }
                        else
                        {
                            Error = true;
                            Salir = true;
                        }
                        break;
                    case 3:
                        Error = true;
                        Salir = true;
                        break;
                    case 4:
                        if (cadena[i] == 61) { Estado = 3; }
                        else
                        {
                            Error = true;
                            Salir = true;
                        }
                        break;
                }
            }
        }
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

