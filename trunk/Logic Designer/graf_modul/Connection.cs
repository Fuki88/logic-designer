using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

namespace Digi_graf_modul
{
    public partial class Connection : Control
    {
        public Connection()
        {
            lblStart.AutoSize = true;
            if (!graf_modul.form.SetLabelVisible)
            {
                lblStart.Visible = true;
                lblEnd.Visible = true;
                lblMid.Visible = true;
            }
            else
            {
                lblStart.Visible = false;
                lblEnd.Visible = false;
                lblMid.Visible = false;
            }
            lblStart.MouseMove += new MouseEventHandler(lblStartEnd_MouseMove);
            lblStart.MouseDown += new MouseEventHandler(lblStartEnd_MouseDown);
            lblStart.MouseUp += new MouseEventHandler(lblStart_MouseUp);

            lblEnd.AutoSize = true;
            lblEnd.MouseMove += new MouseEventHandler(lblStartEnd_MouseMove);
            lblEnd.MouseDown += new MouseEventHandler(lblStartEnd_MouseDown);
            lblEnd.MouseUp += new MouseEventHandler(lblEnd_MouseUp);

            lblMid.AutoSize = true;
            lblMid.MouseMove += new MouseEventHandler(lblStartEnd_MouseMove);
            lblMid.MouseDown += new MouseEventHandler(lblStartEnd_MouseDown);
            lblMid.MouseUp += new MouseEventHandler(lblMid_MouseUp);
        }
        private int _Width = 1;
        private string _Name = "";
        private Point _Start;
        private Point _End;
        public Point _Mid;
        private Color _color = Color.Black;
        public ArrayList Points = new ArrayList();
        public ArrayList PointMove = new ArrayList();
        private NodeCtrl _StartNode;
        private NodeCtrl _EndNode;
        private PictureBox _StartPortObj;
        private PictureBox _EndPortObj;
        private int _StartPort;
        private int _EndPort;
        private GraphicsPath path = new GraphicsPath();
        public string Type = "";
        public Label lblStart = new Label();
        public Label lblEnd = new Label();
        public Label lblMid = new Label();
        private bool mouseDown = false;
        private Point lblStartPos = new Point(0, 0);
        private int coordXStart = 0;
        private int coordYStart = 0;
        private int coordXEnd = -45;
        private int coordYEnd = 0;
        private int coordYMid = -20;
        private int coordXMid = 0;
        public bool kresli = true;
        public bool label_visible = false;
        public PictureBox StartPortPic;
        public PictureBox EndPortPic;
        
        private Point[] pts = new Point[3];
        public float startAngle;

        public int ConWidth
        {
            set { _Width = value; }
            get { return _Width; }
        }

        public PictureBox StartPortObj
        {
            set { _StartPortObj = value; }
            get { return _StartPortObj; }
        }

        public PictureBox EndPortObj
        {
            set { _EndPortObj = value; }
            get { return _EndPortObj; }
        }

        public int StartPort
        {
            set { _StartPort = value; }
            get { return _StartPort; }
        }

        public int EndPort
        {
            set { _EndPort = value; }
            get { return _EndPort; }
        }

        public NodeCtrl StartNode
        {
            set { _StartNode = value; }
            get { return _StartNode; }
        }
        
        public NodeCtrl EndNode
        {
            set { _EndNode = value; }
            get { return _EndNode; }
        }
        
        public string Name
        {
            set { _Name = value; }
            get { return _Name; }
        }

        public Point Start
        {
            set { _Start = value; }
            get { return _Start; }
        }

        public int MidX
        {
            set { _Mid.X = value; }
            get { return _Mid.X; }
        }

        public int MidY
        {
            set { _Mid.Y = value; }
            get { return _Mid.Y; }
        }

        public Point Mid
        {
            set { _Mid = value; }
            get { return _Mid; }
        }

        public Point End
        {
            set { _End = value; }
            get { return _End; }
        }

        public Color color
        {
            set { _color = value; }
            get { return _color; }
        }

        public GraphicsPath Path
        {
            get { return path; }
        }

        private void lblStartEnd_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;

        }

        private void lblStart_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            coordXStart = ((Label)sender).Left - Start.X;
            coordYStart = ((Label)sender).Top - Start.Y;

        }

        private void lblEnd_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            coordXEnd = ((Label)sender).Left - End.X;
            coordYEnd = ((Label)sender).Top - End.Y;
        }

        private void lblMid_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            coordXMid = ((Label)sender).Left - Mid.X;
            coordYMid = ((Label)sender).Top - Mid.Y;
        }

        private void lblStartEnd_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                ((Label)sender).Left = -graf_modul.form.Left - graf_modul.form.GetMainPanel.Left + System.Windows.Forms.Cursor.Position.X - 10 + graf_modul.form.GetMainHScroll.Value - 90;
                ((Label)sender).Top = -graf_modul.form.Top - graf_modul.form.GetMainPanel.Top + System.Windows.Forms.Cursor.Position.Y - 45 + graf_modul.form.GetMainVScroll.Value - 45;
                 graf_modul.form.PaintMainMove();

            }
        }

        public void SetLabelVisible()
        {
            if (!graf_modul.form.SetLabelVisible)
            {
                if (Type != "ARC")
                {
                    lblEnd.Visible = true;
                    lblStart.Visible = true;
                }
                else
                    lblMid.Visible = true;
                label_visible = false;
            }
            else
            {
                if (Type != "ARC")
                {
                    lblEnd.Visible = false;
                    lblStart.Visible = false;
                }
                else
                    lblMid.Visible = false;
                label_visible = true;
            }
        }

        public void SetLabelPos()
        {
            lblStart.BringToFront();
            lblStart.Top = Start.Y + coordYStart;
            lblStart.Left = Start.X + coordXStart;

            lblEnd.BringToFront();
            lblEnd.Top = End.Y + coordYEnd;
            lblEnd.Left = End.X + coordXEnd;

            lblMid.BringToFront();
            lblMid.Top = Mid.Y + coordYMid;
            lblMid.Left = Mid.X + coordXMid;

        }


        public void RefreshPath()
        {

            path.Reset();
            path.StartFigure();

            if (Start.Y != End.Y)
            {
                int i;

                if (Points.Count != 0)
                {

                    for (i = 0; i < Points.Count; i += 2)
                    {
                        //MessageBox.Show(Points.Count.ToString());
                        if (Type != "ARC")
                            path.AddLine((Point)Points[i], (Point)Points[i + 1]);
                    }

                    if (Type == "ARC")
                    {
                        pts[0] = Start;
                        pts[1] = Mid;
                        pts[2] = End;
                        path.AddCurve(pts);

                   

                    }
                }
            }
            else path.AddLine(Start, End);

            if (Type == "ARC")
            {


                int a = Math.Abs(End.X - Mid.X);
                int b = Math.Abs(End.Y - Mid.Y);
                double c = Math.Sqrt((double)((a * a) + (b * b)));

                double angle = Math.Asin(b / c) * (180 / Math.PI);

                float tmpAngle;

                if (Mid.X > End.X)
                {
                    tmpAngle = 0;
                    angle = angle * (-1);
                }
                else tmpAngle = 180;

                if (Mid.Y > End.Y) startAngle = tmpAngle - (float)angle - 22;
                else startAngle = tmpAngle + (float)angle - 22;

                path.AddPie(End.X - 20, End.Y - 20, 40, 40, startAngle, 45);
            
            }
           
        }

    }
}
