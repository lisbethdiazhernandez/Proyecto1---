using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto
{
    public class Validations
    {

        public string FilePath = "";
        public string cadena = string.Empty;
        string newstring = string.Empty;
        string Content = string.Empty; string temporal = string.Empty;
        public List<string> error = new List<string>();
        public List<string> sets = new List<string>();
        string siguiente = "="; bool set = false; bool tok = false; bool act = false;
        public List<string> tokens = new List<string>();
        public List<string> actions = new List<string>();
        int linenum = 0; static Graph graph;
        public Dictionary<string, string> SetsDic = new Dictionary<string, string>();
        public Dictionary<string, List<string>> TokenDic = new Dictionary<string, List<string>>();
        public Dictionary<string, string> ActionsDic = new Dictionary<string, string>();
        static Tree arbol = new Tree();
        static Dictionary<int, List<int>> Follows = new Dictionary<int, List<int>>();
        string validarcaracteres = string.Empty;
        static Dictionary<string, List<List<string>>> transitions = new Dictionary<string, List<List<string>>>();
        static Dictionary<int, List<int>> P = new Dictionary<int, List<int>>();
        static Dictionary<int, List<int>> Follow = new Dictionary<int, List<int>>();

        public void ProcesarArchivo(string filepath)
        {
           Read(filepath);
        }

        public void Read(string filepath)
        {
            string message = string.Empty;
            List<string> Text_archivo = new List<string>();
            string line = string.Empty; bool errorfound = false;

            using (var stream = new FileStream(filepath, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream && !errorfound)
                    {
                        line = reader.ReadLine();
                        linenum++;
                        if (error.Count == 0)
                        {
                            if (!set)
                            {
                                line = ValidacionesGeneral(line);
                            }
                            if (set && line != "" && !tok)
                            {
                                string replacement = Regex.Replace(line, @"\t|\n|\r| ", "");
                                ValidacionesSets(replacement);
                            }
                            else if (tok)
                            {
                                string replacement = Regex.Replace(line, @"\t|\n|\r| ", "");
                                ValidacionesTokens(replacement);
                            }
                        }
                        else
                        {
                            errorfound = true;
                            if (error.ElementAt(0) != "fin")
                            {
                                DialogResult dialog = MessageBox.Show(error.ElementAt(0));
                                if (dialog == DialogResult.OK)
                                {Application.Exit(); }
                            }
                        }
                    }
                }
            }
        }
        
        public DataTable MostrarFirst()
        {
            DataTable dtData = new DataTable("First&Last");
            dtData.Columns.Add("Caracter", typeof(string));
            dtData.Columns.Add("First", typeof(string));
            dtData.Columns.Add("Last", typeof(string));
            dtData.Columns.Add("nullable", typeof(string));

            foreach (var item in graph.vertices)
            {
                DataRow newrow = dtData.NewRow();
                newrow["Caracter"] = item.Value.value;
                newrow["First"] = String.Join(",", item.Value.First);
                newrow["Last"] = String.Join(",", item.Value.Last);
                newrow["nullable"] = item.Value.nullable == true? "yes" : "no";
                dtData.Rows.Add(newrow);
            }
            return dtData;
        }
        public DataTable MostrarTransiciones()
        {
            DataTable dtData = new DataTable("Transitions");
            dtData.Columns.Add("Numero", typeof(string));
            dtData.Columns.Add("Estados", typeof(string));

            foreach (var item in P)
            {
                DataRow newrow = dtData.NewRow();
                newrow["Numero"] = item.Key;
                newrow["Estados"] = String.Join(",", item.Value);
                dtData.Rows.Add(newrow);
            }
            return dtData;
        }
        public DataTable MostrarFollows()
        {
            DataTable dtData = new DataTable("Follows");
            dtData.Columns.Add("Caracter", typeof(string));
            dtData.Columns.Add("Follow", typeof(string));

            foreach (var item in Follows)
            {
                DataRow newrow = dtData.NewRow();
                newrow["Caracter"] = item.Key.ToString();
                newrow["Follow"] = String.Join(",", item.Value);
                dtData.Rows.Add(newrow);
            }
            return dtData;
        }
        public DataTable MostrarSets()
        {
            DataTable dtData = new DataTable("Sets");
            dtData.Columns.Add("Set", typeof(string));
            dtData.Columns.Add("Contenido", typeof(string));

            foreach (var item in SetsDic)
            {
                DataRow newrow = dtData.NewRow();
                newrow["Set"] = item.Key.ToString();
                newrow["Contenido"] = item.Value;
                dtData.Rows.Add(newrow);
            }
            return dtData;
        }
        public string ValidacionesGeneral(string linea)
        {
            string replacement = Regex.Replace(linea, @"\t|\n|\r| ", "");
            if (replacement.Substring(0, 4).ToLower() == "sets")
            {
                set = true;
                return "";
            }
            else
            {
               error.Add("Error en linea: " + linenum.ToString() + ". Debe de iniciar con indicar los SETS colocando y definiendolos");
               
            }
            return replacement;
        }

        public string ValidarCaracteres(List<string> specialCharacters, string cadena, string esperado)
        {
            int inicio = 0; int length = 1; int cadenalength = 1;
            cadenalength = cadena.Length;
            for (int i = 0; i < cadenalength; i++)
            {
                if (cadena.Length >= 6)
                {
                    if (cadena.Substring(0, i) == "TOKENS")
                    {
                        return "TOKENS";
                    }
                    else if (cadena.Substring(0, 6) == "TOKENS")
                    {
                        return "TOKENS";
                    }
                    else if (cadena.Substring(0, 3) == "CHR")
                    {
                        string palabra = "CHR";
                        return palabra;
                    }
                }
                if (esperado == "." && cadena.Length >= (length + 1))
                {
                    if (cadena.Substring(i, length + 1) == "..")
                    {
                        string palabra = "..";
                        return palabra;
                    }
                    if (cadena.Substring(0, 3) == "CHR")
                    {
                        string palabra = "CHR";
                        return palabra;
                    }
                    else if (cadena.Substring(i, length) == "+")
                    {
                        string palabra = "+";
                        return palabra;
                    }
                    else if (cadena.Substring(i, length) == "." && cadena.Substring((i + 1), length) != ".")
                    {
                        error.Add("Error en linea: " + linenum.ToString() + " se esperaban 2 puntos consecutivos. Se obtuvo " + cadena.Substring(i, length + 1));
                        return "error";
                    }
                    else if (!specialCharacters.Contains(cadena.Substring(i, length)))
                    {
                        if (cadena.Contains("="))
                        { return "="; }
                        else { temporal = cadena; return ""; }
                    }

                }
                if (cadena.Substring(0, 1) == "'")
                {
                    abre = true;
                    return cadena.Substring(0, 1);
                }

                else if (cadena.Substring(i, length) == esperado)
                {
                    if (!abre && i > 0 && esperado == "'")
                    {
                        error.Add("Error en linea: " + linenum.ToString() + ". la comilla ' se esperaba antes de  " + cadena.Substring(0, i));
                        return "error";
                    }
                    else
                    {
                        string palabra = cadena.Substring(0, i);
                        return palabra;
                    }

                }
                if (specialCharacters.Contains(cadena.Substring(i, length)) && cadena.Substring(i, length) != esperado)
                {
                    error.Add("Error en linea: " + linenum.ToString() + ". Se esperaba el caracter " + esperado);
                    return "error";
                }
            }
            return "";
        }
        bool abre = false;

        public List<string> ValidacionesSets(string cadena)
        {
            SpecialCharacters specialCharacters = new SpecialCharacters();
            string nuevacadena = string.Empty;
            nuevacadena += cadena;
            string respuesta = string.Empty;
            string nombre = string.Empty; string content = string.Empty;
            bool terminar = false;
            if (cadena == "=")
            {
                sets.Add(temporal);
                temporal = ""; terminar = true; Content = ""; siguiente = "'";
            }
            while (!terminar)
            {
                switch (siguiente)
                {
                    case "=":
                        nombre = ValidarCaracteres(specialCharacters.SpecialOnSets, nuevacadena, "=");
                        if (nombre == "TOKENS")
                        {
                            terminar = true; tok = true;
                            siguiente = "=";
                            nuevacadena = nuevacadena.Length > 0 ? nuevacadena.Substring(nombre.Length, nuevacadena.Length - nombre.Length) : "";
                        }
                        else
                        {
                            if(sets.Count > 0)
                            {
                                SetsDic.Add(sets.Last(), Content);
                                Content = "";
                            }

                            sets.Add(nombre);
                            nuevacadena = nuevacadena.Length > 0 ? nuevacadena.Substring(nombre.Length + 1, nuevacadena.Length - nombre.Length - 1) : "";
                            siguiente = "'";
                        }
                        break;
                    case "'":
                        respuesta = ValidarCaracteres(specialCharacters.SpecialOnSets, nuevacadena, "'");
                        if (respuesta == "TOKENS")
                        {
                            siguiente = "="; terminar = true; tok = true;
                            if (abre) { nuevacadena = nuevacadena.Length > 0 ? nuevacadena.Substring(nombre.Length, nuevacadena.Length - nombre.Length) : ""; }
                            else
                            {
                                error.Add("Error en linea: " + linenum);
                            }
                        }
                        else if (respuesta == "CHR")
                        {
                            nuevacadena = nuevacadena.Length > 0 ? nuevacadena.Substring(respuesta.Length, nuevacadena.Length - respuesta.Length) : "";
                            respuesta = ValidarCaracteres(specialCharacters.SpecialOnSets, nuevacadena, "(");
                            nuevacadena = nuevacadena.Length > 0 ? nuevacadena.Substring(1, nuevacadena.Length - 1) : "";
                            respuesta = ValidarCaracteres(specialCharacters.SpecialOnSets, nuevacadena, ")");
                            nuevacadena = nuevacadena.Length > 0 ? nuevacadena.Substring(respuesta.Length+1, nuevacadena.Length - (respuesta.Length+1)) : "";
                           Content += "CHR(" + respuesta + ")";
                            respuesta = "CHR";
                            siguiente = ".";
                        }
                       else  if (respuesta != "error")
                        {
                            if (abre)
                            {
                                Content += "'";
                                respuesta = nuevacadena.Length > 0 ? ValidarCaracteres(specialCharacters.SpecialOnSets, nuevacadena.Substring(1, nuevacadena.Length - 1), "'") : "";
                                if (respuesta.Length == 0)
                                {
                                    error.Add("Error en linea: " + linenum + " no se encontro la ' la cual fue abierta pero no cerrada, ambas comillas deben de encontrarse en la misma linea"); terminar = true;
                                }
                                else
                                {
                                    Content += respuesta + "'";
                                }
                            }
                            else
                            {
                                error.Add("Error en linea: " + linenum + "se encontro la ' la cual nunca fue abierta");
                                terminar = true;
                            }
                        }
                        else
                        {
                            terminar = true;
                        }
                        nuevacadena = nuevacadena.Length > 0 && respuesta != "error" && respuesta!= "CHR"? nuevacadena.Substring(respuesta.Length + 2, nuevacadena.Length - respuesta.Length - 2) : (nuevacadena = respuesta == "CHR" ?  nuevacadena: "");
                        if (nuevacadena.Length == 0)
                        { terminar = true; }
                        siguiente = ".";
                        break;
                    case ".":
                        respuesta = ValidarCaracteres(specialCharacters.SpecialOnSets, nuevacadena, ".");
                        if (respuesta == "TOKENS")
                        {
                            siguiente = "="; SetsDic.Add(sets.Last(), Content);
                            Content = "";
                            terminar = true; tok = true;
                            nuevacadena = nuevacadena.Length > 0 ? nuevacadena.Substring(nombre.Length, nuevacadena.Length - nombre.Length) : "";
                        }
                        else if (respuesta == "error")
                        {
                            terminar = true;
                        }
                        else
                        {
                            if (respuesta == "=")
                            { siguiente = "="; }
                            else if (respuesta == "")
                            { terminar = true; }
                            else if (respuesta != "error" && respuesta != "=")
                            {
                                Content += respuesta;
                                nuevacadena = nuevacadena.Length > 0 ? nuevacadena.Substring(respuesta.Length, nuevacadena.Length - respuesta.Length) : "";
                                siguiente = "'";
                            }
                        }
                        if (nuevacadena.Length == 0)
                        { terminar = true; }
                        break;
                }
            }
            return sets;
        }

        public string ValidarCaracteresTokens(List<string> specialCharacters, string cadena, string esperado)
        {
            int length = 1; int cadenalength = 1; string contenido = string.Empty;
            cadenalength = cadena.Length;

            for (int i = 0; i <= cadenalength; i++)
            {
                
                if (cadena.Substring(0, i).ToUpper() == "TOKEN")
                {
                    return "TOKEN";
                }
                if (cadena.Substring(0, i).ToUpper() == "ACTIONS")
                {
                    return "ACTIONS";
                }
                if (sets.Contains(cadena.Substring(0, i)))
                {
                    tokens.Add(cadena.Substring(0, i));
                    return cadena.Substring(0, i);
                }
                if (specialCharacters.Contains(cadena.Substring(0, 1)))
                {
                    tokens.Add(cadena.Substring(0, 1));
                    return cadena.Substring(0, 1);
                }
                else if (cadena.Length < i + length)
                {
                    error.Add("No se encontro el set " + cadena);
                   
                    return "error";
                }
                else if (cadena.Substring(i, length) == "=" && esperado != "'")
                {
                    string palabra = cadena.Substring(0, i);
                    return palabra;
                }
                else if (cadena.Length >= i + length)
                {
                    if (cadena.Substring(i, length) == esperado)
                    {
                        if(cadena.Length >= 2)
                        {
                            
                                if (cadena.Length <= 2 && cadena.Substring(0, 2) == "''")
                                {
                                    tokens.Add(cadena.Substring(0, 1));
                                    return cadena.Substring(0, 1);
                                }
                                else if (cadena.Substring(0, 2) == "''" && cadena.Substring(0, 3) != "'''")
                                {
                                    tokens.Add(cadena.Substring(0, 1));
                                    return cadena.Substring(0, 1);
                                }
                        }
                        if (cadena.Substring(0, 1) == "'")
                        {
                            return "'";
                        }
                        else
                        {
                            
                            tokens.Add(cadena.Substring(0, i));
                            return cadena.Substring(0, i);
                        }
                    }
                }
            }
            return "";
        }
        string numero = string.Empty;
        public List<string> ValidacionesTokens(string cadena)
        {
            SpecialCharacters specialCharacters = new SpecialCharacters();

            newstring += cadena;
            string respuesta = string.Empty;
            string nombre = string.Empty;
            string Content = string.Empty; bool terminar = false;
            if (!newstring.Contains("TOKEN") && !newstring.Contains(siguiente))
            {
                if (newstring.Contains("ACTIONS"))
                { siguiente = "'"; }
                else { terminar = true; }
            }
            else if (newstring.Contains("TOKEN") && !newstring.Contains(siguiente))
            {
                if (!newstring.Contains("="))
                {
                    terminar = true;
                }
                else if (newstring.Contains("ACTIONS"))
                { siguiente = "'"; }
            }
            while (!terminar && newstring.Length > 0)
            {
                switch (siguiente)
                {
                    case "=":
                        if (numero != "")
                        {
                            if (!TokenDic.Keys.Contains(numero))
                            {
                                TokenDic.Add(numero, tokens.GetRange(0, tokens.Count));
                            }
                            else
                            {
                                error.Add("El numero de token " + numero + " ya existe, por lo cual no puede ser creado"); terminar = true;
                            }
                            tokens.Clear();
                        }
                        nombre = ValidarCaracteresTokens(specialCharacters.SpecialOnTokens, newstring, "=");
                        if (nombre.ToUpper() == "TOKEN")
                        {
                            siguiente = "=";
                            newstring = newstring.Length > 0 ? newstring.Substring(nombre.Length, newstring.Length - nombre.Length) : "";
                            numero = newstring.Length > 0 ? ValidarCaracteresTokens(specialCharacters.SpecialOnTokens, newstring, "=") : "";
                            newstring = newstring.Length > 0 ? newstring.Substring(numero.Length + 1, newstring.Length - numero.Length - 1) : "";
                        }
                        siguiente = "'";
                        break;
                    case "'":
                        respuesta = ValidarCaracteresTokens(specialCharacters.SpecialOnTokens, newstring, "'");
                        if (newstring.Substring(0, 1) == "'")
                        {
                            newstring = newstring.Length > 0 ? newstring.Substring(1, newstring.Length - 1) : "";
                            respuesta = ValidarCaracteresTokens(specialCharacters.SpecialOnTokens, newstring, "'");
                            newstring = newstring.Length > 0 ? newstring.Substring(respuesta.Length + 1, newstring.Length - (respuesta.Length + 1)) : "";
                            siguiente = "'";
                        }
                        else if (newstring.Substring(0, 1) == "(")
                        {
                            newstring = newstring.Length > 0 ? newstring.Substring(1, newstring.Length - 1) : "";
                            respuesta = ValidarCaracteresTokens(specialCharacters.SpecialOnTokens, newstring, ")");
                            newstring = newstring.Length > 0 ? newstring.Substring(respuesta.Length , newstring.Length - (respuesta.Length)) : "";
                            siguiente = "'";
                        }
                        else if (sets.Contains(respuesta))
                        {
                            newstring = newstring.Length > 0 ? newstring.Substring(respuesta.Length, newstring.Length - respuesta.Length) : "";
                        }
                        else if (respuesta.ToUpper() == "TOKEN")
                        {
                            if (!TokenDic.Keys.Contains(numero))
                            {
                                TokenDic.Add(numero, tokens.GetRange(0, tokens.Count)); tokens.Clear(); numero = "";
                                siguiente = "=";
                            }
                            else
                            {
                                error.Add("El numero de token " + numero + " ya existe, por lo cual no puede ser creado"); terminar = true;
                          
                            }
                        }
                        else if (respuesta.ToUpper() == "ACTIONS")
                        {
                            if (!TokenDic.Keys.Contains(numero))
                            {
                                TokenDic.Add(numero, tokens.GetRange(0, tokens.Count));
                            }
                            else
                            {
                               error.Add("El numero de token " + numero + " ya existe, por lo cual no puede ser creado"); terminar = true;
                            }
                            tokens.Clear(); numero = "";
                            terminar = true;
                            ValidarActions();
                        }
                        else if(respuesta =="error")
                        {
                            terminar = true;
                        }
                        else
                        {
                            newstring = newstring.Length > 0 ? newstring.Substring(respuesta.Length, newstring.Length - respuesta.Length) : "";
                        }

                        break;
                }
            }

            return sets;
        }

        public List<string> ValidarActions()
        {
            bool concatenacion = true; int count = 0; bool parentesis = false; bool parentesis2 = false;
            List<string> listadeNodostemp = new List<string>();
            List<List<string>> listaexpresionestemp = new List<List<string>>();
            List<string> listadeNodos = new List<string>();
            foreach (var item in TokenDic.Values)
            {
                listadeNodos.Clear();
                listadeNodostemp.Clear();
                foreach (var dato in item)
                {
                    if (dato != "(" && dato != "*" && dato != "?" && dato != "+" && dato != ")" && dato != "|")
                    {
                        if(parentesis == true)
                        {
                            listadeNodos.Add(dato);
                        }
                        else if (concatenacion == true)
                        {
                            listadeNodostemp.Clear();
                            listadeNodostemp.AddRange(listadeNodos);
                            listadeNodos.Clear();
                            if (listadeNodostemp.Count > 0)
                            {
                                listadeNodos.Add("(");
                                listadeNodos = listadeNodos.Concat(listadeNodostemp).ToList();
                                listadeNodos.Add(".");
                                listadeNodos.Add(dato);
                                listadeNodos.Add(")");
                            }
                            else
                            {
                                listadeNodos.Add(dato);
                            }
                            concatenacion = true;
                        }
                        else
                        {
                            listadeNodostemp.Clear();
                            listadeNodostemp.AddRange(listadeNodos);
                            listadeNodos.Clear();
                            listadeNodos.Add("(");
                            listadeNodos = listadeNodos.Concat(listadeNodostemp).ToList();
                            listadeNodos.Add(dato);
                            listadeNodos.Add(")");
                            concatenacion = true;
                        }
                    }
                    else if(dato == "(")
                    {
                        if (listadeNodos.Last() != "|")
                        {
                            listadeNodos.Add(".");
                        }
                            listadeNodos.Add("(");
                        parentesis = true;
                    }
                    else if (dato == ")")
                    {
                        listadeNodos.Add(")");
                        parentesis = false;
                    }
                    else if (dato == "|")
                    {
                        listadeNodos.Add(dato);
                        concatenacion = false;
                    }
                    else
                    {
                        if(dato == "*" || dato == "+" || dato == "?")
                        {
                            listadeNodostemp.Clear();
                            string last = listadeNodos.Last();
                            if (last != ")")
                            {
                                listadeNodos.RemoveAt(listadeNodos.Count - 1);
                                listadeNodostemp.AddRange(listadeNodos);
                                listadeNodos.Clear();
                                listadeNodos = listadeNodos.Concat(listadeNodostemp).ToList();
                                listadeNodos.Add("(");
                                listadeNodos.Add("(");
                                listadeNodos.Add(last);
                                listadeNodos.Add(dato);
                                listadeNodos.Add(")");
                            }
                            else if(concatenacion)
                            {
                                int position = listadeNodos.Count-2;
                                List<string> anteriores = new List<string>();
                                anteriores = listadeNodos.GetRange(0, position);
                                listadeNodostemp = listadeNodos.GetRange(position, 1);
                                listadeNodos.Clear();
                                listadeNodos = listadeNodos.Concat(anteriores).ToList();
                                listadeNodos.Add("(");
                                listadeNodos = listadeNodos.Concat(listadeNodostemp).ToList();
                                listadeNodos.Add(dato);
                                listadeNodos.Add(")");
                                listadeNodos.Add(")");
                            }
                            else
                            {
                                int position = listadeNodos.LastIndexOf("(");
                                List<string> anteriores = new List<string>();
                                anteriores = listadeNodos.GetRange(0, position);
                                listadeNodostemp = listadeNodos.GetRange(position, listadeNodos.Count - position);
                                listadeNodos.Clear();
                                listadeNodos = listadeNodos.Concat(anteriores).ToList();
                                listadeNodos.Add("(");
                                listadeNodos = listadeNodos.Concat(listadeNodostemp).ToList();
                                listadeNodos.Add(dato);
                                listadeNodos.Add(")");
                            }
                        } 
                    }
                }
                count++;
                if (count <= TokenDic.Values.Count)
                {
                    listadeNodostemp.Clear();
                    listadeNodostemp.AddRange(listadeNodos);
                    listadeNodos.Clear();
                    if (listadeNodostemp.ElementAt(listadeNodostemp.Count - 1) != ")")
                    {
                        listadeNodos.Add("(");
                        listadeNodos.AddRange(listadeNodostemp);
                        listadeNodos.Add(")");
                    }
                    else { listadeNodos.AddRange(listadeNodostemp); }
                    List<string> temp = new List<string>();
                    temp.AddRange(listadeNodos);
                    listaexpresionestemp.Add(temp);
                    concatenacion = true;
                }
            }
            listadeNodos.Clear();
            foreach (var expresion in listaexpresionestemp)
            {
                listadeNodostemp.Clear();
                listadeNodostemp.AddRange(listadeNodos);
                listadeNodos.Clear();
                if (listadeNodostemp.Count > 0)
                {
                    listadeNodos.Add("(");
                    listadeNodos = listadeNodos.Concat(listadeNodostemp).ToList();
                    listadeNodos.Add("|");
                    if (expresion.First() !="(")
                    {
                        listadeNodos.Add("(");
                        listadeNodos.AddRange(expresion);
                        listadeNodos.Add(")");
                    }
                    else
                    {
                        listadeNodos.AddRange(expresion);
                    }
                    
                    listadeNodos.Add(")");
                }
                else
                {
                    listadeNodos.AddRange(expresion);
                }
            }

            listadeNodostemp.Clear();
            listadeNodostemp.AddRange(listadeNodos);
            listadeNodos.Clear();
            listadeNodos.Add("(");
            listadeNodos = listadeNodos.Concat(listadeNodostemp).ToList();
            listadeNodos.Add("."); listadeNodos.Add("#"); listadeNodos.Add(")");
            string expresiioon = String.Join(" ", listadeNodos);

            graph = arbol.CreateTree(listadeNodos, 0, 0);
            //Asigno first
            vertice graph1 = arbol.AssignFirst(graph, graph.head);
            //Asigno Last
            vertice graph2 = arbol.AssignLast(graph, graph.head);
            int nt = arbol.NoTerminales;
            Follows = arbol.AssignFollow(graph);
            if (Follows.Count < nt)
            {
                List<int> tem = new List<int>();
                Follows.Add(Follows.Count +1, tem);
            }
            foreach (var vertu in graph.vertices)
            {
                List<List<string>> TempData = new List<List<string>>();
                if (vertu.Value.value != "*" && vertu.Value.value != "+" && vertu.Value.value != "?" && vertu.Value.value != "|" && vertu.Value.value != "." && transitions.Keys.Contains(vertu.Value.value) == false)
                {
                    transitions.Add(vertu.Value.value, TempData);
                }
            }


            P.Add(1, graph.vertices.ToList().FirstOrDefault(x => x.Key == 1).Value.First);
            try
            {
                P = arbol.CreateTransitions(graph, transitions, P, 1);
            }
            catch(Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                string p = string.Empty;
            }
            error.Add("fin");
            return actions;
        }
    }
}
