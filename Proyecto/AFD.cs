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
                string intervalonicial= string.Empty; string intervalofinal = string.Empty;
                for (int i =0; i< key.Length; i++)
                {
                    string temporal = key.Substring(0,i);
                    string inicial;
                    if(temporal.Substring(i,2) == "..")
                    {
                        intervalonicial = temporal.Substring(0, i);
                        temporal = temporal.Substring(i+2, temporal.Length);
                    }
                    else if (temporal.Substring(i, 1) == "+")
                    {
                        intervalofinal = temporal.Substring(0, i);
                        temporal = temporal.Substring(i + 1, temporal.Length);
                        validacion += "if ( cadena[i] >= " + System.Convert.ToInt32(intervalonicial) + " &&  cadena[i] <= " + System.Convert.ToInt32(intervalofinal);
                    }
                    else if (intervalonicial== "" && intervalofinal == "" && !temporal.Contains(".."))
                    {
                        validacion += " || cadena[i] == " + temporal; 
                    }

                }
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
