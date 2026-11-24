using System;

namespace the_project
{
    public class Ders
    {
        public string Ad { get; }
        public string No { get; }
        public int Kredi { get; }

        public Ders(string ad, string no, int kredi)
        {
            if (string.IsNullOrWhiteSpace(ad)) throw new ArgumentException("ders adi bos olamaz");
            if (kredi <= 0) throw new ArgumentException("kredi pozitif olmali");
            Ad = ad.Trim();
            No = (no ?? "").Trim();
            Kredi = kredi;
        }

        public override string ToString()
        {
            return No == "" ? Ad : $"{No} - {Ad}";
        }
    }
}
