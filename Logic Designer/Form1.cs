﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Resources;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using PluginInterface;
using System.Xml.Serialization; 

namespace Logic_Designer
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
        }


        public static ArrayList CONNECTIONS = new ArrayList();
        public static ArrayList NODES = new ArrayList();

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
            //textSaveFile.DefaultExt = "*.blif";
            textSaveFile.Filter = "VHDL|*.vhd;*.vhdl|Blif|*.blif|Kiss|*.kiss|PLA|*.pla|Verilog|*.v|All files|*.*";

            //ukaz okno a skontroluj ci je def. nazov
            if (textSaveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                textSaveFile.FileName != "")
            {
                //ulozit text ako plain text
                if (textSaveFile.FilterIndex == 2)
                {
                    Blif bl = new Blif();
                    MessageBox.Show("ukladam do blifka");
                    bl.svblf(textSaveFile.FileName);
                }
                else
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
            //textOpenFile.DefaultExt = "*.blif";
            textOpenFile.Filter = "VHDL|*.vhd;*.vhdl|Blif|*.blif|Kiss|*.kiss|PLA|*.pla|Verilog|*.v|All files|*.*";

            //ked je subor otvoreny
            if (textOpenFile.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                textOpenFile.FileName.Length > 0)
            {
                //nacitat do rich text boxu subor v plain texte
                this.numberedRichTextBox1.textRichTextBox1.LoadFile(textOpenFile.FileName, RichTextBoxStreamType.PlainText);

                //z koncovky sa predurci pouzite zvyraznovanie syntaxe
                string extension = Path.GetExtension(textOpenFile.FileName);
                numberedRichTextBox1.setExtension(extension.Substring(1));

                //zvyrazni sa cely text
                numberedRichTextBox1.highLightSyntaxOnOpen();
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

        private void graf_modul1_Load(object sender, EventArgs e)
        {
            graf_modul1.BringToFront();
            graf_modul1.Show();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        public static bool UlozUzly()
        {
            try
            {
                Stream stream = File.Open("obvod_tmp.z5", FileMode.Create);
                BinaryFormatter bF = new BinaryFormatter();

                foreach (NODE_CTRL node in NODES)
                {
                    SavedNode _node = new SavedNode();
                    //... tu sa naplnaju vlastnosti objektu
                    _node.ConIN = node.ConIN;
                    _node.ConOut = node.ConOut;
                    _node.Name = node.Type;
                    _node.Type = node.Type;
                    _node.id = node.ID;
                    _node.X = node.Left;
                    _node.Y = node.Top;
                    MessageBox.Show(_node.Name);
                    bF.Serialize(stream, _node);
                }

                stream.Close();
                MessageBox.Show("uzavrel som stream");
                stream = File.Open("arc_tmp.z5", FileMode.Create);
                bF = new BinaryFormatter();

                foreach (CONNECTION con in CONNECTIONS)
                {
                    SavedCon _con = new SavedCon();
                    _con.Name = con.Name;
                    _con.StartNode = con.StartNode_Text;
                    _con.EndNode = con.EndNode_Text;
                    bF.Serialize(stream, _con);
                }
                stream.Close();
                return true;


            }
            catch
            {
                return false;
            }
        }

        public static void ClearCon()
        {
            CONNECTIONS.Clear();
        }

        public static void ClearNode()
        {

            NODES.Clear();
        }

        public static bool SetCon(string Name, string StartNode_Text, string EndNode_Text)
        {
            try
            {
                CONNECTION C = new CONNECTION();
                C.Name = Name;
                C.StartNode_Text = StartNode_Text;
                C.EndNode_Text = EndNode_Text;
                CONNECTIONS.Add(C);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool SetNode(int ID, ArrayList ConIN, ArrayList ConOut, string Text, string Type, int Left, int Top)
        {
            try
            {
                NODE_CTRL N = new NODE_CTRL();
                N.ConIN = ConIN;
                N.ConOut = ConOut;
                N.Text = Text;
                N.Type = Type;
                N.ID = ID;
                N.Left = Left;
                N.Top = Top;
                NODES.Add(N);
            }
            catch {
                return false;
            }
            return true;
        }

    }
}
