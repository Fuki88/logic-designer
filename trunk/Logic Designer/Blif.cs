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
        Form1 host = new Form1();
       
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
			
            */
            get { return myHost; }
            set { myHost = value; }
        }

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
                        if ((objekt.Name == "OR2" || objekt.Name == "OR3"))
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
                        if ((objekt.Name == "AND2" || objekt.Name == "AND3"))
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
    }
}

