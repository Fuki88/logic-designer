using System;
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
        public static bool erroroccured = false;

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
                this.graf_modul1.SaveToCore();
                //ulozit text ako plain text
                if (textSaveFile.FilterIndex == 2)
                {
                   Plugin1.Blif bl = new Plugin1.Blif();
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
                if (textOpenFile.FilterIndex == 2)
                {
                    Plugin1.Blif bl = new Plugin1.Blif();
                    MessageBox.Show("nacitavam z blifka");
                    bl.opblf(textOpenFile.FileName);
                    this.graf_modul1.Cleanup();
                    this.graf_modul1.NacitajUzly("obvod_tmp.z5");
                }
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

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        public static String Model_Name;

        public static void SetModelName(string Nazov) {
            Model_Name = Nazov;
        }

        public static bool UlozUzly(string FileName)
        {
            try
            {
                ArrayList _Nodes = new ArrayList();
                Stream stream = File.Open(FileName, FileMode.Create);
                BinaryFormatter bF = new BinaryFormatter();
                foreach (NODE_CTRL node in NODES)
                {
                    PluginInterface.SavedNode _node = new PluginInterface.SavedNode();
                    //... tu sa naplnaju vlastnosti objektu
                    _node.ConIN = node.ConIN;
                    _node.ConOut = node.ConOut;
                    _node.Name = node.Text;
                    _node.Type = node.Type;
                    _node.id = node.ID;
                    _node.X = node.Left;
                    _node.Y = node.Top;
                    // bF.Serialize(stream, _node);
                    _Nodes.Add(_node);
                }

                bF.Serialize(stream, _Nodes);
                stream.Close();
                //MessageBox.Show("uzavrel som stream");

                string FileConName = FileName + "c";
                ArrayList _Cons = new ArrayList();
                stream = File.Open(FileConName, FileMode.Create);
                bF = new BinaryFormatter();

                foreach (CONNECTION con in CONNECTIONS)
                {
                    //myCons.Add(con);

                    PluginInterface.SavedCon _con = new PluginInterface.SavedCon();
                    _con.Name = con.Name;
                    _con.StartNode = con.StartNode_Text;
                    _con.EndNode = con.EndNode_Text;
                    _Cons.Add(_con);


                }
                bF.Serialize(stream, _Cons);
                stream.Close();
                return true;


            }
            catch
            {
                return false;
            }
        }
        
        
        
        public static bool UlozUzly()
        {
            try
            {
                Stream stream = File.Open("blif_obvod_tmp.z5", FileMode.Create);
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
                    bF.Serialize(stream, _node);
                   
                }

                stream.Close();
                //MessageBox.Show("uzavrel som stream");
                stream = File.Open("obvod_tmp.z5c", FileMode.Create);
                bF = new BinaryFormatter();

                foreach (CONNECTION con in CONNECTIONS)
                {
                    SavedCon _con = new SavedCon();
                    _con.Name = con.Name;
                    _con.StartNode = con.StartNode_Text;
                    _con.EndNode = con.EndNode_Text;
                    //MessageBox.Show(con.StartNode_Text); MessageBox.Show(con.EndNode_Text);
                    bF.Serialize(stream, _con);
                }
                stream.Close();
                UlozUzly("obvod_tmp.z5");
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

        public static void Cleanup(){
            ClearCon();
            ClearNode();
            
        }

        public static void CreateNode(int x, int y, int ID, string Name, string Type, string[] PortsIN, string PortsOUT){
            NODE_CTRL N = new NODE_CTRL();
            if (PortsIN != null)
            {
                foreach (string INn in PortsIN)
                    N.ConIN.Add(INn);
            }
            if (PortsOUT != "")
            {
                N.ConOut.Add(PortsOUT);
            }
            N.Text = Name;
            N.Type = Type;
            N.ID = ID;
            N.Left = x;
            N.Top = y;
            NODES.Add(N);

        }

        public static void MakeCons() { //vytvori prepojenia k nodom (ak nahodou neexistuju)

            int flag = 0;

            foreach (NODE_CTRL node in NODES)
            {

                foreach (string cI in node.ConIN)
                {
                    foreach (NODE_CTRL nn in NODES) {
                        foreach (string cO in nn.ConOut) {
                            if (cI == cO) {
                                foreach (CONNECTION conn in CONNECTIONS)
                                {
                                    if (conn.Name == cI)
                                    {
                                        conn.EndNode_Text = node.Type;
                                        conn.StartNode_Text = nn.Type;
                                        flag = 1;
                                    }
                                }
                                if (flag == 0) {
                                    CONNECTION connection = new CONNECTION();
                                    connection.Name = cI;
                                    connection.EndNode_Text = node.Type;
                                    connection.StartNode_Text = nn.Type;
                                    CONNECTIONS.Add(connection);
                                } flag = 0;
                            }
                        }
                    
                    }
                    
                }

            }

            //foreach (NODE_CTRL abc in NODES) MessageBox.Show(abc.Type);
            //foreach (CONNECTION con in CONNECTIONS) MessageBox.Show(con.Name);
        }

        public static void MakePins()
        { //vytvori vstupne piny
            ArrayList pins = new ArrayList();
            int flag = 0;
            int i = 0;

            foreach (NODE_CTRL node in NODES)
            {
                //MessageBox.Show(node.Text + " je " + node.Level.ToString());
                foreach (string cI in node.ConIN)
                {
                    foreach (NODE_CTRL nn in NODES)
                    {
                        foreach (string cO in nn.ConOut)
                        {
                            if (cI == cO)
                            {
                                flag = 1; // ak sa nasla dvojica portov ktore sa daju prepojit
                                if (node.Level <= nn.Level) node.Level = nn.Level + 1;
                                
                                //MessageBox.Show("Node " + node.Text + ": " + node.Level.ToString() + "\nnn " + nn.Text + ": " + nn.Level.ToString());
                            }
                            //                            else nn.Left += 100; // posun doprava
                        }

                    }
                    if (flag == 0) { // kedze sa nenasla dvojica portov na prepojenie
                        ArrayList outs = new ArrayList();
                        ArrayList ins = new ArrayList();
                        outs.Add(cI);
                        NODE_CTRL N = new NODE_CTRL();
                        N.ConIN = ins;
                        N.ConOut = outs;
                        N.Text = cI;
                        N.Type = "IN";
                        N.ID = CONNECTIONS.Count+1;
                        N.Left = 20;
                        N.Top = node.Top + i * 50;
                        pins.Add(N);
                    }
                    flag = 0;
                }
            }

            foreach (NODE_CTRL node in NODES)
            {
                foreach (string cO in node.ConOut)
                {
                    foreach (NODE_CTRL nn in NODES)
                    {
                        foreach (string cI in nn.ConIN)
                        {
                            if (cI == cO) flag = 1;
                            nn.Left = 50 + (nn.Level * 100);
                        }
                    }
                    if (flag == 0)
                    { // kedze sa nenasla dvojica portov na prepojenie
                        ArrayList outs = new ArrayList();
                        ArrayList ins = new ArrayList();
                        ins.Add(cO);
                        NODE_CTRL N = new NODE_CTRL();
                        N.ConIN = ins;
                        N.ConOut = outs;
                        N.Text = cO;
                        N.Type = "OUT";
                        N.ID = CONNECTIONS.Count + 1;
                        N.Left = 150 + (node.Level * 100);
                        N.Top = node.Top + 40;
                        pins.Add(N);
                    }
                    flag = 0;
                }
            }
            
            NODES.AddRange(pins);
        }


        private void verifikacia1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

     private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2)
            {
                verifikacia1.verifiakaciaSelected();
            }
            else
            {
                verifikacia1.verifiakaciaDeSelected();
            }

            if (tabControl1.SelectedIndex == 1 && NumberedRichTextBox.textComboBox1.SelectedItem.ToString() == "vhdl")
               {
                   ParseVHDL parser = new ParseVHDL();
                   parser.Parse(this.numberedRichTextBox1.textRichTextBox1.Text);
                   parser.Draw();
                   try
                   {
                       this.graf_modul1.Cleanup();
                       this.graf_modul1.NacitajUzly("obvod_tmp.z5");
                   }
                   catch
                   {

                   }
                
               }

        }
        
        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "help.chm");
        }

    }
}
