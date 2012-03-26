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
//using PluginInterface;
using System.Xml.Serialization;  // Podpora XML


namespace Digi_graf_modul
{
    public partial class graf_modul : UserControl
    {

       // bool paint = false;


        struct ConPort
        {
            public int x;
            public int y;
            public string name;
        }

        public struct Gate
        {
            public string name;
            public string text;
            public ArrayList PortsIN;
            public ArrayList PortsOUT;
            public void AddPortIN(int x, int y, string name) //prida vstup. porty hradla so sur. x a y do konk. hradla
            {
                ConPort port = new ConPort();
                port.x = x;
                port.y = y;
                port.name = name;
                PortsIN.Add(port);
            }
            public void AddPortOUT(int x, int y, string name) //prida vystup. porty hradla so sur. x a y do konk. hradla
            {
                ConPort port = new ConPort();
                port.x = x;
                port.y = y;
                port.name = name;
                PortsOUT.Add(port);
            }

            private Bitmap picture;
            public Bitmap Picture
            {
                get { return picture; }
                set { picture = value; }
            }

            [XmlElementAttribute("Picture")]
            public byte[] PictureByteArray
            {
                get
                {
                    if (picture != null)
                    {
                        TypeConverter BitmapConverter = TypeDescriptor.GetConverter(picture.GetType());
                        return (byte[])BitmapConverter.ConvertTo(picture, typeof(byte[]));
                    }
                    else
                        return null;
                }

                set
                {
                    if (value != null)
                        picture = new Bitmap(new MemoryStream(value));
                    else
                        picture = null;
                }
            }
        }


        private const string XML_FILE_NAME = "testNode.xml";

        private bool changed = false;
        private ArrayList zoznamFunkcii = new ArrayList();

        private int modX = 89;
        private int modY = 55;

        public ArrayList Gates = new ArrayList();

        private string TypObvod = "N/A";
        public string ActualFileName;
        private string tmp_action = "";
        private string action = "";
        private string action2 = "";
        public static graf_modul form;
        public bool mouseDown = false;
        public bool StartCon = false;
        public ArrayList Connections = new ArrayList();
        public ArrayList Nodes = new ArrayList();
        private int ConCount = 0;

        private ArrayList tmpCon = new ArrayList();

        public bool SetLabelVisible = false;

        private ToolTip tip = new ToolTip();

        public Graphics gpic1;
        public SolidBrush brushpic1;
        public Pen penpic1;

        public Object active_object = null;
        public Gate active_gate;

        private bool GridOn = false;
        private Bitmap DrawArea;

        public MenuStrip main_menu;
        private Connection con_move = null;
        private int con_move_pt = 0;
        private string direction = "";

        private string _ModelName = "untitled";

        private int _StateFirst = 0;
        private int _StateSecond = 0;

        public int StateFirst
        {
            get { return _StateFirst; }
            set { _StateFirst = value; }
        }

        public int StateSecond
        {
            get { return _StateSecond; }
            set { _StateSecond = value; }
        }

        public string ModelName
        {
            get { return _ModelName; }
            set { _ModelName = value; }
        }

         public graf_modul()
        {
            InitializeComponent();
            form = this;
            //main_menu = mainMenu;
        }

        public string ModelType
        {
            get { return TypObvod; }
            set { TypObvod = value; }
        }

        public PictureBox GetMainPict
        {
            get { return pictureBox; }
        }

        public SplitContainer GetMainPanel
        {
            get { return splitContainer1; }
        }

        public HScrollBar GetMainHScroll
        {
            get { return hScrollBar; }
        }

        public VScrollBar GetMainVScroll
        {
            get { return vScrollBar; }
        }

        public graf_modul GetForm
        {
            get
            {
                return form;
            }
        }


/*/////////////////////////////--\/--\/--\/--\/--///////////////////////////////////////*/

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            //TextBox textBox = new TextBox();
            MaskedTextBox textBox = new MaskedTextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            string strFirst = "";
            string strSecond = "";

            for (int i = 1; i <= graf_modul.form.StateFirst; i++)
            {
                strFirst += "9";
            }

            for (int i = 1; i <= graf_modul.form.StateSecond; i++)
            {
                strSecond += "9";
            }

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

       

/*////////////////////////////--/\--/\--/\--/\--//////////////////////////////////////////////*/

        tmpNode uzol = null;
        List<tmpNode> _nodes = new List<tmpNode>(); // zoznam hradiel

        public void OpenXML()
        {//otvorenie xml suboru s hradlami, tento je vytvarany pomocou grafickeho editora hradiel
            _nodes.Clear();
            try
            {
                TextReader reader = new StreamReader(XML_FILE_NAME);
                XmlSerializer serializer = new XmlSerializer(typeof(List<tmpNode>));
                _nodes = (List<tmpNode>)serializer.Deserialize(reader);
                _nodes.Add(uzol);
                reader.Close();
                //MessageBox.Show("Zoznam hradiel úspešne načítaný!");
            }

            catch (Exception es)
            {
                //return false;
                MessageBox.Show(es.Message);
            }

        }


        public void SetGates()
        {//nastavenie hradiel - precitanie z XML suboru a vykreslenie vo viewliste
            OpenXML();
            Gates.Clear();
            listViewMain.Clear();

            ImageList largeImage = new ImageList(); //zoznam obrazkov hradiel
            largeImage.ImageSize = new Size(50, 50);
            listViewMain.LargeImageList = largeImage;


                foreach (tmpNode uzlik in _nodes)
                {
                    
                    if (uzlik != null && uzlik.type == "Logic")
                    {
                        Gate gate1 = new Gate();
                        gate1.PortsOUT = new ArrayList();
                        gate1.PortsIN = new ArrayList();
                        foreach (Port portik in uzlik.portOut)
                        {
                            gate1.AddPortOUT(portik.X, portik.Y, portik.Name);
                        }
                        foreach (Port portik in uzlik.portIn)
                        {
                            gate1.AddPortIN(portik.X, portik.Y, portik.Name);
                        }
                        gate1.name = uzlik.name;
                        largeImage.Images.Add((Image)uzlik.Picture);
                        Gates.Add(gate1);
                    }
                }
            
            int i = 0;

            foreach (Gate gate in Gates)
            {
                listViewMain.Items.Add(new ListViewItem(gate.name));
                listViewMain.Items[i].ImageIndex = i;
               // MessageBox.Show(gate.name.ToString());
                i++;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {//pri naloadovani formu sa vykona:
            
            ModelType = "Logic";
            pictureBox.AllowDrop = true;
            listViewMain.View = View.LargeIcon;
            SetGates();
            hScrollBar.Maximum = pictureBox.Width;
            vScrollBar.Maximum = pictureBox.Height;
            DrawArea = new Bitmap(tabPage1.Width, tabPage1.Height);
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.DoubleBuffer |
          ControlStyles.UserPaint |
          ControlStyles.AllPaintingInWmPaint
          | ControlStyles.SupportsTransparentBackColor,
          true);
            this.UpdateStyles();
            //pictureBox.BackColor = Color.White;
            gpic1 = pictureBox.CreateGraphics();//nastavenie grafiky
            brushpic1 = new SolidBrush(Color.Blue);
            penpic1 = new Pen(brushpic1);

            ///////////////////////////////
            /* Image img = 
             this.btnImage.Image = img;
             this.picBox.AllowDrop = true;
             this.btnImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnImage_MouseDown);
             this.picBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.pictureBox_DragDrop);
             this.picBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.pictureBox_DragEnter);
             //////////////////////////////
             */
            this.Visible = false;
           
            // frmNew frm = new frmNew();
            //frm.ShowDialog();
        }


        /*////////////////////////////--\/--\/--\/--\/--//////////////////////////////////////////////*/

        private void toolConnect_Click(object sender, EventArgs e)
        {
            action = "connect";
            // toolAction.Text = action;
        }
  

        private void GetAllCons(PictureBox port, Connection conx)
        {

            //if (tmpCon.Contains(conx)) return;
            //if (tmpCon.Count >= Connections.Count) return;
            foreach (Connection con in Connections)
            {
                if (con.StartPortPic == port /*&& con != conx*/)
                {
                    //MessageBox.Show(port.Tag + " " + con.StartPortPic.Tag);
                    if (!tmpCon.Contains(con))
                    {
                        tmpCon.Add(con);
                        GetAllCons(con.EndPortPic, con);
                    }
                    else continue;
                }
                if (con.EndPortPic == port /*&& con != conx*/)
                {
                    if (!tmpCon.Contains(con))
                    {
                        tmpCon.Add(con);
                        GetAllCons(con.StartPortPic, con);
                    }
                    else continue;
                }
            }
        }

        public string PortName(NodeCtrl n, int port)
        {
            if (port > n.ConIN.Count)
                return (String)n.ConOut[port - 1 - n.ConIN.Count];
            else
                return (String)n.ConIN[port - 1];
        }

        public void RenamePorts()
        { //premenovanie portov
            foreach (Connection con in Connections)
            {
                
                if (con.StartPort <= con.StartNode.ConIN.Count)
                {
                    if (con.StartNode.ConIN.Contains(PortName(con.StartNode, con.StartPort)))
                        con.StartNode.ConIN[con.StartNode.ConIN.IndexOf(PortName(con.StartNode, con.StartPort))] = con.Name;
                }
                else
                {
                    if (con.StartNode.ConOut.Contains(PortName(con.StartNode, con.StartPort)))
                        con.StartNode.ConOut[con.StartNode.ConOut.IndexOf(PortName(con.StartNode, con.StartPort))] = con.Name;

                }

                if (con.EndPort <= con.EndNode.ConIN.Count)
                {
                    if (con.EndNode.ConIN.Contains(PortName(con.EndNode, con.EndPort)))
                        con.EndNode.ConIN[con.EndNode.ConIN.IndexOf(PortName(con.EndNode, con.EndPort))] = con.Name;
                }
                else
                {
                    if (con.EndNode.ConOut.Contains(PortName(con.EndNode, con.EndPort)))
                        con.EndNode.ConOut[con.EndNode.ConOut.IndexOf(PortName(con.EndNode, con.EndPort))] = con.Name;
                }
            }
        }

          bool CheckConnetions(string Name, PictureBox port)
        {
            ArrayList checkCons = new ArrayList();

            int checkConsNo = Connections.ToArray().Count(delegate(object cx) { return ((Connection)cx).Name == Name; });

            /*foreach (Connection con in Connections)
            {
                if (con.Name == Name) 
                    if (!checkCons.Contains(con)) checkCons.Add(con);
            }*/

            GetAllCons(port, null);

            //MessageBox.Show(tmpCon.Count.ToString() + " " + Connections.Count.ToString());

            if (tmpCon.Count < checkConsNo)
            {
                string newName = "";

                Connection con = (Connection)tmpCon[0];

                bool draw = true;
                while (draw)
                {
                    DialogResult dlg = InputBox("Upozornenie", "Rozdelili ste spojenie na dve časti. Jedna časť z nich musí byť premenovaná. Zadaj názov spojenia:", ref newName);

                    if (dlg == DialogResult.OK)
                    {
                        if (newName == "")
                        {
                            MessageBox.Show("Musíte zadať názov!", "Upozornenie");
                            continue;
                        }

                        con.Name = newName;
                        draw = false;
                        foreach (Connection c in Connections)
                        {
                            if (newName == c.Name && c != con)
                            {
                                /*if (((c.StartNode == con.StartNode && c.StartPort == con.StartPort) || 
                                    (c.StartNode == con.EndNode && c.StartPort == con.EndPort) ||
                                    (c.EndNode == con.StartNode && c.EndPort == con.StartPort) ||
                                    (c.EndNode == con.EndNode && c.EndPort == con.EndPort)) && ModelType == "Logic")
                                {*/
                                draw = true;
                                break;
                            }
                            else draw = false;

                        }



                        if (draw)
                        {
                            MessageBox.Show("Spojenie s rovnakým názvom už existuje. Zadajte nový.", "Chyba");
                            continue;
                        }
                        else
                        {
                            foreach (Connection cx in tmpCon)
                            {
                                cx.Name = con.Name;
                            }
                            RenamePorts();
                        }

                    
                    }
                    else
                    {
                        MessageBox.Show("Musíte zadať názov!", "Upozornenie");
                    }
                }
                return true;

            }
            else return false;

        }
        
        /*////////////////////////////--/\--/\--/\--/\--//////////////////////////////////////////////*/


        
// 4 XXX
        
        
        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listViewMain_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        


        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            pictureBox.Left = (-1) * hScrollBar.Value;
            PaintMain();
        }

        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            pictureBox.Top = (-1) * vScrollBar.Value;
            PaintMain();
        }

