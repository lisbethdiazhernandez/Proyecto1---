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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFirst)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFollow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAFD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSets)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridFirst
            // 
            this.dataGridFirst.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridFirst.Location = new System.Drawing.Point(21, 29);
            this.dataGridFirst.Name = "dataGridFirst";
            this.dataGridFirst.Size = new System.Drawing.Size(450, 198);
            this.dataGridFirst.TabIndex = 0;
            // 
            // dataGridFollow
            // 
            this.dataGridFollow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridFollow.Location = new System.Drawing.Point(488, 29);
            this.dataGridFollow.Name = "dataGridFollow";
            this.dataGridFollow.Size = new System.Drawing.Size(320, 198);
            this.dataGridFollow.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(507, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Follow";
            // 
            // dataGridAFD
            // 
            this.dataGridAFD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridAFD.Location = new System.Drawing.Point(21, 246);
            this.dataGridAFD.Name = "dataGridAFD";
            this.dataGridAFD.Size = new System.Drawing.Size(551, 150);
            this.dataGridAFD.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 230);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Automata Finito Determinista";
            // 
            // dataGridSets
            // 
            this.dataGridSets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSets.Location = new System.Drawing.Point(814, 29);
            this.dataGridSets.Name = "dataGridSets";
            this.dataGridSets.Size = new System.Drawing.Size(262, 198);
            this.dataGridSets.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(740, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Sets";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1088, 408);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dataGridSets);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dataGridAFD);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dataGridFollow);
            this.Controls.Add(this.dataGridFirst);
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
    }
}