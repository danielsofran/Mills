using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moară
{
    public partial class Piesa : UserControl
    {
        Color tcolor = Color.FromArgb(255, 195, 98, 35);
        string _color="";
        public string Culoare { get => _color;
            set
            {
                _color = value;
                if (_color == "alb") { pictureBox1.BackgroundImage = Image.FromFile("alba.png"); Pus = true; }
                else if (_color == "negru") { pictureBox1.BackgroundImage = Image.FromFile("neagra.png"); Pus = true; }
                else this.Clear();
            } 
        }
        public bool Pus = false;
        public bool InMill = false;

        public Piesa()
        {
            InitializeComponent();
            pictureBox1.BackColor = tcolor;
            this.BackColor = tcolor;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string m1 = (this.ParentForm as Form1).mutare;
            if ((this.ParentForm as Form1).PunePiesa)
            {
                if (m1 == "ia" && Pus && Culoare != (this.ParentForm as Form1).PColor && ((this.ParentForm as Form1).PotLuaDinMoara || !InMill))
                {
                    Pus = false;
                    (this.ParentForm as Form1)[Index].Clear();
                    //(this.ParentForm as Form1).mutare = ((this.ParentForm as Form1).PColor == "alb") ? ("negru") : ("alb");
                    (this.ParentForm as Form1).Piece_Put(this);
                }
                else if (!Pus && m1[0] != 'm')
                {
                    Pus = true;
                    Culoare = m1;
                    (this.ParentForm as Form1).Piece_Put(this);
                }
            }
            else (this.ParentForm as Form1).Piece_Put(this);
        }

        public string Index { get => Tag as string; }

        public void Clear()
        {
            pictureBox1.BackgroundImage = null;
            pictureBox1.BackColor = tcolor;
        }
    }
}
