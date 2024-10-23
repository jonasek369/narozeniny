using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace narozeniny
{
    public class Osoba
    {

        public string Jmeno { get; set; }
        public DateTime Narozeniny { get; set; }

        public Osoba(string jmeno, DateTime narozeniny)
        {
            Jmeno = jmeno;
            Narozeniny = narozeniny;
        }

        public int SpoctiVek()
        {
            DateTime dnes = DateTime.Now;
            int vek = dnes.Year - Narozeniny.Year;
            if (dnes < Narozeniny.AddYears(vek))
                vek--;
            return vek;
        }

        public int ZbyvaDni()
        {
            DateTime dnes = DateTime.Now;
            DateTime dalsiNarozeniny = Narozeniny.AddYears(SpoctiVek() + 1);

            TimeSpan rozdil = dalsiNarozeniny - DateTime.Today;

            return Convert.ToInt32(rozdil.TotalDays);
        }

        public override string ToString()
        {
            return Jmeno;
        }
    }
}
