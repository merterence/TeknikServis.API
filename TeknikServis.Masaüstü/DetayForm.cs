using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TeknikServis.Masaüstü.Models;

namespace TeknikServis.Masaüstü
{
    public partial class DetayForm : Form
    {
        // ✅ DTO nesnesini saklayacağımız alan
        private readonly ServisTalebiDto _talep;

        // ✅ Constructor: Form açıldığında verileri alacak
        public DetayForm(ServisTalebiDto talep)
        {
            InitializeComponent();
            _talep = talep;

            // Verileri formdaki bileşenlere yerleştir
            txtAdSoyad.Text = _talep.KullaniciAdi;
            txtUrunAdi.Text = _talep.UrunAdi;
            txtAciklama.Text = _talep.Aciklama;
            txtDurum.Text = _talep.TalepDurumu;
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUrunAdi_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
