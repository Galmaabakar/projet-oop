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
            if (comboBox1.SelectedItem == "ogrenci")
            {
                ogrenci ogrenci = new ogrenci(textBox3.Text, textBox4.Text, comboBox1.SelectedItem.ToString());
                string gal = ogrenci.ToString(textBox3.Text, comboBox1.SelectedItem.ToString(), textBox1.Text, textBox2.Text);
                string he = ogrenci.girisoldu;
                listt.list.Add(textBox1.Text);
                listt.list.Add(textBox3.Text);
                listt.list.Add(comboBox1.SelectedItem);
                var bildi = gal;
                label5.Text = he;
                if (he == "giris basari ile tamamlandi")
                {
                    MessageBox.Show(gal);
                    ogrencif.ShowDialog();
                }
            }
            else if(comboBox1.SelectedItem == "personel_kutuphane")
            {
                personel_kutuphaneclass kutuphane = new personel_kutuphaneclass(textBox3.Text, textBox4.Text, comboBox1.SelectedItem.ToString());
                string gal = kutuphane.ToString(textBox3.Text, comboBox1.SelectedItem.ToString(), textBox1.Text, textBox2.Text);
                string he = kutuphane.girisoldu;
                listt.list.Add(textBox1.Text);
                listt.list.Add(textBox3.Text);
                listt.list.Add(comboBox1.SelectedItem);
                var bildi = gal;
                label5.Text = he;
                if (he == "giris basari ile tamamlandi")
                {
                    MessageBox.Show(gal);
                    pkutuphane.ShowDialog();
                }
            }
            else if (comboBox1.SelectedItem == "personel_reyon")
            {
                reyonclass reyon = new reyonclass(textBox3.Text, textBox4.Text, comboBox1.SelectedItem.ToString());
                string gal = reyon.ToString(textBox3.Text, comboBox1.SelectedItem.ToString(), textBox1.Text, textBox2.Text);
                string he = reyon.girisoldu;
                listt.list.Add(textBox1.Text);
                listt.list.Add(textBox3.Text);
                listt.list.Add(comboBox1.SelectedItem);
                var bildi = gal;
                label5.Text = he;
                if (he == "giris basari ile tamamlandi")
                {
                    MessageBox.Show(gal);
                    preyon.ShowDialog();
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            yoneticif.ShowDialog();
        }
    }
}
