using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using TeknikServis.Masaüstü.Models;

namespace TeknikServis.Masaüstü
{
    public partial class Form1 : Form
    {
        // ✅ AŞAMA 1: HttpClient nesnesini buraya ekledik
        private readonly HttpClient _httpClient = new HttpClient();

        public Form1()
        {
            InitializeComponent();

            // 📌 Form yüklendiğinde çalışacak olan metodu bağla
            this.Load += Form1_Load;

            // 📌 CellDoubleClick eventini bağla
            this.dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string apiUrl = "https://localhost:44365/api/ServisTalebi";

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonData = await response.Content.ReadAsStringAsync();
                    var talepler = JsonConvert.DeserializeObject<List<ServisTalebiDto>>(jsonData);

                    if (talepler.Count > 0)
                    {
                        dataGridView1.DataSource = talepler;
                        dataGridView1.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                        dataGridView1.Columns["Aciklama"].HeaderText = "Açıklama";
                        dataGridView1.Columns["KullaniciAdi"].HeaderText = "Ad Soyad";
                        dataGridView1.Columns["Email"].HeaderText = "E-Posta";
                        dataGridView1.Columns["Adres"].HeaderText = "Adres";
                        dataGridView1.Columns["TalepDurumu"].HeaderText = "Talep Durumu";
                        dataGridView1.Columns["TalepTarihi"].HeaderText = "Tarih";
                    }
                }
                else
                {
                    MessageBox.Show("Veri çekilemedi: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }

        // ✅ Yeni eklenen: Çift tıklanınca detay formunu göster
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dataGridView1.Rows[e.RowIndex].DataBoundItem as ServisTalebiDto;

                if (selectedRow != null)
                {
                    DetayForm detayForm = new DetayForm(selectedRow);
                    detayForm.ShowDialog(); // Modal olarak aç
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Gerekirse bu kalsın, şimdilik boş
        }
    }
}
