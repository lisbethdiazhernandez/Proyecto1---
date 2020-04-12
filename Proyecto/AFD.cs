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
        Dictionary<string, List<string>> tokendic = new Dictionary<string, List<string>>();
        Dictionary<int, List<string>> P = new Dictionary<int, List<string>>();
        
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

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog() { Filter = "Text File|*.txt", Multiselect = false })
            {
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader str = new StreamReader(fileDialog.FileName))
                    {
                        FileName = fileDialog.FileName;
                    }
                }
            }
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

            //string path = System.IO.Directory.GetCurrentDirectory();
            string path = "\\Desktop\\CLASE.cs";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    string text = "using System; @  using System.Collections.Generic; @ using System.Linq; @ using System.Text; @ using System.Threading.Tasks; @ namespace Proyecto @ { @  class Prueba @ {";
                    text += "public void Verificar(string texto) @ { ";
                    text += " for (int i = 0; i < texto.Length; i++) @ { ";
                    text = text.Replace("@", Environment.NewLine);
                    sw.Write(text);
                }
                int contadorestados = 1;
                using (StreamWriter swa = File.AppendText(path))
                {
                    swa.WriteLine("int Estado = 0; bool Salir = false; bool Error = false; ");
                    swa.WriteLine("switch (Estado) {");
                    string thecase = string.Empty;
                    for (int i = 0; i < transiciones.Count; i++)
                    {
                        thecase += "case " + contadorestados + " : @";
                        foreach (var item in transiciones)
                        {
                            if (!thecase.Contains("if"))
                            {
                                var ifs = validationsets.FirstOrDefault(x => x.Key == item.Key).Value;
                                thecase += ifs + "{ ";
                                List<string> temp = item.Value[contadorestados - 1];
                                int posicion = P.LastOrDefault(x => x.Value.All(temp.Contains) && x.Value.Count == temp.Count).Key;
                                thecase += "Estado = " + posicion + "; }";
                            }
                            else
                            {
                                var ifs = validationsets.FirstOrDefault(x => x.Key == item.Key).Value;
                                thecase += "else " + ifs + "{";
                                List<string> temp = item.Value[contadorestados - 1];
                                int posicion = P.LastOrDefault(x => x.Value.All(temp.Contains) && x.Value.Count == temp.Count).Key;
                                thecase += "Estado = " + posicion + ";  }";
                            }
                        }
                        thecase += "else { @ Error = true; @ Salir = true; @ } @ break;";
                        thecase = thecase.Replace("@", Environment.NewLine);
                        swa.Write(thecase);
                        thecase = ""; contadorestados++;
                    }
                    thecase = "} @ } @ } @ } @ }";
                    thecase = thecase.Replace("@", Environment.NewLine);
                    swa.Write(thecase);
                }
            }
           
        }
    }
}
