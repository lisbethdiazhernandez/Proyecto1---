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
    public partial class AFD : Form
    {
        public string FileName = string.Empty;
        public AFD(string filename)
        {
            FileName = filename;
            InitializeComponent();
            Validations validations = new Validations();
            validations.Read(FileName);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
