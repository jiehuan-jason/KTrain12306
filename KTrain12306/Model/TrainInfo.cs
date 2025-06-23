using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTrain12306
{
    public class TrainInfo
    {
        public DateTime date { get; set; }
        public String StationTrainCode { get; set; }
        public String StartStationTelecode { get; set; }
        public String EndStationTelecode { get; set; }
        public String FromStationTelecode { get; set; }
        public String ToStationTelecode { get; set; }
        public String start_station_name { get; set; }
        public String end_station_name { get; set; }
        public String from_station_name { get; set; }
        public String to_station_name { get; set; }
        public String StartTime { get; set; }
        public String ArriveTime { get; set; }
        public short DayDifference { get; set; }
        public String lishi { get; set; }
        public String tips { get; set; }
        public string rw_num { get; set; } = "-1";
        public string srrb_num { get; set; } = "-1";
        public string gg_num { get; set; } = "-1";
        public string gr_num { get; set; } = "-1";
        public string rz_num { get; set; } = "-1";
        public string tz_num { get; set; } = "-1";
        public string wz_num { get; set; } = "-1";
        public string yb_num { get; set; } = "-1";
        public string yw_num { get; set; } = "-1";
        public string yz_num { get; set; } = "-1";
        public string ze_num { get; set; } = "-1";
        public string zy_num { get; set; } = "-1";
        public string swz_num { get; set; } = "-1";
        public string dw_num { get; set; } = "-1";
        public string yxyd_num { get; set; } = "-1";
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
        public string yxyd_price { get; set; }
        public string swz_price { get; set; }
        public string dw_price { get; set; }
        public string ydrz_num { get; set; } = "-1";
        public string ydrz_price { get; set; }
        public string edrz_num { get; set; } = "-1";
        public string edrz_price { get; set; }
        public string ydrw_num { get; set; } = "-1";
        public string ydrw_price { get; set; }
        public string edrw_num { get; set; } = "-1";
        public string edrw_price { get; set; }
        public string wz_price { get; set; }
        public string add_day_display { get; set; }
        public string remark { get; set; } = "";
        public bool isBeginSale { get; set; }

        public List<SeatData> SeatDatas { set; get; }

        //勿忘调用initStationsName！
        public TrainInfo(String trainAllData, DateTime date)
        {
            this.date = date;
            var split = trainAllData.Split('|');
            var ypInfoNewString = split[39];
            tips = split[1];
            StationTrainCode = split[3];
            StartStationTelecode = split[4];
            EndStationTelecode = split[5];
            FromStationTelecode = split[6];
            ToStationTelecode = split[7];
            //initStationsName();
            StartTime = split[8];
            ArriveTime = split[9];
            lishi = split[10];
            if (split[1].Equals("预订"))
                isBeginSale = true;
            else isBeginSale = false;
            DayDifference = GetDayDifference();
            InitAddDayDisplay();
            this.ParseYpInfoNew(ypInfoNewString);
        }

        public async Task initStationsName()
        {
            var stations = await StationUtils.getStationInfoArray();
            var stationL = from station in stations
                        where station.station_telecode.Equals(StartStationTelecode)
                        select station;
            start_station_name = stationL.First().station_name;
            stationL = from station in stations
                        where station.station_telecode.Equals(EndStationTelecode)
                        select station;
            end_station_name = stationL.First().station_name;
            stationL = from station in stations
                       where station.station_telecode.Equals(FromStationTelecode)
                       select station;
            from_station_name = stationL.First().station_name;
            stationL = from station in stations
                       where station.station_telecode.Equals(ToStationTelecode)
                       select station;
            to_station_name = stationL.First().station_name;
        }

        private void ParseYpInfoNew(string ypInfoNewString)
        {
           

            // 检查输入字符串是否为空或长度不符合预期
            if (string.IsNullOrEmpty(ypInfoNewString) || ypInfoNewString.Length % 10 != 0)
            {
                Console.WriteLine("警告：yp_info_new 字符串格式不正确。");
                return;
            }

            // 计算有多少个10字符的片段
            int segmentCount = ypInfoNewString.Length / 10;

            // 遍历每个10字符的片段
            for (int i = 0; i < segmentCount; i++)
            {
                string segment = ypInfoNewString.Substring(i * 10, 10);

                // 提取座位类型代码（第一个字符）
                string seatTypeCode = segment.Substring(0, 1);

                // 提取价格的原始数字字符串（从第二个字符到第六个字符）
                string priceString = segment.Substring(1, 5);

                // 提取特殊标志位（第七个字符）
                string specialFlag = segment.Substring(6, 1);

                string ticketsCount = segment.Substring(7, 3);

                if (specialFlag == "3")
                {
                    // 如果标志位是 "3"，通常表示“无座”或某种特殊情况
                    // 在C#中，我们将其类型设置为 "W" (无座)
                    ChooseSeatTypeAndSave("W", priceString, ticketsCount);
                }
                else
                {
                    ChooseSeatTypeAndSave(seatTypeCode, priceString, ticketsCount);
                }
                


            }
            return;
        }

        private void ChooseSeatTypeAndSave(string type, string price, string count)
        {
            switch (type)
            {
                case "1": // 硬座
                    this.yz_price = price;
                    this.yz_num = count;
                    break;
                case "2": // 软座
                    this.rz_price = price;
                    this.rz_num = count;
                    break;
                case "3": // 硬卧
                    this.yw_price = price;
                    this.yw_num = count;
                    break;
                case "4": // 软卧
                    this.rw_price = price;
                    this.rw_num = count;
                    break;
                case "6": // 高级软卧
                    this.gr_price = price;
                    this.gr_num = count;
                    break;
                case "7": // 一等软座
                          // 假设 'ydrz_price' 和 'ydrz_num' 对应 '7'
                    this.ydrz_price = price;
                    this.ydrz_num = count;
                    break;
                case "8": // 二等软座
                          // 假设 'edrz_price' 和 'edrz_num' 对应 '8'
                    this.edrz_price = price;
                    this.edrz_num = count;
                    break;
                case "9": // 商务座
                    this.swz_price = price;
                    this.swz_num = count;
                    break;
                case "A": // 高级软卧（动卧） - 假设也归到高级软卧价格
                    this.gr_price = price; // 价格和数量可能与 '6' 共用
                    this.gr_num = count;
                    break;
                case "D": // 优选一等
                    this.yxyd_price = price;
                    this.yxyd_num = count;
                    break;
                case "F": // 动卧
                    this.dw_price = price;
                    this.dw_num = count;
                    break;
                case "I": // 一等卧
                    this.ydrw_price = price;
                    this.ydrw_num = count;
                    break;
                case "J": // 二等卧
                    this.edrw_price = price;
                    this.edrw_num = count;
                    break;
                case "M": // 一等座
                    this.zy_price = price;
                    this.zy_num = count;
                    break;
                case "O": // 二等座
                    this.ze_price = price;
                    this.ze_num = count;
                    break;
                case "P": // 特等座
                    this.tz_price = price;
                    this.tz_num = count;
                    break;
                case "W": // 无座 (这是 bI 函数中的特殊处理)
                    this.wz_price = price;
                    this.wz_num = count;
                    break;
                // 如果还有其他类型代码，可以在这里继续添加 case
                default:
                    // 如果遇到未知的类型代码，可以打印警告或记录日志
                    Console.WriteLine($"警告：遇到未知的座位类型代码 '{type}'。价格: {price}, 数量: {count}");
                    break;
            }
        }


        private short GetDayDifference()
        {
            var hours = short.Parse(StartTime.Substring(0,2))+ short.Parse(lishi.Substring(0, 2));
            var mins = short.Parse(StartTime.Substring(3, 2)) + short.Parse(lishi.Substring(3, 2));
            short DayDifference = 0;
            if (mins >= 60) hours++;
            if (hours >= 24)
                if (hours >= 48)
                    DayDifference = 2;
                else
                    DayDifference = 1;
            else DayDifference = 0;
            return DayDifference;
        }

        private void InitAddDayDisplay()
        {
            if (DayDifference == 0)
                add_day_display = "";
            else
                add_day_display = "+" + DayDifference;
        }
    }
}
