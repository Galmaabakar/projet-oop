using System.Linq;
using System.Windows.Forms;

namespace the_project
{
    public class personel_kutuphaneclass : gecis
    {
        public const string TIP = "personel_kutuphane";

        public personel_kutuphaneclass(string no, string ad, string soyad) : base(no, ad, soyad)
        {
        }

        public override string kim => TIP;

        public override Form EkranOlustur()
        {
            return new personel_kutuphane(this);
        }

        // Ajoute un livre au catalogue ; renvoie null si c'est impossible.
        public Kitap KitapEkle(string ad, string yazar, out string mesaj)
        {
            ad = (ad ?? "").Trim();
            yazar = (yazar ?? "").Trim();
            if (ad == "" || yazar == "")
            {
                mesaj = "kitap adi ve yazar bos olamaz";
                return null;
            }
            Kitap kitap = new Kitap(ad, yazar, false);
            if (Veriler.Kitaplar.Any(k => k.AyniKitapMi(kitap)))
            {
                mesaj = "bu kitap zaten kayitli";
                return null;
            }
            Veriler.Kitaplar.Add(kitap);
            mesaj = "kitabiniz basari ile kaydedildi";
            return kitap;
        }

        public void KitapVer(Kitap kitap)
        {
            Veriler.KitapAlimlari.Add(new KitapAlimi(kitap, this));
        }
    }
}
