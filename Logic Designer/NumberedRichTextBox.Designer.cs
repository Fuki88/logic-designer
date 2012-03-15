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
        private void InitializeComponent()
        {
            this.textSplitContainer2 = new System.Windows.Forms.SplitContainer();
            this.textPanel1 = new System.Windows.Forms.Panel();
            this.textSplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textNumbering = new System.Windows.Forms.Label();
            this.textHighlightDown = new System.Windows.Forms.Label();
            this.textHighlightUp = new System.Windows.Forms.Label();
            this.textRichTextBox1 = new System.Windows.Forms.RichTextBox();
            this.textLabelStatus = new System.Windows.Forms.Label();
            Logic_Designer.NumberedRichTextBox.textComboBox1 = new System.Windows.Forms.ComboBox();
            this.textSplitContainer2.Panel1.SuspendLayout();
            this.textSplitContainer2.Panel2.SuspendLayout();
            this.textSplitContainer2.SuspendLayout();
            this.textPanel1.SuspendLayout();
            this.textSplitContainer1.Panel1.SuspendLayout();
            this.textSplitContainer1.Panel2.SuspendLayout();
            this.textSplitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textSplitContainer2
            // 
            this.textSplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textSplitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.textSplitContainer2.Location = new System.Drawing.Point(0, 0);
            this.textSplitContainer2.Name = "textSplitContainer2";
            this.textSplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // textSplitContainer2.Panel1
            // 
            this.textSplitContainer2.Panel1.Controls.Add(this.textPanel1);
            // 
            // textSplitContainer2.Panel2
            // 
            this.textSplitContainer2.Panel2.Controls.Add(Logic_Designer.NumberedRichTextBox.textComboBox1);
            this.textSplitContainer2.Panel2.Controls.Add(this.textLabelStatus);
            this.textSplitContainer2.Panel2MinSize = 15;
            this.textSplitContainer2.Size = new System.Drawing.Size(899, 785);
            this.textSplitContainer2.SplitterDistance = 764;
            this.textSplitContainer2.SplitterWidth = 1;
            this.textSplitContainer2.TabIndex = 1;
            // 
            // textPanel1
            // 
            this.textPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textPanel1.Controls.Add(this.textSplitContainer1);
            this.textPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textPanel1.Location = new System.Drawing.Point(0, 0);
            this.textPanel1.Name = "textPanel1";
            this.textPanel1.Size = new System.Drawing.Size(899, 764);
            this.textPanel1.TabIndex = 1;
            // 
            // textSplitContainer1
            // 
            this.textSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textSplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.textSplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.textSplitContainer1.Name = "textSplitContainer1";
            // 
            // textSplitContainer1.Panel1
            // 
            this.textSplitContainer1.Panel1.Controls.Add(this.textNumbering);
            // 
            // textSplitContainer1.Panel2
            // 
            this.textSplitContainer1.Panel2.Controls.Add(this.textHighlightDown);
            this.textSplitContainer1.Panel2.Controls.Add(this.textHighlightUp);
            this.textSplitContainer1.Panel2.Controls.Add(this.textRichTextBox1);
            this.textSplitContainer1.Size = new System.Drawing.Size(895, 760);
            this.textSplitContainer1.SplitterDistance = 54;
            this.textSplitContainer1.SplitterWidth = 1;
            this.textSplitContainer1.TabIndex = 0;
            // 
            // textNumbering
            // 
            this.textNumbering.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textNumbering.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textNumbering.BackColor = System.Drawing.SystemColors.Control;
            this.textNumbering.ForeColor = System.Drawing.SystemColors.GrayText;
            this.textNumbering.Location = new System.Drawing.Point(0, 0);
            this.textNumbering.Name = "textNumbering";
            this.textNumbering.Size = new System.Drawing.Size(54, 760);
            this.textNumbering.TabIndex = 0;
            this.textNumbering.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textHighlightDown
            //
            this.textHighlightDown.BackColor = System.Drawing.SystemColors.Control;
            this.textHighlightDown.Location = new System.Drawing.Point(0, 18);
            this.textHighlightDown.Name = "textHighlightDown";
            this.textHighlightDown.Size = new System.Drawing.Size(0, 1);
            this.textHighlightDown.TabIndex = 0;
            // 
            // textHighlightUp
            //
            this.textHighlightUp.BackColor = System.Drawing.SystemColors.Control;
            this.textHighlightUp.Location = new System.Drawing.Point(0, 0);
            this.textHighlightUp.Name = "textHighlightUp";
            this.textHighlightUp.Size = new System.Drawing.Size(0, 1);
            this.textHighlightUp.TabIndex = 0;
            // 
            // textRichTextBox1
            // 
            this.textRichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textRichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textRichTextBox1.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textRichTextBox1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textRichTextBox1.Location = new System.Drawing.Point(0, 0);
            this.textRichTextBox1.Name = "textRichTextBox1";
            this.textRichTextBox1.Size = new System.Drawing.Size(840, 760);
            this.textRichTextBox1.TabIndex = 0;
            this.textRichTextBox1.Text = "";
            this.textRichTextBox1.WordWrap = false;
            this.textRichTextBox1.VScroll += new System.EventHandler(this.textRichTextBox1_VScroll);
            this.textRichTextBox1.SelectionChanged += new System.EventHandler(this.textRichTextBox1_SelectionChanged);
            this.textRichTextBox1.Resize += new System.EventHandler(this.textRichTextBox1_Resize);
            this.textRichTextBox1.FontChanged += new System.EventHandler(this.textRichTextBox1_FontChanged);
            this.textRichTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textRichTextBox1_KeyPress);
            this.textRichTextBox1.TextChanged += new System.EventHandler(this.textRichTextBox1_TextChanged);
            // 
            // textLabelStatus
            // 
            this.textLabelStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textLabelStatus.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textLabelStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.textLabelStatus.Location = new System.Drawing.Point(0, 0);
            this.textLabelStatus.Name = "textLabelStatus";
            this.textLabelStatus.Size = new System.Drawing.Size(899, 20);
            this.textLabelStatus.TabIndex = 0;
            this.textLabelStatus.Text = "Sumár:   1 r.    0 z.    |    Aktuálny:    1. r.    1. z.    ";
            this.textLabelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textComboBox1
            //
            Logic_Designer.NumberedRichTextBox.textComboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            Logic_Designer.NumberedRichTextBox.textComboBox1.Location = new System.Drawing.Point(775, 0);
            Logic_Designer.NumberedRichTextBox.textComboBox1.Name = "textComboBox1";
            Logic_Designer.NumberedRichTextBox.textComboBox1.Size = new System.Drawing.Size(121, 21);
            Logic_Designer.NumberedRichTextBox.textComboBox1.TabIndex = 0;
            Logic_Designer.NumberedRichTextBox.textComboBox1.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectionChangeCommitted);
            // 
            // NumberedRichTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textSplitContainer2);
            this.Name = "NumberedRichTextBox";
            this.Size = new System.Drawing.Size(899, 785);
            this.textSplitContainer2.Panel1.ResumeLayout(false);
            this.textSplitContainer2.Panel2.ResumeLayout(false);
            this.textSplitContainer2.ResumeLayout(false);
            this.textPanel1.ResumeLayout(false);
            this.textSplitContainer1.Panel1.ResumeLayout(false);
            this.textSplitContainer1.Panel2.ResumeLayout(false);
            this.textSplitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer textSplitContainer2;
        private System.Windows.Forms.SplitContainer textSplitContainer1;
        private System.Windows.Forms.Label textNumbering;
        public System.Windows.Forms.RichTextBox textRichTextBox1;
        private System.Windows.Forms.Panel textPanel1;
        private System.Windows.Forms.Label textLabelStatus;
        private System.Windows.Forms.Label textHighlightDown;
        private System.Windows.Forms.Label textHighlightUp;
        private static System.Windows.Forms.ComboBox textComboBox1;

        //public aby som mohol pristupovat k rich text boxu z form1.cs
    }
}
