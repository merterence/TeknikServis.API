using Newtonsoft.Json;
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
    public partial class LoginForm: Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private async void btnGiris_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string sifre = txtSifre.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sifre))
            {
                MessageBox.Show("Email ve şifre boş olamaz!");
                return;
            }

            using (var httpClient = new HttpClient())
            {
                string url = $"https://localhost:44365/api/Kullanici/login?email={email}&sifre={sifre}";

                try
                {
                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var kullanici = JsonConvert.DeserializeObject<KullaniciDto>(json);

                        if (kullanici != null)
                        {
                            if (kullanici.IsAdmin)
                            {
                                Form1 adminPanel = new Form1(kullanici);
                                adminPanel.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Sadece admin kullanıcılar giriş yapabilir.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı bulunamadı.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Giriş başarısız. Hatalı email veya şifre.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }
    }
}

