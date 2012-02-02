using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Logic_Designer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void loadModuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = this.loadTextEditorModule.ShowDialog();
            if (result == DialogResult.OK)
            {
                toolStripStatusLabel1.Text = "Module " + loadTextEditorModule.FileName + " will be loaded";
                numberedRichTextBox1.textRichTextBox1.Clear();
            }
        }

        private void test1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("Chcete uložiť aktuálny súbor?", "Logic Designer", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                saveFile(); 
            }
            numberedRichTextBox1.textRichTextBox1.Clear();
        }

        private void test2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }
        public void saveFile()
        {
            SaveFileDialog textSaveFile = new SaveFileDialog();

            //default blif + kiss, pla, vhdl
            textSaveFile.Title = "Uložiť súbor";
            textSaveFile.DefaultExt = "*.blif";
            textSaveFile.Filter = "Blif|*.blif|Kiss|*.kiss|PLA|*.pla|VHDL|*.vhdl";

            //ukaz okno a skontroluj ci je def. nazov
            if (textSaveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                textSaveFile.FileName != "")
            {
                //ulozit text ako plain text
                this.numberedRichTextBox1.textRichTextBox1.SaveFile(textSaveFile.FileName, RichTextBoxStreamType.PlainText);
            }

        }

        //otvorit subor
        public void openFile()
        {
            OpenFileDialog textOpenFile = new OpenFileDialog();

            //jeden subor + default .blif + ostatne .blif, .kiss, .pla, .vhd, .vhdl
            textOpenFile.Title = "Otvoriť súbor";
            textOpenFile.Multiselect = false;
            textOpenFile.DefaultExt = "*.blif";
            textOpenFile.Filter = "Blif|*.blif|Kiss|*.kiss|PLA|*.pla|VHDL|*.vhd;*.vhdl";

            //ked je subor otvoreny
            if (textOpenFile.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                textOpenFile.FileName.Length > 0)
            {
                //nacitat do rich text boxu subor v plain texte
                this.numberedRichTextBox1.textRichTextBox1.LoadFile(textOpenFile.FileName, RichTextBoxStreamType.PlainText);
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            numberedRichTextBox1.textRichTextBox1.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            numberedRichTextBox1.textRichTextBox1.Redo();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            numberedRichTextBox1.textRichTextBox1.Cut();
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            numberedRichTextBox1.textRichTextBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            numberedRichTextBox1.textRichTextBox1.Paste();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            numberedRichTextBox1.textRichTextBox1.SelectAll();
        }

        private void aboutLogicDesignerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.Show();
        }

    }
}
