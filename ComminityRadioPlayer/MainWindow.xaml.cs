using ComminityRadioPlayer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vlc.DotNet.Wpf;

namespace ComminityRadioPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
            Settings.Read();
            if (Connection.stations.Any() == false)
            {
                GetStation();
            }

            this.SliderVolume.Value = Settings.setting.Volume;
            this.ListBox1.ItemsSource = Connection.stations;
            var vlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(System.Environment.CurrentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
            var options = new string[]
          {
              // VLC options can be given here. Please refer to the VLC command line documentation.
          };
            this.MyControl.SourceProvider.CreatePlayer(vlcLibDirectory, options);
            SetStationInfo();
        }

        private void ListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.setting.lastStation = ListBox1.SelectedItem as Station;
            Settings.Write();
            Play();
        }

        private void Play()
        {
            try
            {
                if (this.MyControl.SourceProvider.MediaPlayer.IsPlaying())
                {
                    this.MyControl.SourceProvider.MediaPlayer.Stop();
                }

            }
            catch (Exception)
            {


            }
            if (Settings.setting.lastStation == null)
            {
                Settings.setting.lastStation = Connection.stations.First();
            }
            if (Settings.setting.lastStation == null)
            {
                return;
            }
            this.ButtonPlay.IsEnabled = false;
            this.ButtonStop.IsEnabled = true;
            this.Title = "再生中: " + Settings.setting.lastStation.Name;



            // Load libvlc libraries and initializes stuff. It is important that the options (if you want to pass any) and lib directory are given before calling this method.
            this.MyControl.SourceProvider.MediaPlayer.Play(Settings.setting.lastStation.Url.Trim());
            SetVolume();
            SetStationInfo();
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            Play();
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {

            this.ButtonPlay.IsEnabled = true;
            this.ButtonStop.IsEnabled = false;
            try
            {
                if (this.MyControl.SourceProvider.MediaPlayer.IsPlaying())
                {
                    this.MyControl.SourceProvider.MediaPlayer.Stop();
                }
            }
            catch (Exception)
            {
            }
            SetStationInfo();
            this.Title = "停止中: " + Settings.setting.lastStation.Name;
        }

        private void ButtonGetStations_Click(object sender, RoutedEventArgs e)
        {
            GetStation();
        }

        private void GetStation()
        {
            var awin2 = new GetStations();
            awin2.ShowDialog();
            SetStationInfo();
        }

        private void SetStationInfo()
        {
            if (Settings.setting.lastStation == null)
            {
                Settings.setting.lastStation = Connection.stations.FirstOrDefault();
            }
            if (Settings.setting.lastStation == null)
            {
                return;
            }

            this.LabelSelectedRadio.Content = Settings.setting.lastStation.Name;



            if (System.IO.File.Exists(System.Environment.CurrentDirectory + "\\img\\" + Settings.setting.lastStation.filename))
            {
                ImageSelectedStation.Source = new BitmapImage(new Uri(System.Environment.CurrentDirectory + "\\img\\" + Settings.setting.lastStation.filename));
                this.Icon = new BitmapImage(new Uri(System.Environment.CurrentDirectory + "\\img\\" + Settings.setting.lastStation.filename));

            }

            this.ListBox1.ItemsSource = Connection.stations;


        }

        public void Dispose()
        {
            MyControl.Dispose();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MyControl.Dispose();

        }

        private void SliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Settings.setting.Volume = this.SliderVolume.Value;
            Settings.Write();
            SetVolume();
        }
        private void SetVolume()
        {
            if (this.MyControl.SourceProvider.MediaPlayer != null)
            {
                this.MyControl.SourceProvider.MediaPlayer.Audio.Volume = (int)(Settings.setting.Volume * 10);

            }
        }
        /// <summary>
        /// 放送局検索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //全表示
            if(TextBoxSearch.Text=="")
            {
                this.ListBox1.ItemsSource = Connection.stations;
            }
            //絞り込み
            else
            {
                this.ListBox1.ItemsSource = Connection.stations.Where(b =>b.Name.Contains(TextBoxSearch.Text));
            }
        }
    }
}
