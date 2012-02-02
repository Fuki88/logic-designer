using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Logic_Designer
{
    public partial class NumberedRichTextBox : UserControl
    {
        public NumberedRichTextBox()
        {
            InitializeComponent();
        }

        /*PREBRATE A UPREVENE Z http://www.codeproject.com/KB/edit/numberedtextbox.aspx
            zaciatok*/
        //aktualizovat cislovanie
        private void textNumbering_update()
        {
            //we get index of first visible char and number of first visible line
            Point pos = new Point(0, 0);
            int firstIndex = textRichTextBox1.GetCharIndexFromPosition(pos);
            int firstLine = textRichTextBox1.GetLineFromCharIndex(firstIndex);

            //now we get index of last visible char and number of last visible line
            pos.X = ClientRectangle.Width;
            pos.Y = ClientRectangle.Height;
            int lastIndex = textRichTextBox1.GetCharIndexFromPosition(pos);
            int lastLine = textRichTextBox1.GetLineFromCharIndex(lastIndex);

            //this is point position of last visible char, we'll use its Y value for calculating numberLabel size
            pos = textRichTextBox1.GetPositionFromCharIndex(lastIndex);


            //finally, renumber label
            textNumbering.Text = "";
            for (int i = firstLine; i <= lastLine + 1; i++)
            {
                textNumbering.Text += i + 1 + " \n";
            }

        }

        //ak nastala zmena textu
        private void textRichTextBox1_TextChanged(object sender, EventArgs e)
        {
            textNumbering_update();
        }

        //numbering sa musi memit podla scrollovania
        private void textRichTextBox1_VScroll(object sender, EventArgs e)
        {
            //move location of numberLabel for amount of pixels caused by scrollbar
            int d = textRichTextBox1.GetPositionFromCharIndex(0).Y % (textRichTextBox1.Font.Height + 1);
            textNumbering.Location = new Point(0, d);

            textNumbering_update();
        }

        //ak nastala zmena velkosti okna
        private void textRichTextBox1_Resize(object sender, EventArgs e)
        {
            textRichTextBox1_VScroll(null, null);
        }

        //ak sa zmeni font
        private void textRichTextBox1_FontChanged(object sender, EventArgs e)
        {
            textNumbering_update();
            textRichTextBox1_VScroll(null, null);
        }
        /*PREBRATE A UPREVENE Z http://www.codeproject.com/KB/edit/numberedtextbox.aspx
            koniec*/
    }
}
