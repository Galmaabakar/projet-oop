using System;
using System.Collections.Generic;
using System.Linq;

namespace the_project
{
    // Une vente terminee (un ticket de caisse).
    public class Satis
    {
        // cle dans la base de donnees (0 = pas encore enregistre)
        public long Id { get; internal set; }
        public IReadOnlyList<SatisKalemi> Kalemler { get; }
        public int Toplam { get; }
        public string PersonelNo { get; }
        public DateTime Zaman { get; }

        public Satis(IEnumerable<SatisKalemi> kalemler, reyonclass personel)
            : this(kalemler, personel.no, DateTime.Now)
        {
        }

        // utilise au chargement depuis la base
        public Satis(IEnumerable<SatisKalemi> kalemler, string personelNo, DateTime zaman)
        {
            Kalemler = kalemler.ToList().AsReadOnly();
            Toplam = Kalemler.Sum(k => k.Fiat);
            PersonelNo = personelNo;
            Zaman = zaman;
        }

        public override string ToString()
        {
            return $"{Zaman:dd.MM HH:mm} {Kalemler.Count} urun: {Toplam} TL";
        }
    }
}
