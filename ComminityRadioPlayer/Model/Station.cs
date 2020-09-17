using System;
using System.Collections.Generic;
using System.Text;

namespace ComminityRadioPlayer.Model
{
    /// <summary>
    /// 放送局
    /// </summary>
  public  class Station
    {
        /// <summary>
        /// 放送局名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///お気に入り
        /// </summary>
        public bool Like { get; set; } = false;
        /// <summary>
        /// ストリーミングURL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// ロゴファイル名
        /// </summary>
        public string filename { get; set; } = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
    }
}
