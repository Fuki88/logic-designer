namespace Logic_Designer.verifikacia
{
    partial class verifikacia
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSimulate = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSimulate
            // 
            this.btnSimulate.Location = new System.Drawing.Point(23, 23);
            this.btnSimulate.Name = "btnSimulate";
            this.btnSimulate.Size = new System.Drawing.Size(111, 23);
            this.btnSimulate.TabIndex = 0;
            this.btnSimulate.Text = "Vyhodnoť obvod";
            this.btnSimulate.UseVisualStyleBackColor = true;
            this.btnSimulate.Click += new System.EventHandler(this.btnSimulate_Click);
            // 
            // textBox1
            // 
            this.textBox1.HideSelection = false;
            this.textBox1.Location = new System.Drawing.Point(140, 26);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(813, 197);
            this.textBox1.TabIndex = 1;
            // 
            // verifikacia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnSimulate);
            this.Name = "verifikacia";
            this.Size = new System.Drawing.Size(953, 571);
            this.Load += new System.EventHandler(this.verifikacia_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSimulate;
        private System.Windows.Forms.TextBox textBox1;
    }
}
