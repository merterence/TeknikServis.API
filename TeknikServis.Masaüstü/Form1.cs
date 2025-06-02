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
using Newtonsoft.Json;
using TeknikServis.Masaüstü.Models;

namespace TeknikServis.Masaüstü
{

    public partial class Form1 : Form
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private List<int> oncekiIdler = new List<int>();
        private List<ServisTalebiDto> tumTalepler = new List<ServisTalebiDto>();
        private readonly KullaniciDto _aktifKullanici;

        public Form1(KullaniciDto kullanici)
        {
            InitializeComponent();
            _aktifKullanici = kullanici;

            this.Load += Form1_Load;
            this.dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;

            // Giriş yapan kullanıcıyı mesaj olarak gösterebilirsin (opsiyonel)
            this.Text = $"Hoş geldin, {_aktifKullanici.AdSoyad}";
        }


        private async void YukleVerileri()
        {
            try
            {
                string apiUrl = _aktifKullanici.IsAdmin
            ? "https://localhost:44365/api/ServisTalebi"
            : $"https://localhost:44365/api/ServisTalebi/kullanici?id={_aktifKullanici.Id}";

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonData = await response.Content.ReadAsStringAsync();
                    var talepler = JsonConvert.DeserializeObject<List<ServisTalebiDto>>(jsonData);

                    dataGridView1.DataSource = talepler.Select(t => new
                    {
                        AdSoyad = t.Kullanici?.AdSoyad ?? "Bilinmiyor",
                        UrunAdi = t.UrunAdi,
                        Aciklama = t.Aciklama,
                        TalepDurumu = t.TalepDurumu,
                        TalepTarihi = t.TalepTarihi
                    }).ToList();

                    

                    //dataGridView1.AutoGenerateColumns = false;
                    //dataGridView1.Columns.Clear();

                    //var kullaniciAdSoyadColumn = new DataGridViewTextBoxColumn();
                    //kullaniciAdSoyadColumn.HeaderText = "Ad Soyad";
                    //kullaniciAdSoyadColumn.DataPropertyName = "Kullanici.AdSoyad";
                    //kullaniciAdSoyadColumn.Name = "AdSoyad";
                    //dataGridView1.Columns.Add(kullaniciAdSoyadColumn);

                    //var urunAdiColumn = new DataGridViewTextBoxColumn();
                    //urunAdiColumn.HeaderText = "Ürün Adı";
                    //urunAdiColumn.DataPropertyName = "UrunAdi";
                    //dataGridView1.Columns.Add(urunAdiColumn);

                    //var aciklamaColumn = new DataGridViewTextBoxColumn();
                    //aciklamaColumn.HeaderText = "Açıklama";
                    //aciklamaColumn.DataPropertyName = "Aciklama";
                    //dataGridView1.Columns.Add(aciklamaColumn);

                    //var talepDurumuColumn = new DataGridViewTextBoxColumn();
                    //talepDurumuColumn.HeaderText = "Durum";
                    //talepDurumuColumn.DataPropertyName = "TalepDurumu";
                    //dataGridView1.Columns.Add(talepDurumuColumn);

                    //var talepTarihiColumn = new DataGridViewTextBoxColumn();
                    //talepTarihiColumn.HeaderText = "Tarih";
                    //talepTarihiColumn.DataPropertyName = "TalepTarihi";
                    //dataGridView1.Columns.Add(talepTarihiColumn);

                    //dataGridView1.DataSource = tumTalepler.Select(t => new
                    //{
                    //    AdSoyad = t.Kullanici?.AdSoyad ?? "Bilinmiyor",
                    //    UrunAdi = t.UrunAdi,
                    //    Aciklama = t.Aciklama,
                    //    TalepDurumu = t.TalepDurumu,
                    //    TalepTarihi = t.TalepTarihi
                    //}).ToList();

                    //oncekiIdler = tumTalepler
                    //    .Where(t => t != null)
                    //    .Select(t => t.Id)
                    //    .ToList();
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
            YukleVerileri();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && tumTalepler.Count > 0)
            {
                var adSoyad = dataGridView1.Rows[e.RowIndex].Cells["AdSoyad"].Value?.ToString();

                var selectedTalep = tumTalepler
                    .FirstOrDefault(t => t.Kullanici?.AdSoyad == adSoyad);

                if (selectedTalep != null)
                {
                    DetayForm detayForm = new DetayForm(selectedTalep);
                    var result = detayForm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        YukleVerileri();
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Opsiyonel
        }

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

                    var yeniTalepler = talepler
                        .Where(t => t != null && !oncekiIdler.Contains(t.Id))
                        .ToList();

                    if (yeniTalepler.Any())
                    {
                        System.Media.SystemSounds.Asterisk.Play();

                        dataGridView1.DataSource = talepler.Select(t => new
                        {
                            AdSoyad = t.Kullanici?.AdSoyad ?? "Bilinmiyor",
                            UrunAdi = t.UrunAdi,
                            Aciklama = t.Aciklama,
                            TalepDurumu = t.TalepDurumu,
                            TalepTarihi = t.TalepTarihi
                        }).ToList();

                        oncekiIdler = talepler
                            .Where(t => t != null)
                            .Select(t => t.Id)
                            .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }

        private void txtUrunAdi_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
