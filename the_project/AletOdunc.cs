using System;

namespace the_project
{
    // Un pret d'outil (et son retour).
    public class AletOdunc
    {
        // cle dans la base de donnees (0 = pas encore enregistre)
        public long Id { get; internal set; }
        public Alet Alet { get; }
        public string AlanKisi { get; }
        public string PersonelNo { get; }
        public DateTime VerilisZamani { get; }
        public DateTime? IadeZamani { get; private set; }

        public bool IadeEdildi => IadeZamani.HasValue;

        public AletOdunc(Alet alet, string alanKisi, personel_aletclass personel)
            : this(alet, alanKisi, personel.no, DateTime.Now, null)
        {
        }

        // utilise au chargement depuis la base
        public AletOdunc(Alet alet, string alanKisi, string personelNo, DateTime verilisZamani, DateTime? iadeZamani)
        {
            Alet = alet;
            AlanKisi = alanKisi;
            PersonelNo = personelNo;
            VerilisZamani = verilisZamani;
            IadeZamani = iadeZamani;
        }

        public void IadeEt(DateTime zaman)
        {
            IadeZamani = zaman;
        }

        public override string ToString()
        {
            string durum = IadeEdildi ? $"iade {IadeZamani:dd.MM HH:mm}" : "oduncte";
            return $"{VerilisZamani:dd.MM HH:mm} {Alet.Ad} -> {AlanKisi} ({durum})";
        }
    }
}
