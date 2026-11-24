namespace the_project
{
    // Un outil que le personnel peut preter.
    public class Alet
    {
        public string Ad { get; }
        public string Numara { get; }
        // null quand l'outil est disponible
        public string OduncAlan { get; set; }

        public bool Musait => OduncAlan == null;

        public Alet(string ad, string numara)
        {
            Ad = ad;
            Numara = numara;
        }

        public override string ToString()
        {
            return Musait ? $"{Ad} - {Numara}" : $"{Ad} - {Numara} (odunc: {OduncAlan})";
        }
    }
}
