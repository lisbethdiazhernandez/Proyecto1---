namespace Proyecto
{
    partial class ReadText
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.TextUploaded = new System.Windows.Forms.RichTextBox();
            this.SelectFile = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // TextUploaded
            // 
            this.TextUploaded.Location = new System.Drawing.Point(12, 31);
            this.TextUploaded.Name = "TextUploaded";
            this.TextUploaded.Size = new System.Drawing.Size(331, 207);
            this.TextUploaded.TabIndex = 0;
            this.TextUploaded.Text = "";
            // 
            // SelectFile
            // 
            this.SelectFile.Location = new System.Drawing.Point(12, 2);
            this.SelectFile.Name = "SelectFile";
            this.SelectFile.Size = new System.Drawing.Size(75, 23);
            this.SelectFile.TabIndex = 1;
            this.SelectFile.Text = "Select File";
            this.SelectFile.UseVisualStyleBackColor = true;
            this.SelectFile.Click += new System.EventHandler(this.SelectFile_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(94, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Verificar Lenguaje";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ReadText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 270);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.SelectFile);
            this.Controls.Add(this.TextUploaded);
            this.Name = "ReadText";
            this.Text = "File Upload";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RichTextBox TextUploaded;
        private System.Windows.Forms.Button SelectFile;
        private System.Windows.Forms.Button button1;
    }
}

