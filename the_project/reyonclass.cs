using System.Windows.Forms;

namespace the_project
{
    public class reyonclass : gecis
    {
        public const string TIP = "personel_reyon";

        public reyonclass(string no, string ad, string soyad) : base(no, ad, soyad)
        {
        }

        public override string kim => TIP;

        public override Form EkranOlustur()
        {
            return new personel_reyon(this);
        }

        // Termine la vente : enregistre le ticket et vide le panier
        // pour le client suivant. Renvoie null si le panier est vide.
        public Satis SatisYap(Sepet sepet)
        {
            if (sepet.Bos) return null;

            Satis satis = new Satis(sepet.Kalemler, this);
            foreach (SatisKalemi kalem in sepet.Kalemler)
            {
                Veriler.SatilanUrunler.Add(kalem);
            }
            Veriler.Satislar.Add(satis);
            sepet.Temizle();
            return satis;
        }
    }
}
