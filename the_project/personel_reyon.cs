using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace the_project
{
    public partial class personel_reyon : Form
    {
        public personel_reyon()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string urun = textBox1.Text.Trim() + " - " + textBox2.Text.Trim();
            if (comboBox1.Items.Contains(urun))
            {
                // int.Parse plantait si le prix etait vide ou non numerique
                int fiat;
                if (!int.TryParse(textBox3.Text, out fiat) || fiat < 0)
                {
                    MessageBox.Show("gecerli bir fiat giriniz");
                    return;
                }
                listt.fiat.Add(fiat);
                listt.alinanurun.Add(urun);
                MessageBox.Show("urununuz alindi");
            }
            else if (comboBox2.Items.Contains(urun))
            {
                MessageBox.Show("urunuz stokta yoktur ,bir hafta icinde gelecek");
            }
            else { MessageBox.Show("istediginiz urun bizde yoktur"); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listt.fiat.Count == 0)
            {
                MessageBox.Show("henuz urun alinmadi");
                return;
            }
            int toplam = listt.fiat.Sum();
            MessageBox.Show(toplam.ToString());
            listt.toplamfiat.Add(toplam);
            // on remet a zero pour le client suivant, sinon chaque clic
            // re-additionnait les memes produits dans toplamfiat
            listt.fiat.Clear();
        }
    }
}
