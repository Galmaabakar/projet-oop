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
        {
            Kitap = kitap;
            PersonelNo = personel.no;
            Zaman = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Zaman:HH:mm} {Kitap} (personel {PersonelNo})";
        }
    }
}
