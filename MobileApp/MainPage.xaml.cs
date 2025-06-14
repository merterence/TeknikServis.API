namespace MobileApp
{
    public partial class MainPage : ContentPage
    {
        

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCallAPİButtonClicked(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://10.0.2.2:5122/api/Kullanici/1");
            var data = await response.Content.ReadAsStringAsync();
        }
    }

}
