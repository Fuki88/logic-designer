using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Digi_graf_modul
{
    public partial class FrmNazov : Form
    {
        public FrmNazov()
        {
            InitializeComponent();
        }

        private void lbl1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnNazov_Click(object sender, EventArgs e)
        {
            graf_modul.form.ModelName = textBox1.Text;
            this.Close();
        }
    }
}
