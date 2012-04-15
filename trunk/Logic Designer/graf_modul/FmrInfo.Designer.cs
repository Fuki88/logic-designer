namespace Digi_graf_modul
{
    partial class frmInfo
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listFunc = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblPocetHradiel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblNazov = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listFunc);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblPocetHradiel);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblNazov);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(936, 206);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Model";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // listFunc
            // 
            this.listFunc.FormattingEnabled = true;
            this.listFunc.Location = new System.Drawing.Point(162, 104);
            this.listFunc.Name = "listFunc";
            this.listFunc.Size = new System.Drawing.Size(768, 82);
            this.listFunc.TabIndex = 5;
            this.listFunc.SelectedIndexChanged += new System.EventHandler(this.listFunc_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Výstupné funkcie";
            // 
            // lblPocetHradiel
            // 
            this.lblPocetHradiel.AutoSize = true;
            this.lblPocetHradiel.Location = new System.Drawing.Point(159, 67);
            this.lblPocetHradiel.Name = "lblPocetHradiel";
            this.lblPocetHradiel.Size = new System.Drawing.Size(13, 13);
            this.lblPocetHradiel.TabIndex = 3;
            this.lblPocetHradiel.Text = "0";
            this.lblPocetHradiel.Click += new System.EventHandler(this.lblPocetHradiel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Počet hradiel";
            // 
            // lblNazov
            // 
            this.lblNazov.AutoSize = true;
            this.lblNazov.Location = new System.Drawing.Point(156, 30);
            this.lblNazov.Name = "lblNazov";
            this.lblNazov.Size = new System.Drawing.Size(27, 13);
            this.lblNazov.TabIndex = 1;
            this.lblNazov.Text = "N/A";
            this.lblNazov.Click += new System.EventHandler(this.lblNazov_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Názov modelu:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // frmInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 240);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmInfo";
            this.Text = "FmrInfo";
            this.Load += new System.EventHandler(this.frmInfo_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNazov;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPocetHradiel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listFunc;
    }
}