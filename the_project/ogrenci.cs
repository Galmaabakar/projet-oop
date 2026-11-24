using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace the_project
{
    public class ogrenci : gecis
    {
        public const string TIP = "ogrenci";
        public const int KREDI_LIMITI = 39;

        public ogrenci(string no, string ad, string soyad) : base(no, ad, soyad)
        {
        }

        public override string kim => TIP;

        public override Form EkranOlustur()
        {
            return new ogrenciform(this);
        }

        // Les cours de CET etudiant (avant, les credits de tous les
        // etudiants etaient additionnes ensemble).
        public IEnumerable<DersKaydi> AldigiDersler
        {
            get { return Veriler.DersKayitlari.Where(k => k.OgrenciNo == no); }
        }

        public int ToplamKredi
        {
            get { return AldigiDersler.Sum(k => k.Ders.Kredi); }
        }

        public bool DersAl(Ders ders, out string mesaj)
        {
            if (ders == null)
            {
                mesaj = "bir ders secmeniz lazim";
                return false;
            }
            if (AldigiDersler.Any(k => k.Ders == ders))
            {
                mesaj = "bu dersi zaten aldiniz";
                return false;
            }
            int yeniToplam = ToplamKredi + ders.Kredi;
            if (yeniToplam > KREDI_LIMITI)
            {
                mesaj = $"bu ders ile {yeniToplam} kredi olur, limitiniz {KREDI_LIMITI}";
                return false;
            }

            Veriler.DersKayitlari.Add(new DersKaydi(this, ders));
            mesaj = $"dersiniz basari ile alindi (toplam {yeniToplam}/{KREDI_LIMITI} kredi)";
            return true;
        }
    }
}
