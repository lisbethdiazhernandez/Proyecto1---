using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        public Form1(string filename)
        {
            FileName = filename;
            InitializeComponent();
            validations.Read(FileName);
            if(validations.error.Last() == "fin")
            {
                Mostrar();
            }
        }
        public void Mostrar()
        {
            dataGridFirst.DataSource = validations.MostrarFirst();
            dataGridFollow.DataSource = validations.MostrarFollows();
            dataGridSets.DataSource = validations.MostrarSets();
        }
    }
}
