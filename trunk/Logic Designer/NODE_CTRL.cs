using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Logic_Designer
{
    class NODE_CTRL
    {
        public ArrayList ConIN = new ArrayList();
        public ArrayList ConOut = new ArrayList();
        public ArrayList Connections = new ArrayList();
        public string Text;
        public string Type = null;
        public int Left;
        public int Top;

        private int _ID = -1;

        public int ID
        {
            set { _ID = value; }
            get { return _ID; }
        }


    }
}
