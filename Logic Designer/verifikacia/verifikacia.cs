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

namespace Logic_Designer.verifikacia
{
    public partial class verifikacia : UserControl
    {
        
        //---------------------------------------------------------------------------------------------------
        //Pravdivostny vektor
        //---------------------------------------------------------------------------------------------------

        //sem sa ulozia vsetky uzly a spojenia - uchovaju hodnotu
        public ArrayList Gates = new ArrayList();
        public ArrayList Connections = new ArrayList();
        public int lolnatretiu;

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
            public Point startP;
            public Point endP;

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
            bool err = true;

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
                err = resolveCircuit();

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

            //skontroluje ci boli vsetky OUT zapojene
            if (err == false)
            {
                MessageBox.Show("Nezapojili ste vsetky vystupy 'OUT' k hradlam - hodnota nezapojeneho vystupneho vektora ostane nulova");
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
                tmpCon.startP = Con.Start;
                tmpCon.endP = Con.End;
                Connections.Add(tmpCon);
                //MessageBox.Show(tmpCon.Name + "\n" + tmpCon.StartGateID.ToString() + "\n" + tmpCon.EndGateID.ToString());
            }
        }

        //ma cenu vobec obvod vyhodnotit?
        private bool startResolving()
        {
            int o = 0; //sem sa zrataju vsetky vystupy
            int i = 0; //sem sa zrataju vsetky vstupy

            bool err_i = true;

            bool result = true;

            //zrata IN a OUT
            foreach (Gate Gate in Gates)
            {
                //skontroluje OUT
                if (Gate.type == "OUT")
                {
                    o++;
                }

                //skontroluje IN
                if (Gate.type == "IN")
                {
                    i++;

                    foreach (Connection Con in Connections)
                    {
                        //MessageBox.Show("ID= " + Gate.ID.ToString() + "\ndetail= " + Gate.ToString() + "\nCon= " + Con.Name + "\nStartGate= " + Con.StartGateID + "\nEndGate= " + Con.EndGateID);
                        if (Con.StartGateID == Gate.ID)
                        {
                            err_i = false; //ak existuje aspon jeden zapojeny IN
                        }
                    }
                }
            }

            if (i == 0 && o == 0)
            {
                MessageBox.Show("Nepridali a nezapojili ste ziadne vstupy 'IN' a vystupy 'OUT' k hradlam - obvod nemozno vyhodnotit");
                result = false;
            }
            else
            {
                if (i == 0)
                {
                    MessageBox.Show("Nepridali a nezapojili ste ziadne vstupy 'IN' - obvod nemozno vyhodnotit");
                    result = false;
                }
                if (o == 0)
                {
                    MessageBox.Show("Nepridali a nezapojili ste ziadne vystupy 'OUT' k hradlam - obvod nemozno vyhodnotit");
                    result = false;
                }
            }

            if (err_i == true && i != 0 && o != 0)
            {
                MessageBox.Show("Nezapojili ste ani jeden vstup 'IN' k hradlu - obvod nemozno vyhodnotit");
                result = false;
            }

            return result;
        }

        //prepocita obvod hodnoty v obvode
        private bool resolveCircuit()
        {
            bool becameRoot = false; //kontrolujem ci sa tejto iteracii stal aspon jeden obvod rootom, ak nie tak ukoncim simulaciu
            bool endSim = false; //ak true tak skonci simulacia

            bool result = true;

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
                                        //MessageBox.Show("1 ID= " + gate.ID.ToString() + " " + gate.ToString() + "\nset= " + gate.set.ToString() + "\nInCon= " + gate.InConnection.ToString() + "\nOutPorts= " + gate.OutPorts.Length);
                                        gate.InPorts[gate.set] = Con.Value;
                                        gate.set++;
                                        //MessageBox.Show("2 ID= " + gate.ID.ToString() + " " + gate.ToString() + "\nset= " + gate.set.ToString() + "\nInCon= " + gate.InConnection.ToString() + "\nOutPorts= " + gate.OutPorts.Length);
                                    }

