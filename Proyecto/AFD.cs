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
        Dictionary<int, List<int>> P = new Dictionary<int, List<int>>();
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
            P = validations.P;
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

             string path = System.IO.Directory.GetCurrentDirectory();

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.Write("using System; \\r\\n  using System.Collections.Generic; \\r\\n using System.Linq; \\r\\n  using System.Text; \\r\\n  using System.Threading.Tasks; \\r\\n  namespace Proyecto \\r\\n  { \\r\\n  class Prueba \\r\\n {");
                }
                int contadorestados = 1;
                using (StreamWriter swa = File.AppendText(path))
                {
                    swa.Write("int Estado =0;");
                    swa.Write("switch (Estado) {;");
                    string thecase = string.Empty;
                   foreach(var item in transiciones)
                    {
                        if (!thecase.Contains("if"))
                        {
                            thecase += "case " + contadorestados + " : \\r\\n";
                            var ifs = validationsets.FirstOrDefault(x => x.Key == item.Key).Value;
                            thecase += "{ " + ifs;
                            int posicion = P.FirstOrDefault(x => x.Value.Select(y =>y.ToString()).ToList() == item.Value[contadorestados - 1]).Key;
                            thecase += "Estado = " + posicion + "}";
                        }
                        else
                        {
                            thecase += "case " + contadorestados + " : \\r\\n";
                            var ifs = validationsets.FirstOrDefault(x => x.Key == item.Key).Value;
                            thecase += "{ else " + ifs;
                            int posicion = P.FirstOrDefault(x => x.Value.ConvertAll<string>(y => y.ToString()) == item.Value[contadorestados - 1]).Key;
                            thecase += "Estado = " + posicion + "}";
                        }
                    }
                }
            }
           
        }
    }
}
