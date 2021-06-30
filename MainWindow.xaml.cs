using Newtonsoft.Json.Linq;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TestApp1 {

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window {

        private JObject jObject;

        public MainWindow() {
            InitializeComponent();
        }

        private void getWeather() {
            var url = DownloadString(@"https://weather.tsukumijima.net/api/forecast/city/140010");
            jObject = JObject.Parse(url);

            setParameter();
            setImage();
        }

        private string DownloadString(string url) {
            using (var webClient = new System.Net.WebClient()) {
                webClient.Encoding = System.Text.Encoding.UTF8;
                return webClient.DownloadString(url).Replace("\n", "");

            }
        }

        private string getImagePath() {
            if (jObject["forecasts"][1]["telop"].ToString() == "晴れ") return @"./image/taiyou.png";
            if (jObject["forecasts"][1]["telop"].ToString() == "曇り") return @"./image/kumo.png";
            return @"./image/rain_ame.png";
        }

        private void setParameter() {
            daysTextBlock.Text = (jObject["forecasts"][1]["dateLabel"].ToString() + " " + jObject["forecasts"][1]["date"].ToString());
            titleTextBlock.Text = (jObject["title"].ToString());
            weatherTextBlock.Text = ("天気: " + jObject["forecasts"][1]["telop"].ToString());
            rainyTextBlock.Text = ("降水確率: " + jObject["forecasts"][1]["chanceOfRain"]["T00_06"].ToString());
            maxTextBlock.Text = ("最高気温: " + jObject["forecasts"][1]["temperature"]["max"]["celsius"].ToString() + "℃");
            minTextBlock.Text = ("最低気温: " + jObject["forecasts"][1]["temperature"]["min"]["celsius"].ToString() + "℃");
        }

        private void setImage() {
            ImageSource imageSource = new BitmapImage(new Uri(getImagePath(), UriKind.Relative));
            image.Source = imageSource;
        }

        private void TitleTextBlock_Loaded(object sender, RoutedEventArgs e) {
            getWeather();
        }



    }
}
