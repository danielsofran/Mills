using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moară
{
    public partial class Form1 : Form
    {
        public string PColor = "negru";
        public string mutare = "alb";

        //public Piesa[,] Piese = new Piesa[4, 9];
        public List<string> MoriLibere = new List<string> { "11 12 13", "21 22 23", "31 32 33", "18 28 38", "14 24 34", "35 36 37", "25 26 27", "15 16 17", "11 18 17", "21 28 27", "31 38 37", "12 22 32", "16 26 36", "13 14 15", "23 24 25", "33 34 35" };
        public List<string> MoriOcupate = new List<string>();

        public bool PotLuaDinMoara
        {
            get
            {
                bool rez = true;
                List<Piesa> l = (PColor == "alb") ? PieseNegre : PieseAlbe;
                foreach (Piesa p in l)
                    if (!p.InMill)
                        rez = false;
                //MessageBox.Show(rez.ToString());
                return rez;
            }
        }
        public bool PunePiesa
        {
            get => numericUpDown1.Value + numericUpDown2.Value != 0;
        }
        public string From = "";
        public string NextMove { get => mutare == "alb" ? "negru" : "alb"; }

        public Piesa this[string s]
        {
            get
            {
                int nr_ord = 0;
                if (int.Parse(s[0].ToString()) == 1) nr_ord = 0;
                else if (int.Parse(s[0].ToString()) == 2) nr_ord = 8;
                else nr_ord = 16;
                nr_ord += int.Parse(s[1].ToString());
                return Controls.Find("piesa" + nr_ord, true)[0] as Piesa;
            }
        }

        public Piesa this[int nr_ord] { get=>Controls.Find("piesa" + nr_ord, true)[0] as Piesa;}

        public List<Piesa> Piese
        {
            get
            {
                List<Piesa> rez = new List<Piesa>();
                for(int i=1; i<=24;++i)
                    rez.Add(this[i]);
                return rez;
            }
        }
        public List<Piesa> PieseAlbe
        {
            get
            {
                List<Piesa> rez = new List<Piesa>();
                for (int i = 1; i <= 24; ++i)
                    if(this[i].Culoare == "alb")
                        rez.Add(this[i]);
                return rez;
            }
        }
        public List<Piesa> PieseNegre
        {
            get
            {
                List<Piesa> rez = new List<Piesa>();
                for (int i = 1; i <= 24; ++i)
                    if (this[i].Culoare == "negru")
                        rez.Add(this[i]);
                return rez;
            }
        }

        public Dictionary<string, List<string>> Vecini = new Dictionary<string, List<string>>();
        
        public Form1()
        {
            InitializeComponent();
            for(int i=1;i<=9;++i)
            {
                Piesa pa = new Piesa();
                //pa.Name = "pl" + i;
                pa.Enabled = false;
                pa.Culoare = "alb";
                flowLayoutPanel1.Controls.Add(pa);
                Piesa pn = new Piesa();
                //pn.Name = "pl" + i;
                pn.Enabled = false;
                pn.Culoare = "negru";
                flowLayoutPanel2.Controls.Add(pn);
            }
            // initialization of vecini
            Vecini["11"] = new List<string> { "12", "18"};
            Vecini["12"] = new List<string> { "11", "13", "22" };
            Vecini["13"] = new List<string> { "12", "14" };
            Vecini["14"] = new List<string> { "13", "15", "24" };
            Vecini["15"] = new List<string> { "14", "16" };
            Vecini["16"] = new List<string> { "15", "17", "26" };
            Vecini["17"] = new List<string> { "16", "18" };
            Vecini["18"] = new List<string> { "11", "17", "28" };
            Vecini["21"] = new List<string> { "22", "28" };
            Vecini["22"] = new List<string> { "12", "32", "21", "23" };
            Vecini["23"] = new List<string> { "22", "24" };
            Vecini["24"] = new List<string> { "34", "14", "23", "25" };
            Vecini["25"] = new List<string> { "24", "26" };
            Vecini["26"] = new List<string> { "16", "36", "25", "27" };
            Vecini["27"] = new List<string> { "26", "28" };
            Vecini["28"] = new List<string> { "38", "18", "21", "27" };
            Vecini["31"] = new List<string> { "32", "38" };
            Vecini["32"] = new List<string> { "31", "33", "22" };
            Vecini["33"] = new List<string> { "32", "34" };
            Vecini["34"] = new List<string> { "33", "35", "24" };
            Vecini["35"] = new List<string> { "34", "36" };
            Vecini["36"] = new List<string> { "35", "37", "26" };
            Vecini["37"] = new List<string> { "36", "38" };
            Vecini["38"] = new List<string> { "31", "37", "28" };
        }

        public void Piece_Put(Piesa sender)
        {
            // pe mutari
            if (PunePiesa)
            {
                if (CheckMill(sender.Culoare)) return;
                if (mutare == "alb") { mutare = "negru"; PColor = "negru"; flowLayoutPanel1.Controls.RemoveAt(0); numericUpDown1.Value = numericUpDown1.Value - 1; SchimbaPlayer(); }
                else if (mutare == "negru") { mutare = "alb"; PColor = "alb"; flowLayoutPanel2.Controls.RemoveAt(0); numericUpDown2.Value = numericUpDown2.Value - 1; SchimbaPlayer(); }
                else if (mutare == "ia") // sender ul e piesa luata
                {
                    // daca exista mori care au fost stricate, luam din MoriOcupate si punem in MoriLibere
                    EraseMills(sender);
                    // adaugare la piese luate
                    if (sender.Culoare == "alb")
                    {
                        Piesa pa = new Piesa();
                        pa.Culoare = "alb";
                        pa.Enabled = false;
                        flowLayoutPanel4.Controls.Add(pa);
                        numericUpDown4.Value++;
                        flowLayoutPanel2.Controls.RemoveAt(0);
                        numericUpDown2.Value--;
                    }
                    else if (sender.Culoare == "negru")
                    {
                        Piesa pn = new Piesa();
                        pn.Culoare = "negru";
                        pn.Enabled = false;
                        flowLayoutPanel3.Controls.Add(pn);
                        numericUpDown3.Value++;
                        flowLayoutPanel1.Controls.RemoveAt(0);
                        numericUpDown1.Value--;
                    }
                    mutare = (PColor == "negru") ? ("alb") : ("negru");
                    SchimbaPlayer();
                }
            }
            else
            {
                if (mutare.StartsWith("ia"))
                {
                    if (sender.Pus && sender.Culoare == mutare.Substring(2) && (PotLuaDinMoara || !sender.InMill))
                    {
                        EraseMills(sender);
                        sender.Clear();
                        sender.Pus = sender.InMill = false;
                        mutare = mutare.Substring(2);
                    }
                }
                else if(From == "")
                {
                    if (sender.Pus && sender.Culoare == mutare)
                    {
                        From = sender.Index;
                        //MessageBox.Show("from " + sender.Index);
                    }
                }
                else
                {
                    if ( sender.Culoare=="alb" && PieseAlbe.Count==3 || sender.Culoare=="negru" && PieseNegre.Count==3 ||  IsVecin(From, sender.Index) && !sender.Pus)
                    {
                        //MessageBox.Show("sender " + sender.Index);
                        sender.Culoare = this[From].Culoare;
                        EraseMills(this[From]);
                        this[From].Clear();
                        this[From].Pus = this[From].InMill = false;
                        From = "";
                        if (CheckMill(sender.Culoare))
                        {
                            mutare = "ia" + NextMove;
                            SchimbaPlayer();
                        }
                        else
                        {
                            mutare = NextMove;
                            SchimbaPlayer();
                        }
                    }
                    else From = "";
                }
            }
            if (GameOver) MessageBox.Show(Winner + " has won!");
        }
        
        public bool CheckMill(string culoare) 
        {
            // moara, luata, ...
            foreach (string moara in MoriLibere)
            {
                string[] s = moara.Split(' ');
                bool emoara = true;
                foreach (string piesa in s)
                {
                    Piesa p = this[piesa];
                    if (p == null || !p.Pus || culoare != p.Culoare)
                        emoara = false;
                }
                if (emoara)
                {
                    mutare = "ia";
                    foreach (string piesa in s)
                        this[piesa].InMill = true;
                    MoriLibere.Remove(moara);
                    MoriOcupate.Add(moara);
                    return true;
                }
            }
            return false;
        }

        private void SchimbaPlayer()
        {
            Color aux = pictureBox1.BackColor;
            pictureBox1.BackColor = pictureBox2.BackColor;
            pictureBox2.BackColor = aux;
        }
        public void EraseMills(Piesa sender)
        {
            List<string> l = new List<string>();
            foreach (string m in MoriOcupate)
            {
                if (m.Contains(sender.Index))
                {
                    l.Add(m);
                    MoriLibere.Add(m);
                }
            }
            foreach (string s in l) MoriOcupate.Remove(s);
        }
        public bool GameOver
        {
            get
            {
                if (PunePiesa) return false;
                bool loc = false;
                if (mutare == "alb")
                {
                    foreach (Piesa p in PieseAlbe)
                        foreach (string s in Vecini[p.Index])
                            if (!this[s].Pus)
                                loc = true;
                }
                else if(mutare == "negru")
                {
                    foreach (Piesa p in PieseNegre)
                        foreach (string s in Vecini[p.Index])
                            if (!this[s].Pus)
                                loc = true;
                }
                if(!loc)
                {
                    if (mutare == "alb") Winner = textBox2.Text;
                    else if (mutare == "negru") Winner = textBox1.Text;
                    return true;
                }
                if(flowLayoutPanel3.Controls.Count == 7)
                {
                    Winner = textBox1.Text;
                    return true;
                }
                else if(flowLayoutPanel4.Controls.Count == 7)
                {
                    Winner = textBox2.Text;
                    return true;
                }
                return false;
            }
        }
        public string Winner { get; set; }
        public bool IsVecin(string p1, string p2)
        {
            foreach (string p in Vecini[p1])
                if (p == p2)
                    return true;
            return false;
        }
        public bool IsVecin(Piesa p1, Piesa p2)
        {
            foreach (string p in Vecini[p1.Index])
                if (p == p2.Index)
                    return true;
            return false;
        }
    }
}
