using System;

namespace the_project
{
    public class Kitap
    {
        // cle dans la base de donnees (0 = pas encore enregistre)
        public long Id { get; internal set; }
        public string Ad { get; }
        public string Yazar { get; }
        public bool BestSeller { get; }

        public Kitap(string ad, string yazar, bool bestSeller)
        {
            Ad = ad;
            Yazar = yazar;
            BestSeller = bestSeller;
        }

        public bool AyniKitapMi(Kitap diger)
        {
            return string.Equals(Ad, diger.Ad, StringComparison.OrdinalIgnoreCase)
                && string.Equals(Yazar, diger.Yazar, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return $"{Ad} - {Yazar}";
        }
    }
}