                                    //MessageBox.Show("ID= " + gate.ID.ToString() + " " + gate.ToString() + "\nset= " + gate.set.ToString() + "\nInCon= " + gate.InConnection.ToString() + "\nOutPorts= " + gate.OutPorts.Length);
                                    //gate sa moze stat rootom iba ak ma uz nastavene vsetky porty
                                    if (gate.set >= gate.InConnection)
                                    {
                                        gate.root = true;
                                        gate.function(); //ak sa stane rootom tak ju uz musim vyhodnotit
                                        becameRoot = true;
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

                /*
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
                */

                //skontrolujem ci som uz na konci, ak ano tak vsetky gate maju nastavene vsetky zapojene inporty
                endSim = true;
                foreach (Gate Gate in Gates)
                {
                    if (becameRoot == true && Gate.set < Gate.InConnection)
                    {
                        //MessageBox.Show("ID= " + Gate.ID.ToString() + " " + Gate.ToString() + "\nResult= " + Gate.OutPorts[0].ToString() + "\nRoot= " + Gate.root.ToString() + "\nset= " + Gate.set.ToString() + "\nInCon= " + Gate.InConnection.ToString());
                        endSim = false;
                    }
                    //MessageBox.Show("ID= " + Gate.ID.ToString() +" " + Gate.ToString() + "\nResult= " + Gate.OutPorts[0].ToString() + "\nRoot= " + Gate.root.ToString());
                }

                /*
                foreach (Gate gate in Gates)
                {
                    MessageBox.Show("ID= " + gate.ID.ToString() + " " + gate.ToString() + "\nResult= " + gate.OutPorts[0].ToString() + "\nRoot= " + gate.root.ToString());
                }
                */
                becameRoot = false;
            }

            //naspat resetnut rootov iba na IN, a set na 0 vsade
            foreach (Gate gate in Gates)
            {
                if (gate.type == "OUT" && gate.set == 0)
                {
                    result = false;
                }
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

            return result;
        }
        
        private void btnSimulate_Click(object sender, EventArgs e)
        {      
            try 
            {
                //najprv sa musia nacitat spojenia (pred hradlami)!!!
                loadConnections();
                loadGates();

                if (startResolving() == true)
                {
                    textBox1.Text = getTruthVector();
                }

                Gates.Clear();
                Connections.Clear();

                //tmpCon = (Digi_graf_modul.Connection)Digi_graf_modul.graf_modul.form.Connections[0];
                //MessageBox.Show(tmpCon.StartNode.Type.ToString());
                //textBox1.Text = TEST(Digi_graf_modul.graf_modul.form.active_gate);
            }
            catch (Exception exception)
            {
                
            }
        }

        private void verifikacia_Load(object sender, EventArgs e)
        {

        }



        //---------------------------------------------------------------------------------------------------
        //Vizualizacia
        //---------------------------------------------------------------------------------------------------
        

        public Graphics gpic1;
        public SolidBrush brushpic1;
        public Pen penpic1;
        public Pen penFalse = new Pen(Color.Red, 2f);
        public Pen penTrue = new Pen(Color.Green, 2f);

        public static System.Windows.Forms.PictureBox pictureBox;

        public verifikacia()
        {
            InitializeComponent();
            
            pictureBox1.AllowDrop = true;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.DoubleBuffer |
          ControlStyles.UserPaint |
          ControlStyles.AllPaintingInWmPaint
          | ControlStyles.SupportsTransparentBackColor,
          true);
            this.UpdateStyles();

            brushpic1 = new SolidBrush(Color.Blue);
            penpic1 = new Pen(brushpic1);
             
