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

        //reprezentuje logicke hradlo vo vseobecnosti 
        public class Gate
        {
            public bool[] InPorts;
            public bool[] OutPorts;

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
            public AND(int InPortsNum)
            {
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
            public NAND(int InPortsNum)
            {
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
            public OR(int InPortsNum)
            {
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
            public NOR(int InPortsNum)
            {
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
            public XOR()
            {
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
            public NOT()
            {
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

        //reprezentuje cely obvod
        public class Circuit
        {
            public bool[] InPorts;
            public bool[] OutPorts;
            public bool[] z;

            public ArrayList hradla = new ArrayList();


            public Circuit(int InPortsNum, int OutPortsNum, int zNum)
            {
                InPorts = new bool[InPortsNum];
                for (int i = 0; i < InPorts.Length; i++)
                {
                    InPorts[i] = true;
                }

                OutPorts = new bool[OutPortsNum];
                for (int i = 0; i < OutPorts.Length; i++)
                {
                    OutPorts[i] = true;
                }

                z = new bool[zNum];
                for (int i = 0; i < z.Length; i++)
                {
                    z[i] = true;
                }
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

        public string TEST(Digi_graf_modul.graf_modul.Gate gate)
        {
            string output = "";
            Gate testGate;

            if (gate.name.StartsWith("AND"))
            {
                testGate = new AND(gate.PortsIN.Count);
            }
            else
            {
                if(gate.name.StartsWith("NAND"))
                {
                    testGate = new NAND(gate.PortsIN.Count);
                }
                else
                {
                    if(gate.name.StartsWith("OR"))
                    {
                        testGate = new OR(gate.PortsIN.Count);
                    }
                    else
                    {
                        if(gate.name.StartsWith("NOR"))
                        {
                            testGate = new NOR(gate.PortsIN.Count);
                        }
                        else
                        {
                            if(gate.name.StartsWith("XOR"))
                            {
                                testGate = new XOR();
                            }
                            else
                            {
                                if(gate.name.StartsWith("INV"))
                                {
                                    testGate = new NOT();
                                }
                                else
                                {
                                    testGate = null;
                                }
                            }
                        }
                    }
                }
            }

            //MessageBox.Show( ((Gate)obvod.hradla[0]).function().ToString() );

            output = vektorLogickehoClena(testGate);

            return output;
        }

        private void btnSimulate_Click(object sender, EventArgs e)
        {      
            try 
            {
                textBox1.Text = TEST(Digi_graf_modul.graf_modul.form.active_gate);
            }
            catch (Exception exception)
            {
                MessageBox.Show("V grafickom editore sa nenachadza ziadny logicky clen.");
            }
        }
    }
}
