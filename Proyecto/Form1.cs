using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto
{
    public partial class ReadText : Form
    {
        public string FileName = string.Empty;
        public ReadText()
        {
            InitializeComponent();
        }
      
        private void SelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog() { Filter = "Text File|*.txt", Multiselect = false})
            {
                if(fileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader str = new StreamReader(fileDialog.FileName))
                    {
                        FileName = fileDialog.FileName;
                        TextUploaded.Text = str.ReadToEnd();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 afd = new Form1(FileName);
            afd.ShowDialog();
        }
    }
}
