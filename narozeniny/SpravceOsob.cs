using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace narozeniny
{
    public class SpravceOsob
    {
        public ObservableCollection<Osoba> Osoby { get; set; }
        public SpravceOsob()
        {
            Osoby = new ObservableCollection<Osoba>();
        }
        public Osoba? NejblizsiOsoba { get; set; }


        private void NajdiNejblizsi()
        {
            var serazeneOsoby = Osoby.OrderBy(o => o.ZbyvaDni());
            if (serazeneOsoby.Any())
                NejblizsiOsoba = serazeneOsoby.First();
            else
                NejblizsiOsoba = null;
        }

        public void Pridej(string jmeno, DateTime? datumNarozeni)
        {
            if (jmeno.Length < 3)
                throw new ArgumentException("Jméno mesí obsahovat alespoň 3 znaky");
            if (datumNarozeni == null)
                throw new ArgumentException("Musíte zadat datum narození");
            if (datumNarozeni.Value.Date > DateTime.Today)
                throw new ArgumentException("Tento člověk se ještě nenarodil");

            Osoba osoba = new Osoba(jmeno, datumNarozeni?? DateTime.Now);
            Osoby.Add(osoba);
            NajdiNejblizsi();
        }

        void Odeber(Osoba osoba)
        {
            Osoby.Remove(osoba);
            NajdiNejblizsi();
        }
    }
}