        public void RefreshListCons()
        {
            listConsMain.Items.Clear();
            bool exist = false;
            foreach (Connection con in Connections)
            {
                ListViewItem item = new ListViewItem(con.Name);
                item.Tag = con;
                exist = false;

                if (con.Type != "ARC")
                {
                    foreach (ListViewItem litem in listConsMain.Items)
                    {
                        if (litem.Text == con.Name) exist = true;
                    }
                }
                if (!exist)
                    listConsMain.Items.Add(item);
                //listConsMain.Items.Add(con.Name);
                //((ListViewItem)listConsMain.Items[listConsMain.Items.Count - 1]).Text = con.Name;
            }
            listConsMain.Refresh();
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            int x = System.Windows.Forms.Cursor.Position.X - this.Location.X - pictureBox.Left - 5 - modX;
            int y = System.Windows.Forms.Cursor.Position.Y - this.Location.Y - pictureBox.Top - 31 - modY;

            if (action == "delete") // co sa ma vykonat po stlaceni delete tlacidla
            {
                Connection conx = null;
                foreach (Connection con in Connections)
                {
                    penpic1.Width = 20;
                    if (con.Path.IsOutlineVisible(e.X, e.Y, penpic1))
                    {
                        conx = con;
                        break;
                    }

                }

                if (conx != null)
                { //zmazanie spojenia
                    conx.StartNode.Connections.Remove(conx);
                    conx.EndNode.Connections.Remove(conx);
                    Connections.Remove(conx);
                    pictureBox.Controls.Remove(conx.lblStart);
                    pictureBox.Controls.Remove(conx.lblEnd);
                    pictureBox.Controls.Remove(conx.lblMid);
                    tmpCon.Clear();
                    RefreshListCons();

                    int con1 = Connections.ToArray().Count(delegate(object cx) { return ((Connection)cx).EndPortPic == conx.EndPortPic; });
                    con1 += Connections.ToArray().Count(delegate(object cx) { return ((Connection)cx).StartPortPic == conx.EndPortPic; });
                    int con2 = Connections.ToArray().Count(delegate(object cx) { return ((Connection)cx).StartPortPic == conx.StartPortPic; });
                    con2 += Connections.ToArray().Count(delegate(object cx) { return ((Connection)cx).EndPortPic == conx.StartPortPic; });

                    if (ModelType == "Logic" && con1 != 0 && con2 != 0) CheckConnetions(conx.Name, conx.StartPortPic);

                    RefreshListCons(); // obnov zoznam prepojeni
                    RefreshConLabels(); //obnov labely nad spojeniami


                    PaintMain();

                }
            }
            foreach (NodeCtrl node in Nodes)
            {
                node.BackColor = Color.Transparent;
            }

            mouseDown = true;
        }


        public void RefreshConLabels()
        {
            foreach (Connection con in Connections)
            {
                con.lblEnd.Text = con.Name;
                con.lblStart.Text = con.Name;
                //con.lblEnd.Text = PortName(con.EndNode, (int)con.EndPortPic.Tag);
                //con.lblStart.Text = PortName(con.StartNode, (int)con.StartPortPic.Tag);
            }
        }

        public void StartConnection(NodeCtrl n, int port, string type, PictureBox pic)
        { //vytvorenie noveho prepojenia so zaciatocnym nodom
            Connection con = new Connection(); //nove prepojenie
            con.StartNode = n; // nastav pociatocny uzol
            // YYY vystrihnute //if (ModelType == "Petri" || ModelType == "FSM") type = "ARC";
            con.Type = type; // typ prepojenia
            con.StartPortPic = pic; // obrazok zac. portu
            con.StartPort = port; //zac. port
            active_object = con; // s tym to objektom budeme pracovat
            StartCon = true;
        }

        public void Load_StartConnection(Connection con, NodeCtrl n, int port, string type, PictureBox pic) {
            con.Type = type;
            con.StartNode = n;
            con.StartPortPic = pic;
            con.StartPort = port;
            active_object = con;
            StartCon = true;
        }

        public void StartConnection(NodeCtrl n, int port, string type)
        {
            Connection con = new Connection();
            con.StartNode = n; // nastav pociatocny uzol

            if (ModelType == "Petri" || ModelType == "FSM")
            {
                type = "ARC";

                foreach (PictureBox portx in con.StartNode.Controls)
                {
                    if (portx.Tag.ToString() == port.ToString())
                    {
                        con.StartPortPic = portx;
                        break;
                    }
                }

            }
            con.Type = type;
            con.StartPort = port;
            active_object = con;
            StartCon = true;
        }


        public void EndConnection(NodeCtrl n, int port, string name)
        {
            if (StartCon == true) //ak existuje zaciatok spojenia:
            {
                Connection con = (Connection)active_object; // nastav aktualne prepojenie na aktivne - s nim budeme pracovat
                ConCount++;
                //con.Name = "con" + ConCount;
                con.Name = name; // asi nazov prepojenia
                con.EndNode = n; // nastav konecny uzol
                foreach (PictureBox portx in con.EndNode.Controls)
                {
                    if (portx.Tag.ToString() == port.ToString())
                    {
                        con.EndPortPic = portx;
                        break;
                    }
                }

                con.EndPort = port; //nastav koncovy port
                //con.Points = new ArrayList();

                int SportY = con.StartNode.Top + con.StartNode.Controls[con.StartPort - 1].Top + 2;
                int SportX = con.StartNode.Left + con.StartNode.Controls[con.StartPort - 1].Left;

                int EportY = con.EndNode.Top + con.EndNode.Controls[con.EndPort - 1].Top + 2;
                int EportX = con.EndNode.Left + con.EndNode.Controls[con.EndPort - 1].Left;

                con.Start = new Point(SportX, SportY); //nastavenie suradnic zaciatocneho portu
                con.End = new Point(EportX, EportY); // nastavenie suradnic koncoveho portu

                Point tmp_pt1 = new Point();
                Point tmp_pt2 = new Point();

                //MessageBox.Show(con.ToString());

                con.Points.Add(con.Start);
                if (!con.Type.Equals("ARC"))
                {
                    con.PointMove.Add(con.Start);
                    if (con.Start.X < con.End.X)
                        tmp_pt1.X = Math.Abs(con.Start.X - con.End.X) / 2 + con.Start.X;
                    else
                        tmp_pt1.X = Math.Abs(con.Start.X - con.End.X) / 2 + con.End.X;
                    tmp_pt1.Y = con.Start.Y;
                    con.Points.Add(tmp_pt1);
                    con.PointMove.Add(tmp_pt1);
                    tmp_pt2.X = tmp_pt1.X;
                    tmp_pt2.Y = con.End.Y;
                    con.Points.Add(tmp_pt2);
                    con.Points.Add(con.End);
                    con.PointMove.Add(tmp_pt2);
                } // poziadat o vysvetlenie /\

                if (con.Type == "ARC")
                {
                    Random rand = new Random(System.DateTime.Now.Millisecond);
                    con.Mid = new Point(Math.Abs(con.Start.X - con.End.X), Math.Abs(con.Start.Y - con.End.Y));
                    if (con.Start.X > con.End.X) con.MidX = con.End.X + con.Mid.X / 2 + rand.Next(-50, 50);
                    else con.MidX = con.Start.X + con.Mid.X / 2 + rand.Next(-50, 50);
                    if (con.Start.Y > con.End.Y) con.MidY = con.End.Y + con.Mid.Y / 2 + rand.Next(-50, 50);
                    else con.MidY = con.Start.Y + con.Mid.Y / 2 + rand.Next(-50, 50);
                    con.Points.Add(con.Mid);
                }
                con.PointMove.Add(con.End);

                con.StartNode.Connections.Add(con);
                if (con.StartNode != con.EndNode)
                    con.EndNode.Connections.Add(con);

                /*string value = con.Name;

                if (SetName)
                {
                    if (InputBox("Spoj", "Zadaj názov spojenia", ref value) == DialogResult.OK)
                        con.Name = value;
                }*/
                if (con.Type != "ARC")
                {
                    con.lblEnd.Text = con.Name;
                    pictureBox.Controls.Add(con.lblEnd);
                    con.lblStart.Text = con.Name;
                    pictureBox.Controls.Add(con.lblStart);
                }
                else
                {
                    con.lblMid.Text = con.Name;
                    pictureBox.Controls.Add(con.lblMid);
                }
                con.SetLabelPos();
                Connections.Add(con);
                StartCon = false;
                RefreshConLabels();
            }
        }

