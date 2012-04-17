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
namespace Logic_Designer
{
    public class Blif //: System.Windows.Forms.UserControl, IPlugin
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
        //Form1 myHost;
        //Form1 host = new Form1();
       
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
      /*  public IPluginHost Host
        {
            //This part is currently not really implemented
            /*
            Here's the scoop though... You can make the host program
            Implement this interface... this essentially gives you the ability
            to allow plugins to access some functionality of the host program.
			
            Example: an mp3 player.  If you had the IPluginHost interface as like:
			
            public interface IPluginHost
            {
                void Play(string FileName);
                void Stop();			
            }
			
            what you would do is when the plugin is loaded in the host (this would be
            in like the PluginServices.cs file in the AddPlugin() method) you would 
            set:
                newPlugin.Host = this;
				
            this would give the plugin a reference to the host... now since the plugin
            knows the host contains these methods, it can easily access them just like:
			
                this.Host.Play("C:\MyCoolMp3.mp3");
				
            and then they could go:
			
                this.Host.Stop();
				
            all this being from right inside the plugin!  I hope you get the point.  It 
            just means that you can indeed make your plugins able to interact with the 
            host program itself.  Let's face it.. It would be no fun if you couldn't do this,
            because otherwise all the plugin is, is just standalone functionality running
            inside the host program.. (of course there are cases when you can still accomplish
            many things without needing to allow the plugin to play with the host... for example
            you could have an spam filter, and have each plugin be a different filter... that would
            be pretty simple to make plugins for...
			
            so anyhow, that is what the host thingy is all aboot, eh!	
			
            
            get { return myHost; }
            set { myHost = value; }
        }
*/
        public string PlugName
        {
            get { return myName; }
        }

        /*public System.Windows.Forms.UserControl MainInterface
        {
            get { return this; }
        }
        */
        public string Version
        {
            get { return myVersion; }
        }



        /*public void Dispose()
        {
            //Put any cleanup code in here for when the program is stopped
        }*/

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /*protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                menu1.Dispose();
                menu.Dispose();
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }*/


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
        // ARRAYLIST TO STRING
        public void svblf(string nazov)
        {
            saveBlif(nazov);
        }
        //SPRACOVANIE OBVODU DO SUBORU
        public void saveBlif(string nazov) //object sender, EventArgs e
        {
            Stream stream = null;
            SavedNode uzol = null;
            StringBuilder subor = new StringBuilder();
            string[] pomocna;
            string pomoc;
            ArrayList inputs = new ArrayList();
            ArrayList outputs = new ArrayList();
            ArrayList vstupy_f = new ArrayList();
            ArrayList vstupy_and = new ArrayList();

            Form1.UlozUzly();
            int pocet_obj = 0;
            try
            {
                stream = File.Open("obvod_tmp.z5", FileMode.Open);
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
                    //List<SavedNode> kopia = new List<SavedNode>();
                    //kopia = objekty;
                    //STREAM STOP

                    /*foreach (SavedNode objekt in objekty)
                    {
                        MessageBox.Show(objekt.Name);
                    }*/

                    //SUBOR START
                    //nazov modelu
                    subor.AppendLine(".model model1");

                    //vstupy
                    foreach (SavedNode objekt in objekty)
                    {
                        if (objekt.Name == "IN")
                        {
                            inputs.Add(ArrayListToString(objekt.ConOut));
                        }
                    }
                    subor.AppendLine(".inputs " + ArrayListToString(inputs));
                    //MessageBox.Show(subor.ToString());

                    //vystupy
                    foreach (SavedNode objekt in objekty)
                    {
                        if (objekt.Name == "OUT")
                        {
                            outputs.Add(ArrayListToString(objekt.ConIN));
                        }
                    }
                    subor.AppendLine(".outputs " + ArrayListToString(outputs));
                    //MessageBox.Show(subor.ToString());

                    //funkcie
                    foreach (SavedNode objekt in objekty)
                    {
                        //vystup objektu je globalny vystup => obvod pre dany vystup a je to OR
                        //if (outputs.Contains(ArrayListToString(objekt.ConOut)) && (objekt.Name != "OUT") && (objekt.Name == "OR2" || objekt.Name == "OR3"))
                        if ((objekt.Name == "OR2" || objekt.Name == "OR3" || objekt.Name == "OR4" || objekt.Name == "OR5" || objekt.Name == "OR6" || objekt.Name == "OR7"))
                        {
                            //MessageBox.Show(objekt.Name);                            
                            subor.AppendLine(".names " + ArrayListToString(objekt.ConIN) + " " + ArrayListToString(objekt.ConOut));
                            //MessageBox.Show(subor.ToString());
                            //pre pocet riadkov funkcie
                            for (int i = 0; i < objekt.ConIN.Count; i++)
                            {
                                //MessageBox.Show(objekt.ConIN.Count.ToString());
                                char[] riadok_f = new char[objekt.ConIN.Count + 2];
                                for (int j = 0; j < objekt.ConIN.Count + 2; j++)
                                {
                                    if (j == i)
                                        riadok_f[j] = '1';
                                    else if (j == objekt.ConIN.Count)
                                        riadok_f[objekt.ConIN.Count] = ' ';
                                    else if (j == objekt.ConIN.Count + 1)
                                        riadok_f[objekt.ConIN.Count + 1] = '1';
                                    else
                                        riadok_f[j] = '-';
                                }
                                string riadok = new string(riadok_f);
                                //MessageBox.Show(riadok);
                                subor.AppendLine(riadok);

                            }
                            //MessageBox.Show(subor.ToString());
                        }

                        ArrayList kopia_and = new ArrayList();

                        //vystup objektu je globalny vystup => obvod pre dany vystup a je to AND
                        //if (outputs.Contains(ArrayListToString(objekt.ConOut)) && (objekt.Name != "OUT") && (objekt.Name == "AND2" || objekt.Name == "AND3"))
                        if ((objekt.Name == "AND2" || objekt.Name == "AND3" || objekt.Name == "AND4" || objekt.Name == "AND5" || objekt.Name == "AND6" || objekt.Name == "AND7"))
                        {
                            //MessageBox.Show(objekt.Name);
                            //char[] arr = new char[] {'!'};
                            vstupy_and = objekt.ConIN;

                            for (int i = 0; i < vstupy_and.Count; i++)
                            {
                                kopia_and.Add(vstupy_and[i]);
                                if (vstupy_and[i].ToString().Contains("!"))
                                {
                                    vstupy_and[i] = vstupy_and[i].ToString().TrimStart('!');
                                }
                            }

                            //MessageBox.Show(ArrayListToString(vstupy_and));
                            //MessageBox.Show(ArrayListToString(kopia_and));
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
                            //MessageBox.Show(riadok);
                            subor.AppendLine(riadok);

                            //}
                            //MessageBox.Show(subor.ToString());
                        }


                    }
                    subor.AppendLine(".end");
                    MessageBox.Show(subor.ToString());

                    //ulozenie do suboru
                    /* SaveFileDialog dlg = new SaveFileDialog();
                     dlg.Filter = "BLIF súbory (*.blif)|*.blif";
                     dlg.Title = "Výstupný súbor";
                     */
                    //if (dlg.ShowDialog() == DialogResult.OK)
                    //{                        
                    StreamWriter sw = new FileInfo(nazov).CreateText();
                    sw.Write(subor.ToString());
                    sw.Flush();
                    sw.Close();
                    sw = null;
                    // }
                }
            }

