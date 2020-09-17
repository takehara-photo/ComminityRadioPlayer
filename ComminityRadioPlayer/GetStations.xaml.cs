using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ComminityRadioPlayer
{
    /// <summary>
    /// GetStations.xaml の相互作用ロジック
    /// </summary>
    public partial class GetStations : Window
    {/// <summary>
     /// 放送局取得
     /// </summary>
        public GetStations()
        {
            InitializeComponent();
            SetupTimer();
            Task task = Task.Run(() =>
            {
                this.Dispatcher.Invoke((Action)(() =>
                  {
                      Connection.stations.Clear();
                  }));

                var a = Connection.Get();
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Connection.stations.Clear();
                    foreach (var item in a)
                    {
                        Connection.stations.Add(item);
                    }
                    Settings.setting.Stations = a;
                    Settings.Write();
                    Settings.Read();
                    this.Close();
                }));

            });
        }
        private void MyTimerMethod(object sender, EventArgs e)
        {
            this.LabelCountAll.Content = Connection.countStations;
            this.LabelNow.Content = Connection.completed;
            try
            {
                if (Connection.completed != 0 && Connection.countStations != 0)
                {
                    this.ProgressBar1.Value = double.Parse(Connection.completed.ToString()) / double.Parse(Connection.countStations.ToString()) * 100;
                }
                else
                {
                    this.ProgressBar1.Value = 0;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private DispatcherTimer _timer;

        // タイマを設定する
        private void SetupTimer()
        {

            _timer = new DispatcherTimer();
            //インターバル
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += new EventHandler(MyTimerMethod);
            _timer.Start();

            // 画面が閉じられるときに、タイマを停止
            this.Closing += new CancelEventHandler(StopTimer);
        }
        private void StopTimer(object sender, CancelEventArgs e)
        {
            _timer.Stop();

        }
    }
}
