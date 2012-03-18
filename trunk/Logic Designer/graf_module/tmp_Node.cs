using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;	 // For serialization of an object to an XML Document file.
using System.Runtime.Serialization.Formatters.Binary; // For serialization of an object to an XML Binary file.
using System.IO;				 // For reading/writing data to an XML file.
using System.IO.IsolatedStorage; // For accessing user isolated data.

namespace Digi_graf_modul
{
    //[Serializable]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class tmpNode
    {
        //public ArrayList portIn = new ArrayList();
        //public ArrayList portOut = new ArrayList();

        private Bitmap picture;
        //[XmlElement("tmpNode")]
        public tmpNode()
        {
        }

        private List<Port> _portIn = new List<Port>();
        private List<Port> _portOut = new List<Port>();

        public Port[] portIn;
        public Port[] portOut;

        [XmlElement("Type")]
        public string type
        {
            get;
            set;
        }

        [XmlElement("X")]
        public int X
        {
            get;
            set;
        }

        [XmlElement("Y")]
        public int Y
        {
            get;
            set;
        }

        [XmlElement("Name")]
        public string name
        {
            get;
            set;
        }


        [XmlIgnoreAttribute()]
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
                    TypeConverter BitmapConverter =
                    TypeDescriptor.GetConverter(picture.GetType());
                    return (byte[])
                    BitmapConverter.ConvertTo(picture, typeof(byte[]));
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

        public void PridajPortIn(int x, int y, string name)
        {
            Port p = new Port();

            p.X = x;
            p.Y = y;
            p.Name = name;
            p.Type = "IN";

            _portIn.Add(p);
            portIn = _portIn.ToArray();
        }

        public void PridajPortOut(int x, int y, string name)
        {
            Port p = new Port();

            p.X = x;
            p.Y = y;
            p.Name = name;
            p.Type = "OUT";

            _portOut.Add(p);
            portOut = _portOut.ToArray();
        }

    }
}
