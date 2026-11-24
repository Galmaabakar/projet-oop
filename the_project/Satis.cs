using System;
using System.Collections.Generic;
using System.Linq;

namespace the_project
{
    // Une vente terminee (un ticket de caisse).
    public class Satis
    {
        public IReadOnlyList<SatisKalemi> Kalemler { get; }
        public int Toplam { get; }
        public string PersonelNo { get; }
        public DateTime Zaman { get; }

        public Satis(IEnumerable<SatisKalemi> kalemler, reyonclass personel)
        {
            Kalemler = kalemler.ToList().AsReadOnly();
            Toplam = Kalemler.Sum(k => k.Fiat);
            PersonelNo = personel.no;
            Zaman = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Zaman:HH:mm} {Kalemler.Count} urun: {Toplam} TL";
        }
    }
}
