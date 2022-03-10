using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moară
{
    class Tabla
    {
        public enum Culoare
        {
            Nimic = 0, Negru = 1, Alb = 2
        }
        public Dictionary<string, List<string>> Vecini = new Dictionary<string, List<string>>();
        public Dictionary<string, Culoare> Piese = new Dictionary<string, Culoare>();
        public List<string> MoriLibere = new List<string> { "11 12 13", "21 22 23", "31 32 33", "18 28 38", "14 24 34", "35 36 37", "25 26 27", "15 16 17", "11 18 17", "21 28 27", "31 38 37", "12 22 32", "16 26 36", "13 14 15", "23 24 25", "33 34 35" };
        public List<string> MoriOcupate = new List<string>();
        public Culoare this[int x, int y]
        {
            get => Piese[x.ToString() + y.ToString()];
        }
        public Culoare this[string s]
        {
            get => Piese[s];
        }
        public Tabla()
        {
            // initialization of vecini
            Vecini["11"] = new List<string> { "12", "18" };
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
            // initialization of Piese
            foreach (string p in Vecini.Keys)
                Piese.Add(p, Culoare.Nimic);
        }
    }
}