            pictureBox = pictureBox1;
        }

        public bool conVal(Digi_graf_modul.Connection connection)
        {
            bool result = true;
            foreach (Connection con in Connections)
            {
                if (con.Name == connection.Name)
                {
                    result = con.Value;
                    return result;
                }
            }

            return result;
        }

        public void paintAll()
        {
            pictureBox1.Invalidate(false);
            pictureBox1.Update();
            pictureBox1_Paint(pictureBox1, new PaintEventArgs(pictureBox1.CreateGraphics(), new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height)));
        }

        public void clearPaint()
        {
            pictureBox1.Invalidate(false);
            pictureBox1.Update();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
                Graphics g = e.Graphics;

                brushpic1.Color = Color.Black;

                foreach (Digi_graf_modul.Connection con in Digi_graf_modul.graf_modul.form.Connections)
                {
                    penpic1.Width = con.ConWidth;

                    con.Start = new Point(con.StartNode.Left + con.StartPortPic.Left, con.StartNode.Top + con.StartPortPic.Top);
                    con.End = new Point(con.EndNode.Left + con.EndPortPic.Left, con.EndNode.Top + con.EndPortPic.Top);

                    con.RefreshPath();
                    if (conVal(con) == true)
                    {
                        penpic1 = penTrue;
                    }
                    else
                    {
                        penpic1 = penFalse;
                    }

                    g.DrawPath(penpic1, con.Path);
                    brushpic1.Color = Color.Black;
                    if (con.Type == "ARC") g.FillPie(brushpic1, con.End.X - 20, con.End.Y - 20, 40, 40, con.startAngle, 45);

                    if (conVal(con) == true)
                    {
                        brushpic1.Color = Color.Green;
                    }
                    else
                    {
                        brushpic1.Color = Color.Red;
                    }

                    foreach (Point pt in con.Points)
                    {
                        if (pt != con.Start && pt != con.End)
                        {
                            g.FillEllipse(brushpic1, pt.X - 4, pt.Y - 4, 8, 8);
                        }
                    }
                
                }

                g.Dispose();
        }

        //Digi_graf_modul.NodeCtrl Node in Digi_graf_modul.graf_modul.form.Nodes
        //Digi_graf_modul.Connection Con in Digi_graf_modul.graf_modul.form.Connections

        public void initViz()
        {
            loadConnections();
            loadGates();

            foreach (Digi_graf_modul.NodeCtrl Node in Digi_graf_modul.graf_modul.form.Nodes)
            {
                pictureBox1.Controls.Add(Node);
            }
        }

        public void cancelViz()
        {
            foreach (Digi_graf_modul.NodeCtrl Node in Digi_graf_modul.graf_modul.form.Nodes)
            {
                Digi_graf_modul.graf_modul.form.pictureBox.Controls.Add(Node);
            }

            Gates.Clear();
            Connections.Clear();
        }

        public void refresh()
        {
            resolveCircuit();
            paintAll();
        }

        //refresh
        private void button1_Click_1(object sender, EventArgs e)
        {
            initViz();
            refresh();
        }

        //send_back
        private void button2_Click(object sender, EventArgs e)
        {
            cancelViz();
            clearPaint();
        }

        //otoci hodnotu v hradle IN, na ktore klniknem
        private void changeInVal(int x, int y)
        {
            int c = 20;
            IN tmp;

            foreach (Connection con in Connections)
            {
                foreach (Gate gate in Gates)
                {
                    if (con.StartGateID == gate.ID && gate.type == "IN")
                    {
                        //MessageBox.Show("x=" + x + "\ny=" + y + "\n\nIn x=" + con.startP.X + "\n\nIn y=" + con.startP.Y);
                        if (((x > con.startP.X - c && y > con.startP.Y - c) && (x < con.startP.X + c && y < con.startP.Y + c)))
                        {
                            tmp = (IN)gate;
                            tmp.toggleOutPortValue();
                        }
                    }
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    changeInVal(e.X, e.Y);
                    refresh();
                    break;
                case MouseButtons.Right:
                    break;
                case MouseButtons.Middle:
                    break;
                default:
                    break;
            } 
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                default:
                    paintAll();
                    break;
            } 
        }
    }
}
