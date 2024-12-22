using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTrain12306
{
    class TrainInfo
    {
        public String station_train_code { get; set; }
        public String start_station_telecode { get; set; }
        public String end_station_telecode { get; set; }
        public String from_station_telecode { get; set; }
        public String to_station_telecode { get; set; }
        public String start_station_name { get; set; }
        public String end_station_name { get; set; }
        public String from_station_name { get; set; }
        public String to_station_name { get; set; }
        public String start_time { get; set; }
        public String arrive_time { get; set; }
        public short day_difference { get; set; }
        public String lishi { get; set; }
        public string rw_num { get; set; }
        public string srrb_num { get; set; }
        public string gg_num { get; set; }
        public string gr_num { get; set; }
        public string rz_num { get; set; }
        public string tz_num { get; set; }
        public string wz_num { get; set; }
        public string yb_num { get; set; }
        public string yw_num { get; set; }
        public string yz_num { get; set; }
        public string ze_num { get; set; }
        public string zy_num { get; set; }
        public string swz_num { get; set; }
        public string wz_seat_type_code { get; set; }
        public string yz_price { get; set; }
        public string rz_price { get; set; }
        public string yw_price { get; set; }
        public string rw_price { get; set; }
        public string gr_price { get; set; }
        public string zy_price { get; set; }
        public string ze_price { get; set; }
        public string tz_price { get; set; }
        public string gg_price { get; set; }
        public string yb_price { get; set; }
        public string bxyw_num { get; set; }
        public string bxyw_price { get; set; }
        public string swz_price { get; set; }
        public string hbyz_num { get; set; }
        public string hbyz_price { get; set; }
        public string hbyw_num { get; set; }
        public string hbyw_price { get; set; }
        public string bxrz_num { get; set; }
        public string bxrz_price { get; set; }
        public string tdrz_num { get; set; }
        public string tdrz_price { get; set; }
        public string srrb_price { get; set; }
        public string errb_num { get; set; }
        public string errb_price { get; set; }
        public string yrrb_num { get; set; }
        public string yrrb_price { get; set; }
        public string ydsr_num { get; set; }
        public string ydsr_price { get; set; }
        public string edsr_num { get; set; }
        public string edsr_price { get; set; }
        public string hbrz_num { get; set; }
        public string hbrz_price { get; set; }
        public string hbrw_num { get; set; }
        public string hbrw_price { get; set; }
        public string ydrz_num { get; set; }
        public string ydrz_price { get; set; }
        public string edrz_num { get; set; }
        public string edrz_price { get; set; }
        public string wz_price { get; set; }
        public string add_day_display { get; set; }

        public List<SeatData> SeatDatas { set; get; }

        public TrainInfo(String json)
        {
            
        }

        public static TrainInfo GetTrainInfo(String json)
        {
            var info = JsonConvert.DeserializeObject<TrainInfo>(json);
            info.SeatDatas = SeatData.GetSeatDatas(info);
            info.init_add_day_display();
            return info;
        }

        private void init_add_day_display()
        {
            if (day_difference == 0)
                add_day_display = "";
            else
                add_day_display = "+" + day_difference;
        }
    }
}
