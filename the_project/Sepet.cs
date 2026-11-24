using System;
using System.Collections.Generic;
using System.Linq;

namespace the_project
{
    // Le panier du client en cours.
    public class Sepet
    {
        private readonly List<SatisKalemi> kalemler = new List<SatisKalemi>();

        public IReadOnlyList<SatisKalemi> Kalemler => kalemler.AsReadOnly();
        public bool Bos => kalemler.Count == 0;
        public int Toplam => kalemler.Sum(k => k.Fiat);

        public void Ekle(Urun urun, int fiat)
        {
            if (urun == null) throw new ArgumentNullException(nameof(urun));
            if (!urun.Stokta) throw new InvalidOperationException(urun.Ad + " stokta yok");
            if (fiat < 0) throw new ArgumentException("fiat negatif olamaz");
            kalemler.Add(new SatisKalemi(urun, fiat));
        }

        public void Temizle()
        {
            kalemler.Clear();
        }
    }
}