      public void Load_EndConnection(Connection con, NodeCtrl n, int port, string name){
          if (StartCon == true) //ak existuje zaciatok spojenia:
          {
              //Connection con = (Connection)active_object; // nastav aktualne prepojenie na aktivne - s nim budeme pracovat
              ConCount++;
              //con.Name = "con" + ConCount;
              con.Name = name; // asi nazov prepojenia
              con.EndNode = n; // nastav konecny uzol
              foreach (PictureBox portx in con.EndNode.Controls)
              {
                  if (portx.Tag.ToString() == port.ToString())
                  {
                      con.EndPortPic = portx;
                      break;
                  }
              }

              con.EndPort = port; //nastav koncovy port
              //con.Points = new ArrayList();

              int SportY = con.StartNode.Top + con.StartNode.Controls[con.StartPort - 1].Top + 2;
              int SportX = con.StartNode.Left + con.StartNode.Controls[con.StartPort - 1].Left;

              int EportY = con.EndNode.Top + con.EndNode.Controls[con.EndPort - 1].Top + 2;
              int EportX = con.EndNode.Left + con.EndNode.Controls[con.EndPort - 1].Left;

              con.Start = new Point(SportX, SportY); //nastavenie suradnic zaciatocneho portu
              con.End = new Point(EportX, EportY); // nastavenie suradnic koncoveho portu

              Point tmp_pt1 = new Point();
              Point tmp_pt2 = new Point();

              //MessageBox.Show(con.ToString());

              con.Points.Add(con.Start);
              if (!con.Type.Equals("ARC"))
              {
                  con.PointMove.Add(con.Start);
                  if (con.Start.X < con.End.X)
                      tmp_pt1.X = Math.Abs(con.Start.X - con.End.X) / 2 + con.Start.X;
                  else
                      tmp_pt1.X = Math.Abs(con.Start.X - con.End.X) / 2 + con.End.X;
                  tmp_pt1.Y = con.Start.Y;
                  con.Points.Add(tmp_pt1);
                  con.PointMove.Add(tmp_pt1);
                  tmp_pt2.X = tmp_pt1.X;
                  tmp_pt2.Y = con.End.Y;
                  con.Points.Add(tmp_pt2);
                  con.Points.Add(con.End);
                  con.PointMove.Add(tmp_pt2);
              } // poziadat o vysvetlenie /\

              if (con.Type == "ARC")
              {
                  Random rand = new Random(System.DateTime.Now.Millisecond);
                  con.Mid = new Point(Math.Abs(con.Start.X - con.End.X), Math.Abs(con.Start.Y - con.End.Y));
                  if (con.Start.X > con.End.X) con.MidX = con.End.X + con.Mid.X / 2 + rand.Next(-50, 50);
                  else con.MidX = con.Start.X + con.Mid.X / 2 + rand.Next(-50, 50);
                  if (con.Start.Y > con.End.Y) con.MidY = con.End.Y + con.Mid.Y / 2 + rand.Next(-50, 50);
                  else con.MidY = con.Start.Y + con.Mid.Y / 2 + rand.Next(-50, 50);
                  con.Points.Add(con.Mid);
              }
              con.PointMove.Add(con.End);

              con.StartNode.Connections.Add(con);
              if (con.StartNode != con.EndNode)
                  con.EndNode.Connections.Add(con);

              /*string value = con.Name;

              if (SetName)
              {
                  if (InputBox("Spoj", "Zadaj názov spojenia", ref value) == DialogResult.OK)
                      con.Name = value;
              }*/
              //if (con.Type != "ARC")
              //{
                  con.lblEnd.Text = con.Name;
                  pictureBox.Controls.Add(con.lblEnd);
                  con.lblStart.Text = con.Name;
                  pictureBox.Controls.Add(con.lblStart);
              //}
              /*else
              {
                  con.lblMid.Text = con.Name;
                  pictureBox.Controls.Add(con.lblMid);
              }*/
              con.SetLabelPos();
              //Connections.Add(con);
              StartCon = false;
              RefreshConLabels();
          }
        }


