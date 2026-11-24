using System.Linq;
using System.Windows.Forms;

namespace the_project
{
    // Personnel charge du pret des outils (aletler).
    public class personel_aletclass : gecis
    {
        public const string TIP = "personel_alet";

        public personel_aletclass(string no, string ad, string soyad) : base(no, ad, soyad)
        {
        }

        public override string kim => TIP;

        public override Form EkranOlustur()
        {
            return new personel_alet(this);
        }

        public bool OduncVer(Alet alet, string alanKisi, out string mesaj)
        {
            alanKisi = (alanKisi ?? "").Trim();
            if (alet == null)
            {
                mesaj = "bir alet seciniz";
                return false;
            }
            if (alanKisi == "")
            {
                mesaj = "aleti alan kisinin adini giriniz";
                return false;
            }
            if (!alet.Musait)
            {
                mesaj = $"{alet.Ad} zaten {alet.OduncAlan} kisisinde";
                return false;
            }

            alet.OduncAlan = alanKisi;
            Veriler.AletOdunclari.Add(new AletOdunc(alet, alanKisi, this));
            mesaj = $"{alet.Ad}, {alanKisi} kisisine odunc verildi";
            return true;
        }

        public bool IadeAl(Alet alet, out string mesaj)
        {
            if (alet == null)
            {
                mesaj = "bir alet seciniz";
                return false;
            }
            if (alet.Musait)
            {
                mesaj = $"{alet.Ad} zaten depoda";
                return false;
            }

            AletOdunc odunc = Veriler.AletOdunclari.Last(o => o.Alet == alet && !o.IadeEdildi);
            odunc.IadeEt();
            // la ligne a change : on previent les listes affichees
            Veriler.AletOdunclari.ResetItem(Veriler.AletOdunclari.IndexOf(odunc));
            mesaj = $"{alet.Ad}, {alet.OduncAlan} kisisinden geri alindi";
            alet.OduncAlan = null;
            return true;
        }
    }
}
