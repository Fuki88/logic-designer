using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Logic_Designer.verifikacia
{
    public partial class verifikacia : UserControl
    {
        public verifikacia()
        {
            InitializeComponent();
        }

        //sem sa ulozia vsetky uzly a spojenia - uchovaju hodnotu
        public ArrayList Gates = new ArrayList();
        public ArrayList Connections = new ArrayList();

        //reprezentuje logicke hradlo vo vseobecnosti 
        public class Gate
        {
            public bool[] InPorts;
            public bool[] OutPorts;
            public int ID;
            public string OutConnection; //nazov out connection
            public int set; //hovori, ze kolko portov uz bolo nastavenych
            public bool root; //pri iteracii simulacie sa zacina od tohto obvodu
            public string type;
            public int InConnection; //pocet zapojenych vstupov

            //nastavit hotnotu na porte (ak sa nenastavi tak vrati false)
            public bool setPortValue(int portNum, bool value)
            {
                bool result = false;
                if (portNum < InPorts.Length)
                {
                    InPorts[portNum] = value;
                    result = true;
                }
                return result;
            }

            //prehodi hotnotu na porte (ak sa nenastavi tak vrati false)
            public bool togglePortValue(int portNum)
            {
                bool result = false;
                if (portNum < InPorts.Length)
                {
                    if (InPorts[portNum] == true)
                    {
                        InPorts[portNum] = false;
                    }
                    else
                    {
                        InPorts[portNum] = true;
                    }
                    result = true;
                }
                return result;
            }

            //funkcia hradla - pre kazde hradlo ina
            public virtual bool function()
            {
                bool output = false;
                return output;
            }
        }

        //hradlo AND
        class AND : Gate
        {
            //konstruktor - pri vytvoreni instancie sa nastavia hodnoty vsetkych portov na 1
            public AND(int InPortsNum, int ID)
            {
                this.ID = ID;
                set = 0;
                root = false;
                type = "AND";
                InConnection = 0;

                InPorts = new bool[InPortsNum];
                for (int i = 0; i < InPorts.Length; i++)
                {
                    InPorts[i] = true;
                }

                OutPorts = new bool[1];
                OutPorts[0] = true;
            }

            //samotna funkcia and
            public override bool function()
            {
                OutPorts[0] = true;
                for (int i = 0; i < InPorts.Length; i++)
                {
                    if (InPorts[i] != true)
                    {
                        OutPorts[0] = false;
                    }
                }
                return OutPorts[0];
            }
        }

        //hradlo NAND
        class NAND : Gate
        {
            //konstruktor - pri vytvoreni instancie sa nastavia hodnoty vsetkych portov na 1
            public NAND(int InPortsNum, int ID)
            {
                this.ID = ID;
                set = 0;
                root = false;
                type = "NAND";
                InConnection = 0;

                InPorts = new bool[InPortsNum];
                for (int i = 0; i < InPorts.Length; i++)
                {
                    InPorts[i] = true;
                }

                OutPorts = new bool[1];
                OutPorts[0] = false;
            }

            //samotna funkcia nand
            public override bool function()
            {
                OutPorts[0] = false;
                for (int i = 0; i < InPorts.Length; i++)
                {
                    if (InPorts[i] == false)
                    {
                        OutPorts[0] = true;
                    }
                }
                return OutPorts[0];
            }
        }

        //hradlo OR
        class OR : Gate
        {
            //konstruktor - pri vytvoreni instancie sa nastavia hodnoty vsetkych portov na 0
            public OR(int InPortsNum, int ID)
            {
                this.ID = ID;
                set = 0;
                root = false;
                type = "OR";
                InConnection = 0;

                InPorts = new bool[InPortsNum];
                for (int i = 0; i < InPorts.Length; i++)
                {
                    InPorts[i] = false;
                }

                OutPorts = new bool[1];
                OutPorts[0] = false;
            }

            //samotna funkcia or
            public override bool function()
            {
                OutPorts[0] = false;
                for (int i = 0; i < InPorts.Length; i++)
                {
                    if (InPorts[i] == true)
                    {
                        OutPorts[0] = true;
                    }
                }
                return OutPorts[0];
            }
        }

        //hradlo NOR
        class NOR : Gate
        {
            //konstruktor - pri vytvoreni instancie sa nastavia hodnoty vsetkych portov na 0
            public NOR(int InPortsNum, int ID)
            {
                this.ID = ID;
                set = 0;
                root = false;
                type = "NOR";
                InConnection = 0;

                InPorts = new bool[InPortsNum];
                for (int i = 0; i < InPorts.Length; i++)
                {
                    InPorts[i] = false;
                }

                OutPorts = new bool[1];
                OutPorts[0] = true;
            }

            //samotna funkcia nor
            public override bool function()
            {
                OutPorts[0] = true;
                for (int i = 0; i < InPorts.Length; i++)
                {
                    if (InPorts[i] == true)
                    {
                        OutPorts[0] = false;
                    }
                }
                return OutPorts[0];
            }
        }

        //hradlo XOR
        class XOR : Gate
        {
            //konstruktor - pri vytvoreni instancie sa nastavia hodnoty vsetkych portov na 0
            public XOR(int ID)
            {
                this.ID = ID;
                set = 0;
                root = false;
                type = "XOR";
                InConnection = 0;

                InPorts = new bool[2];
                InPorts[0] = false;
                InPorts[1] = false;

                OutPorts = new bool[1];
                OutPorts[0] = true;
            }

            //samotna funkcia xor
            public override bool function()
            {
                if (InPorts[0] == InPorts[1])
                {
                    OutPorts[0] = false;
                }
                else
                {
                    OutPorts[0] = true;
                }
                return OutPorts[0];
            }
        }

        //hradlo NOT
        class NOT : Gate
        {
            //konstruktor - pri vytvoreni instancie sa nastavia hodnoty vsetkych portov na 0
            public NOT(int ID)
            {
                this.ID = ID;
                set = 0;
                root = false;
                type = "NOT";
                InConnection = 0;

                InPorts = new bool[1];
                InPorts[0] = false;

                OutPorts = new bool[1];
                OutPorts[0] = true;
            }

            //samotna funkcia not
            public override bool function()
            {
                if (InPorts[0] == true)
                {
                    OutPorts[0] = false;
                }
                else
                {
                    OutPorts[0] = true;
                }
                return OutPorts[0];
            }
        }

        //vstup obvodu
        class IN : Gate
        {
            public IN(int ID)
            {
                this.ID = ID;
                set = 0;
                root = true;
                type = "IN";
                InConnection = 0;

                InPorts = new bool[1];
                InPorts[0] = false;

                OutPorts = new bool[1];
                OutPorts[0] = false;
            }

            public override bool function()
            {
                if (InPorts[0] == true)
                {
                    OutPorts[0] = true;
                }
                else
                {
                    OutPorts[0] = false;
                }
                return OutPorts[0];
            }

            //prehodi hotnotu na porte (ak sa nenastavi tak vrati false)
            public void toggleOutPortValue()
            {
                if (InPorts[0] == true)
                {
                    OutPorts[0] = false;
                    InPorts[0] = false;
                }
                else
                {
                    OutPorts[0] = true;
                    InPorts[0] = true;
                }
            }
        }

        //vystup obvodu
        class OUT : Gate
        {
            public OUT(int ID)
            {
                this.ID = ID;
                set = 0;
                root = false;
                type = "OUT";
                InConnection = 0;

                InPorts = new bool[1];
                InPorts[0] = false;

                OutPorts = new bool[1];
                OutPorts[0] = false;
            }

            public override bool function()
            {
                if (InPorts[0] == true)
                {
                    OutPorts[0] = true;
                }
                else
                {
                    OutPorts[0] = false;
                }
                return OutPorts[0];
            }
        }

        //uklada aktualny stav spojenia
        class Connection
        {
            public string Name;
            public bool Value;
            public int StartGateID;
            public int EndGateID;

            public Connection(string Name)
            {
                this.Name = Name;
                Value = false;
                StartGateID = -1;
                EndGateID = -1;
            }
        }

        public char boolToNumber(bool val)
        {
            if (val == true)
            {
                return '1';
            }
            else
            {
                return '0';
            }
        }

        //vytvori vystupny vektor pre vsetky mozne kombinacie vstupov pre logicky clen 
        public string vektorLogickehoClena(Gate hradlo)
        {
            string output = "y = ";

            /*//pomocne
            string[] x = new string[hradlo.InPorts.Length];
            for (int j = 0; j < hradlo.InPorts.Length; j++)
            {
                x[j] = "x" + j.ToString() + " = ";
            }
            //------*/

            //najprv nastavim vsetky vstupne porty na true -- pri generovani vstupnych vektorov sa potom vsetky hned prepnu na false
            for (int j = 0; j < hradlo.InPorts.Length; j++)
            {
                hradlo.InPorts[j] = true;
            }

            //generovanie vsetkych vstupov a ulozenie vystupu
            for (int i = 0; i < Math.Pow(2, hradlo.InPorts.Length); i++)
            {
                for (int j = 0; j < hradlo.InPorts.Length; j++)
                {
                    if (i % Math.Pow(2, j) == 0)
                    {
                        hradlo.togglePortValue(j);
                        //MessageBox.Show(i.ToString() + "\n" + j.ToString() + " == " + hradlo.InPorts[j].ToString());
                    }
                    /*//pomocne
                    x[j] = x[j] + boolToNumber(hradlo.InPorts[j]);
                    //------*/
                }
                output = output + boolToNumber(hradlo.function());
            }

            /*//pomocne
            for (int j = 0; j < hradlo.InPorts.Length; j++)
            {
                MessageBox.Show(x[j]);
            }
            //------*/

            return output;
        }

        //vrati hodnotu pravdivostnych vektorov
        public string getTruthVector()
        {
            int totalINgates = CountINgates();
            int totalOUTgates = CountOUTgates();
            string output = "";
            string[] vystupy = new string[totalOUTgates];
            IN tmp;
            OUT outTmp;

            //inicializovat vystupy
            for(int j = 0; j <  totalOUTgates; j++)
            {
                vystupy[j] = "out"+j+"=";
            }

            //najprv nastavim vsetky IN na true -- pri generovani vstupnych vektorov sa potom vsetky hned prepnu na false
            foreach (Gate gate in Gates)
            {
                if (gate.type == "IN")
                {
                    gate.OutPorts[0] = true;
                    gate.InPorts[0] = true;
                }
            }

            //generovanie vsetkych vstupov a ulozenie vystupu
            for (int i = 0; i < Math.Pow(2, totalINgates); i++)
            {
                for (int j = 0; j < totalINgates; j++)
                {
                    if (i % Math.Pow(2, j) == 0)
                    {
                        tmp = (IN)Gates[FindINgate(j)];
                        tmp.toggleOutPortValue();
                        //MessageBox.Show(i.ToString() + "\n" + j.ToString() + " == " + tmp.OutPorts[0].ToString());
                    }
                }
                //vypocita hodnoty v obvode
                resolveCircuit();

                //ulozi hodnoty do premennych pre kazdy vystup
                for(int j = 0; j <  totalOUTgates; j++)
                {
                    outTmp = (OUT)Gates[FindOUTgate(j)];
                    vystupy[j] = vystupy[j] + boolToNumber(outTmp.OutPorts[0]);
                }
            }

            //vyrobit jeden vystup
            for (int j = 0; j < totalOUTgates; j++)
            {
                output = output + vystupy[j] + "\r\n";
            }
            
            return output;
        }

        //vrati poziciu n-teho IN gatu v Gates(v arrayliste) - napr. ak mam 3 IN medzi gates, druhy IN je ulozeny v Gates[4], tak vrati 4 (<- FindINgate(1)) 
        private int FindINgate(int n)
        {
            int result = -1;
            int i = 0;
            int j = 0;

            foreach (Gate gate in Gates)
            {
                if (gate.type == "IN")
                {
                    if (j == n)
                    {
                        //MessageBox.Show("find="+gate.ID.ToString());
                        result = i;
                        return result;
                    }
                    j++;
                }
                i++;
            }

            return result;
        }

        //vrati poziciu n-teho OUT gatu v Gates(v arrayliste)
        private int FindOUTgate(int n)
        {
            int result = -1;
            int i = 0;
            int j = 0;

            foreach (Gate gate in Gates)
            {
                if (gate.type == "OUT")
                {
                    if (j == n)
                    {
                        //MessageBox.Show("find=" + gate.ID.ToString() + " " + gate.ToString());
                        result = i;
                        return result;
                    }
                    j++;
                }
                i++;
            }

            return result;
        }

        //spocita pocet IN gates
        private int CountINgates()
        {
            int i = 0;
            foreach (Gate gate in Gates)
            {
                if (gate.type == "IN")
                {
                    i++;
                }
            }
            return i;
        }

        //spocita pocet OUT gates
        private int CountOUTgates()
        {
            int i = 0;
            
            foreach (Gate gate in Gates)
            {
                if (gate.type == "OUT")
                {
                    i++;
                }
            }
            return i;
        }

        //spocita pocet zapojenych spojeni pre gate s ID
        private int CountInConnections(int ID)
        {
            int i = 0;
            foreach (Connection Con in Connections)
            {
                if (Con.EndGateID == ID)
                {
                    i++;
                }
            }
            return i;
        }

        //prejde uzly z graf_modulu a ulozi potrebne informacie(ID, funkcia, pocet vstupov)
        private void loadGates()
        {
            foreach (Digi_graf_modul.NodeCtrl Node in Digi_graf_modul.graf_modul.form.Nodes)
            {
                if (Node.Type.StartsWith("IN"))
                {
                    IN tmpGate = new IN(Node.ID);
                    tmpGate.OutConnection = Node.ConOut[0].ToString();
                    tmpGate.InConnection = CountInConnections(tmpGate.ID);
                    Gates.Add(tmpGate);
                    //MessageBox.Show(tmpGate.ID + "\n" + tmpGate.InConnection);
                }
                if (Node.Type.StartsWith("OUT"))
                {
                    OUT tmpGate = new OUT(Node.ID);
                    tmpGate.InConnection = CountInConnections(tmpGate.ID);
                    Gates.Add(tmpGate);
                    //MessageBox.Show(tmpGate.ID + "\n" + tmpGate.InConnection);
                }
                if (Node.Type.StartsWith("AND"))
                {
                    AND tmpGate = new AND(Node.ConIN.Count, Node.ID);
                    tmpGate.OutConnection = Node.ConOut[0].ToString();
                    tmpGate.InConnection = CountInConnections(tmpGate.ID);
                    Gates.Add(tmpGate);
                    //MessageBox.Show(tmpGate.ID + "\n" + tmpGate.InConnection);
                }
                if (Node.Type.StartsWith("NAND"))
                {
                    NAND tmpGate = new NAND(Node.ConIN.Count, Node.ID);
                    tmpGate.OutConnection = Node.ConOut[0].ToString();
                    tmpGate.InConnection = CountInConnections(tmpGate.ID);
                    Gates.Add(tmpGate);
                    //MessageBox.Show(tmpGate.ID + "\n" + tmpGate.OutPorts[0]);
                }
                if (Node.Type.StartsWith("OR"))
                {
                    OR tmpGate = new OR(Node.ConIN.Count, Node.ID);
                    tmpGate.OutConnection = Node.ConOut[0].ToString();
                    tmpGate.InConnection = CountInConnections(tmpGate.ID);
                    Gates.Add(tmpGate);
                    //MessageBox.Show(tmpGate.ID + "\n" + tmpGate.OutPorts[0]);
                }
                if (Node.Type.StartsWith("NOR"))
                {
                    NOR tmpGate = new NOR(Node.ConIN.Count, Node.ID);
                    tmpGate.OutConnection = Node.ConOut[0].ToString();
                    tmpGate.InConnection = CountInConnections(tmpGate.ID);
                    Gates.Add(tmpGate);
                    //MessageBox.Show(tmpGate.ID + "\n" + tmpGate.OutPorts[0]);
                }
                if (Node.Type.StartsWith("XOR"))
                {
                    XOR tmpGate = new XOR(Node.ID);
                    tmpGate.OutConnection = Node.ConOut[0].ToString();
                    tmpGate.InConnection = CountInConnections(tmpGate.ID);
                    Gates.Add(tmpGate);
                    //MessageBox.Show(tmpGate.ID + "\n" + tmpGate.OutPorts[0]);
                }
                if (Node.Type.StartsWith("INV"))
                {
                    NOT tmpGate = new NOT(Node.ID);
                    tmpGate.OutConnection = Node.ConOut[0].ToString();
                    tmpGate.InConnection = CountInConnections(tmpGate.ID);
                    Gates.Add(tmpGate);
                    //MessageBox.Show(tmpGate.ID + "\n" + tmpGate.OutPorts[0]);
                }
            }
        }

        //prejde spojenia z graf_modulu a ulozi potrebne informacie(nazov, vstupny a vystupny uzol v smere od IN po OUT)
        private void loadConnections()
        {
            foreach (Digi_graf_modul.Connection Con in Digi_graf_modul.graf_modul.form.Connections)
            {
                Connection tmpCon = new Connection(Con.Name);
                if (!Con.StartNode.Type.StartsWith("OUT") && Con.Name == Con.StartNode.ConOut[0].ToString())
                {
                    tmpCon.StartGateID = Con.StartNode.ID;
                    tmpCon.EndGateID = Con.EndNode.ID;
                }
                if (!Con.EndNode.Type.StartsWith("OUT") && Con.Name == Con.EndNode.ConOut[0].ToString())
                {
                    tmpCon.StartGateID = Con.EndNode.ID;
                    tmpCon.EndGateID = Con.StartNode.ID;
                }
                Connections.Add(tmpCon);
                //MessageBox.Show(tmpCon.Name + "\n" + tmpCon.StartGateID.ToString() + "\n" + tmpCon.EndGateID.ToString());
            }
        }

        //prepocita obvod hodnoty v obvode
        private void resolveCircuit()
        {
            bool endSim = false;

            while (endSim == false)
            {
                //prejdem vsetky spojenia a hladam root gates a za nimi hned zapojene gates
                foreach (Gate rootGate in Gates)
                {
                     if (rootGate.root == true)
                     {
                         foreach (Connection Con in Connections)
                         {
                            foreach (Gate gate in Gates)
                            {
                                //prenesiem hodnoty medzi root -> connection -> gate
                                if (Con.StartGateID == rootGate.ID && Con.EndGateID == gate.ID)
                                {
                                    Con.Value = rootGate.OutPorts[0];
                                    //nastavujem iba predtym nenastavene porty
                                    if (gate.set < gate.InConnection && gate.set >= 0)
                                    {
                                        //MessageBox.Show("ID= " + gate.ID.ToString() + " " + gate.ToString() + "\nset= " + gate.set.ToString() + "\nInCon= " + gate.InConnection.ToString() + "\nOutPorts= " + gate.OutPorts.Length);
                                        gate.InPorts[gate.set] = Con.Value;
                                        gate.set++;
                                    }

                                    //MessageBox.Show("ID= " + gate.ID.ToString() + " " + gate.ToString() + "\nset= " + gate.set.ToString() + "\nInCon= " + gate.InConnection.ToString() + "\nOutPorts= " + gate.OutPorts.Length);
                                    //gate sa moze stat rootom iba ak ma uz nastavene vsetky porty
                                    if (gate.set >= gate.InConnection)
                                    {
                                        gate.root = true;
                                        gate.function(); //ak sa stane rootom tak ju uz mozem vyhodnotit
                                    }
                                }
                            }
                         }
                         //stary root zrusim, ak to nieje uz OUT
                         if (rootGate.type != "OUT")
                         {
                             //MessageBox.Show("ID= " + rootGate.ID.ToString() + "\nResult= " + rootGate.OutPorts[0].ToString() + "\nRoot= " + rootGate.root.ToString());
                             rootGate.root = false;
                         }
                    }
                }

                //vypocitam hodnoty v branach
                foreach (Gate gate in Gates)
                {
                    //MessageBox.Show("ID= " + gate.ID.ToString() + " " + gate.ToString() + "\nResult= " + gate.OutPorts[0].ToString() + "\nInports= " + gate.InPorts[0].ToString());
                    gate.function();
                    //MessageBox.Show("ID= " + gate.ID.ToString() + " " + gate.ToString() +"\nResult= " + gate.OutPorts[0].ToString() + "\nRoot= " + gate.root.ToString());
                }

                //skontrolujem ci uz niesom na konci nahodou(kontrolujem iba zapojene OUTy), ak som na konci tak maju vsetky zapojene OUT hodnotu root
                endSim = true;
                foreach (Connection Con in Connections)
                {
                    foreach (Gate outGate in Gates)
                    {
                        if (outGate.type == "OUT" && Con.EndGateID == outGate.ID && outGate.root != true)
                        {
                            endSim = false;
                        }
                        //MessageBox.Show("ID= " + outGate.ID.ToString() +" " + outGate.ToString() + "\nResult= " + outGate.OutPorts[0].ToString() + "\nRoot= " + outGate.root.ToString());
                    }
                }
                
                foreach (Gate gate in Gates)
                {
                    //MessageBox.Show("ID= " + gate.ID.ToString() + " " + gate.ToString() + "\nResult= " + gate.OutPorts[0].ToString() + "\nRoot= " + gate.root.ToString());
                }
            }

            //naspat resetnut rootov iba na IN, a set na 0 vsade
            foreach (Gate gate in Gates)
            {
                gate.set = 0;
                if (gate.type == "IN")
                {
                    gate.root = true;
                }
                else
                {
                    gate.root = false;
                }
            }
        }
        
        private void btnSimulate_Click(object sender, EventArgs e)
        {      
            try 
            {
                //aby sa mohol obvod vyhodnotit, tak musia byt priradene vstupy na IN a vystupy na OUT
                //connection ma StartNode a EndNode (treba vsak dat pozor, lebo to zalezi iba od smeru ako sa to nakresli)
                //connection ma unikatny nazov Name
                //NodeCtrl ma Type a ID
                //problem moze byt, ze ktory connection vybrat tak aby to islo vzdy smerom k OUT (treba nejak cez ConIN a ConOUT)

                //najprv sa musia nacitat spojenia!!!
                loadConnections();
                loadGates();

                textBox1.Text = getTruthVector();

                Gates.Clear();
                Connections.Clear();

                //tmpCon = (Digi_graf_modul.Connection)Digi_graf_modul.graf_modul.form.Connections[0];
                //MessageBox.Show(tmpCon.StartNode.Type.ToString());
                //textBox1.Text = TEST(Digi_graf_modul.graf_modul.form.active_gate);
            }
            catch (Exception exception)
            {
                MessageBox.Show("V grafickom editore sa nenachadza ziadny logicky clen.\n"+exception);
            }
        }
    }
}
