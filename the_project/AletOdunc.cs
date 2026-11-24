using System;

namespace the_project
{
    // Un pret d'outil (et son retour).
    public class AletOdunc
    {
        public Alet Alet { get; }
        public string AlanKisi { get; }
        public string PersonelNo { get; }
        public DateTime VerilisZamani { get; }
        public DateTime? IadeZamani { get; private set; }

        public bool IadeEdildi => IadeZamani.HasValue;

        public AletOdunc(Alet alet, string alanKisi, personel_aletclass personel)
        {
            Alet = alet;
            AlanKisi = alanKisi;
            PersonelNo = personel.no;
            VerilisZamani = DateTime.Now;
        }

        public void IadeEt()
        {
            IadeZamani = DateTime.Now;
        }

        public override string ToString()
        {
            string durum = IadeEdildi ? $"iade {IadeZamani:HH:mm}" : "oduncte";
            return $"{VerilisZamani:HH:mm} {Alet.Ad} -> {AlanKisi} ({durum})";
        }
    }
}
