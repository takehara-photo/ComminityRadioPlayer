using ComminityRadioPlayer.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ComminityRadioPlayer
{
    public static class Settings
    {
        public static SettingModel setting = new SettingModel();

        public static string filename = "setting.json";

        public static void Write()
        {
            foreach (var item in setting.Stations.Where(b =>b.filename.Contains("\\")))
            {
  
                    item.filename = item.filename.Substring(item.filename.IndexOf("\\img\\"));
                    item.filename = item.filename.Replace("\\img\\", "");
              
            }
            //setting.lastStation.filename=setting.lastStation.filename.Replace(System.Environment.CurrentDirectory + "\\img\\", "");
            if (setting.lastStation.filename.Contains("\\"))
            {
                setting.lastStation.filename = setting.lastStation.filename.Substring(setting.lastStation.filename.IndexOf("\\img\\"));
                setting.lastStation.filename = setting.lastStation.filename.Replace("\\img\\", "");

            }

            foreach (var item in setting.Stations)
            {
                item.Url = item.Url.Trim();
            }

            //書込
            Encoding enc = Encoding.GetEncoding("utf-8");
            StreamWriter writer = new StreamWriter(filename, false, enc);
            writer.WriteLine(JsonConvert.SerializeObject(setting));
            writer.Close();

            //Read();
        }

        public static void Read()
        {

            if (System.IO.File.Exists(filename))
            {
                try
                {
                    StreamReader sr = new StreamReader(filename, Encoding.GetEncoding("utf-8"));
                    setting = JsonConvert.DeserializeObject<SettingModel>(sr.ReadToEnd());
                    sr.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        
            Connection.stations.Clear();
            foreach (var item in setting.Stations)
            {
                var a = new Station();
                a.filename = System.Environment.CurrentDirectory + "\\img\\" + item.filename;
                a.Name = item.Name;
                a.Url = item.Url;
                Connection.stations.Add(a);
            }
            Console.WriteLine("read");
           
        }


    }

    public class SettingModel
    {
        public List<Station> Stations { get; set; } = new List<Station>();
        public Station lastStation { get; set; } = new Station();
        public double Volume { get; set; } = 100.0;
    }

}
