using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Digi_graf_modul
{
    public partial class NodeCtrl : UserControl
    {
 
        public object tmp;

      public NodeCtrl() //
        {
          
            this.SuspendLayout();
            this.BackColor = Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Name = "NodeCtrl";
            this.Size = new System.Drawing.Size(80, 80);

            this.MouseMove += new MouseEventHandler(Node_MouseMove);
            this.Paint += new PaintEventHandler(NodeCtrl_Paint);
            this.ResumeLayout(false);
        }

        void NodeCtrl_Paint(object sender, PaintEventArgs e)
        {
            if (frm.ModelType == "FSM")
            {
                Graphics g = e.Graphics;
                SolidBrush b = new SolidBrush(Color.Black);
                g.DrawString(this.Text, new Font(this.Font, FontStyle.Regular), b, new Point(50 / 2 - this.Text.Length / 2 * 10, 50 / 2 - 7));
            }

        }

        public ArrayList ConIN = new ArrayList();// nazvy vstupov
        public ArrayList ConOut = new ArrayList();// nazvy vystupov
        public ArrayList Connections = new ArrayList();
        private graf_modul frm = graf_modul.form;
        private ToolTip tip = new ToolTip();

        public string Type = null;

        private string _Text = "";

        private int portCount = 0;

        private int _ID = -1;

        public int ID
        {
            set { _ID = value; }
            get { return _ID; }
        }

        public string ShownText
        {
            set { _Text = value; }
            get { return _Text; }
        }

        public int PortCount
        {
            get { return portCount; }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void LoadIn(int x, int y, string Name)
        {
            portCount++;
            PictureBox port = new PictureBox();
            port.BackColor = Color.BlueViolet;
            port.Height = 5;
            port.Width = 5;
            port.Top = y;
            port.Left = x;
            port.Tag = portCount;
            port.Name = "port" + portCount.ToString();
            port.Click += new EventHandler(port_Click);
            port.MouseMove += new MouseEventHandler(port_MouseMove);
            base.Controls.Add(port);
            tip.SetToolTip(port, Name);
          
        }

        public void LoadOut(int x, int y, string Name)
        {
            portCount++;
            PictureBox port = new PictureBox();
            port.BackColor = Color.Yellow;
            port.Height = 5;
            port.Width = 5;
            port.Top = y;
            port.Left = x;
            port.Tag = portCount;
            port.Name = "port" + portCount.ToString();
            port.Click += new EventHandler(port_Click);
            port.MouseMove += new MouseEventHandler(port_MouseMove);
            base.Controls.Add(port);
            tip.SetToolTip(port, Name);
       
        }



        public void AddIn(int x, int y, string Name)
        {
            portCount++;
            PictureBox port = new PictureBox();
            port.BackColor = Color.BlueViolet;
            port.Height = 5;
            port.Width = 5;
            port.Top = y;
            port.Left = x;
            port.Tag = portCount;
            port.Name = "port" + portCount.ToString();
            port.Click += new EventHandler(port_Click);
            port.MouseMove += new MouseEventHandler(port_MouseMove);
            base.Controls.Add(port);
            tip.SetToolTip(port, Name);
            ConIN.Add(Name);
        }

        public void AddOut(int x, int y, string Name)
        {
            portCount++;
            PictureBox port = new PictureBox();
            port.BackColor = Color.Yellow;
            port.Height = 5;
            port.Width = 5;
            port.Top = y;
            port.Left = x;
            port.Tag = portCount;
            port.Name = "port" + portCount.ToString();
            port.Click += new EventHandler(port_Click);
            port.MouseMove += new MouseEventHandler(port_MouseMove);
            base.Controls.Add(port);
            tip.SetToolTip(port, Name);
            ConOut.Add(Name);
        }

        public void UpdateToolTip()
        {
            int i = 0;
            foreach (PictureBox port in base.Controls)
            {
                i++;
                if (i <= ConIN.Count)
                    tip.SetToolTip(port, ConIN[i - 1].ToString());
                else
                    tip.SetToolTip(port, ConOut[(i - 1)-ConIN.Count].ToString());
            }
        }

        private void port_Click(Object sender, EventArgs e)
        {
            if (frm.StartCon == false) frm.StartConnection(this, Convert.ToInt32(((PictureBox)sender).Tag),"",(PictureBox)sender);
            else frm.EndConnection(this, Convert.ToInt32(((PictureBox)sender).Tag),true,(PictureBox)sender);
            frm.PaintMain();
        }


        private void Node_MouseMove(object sender, MouseEventArgs e)
        {
            frm.setGroupNode(this.Text, this.ConIN.Count, this.ConOut.Count, this.Type);
        }
        

        private void port_MouseMove(object sender, MouseEventArgs e)
        {
            int pocet = 0;
            string nazov = "N/A";

            foreach (Connection con in Connections)
            {
                if (con.EndNode == this && con.EndPort == Convert.ToInt32(((PictureBox)sender).Tag)) pocet++;
                if (con.StartNode == this && con.StartPort == Convert.ToInt32((((PictureBox)sender).Tag))) pocet++;
            }

            int i = Convert.ToInt32(((PictureBox)sender).Tag);

            if (i <= ConIN.Count)
                nazov = ConIN[i - 1].ToString();
            else
                nazov =  ConOut[(i - 1) - ConIN.Count].ToString();

            frm.setGroupPort(nazov, pocet);
        }

        private void NodeCtrl_Load(object sender, EventArgs e)
        {
            tip.InitialDelay = 0;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // NodeCtrl
            // 
            this.Name = "NodeCtrl";
            this.Load += new System.EventHandler(this.NodeCtrl_Load_1);
            this.ResumeLayout(false);

        }

        private void NodeCtrl_Load_1(object sender, EventArgs e)
        {

        }

    }
}
