using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;
using PluginInterface;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Plugin1
{
    /// <summary>
    /// Summary description for ctlMain.
    /// </summary>
    public class Blif : System.Windows.Forms.UserControl, IPlugin
    {
        private TextBox txtFile;
        private ToolStripMenuItem menuFile1;
        private System.Windows.Forms.OpenFileDialog cdbOpen;
        private System.Windows.Forms.TextBox txtFile1;
        private System.Windows.Forms.LinkLabel butOpenFile;
        private System.Windows.Forms.LinkLabel butSaveFile;
        private System.Windows.Forms.SaveFileDialog cdbSave;
        private TextBox txtVstupy;
        private TextBox textBoxModel;
        private TextBox textBoxPocetVstupov;
        private TextBox textBoxVstupy;
        private TextBox textBoxPocetVystupov;
        private TextBox textBoxVystupy;
        private TextBox textBoxPocetPremenne;
        private TextBox textBoxPremenne;
        private TextBox textBoxPremennaVystup;
        private TextBox textBoxObvod;
        private TextBox textBoxVstupAnd;
        private ToolStripMenuItem menu;
        private ToolStripMenuItem menu1;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        //Declarations of all our internal plugin variables
        string myName = "BLIF";
        string myDescription = "Pridáva podporu pre súborový formát BLIF.";
        string myAuthor = "Tomáš Takács";
        string myVersion = "1.0.0";
        string myType = "Logic";
        IPluginHost myHost = null;
        //System.Windows.Forms.UserControl myMainInterface = new ctlMain();

        /// <summary>
        /// Description of the Plugin's purpose
        /// </summary>
        public string Description
        {
            get { return myDescription; }
        }

        /// <summary>
        /// Author of the plugin
        /// </summary>

        public string Type
        {
            get { return myType; }
        }

        public string Author
        {
            get { return myAuthor; }
        }

        /// <summary>
        /// Host of the plugin.
        /// </summary>
        public IPluginHost Host
        {

            get { return myHost; }
            set { myHost = value; }
        }

        public string PlugName
        {
            get { return myName; }
        }

        public System.Windows.Forms.UserControl MainInterface
        {
            get { return this; }
        }

        public string Version
        {
            get { return myVersion; }
        }

        public void Initialize()
        {
            this.Visible = false;
            txtFile = ((TextBox)myHost.ReturnObject("txtMain"));
            menuFile1 = (ToolStripMenuItem)myHost.ReturnObject("menuFile");

            if (menuFile1 != null)
            {

                menu1 = new ToolStripMenuItem("Otvor BLIF");
                //menu1.Click += new EventHandler(butOpenFile_LinkClicked);

                menuFile1.DropDownItems.Insert(0, menu1);
            }

            if (menuFile1 != null)
            {

                menu = new ToolStripMenuItem("Ulož BLIF");
                //menu.Click += new EventHandler(saveBlif);

                menuFile1.DropDownItems.Insert(0, menu);
            }



            //This is the first Function called by the host...
            //Put anything needed to start with here first
        }

        /*public void Dispose()
        {
            //Put any cleanup code in here for when the program is stopped
        }*/




        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                menu1.Dispose();
                menu.Dispose();
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.cdbOpen = new System.Windows.Forms.OpenFileDialog();
            this.txtFile1 = new System.Windows.Forms.TextBox();
            this.butOpenFile = new System.Windows.Forms.LinkLabel();
            this.butSaveFile = new System.Windows.Forms.LinkLabel();
            this.cdbSave = new System.Windows.Forms.SaveFileDialog();
            this.txtVstupy = new System.Windows.Forms.TextBox();
            this.textBoxModel = new System.Windows.Forms.TextBox();
            this.textBoxPocetVstupov = new System.Windows.Forms.TextBox();
            this.textBoxVstupy = new System.Windows.Forms.TextBox();
            this.textBoxPocetVystupov = new System.Windows.Forms.TextBox();
            this.textBoxVystupy = new System.Windows.Forms.TextBox();
            this.textBoxPocetPremenne = new System.Windows.Forms.TextBox();
            this.textBoxPremenne = new System.Windows.Forms.TextBox();
            this.textBoxPremennaVystup = new System.Windows.Forms.TextBox();
            this.textBoxObvod = new System.Windows.Forms.TextBox();
            this.textBoxVstupAnd = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cdbOpen
            // 
            this.cdbOpen.Title = "Open...";
            // 
            // txtFile1
            // 
            this.txtFile1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile1.Location = new System.Drawing.Point(3, 25);
            this.txtFile1.Multiline = true;
            this.txtFile1.Name = "txtFile1";
            this.txtFile1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFile1.Size = new System.Drawing.Size(303, 319);
            this.txtFile1.TabIndex = 0;
            // 
            // butOpenFile
            // 
            this.butOpenFile.ActiveLinkColor = System.Drawing.Color.Blue;
            this.butOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butOpenFile.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.butOpenFile.Location = new System.Drawing.Point(16, 352);
            this.butOpenFile.Name = "butOpenFile";
            this.butOpenFile.Size = new System.Drawing.Size(112, 16);
            this.butOpenFile.TabIndex = 1;
            this.butOpenFile.TabStop = true;
            this.butOpenFile.Text = "Open File...";
            this.butOpenFile.VisitedLinkColor = System.Drawing.Color.Blue;
            //this.butOpenFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.butOpenFile_LinkClicked);
            // 
            // butSaveFile
            // 
            this.butSaveFile.ActiveLinkColor = System.Drawing.Color.Blue;
            this.butSaveFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butSaveFile.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.butSaveFile.Location = new System.Drawing.Point(890, 352);
            this.butSaveFile.Name = "butSaveFile";
            this.butSaveFile.Size = new System.Drawing.Size(72, 16);
            this.butSaveFile.TabIndex = 2;
            this.butSaveFile.TabStop = true;
            this.butSaveFile.Text = "Save File...";
            this.butSaveFile.VisitedLinkColor = System.Drawing.Color.Blue;
            this.butSaveFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.butSaveFile_LinkClicked);
            // 
            // cdbSave
            // 
            this.cdbSave.Filter = "Text Files|*.txt";
            this.cdbSave.Title = "Save...";
            // 
            // txtVstupy
            // 
            this.txtVstupy.Location = new System.Drawing.Point(340, 39);
            this.txtVstupy.Multiline = true;
            this.txtVstupy.Name = "txtVstupy";
            this.txtVstupy.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtVstupy.Size = new System.Drawing.Size(223, 305);
            this.txtVstupy.TabIndex = 3;
            this.txtVstupy.TextChanged += new System.EventHandler(this.txtVstupy_TextChanged);
            // 
            // textBoxModel
            // 
            this.textBoxModel.Location = new System.Drawing.Point(619, 39);
            this.textBoxModel.Name = "textBoxModel";
            this.textBoxModel.Size = new System.Drawing.Size(100, 20);
            this.textBoxModel.TabIndex = 4;
            // 
            // textBoxPocetVstupov
            // 
            this.textBoxPocetVstupov.Location = new System.Drawing.Point(619, 75);
            this.textBoxPocetVstupov.Name = "textBoxPocetVstupov";
            this.textBoxPocetVstupov.Size = new System.Drawing.Size(100, 20);
            this.textBoxPocetVstupov.TabIndex = 5;
            // 
            // textBoxVstupy
            // 
            this.textBoxVstupy.Location = new System.Drawing.Point(619, 114);
            this.textBoxVstupy.Name = "textBoxVstupy";
            this.textBoxVstupy.Size = new System.Drawing.Size(100, 20);
            this.textBoxVstupy.TabIndex = 6;
            // 
            // textBoxPocetVystupov
            // 
            this.textBoxPocetVystupov.Location = new System.Drawing.Point(619, 155);
            this.textBoxPocetVystupov.Name = "textBoxPocetVystupov";
            this.textBoxPocetVystupov.Size = new System.Drawing.Size(100, 20);
            this.textBoxPocetVystupov.TabIndex = 7;
            // 
            // textBoxVystupy
            // 
            this.textBoxVystupy.Location = new System.Drawing.Point(619, 194);
            this.textBoxVystupy.Name = "textBoxVystupy";
            this.textBoxVystupy.Size = new System.Drawing.Size(100, 20);
            this.textBoxVystupy.TabIndex = 8;
            // 
            // textBoxPocetPremenne
            // 
            this.textBoxPocetPremenne.Location = new System.Drawing.Point(619, 231);
            this.textBoxPocetPremenne.Name = "textBoxPocetPremenne";
            this.textBoxPocetPremenne.Size = new System.Drawing.Size(100, 20);
            this.textBoxPocetPremenne.TabIndex = 9;
            // 
            // textBoxPremenne
            // 
            this.textBoxPremenne.Location = new System.Drawing.Point(619, 269);
            this.textBoxPremenne.Name = "textBoxPremenne";
            this.textBoxPremenne.Size = new System.Drawing.Size(100, 20);
            this.textBoxPremenne.TabIndex = 10;
            // 
            // textBoxPremennaVystup
            // 
            this.textBoxPremennaVystup.Location = new System.Drawing.Point(619, 308);
            this.textBoxPremennaVystup.Name = "textBoxPremennaVystup";
            this.textBoxPremennaVystup.Size = new System.Drawing.Size(100, 20);
            this.textBoxPremennaVystup.TabIndex = 11;
            // 
            // textBoxObvod
            // 
            this.textBoxObvod.Location = new System.Drawing.Point(764, 39);
            this.textBoxObvod.Multiline = true;
            this.textBoxObvod.Name = "textBoxObvod";
            this.textBoxObvod.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxObvod.Size = new System.Drawing.Size(219, 289);
            this.textBoxObvod.TabIndex = 12;
            // 
            // textBoxVstupAnd
            // 
            this.textBoxVstupAnd.Location = new System.Drawing.Point(619, 334);
            this.textBoxVstupAnd.Name = "textBoxVstupAnd";
            this.textBoxVstupAnd.Size = new System.Drawing.Size(100, 20);
            this.textBoxVstupAnd.TabIndex = 13;
            // 
            // ctlMain
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.textBoxVstupAnd);
            this.Controls.Add(this.textBoxObvod);
            this.Controls.Add(this.textBoxPremennaVystup);
            this.Controls.Add(this.textBoxPremenne);
            this.Controls.Add(this.textBoxPocetPremenne);
            this.Controls.Add(this.textBoxVystupy);
            this.Controls.Add(this.textBoxPocetVystupov);
            this.Controls.Add(this.textBoxVstupy);
            this.Controls.Add(this.textBoxPocetVstupov);
            this.Controls.Add(this.textBoxModel);
            this.Controls.Add(this.txtVstupy);
            this.Controls.Add(this.butSaveFile);
            this.Controls.Add(this.butOpenFile);
            this.Controls.Add(this.txtFile1);
            this.Name = "ctlMain";
            this.Size = new System.Drawing.Size(986, 376);
            this.Load += new System.EventHandler(this.ctlMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        // ARRAYLIST TO STRING
        public string ArrayListToString(System.Collections.ArrayList ar)
        {
            return ArrayListToString(ar, " ");
        }
        public string ArrayListToString(System.Collections.ArrayList ar, char delim)
        {
            return ArrayListToString(ar, delim.ToString());
        }
        public string ArrayListToString(System.Collections.ArrayList ar, string delim)
        {
            return string.Join(delim, (string[])ar.ToArray(typeof(string)));
        }

        public void svblf(String Nazov){
        saveBlif(Nazov);
        }

        public void opblf(String Nazov)
        {
            OpenBlif(Nazov);
        }
        //SPRACOVANIE OBVODU DO SUBORU
        public void saveBlif(string Nazov)
        {
            Stream stream = null;
            SavedNode uzol = null;
            StringBuilder subor = new StringBuilder();
            ArrayList inputs = new ArrayList();
            ArrayList outputs = new ArrayList();
            ArrayList vstupy_f = new ArrayList();
            ArrayList vstupy_and = new ArrayList();
            ArrayList vstupy_or = new ArrayList();


            Logic_Designer.Form1.UlozUzly();
            int pocet_obj = 0;
            try
            {
                stream = File.Open("blif_obvod_tmp.z5", FileMode.Open);
                BinaryFormatter bF = new BinaryFormatter();
                //stream.Position = 0;

                if (stream != null)
                {
                    //STREAM START
                    List<SavedNode> objekty = new List<SavedNode>();
                    stream.Position = 0;
                    while (stream.Position != stream.Length)
                    {
                        uzol = (SavedNode)bF.Deserialize(stream);
                        objekty.Add(uzol);
                        pocet_obj++;
                    }
                    stream.Close();

                    //SUBOR START
                    //nazov modelu
                    string nazov_modelu = "model1";//myHost.GetModelName();
                    subor.AppendLine(".model " + nazov_modelu);

                    //vstupy
                    foreach (SavedNode objekt in objekty)
                    {
                        if (objekt.Type == "IN")
                        {
                            inputs.Add(ArrayListToString(objekt.ConOut));
                        }
                    }
                    subor.AppendLine(".inputs " + ArrayListToString(inputs));
                    //MessageBox.Show(subor.ToString());

                    //vystupy
                    foreach (SavedNode objekt in objekty)
                    {
                        if (objekt.Type == "OUT")
                        {
                            outputs.Add(ArrayListToString(objekt.ConIN));
                        }
                    }
                    subor.AppendLine(".outputs " + ArrayListToString(outputs));

                    //funkcie
                    foreach (SavedNode objekt in objekty)
                    {
                        //objekt je OR 
                        if ((objekt.Type == "OR2" || objekt.Type == "OR3" || objekt.Type == "OR4" || objekt.Type == "OR5"
                                || objekt.Type == "OR6" || objekt.Type == "OR7" || objekt.Type == "OR8"))
                        {
                            ArrayList kopia_or = new ArrayList();
                            vstupy_or = objekt.ConIN;

                            for (int i = 0; i < vstupy_or.Count; i++)
                            {
                                kopia_or.Add(vstupy_or[i]);
                                if (vstupy_or[i].ToString().Contains("!"))
                                {
                                    vstupy_or[i] = vstupy_or[i].ToString().TrimStart('!');
                                }
                            }
                            subor.AppendLine(".names " + ArrayListToString(objekt.ConIN) + " " + ArrayListToString(objekt.ConOut));

                            //pre pocet riadkov funkcie
                            for (int i = 0; i < objekt.ConIN.Count; i++)
                            {
                                //MessageBox.Show(objekt.ConIN.Count.ToString());
                                char[] riadok_f = new char[objekt.ConIN.Count + 2];
                                for (int j = 0; j < objekt.ConIN.Count + 2; j++)
                                {
                                    if ((j == i) && (j < objekt.ConIN.Count) && (kopia_or[i].ToString().Contains("!")))
                                        riadok_f[j] = '0';
                                    else if ((j == i) && (j < objekt.ConIN.Count) && !(kopia_or[i].ToString().Contains("!")))
                                        riadok_f[j] = '1';
                                    else if (j == objekt.ConIN.Count)
                                        riadok_f[objekt.ConIN.Count] = ' ';
                                    else if (j == objekt.ConIN.Count + 1)
                                        riadok_f[objekt.ConIN.Count + 1] = '1';
                                    else
                                        riadok_f[j] = '-';

                                }
                                string riadok = new string(riadok_f);
                                subor.AppendLine(riadok);

                            }
                        }


                        //objekt je NAND
                        if ((objekt.Type == "NAND2" || objekt.Type == "NAND3" || objekt.Type == "NAND4" || objekt.Type == "NAND5"
                                || objekt.Type == "NAND6" || objekt.Type == "NAND7" || objekt.Type == "NAND8"))
                        {
                            subor.AppendLine(".names " + ArrayListToString(objekt.ConIN) + " " + ArrayListToString(objekt.ConOut));

                            //pre pocet riadkov funkcie
                            for (int i = 0; i < objekt.ConIN.Count; i++)
                            {
                                char[] riadok_f = new char[objekt.ConIN.Count + 2];
                                for (int j = 0; j < objekt.ConIN.Count + 2; j++)
                                {
                                    if (j == i)
                                        riadok_f[j] = '0';
                                    else if (j == objekt.ConIN.Count)
                                        riadok_f[objekt.ConIN.Count] = ' ';
                                    else if (j == objekt.ConIN.Count + 1)
                                        riadok_f[objekt.ConIN.Count + 1] = '1';
                                    else
                                        riadok_f[j] = '-';
                                }
                                string riadok = new string(riadok_f);
                                subor.AppendLine(riadok);

                            }
                        }

                        //AND                        
                        if ((objekt.Type == "AND2" || objekt.Type == "AND3" || objekt.Type == "AND4" || objekt.Type == "AND5"
                            || objekt.Type == "AND6" || objekt.Type == "AND7" || objekt.Type == "AND8"))
                        {
                            ArrayList kopia_and = new ArrayList();
                            vstupy_and = objekt.ConIN;

                            for (int i = 0; i < vstupy_and.Count; i++)
                            {
                                kopia_and.Add(vstupy_and[i]);
                                if (vstupy_and[i].ToString().Contains("!"))
                                {
                                    vstupy_and[i] = vstupy_and[i].ToString().TrimStart('!');
                                }
                            }

                            subor.AppendLine(".names " + ArrayListToString(objekt.ConIN) + " " + ArrayListToString(objekt.ConOut));

                            char[] riadok_f = new char[objekt.ConIN.Count + 2];
                            for (int i = 0; i < objekt.ConIN.Count + 2; i++)
                            {
                                if (i != objekt.ConIN.Count && (i < objekt.ConIN.Count) && (kopia_and[i].ToString().Contains("!")))
                                    riadok_f[i] = '0';
                                else if (i != objekt.ConIN.Count && (i < objekt.ConIN.Count) && !(kopia_and[i].ToString().Contains("!")))
                                    riadok_f[i] = '1';
                                else if (i == objekt.ConIN.Count + 1)
                                    riadok_f[i] = '1';
                                else
                                    riadok_f[i] = ' ';
                            }

                            string riadok = new string(riadok_f);
                            subor.AppendLine(riadok);
                        }

                        //NOR
                        if ((objekt.Type == "NOR2" || objekt.Type == "NOR3" || objekt.Type == "NOR4" || objekt.Type == "NOR5"
                            || objekt.Type == "NOR6" || objekt.Type == "NOR7" || objekt.Type == "NOR8"))
                        {
                            subor.AppendLine(".names " + ArrayListToString(objekt.ConIN) + " " + ArrayListToString(objekt.ConOut));

                            char[] riadok_f = new char[objekt.ConIN.Count + 2];
                            for (int i = 0; i < objekt.ConIN.Count + 2; i++)
                            {
                                if (i != objekt.ConIN.Count && (i < objekt.ConIN.Count))
                                    riadok_f[i] = '0';
                                else if (i == objekt.ConIN.Count + 1)
                                    riadok_f[i] = '1';
                                else
                                    riadok_f[i] = ' ';
                            }

                            string riadok = new string(riadok_f);
                            subor.AppendLine(riadok);
                        }

                        //objekt je XOR
                        if (objekt.Type == "XOR")
                        {
                            subor.AppendLine(".names " + ArrayListToString(objekt.ConIN) + " " + ArrayListToString(objekt.ConOut));

                            for (int i = 0; i < objekt.ConIN.Count; i++)
                            {
                                char[] riadok_f = new char[objekt.ConIN.Count + 2];
                                for (int j = 0; j < objekt.ConIN.Count + 2; j++)
                                {
                                    if (j == i)
                                        riadok_f[j] = '0';
                                    else if (j == objekt.ConIN.Count)
                                        riadok_f[objekt.ConIN.Count] = ' ';
                                    else if (j == objekt.ConIN.Count + 1)
                                        riadok_f[objekt.ConIN.Count + 1] = '1';
                                    else
                                        riadok_f[j] = '1';
                                }
                                string riadok = new string(riadok_f);
                                subor.AppendLine(riadok);

                            }
                        }

                        //objekt je INV
                        if (objekt.Type == "INV")
                        {
                            ArrayList vstupy_inv = new ArrayList();
                            bool flag = false;
                            vstupy_inv = objekt.ConIN;

                            //true ak je vstup invertora medzi globalnymi vstupmi
                            foreach (string vstup_inv in vstupy_inv)
                            {
                                if (inputs.Contains(vstup_inv))
                                    flag = true;

                            }

                            //vytvor samostatny invertor ak nie je jeho vstup medzi globalnymi vstupmi alebo aj jeho vystup je globalny
                            if (flag == false || outputs.Contains(ArrayListToString(objekt.ConOut)))
                            {
                                subor.AppendLine(".names " + ArrayListToString(objekt.ConIN) + " " + ArrayListToString(objekt.ConOut));
                                char[] riadok_f = new char[objekt.ConIN.Count + 2];

                                for (int i = 0; i < objekt.ConIN.Count + 2; i++)
                                {
                                    if (i == objekt.ConIN.Count - 1)
                                        riadok_f[i] = '0';
                                    else if (i == objekt.ConIN.Count + 1)
                                        riadok_f[i] = '1';
                                    else
                                        riadok_f[i] = ' ';
                                }

                                string riadok = new string(riadok_f);
                                subor.AppendLine(riadok);
                            }
                        }
                    }
                    subor.AppendLine(".end");
                    //MessageBox.Show(subor.ToString());

                    //ulozenie do suboru
                    //SaveFileDialog dlg = new SaveFileDialog();
                    //dlg.Filter = "BLIF súbory (*.blif)|*.blif";
                    //dlg.Title = "Výstupný súbor";

                    //if (dlg.ShowDialog() == DialogResult.OK)
                    //{
                        StreamWriter sw = new FileInfo(Nazov).CreateText();
                        sw.Write(subor.ToString());
                        sw.Flush();
                        sw.Close();
                        sw = null;
                    //}
                }
            }

            catch (Exception es)
            {
                //return false;
                MessageBox.Show(es.Message);
            }
        }


        private void OpenBlif(String Nazov)
        {
            //try
           // {
           // 

                // uspesne otvorenie suboru
              
                    Logic_Designer.Form1.Cleanup();




                    System.IO.TextReader reader = File.OpenText(Nazov);
                    string line;                //premenna pre riadok v subore BLIF
                    Random rand = new Random(); //nahodna premenna
                    string nazov_modelu = null;        //meno modelu
                    string premenna_vystup = null;     //nazov vystupnej funkcie
                    int pocet_funkcii = 0;      //pocet funkcii v BLIF subore
                    int pocet_vstupov = 0;      //pocet vstupov v modeli
                    int pocet_vystupov = 0;      //pocet vystupov v modeli
                    int riadky_funkcie = 0;     //pocet riadkov jednej funkcie => pocet vstupov do OR
                    string[] premenne_funkcie;  //pole premennych vo fukcii
                    string[] vstupy_and;        //pole vstupov do AND
                    int cislo_and = 0;          //identifikator AND
                    int cislo_or = 0;           //identifikator OR
                    int cislo_inv = 0;          //identifikator INVERTORU                

                    //suradnice vstupov
                    int x_in = 100;
                    int y_in = 0;
                    //suradnice INVERTOROV => 0. stupen
                    int x_inv = 300;
                    int y_inv = 0;
                    //suradnice AND-ov => 1. stupen
                    int x1 = 400;
                    int y1 = 0;
                    //suradnice OR-ov => 2. stupen
                    int x2 = 500;
                    int y2 = 0;
                    //suradnice vystupov
                    int x_out = 600;
                    int y_out = 0;

                    //******************
                    int flag = 0;
                    int pocet_vstupov_AND_hradla = 0;
                    int pocet_vstupov_OR_hradla = 0;

                    //*******************

                    // zoznam invertorov
                    ArrayList invertory = new ArrayList();

                    // zoznam vstupov
                    ArrayList inputs = new ArrayList();

                    // zoznam vystupov

                    ArrayList outputs = new ArrayList();

                    // zoznam vstupov do AND-u
                    ArrayList and_inputs = new ArrayList();

                    // zoznam vstupov do AND-u
                    ArrayList or_inputs = new ArrayList();

                    // zoznam priamych premennych vo funkcii
                    ArrayList premenne_f = new ArrayList();

                    //********************
                    ArrayList funkcia = new ArrayList();
                    ArrayList vsetky_f = new ArrayList();

                    //********************

                    //citame subor po riadkoch
                    while ((line = reader.ReadLine()) != null)
                    {
                        //nahradine viac medzier a taby len jednou medzerou
                        line = Regex.Replace(line, @"\s+", " ");
                        line = Regex.Replace(line, @"\t+", " ");
                        line = line.Trim();

                        // prvky v danom riadku dane do pola
                        string[] items = line.Split(' ');

                        foreach (string item in items)
                        {
                            // NAZOV MODELU
                            if (item.StartsWith(".model"))
                            {
                                nazov_modelu = items[1];
                               Logic_Designer.Form1.SetModelName(nazov_modelu);
                            }

                            // VSTUPY
                            if ((item.StartsWith(".inputs")))
                            {
                                pocet_vstupov = 0;
                                foreach (string item2 in items)
                                {
                                    pocet_vstupov++;
                                }

                                // zaratalo aj ,,.inputs" => treba odratat z poctu
                                pocet_vstupov = pocet_vstupov - 1;

                                for (int i = 1; i <= pocet_vstupov; i++)
                                {
                                    inputs.Add(items[i]);
                                    Logic_Designer.Form1.CreateNode(x_in, y_in = y_in + 100, i, "IN" + i.ToString(), "IN", null, inputs[i - 1].ToString());
                                }
                            }

                            // VYSTUPY
                            if ((item.StartsWith(".outputs")))
                            {
                                pocet_vystupov = 0;
                                foreach (string item2 in items)
                                {
                                    pocet_vystupov++;
                                }

                                // zaratalo aj ,,.outputs" => treba odratat z poctu
                                pocet_vystupov = pocet_vystupov - 1;

                                for (int i = 1; i <= pocet_vystupov; i++)
                                {
                                    outputs.Add(items[i]);
                                    string[] str = new string[1];
                                    str[0] = items[i];
                                    Logic_Designer.Form1.CreateNode(x_out, y_out = y_out + 100, i, "OUT" + i.ToString(), "OUT", str, "");
                                }
                            }

                            // FUNKCIE MODELU
                            if ((item.StartsWith(".names")))
                            {
                                //****************************
                                if (pocet_funkcii > 0)
                                {
                                    vsetky_f.Add(ArrayListToString(funkcia));
                                    //MessageBox.Show(pocet_funkcii.ToString());
                                    funkcia.Clear();
                                }

                                if ((flag == 1) && (and_inputs.Count >= 2))
                                {
                                    vstupy_and = (string[])and_inputs.ToArray(typeof(string));
                                    Logic_Designer.Form1.CreateNode(x1, y1 = y1 + 100, cislo_and, "AND" + cislo_and.ToString(), "AND" + pocet_vstupov_AND_hradla.ToString(), vstupy_and, premenna_vystup);
                                    //MessageBox.Show("NAMES");
                                    flag = 0;
                                    riadky_funkcie = 0;
                                    or_inputs.Clear();
                                }
                                //***************************

                                pocet_funkcii++;
                                if (riadky_funkcie > 1)
                                {
                                    //Potrebujeme dalsi OR
                                    cislo_or++;
                                    string[] vstup_str = (string[])or_inputs.ToArray(typeof(string));
                                    //MessageBox.Show("OR" + cislo_or.ToString());
                                    Logic_Designer.Form1.CreateNode(x2, y2 = y2 + 100, cislo_or, "OR" + cislo_or.ToString(), "OR" + riadky_funkcie.ToString(), vstup_str, premenna_vystup);
                                    riadky_funkcie = 0;
                                    or_inputs.Clear();
                                }

                                int premenne = 0;

                                foreach (string item2 in items)
                                {
                                    premenne++;
                                }

                                //zaratalo aj .names => dekrementujem
                                premenne = premenne - 1;

                                if (pocet_funkcii >= 1)
                                {
                                    premenne_f.Clear();

                                    for (int i = 1; i <= premenne - 1; i++)
                                    {
                                        if (pocet_funkcii >= 1)
                                        {
                                            premenne_f.Add(items[i]);
                                        }
                                    }

                                    premenna_vystup = items[premenne];
                                }
                            }
                        }

                        if (((items[0].StartsWith("0")) || (items[0].StartsWith("1")) || (items[0].StartsWith("-"))) && (items[0].Length > 1))
                        {
                            //pocet vstupv do OR-u
                            riadky_funkcie++;

                            if ((flag == 1) && (riadky_funkcie > 1) && (and_inputs.Count >= 2))
                            {
                                vstupy_and = (string[])and_inputs.ToArray(typeof(string));
                                flag = 0;
                                Logic_Designer.Form1.CreateNode(x1, y1 = y1 + 100, cislo_and, "AND" + cislo_and.ToString(), "AND" + pocet_vstupov_AND_hradla.ToString(), vstupy_and, "and_" + cislo_and.ToString() + "_out");
                            }

                            //vymazanie zoznamu vstupov do AND-u
                            and_inputs.Clear();

                            //len pretypovanie
                            premenne_funkcie = (string[])premenne_f.ToArray(typeof(string));

                            char[] hodnoty_funkcie = items[0].ToCharArray();
                            pocet_vstupov_AND_hradla = 0;
                            pocet_vstupov_OR_hradla = 0;

                            //pocet vstupnych premennych funkcie
                            int pocet_premennych = items[0].Length;

                            if (riadky_funkcie == 1)
                            {
                                flag = 1;

                                funkcia.Add(premenna_vystup + " = ");

                                // Vystupna premenna je v priamom stave
                                if (items[1] == "1")
                                {
                                    for (int i = 0; i < pocet_premennych; i++)
                                    {


                                        if (((hodnoty_funkcie[i].ToString()) == "0") && !(pocet_premennych == 1))
                                        {
                                            and_inputs.Add("!" + premenne_funkcie[i].ToString());
                                            funkcia.Add("!" + premenne_funkcie[i].ToString() + ".");

                                            if (invertory.Contains("!" + premenne_funkcie[i].ToString()))
                                            {
                                            }
                                            else
                                            {
                                                invertory.Add("!" + premenne_funkcie[i].ToString());
                                                string[] vstup_inv = new string[1];
                                                vstup_inv[0] = (premenne_funkcie[i].ToString());
                                                cislo_inv++;
                                                Logic_Designer.Form1.CreateNode(x_inv, y_inv = y_inv + 100, cislo_inv, "INV" + cislo_inv.ToString(), "INV", vstup_inv, "!" + premenne_funkcie[i].ToString());
                                            }

                                            pocet_vstupov_AND_hradla++;
                                        }

                                        if (((hodnoty_funkcie[i].ToString()) == "0") && (outputs.Contains(premenna_vystup)) && (pocet_premennych == 1))
                                        {
                                            //and_inputs.Add("!" + premenne_funkcie[i].ToString());
                                            funkcia.Add("!" + premenne_funkcie[i].ToString() + ".");

                                            if (invertory.Contains("!" + premenne_funkcie[i].ToString()))
                                            {
                                            }
                                            else
                                            {
                                                invertory.Add("!" + premenne_funkcie[i].ToString());
                                                string[] vstup_inv = new string[1];
                                                vstup_inv[0] = (premenne_funkcie[i].ToString());
                                                cislo_inv++;
                                                Logic_Designer.Form1.CreateNode(x_inv, y_inv = y_inv + 100, cislo_inv, "INV" + cislo_inv.ToString(), "INV", vstup_inv, premenna_vystup);
                                            }

                                            //pocet_vstupov_AND_hradla++;
                                        }

                                        if (hodnoty_funkcie[i].ToString() == "1")
                                        {
                                            and_inputs.Add(premenne_funkcie[i].ToString());
                                            funkcia.Add(premenne_funkcie[i].ToString() + ".");
                                            pocet_vstupov_AND_hradla++;
                                        }

                                        if (hodnoty_funkcie[i].ToString() == "-")
                                        {
                                        }
                                    }

                                    if (and_inputs.Count == 1)
                                    {
                                        or_inputs.Add(and_inputs[0].ToString());
                                    }

                                    if (and_inputs.Count >= 2)
                                    {
                                        cislo_and++;
                                        or_inputs.Add("and_" + cislo_and.ToString() + "_out");
                                        vstupy_and = (string[])and_inputs.ToArray(typeof(string));
                                    }

                                }

                                // Vystupna premenna je v negovanom stave

                                if (items[1] == "0")
                                {
                                    MessageBox.Show("Nekorektný BLIF súbor - výstupna funkcia musí byť jednotková" + "\nNa prevod do korektného stavu použite prosím nástroj KONVERZIA SÚBOROV" + "\nZdrojový aj cieľový formát vyberte BLIF");
                                    Logic_Designer.Form1.Cleanup();
                                    reader.Close();
                                    reader = null;
                                }
                            }

                            if (riadky_funkcie > 1)
                            {

                                funkcia.Add(" + ");

                                for (int i = 0; i < pocet_premennych; i++)
                                {
                                    if ((hodnoty_funkcie[i].ToString()) == "0" && !(pocet_premennych == 1))
                                    {
                                        and_inputs.Add("!" + premenne_funkcie[i].ToString());
                                        funkcia.Add("!" + premenne_funkcie[i].ToString() + ".");

                                        if (invertory.Contains("!" + premenne_funkcie[i].ToString()))
                                        {
                                        }
                                        else
                                        {
                                            invertory.Add("!" + premenne_funkcie[i].ToString());
                                            string[] vstup_inv = new string[1];
                                            vstup_inv[0] = (premenne_funkcie[i].ToString());
                                            cislo_inv++;
                                            Logic_Designer.Form1.CreateNode(x_inv, y_inv = y_inv + 100, cislo_inv, "INV" + cislo_inv.ToString(), "INV", vstup_inv, "!" + premenne_funkcie[i].ToString());
                                        }
                                        pocet_vstupov_AND_hradla++;
                                    }

                                    if (((hodnoty_funkcie[i].ToString()) == "0") && (outputs.Contains(premenna_vystup)) && (pocet_premennych == 1))
                                    {
                                        //and_inputs.Add("!" + premenne_funkcie[i].ToString());
                                        funkcia.Add("!" + premenne_funkcie[i].ToString() + ".");

                                        if (invertory.Contains("!" + premenne_funkcie[i].ToString()))
                                        {
                                        }
                                        else
                                        {
                                            invertory.Add("!" + premenne_funkcie[i].ToString());
                                            string[] vstup_inv = new string[1];
                                            vstup_inv[0] = (premenne_funkcie[i].ToString());
                                            cislo_inv++;
                                            Logic_Designer.Form1.CreateNode(x_inv, y_inv = y_inv + 100, cislo_inv, "INV" + cislo_inv.ToString(), "INV", vstup_inv, premenna_vystup);
                                        }

                                        //pocet_vstupov_AND_hradla++;
                                    }

                                    if (hodnoty_funkcie[i].ToString() == "1")
                                    {
                                        and_inputs.Add(premenne_funkcie[i].ToString());
                                        funkcia.Add(premenne_funkcie[i].ToString() + ".");
                                        pocet_vstupov_AND_hradla++;
                                    }

                                    if (hodnoty_funkcie[i].ToString() == "-")
                                    {
                                    }
                                }

                                if (and_inputs.Count == 1)
                                {
                                    or_inputs.Add(and_inputs[0].ToString());
                                }

                                if (and_inputs.Count >= 2)
                                {
                                    cislo_and++;
                                    or_inputs.Add("and_" + cislo_and.ToString() + "_out");
                                    vstupy_and = (string[])and_inputs.ToArray(typeof(string));
                                    Logic_Designer.Form1.CreateNode(x1, y1 = y1 + 100, cislo_and, "AND" + cislo_and.ToString(), "AND" + pocet_vstupov_AND_hradla.ToString(), vstupy_and, "and_" + cislo_and.ToString() + "_out");
                                }

                            }
                        }

                        if (((items[0].StartsWith("0")) || (items[0].StartsWith("1")) || (items[0].StartsWith("-"))) && (items[0].Length == 1))
                        {
                            premenne_funkcie = (string[])premenne_f.ToArray(typeof(string));
                            char[] hodnoty_funkcie = items[0].ToCharArray();

                            if ((hodnoty_funkcie[0].ToString()) == "0")
                            {
                                if (invertory.Contains(premenne_funkcie[0].ToString()))
                                {
                                }
                                else
                                {
                                    invertory.Add(premenne_funkcie[0].ToString());
                                    string[] vstup_inv = new string[1];
                                    vstup_inv[0] = (premenne_funkcie[0].ToString());
                                    cislo_inv++;
                                    Logic_Designer.Form1.CreateNode(x_inv, y_inv = y_inv + 100, cislo_inv, "INV" + cislo_inv.ToString(), "INV", vstup_inv, premenna_vystup);
                                }
                            }
                        }

                        if ((line.StartsWith(".end")))
                        {

                            if ((flag == 1) && (and_inputs.Count >= 2))
                            {
                                //MessageBox.Show("END");
                                vstupy_and = (string[])and_inputs.ToArray(typeof(string));

                                Logic_Designer.Form1.CreateNode(x1, y1 = y1 + 100, cislo_and, "AND" + cislo_and.ToString(), "AND" + pocet_vstupov_AND_hradla.ToString(), vstupy_and, premenna_vystup);
                                flag = 0;
                                riadky_funkcie = 0;
                                or_inputs.Clear();

                            }

                            if (riadky_funkcie > 1)
                            {
                                cislo_or++;
                                //MessageBox.Show("OR" + cislo_or.ToString());                            
                                string[] vstup_str = (string[])or_inputs.ToArray(typeof(string));

                                Logic_Designer.Form1.CreateNode(x2, y2 = y2 + 100, cislo_or, "OR" + cislo_or.ToString(), "OR" + riadky_funkcie.ToString(), vstup_str, premenna_vystup);
                                or_inputs.Clear();


                            }
                            //MessageBox.Show("BLIF OK");
                            //MessageBox.Show(ArrayListToString(funkcia));


                            vsetky_f.Add(ArrayListToString(funkcia));
                            /*for (int i = 0; i < vsetky_f.Count; i++)
                            {
                                MessageBox.Show(vsetky_f[i].ToString());
                            }*/

                            funkcia.Clear();
                            vsetky_f.Clear();
                            riadky_funkcie = 0;
                        }
                    }

                    reader.Close();
                    reader = null;
                
                Logic_Designer.Form1.MakeCons();
                Logic_Designer.Form1.UlozUzly("obvod_tmp.z5");
           // }


            //catch (Exception es)
           // {
            //    //return false;
            //    MessageBox.Show(es.Message);
           // }
        }

        private void butSaveFile_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            if (System.IO.File.Exists(cdbOpen.FileName))
            {
                cdbSave.FileName = cdbOpen.FileName;
            }

            if (cdbSave.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new FileInfo(cdbSave.FileName).CreateText();
                sw.Write(this.txtFile.Text);
                sw.Flush();
                sw.Close();
                sw = null;
            }
        }

        private void ctlMain_Load(object sender, System.EventArgs e)
        {

        }

        private void txtVstupy_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

