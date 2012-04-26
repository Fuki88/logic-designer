using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections;

namespace Logic_Designer
{
    public class Hradlo
    {
        public int inputs = 0, outputs = 0;
        public List<String> input_names = new List<string>();
        public List<String> output_names = new List<string>();
        public String type;
        public String name;
    }

    public class Spoj
    {
        public String conA, conB;
        public String nodeA, nodeB;
    }

    public class Suciastka
    {
        public String name;
        public String cName;
        public String oName;
        public String type;
        public ArrayList conIn = new ArrayList();
        public ArrayList conOut = new ArrayList();
        public int X, Y;
        public int vstupy;
    }

    public class ParseVHDL
    {
        static int lineNumber = 0;
        static String[] code;
        static List<Hradlo> nodes = new List<Hradlo>();

        public ArrayList circuit_nodes = new ArrayList();
        public List<String> circuit_output_names = new List<string>();
        public List<Hradlo> components = new List<Hradlo>();

        public ArrayList vstupy = new ArrayList();
        public ArrayList vystupy = new ArrayList();

        static char[] delimiterChars = { ' ', ',', '.', ':', '\t', '(', ')', ';' }; //oddelovace slov

        String getLine(){
            Regex.Replace(code[lineNumber], @"\s+", " "); // odstranenie viacnasobnych medzier
            return code[lineNumber];            
        }

        void setLine(int index = 1)
        {
            lineNumber += index;
        }

        String[] getArray()
        {
            return getLine().Trim().Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries); // rozdelit na slova a ulozit do pola (trim odstrani medzery)
        }

