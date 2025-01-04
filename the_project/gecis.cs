using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace the_project
{
    public abstract class gecis
    {
        public string no {  get; set; }
        public string sifre {  get; set; }
        public string kim { get; set; }
        public abstract string girisoldu { set; get; }
        public gecis(string no,string sifre,string kim) 
        {
            this.no = no;
            this.sifre = sifre;

        }

    }
}
