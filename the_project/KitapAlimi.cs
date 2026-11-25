using System;

namespace the_project
{
    // Un livre remis par le personnel de la bibliotheque.
    public class KitapAlimi
    {
        public Kitap Kitap { get; }
        public string PersonelNo { get; }
        public DateTime Zaman { get; }

        public KitapAlimi(Kitap kitap, personel_kutuphaneclass personel)
            : this(kitap, personel.no, DateTime.Now)
        {
        }

        // utilise au chargement depuis la base
        public KitapAlimi(Kitap kitap, string personelNo, DateTime zaman)
        {
            Kitap = kitap;
            PersonelNo = personelNo;
            Zaman = zaman;
        }

        public override string ToString()
        {
            return $"{Zaman:dd.MM HH:mm} {Kitap} (personel {PersonelNo})";
        }
    }
}
