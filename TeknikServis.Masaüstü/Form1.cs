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

        // ✅ AŞAMA 2: Önceki ID'leri tutmak için liste oluşturduk
        private List<int> oncekiIdler = new List<int>();

        public Form1()
        {
            InitializeComponent();

            // 📌 Form yüklendiğinde çalışacak olan metodu bağla
            this.Load += Form1_Load;

            // 📌 CellDoubleClick eventini bağla
            this.dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
        }

        // ✅ YENİ: YukleVerileri() metodunu ekliyoruz
        private async void YukleVerileri()
        {
            try
            {
                string apiUrl = "https://localhost:44365/api/ServisTalebi";

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonData = await response.Content.ReadAsStringAsync();
                    var talepler = JsonConvert.DeserializeObject<List<ServisTalebiDto>>(jsonData);

                    dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.Columns.Clear(); // Eski kolonları temizle

                    // Kullanıcı Adı Kolonu
                    var kullaniciAdSoyadColumn = new DataGridViewTextBoxColumn();
                    kullaniciAdSoyadColumn.HeaderText = "Ad Soyad";
                    kullaniciAdSoyadColumn.DataPropertyName = "Kullanici.AdSoyad";
                    dataGridView1.Columns.Add(kullaniciAdSoyadColumn);

                    // Ürün Adı Kolonu
                    var urunAdiColumn = new DataGridViewTextBoxColumn();
                    urunAdiColumn.HeaderText = "Ürün Adı";
                    urunAdiColumn.DataPropertyName = "UrunAdi";
                    dataGridView1.Columns.Add(urunAdiColumn);

                    // Açıklama Kolonu
                    var aciklamaColumn = new DataGridViewTextBoxColumn();
                    aciklamaColumn.HeaderText = "Açıklama";
                    aciklamaColumn.DataPropertyName = "Aciklama";
                    dataGridView1.Columns.Add(aciklamaColumn);

                    // Talep Durumu Kolonu
                    var talepDurumuColumn = new DataGridViewTextBoxColumn();
                    talepDurumuColumn.HeaderText = "Durum";
                    talepDurumuColumn.DataPropertyName = "TalepDurumu";
                    dataGridView1.Columns.Add(talepDurumuColumn);

                    // Talep Tarihi Kolonu
                    var talepTarihiColumn = new DataGridViewTextBoxColumn();
                    talepTarihiColumn.HeaderText = "Tarih";
                    talepTarihiColumn.DataPropertyName = "TalepTarihi";
                    dataGridView1.Columns.Add(talepTarihiColumn);

                    dataGridView1.DataSource = talepler;

                    // ✅ Mevcut ID'leri listeye kaydet
                    oncekiIdler = talepler.Select(t => t.Id).ToList();
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

        private void Form1_Load(object sender, EventArgs e)
        {
            YukleVerileri(); // 📌 Yalnızca YukleVerileri metodunu çağırıyoruz
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
                    var result = detayForm.ShowDialog(); // ➡️ dikkat

                    if (result == DialogResult.OK)
                    {
                        // 📌 Eğer güncelleme yapılmışsa listeyi yenile
                        YukleVerileri();
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Gerekirse bu kalsın, şimdilik boş
        }

        // ✅ AŞAMA 3: Timer ile sürekli kontrol ve bildirim sesi
        private async void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                string apiUrl = "https://localhost:44365/api/ServisTalebi";
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonData = await response.Content.ReadAsStringAsync();
                    var talepler = JsonConvert.DeserializeObject<List<ServisTalebiDto>>(jsonData);

                    var yeniTalepler = talepler.Where(t => !oncekiIdler.Contains(t.Id)).ToList();

                    if (yeniTalepler.Any())
                    {
                        // 🔊 Basit bildirim sesi
                        System.Media.SystemSounds.Asterisk.Play();

                        // Listeyi güncelle
                        dataGridView1.DataSource = talepler;
                        oncekiIdler = talepler.Select(t => t.Id).ToList();
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
