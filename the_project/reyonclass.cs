using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace the_project
{
    public class reyonclass : gecis, giris
    {
        public string ad { get; set; }
        public string soyad { get; set; }
        private string _girisoldu;
        public override string girisoldu
        {
            get
            { return _girisoldu; }
            set
            {
                _girisoldu = value;

            }
        }
        string galma = "hosgeldiniz";
        public reyonclass(string no, string sifre, string kim) : base(no, sifre, kim)
        {
            no = no;
            if (sifre == "123456" && kim == "personel_reyon")
            {

                girisoldu = "giris basari ile tamamlandi";

            }
            else
            {
                girisoldu = "hatali giris ";

            }
        }
        //giris yapildiginda kimin yaptigi ekrana gostermek
        public string ToString(string no, string kim, string ad, string soyad)
        {
            return $"{no} nolu, Tip: {kim}, {ad} adli,{soyad} Soyadli giris yapti";
        }
    }
}
