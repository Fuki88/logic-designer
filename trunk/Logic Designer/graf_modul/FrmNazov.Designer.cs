namespace Digi_graf_modul
{
    partial class FrmNazov
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
            this.lbl1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnNazov = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(160, 33);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(115, 13);
            this.lbl1.TabIndex = 0;
            this.lbl1.Text = "Zadajte názov modelu:";
            this.lbl1.Click += new System.EventHandler(this.lbl1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(134, 61);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(168, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnNazov
            // 
            this.btnNazov.Location = new System.Drawing.Point(336, 103);
            this.btnNazov.Name = "btnNazov";
            this.btnNazov.Size = new System.Drawing.Size(75, 23);
            this.btnNazov.TabIndex = 2;
            this.btnNazov.Text = "OK";
            this.btnNazov.UseVisualStyleBackColor = true;
            this.btnNazov.Click += new System.EventHandler(this.btnNazov_Click);
            // 
            // FrmNazov
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 138);
            this.Controls.Add(this.btnNazov);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lbl1);
            this.Name = "FrmNazov";
            this.Text = "FrmNazov";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnNazov;
    }
}