        public void Load_EndConnection(Connection con,NodeCtrl n, int port, bool SetName, PictureBox pic) {
            //ukoncenie spojenia v pictureboxe
            if (StartCon == true)
            {
                //Connection con = (Connection)active_object;
                string ConNameTmp = "";
                ConCount++;
               // con.Name = "con" + ConCount;
              //  con.Name = PortName(con.StartNode, con.StartPort);

                string value = con.Name;

                con.EndNode = n;
                con.EndPort = port;
                con.EndPortPic = pic;
                //if (ModelType == "Logic")
               // {
                    foreach (Connection c in Connections)
                    {
                        if (c.EndPortPic == pic || c.StartPortPic == pic || c.EndPortPic == con.StartPortPic || c.StartPortPic == con.StartPortPic)
                        {
                            ConNameTmp = con.Name;
                            con.Name = c.Name;
                            SetName = false;
                        }
                    }
                //}
                    
                foreach (Connection c in Connections)
                {
                    if (c.Name == ConNameTmp) c.Name = con.Name;
                }

                if (SetName)
                {
                    //bool draw = true;
                    //while (draw)
                    //{
                    //    DialogResult dlg = InputBox("Spoj", "Zadaj názov spojenia", ref value);

                        /*if (dlg == DialogResult.OK)
                        {
                            if (value == "")
                            {
                                MessageBox.Show("Musíte zadať názov!", "Upozornenie");
                                continue;
                            }

                            con.Name = value;

                            draw = false;
                    */
                            /*if (con.Type != "ARC")
                            {
                                foreach (Connection c in Connections)
                                {
                                    if (value == c.Name)
                                    {
                                        if (((c.StartNode == con.StartNode && c.StartPort == con.StartPort) ||
                                            (c.StartNode == con.EndNode && c.StartPort == con.EndPort) ||
                                            (c.EndNode == con.StartNode && c.EndPort == con.StartPort) ||
                                            (c.EndNode == con.EndNode && c.EndPort == con.EndPort)) && ModelType == "Logic")
                                        {
                                            draw = false;
                                            break;
                                        }
                                        else draw = true;
                                    }
                                }

                                if (draw)
                                {
                                    MessageBox.Show("Spojenie s rovnakým názvom už existuje. Zadajte nový.", "Chyba");
                                    continue;
                                }
                            }*/
                        
                            if (port > con.EndNode.ConIN.Count)
                            {
                                con.EndNode.ConOut[port - con.EndNode.ConIN.Count - 1] = con.Name;
                            }
                            else
                            {
                                con.EndNode.ConIN[port - 1] = con.Name;
                            }
                            if (con.StartPort > con.StartNode.ConIN.Count)
                            {
                                con.StartNode.ConOut[con.StartPort - con.StartNode.ConIN.Count - 1] = con.Name;
                            }
                            else
                            {
                                con.StartNode.ConIN[con.StartPort - 1] = con.Name;
                            }
                       /* }
                        else
                        {
                            StartCon = false;
                            return;
                        }*/
                    //}

                }

                con.Start = new Point(con.StartNode.Left + con.StartPortPic.Left, con.StartNode.Top + con.StartPortPic.Top);
                con.End = new Point(con.EndNode.Left + con.EndPortPic.Left, con.EndNode.Top + con.EndPortPic.Top);


                Point tmp_pt1 = new Point();
                Point tmp_pt2 = new Point();

                //MessageBox.Show(con.ToString());

                con.Points.Add(con.Start);
                if (con.Type != "ARC")
                {
                    if (con.Start.X < con.End.X)
                        tmp_pt1.X = Math.Abs(con.Start.X - con.End.X) / 2 + con.Start.X;
                    else
                        tmp_pt1.X = Math.Abs(con.Start.X - con.End.X) / 2 + con.End.X;
                    tmp_pt1.Y = con.Start.Y;
                    con.Points.Add(tmp_pt1);
                    tmp_pt2.X = tmp_pt1.X;
                    tmp_pt2.Y = con.End.Y;
                    con.Points.Add(tmp_pt2);
                }
                MessageBox.Show("OK");
                if (con.Type == "ARC")
                /*{
                    Random rand = new Random(System.DateTime.Now.Millisecond);
                    con.Mid = new Point(Math.Abs(con.Start.X - con.End.X), Math.Abs(con.Start.Y - con.End.Y));
                    if (con.Start.X > con.End.X) con.MidX = con.End.X + con.Mid.X / 2 + rand.Next(-50, 50);
                    else con.MidX = con.Start.X + con.Mid.X / 2 + rand.Next(-50, 50);
                    if (con.Start.Y > con.End.Y) con.MidY = con.End.Y + con.Mid.Y / 2 + rand.Next(-50, 50);
                    else con.MidY = con.Start.Y + con.Mid.Y / 2 + rand.Next(-50, 50);
                    con.Points.Add(con.Mid);
                }*/
                con.Points.Add(con.End);

                con.StartNode.Connections.Add(con);
                if (con.StartNode != con.EndNode)
                    con.EndNode.Connections.Add(con);

                if (con.Type != "ARC")
                {
                    con.lblEnd.Text = con.Name;
                    pictureBox.Controls.Add(con.lblEnd);
                    con.lblStart.Text = con.Name;
                    pictureBox.Controls.Add(con.lblStart);
                }
                else
                {
                    con.lblMid.Text = con.Name;
                    pictureBox.Controls.Add(con.lblMid);
                }
                con.SetLabelPos();
                //Connections.Add(con);
                StartCon = false;
               // RenamePorts();
                RefreshListCons();
                RefreshConLabels();

            }
        
        }
        
        
        public void EndConnection(NodeCtrl n, int port, bool SetName, PictureBox pic)
        { //ukoncenie spojenia v pictureboxe
            if (StartCon == true)
            {
                Connection con = (Connection)active_object;
                string ConNameTmp = "";
                ConCount++;
                con.Name = "con" + ConCount;
                con.Name = PortName(con.StartNode, con.StartPort);

                string value = con.Name;

                con.EndNode = n;
                con.EndPort = port;
                con.EndPortPic = pic;
                if (ModelType == "Logic")
                {
                    foreach (Connection c in Connections)
                    {
                        if (c.EndPortPic == pic || c.StartPortPic == pic || c.EndPortPic == con.StartPortPic || c.StartPortPic == con.StartPortPic)
                        {
                            ConNameTmp = con.Name;
                            con.Name = c.Name;
                            SetName = false;
                        }
                    }
                }

                foreach (Connection c in Connections)
                {
                    if (c.Name == ConNameTmp) c.Name = con.Name;
                }

                if (SetName)
                {
                    bool draw = true;
                    while (draw)
                    {
                        DialogResult dlg = InputBox("Spoj", "Zadaj názov spojenia", ref value);

                        if (dlg == DialogResult.OK)
                        {
                            if (value == "")
                            {
                                MessageBox.Show("Musíte zadať názov!", "Upozornenie");
                                continue;
                            }

                            con.Name = value;

                            draw = false;

                            if (con.Type != "ARC")
                            {
                                foreach (Connection c in Connections)
                                {
                                    if (value == c.Name)
                                    {
                                        if (((c.StartNode == con.StartNode && c.StartPort == con.StartPort) ||
                                            (c.StartNode == con.EndNode && c.StartPort == con.EndPort) ||
                                            (c.EndNode == con.StartNode && c.EndPort == con.StartPort) ||
                                            (c.EndNode == con.EndNode && c.EndPort == con.EndPort)) && ModelType == "Logic")
                                        {
                                            draw = false;
                                            break;
                                        }
                                        else draw = true;
                                    }
                                }

                                if (draw)
                                {
                                    MessageBox.Show("Spojenie s rovnakým názvom už existuje. Zadajte nový.", "Chyba");
                                    continue;
                                }
                            }

                            if (port > con.EndNode.ConIN.Count)
                            {
                                con.EndNode.ConOut[port - con.EndNode.ConIN.Count - 1] = con.Name;
                            }
                            else
                            {
                                con.EndNode.ConIN[port - 1] = con.Name;
                            }
                            if (con.StartPort > con.StartNode.ConIN.Count)
                            {
                                con.StartNode.ConOut[con.StartPort - con.StartNode.ConIN.Count - 1] = con.Name;
                            }
                            else
                            {
                                con.StartNode.ConIN[con.StartPort - 1] = con.Name;
                            }
                        }
                        else
                        {
                            StartCon = false;
                            return;
                        }
                    }

                }

                if (((con.StartNode.Type != "TRANSITION" && con.EndNode.Type != "TRANSITION") || (con.StartNode.Type == "TRANSITION" && con.EndNode.Type == "TRANSITION")) && ModelType == "Petri")
                {
                    MessageBox.Show("Spojenie môže byť len medzi miestom a prechodom!", "Chyba");
                    StartCon = false;
                    return;
                }

                con.Start = new Point(con.StartNode.Left + con.StartPortPic.Left, con.StartNode.Top + con.StartPortPic.Top);
                con.End = new Point(con.EndNode.Left + con.EndPortPic.Left, con.EndNode.Top + con.EndPortPic.Top);


                Point tmp_pt1 = new Point();
                Point tmp_pt2 = new Point();

                //MessageBox.Show(con.ToString());

                con.Points.Add(con.Start);
                if (con.Type != "ARC")
                {
                    if (con.Start.X < con.End.X)
                        tmp_pt1.X = Math.Abs(con.Start.X - con.End.X) / 2 + con.Start.X;
                    else
                        tmp_pt1.X = Math.Abs(con.Start.X - con.End.X) / 2 + con.End.X;
                    tmp_pt1.Y = con.Start.Y;
                    con.Points.Add(tmp_pt1);
                    tmp_pt2.X = tmp_pt1.X;
                    tmp_pt2.Y = con.End.Y;
                    con.Points.Add(tmp_pt2);
                }

                if (con.Type == "ARC")
                {
                    Random rand = new Random(System.DateTime.Now.Millisecond);
                    con.Mid = new Point(Math.Abs(con.Start.X - con.End.X), Math.Abs(con.Start.Y - con.End.Y));
                    if (con.Start.X > con.End.X) con.MidX = con.End.X + con.Mid.X / 2 + rand.Next(-50, 50);
                    else con.MidX = con.Start.X + con.Mid.X / 2 + rand.Next(-50, 50);
                    if (con.Start.Y > con.End.Y) con.MidY = con.End.Y + con.Mid.Y / 2 + rand.Next(-50, 50);
                    else con.MidY = con.Start.Y + con.Mid.Y / 2 + rand.Next(-50, 50);
                    con.Points.Add(con.Mid);
                }
                con.Points.Add(con.End);

                con.StartNode.Connections.Add(con);
                if (con.StartNode != con.EndNode)
                    con.EndNode.Connections.Add(con);

                if (con.Type != "ARC")
                {
                    con.lblEnd.Text = con.Name;
                    pictureBox.Controls.Add(con.lblEnd);
                    con.lblStart.Text = con.Name;
                    pictureBox.Controls.Add(con.lblStart);
                }
                else
                {
                    con.lblMid.Text = con.Name;
                    pictureBox.Controls.Add(con.lblMid);
                }
                con.SetLabelPos();
                Connections.Add(con);
                StartCon = false;
                RenamePorts();
                RefreshListCons();
                RefreshConLabels();

            }
        }


        private void node_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (NodeCtrl node in Nodes)
            {
                node.BackColor = Color.Transparent;
                foreach (Connection con in node.Connections)
                {
                    con.ConWidth = 1;
                }
                //node.BackColor = Color.White;
            }

            foreach (Connection con in ((NodeCtrl)sender).Connections)
            {
                con.ConWidth = 3;
            }

