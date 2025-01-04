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
        public ogrenciform()
        {
            InitializeComponent();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ogrenci ders listesine ders eklemesi icin
            comboBox1.Items.Add(textBox1.Text);
            MessageBox.Show("ders basari ile eklendi");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ogrenci ders secimi icin
            if (comboBox1.SelectedItem == " ") { MessageBox.Show("bir ders secmeniz lazim"); }
            else {
                MessageBox.Show("dersiniz basari ile alindi");
                listt.ders.Add(textBox7.Text);
                listt.ders.Add(textBox8.Text);
                listt.ders.Add(comboBox1.SelectedText);
                listt.kredi.Add(int.Parse(textBox5.Text));
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //ogrenciinin fazla ders secmemesi icin
            int ga = 0;
            foreach (var i in listt.kredi)
            {
                ga = ga + i;
            }
            if (ga >= 40) 
            {
                MessageBox.Show($"{ga} kadar kredili ders alamazsiniz limitiiz 39");
                listt.ders.Clear();
                listt.kredi.Clear();
            }
            else { MessageBox.Show("basari ile kaydedildi"); }
        }
    }
}
