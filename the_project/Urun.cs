using System;

namespace the_project
{
    public class Urun
    {
        // cle dans la base de donnees (0 = pas encore enregistre)
        public long Id { get; internal set; }
        public string Ad { get; }
        public string Numara { get; }
        // false = produit attendu dans une semaine
        public bool Stokta { get; }

        public Urun(string ad, string numara, bool stokta)
        {
            Ad = ad;
            Numara = numara;
            Stokta = stokta;
        }

        public bool Eslesir(string ad, string numara)
        {
            return string.Equals(Ad, (ad ?? "").Trim(), StringComparison.CurrentCultureIgnoreCase)
                && Numara == (numara ?? "").Trim();
        }

        public override string ToString()
        {
            return $"{Ad} - {Numara}";
        }
    }
}
