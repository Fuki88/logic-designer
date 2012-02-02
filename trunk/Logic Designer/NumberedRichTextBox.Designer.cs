using System;
using System.Windows.Forms;

namespace Logic_Designer
{
    partial class NumberedRichTextBox
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
        public void InitializeComponent()
        {
            this.textSplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textNumbering = new System.Windows.Forms.Label();
            this.textRichTextBox1 = new System.Windows.Forms.RichTextBox();
            this.textSplitContainer1.Panel1.SuspendLayout();
            this.textSplitContainer1.Panel2.SuspendLayout();
            this.textSplitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textSplitContainer1
            // 
            this.textSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textSplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.textSplitContainer1.Name = "textSplitContainer1";
            // 
            // textSplitContainer1.Panel1
            // 
            this.textSplitContainer1.Panel1.Controls.Add(this.textNumbering);
            // 
            // textSplitContainer1.Panel2
            // 
            this.textSplitContainer1.Panel2.Controls.Add(this.textRichTextBox1);
            this.textSplitContainer1.Size = new System.Drawing.Size(510, 384);
            this.textSplitContainer1.SplitterDistance = 25;
            this.textSplitContainer1.SplitterWidth = 1;
            this.textSplitContainer1.TabIndex = 0;
            // 
            // textNumbering
            // 
            this.textNumbering.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textNumbering.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textNumbering.ForeColor = System.Drawing.SystemColors.GrayText;
            this.textNumbering.Location = new System.Drawing.Point(0, 0);
            this.textNumbering.Name = "textNumbering";
            this.textNumbering.Size = new System.Drawing.Size(25, 384);
            this.textNumbering.TabIndex = 0;
            this.textNumbering.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textRichTextBox1
            // 
            this.textRichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textRichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textRichTextBox1.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textRichTextBox1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textRichTextBox1.Location = new System.Drawing.Point(0, 0);
            this.textRichTextBox1.Name = "textRichTextBox1";
            this.textRichTextBox1.Size = new System.Drawing.Size(484, 384);
            this.textRichTextBox1.TabIndex = 0;
            this.textRichTextBox1.Text = "";
            this.textRichTextBox1.WordWrap = false;
            this.textRichTextBox1.VScroll += new System.EventHandler(this.textRichTextBox1_VScroll);
            this.textRichTextBox1.Resize += new System.EventHandler(this.textRichTextBox1_Resize);
            this.textRichTextBox1.FontChanged += new System.EventHandler(this.textRichTextBox1_FontChanged);
            this.textRichTextBox1.TextChanged += new System.EventHandler(this.textRichTextBox1_TextChanged);

            //this.textRichTextBox1.Focused
            //this.textRichTextBox1.LostFocus

            // 
            // NumberedRichTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textSplitContainer1);
            this.Name = "NumberedRichTextBox";
            this.Size = new System.Drawing.Size(510, 384);
            this.textSplitContainer1.Panel1.ResumeLayout(false);
            this.textSplitContainer1.Panel2.ResumeLayout(false);
            this.textSplitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer textSplitContainer1;
        private System.Windows.Forms.Label textNumbering;
        //public aby som mohol pristupovat k rich text boxu z form1.cs
        public System.Windows.Forms.RichTextBox textRichTextBox1;

    
    }
}
