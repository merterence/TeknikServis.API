﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
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
            txtAdSoyad.Text = _talep.Kullanici.AdSoyad;
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

        private void DetayForm_Load(object sender, EventArgs e)
        {

        }

        private async void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                // 1️⃣ Güncellenen alanları DTO’ya yaz
                _talep.Aciklama = txtAciklama.Text;
                _talep.TalepDurumu = txtDurum.Text;

                // Eğer ürün adı da düzenlenebilir olacaksa ekleyebilirsin:
                _talep.UrunAdi = txtUrunAdi.Text;

                // 2️⃣ DTO'da null olan kritik alanları kontrol et
                if (_talep.KullaniciId == 0)
                {
                    MessageBox.Show("Kullanıcı ID boş olamaz.");
                    return;
                }

                // 3️⃣ DTO’yu JSON’a çevir ve PUT isteği yap
                var jsonData = JsonConvert.SerializeObject(_talep);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                string apiUrl = $"https://localhost:44365/api/ServisTalebi/{_talep.Id}";
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PutAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Talep başarıyla güncellendi!");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Güncelleme başarısız: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }


    }
}

