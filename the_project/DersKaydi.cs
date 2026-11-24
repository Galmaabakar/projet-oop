namespace the_project
{
    // Un cours choisi par un etudiant.
    public class DersKaydi
    {
        public string OgrenciNo { get; }
        public string OgrenciAdSoyad { get; }
        public Ders Ders { get; }

        public DersKaydi(ogrenci ogrenci, Ders ders)
        {
            OgrenciNo = ogrenci.no;
            OgrenciAdSoyad = ogrenci.ad + " " + ogrenci.soyad;
            Ders = ders;
        }

        public override string ToString()
        {
            return $"{OgrenciNo} {OgrenciAdSoyad}: {Ders.Ad} ({Ders.Kredi} kredi)";
        }
    }
}
