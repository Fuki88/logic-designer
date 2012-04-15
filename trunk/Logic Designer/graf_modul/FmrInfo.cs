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
    public partial class frmInfo : Form
    {
        public frmInfo()
        {
            InitializeComponent();
        }

        ArrayList functions = new ArrayList();
        


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblNazov_Click(object sender, EventArgs e)
        {

        }

        private void listFunc_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblPocetHradiel_Click(object sender, EventArgs e)
        {

        }

        private void frmInfo_Load(object sender, EventArgs e)
        {
            groupBox1.Visible = false;


            lblNazov.Text = graf_modul.form.ModelName.ToString();
            lblPocetHradiel.Text = graf_modul.form.Nodes.Count.ToString();
            functions = graf_modul.form.GetFunctions();

            foreach (string str in functions)
            {
                listFunc.Items.Add(str);
            }

             groupBox1.Visible = true;
        }
    }
}