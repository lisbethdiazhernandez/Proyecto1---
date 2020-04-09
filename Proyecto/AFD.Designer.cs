namespace Proyecto
{
    partial class Form1
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
            this.dataGridFirst = new System.Windows.Forms.DataGridView();
            this.dataGridFollow = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridAFD = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridSets = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFirst)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFollow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAFD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSets)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridFirst
            // 
            this.dataGridFirst.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridFirst.Location = new System.Drawing.Point(42, 56);
            this.dataGridFirst.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.dataGridFirst.Name = "dataGridFirst";
            this.dataGridFirst.RowHeadersWidth = 82;
            this.dataGridFirst.Size = new System.Drawing.Size(900, 381);
            this.dataGridFirst.TabIndex = 0;
            // 
            // dataGridFollow
            // 
            this.dataGridFollow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridFollow.Location = new System.Drawing.Point(976, 56);
            this.dataGridFollow.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.dataGridFollow.Name = "dataGridFollow";
            this.dataGridFollow.RowHeadersWidth = 82;
            this.dataGridFollow.Size = new System.Drawing.Size(640, 381);
            this.dataGridFollow.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1014, 19);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 25);
            this.label3.TabIndex = 5;
            this.label3.Text = "Follow";
            // 
            // dataGridAFD
            // 
            this.dataGridAFD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridAFD.Location = new System.Drawing.Point(42, 473);
            this.dataGridAFD.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.dataGridAFD.Name = "dataGridAFD";
            this.dataGridAFD.RowHeadersWidth = 82;
            this.dataGridAFD.Size = new System.Drawing.Size(1102, 288);
            this.dataGridAFD.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 442);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(288, 25);
            this.label4.TabIndex = 7;
            this.label4.Text = "Automata Finito Determinista";
            // 
            // dataGridSets
            // 
            this.dataGridSets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSets.Location = new System.Drawing.Point(1628, 56);
            this.dataGridSets.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.dataGridSets.Name = "dataGridSets";
            this.dataGridSets.RowHeadersWidth = 82;
            this.dataGridSets.Size = new System.Drawing.Size(524, 381);
            this.dataGridSets.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1480, 19);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 25);
            this.label5.TabIndex = 9;
            this.label5.Text = "Sets";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1235, 544);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(307, 129);
            this.button1.TabIndex = 10;
            this.button1.Text = "Cargar Archivo";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2176, 785);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dataGridSets);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dataGridAFD);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dataGridFollow);
            this.Controls.Add(this.dataGridFirst);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "Form1";
            this.Text = "AFD";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFirst)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFollow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAFD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSets)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridFollow;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridAFD;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridSets;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dataGridFirst;
        private System.Windows.Forms.Button button1;
    }
}