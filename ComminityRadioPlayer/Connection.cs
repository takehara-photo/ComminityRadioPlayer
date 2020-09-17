using ComminityRadioPlayer.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Vlc.DotNet.Wpf;

namespace ComminityRadioPlayer
{
   public static class Connection
    {
        public static ObservableCollection<Station> stations = new ObservableCollection<Station>();
        public static int countStations = 0;
        public static int completed = 0;

        public static List<Station> Get()
        {

            countStations = 0;
            completed = 1;

            var stationList = new List<Station>();
            var Folderpath = "img";
            //folder
            if (!Directory.Exists(Folderpath))
            {
                Directory.CreateDirectory(Folderpath);
            }
            //remove img
            DirectoryInfo target = new DirectoryInfo(Folderpath);
            //ファイル消す
            foreach (FileInfo file in target.GetFiles())
            {
                file.Delete();
            }


            WebClient wc = new WebClient();
            byte[] data = wc.DownloadData("https://www.jcbasimul.com/");
            Encoding enc = Encoding.GetEncoding("utf-8");
            var html = enc.GetString(data).Replace("\r\n", "\n").Split(new[] { '\n', '\r' });
            int i = 1;
             countStations = html.Count(b => b.Contains("https://www.jcbasimul.com/radio"));
            foreach (var item in html.Where(b => b.Contains("https://www.jcbasimul.com/radio")))
            {
                var station = new Station();

                var url = item.Replace("<div class=\"card_link\" id=\"", "").Replace("\"></div>", "").Trim();

                Console.WriteLine(i + ":" + url);
                i++;

                WebClient wc1 = new WebClient();
                byte[] data1 = wc1.DownloadData(url);
                Encoding enc1 = Encoding.GetEncoding("utf-8");
                var html1 = enc.GetString(data1).Replace("\r\n", "\n").Split(new[] { '\n', '\r' });

                foreach (var item1 in html1.Where(b => b.Contains("//") == false).Where(c => c.Contains("<h3>")))
                {
                    var name = item1.Replace("</h3>", "").Replace("<h3>", "").Trim();
                    Console.WriteLine("station:" + name);
                    station.Name = name;
                }

                //download logo img
                foreach (var item2 in html1.Where(b =>b.Contains("wp-content/uploads")&&b.Contains("https://") && (b.Contains(".jpg")|| b.Contains("png"))))
                {

                    var imgUrl = item2.Substring(item2.IndexOf("https://"));
                    imgUrl = imgUrl.Remove(imgUrl.IndexOf("\"") );

                    System.Net.WebClient wcd = new System.Net.WebClient();
                    wcd.DownloadFile(imgUrl,"img\\"+ station.filename) ;
                    wcd.Dispose();

                    break;
                }


                foreach (var item1 in html1.Where(c => c.Contains("https://musicbird.leanplayer.com")))
                {
                    var url1 = item1.Replace("<script type=\"text/javascript\" src=\"", "").Replace("\"></script>", "").Trim();
                    //  Console.WriteLine(url1);
                  

                    WebClient wc2 = new WebClient();
                    byte[] data2 = wc2.DownloadData(url1);
                    Encoding enc2 = Encoding.GetEncoding("utf-8");
                    var html2 = enc.GetString(data2).Replace("\r\n", "\n").Trim().Split(new[] { '\n', '\r', '<' });
                    foreach (var item3 in html2.Where(b => b.Contains("audio")))
                    {
                        var audio = item3.Replace("preload=\"none\">", "").Substring(item3.IndexOf("https://")).Replace("\"", "").Trim();
                        //Console.WriteLine(item3);
                        Console.WriteLine(audio);
                        station.Url = audio;
                        break;
                    }
                    completed++;
                    break;
                }
               stationList.Add(station);
              

            }
            return stationList;
        }

        private static void CreateDirectory(string folderpath, object disideOfSelsct)
        {
            throw new NotImplementedException();
        }
    }
}
