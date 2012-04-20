using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginInterface
{
    public class ApplicationCore : IApplicationBase
    {
        public delegate void MaximalizujEventHandler(object sender, EventArgs e);
        public event MaximalizujEventHandler MaximalizujEvent;

        public ApplicationCore()
        {

        }

        /// 

        // ulozi aktualny obvod do docasneho suboru
        // public bool UlozUzly();

        // vymaze sa vsetko (ako keby sa vytvoril novy projekt)
        public void Cleanup(){
            ConCount = 0;
            SetGates();
            pictureBox.Controls.Clear();
            Nodes.Clear();
            Connections.Clear();
            RefreshListCons();
            RefreshListNodes();
            PaintMain();
           // YYY toto nevieme na co je txtMain.Text = "";
            this.Text = "Digi Creator - " + ModelName.ToString();
            //MainSurface1.Invalidate();
        }

        // vytvory uzol s danymi parametrami
        //public void CreateNode(int x, int y, int ID, string Name, string Type, string[] portsIN, string portsOUT);

        public void CreateNode2(int x, int y)
        { //vytvor uzol - vytvara sa v picturebox tahanim zo zoznamu hradiel

            NodeCtrl node = new NodeCtrl();
            node.Left = x;
            node.Top = y;
            node.Text = active_gate.name;

            string value = node.Text;

            /* YYY
                        if (ModelType == "Petri" || ModelType == "FSM")
                        {
                            bool draw = true;
                            while (draw)
                            {
                                DialogResult dlg = InputBox("Názov", "Zadaj názov:", ref value);
                                if (dlg == DialogResult.OK)
                                {
                                    node.Text = value;
                                    draw = false;
                                    foreach (NodeCtrl n in Nodes)
                                    {
                                        if (value == n.Text)
                                        {
                                            MessageBox.Show("Uzol s rovnakým názvom už existuje. Zadajte nový.", "Chyba");
                                            draw = true;
                                        }
                                    }
                                }
                                else
                                {
                                    draw = false;
                                    return;
                                }
                            }
                        }*/

            node.ShownText = active_gate.name;
            node.Type = active_gate.name;

            node = SetNodeType(node); //nastavime typ hradla (AND2, AND3...)
            // pohyb hradla po ploche:
            node.MouseDown += new MouseEventHandler(node_MouseDown);
            node.MouseUp += new MouseEventHandler(node_MouseUp);
            node.MouseMove += new MouseEventHandler(node_MouseMove);
            node.MouseClick += new MouseEventHandler(node_Click);
            node.DoubleClick += new EventHandler(node_DoubleClick);
            node.MouseEnter += new EventHandler(node_MouseEnter);

            foreach (ConPort port in active_gate.PortsIN)
            {// pre vsetky vstupne porty vo vyznacenom hradle pridat do uzla
                node.AddIn(port.x, port.y, port.name);
            }
            foreach (ConPort port in active_gate.PortsOUT)
            {// pre vsetky vystupne porty vo vyznacenom hradle pridat do uzla
                node.AddOut(port.x, port.y, port.name);
            }
            node.BackColor = Color.Transparent;
            Nodes.Add(node);
            pictureBox.Controls.Add(node);
            PaintMain();

            RefreshListNodes();

        }


        public void ConnectAll()
        {
            foreach (NodeCtrl node in Nodes)
            {
                foreach (String str in node.ConIN)
                {
                    int numb = node.ConIN.IndexOf(str) + 1;
                    PictureBox portx = null;
                    foreach (PictureBox port in node.Controls)
                    {
                        if (port.Tag.ToString() == numb.ToString())
                        {
                            portx = port;
                            MessageBox.Show(portx.Tag.ToString());
                            continue;
                        }
                    }

                    StartConnection(node, node.ConIN.IndexOf(str) + 1, "", portx);

                    try
                    {
                        EndConnection(((NodeCtrl)FindNode(node, str)[0]), ((int)FindNode(node, str)[1]) + 2, str);
                    }
                    catch
                    {
                    }
                }
            }
            //pictureBox.Invalidate();
            PaintMain();
            RefreshListCons();
        }



        /// 
        public bool UlozUzly(string FileName)
        {
            try
            {
                ArrayList _Nodes = new ArrayList();
                Stream stream = File.Open(FileName, FileMode.Create);
                BinaryFormatter bF = new BinaryFormatter();
                foreach (NodeCtrl node in Nodes)
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
                MessageBox.Show("uzavrel som stream");

                string FileConName = FileName + "c";
                ArrayList _Cons = new ArrayList();
                stream = File.Open(FileConName, FileMode.Create);
                bF = new BinaryFormatter();

                foreach (Connection con in Connections)
                {
                    //myCons.Add(con);

                    PluginInterface.SavedCon _con = new PluginInterface.SavedCon();
                    _con.Name = con.Name;
                    _con.StartNode = con.StartNode.Text;
                    _con.EndNode = con.EndNode.Text;
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
        //toto bude nasa funkcia
        public void NacitajUzly(string FileName)
        {
            ArrayList _Nodes = new ArrayList();

            Cleanup();


            Stream stream = File.Open(FileName, FileMode.Open);
            BinaryFormatter bF = new BinaryFormatter();
            _Nodes = (ArrayList)bF.Deserialize(stream);

            ArrayList _Cons = new ArrayList();

            string FileConName = FileName + "c";
            Stream stream1 = File.Open(FileConName, FileMode.Open);
            BinaryFormatter bF1 = new BinaryFormatter();
            _Cons = (ArrayList)bF1.Deserialize(stream1);

            foreach (PluginInterface.SavedCon conex in _Cons)
            {

                Connection con = new Connection();
                con.Name = conex.Name;
                Connections.Add(con);
            }

            foreach (PluginInterface.SavedNode nodex in _Nodes)
            {
                NodeCtrl node = new NodeCtrl();
                node.ConIN = nodex.ConIN;
                node.ConOut = nodex.ConOut;
                node.Text = nodex.Name;
                node.Type = nodex.Type;
                node.ID = nodex.id;
                node.Left = nodex.X;
                node.Top = nodex.Y;
                foreach (Gate g in Gates)
                {
                    if (g.name == node.Type)
                        active_gate = g;
                }
                Nodes.Add(node);
                //MessageBox.Show(node.ConIN.
                LoadNodes(node);

                foreach (Connection C in Connections)
                {
                    foreach (string co in node.ConOut)
                    {
                        if (co == C.Name) C.StartNode = node;
                    }
                    foreach (string ci in node.ConIN)
                    {
                        if (ci == C.Name) C.EndNode = node;
                    }
                }

            }

            stream1.Position = 0;
            foreach (PluginInterface.SavedCon conex in _Cons)
            {
                foreach (Connection con in Connections)
                {
                    int a = 0;
                    int b = 0;
                    int p = 0;
                    int q = 0;
                    if (con.Name == conex.Name)
                    {
                        con.StartNode.Text = conex.StartNode;
                        con.EndNode.Text = conex.EndNode;
                        foreach (String n in con.StartNode.ConOut)
                        {
                            // MessageBox.Show("ConOut" + n);                        
                            a++;
                            if (n == con.Name)
                            {
                                p = a;
                                /*      MessageBox.Show("Start"+p.ToString());
                                      MessageBox.Show(n);
                                      MessageBox.Show("meno prepojenia:" + con.Name);
                                */
                                int numb = con.StartNode.ConOut.IndexOf(n) + 1;
                                PictureBox portx = null;
                                foreach (PictureBox port in con.StartNode.Controls)
                                {
                                    //if (port.Tag.ToString() == numb.ToString())

                                    if (port.Tag.ToString() == con.StartNode.PortCount.ToString())
                                    {
                                        portx = port;
                                        //MessageBox.Show("portx:"+portx.Tag.ToString());
                                        //continue;
                                    }
                                }

                                Load_StartConnection(con, con.StartNode, numb, "", portx);
                                // Load_StartConnection(con, ((NodeCtrl)FindNode(con.StartNode, n)[1]), ((int)FindNode(con.StartNode, n)[0]) + 2, "", portx);           
                            }
                        }
                        foreach (String n in con.EndNode.ConIN)
                        {
                            b++;
                            if (n == con.Name)
                            {
                                q = b;
                                /* MessageBox.Show("End:" + q.ToString());
                                 MessageBox.Show(n);
                                 MessageBox.Show("meno prepojenia:" + con.Name);
                                */
                                // Load_EndConnection(con,((NodeCtrl)FindNode(con.EndNode, n)[0]), ((int)FindNode(con.EndNode, n)[1]) + 2, n);

                                Load_EndConnection(con, con.EndNode, q, n);
                            }
                        }


                    }
                }


            }
            stream.Close();
            stream1.Close();

            PaintMain();
            RefreshListCons();

        }


    }
}
