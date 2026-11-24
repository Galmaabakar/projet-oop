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
    public partial class ogrenciform : Form
    {
        private const int KREDI_LIMITI = 39;

        public ogrenciform()
        {
            InitializeComponent();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ogrenci ders listesine ders eklemesi icin
            string yeniDers = textBox1.Text.Trim();
            if (yeniDers == "")
            {
                MessageBox.Show("ders adi bos olamaz");
                return;
            }
            if (comboBox1.Items.Contains(yeniDers))
            {
                MessageBox.Show("bu ders zaten listede");
                return;
            }
            comboBox1.Items.Add(yeniDers);
            MessageBox.Show("ders basari ile eklendi");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ogrenci ders secimi icin
            // SelectedItem vaut null quand rien n'est choisi (jamais " ")
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("bir ders secmeniz lazim");
                return;
            }
            // int.Parse plantait si la case kredi etait vide ou non numerique
            int kredi;
            if (!int.TryParse(textBox5.Text, out kredi) || kredi <= 0)
            {
                MessageBox.Show("gecerli bir kredi giriniz");
                return;
            }
            // on verifie la limite AVANT d'ajouter le cours
            int toplam = listt.kredi.Sum();
            if (toplam + kredi > KREDI_LIMITI)
            {
                MessageBox.Show($"bu ders ile {toplam + kredi} kredi olur, limitiniz {KREDI_LIMITI}");
                return;
            }

            listt.ders.Add(textBox7.Text);
            listt.ders.Add(textBox8.Text);
            // SelectedText = texte surligne dans la zone d'edition (souvent vide) -> SelectedItem
            listt.ders.Add(comboBox1.SelectedItem.ToString());
            listt.kredi.Add(kredi);
            MessageBox.Show("dersiniz basari ile alindi");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //ogrenciinin fazla ders secmemesi icin
            int ga = listt.kredi.Sum();
            if (ga > KREDI_LIMITI)
            {
                MessageBox.Show($"{ga} kadar kredili ders alamazsiniz limitiniz {KREDI_LIMITI}");
                listt.ders.Clear();
                listt.kredi.Clear();
            }
            else { MessageBox.Show($"basari ile kaydedildi, toplam kredi: {ga}"); }
        }
    }
}
