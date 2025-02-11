using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTrain12306.Model
{
    public class TrainPassInfo
    {
        public string Date { get; set; }
        public string from_station { get; set; }
        public string station_train_code { get; set; }
        public string to_station { get; set; }
        public int total_num { get; set; }
        public string train_no { get; set; }
    }

    public class ApiResponse
    {
        public bool Status { get; set; }
        public string ErrorMsg { get; set; }
        public TrainPassInfo[] Data { get; set; }
    }

    public class TrainStation
    {
        public string arrive_day_str { get; set; }
        public string station_name { get; set; }
        public string start_time { get; set; }
        public string arrive_time { get; set; }
        public string running_time { get; set; }
        public string station_train_code { get; set; }
        public string arrive_day_diff { get; set; }
    }
}