            catch (Exception es)
            {
                //return false;
                MessageBox.Show(es.Message);
            }
        }


        private void NacitajUzly()
        {

            //Nastavenie filtra suborov
            cdbOpen.Filter = "BLIF súbory (*.blif)|*.blif";
            cdbOpen.Title = "Typ súboru";

            // uspesne otvorenie suboru
            if (cdbOpen.ShowDialog() == DialogResult.OK)
            {
               
                Form1.Cleanup();
                this.txtFile.Clear();
                this.txtVstupy.Clear();
                this.textBoxObvod.Clear();


                System.IO.TextReader tr = File.OpenText(cdbOpen.FileName);
                this.txtFile.Text = tr.ReadToEnd();
                tr.Close();
                tr = null;


                System.IO.TextReader reader = File.OpenText(cdbOpen.FileName);
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
                                Form1.CreateNode(x_in, y_in = y_in + 100, i, "IN" + i.ToString(), "IN", null, inputs[i - 1].ToString());
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
                                Form1.CreateNode(x_out, y_out = y_out + 100, i, "OUT" + i.ToString(), "OUT", str, "");
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
                                Form1.CreateNode(x1, y1 = y1 + 100, cislo_and, "AND" + cislo_and.ToString(), "AND" + pocet_vstupov_AND_hradla.ToString(), vstupy_and, premenna_vystup);
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
                                Form1.CreateNode(x2, y2 = y2 + 100, cislo_or, "OR" + cislo_or.ToString(), "OR" + riadky_funkcie.ToString(), vstup_str, premenna_vystup);
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

                    if ((items[0].StartsWith("0")) || (items[0].StartsWith("1")) || (items[0].StartsWith("-")))
                    {
                        //pocet vstupv do OR-u
                        riadky_funkcie++;

                        if ((flag == 1) && (riadky_funkcie > 1) && (and_inputs.Count >= 2))
                        {
                            //*******************************
                            //MessageBox.Show("andXout");
                            vstupy_and = (string[])and_inputs.ToArray(typeof(string));
                            flag = 0;
                            Form1.CreateNode(x1, y1 = y1 + 100, cislo_and, "AND" + cislo_and.ToString(), "AND" + pocet_vstupov_AND_hradla.ToString(), vstupy_and, "and_" + cislo_and.ToString() + "_out");

                            //******************************************
                        }

                        //vymazanie zoznamu vstupov do AND-u
                        and_inputs.Clear();

                        //len pretypovanie
                        premenne_funkcie = (string[])premenne_f.ToArray(typeof(string));

                        char[] hodnoty_funkcie = items[0].ToCharArray();
                        //********************************
                        pocet_vstupov_AND_hradla = 0;
                        pocet_vstupov_OR_hradla = 0;
                        //*********************************

                        //pocet vstupnych premennych funkcie
                        int pocet_premennych = items[0].Length;

                        if (riadky_funkcie == 1)
                        {
                            //*************
                            flag = 1;

                            funkcia.Add(premenna_vystup + " = ");
                            //MessageBox.Show(ArrayListToString(funkcia));
                            //*************

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
                                            Form1.CreateNode(x_inv, y_inv = y_inv + 100, cislo_inv, "INV" + cislo_inv.ToString(), "INV", vstup_inv, "!" + premenne_funkcie[i].ToString());
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
                                            Form1.CreateNode(x_inv, y_inv = y_inv + 100, cislo_inv, "INV" + cislo_inv.ToString(), "INV", vstup_inv, premenna_vystup);
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

                                //myHost.CreateNode(x1, y1 = y1 + 100, cislo_and, "AND" + cislo_and.ToString(), "AND" + pocet_vstupov_AND_hradla.ToString(), vstupy_and, "and_" + cislo_and.ToString() + "_out");
                                //myHost.CreateNode(x1, y1 = y1 + 100, cislo_and, "AND" + cislo_and.ToString(), "AND" + pocet_vstupov_AND_hradla.ToString(), vstupy_and, premenna_vystup);

                            }

                            // Vystupna premenna je v negovanom stave
                            /*if (items[1] == "0")
                            {
                                for (int i = 0; i < pocet_premennych; i++)
                                {
                                    if ((hodnoty_funkcie[i].ToString()) == "0")
                                    {
                                        and_inputs.Add("!" + premenne_funkcie[i].ToString());

                                        if (invertory.Contains("!" + premenne_funkcie[i].ToString()))
                                        {

                                        }
                                        else
                                        {
                                            invertory.Add("!" + premenne_funkcie[i].ToString());
                                            string[] vstup_inv = new string[1];
                                            vstup_inv[0] = (premenne_funkcie[i].ToString());
                                            cislo_inv++;
                                            myHost.CreateNode(x_inv, y_inv = y_inv + 100, cislo_inv, "INV" + cislo_inv.ToString(), "INV", vstup_inv, "!" + premenne_funkcie[i].ToString());
                                        }
                                        pocet_vstupov_AND_hradla++;
                                    }

                                    if (hodnoty_funkcie[i].ToString() == "1")
                                    {
                                        and_inputs.Add(premenne_funkcie[i].ToString());
                                        pocet_vstupov_AND_hradla++;
                                    }

                                    if (hodnoty_funkcie[i].ToString() == "-")
                                    {
                                    }
                                }
                                cislo_and++;
                                vstupy_and = (string[])and_inputs.ToArray(typeof(string));

                                myHost.CreateNode(x1, y1 = y1 + 100, cislo_and, "AND" + cislo_and.ToString(), "AND" + pocet_vstupov_AND_hradla.ToString(), vstupy_and, "and_" + cislo_and.ToString() + "_out");

                            }*/
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
                                        Form1.CreateNode(x_inv, y_inv = y_inv + 100, cislo_inv, "INV" + cislo_inv.ToString(), "INV", vstup_inv, "!" + premenne_funkcie[i].ToString());
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
                                        Form1.CreateNode(x_inv, y_inv = y_inv + 100, cislo_inv, "INV" + cislo_inv.ToString(), "INV", vstup_inv, premenna_vystup);
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
                                Form1.CreateNode(x1, y1 = y1 + 100, cislo_and, "AND" + cislo_and.ToString(), "AND" + pocet_vstupov_AND_hradla.ToString(), vstupy_and, "and_" + cislo_and.ToString() + "_out");
                            }

                        }
                    }

                    if ((line.StartsWith(".end")))
                    {
                        //**************************************
                        if ((flag == 1) && (and_inputs.Count >= 2))
                        {
                            //MessageBox.Show("END");
                            vstupy_and = (string[])and_inputs.ToArray(typeof(string));

                            Form1.CreateNode(x1, y1 = y1 + 100, cislo_and, "AND" + cislo_and.ToString(), "AND" + pocet_vstupov_AND_hradla.ToString(), vstupy_and, premenna_vystup);
                            flag = 0;
                            riadky_funkcie = 0;
                            or_inputs.Clear();

                        }
                        //**************************************88

                        if (riadky_funkcie > 1)
                        {
                            cislo_or++;
                            //MessageBox.Show("OR" + cislo_or.ToString());                            
                            string[] vstup_str = (string[])or_inputs.ToArray(typeof(string));

                            Form1.CreateNode(x2, y2 = y2 + 100, cislo_or, "OR" + cislo_or.ToString(), "OR" + riadky_funkcie.ToString(), vstup_str, premenna_vystup);
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
                //Obvod circuit = new Obvod(nazov_modelu, pocet_vstupov, inputs);

                reader.Close();
                reader = null;


            }


            Form1.MakeCons();
            Form1.UlozUzly();
        }

    }
}