        public void Parse(String rawCode)
        {
            code = Regex.Split(rawCode, "\n"); //rozdeli na riadky
            nodes.Clear();
            circuit_nodes.Clear();

            while (lineNumber < code.Length)
            { // pokial neprideme na koniec
                if (getLine().Length == 0) break;
                string[] words = getArray();   // nacita riadok ako pole
                //MessageBox.Show(words[0]);

                if (words[0].Contains("--"))
                {
                    setLine();
                    continue; // preskocenie komentarov
                }

                // rozhodovanie
                switch (words[0].ToLower())
                {
                    case "entity":
                        if (words[1] != "obvod")
                        {
                            Hradlo hradlo = new Hradlo();
                            hradlo.name = words[1];
                            setLine();
                            words = getArray();
                            if (words[0] == "port")
                            {
                                for (int i = 1; i < words.Length; i += 3)
                                {
                                    // pridanie vstupov a vystupov hradla
                                    if (words[i + 1] == "in")
                                    {
                                        hradlo.input_names.Add(words[i]);
                                        hradlo.inputs++;
                                    }
                                    else if (words[i + 1] == "out")
                                    {
                                        hradlo.output_names.Add(words[i]);
                                        hradlo.outputs++;
                                    }
                                }

                                setLine();
                                words = getArray();
                                if (words[0] == "end" && words[1] == hradlo.name)
                                {
                                    // podmienku aby to nepridalo hradlo ktore uz v zozname je
                                    nodes.Add(hradlo);
                                }
                                else setLine(-1);
                            }
                            else setLine(-1);
                        }
                        else if (words[1] == "obvod")   // entity obvod
                        {
                            setLine();
                            words = getArray();
                            if (words[0] == "port")
                            {
                                //setLine();
                                //words = getArray();
                                //if (words[0] == "end" && words[1] == "obvod") setLine();
                                for (int i = 1; i < words.Length; i += 3)
                                {
                                    // pridanie vstupov a vystupov celkoveho obvodu
                                    if (words[i + 1] == "in") vstupy.Add(words[i]);
                                    else if (words[i + 1] == "out") vystupy.Add(words[i]);
                                }
                            }
                        }
                        break;
                    case "architecture":
                        if (!getLine().Contains("obvod"))
                        {
                            foreach (Hradlo tmp in nodes)
                            {
                                if (tmp.name == words[3]) // ak sa entita s takymto nazvom nachadza medzi definovanymi
                                {
                                    setLine(2); // preskoci begin
                                    words = getArray();
                                    tmp.type = words[3].ToLower(); // zisti typ hradla (AND/OR/XOR/NAND/...)
                                    setLine(); // preskoci end
                                }
                            }
                        } else if (getLine().Contains("obvod")){   // architecture xxx of obvod is
                          /*  setLine();
                            words = getArray();
                            if (words[0] == "component")
                            {
                                Hradlo komponent = new Hradlo();
                                komponent.name = words[1];
                                setLine();
                                words = getArray();
                                if (words[0] == "port")
                                {
                                    for (int i = 1; i < words.Length; i += 3)
                                    {
                                        // pridanie vstupov a vystupov hradla
                                        if (words[i + 1] == "in")
                                        {
                                            komponent.input_names.Add(words[i]);
                                            komponent.inputs++;
                                        }
                                        else if (words[i + 1] == "out")
                                        {
                                            komponent.output_names.Add(words[i]);
                                            komponent.outputs++;
                                        }
                                    }
                                    setLine();
                                    words = getArray();
                                    if (words[0] != "end" || words[1] != "component") setLine(-1);
                                }
                            } */
                        } 
                        break;
                    case "component":
                        Hradlo komponent = new Hradlo();
                        komponent.name = words[1];
                        setLine();
                        words = getArray();
                        if (words[0] == "port")
                        {
                            for (int i = 1; i < words.Length; i += 3)
                            {
                                // pridanie vstupov a vystupov hradla
                                if (words[i + 1] == "in")
                                {
                                    komponent.input_names.Add(words[i]);
                                    komponent.inputs++;
                                }
                                else if (words[i + 1] == "out")
                                {
                                    komponent.output_names.Add(words[i]);
                                    komponent.outputs++;
                                }
                            }
                            setLine();
                            words = getArray();
                            if (words[0] != "end" || words[1] != "component") setLine(-1);
                            this.components.Add(komponent);
                        }
                        break;
                    case "for":
                        Suciastka suc = new Suciastka();
                        suc.name = words[1];
                        suc.cName = words[2];
                        suc.oName = words[6];
                        circuit_nodes.Add(suc);
                        break;
                    case "begin":
                        break;
                    case "end":
                        break;
                    default:
                        bool nasiel = false;
                        foreach (Suciastka tmp in circuit_nodes)
                        {
                            if (tmp.name == words[0]) // ak sa entita s takymto nazvom nachadza medzi definovanymi
                            {
                                nasiel = true;
                                foreach (Hradlo hradlo in nodes)
                                {
                                    if (hradlo.name == tmp.oName)
                                    {
                                        tmp.vstupy = hradlo.inputs;
                                        tmp.type = hradlo.type + hradlo.inputs.ToString();
                                        int i;
                                        for (i = 0; i < tmp.vstupy; i++) tmp.conIn.Add(words[4 + i]);
                                        tmp.conOut.Add(words[4 + i]);
                        
                                    }
                                }
                                //MessageBox.Show("Nazov: " + tmp.name + ", Typ: " + tmp.type + ", vstupy: " + (String)tmp.conIn[0] + " " + (String)tmp.conIn[1]);
                            }
                        }
                        if (!nasiel) MessageBox.Show("Prikaz " + words[0] + " sa nenašiel.");
                        break;
                }
                setLine();
            }
            //MessageBox.Show("Pocet nacitanych hradiel: " + nodes.Count().ToString());
            lineNumber = 0;
        }

        public void Draw()
        {
            //PluginInterface.IPluginHost bla;
            int i = 20;
            int id = 0;

            Form1.ClearNode();
            foreach (Suciastka tmp in circuit_nodes)
            {
                //bla.CreateNode(i, i, i, suc.name, , suc.input_names, suc.output_names);
                //Form1.SetNode(NOD.ID, NOD.ConIN, NOD.ConOut, NOD.Name, NOD.Type, NOD.Left, NOD.Top);
                MessageBox.Show("Nazov: " + tmp.name + ", Typ: " + tmp.type + ", vstupy: " + (String)tmp.conIn[0] + " " + (String)tmp.conIn[1]);
                Form1.SetNode(id++, tmp.conIn, tmp.conOut, tmp.name, tmp.type.ToUpper(), i, i);
                //String[] pole = { tmp.conIn[0].ToString(), tmp.conIn[1].ToString() };
                //Form1.CreateNode(i, i, id++, tmp.name, tmp.type, pole, (String)tmp.conOut[0]);
                i+= 20;                
            }

            Logic_Designer.Form1.ClearCon();
//            foreach (Connection CON in Connections)
//                Logic_Designer.Form1.SetCon(CON.Name, CON.StartNode.Name, CON.EndNode.Name);
            Form1.SetCon("negA", "xNAND2_a", "xNAND2_c");
            //Form1.MakeCons();
            Form1.UlozUzly();
            Form1.UlozUzly("obvod_tmp.z5");
        }
    }
}
