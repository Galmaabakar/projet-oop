namespace the_project
{
    // Un cours choisi par un etudiant.
    public class DersKaydi
    {
        public string OgrenciNo { get; }
        public string OgrenciAdSoyad { get; }
        public Ders Ders { get; }

        public DersKaydi(ogrenci ogrenci, Ders ders)
            : this(ogrenci.no, ogrenci.ad + " " + ogrenci.soyad, ders)
        {
        }

        // utilise au chargement depuis la base
        public DersKaydi(string ogrenciNo, string ogrenciAdSoyad, Ders ders)
        {
            OgrenciNo = ogrenciNo;
            OgrenciAdSoyad = ogrenciAdSoyad;
            Ders = ders;
        }

        public override string ToString()
        {
            return $"{OgrenciNo} {OgrenciAdSoyad}: {Ders.Ad} ({Ders.Kredi} kredi)";
        }
    }
}
