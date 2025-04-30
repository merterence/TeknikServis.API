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
        private readonly HttpClient _httpClient = new HttpClient();
        private List<int> oncekiIdler = new List<int>();

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            this.dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
        }

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
                    dataGridView1.Columns.Clear();

                    var kullaniciAdSoyadColumn = new DataGridViewTextBoxColumn();
                    kullaniciAdSoyadColumn.HeaderText = "Ad Soyad";
                    kullaniciAdSoyadColumn.DataPropertyName = "Kullanici.AdSoyad";
                    dataGridView1.Columns.Add(kullaniciAdSoyadColumn);

                    var urunAdiColumn = new DataGridViewTextBoxColumn();
                    urunAdiColumn.HeaderText = "Ürün Adı";
                    urunAdiColumn.DataPropertyName = "UrunAdi";
                    dataGridView1.Columns.Add(urunAdiColumn);

                    var aciklamaColumn = new DataGridViewTextBoxColumn();
                    aciklamaColumn.HeaderText = "Açıklama";
                    aciklamaColumn.DataPropertyName = "Aciklama";
                    dataGridView1.Columns.Add(aciklamaColumn);

                    var talepDurumuColumn = new DataGridViewTextBoxColumn();
                    talepDurumuColumn.HeaderText = "Durum";
                    talepDurumuColumn.DataPropertyName = "TalepDurumu";
                    dataGridView1.Columns.Add(talepDurumuColumn);

                    var talepTarihiColumn = new DataGridViewTextBoxColumn();
                    talepTarihiColumn.HeaderText = "Tarih";
                    talepTarihiColumn.DataPropertyName = "TalepTarihi";
                    dataGridView1.Columns.Add(talepTarihiColumn);

                    dataGridView1.DataSource = talepler;

                    // ✅ Null güvenliği ile mevcut ID'leri kaydet
                    if (talepler != null && talepler.Any())
                    {
                        oncekiIdler = talepler
                            .Where(t => t != null)
                            .Select(t => t.Id)
                            .ToList();
                    }
                    else
                    {
                        oncekiIdler = new List<int>();
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

        private void Form1_Load(object sender, EventArgs e)
        {
            YukleVerileri();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dataGridView1.Rows[e.RowIndex].DataBoundItem as ServisTalebiDto;

                if (selectedRow != null)
                {
                    DetayForm detayForm = new DetayForm(selectedRow);
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
            // İsteğe bağlı
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
                        dataGridView1.DataSource = talepler;

                        // ✅ Null güvenliği ile ID'leri güncelle
                        if (talepler != null && talepler.Any())
                        {
                            oncekiIdler = talepler
                                .Where(t => t != null)
                                .Select(t => t.Id)
                                .ToList();
                        }
                        else
                        {
                            oncekiIdler = new List<int>();
                        }
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