            ((NodeCtrl)sender).BackColor = Color.SlateGray;
            mouseDown = true;
        }

        private void node_MouseMove(object sender, MouseEventArgs e)
        {
            int x = System.Windows.Forms.Cursor.Position.X - this.Location.X - pictureBox.Left - 5 - modX;
            int y = System.Windows.Forms.Cursor.Position.Y - this.Location.Y - pictureBox.Top - 31 - modY;
            //lblCoord.Text = "Pozicia: " + "X: " + x.ToString() + " Y: " + y.ToString(); //zobrazenie pozicie pri pohybe mysou

            if (/*action == "movenode" &&*/ mouseDown == true && e.Button == MouseButtons.Left)
            { // asi prenasanie nodu po ploche pictureboxu

                NodeCtrl n = (NodeCtrl)sender;

                Cursor = Cursors.SizeAll;

                foreach (Connection con in Connections)
                {
                    if (con.StartNode == n || con.EndNode == n)
                    {

                        con.Start = new Point(con.StartNode.Left + con.StartPortPic.Left, con.StartNode.Top + con.StartPortPic.Top + 2);
                        con.End = new Point(con.EndNode.Left + con.EndPortPic.Left, con.EndNode.Top + con.EndPortPic.Top + 2);

                        if (!con.Type.Equals("ARC"))
                        {
                            con.Points[0] = con.Start;
                            con.Points[1] = new Point(((Point)con.Points[1]).X, con.Start.Y);
                            con.Points[con.Points.Count - 1] = con.End;
                            con.Points[con.Points.Count - 2] = new Point(((Point)con.Points[con.Points.Count - 2]).X, con.End.Y);
                        }
                        else
                        {
                            try
                            {
                                con.Points[0] = con.Start;
                                con.Points[2] = con.End;
                            }
                            catch
                            {
                            }
                        }
                        con.SetLabelPos();
                        continue;
                    }
                }
                n.Top = y;
                n.Left = x;
                PaintMain(); // po presune updatenutie pictureboxu
                return;
                //}
            }

        }

        private void SetCursor()
        { //nastavenie vzhladu kurzora pri roznych akciach
            Cursor = Cursors.Default;
            if (action == "movenode")
            {
                Cursor = Cursors.SizeAll;
            }
            else if (action == "connect")
            {
                Cursor = Cursors.Cross;
            }
            else if (action == "delete")
            {
                Cursor = Cursors.No;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }



        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {

            try
            {
                SetCursor();

                int x = System.Windows.Forms.Cursor.Position.X - this.Location.X - pictureBox.Left - 5 - modX;
                int y = System.Windows.Forms.Cursor.Position.Y - this.Location.Y - pictureBox.Top - 31 - modY;
                //lblCoord.Text = "X: " + x.ToString() + " Y: " + y.ToString();
                
                if (tip != null)
                {
                    //tip.Active = true;
                    //MessageBox.Show(tip.Active.ToString());
                }

                brushpic1.Color = Color.Black;
                //penpic1.Brush = brushpic1;
                penpic1.Width = 1;
                //if (action == "connect") MessageBox.Show(StartCon.ToString());
                //if (action == "connect") MessageBox.Show(mouseDown.ToString());
                if (action == "connect" && StartCon == true)
                {// po stlaceni tlacidla pripojenie a je kliknute na zaciatocne pripojenie rob:
                    //MessageBox.Show("kresli");
                    //pictureBox.Update();

                    int portY = ((Connection)active_object).StartNode.Top + ((Connection)active_object).StartNode.Controls[((Connection)active_object).StartPort - 1].Top;
                    int portX = ((Connection)active_object).StartNode.Left + ((Connection)active_object).StartNode.Controls[((Connection)active_object).StartPort - 1].Left;
           
                   
                    PaintMain();
                    
                    gpic1.DrawLine(penpic1, portX, portY, x, y); // kresli prepojeni
                    //g.Dispose();

                    return;
                   

                }

                if (action2 == "movecon" && mouseDown == true && con_move.Type != "ARC")
                { //nastavenie what to do ak sa hybe prepojenie
              
                    if (direction == "up")
                    {
                        con_move.Points[con_move_pt] = new Point(((Point)con_move.Points[con_move_pt]).X, y);
                        con_move.Points[con_move_pt - 1] = new Point(((Point)con_move.Points[con_move_pt - 1]).X, y);
                     }
                    else if (direction == "left")
                    {
                        con_move.Points[con_move_pt] = new Point(x, ((Point)con_move.Points[con_move_pt]).Y);
                        con_move.Points[con_move_pt - 1] = new Point(x, ((Point)con_move.Points[con_move_pt - 1]).Y);
                     }
                    con_move.kresli = true;
                    PaintMain();
                    return;
                }
                else if (action2 == "movepoint" && mouseDown == true && con_move.Type == "ARC")
                {
                    con_move.MidX = x;
                    con_move.MidY = y;
                    con_move.Points[1] = con_move.Mid;
                    con_move.SetLabelPos();
                    PaintMain();
                    return;
                }

                foreach (Connection con in Connections)
                {
                    penpic1.Width = 20;
                    if (con.Path.IsOutlineVisible(x, y, penpic1))
                    {
                        //nastavenie informacii o danom spojeni v grupe dole (info)
                        //lblStartNode.Text = con.StartNode.Text;
                        //lblEndNode.Text = con.EndNode.Text;
                        //lblNameCon.Text = con.Name;

                        groupCon.Visible = true;
                        //MessageBox.Show(con.Name + " vnutri");
                        //toolTipMain.Dispose();
                        //tip = new ToolTip();
                        //tip.AutomaticDelay = 0;
                        tip.AutoPopDelay = 200;
                        tip.InitialDelay = 1000;
                        tip.ShowAlways = true;
                        //tip.Active = false;
                        string start = "";
                        if (con.StartPort > con.StartNode.ConIN.Count)
                            start = con.StartNode.ConOut[0].ToString();
                        else start = con.StartNode.ConIN[con.StartPort - 1].ToString();

                        int nasiel_pt = 0;
                        if (mouseDown == false && con.Type != "ARC")
                            foreach (Point point in con.Points)
                            {

                                if (point.X >= x - 3 && point.X <= x + 3)
                                {
                                    this.Cursor = Cursors.SizeWE;
                                    direction = "left";
                                    nasiel_pt++;
                                }
                                if (point.Y == y)
                                {
                                    this.Cursor = Cursors.SizeNS;
                                    direction = "up";
                                    nasiel_pt++;
                                }
                                if (nasiel_pt == 2)
                                {
                                    action2 = "movecon";
                                    con_move = con;
                                    con_move_pt = con.Points.IndexOf(point);
                                    //MessageBox.Show(con.Points.IndexOf(point).ToString());
                                    break;
                                }
                            }
                        else if (mouseDown == false && con.Type == "ARC")
                            if (con.Mid.X >= x - 10 && con.Mid.X <= x + 10 && con.Mid.Y >= y - 10 && con.Mid.Y <= y + 10)
                            {
                                this.Cursor = Cursors.SizeAll;
                                action2 = "movepoint";
                                con_move = con;
                            }
                        return;
                    }
                }
                SetCursor();
                groupCon.Visible = false;
                //groupNode.Visible = false;
                //groupPort.Visible = false;
            }
            catch
            {
            }
            //toolTipMain.AutoPopDelay = 1000000;
        }


        public void setGroupNode(string Name, int numIn, int numOut, string Type)
        { //zobrazenie v gruptexte dole pri pohybe mysou nad nodom
            //lblNodeType.Text = Type;
            //lblNameNode.Text = Name;
           // lblPortIn.Text = numIn.ToString();
            //lblPortOut.Text = numOut.ToString();
            //groupNode.Visible = true;
        }

        public void setGroupPort(string Name, int numCons)
        { //zobrazenie v gruptexte dole pri pohybe mysou nad portom

            //lblNamePort.Text = Name;
            //lblPortCons.Text = numCons.ToString();
            //groupPort.Visible = true;
        }

        private void node_MouseUp(object sender, MouseEventArgs e)
        {
            SetCursor();
            mouseDown = false;
            PaintMain();
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        { //po uvolneni tlacidla nad pictureboxom
            //nastav suradnice
            int x = System.Windows.Forms.Cursor.Position.X - this.Location.X - pictureBox.Left - 5 - modX;
            int y = System.Windows.Forms.Cursor.Position.Y - this.Location.Y - pictureBox.Top - 31 - modY;

            foreach (Connection con in Connections)
            {
                int SportY = con.StartNode.Top + con.StartNode.Controls[con.StartPort - 1].Top + 2;
                int SportX = con.StartNode.Left + con.StartNode.Controls[con.StartPort - 1].Left;

                int EportY = con.EndNode.Top + con.EndNode.Controls[con.EndPort - 1].Top + 2;
                int EportX = con.EndNode.Left + con.EndNode.Controls[con.EndPort - 1].Left;

                con.Start = new Point(SportX, SportY);
                con.End = new Point(EportX, EportY);
                
                if (!con.Type.Equals("ARC"))
                {
                    con.Points[0] = con.Start;
                    con.Points[1] = new Point(((Point)con.Points[1]).X, con.Start.Y);
                    con.Points[con.Points.Count - 1] = con.End;
                    con.Points[con.Points.Count - 2] = new Point(((Point)con.Points[con.Points.Count - 2]).X, con.End.Y);
                }
                else
                {
                    try
                    {
                        con.Points[0] = con.Start;
                        con.Points[2] = con.End;
                    }
                    catch
                    {
                    }
                }
            }

            /*if (action == "connect" && StartCon == true)
            {
                Graphics g = Graphics.FromImage(DrawArea);
                SolidBrush brush = new SolidBrush(Color.Black);
                Pen pen = new Pen(brush);
                ((Connection)active_object).End = new Point(x, y);
                g.DrawLine(pen, ((Connection)active_object).Start, ((Connection)active_object).End);
                //GraphicsPath path = new GraphicsPath(
                g.Dispose();

                ((Connection)active_object).Name = "con" + Connections.Count;

                Connections.Add(active_object);

            }*/
            //MessageBox.Show(((Connection)active_object).End.ToString());
            //this.Refresh();
            action2 = "";
            mouseDown = false;
            PaintMain();
            //pictureBox.Refresh();
        }



        public void PaintMain()
        {
            pictureBox_Paint(pictureBox, new PaintEventArgs(pictureBox.CreateGraphics(), new Rectangle(0, 0, pictureBox.Width, pictureBox.Height)));
            pictureBox.Invalidate(false);
            pictureBox.Update();
      }

        private void PaintMain(Rectangle rect)
        {
            pictureBox.Invalidate(rect, false);
           pictureBox.Update();
            //pictureBox1_Paint(MainSurface1, new PaintEventArgs(MainSurface1.CreateGraphics(), new Rectangle(0, 0, MainSurface1.Width, MainSurface1.Height)));
           pictureBox_Paint(pictureBox, new PaintEventArgs(gpic1, new Rectangle(0, 0, pictureBox.Width, pictureBox.Height)));
        }

        private void listViewMain_MouseDown(object sender, MouseEventArgs e)
        {
            Point p = listViewMain.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));
            ListViewItem item = listViewMain.GetItemAt(p.X, p.Y);
            //MessageBox.Show(item.Text);
            if (item != null)
            {
                DoDragDrop(item.ImageIndex, DragDropEffects.Copy);
            }
            
        }

        private void newToolNew_Click(object sender, EventArgs e)
        {
            //pictureBox.Image = null;
        }

        private void pictureBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void pictureBox_DragDrop(object sender, DragEventArgs e)
        { // pretiahnutie hradla zo zoznamu hradiel do  pictureboxu
            tmp_action = action;
            action = "addnode";
            active_gate = (Gate)Gates[(int)e.Data.GetData(typeof(int))];
           
            pictureBox_Click(sender, new EventArgs());
        }



        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            try
            {

                Graphics g = e.Graphics;

                brushpic1.Color = Color.Black;

                foreach (Connection con in Connections)
                {
                    penpic1.Width = con.ConWidth;

                    con.Start = new Point(con.StartNode.Left + con.StartPortPic.Left, con.StartNode.Top + con.StartPortPic.Top);
                    con.End = new Point(con.EndNode.Left + con.EndPortPic.Left, con.EndNode.Top + con.EndPortPic.Top);

                    con.RefreshPath();
                    g.DrawPath(penpic1, con.Path);
                    brushpic1.Color = Color.Black;
                    if (con.Type == "ARC") g.FillPie(brushpic1, con.End.X - 20, con.End.Y - 20, 40, 40, con.startAngle, 45);
                    //g.FillPath(brushpic1, con.Path);
                    brushpic1.Color = Color.Blue;
                    foreach (Point pt in con.Points)
                    {
                        if (pt != con.Start && pt != con.End)
                        {
                            g.FillEllipse(brushpic1, pt.X - 4, pt.Y - 4, 8, 8);
                        }
                    }

                }
            }
            catch
            {
                return;
            }
        }


        int STARTX;
        int STARTY;
        int ENDX;
        int ENDY;
        int ciarka = 0;

        public void PaintMainMove()
        {
            pictureBox.Update();
            pictureBox_Paint(pictureBox, new PaintEventArgs(gpic1, new Rectangle(0, 0, pictureBox.Width, pictureBox.Height)));
        }

        private void node_Click(object sender, EventArgs e)
        {
            StartCon = false;
            //pictureBox.Refresh();

            if (action == "delete")
            {
                active_object = (NodeCtrl)sender;
                ArrayList cons = new ArrayList();
                foreach (Connection con in Connections)
                {
                    if (con.StartNode == active_object || con.EndNode == active_object)
                    {
                        cons.Add(con);
                        pictureBox.Controls.Remove(con.lblStart);
                        pictureBox.Controls.Remove(con.lblEnd);
                    }
                }
                /*foreach (Connection con in cons)
                {
                    Connections.Remove(con);
                }*/



                foreach (Connection conx in ((NodeCtrl)active_object).Connections)
                {
                    tmpCon.Clear();
                    Connections.Remove(conx);

                    int con1 = Connections.ToArray().Count(delegate(object cx) { return ((Connection)cx).EndPortPic == conx.EndPortPic; });
                    con1 += Connections.ToArray().Count(delegate(object cx) { return ((Connection)cx).StartPortPic == conx.EndPortPic; });
                    int con2 = Connections.ToArray().Count(delegate(object cx) { return ((Connection)cx).StartPortPic == conx.StartPortPic; });
                    con2 += Connections.ToArray().Count(delegate(object cx) { return ((Connection)cx).EndPortPic == conx.StartPortPic; });

                    if (ModelType == "Logic" && con1 != 0 && con2 != 0) CheckConnetions(conx.Name, conx.StartPortPic);
                }

                RefreshConLabels();
                RefreshListCons();

                pictureBox.Controls.Remove((NodeCtrl)sender);

                Nodes.Remove(active_object);
                PaintMain();
                //pictureBox.Invalidate();
                RefreshListNodes();
                return;

            }
        }
        private void node_DoubleClick(object sender, EventArgs e)
        {
           /* if (ModelType == "Logic")
            {
                frmNode node_form = new frmNode();
                node_form.node = (NodeCtrl)sender;
                node_form.PortIn = ((NodeCtrl)sender).ConIN;
                node_form.PortOut = ((NodeCtrl)sender).ConOut;
                node_form.NodeName = ((NodeCtrl)sender).Text;
                node_form.SetConnections = Connections;
                node_form.Show();
            }
            */
            /*else if (ModelType == "FSM")
            {
                frmState frm_state = new frmState();
                frm_state.node = (NodeCtrl)sender;
                frm_state.Show();
            }
            else if (ModelType == "Petri")
            {
                frmState frm_state = new frmState();
                frm_state.node = (NodeCtrl)sender;
                if (((NodeCtrl)sender).Type == "TRANSITION")
                    frm_state.SetName = "Názov prechodu";
                else
                    frm_state.SetName = "Názov miesta";
                frm_state.Show();
            }
             */
        }
        int xy = 0;
        string vypis;
        string nieco;
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

        //XXX nasa funkcia
        public void LoadNodes(NodeCtrl node)
        {

            string value = node.Text;
            node.ShownText = active_gate.name;
            SetNodeType(node);
            node.MouseDown += new MouseEventHandler(node_MouseDown);
            node.MouseUp += new MouseEventHandler(node_MouseUp);
            node.MouseMove += new MouseEventHandler(node_MouseMove);
            node.MouseClick += new MouseEventHandler(node_Click);
            node.DoubleClick += new EventHandler(node_DoubleClick);
            node.MouseEnter += new EventHandler(node_MouseEnter);

            foreach (ConPort port in active_gate.PortsIN)
            {// pre vsetky vstupne porty vo vyznacenom hradle pridat do uzla
                node.LoadIn(port.x, port.y, port.name);
            }
            foreach (ConPort port in active_gate.PortsOUT)
            {// pre vsetky vystupne porty vo vyznacenom hradle pridat do uzla
                node.LoadOut(port.x, port.y, port.name);
            }
            node.BackColor = Color.Transparent;
            pictureBox.Controls.Add(node);
            PaintMain();

            RefreshListNodes();
        }

     private void pictureBox_Click(object sender, EventArgs e)
        {
            {
                pictureBox.Focus();
                int x = System.Windows.Forms.Cursor.Position.X - this.Location.X - pictureBox.Left - 5 - modX;
                int y = System.Windows.Forms.Cursor.Position.Y - this.Location.Y - pictureBox.Top - 31 - modY;
                if (action == "addnode")
                { // ak je akcia nastavena na pridavanie hradiel, vytvor hradlo na pictureboxe
                    action = tmp_action;
                    CreateNode2(x, y);

                    return;
                }

                if (((MouseEventArgs)e).Button == MouseButtons.Right)
                {
                    if (StartCon == true)
                    {
                        StartCon = false;
                        PaintMain();
                        return;
                    }
                }

                foreach (NodeCtrl node in Nodes)
                {

                    node.BackColor = Color.Transparent;
                    foreach (Connection con in node.Connections)
                    {
                        con.ConWidth = 1;
                    }
                }
                foreach (Connection con in Connections)
                {
                    penpic1.Width = 20;
                    if (con.Path.IsOutlineVisible(x, y, penpic1))
                    {
                        if (((MouseEventArgs)e).Button == MouseButtons.Right)
                        {

                            //cMenuCon.Left = x;
                            //cMenuCon.Top = y;
                            //cMenuCon.SetBounds(x, y, cMenuCon.Width, cMenuCon.Height);

                            //con.ContextMenuStrip = cMenuCon;
                            //cMenuCon.SourceControl = con;
                            //cMenuCon.Show();
                            active_object = con;
                            con.ContextMenuStrip.Show(new Point(((MouseEventArgs)e).X + pictureBox.Left + form.Left + 95, ((MouseEventArgs)e).Y + pictureBox.Top + form.Top + 85));
                        }
                        if (ModelType == "Logic")
                        {
                            foreach (Connection c in Connections)
                            {
                                if (c.Name == con.Name) c.ConWidth = 3;
                            }
                        }
                        else
                            con.ConWidth = 3;
                    }
                }
                PaintMain();
            }
        }

        public void RefreshListNodes()
        {

            listNodesMain.Items.Clear();
            foreach (NodeCtrl n in Nodes)
            {
                ListViewItem item = new ListViewItem(n.Text);
                item.Tag = n;
                listNodesMain.Items.Add(item);

                //((ListViewItem)listNodesMain.Items[listNodesMain.Items.Count-1]).Text = n.Text;
            }
            listNodesMain.Refresh();

        }

        private NodeCtrl SetNodeType(NodeCtrl node)
        {
            foreach (tmpNode uzlicek in _nodes)
            {
                if (uzlicek.name == active_gate.name)
                {
                    if ((active_gate.name == "IN") || (active_gate.name == "OUT"))
                    {
                        node.Height = uzlicek.X;
                        node.Width = uzlicek.Y;
                        return node;
                    }
                    else
                    {
                        node.BackgroundImage = (Image)uzlicek.Picture;
                        node.Height = uzlicek.X;
                        node.Width = uzlicek.Y;
                        return node;
                    }
                }

                else
                {
                    //MessageBox.Show("ee error");
                }
            }
            return node;
        }

        void node_MouseEnter(object sender, EventArgs e)
        {
            SetCursor();
        }

        public NodeCtrl FindNode(string name)
        {
            foreach (NodeCtrl n in Nodes)
            {
                if (n.Text == name) return n;
            }
            return null;
        }

        public void ConnectNodes(string n1, string n2, string name)
        {
            StartConnection(FindNode(n1), 2, "ARC");
            EndConnection(FindNode(n2), 1, name);
            RefreshListCons();
        }

        public void ConnectNodes(string n1, string n2)
        {
            NodeCtrl nodeStart = FindNode(n1);
            NodeCtrl nodeEnd = FindNode(n2);

            PictureBox minPort1 = null;
            PictureBox minPort2 = null;

            int startPort = 2;
            int endPort = 1;

            foreach (PictureBox portStart in nodeStart.Controls)
            {

                if (minPort1 == null) minPort1 = portStart;

                foreach (PictureBox portEnd in nodeEnd.Controls)
                {
                    if (minPort2 == null) minPort2 = portEnd;
                    if (Math.Abs(portStart.PointToClient(portStart.Location).X - portEnd.PointToClient(portEnd.Location).X) < (Math.Abs(minPort1.PointToClient(minPort1.Location).X - portEnd.PointToClient(portEnd.Location).X)))
                    {
                        minPort1 = portStart;
                        minPort2 = portEnd;
                    }
                }
            }

            if (nodeStart.Left > nodeEnd.Left)
            {
                startPort = 1;
                endPort = 2;
            }

            /*foreach (PictureBox portEnd in nodeEnd.Controls)
            {
                if (minPort2.Left == 0) minPort2 = portEnd;

                foreach (PictureBox portStart in nodeStart.Controls)
                {
                    if (Math.Abs(portStart.Left - portEnd.Left) < (Math.Abs(minPort2.Left - portEnd.Left))) minPort2 = portEnd;
                }
            }*/

            //StartConnection(FindNode(n1), Convert.ToInt32(minPort1.Tag), "ARC");
            //EndConnection(FindNode(n2), Convert.ToInt32(minPort2.Tag), false, "");

            StartConnection(FindNode(n1), startPort, "ARC");
            EndConnection(FindNode(n2), endPort, "");
            RefreshListCons();
        }


        public ArrayList FindNode(NodeCtrl n, string port)
        {
            ArrayList tmp = new ArrayList();

            //if (port.Contains('!')) port = port.Substring(1);

            foreach (NodeCtrl node in Nodes)
            {
                if (n != node)
                {
                    foreach (string str in node.ConIN)
                    {
                        if (str == port)
                        {
                            tmp.Add(node);
                            tmp.Add(node.ConIN.IndexOf(str));
                            return tmp;
                        }
                    }
                    foreach (string str in node.ConOut)
                    {
                        if (str == port)
                        {
                            tmp.Add(node);
                            tmp.Add(node.ConIN.IndexOf(str) + node.ConIN.Count);
                            return tmp;
                        }
                    }
                }
            }
            return null;
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

        private void pictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("aasd");
            try
            {
                vScrollBar.Value = vScrollBar.Value + e.Delta * SystemInformation.MouseWheelScrollLines / 20 * -1;
                vScrollBar_Scroll(sender, new ScrollEventArgs(ScrollEventType.ThumbTrack, vScrollBar.Value));
            }
            catch
            {
            }

            //e.Delta
        }

         private void pictureBox1_MouseHover(object sender, EventArgs e)
        { //volaky tooltip
            int x = System.Windows.Forms.Cursor.Position.X - this.Location.X - pictureBox.Left - 5 - modX;
            int y = System.Windows.Forms.Cursor.Position.Y - this.Location.Y - pictureBox.Top - 31 - modY;
            
            //toolTipMain.AutomaticDelay = 1000;
            //toolTipMain.InitialDelay = 0;
            //toolTipMain.SetToolTip(pictureBox, "asdasd");
            //toolTipMain.Active = true;
            //toolTipMain.AutoPopDelay = 1000;
            //toolTipMain.SetToolTip(pictureBox, "");
            //toolTipMain.ShowAlways = true;
            foreach (Connection con in Connections)
            {
                penpic1.Width = 20;
                if (con.Path.IsOutlineVisible(x, y, penpic1)) {
                    //MessageBox.Show(con.Name + " vnutri");
                    //toolTipMain.Dispose();
                    tip = new ToolTip();
                    tip.AutomaticDelay = 0;
                    tip.InitialDelay = 0;
                    tip.ShowAlways = true;
                    string start = "";
                    if (con.StartPort > con.StartNode.ConIN.Count)
                        start = con.StartNode.ConOut[0].ToString();
                    else start = con.StartNode.ConIN[con.StartPort-1].ToString();

                    string end = "";

                    if (con.EndPort > con.EndNode.ConIN.Count)
                        end = con.EndNode.ConOut[0].ToString();
                    else end = con.EndNode.ConIN[con.EndPort-1].ToString();
                    
                    tip.SetToolTip(pictureBox, start + " -- " + end);
                    
                     
                    //toolTipMain.Show(con.Name, this);
                    //toolTipMain.Active = true;
                }
            }
        }


        private void toolConDel_MouseDown(object sender, MouseEventArgs e)
        {
            //toolAction.Text = "delete";
            action = "delete";
        }



        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
           /*
            //moje | co sa ma vykonat
            if (ciarka == 0)
            {
                ciarka = 1;
                STARTX = System.Windows.Forms.Cursor.Position.X - this.Location.X - pictureBox.Left - 5 - modX;
                STARTY = System.Windows.Forms.Cursor.Position.Y - this.Location.Y - pictureBox.Top - 31 - modY;
            }
            else if (ciarka == 1)
            {
                ciarka = 0;
                ENDX = System.Windows.Forms.Cursor.Position.X - this.Location.X - pictureBox.Left - 5 - modX;
                ENDY = System.Windows.Forms.Cursor.Position.Y - this.Location.Y - pictureBox.Top - 31 - modY;
            }
                //moje
        */
        }
        
        private void toolSelect_Click(object sender, EventArgs e)
        {
            action = "select";
        }



        private void splitContainer3_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (Nodes.Count > 0 || Connections.Count > 0)
            //{
                if (MessageBox.Show("Chcete ukončiť program? Všetky neuložené zmeny sa stratia!", "Pozor", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            //}
            
                
        }
        



        private void pictureBox_DoubleClick(object sender, EventArgs e)
        {

        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void mainMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void listConsMain_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void súborToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void lblCoord_Click(object sender, EventArgs e)
        {

        }

        private void toolMoveNode_Click(object sender, EventArgs e)
        {
            action = "movenode";
            //toolAction.Text = action;
        }

        private void toolAction_Click(object sender, EventArgs e)
        {

        }

        private void toolSelect_MouseHover(object sender, EventArgs e)
        {
            int x = System.Windows.Forms.Cursor.Position.X - this.Location.X - pictureBox.Left - 5 - modX;
            int y = System.Windows.Forms.Cursor.Position.Y - this.Location.Y - pictureBox.Top - 31 - modY;

            //toolTipMain.AutomaticDelay = 1000;
            //toolTipMain.InitialDelay = 0;
            //toolTipMain.SetToolTip(pictureBox, "asdasd");
            //toolTipMain.Active = true;
            //toolTipMain.AutoPopDelay = 1000;
            //toolTipMain.SetToolTip(pictureBox, "");
            //toolTipMain.ShowAlways = true;
            foreach (Connection con in Connections)
            {
                penpic1.Width = 20;
                if (con.Path.IsOutlineVisible(x, y, penpic1))
                {
                    //MessageBox.Show(con.Name + " vnutri");
                    //toolTipMain.Dispose();
                    tip = new ToolTip();
                    tip.AutomaticDelay = 0;
                    tip.InitialDelay = 0;
                    tip.ShowAlways = true;
                    string start = "";
                    if (con.StartPort > con.StartNode.ConIN.Count)
                        start = con.StartNode.ConOut[0].ToString();
                    else start = con.StartNode.ConIN[con.StartPort - 1].ToString();

                    string end = "";

                    if (con.EndPort > con.EndNode.ConIN.Count)
                        end = con.EndNode.ConOut[0].ToString();
                    else end = con.EndNode.ConIN[con.EndPort - 1].ToString();

                    tip.SetToolTip(pictureBox, start + " -- " + end);
                    //toolTipMain.Show(con.Name, this);
                    //toolTipMain.Active = true;
                }
            }
        }

        private void toolConDel_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.Close();
        }
        // toto - newToolStripMenuItem_Click treba dokoncit 
        /*private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cleanup();
            if (Nodes.Count > 0 || Connections.Count > 0)
            {
                if (MessageBox.Show("Chcete vytvoriť nový projekt? Všetky neuložené zmeny sa stratia!", "Pozor", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    frmNew frm = new frmNew();
                    // YYY toto odkomentovat frm.ShowDialog(); najst frmMain a odkomentovat!
                }
            }
            else
            {
                frmNew frm = new frmNew();
                //frm.ShowDialog();
            }
        }*/

        public void Cleanup()
        {
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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // YYY dopln aboutbox
            GetFunctions();
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "help.chm");
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            gpic1 = pictureBox.CreateGraphics();
            PaintMain();
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            //lblCoord.Text = "PL";

            if (!mouseDown) timer1.Enabled = false;
        }

        private void pictureBox_Resize(object sender, EventArgs e)
        {
            PaintMain();
        }

        /*private void toolText_Click(object sender, EventArgs e)
        {
            try
            {
                if (názvySpojeníToolStripMenuItem.Checked == true)
                    názvySpojeníToolStripMenuItem.Checked = false;
                else
                    názvySpojeníToolStripMenuItem.Checked = true;
            }
            catch
            {
            }
            if (SetLabelVisible) SetLabelVisible = false;
            else SetLabelVisible = true;
            foreach (Connection con in Connections)
            {
                con.SetLabelVisible();
            }
        }
     */
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
                        if (con.Name == conex.Name){
                        con.StartNode.Text = conex.StartNode;
                        con.EndNode.Text = conex.EndNode;
                            foreach(String n in con.StartNode.ConOut){
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

                     Load_StartConnection(con,con.StartNode, numb, "", portx);
                   // Load_StartConnection(con, ((NodeCtrl)FindNode(con.StartNode, n)[1]), ((int)FindNode(con.StartNode, n)[0]) + 2, "", portx);            
                                }
                            }
                              foreach(String n in con.EndNode.ConIN){
                                b++;
                                if (n == con.Name)
                                {
                                    q = b;
                                   /* MessageBox.Show("End:" + q.ToString());
                                    MessageBox.Show(n);
                                    MessageBox.Show("meno prepojenia:" + con.Name);
                                   */
                                    // Load_EndConnection(con,((NodeCtrl)FindNode(con.EndNode, n)[0]), ((int)FindNode(con.EndNode, n)[1]) + 2, n);
                   
                                   Load_EndConnection(con,con.EndNode, q, n );
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

        private string zistiOperaciu(string gateName)
        {
            if (gateName.Substring(0, 3).ToLower() == "and")
                return "*";

            if (gateName.Substring(0, 3).ToLower() == "inv")
                return "'";

            if (gateName.Substring(0, 2).ToLower() == "or")
                return "+";

            if (gateName.Substring(0, 3).ToLower() == "xor")
                return "^";

            if (gateName.Substring(0, 4).ToLower() == "nand")
                return "nand";

            if (gateName.Substring(0, 3).ToLower() == "nor")
                return "nor";

            return "";
        }

        public ArrayList GetFunctions()
        {
            if (ModelType == "Logic")
            {
                string filename = "obvod_tmp.z5";

                PluginInterface.SavedNode uzol = new PluginInterface.SavedNode();
                Stream stream = null;
                StringBuilder subor = new StringBuilder();

                char[] znaky = { '*', '+', '^' };
                string tmp = "", aux = "";   //pomocne retazce         
                //ArrayList zoznamFunkcii = new ArrayList();//zoznam funcii jednotlivych hradiel 
                zoznamFunkcii.Clear();
                int pocet_obj = 0;
                try
                {
                   // UlozUzly();
                    stream = File.Open(filename, FileMode.Open);
                    BinaryFormatter bF = new BinaryFormatter();
                    FileInfo fInfo = new FileInfo(@filename);

                    long size = fInfo.Length;
                    if (size == 0)
                    {
                        //MessageBox.Show("Žiadny obvod na zjednodušenie!");
                        zoznamFunkcii.Add("CHYBA");
                        zoznamFunkcii.Add("Žiadny obvod na vypísanie!");
                        return zoznamFunkcii;
                    }


                    if (stream != null)
                    {
                        //STREAM START
                        List<PluginInterface.SavedNode> objekty = new List<PluginInterface.SavedNode>();
                        stream.Position = 0;
                        while (stream.Position != stream.Length)
                        {
                            uzol = (PluginInterface.SavedNode)bF.Deserialize(stream);
                            objekty.Add(uzol);
                            pocet_obj++;
                        }
                        stream.Close();

                        int poz = 0, zahl = 0;
                        foreach (PluginInterface.SavedNode objekt in objekty)
                        {
                            poz++;
                            if (!objekt.Type.Equals("IN") && !objekt.Type.Equals("OUT"))
                                zahl++;

                            for (int i = 0; i < objekt.ConIN.Count; i++)
                            {
                                if (objekt.ConOut.Count > 0 && objekt.ConOut[0].ToString() == objekt.ConIN[i].ToString() && objekt.Type != "IN" && objekt.Type != "OUT")
                                {
                                    //MessageBox.Show("Jeden zo vstupov hradla " + objekt.Name + " má rovnaký názov ako výstup!");
                                    zoznamFunkcii.Add("CHYBA");
                                    zoznamFunkcii.Add("Jeden zo vstupov hradla " + objekt.Name + " má rovnaký názov ako výstup!");
                                    return zoznamFunkcii;
                                }
                            }

                            if (objekt.ConOut.Count <= 0)
                            {
                                //textBoxSmpFcn.AppendText("-" + Environment.NewLine);
                                continue;
                            }

                            //zostavenie vystupnych funkcii jednotlivych hradiel
                            if (objekt.Type.ToString().Equals("IN") ||
                                objekt.Type.ToString().Equals("OUT"))
                                continue;

                            //vytvorenie zapisu "VYSTUP =" pre kazde hradlo
                            aux = objekt.ConOut[0].ToString() + "=";

                            tmp = "";

                            //pridanie zatvorky na zaciatok                       
                            tmp = "(";

                            for (int i = 0; i < objekt.ConIN.Count; i++)
                            {

                                if (zistiOperaciu(objekt.Name) == "nand")
                                {
                                    tmp += objekt.ConIN[i].ToString() + "' +";
                                }
                                else
                                    if (zistiOperaciu(objekt.Name) == "nor")
                                    {
                                        tmp += objekt.ConIN[i].ToString() + "' *";
                                    }
                                    else
                                        tmp += objekt.ConIN[i].ToString() + zistiOperaciu(objekt.Name).ToString();//pridanie znamienka operacie
                            }

                            //orezanie znamienka operacie z konca
                            tmp = tmp.TrimEnd(znaky);

                            // pridanie zatvorky na koniec                       
                            tmp += ")";

                            aux += tmp;
                            zoznamFunkcii.Add(aux);//bola pridada nova funkcia do zoznamu funkcii                        
                            aux = "";
                        }

                        if (zahl == 0)
                        {
                            //textBoxSmpFcn.AppendText("\r\nObvod neobsahuje funkčné hradlá.\r\n");
                            zoznamFunkcii.Add("CHYBA");
                            zoznamFunkcii.Add("Obvod neobsahuje funkčné hradlá!");
                            return zoznamFunkcii;
                        }



                        //oddeluje lavu stranu (LS) od pravej strany (PS) rovnice vystupnej funkcie pre jednotlive hradla obvodu                    
                        string[] oddiel, oddiel1;
                        bool nahradene = false;

                        int pocet = 0;

                        while (true)
                        {
                            for (int index = 0; index < zoznamFunkcii.Count; index++)
                            {
                                oddiel = zoznamFunkcii[index].ToString().Split('=');
                                for (int j = 0; j < zoznamFunkcii.Count; j++)
                                {
                                    oddiel1 = zoznamFunkcii[j].ToString().Split('=');

                                    if ((oddiel1[1].IndexOf(oddiel[0]) != -1) && (oddiel1[1].IndexOf(oddiel[0] + "'") == -1))
                                    {
                                        zoznamFunkcii[j] = zoznamFunkcii[j].ToString().Replace(oddiel[0], oddiel[1]);
                                        nahradene = true;
                                    }
                                    else
                                        if (oddiel1[1].IndexOf(oddiel[0] + "'") != -1)
                                        {
                                            zoznamFunkcii[j] = zoznamFunkcii[j].ToString().Replace(oddiel[0], "(" + oddiel[1] + ")");
                                            nahradene = true;
                                        }
                                }

                                if (nahradene == true)
                                {
                                    zoznamFunkcii.RemoveAt(index);
                                    nahradene = false;
                                }
                            }

                            pocet++;
                            if (pocet > 2 * zoznamFunkcii.Count + 1)
                                break;
                        }

                        //MessageBox.Show(zoznamFunkcii.Count.ToString());
                        return zoznamFunkcii;
                    }
                    else
                    {
                        //MessageBox.Show("Žiadny obvod na zjednodušenie!");
                        zoznamFunkcii.Add("CHYBA");
                        zoznamFunkcii.Add("Žiadny obvod na vypísanie!");
                        return zoznamFunkcii;
                    }
                }
                catch (Exception es)
                {
                    //MessageBox.Show(es.Message);
                    zoznamFunkcii.Add("CHYBA");
                    zoznamFunkcii.Add(es.Message);
                    return zoznamFunkcii;
                }
            }
            else
                return null;
        }

        

        /*private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if ((ActualFileName != null) && (ActualFileName.EndsWith(".z5")))
            {
                UlozUzly(ActualFileName);
            }
            else
            {
                string FileName;
                saveFileDialog1.Filter = "Z5 Files (*.z5)|*z5";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (!saveFileDialog1.FileName.EndsWith(".z5"))
                    {
                        FileName = saveFileDialog1.FileName + ".z5";
                    }
                    else FileName = saveFileDialog1.FileName;
                    ActualFileName = FileName;
                    UlozUzly(FileName);
                }
            }
            
        }
        */
       /* private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Z5 Files (*.z5)|*.z5";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                NacitajUzly(openFileDialog1.FileName);
            }
        }
        */
        private void listNodesMain_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer3_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        /*private void newToolStripButton_Click(object sender, EventArgs e)
        {
            Cleanup();
            if (Nodes.Count > 0 || Connections.Count > 0)
            {
                if (MessageBox.Show("Chcete vytvoriť nový projekt? Všetky neuložené zmeny sa stratia!", "Pozor", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    frmNew frm = new frmNew();
                    // YYY toto odkomentovat frm.ShowDialog(); najst frmMain a odkomentovat!
                }
            }
            else
            {
                frmNew frm = new frmNew();
                //frm.ShowDialog();
            }
        }*/

        /*private void openToolStripButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Z5 Files (*.z5)|*.z5";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                NacitajUzly(openFileDialog1.FileName);
            }
        }
        */
        /*private void saveToolStripButton_Click(object sender, EventArgs e)
        {

            if ((ActualFileName != null) && (ActualFileName.EndsWith(".z5")))
            {
                UlozUzly(ActualFileName);
           }else
           {
               string FileName;
               saveFileDialog1.Filter = "Z5 Files (*.z5)|*z5";
               if (saveFileDialog1.ShowDialog() == DialogResult.OK)
               {
                   if (!saveFileDialog1.FileName.EndsWith(".z5"))
                   {
                       FileName = saveFileDialog1.FileName + ".z5";
                   }
                   else FileName = saveFileDialog1.FileName;
                   ActualFileName = FileName;
                   UlozUzly(FileName);
               }
           }

        }
        */
        /*private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string FileName;
            saveFileDialog1.Filter = "Z5 Files (*.z5)|*z5";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!saveFileDialog1.FileName.EndsWith(".z5"))
                {
                    FileName = saveFileDialog1.FileName + ".z5";
                }
                else FileName = saveFileDialog1.FileName;
                ActualFileName = FileName;
                UlozUzly(FileName);
            }
        }*/

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label_Uzly(object sender, EventArgs e)
        {

        }

        private void toolStrip4_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }



    }
}
