using KTrain12306.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KTrain12306.Util
{
    public class TrainPassInfoUtil
    {
        async static public Task<List<TrainPassInfo>> SearchAndBackTrainPassInfos(String keyword,DateTime date)
        {
            String url = "https://search.12306.cn/search/v1/train/search"+ "?keyword=" + keyword + "&date=" + date.ToString("yyyyMMdd");
            String content = await GetContentFromWeb(url);
            ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

            
            // 查找对应的列车信息
            return (new List<TrainPassInfo>(apiResponse.Data));
        }

        async static public Task<String> GetTrainQueryNo(String train_code, DateTime date)
        {
            List<TrainPassInfo> list_info = await SearchAndBackTrainPassInfos(train_code, date);
            var result = list_info.FirstOrDefault(item => item.station_train_code == train_code);

            if (result != null)
            {
                Console.WriteLine($"找到的列车：{result.station_train_code}, {result.from_station} 到 {result.to_station}, 日期: {result.Date}");
                return result.train_no;
            }
            else
            {
                Console.WriteLine("未找到符合条件的列车");
                return null;
            }
        }

        async static public Task<List<TrainStation>> BackTrainPassInfo(String train_query_no, DateTime date)
        {
            string content = await TrainsListData.getDataFromWebAsync("https://kyfw.12306.cn/otn/queryTrainInfo/query?leftTicketDTO.train_no=" + train_query_no + "&leftTicketDTO.train_date=" + date.ToString("yyyy-MM-dd") + "&rand_code=");
            dynamic json = JsonConvert.DeserializeObject(content);
            var stationData = json.data.data;
            List<TrainStation> trainStations = new List<TrainStation>();

            // 解析每个站点数据
            foreach (var station in stationData)
            {
                var trainStation = new TrainStation
                {
                    arrive_day_str = station.arrive_day_str,
                    arrive_day_diff = station.arrive_day_diff,
                    station_name = station.station_name,
                    start_time = station.start_time,
                    arrive_time = station.arrive_time,
                    running_time = station.running_time,
                    station_train_code = station.station_train_code
                };

                trainStations.Add(trainStation);
            }
            return trainStations;
        }

        async static private Task<string> GetContentFromWeb(String url)
        {
            using(HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return json;
            }
        }
    }
}
