using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTrain12306
{
    //nbo|宁波|NGH|ningbo|nb|942|0910|宁波|||
    public class StationInfo
    {
        public String station_name { get; set; }
        public String station_telecode;
        public String station_city;
        public String station_pinyin;
        public String station_short_pinyin;
        public StationInfo(String str)
        {
            String[] info = str.Split('|');
            station_name = info[1];
            station_telecode = info[2];
            station_pinyin = info[3];
            station_short_pinyin = info[4];
            station_city = info[7];
        }
    }
}
