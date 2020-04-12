using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto
{
    class Class1
    {
        public void Verificar(string texto)
        {


            int Estado = 0; bool Salir = false; bool Error = false;
            string cadena = string.Empty;
            for (int i = 0; i < texto.Length; i++)
            {
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
                }
            }
        }
    }
}
