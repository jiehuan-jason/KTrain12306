﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTrain12306
{     
    class SeatData
    {
        public float price { get; set; }
        public String name { get; set; }
        public String num { get; set; }

        public static List<SeatData> GetSeatDatas(TrainInfo listData)
        {
            var list = new List<SeatData>();
            if (!listData.rw_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.rw_price);
                data.name = "软卧";
                data.num = listData.rw_num;
                list.Add(data);
            }
             if (!listData.rz_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.rz_price);
                data.name = "软座";
                data.num = listData.rz_num;
                list.Add(data);
            }
             if (!listData.yw_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.yw_price);
                data.name = "硬卧";
                data.num = listData.yw_num;
                list.Add(data);
            }
             if (!listData.yz_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.yz_price);
                data.name = "硬座";
                data.num = listData.yz_num;
                list.Add(data);
            }
             if (!listData.ze_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.ze_price);
                data.name = "二等座";
                data.num = listData.ze_num;
                list.Add(data);
            }
             if (!listData.zy_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.zy_price);
                data.name = "一等座";
                data.num = listData.zy_num;
                list.Add(data);
            }
             if (!listData.swz_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.swz_price);
                data.name = "商务座";
                data.num = listData.swz_num;
                list.Add(data);
            }
             if (!listData.srrb_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.srrb_price);
                data.name = "动卧";
                data.num = listData.srrb_num;
                list.Add(data);
            }
             if (!listData.gg_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.gg_price);
                data.name = "gg";
                data.num = listData.gg_num;
                list.Add(data);
            }
             if (!listData.gr_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.gr_price);
                data.name = "高级软卧";
                data.num = listData.gr_num;
                list.Add(data);
            }
             if (!listData.tz_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.tz_price);
                data.name = "特等座";
                data.num = listData.tz_num;
                list.Add(data);
            }
             if (!listData.yb_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.yb_price);
                data.name = "硬包";
                data.num = listData.yb_num;
                list.Add(data);
            }
             if (!listData.wz_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.wz_price);
                data.name = "无座";
                data.num = listData.wz_num;
                list.Add(data);
            }
             if (!listData.bxyw_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.bxyw_price);
                data.name = "包厢硬卧";
                data.num = listData.bxyw_num;
                list.Add(data);
            }
             if (!listData.hbyz_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.hbyz_price);
                data.name = "混编硬座";
                data.num = listData.hbyz_num;
                list.Add(data);
            }
             if (!listData.hbyw_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.hbyw_price);
                data.name = "混编硬卧";
                data.num = listData.hbyw_num;
                list.Add(data);
            }
             if (!listData.bxrz_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.bxrz_price);
                data.name = "优选一等";
                data.num = listData.bxrz_num;
                list.Add(data);
            }
             if (!listData.tdrz_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.tdrz_price);
                data.name = "特等软座";
                data.num = listData.tdrz_num;
                list.Add(data);
            }
             if (!listData.errb_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.errb_price);
                data.name = "二人软包";
                data.num = listData.errb_num;
                list.Add(data);
            }
             if (!listData.yrrb_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.yrrb_price);
                data.name = "一人软包";
                data.num = listData.yrrb_num;
                list.Add(data);
            }
             if (!listData.ydsr_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.ydsr_price);
                data.name = "一等软座";
                data.num = listData.ydsr_num;
                list.Add(data);
            }
             if (!listData.edsr_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.edsr_price);
                data.name = "二等软座";
                data.num = listData.edsr_num;
                list.Add(data);
            }
             if (!listData.hbrz_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.hbrz_price);
                data.name = "混编软座";
                data.num = listData.hbrz_num;
                list.Add(data);
            }
             if (!listData.hbrw_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.hbrw_price);
                data.name = "混编软卧";
                data.num = listData.hbrw_num;
                list.Add(data);
            }
             if (!listData.ydrz_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.ydrz_price);
                data.name = "一等软卧";
                data.num = listData.ydrz_num;
                list.Add(data);
            }
             if (!listData.edrz_num.Equals("-1"))
            {
                SeatData data = new SeatData();
                data.price = SeatData.ConvertToFloat(listData.edrz_price);
                data.name = "二等软卧";
                data.num = listData.edrz_num;
                list.Add(data);
            }
            return list;

        }
        public static float ConvertToFloat(string input)
            {
                if (string.IsNullOrEmpty(input) || input.Length < 2)
                {
                    throw new ArgumentException("输入字符串格式无效，长度必须大于等于2。");
                }

                // 去掉前导零
                input = input.TrimStart('0');

                // 如果字符串为空，表示值为0
                if (string.IsNullOrEmpty(input))
                {
                    return 0.0f;
                }

                // 将最后一位作为小数部分，其余部分作为整数部分
                string integerPart = input.Substring(0, input.Length - 1); // 整数部分
                string fractionalPart = input.Substring(input.Length - 1); // 小数部分

                // 组合为浮点数格式的字符串
                string fullNumber = integerPart + "." + fractionalPart;

                // 转为浮点数
                return float.Parse(fullNumber);
            }
        }
}