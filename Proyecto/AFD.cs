using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto
{
    public partial class Form1 : Form
    {
        public DataGridView DataGridView1 { get { return this.dataGridFirst; } }

        public string FileName = string.Empty; Validations validations = new Validations();
        Dictionary<string, List<List<string>>> transiciones = new Dictionary<string, List<List<string>>>();
        Dictionary<string, string> setsdic = new Dictionary<string, string>();
        Dictionary<string, string> validationsets = new Dictionary<string, string>();
        Dictionary<string, string> actions = new Dictionary<string, string>();
        Dictionary<string, List<string>> tokendic = new Dictionary<string, List<string>>();
        Dictionary<int, List<string>> P = new Dictionary<int, List<string>>();
        Dictionary<string, List<string>> NoToken = new Dictionary<string, List<string>>();

        public Form1(string filename)
        {
            FileName = filename;
            InitializeComponent();
            validations.Read(FileName);
            if(validations.error.Last() == "fin")
            {
                Mostrar();
            }

            transiciones = validations.transitions;
            setsdic = validations.SetsDic;
            tokendic = validations.TokenDic;
            actions = validations.ActionsDic;
            foreach(var item in validations.P)
            {
                P.Add(item.Key, item.Value.Select(x => x.ToString()).ToList());
            }
        }
        public void Mostrar()
        {
            dataGridFirst.DataSource = validations.MostrarFirst();
            dataGridFollow.DataSource = validations.MostrarFollows();
            dataGridSets.DataSource = validations.MostrarSets();
            dataGridAFD.DataSource = validations.MostrarTransiciones();
        }

       public List<string> NoEsObligatorio( List<string> item)
        {
            if(item.Contains("*"))
            {
                int pos = item.FindIndex(x => x == "*");
                int abre = item.FindIndex(x => x == "(");
                int cierra = item.FindIndex(x => x == ")");
                if (cierra > abre)
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        if (item[i] != "*" && item[i] != "(" && item[i] != ")" && (i < abre || i > cierra))
                        {
                            item[i] = "1," + item[i];
                        }
                        else
                        {
                            item[i] = "0," + item[i];
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        if(i == pos || i==pos-1)
                        {
                            item[i] = "0," + item[i];
                        }
                        else
                        {
                            item[i] = "1," + item[i];
                        }
                    }
                }
            }
            else if (item.Contains("|"))
            {
                int pos = item.FindIndex(x => x == "|");
                int abre = item.FindIndex(x => x == "(");
                int cierra = item.FindIndex(x => x == ")");
                for(int i=0; i< item.Count; i++)
                {
                    if (item[i] != "|" && item[i] != "(" && item[i] != ")" && (i< abre || i> cierra))
                    {
                        item[i] = "1," + item[i];
                    }
                    else
                    {
                        item[i] = "0," + item[i];
                    }
                }

            }
            return item;
        }
        public List<string> EsObligatorio(List<string> item)
        {
            List<string> temp = new List<string>();
            foreach(var it in item)
            {
                temp.Add("1," + it);
            }
            return temp;
        }

        public List<string> EncontrarRangos(string Set)
        {
            List<string> valores = new List<string>();
            string[] valoresdelset = Set.Split(',');
            string Obligatorio = valoresdelset[0];
            var key = setsdic.FirstOrDefault(x => x.Key == valoresdelset[1]).Value;
            string validacion = string.Empty;
            if (key != "" && key != null)
            {
                if (key.Contains("'"))
                {
                    key = key.Replace("'", "");
                }

                string intervalonicial = string.Empty; string intervalofinal = string.Empty;
                string temporal = key;

                for (int i = 0; i < key.Length; i++)
                {

                    string inicial;
                    if (temporal.Length >= i + 2)
                    {
                        if (temporal.Substring(i, 2) == "..")
                        {
                            intervalonicial = temporal.Substring(0, 1);
                            temporal = temporal.Substring(3, temporal.Length - (i + 2));
                        }
                        else if (temporal.Substring(i, 1) == "+")
                        {
                            intervalofinal = temporal.Substring(0, 1);
                            temporal = temporal.Substring(2, temporal.Length - 2);
                            for (int j = Convert.ToInt32(intervalonicial.ToCharArray()[0]); j <= Convert.ToInt32(intervalofinal.ToCharArray()[0]); j++)
                            {
                                intervalofinal = ""; intervalonicial = ""; i = 0;
                            }
                        }
                        else if (intervalonicial == "" && intervalofinal == "" && !temporal.Contains(".."))
                        {
                            valores.Add(temporal);
                        }
                        else if (intervalonicial != "" && intervalofinal == "")
                        {
                            intervalofinal = temporal.Substring(0, 1);
                            temporal = temporal.Substring(2, temporal.Length - 2);
                            for(int j= Convert.ToInt32(intervalonicial.ToCharArray()[0]); j<= Convert.ToInt32(intervalofinal.ToCharArray()[0]);j++)
                            {
                                valores.Add(j.ToString());
                            }
                            intervalofinal = ""; intervalonicial = ""; i = 0;
                        }
                    }
                    else if (!temporal.Contains("..") && !temporal.Contains("+") && intervalonicial != "")
                    {
                        intervalofinal = temporal;
                        for (int j = Convert.ToInt32(intervalonicial.ToCharArray()[0]); j <= Convert.ToInt32(intervalofinal.ToCharArray()[0]); j++)
                        {
                            valores.Add(j.ToString());
                        }
                        i = key.Length;
                        intervalofinal = ""; intervalonicial = "";
                    }
                    else
                    {
                        if (validacion != "" && intervalonicial == "")
                        {
                            valores.Add(System.Convert.ToInt32(temporal.ToCharArray()[0]).ToString());
                        }
                        else if (temporal.Contains("+") && intervalonicial != "")
                        {
                            intervalofinal = temporal.Substring(0, 1);
                            temporal = temporal.Length > 2 ? temporal.Substring(2, temporal.Length - 2) : "";
                            for (int j = Convert.ToInt32(intervalonicial.ToCharArray()[0]); j <= Convert.ToInt32(intervalofinal.ToCharArray()[0]); j++)
                            {
                                valores.Add(j.ToString());
                            }
                            intervalofinal = ""; intervalonicial = ""; i = 0;
                        }
                        else
                        {
                            valores.Add(Convert.ToInt32(temporal.ToCharArray()[0]).ToString());
                        }
                        i = key.Length;
                    }
                }

            }
            else
            {
                valores.Add(Set);
            }
            return valores;
        }
    
        private void button1_Click(object sender, EventArgs e)
        {
            #region Asignar valores de token si son obligatorios o no

            foreach (var tokenNumber in tokendic)
           {
                var key = setsdic.FirstOrDefault(x => x.Key == tokenNumber.Key).Value;
                if(key != "" && key != null)
                {
                    if (key.Contains("'"))
                    {
                        key = key.Replace("'", "");
                    }
                    if (tokenNumber.Value.Contains("*") || tokenNumber.Value.Contains("|"))
                    {
                        NoToken.Add(tokenNumber.Key, NoEsObligatorio(tokenNumber.Value));
                    }
                    else
                    {
                        NoToken.Add(tokenNumber.Key, EsObligatorio(tokenNumber.Value));
                    }
                }
                else
                {
                    if (tokenNumber.Value.Contains("*") || tokenNumber.Value.Contains("|"))
                    {
                        NoToken.Add(tokenNumber.Key, NoEsObligatorio(tokenNumber.Value));
                    }
                    else
                    {
                        NoToken.Add(tokenNumber.Key, EsObligatorio(tokenNumber.Value));
                    }
                }
           }
            #endregion

            #region Verificar Rangos en las transiciones y generar los ifs
            foreach (var item in transiciones)
            {
                var key = setsdic.FirstOrDefault(x => x.Key == item.Key).Value;
                string validacion = string.Empty;
                if (key != "" && key != null)
                {
                    if (key.Contains("'"))
                    {
                        key = key.Replace("'", "");
                    }
                    
                    string intervalonicial = string.Empty; string intervalofinal = string.Empty;
                    string temporal = key;

                    for (int i = 0; i < key.Length; i++)
                    {

                        string inicial;
                        if (temporal.Length >= i + 2)
                        {
                            if (temporal.Substring(i, 2) == "..")
                            {
                                intervalonicial = temporal.Substring(0, 1);
                                temporal = temporal.Substring(3, temporal.Length - (i + 2));
                            }
                            else if (temporal.Substring(i, 1) == "+")
                            {
                                intervalofinal = temporal.Substring(0, 1);
                                temporal = temporal.Substring(2, temporal.Length-2);
                                validacion += "if(( cadena[i] >= " + (System.Convert.ToInt32(intervalonicial.ToCharArray()[0])).ToString() + " &&  cadena[i] <= " + System.Convert.ToInt32(intervalofinal) +")";
                                intervalofinal = ""; intervalonicial = "";i = 0;
                            }
                            else if (intervalonicial == "" && intervalofinal == "" && !temporal.Contains(".."))
                            {
                                validacion += " ||( cadena[i] == " + temporal + ")";
                            }
                            else if (intervalonicial != "" && intervalofinal == "")
                            {
                                intervalofinal = temporal.Substring(0, 1);
                                temporal = temporal.Substring(2, temporal.Length-2);
                                validacion += "if((cadena[i] >= " + (System.Convert.ToInt32(intervalonicial.ToCharArray()[0])).ToString() + " &&  cadena[i] <= " + System.Convert.ToInt32(intervalofinal.ToCharArray()[0]).ToString() + ")";
                                intervalofinal = ""; intervalonicial = ""; i = 0;
                            }
                        }
                        else if (!temporal.Contains("..") && !temporal.Contains("+") && intervalonicial != "")
                        {
                            intervalofinal = temporal;
                            validacion += "if((cadena[i] >= " + (System.Convert.ToInt32(intervalonicial.ToCharArray()[0])).ToString() + " &&  cadena[i] <= " + System.Convert.ToInt32(intervalofinal.ToCharArray()[0]).ToString() + ")";
                            i = key.Length;
                            intervalofinal = ""; intervalonicial = "";
                        }
                        else
                        {
                            if (validacion != "" && intervalonicial == "")
                            {
                                validacion += " && (cadena[i] == " + (System.Convert.ToInt32(temporal.ToCharArray()[0])).ToString() + ")";
                            }
                            else if (temporal.Contains("+") && intervalonicial != "")
                            {
                                intervalofinal = temporal.Substring(0, 1);
                                temporal = temporal.Length > 2? temporal.Substring(2, temporal.Length-2): "";
                                validacion += validacion.Contains("if")? (" || ( cadena[i] >= " + (System.Convert.ToInt32(intervalonicial.ToCharArray()[0])).ToString() + " &&  cadena[i] <= " + System.Convert.ToInt32(intervalofinal.ToCharArray()[0]).ToString()) + ")" : (" if((cadena[i] >= " + (System.Convert.ToInt32(intervalonicial.ToCharArray()[0])).ToString() + " && cadena[i] <= " + System.Convert.ToInt32(intervalofinal.ToCharArray()[0]).ToString() + ")");
                                intervalofinal = ""; intervalonicial = ""; i = 0;
                            }
                            else
                            {
                                validacion += validacion.Contains("if") ? (" || cadena[i] == " + (Convert.ToInt32(temporal.ToCharArray()[0])).ToString()) + ")" : (" if((cadena[i] == " + (Convert.ToInt32(temporal.ToCharArray()[0])).ToString() + ")");
                            }
                            i = key.Length;
                        }
                    }

                }
                else
                {
                    validacion = " if (cadena[i] == " + (Convert.ToInt32(item.Key.ToCharArray()[0])).ToString() + ")";
                }
                validacion += validacion.Contains("if((") ? ")" : "";
                validationsets.Add(item.Key, validacion);
            }
            #endregion

            //string path = System.IO.Directory.GetCurrentDirectory();
            string path = "\\Desktop\\GENERACIONCLASE\\CLASE.cs";
            string text;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            if (!File.Exists(path))
            {
                #region Escritura de variables y librerias necesarias
                using (StreamWriter sw = File.CreateText(path))
                {
                    text = "using System; @ using System.Collections.Generic; @ using System.IO; @ using System.Linq; @ namespace ConsoleApp1 @ {";
                    text += " class Program @ { ";
                    text = text.Replace("@", System.Environment.NewLine);
                    sw.Write(text);
                    text = "";
                }
                using (StreamWriter swa = File.AppendText(path))
                {
                    //****************Initialize NoTokens Dictionary ****************//
                    string numtoken = string.Empty;
                    numtoken += " Dictionary<string, List<string>> numToken = new Dictionary<string, List<string>>() @ { ";
                    foreach (var item in NoToken)
                    {
                        numtoken += numtoken.Contains("}") ? "," : "";
                        numtoken += "{ \" " + item.Key + "\" , new List<string> { \"" + string.Join("\",\"", item.Value) + "\" } }";
                    }
                    numtoken += "}; @ ";
                    numtoken = numtoken.Replace("@", Environment.NewLine);
                    swa.Write(numtoken);
                    

                    //***************initialize SETS dictionary ***************//
                    string writesets = string.Empty;
                    writesets += " Dictionary<string, string> sets = new Dictionary<string, string>() @ { ";
                    foreach (var item in setsdic)
                    {
                        writesets += writesets.Contains("}") ? "," : "";
                        writesets += "{ \"" + item.Key + "\" , \"" + item.Value + "\" }";
                    }
                    writesets += "};";
                    writesets = writesets.Replace("@", Environment.NewLine);
                    swa.Write(writesets);

                    swa.Write("public static int token;");

                    text += " @ static void Main(string[] args) @ {";
                    text += " Console.WriteLine(\"Please write file path!\"); @ ";
                    text += " string path = Console.ReadLine(); @ ";
                    text += " string content = string.Empty; @ ";
                    text += "if (File.Exists(path)) @ { @ ";
                    text += " using (StreamReader sr = new StreamReader(path)) @ { @  content = sr.ReadToEnd(); @  } @ } @ else @ { @ ";
                    text += " Console.WriteLine(\"El archivo no existe, verifique la ruta\"); @ } @ ";
                    text += " string cadena = string.Empty; @  var tokenasignado = new Dictionary<string, string>(); @ ";
                    text+= " string[] cadenas =  content.Split(' '); @ int tokenasigna = 0; ";
                    text = text.Replace("@", Environment.NewLine);
                    swa.Write(text);
                
                int contadorestados = 1;
#endregion

                #region Escritura de Diccionario Actions
               
                    //***************initialize ACTIONS dictionary ***************//
                    string writeactions = string.Empty;
                    writeactions += " var actions = new Dictionary<string, string>() @ { ";
                    foreach(var item in actions)
                    {
                        writeactions += writeactions.Contains("}") ? "," : "";
                        writeactions += "{ \"" + item.Key + "\" , \"" + item.Value + "\" }";
                    }
                    writeactions += "};";
                    writeactions = writeactions.Replace("@", Environment.NewLine);
                    swa.Write(writeactions);





                    #endregion

                    #region Escritura de codigo principal 
                    text = "foreach (var item in cadenas) @ {";
                    text += " cadena = item; int Estado = 1; bool Salir = false; bool Error = false; @ ";
                    text += " int EstadoActual = Estado; @ ";
                    text += "  for (int i = 0; i < cadena.Length; i++) @ { ";
                    text += "if (!Salir) @ { @ ";
                    text += "Program here = new Program(); @ tokenasigna = here.ValidarToken(cadena[i].ToString(), cadena.Length, cadena); @ ";
                    text += " switch (Estado) {";
                    text = text.Replace("@", Environment.NewLine);
                    swa.WriteLine(text);
                    #endregion

                    #region Escritura de cases 
                    string thecase = string.Empty;
                    for (int i = 0; i <= transiciones.Count; i++)
                    {
                        swa.Write("case " + contadorestados + " : ");
                        foreach (var item in transiciones)
                        {
                            if (!thecase.Contains("if"))
                            {
                                List<string> temp = item.Value[contadorestados - 1];
                                int posicion = P.LastOrDefault(x => x.Value.All(temp.Contains) && x.Value.Count == temp.Count).Key;
                                var ifs = validationsets.FirstOrDefault(x => x.Key == item.Key).Value;
                                thecase += posicion != 0 ? ifs + "{ " : "";
                                thecase += posicion != 0 ? "Estado = " + posicion + "; }" : "";
                            }
                            else
                            {
                                var ifs = validationsets.FirstOrDefault(x => x.Key == item.Key).Value;
                                List<string> temp = item.Value[contadorestados - 1];
                                int posicion = P.LastOrDefault(x => x.Value.All(temp.Contains) && x.Value.Count == temp.Count).Key;
                                thecase += posicion !=0? "else " + ifs + "{" : "";
                                thecase += posicion != 0 ? "Estado = " + posicion + "; } " : "";
                            }
                        }
                        thecase += thecase!= ""? "else { @ Error = true; @ Salir = true; @ } @ break;" : " @ Error = true; @ Salir = true;  @ break;";
                        thecase = thecase != ""? thecase.Replace("@", Environment.NewLine): "";
                        swa.Write(thecase);
                        thecase = ""; contadorestados++;
                    }
                    #endregion 

                    thecase = " @ } @ } @  if (i != cadena.Length-1 && Salir) @   { @  if (!tokenasignado.Keys.Contains(cadena)) "  + " {  tokenasignado.Add(cadena,actions.FirstOrDefault(x=> x.Key.ToLower() == \"error\" ).Value); } @ } @ }";
                    thecase += " if (actions.Values.Contains(cadena.ToLower())) @ { ";
                    thecase += "@ string reservada = actions.FirstOrDefault(x => x.Value == cadena.ToLower()).Key; @  if (!tokenasignado.Keys.Contains(cadena)) @ { @ tokenasignado.Add(cadena, reservada); @ } @ }";
                    thecase += "else @ {  if(!tokenasignado.Keys.Contains(cadena)) @  { @ tokenasignado.Add(cadena, tokenasigna.ToString()); @ } @ } @ } ";

                    thecase = thecase.Replace("@", Environment.NewLine);
                    swa.Write(thecase);

                    thecase = " string mostrar = string.Empty; ^ ";
                    thecase += " foreach (var tokenasigned in tokenasignado) ^ { ^  mostrar += tokenasigned.Key + \" = \" + tokenasigned.Value + \" @ \" ;";
                    thecase += " ^ } ^  mostrar = mostrar.Replace(\"@\", System.Environment.NewLine); ^ Console.WriteLine(mostrar); ^ Console.ReadKey(); ^ } ";
                    thecase = thecase.Replace("^", Environment.NewLine);
                    swa.Write(thecase);


                    #region Escritura de funcion ValidarToken 
                    string funcion = string.Empty;
                    funcion = " public int ValidarToken(string cadena, int index, string todalacadena) @  { ";
                    funcion += " foreach (var item in numToken) @ { ";
                    funcion += " @  if(item.Value.Count >= cadena.Length) @ {";
                    funcion += " foreach (var valor in item.Value) @ { @ ";
                    funcion += "bool obligatorio = valor.Split(',')[0] == \"1\" ? true: false; @ ";
                    funcion += "List<string> valores = EncontrarRangos(valor, sets);";
                    funcion += "    if (valores.Contains(Convert.ToInt32(cadena.ToCharArray()[0]).ToString()) || valores.Contains(cadena)) @ {";
                    funcion = funcion.Replace("@", Environment.NewLine);
                    swa.Write(funcion);

                    funcion = " @ token = Convert.ToInt32(item.Key); @ } @ } @";
                    funcion += " List<string> todos = item.Value.FindAll(x => x.Split(',')[0] == \"1\").ToList(); @ ";
                    funcion += " List<bool>cumple = new List<bool>(); int cantidad  = todos.Count; int tokentemp=0; @ ";
                    funcion += " foreach (var valor in todos) @ { @ ";
                    funcion += "List<string> valores = EncontrarRangos(valor, sets); @";
                    funcion += " foreach (var caracter in todalacadena) @ { @ ";
                    funcion += " if (valores.Contains(Convert.ToInt32(caracter).ToString()) || valores.Contains(cadena)) @";
                    funcion += "  { " +
                        " @ int verify = 0; @   foreach (var other in todalacadena) @ {  if (valores.Contains(Convert.ToInt32(other).ToString())) @  { verify++; @ } @ } @ if (verify == todalacadena.Length) { cumple.Add(true); } " +
                        "if(valores.All(value => todalacadena.Contains(value))) @";
                    funcion += "  {  @ cumple.Add(true); @ } else { @ cumple.Add(false); @ } @  } @ }";
                    funcion += " tokentemp = Convert.ToInt32(item.Key); @ }";
                    funcion += " if (cumple.FindAll(x=> x == true).Count >= cantidad) @ ";
                    funcion += "{ @ token = tokentemp; return token ; @ }  @ } @ } return token; @ }";

                    funcion = funcion.Replace("@", Environment.NewLine);
                    swa.Write(funcion);


                    #endregion

                    funcion = "";
                    funcion = "public List<string> EncontrarRangos(string Set, Dictionary<string,string> setsdic) " +
                    "{  List<string> valores = new List<string>();" + "string[] valoresdelset = Set.Split(','); string Obligatorio = valoresdelset[0];"
                    + " var key = setsdic.FirstOrDefault(x => x.Key == valoresdelset[1]).Value;"
                    + " string validacion = string.Empty; if (key != \"\" && key != null)"
                    + " {  if (key.Contains(\"'\")) { key = key.Replace(\"'\", \"\"); }  "
                    + " string intervalonicial = string.Empty; string intervalofinal = string.Empty; "
                    + " string temporal = key;"
                    + " for (int i = 0; i < key.Length; i++) { string inicial; if (temporal.Length >= i + 2)  {"
                    + " if (temporal.Substring(i, 2) == \"..\")   {"
                    + " intervalonicial = temporal.Substring(0, 1);";
                    swa.Write(funcion);

                    funcion = " temporal = temporal.Substring(3, temporal.Length - (i + 2));  }"
                    + " else if (temporal.Substring(i, 1) == \" + \") { intervalofinal = temporal.Substring(0, 1);"
                    + " temporal = temporal.Substring(2, temporal.Length - 2);"
                    + " for (int j = Convert.ToInt32(intervalonicial.ToCharArray()[0]); j <= Convert.ToInt32(intervalofinal.ToCharArray()[0]); j++)"
                    + " { intervalofinal = \"\"; intervalonicial = \"\"; i = 0; }  }"
                    + " else if (intervalonicial == \"\" && intervalofinal == \"\" && !temporal.Contains(\"..\"))"
                    + " { valores.Add(temporal);}  else if (intervalonicial != \"\" && intervalofinal == \"\")"
                    + " {intervalofinal = temporal.Substring(0, 1); temporal = temporal.Substring(2, temporal.Length - 2);"
                    + " for (int j = Convert.ToInt32(intervalonicial.ToCharArray()[0]); j <= Convert.ToInt32(intervalofinal.ToCharArray()[0]); j++)"
                    + " {valores.Add(j.ToString());  } intervalofinal = \"\"; intervalonicial = \"\"; i = 0; } }"
                     + " else if (!temporal.Contains(\"..\") && !temporal.Contains(\" + \") && intervalonicial != \"\")"
                     + " {intervalofinal = temporal;";
                     swa.Write(funcion);
                    funcion = " for (int j = Convert.ToInt32(intervalonicial.ToCharArray()[0]); j <= Convert.ToInt32(intervalofinal.ToCharArray()[0]); j++)"
                     + " {valores.Add(j.ToString()); }  i = key.Length;  intervalofinal = \"\"; intervalonicial = \"\";"
                     + "  }  else   { if (validacion != \"\" && intervalonicial == \"\") {"
                     + " valores.Add(System.Convert.ToInt32(temporal.ToCharArray()[0]).ToString()); "
                     + " } else if (temporal.Contains(\" + \") && intervalonicial != \"\")"
                     + " { intervalofinal = temporal.Substring(0, 1);"
                     + " temporal = temporal.Length > 2 ? temporal.Substring(2, temporal.Length - 2) : \"\";"
                     + " for (int j = Convert.ToInt32(intervalonicial.ToCharArray()[0]); j <= Convert.ToInt32(intervalofinal.ToCharArray()[0]); j++)"
                     + " { valores.Add(j.ToString());    }  intervalofinal = \"\"; intervalonicial = \"\"; i = 0;"
                     + " } else   {valores.Add(Convert.ToInt32(temporal.ToCharArray()[0]).ToString());"
                     + " } i = key.Length; }  }  } else   { valores.Add(Set.Split(',')[1]);} return valores; }  } }";

                    swa.Write(funcion);
                }
            }
            string texto = " x a:= b c = d const a";
            TEST test = new TEST();
            //test.Verificar(texto);
            MessageBox.Show("Archivo generado correctamete");
        }
    }
}
