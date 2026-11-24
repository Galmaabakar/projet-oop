using System.ComponentModel;
using System.Linq;

namespace the_project
{
    // Remplace l'ancienne classe listt : chaque liste a un vrai type
    // au lieu de List<object>. Ce sont des BindingList, donc les
    // ListBox/ComboBox qui les affichent se mettent a jour toutes seules.
    public static class Veriler
    {
        public static readonly BindingList<GirisKaydi> Girisler = new BindingList<GirisKaydi>();

        // ---- Ogrenci
        public static readonly BindingList<Ders> Dersler = new BindingList<Ders>
        {
            new Ders("Nesne Yonelimli Programlama", "BIL201", 6),
            new Ders("Veri Yapilari", "BIL202", 6),
            new Ders("Veritabani Sistemleri", "BIL301", 5),
            new Ders("Matematik I", "MAT101", 6),
            new Ders("Fizik I", "FIZ101", 5),
            new Ders("Olasilik ve Istatistik", "MAT201", 4),
            new Ders("Ingilizce", "ING101", 3),
            new Ders("Bilgisayar Aglari", "BIL302", 5),
            new Ders("Isletim Sistemleri", "BIL303", 5)
        };
        public static readonly BindingList<DersKaydi> DersKayitlari = new BindingList<DersKaydi>();

        // ---- Kutuphane
        public static readonly BindingList<Kitap> Kitaplar = new BindingList<Kitap>
        {
            new Kitap("Dusunceler", "Blaise Pascal", true),
            new Kitap("Savas Sanati", "Sun Tzu", true),
            new Kitap("Emile ya da Egitim Uzerine", "Jean-Jacques Rousseau", true),
            new Kitap("Bilimsel Zihniyet", "Gaston Bachelard", true),
            new Kitap("Kapital", "Karl Marx", true),
            new Kitap("Saf Aklin Elestirisi", "Immanuel Kant", true),
            new Kitap("Devlet (Politeia)", "Platon", true),
            new Kitap("Ikinci Cins", "Simone de Beauvoir", true),
            new Kitap("Nasil Dost Edinilir", "Dale Carnegie", false),
            new Kitap("Zengin Baba Yoksul Baba", "Robert Kiyosaki", false),
            new Kitap("Sinirsiz Guc", "Anthony Robbins", false),
            new Kitap("Pozitif Dusunmek", "James Allen", false)
        };
        public static readonly BindingList<KitapAlimi> KitapAlimlari = new BindingList<KitapAlimi>();

        // ---- Reyon
        public static readonly BindingList<Urun> Urunler = new BindingList<Urun>
        {
            new Urun("Macun", "37481", true),
            new Urun("Şampuan", "25830", true),
            new Urun("Ekmek", "49702", true),
            new Urun("Yumurta", "13276", true),
            new Urun("Süt", "78456", true),
            new Urun("Peynir", "96083", true),
            new Urun("Çay", "20457", true),
            new Urun("Bisküvi", "36412", true),
            new Urun("Kek", "29104", true),
            new Urun("Sosis", "84312", true),
            new Urun("Meyve", "68291", true),
            new Urun("Sebze", "47530", true),
            new Urun("Domates", "81652", true),
            new Urun("Patates", "10739", true),
            new Urun("Elma", "32509", true),
            new Urun("Portakal", "50283", true),
            new Urun("Limon", "41875", true),
            new Urun("Zeytin", "62547", true),
            new Urun("Bal", "79016", true),
            new Urun("Fındık", "24380", true),
            new Urun("Kavun", "95623", true),
            new Urun("Karpuz", "58342", true),
            new Urun("Makarna", "67291", true),
            new Urun("Pirinç", "13257", true),
            new Urun("Tuz", "30658", true),
            new Urun("Şeker", "84912", true),
            new Urun("Kahve", "46125", true),
            new Urun("Fasulye", "32704", true),
            new Urun("Biber", "29165", true),
            new Urun("Sarımsak", "80416", true),
            // Produits attendus dans une semaine (avant : des livres par erreur)
            new Urun("Yoğurt", "57204", false),
            new Urun("Tereyağı", "63158", false),
            new Urun("Un", "21876", false),
            new Urun("Mercimek", "39542", false)
        };
        public static readonly BindingList<SatisKalemi> SatilanUrunler = new BindingList<SatisKalemi>();
        public static readonly BindingList<Satis> Satislar = new BindingList<Satis>();

        // ---- Alet
        public static readonly BindingList<Alet> Aletler = new BindingList<Alet>
        {
            new Alet("Matkap", "50101"),
            new Alet("Tornavida seti", "50102"),
            new Alet("Çekiç", "50103"),
            new Alet("Pense", "50104"),
            new Alet("Multimetre", "50105"),
            new Alet("Lehim makinesi", "50106")
        };
        public static readonly BindingList<AletOdunc> AletOdunclari = new BindingList<AletOdunc>();

        public static Urun UrunBul(string ad, string numara)
        {
            return Urunler.FirstOrDefault(u => u.Eslesir(ad, numara));
        }
    }
}
