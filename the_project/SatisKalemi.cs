namespace the_project
{
    // Une ligne du ticket : un produit et son prix.
    public class SatisKalemi
    {
        public Urun Urun { get; }
        public int Fiat { get; }

        public SatisKalemi(Urun urun, int fiat)
        {
            Urun = urun;
            Fiat = fiat;
        }

        public override string ToString()
        {
            return $"{Urun}: {Fiat} TL";
        }
    }
}
