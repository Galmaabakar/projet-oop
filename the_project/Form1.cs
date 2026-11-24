using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace the_project
{
    public partial class Form1 : Form
    {
        ogrenciform ogrencif = new ogrenciform();
        yonetici yoneticif = new yonetici();
        personel_reyon preyon = new personel_reyon();
        personel_kutuphane pkutuphane = new personel_kutuphane();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // SelectedItem est un object : on le convertit en string pour comparer les valeurs
            // (et non les references), et on gere le cas ou rien n'est selectionne.
            string kim = comboBox1.SelectedItem as string;
            if (string.IsNullOrEmpty(kim))
            {
                MessageBox.Show("Lutfen bir giris tipi seciniz");
                return;
            }

            gecis kullanici;
            Form acilacakForm;
            if (kim == "ogrenci")
            {
                kullanici = new ogrenci(textBox3.Text, textBox4.Text, kim);
                acilacakForm = ogrencif;
            }
            else if (kim == "personel_kutuphane")
            {
                kullanici = new personel_kutuphaneclass(textBox3.Text, textBox4.Text, kim);
                acilacakForm = pkutuphane;
            }
            else if (kim == "personel_reyon")
            {
                kullanici = new reyonclass(textBox3.Text, textBox4.Text, kim);
                acilacakForm = preyon;
            }
            else
            {
                // "personel_alet" est dans la liste mais n'a aucun formulaire
                MessageBox.Show($"{kim} icin henuz bir ekran yok");
                return;
            }

            giris kisi = (giris)kullanici;
            kisi.ad = textBox1.Text;
            kisi.soyad = textBox2.Text;

            label5.Text = kullanici.girisoldu;
            if (kullanici.girisoldu == "giris basari ile tamamlandi")
            {
                // on n'enregistre que les connexions reussies
                listt.list.Add(textBox1.Text);
                listt.list.Add(textBox3.Text);
                listt.list.Add(kim);
                MessageBox.Show($"{textBox3.Text} nolu, Tip: {kim}, {textBox1.Text} adli,{textBox2.Text} Soyadli giris yapti");
                acilacakForm.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            yoneticif.ShowDialog();
        }
    }
}
