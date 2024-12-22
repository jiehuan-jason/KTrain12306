using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KTrain12306
{
    class TrainsListData
    {
        public StationInfo from_station { get; set; }
        public StationInfo to_station { get; set; }
        public DateTime date { get; set; }
        public List<TrainInfo> trains_list { get; set; }

        public TrainsListData(StationInfo from, StationInfo to, DateTime date)
        {
            from_station = from;
            to_station = to;
            this.date = date;
        }

        static public async Task<TrainsListData> init(StationInfo from, StationInfo to, DateTime date)
        {
            var data = new TrainsListData(from, to, date);
            data.trains_list = await data.getListFromWeb();
            return data;
        }

        public async Task<List<TrainInfo>> getListFromWeb()
        {
            String content;
            try
            {
                content = await getDataFromWebAsync();

                var jsonObj = JObject.Parse(content);
                var dataArray = jsonObj["data"] as JArray;

                var result = new List<TrainInfo>();

                String content_new = await TrainsListData.GetFromNewAPI();
                
                var newjsonObj = JObject.Parse(content_new);
                var newdataArray = (JArray)newjsonObj["data"]["result"];
                
                List<string> newResult = newdataArray.ToObject<List<string>>();

                // 遍历 data 数组
                foreach (var item in dataArray)
                {
                    var queryLeftNewDTO = item["queryLeftNewDTO"];
                    if (queryLeftNewDTO != null)
                    {
                        var info = TrainInfo.GetTrainInfo(queryLeftNewDTO.ToString());
                        if(date.Date == DateTime.Today)
                        {
                            if(TimeSpan.Parse(info.start_time) > DateTime.Now.TimeOfDay)
                            {
                                result.Add(info);
                            }
                        }else
                            result.Add(info);
                            

                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
                //content = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
        }
        
        private async Task<string> getDataFromWebAsync()
        {
            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

            //Add a user-agent header to the GET request. 
            var headers = httpClient.DefaultRequestHeaders;

            //The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
            //especially if the header value is coming from user input.
            string header = "ie";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }

            header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }

            Uri requestUri = new Uri("https://kyfw.12306.cn/otn/leftTicketPrice/query?leftTicketDTO.train_date=" + date.ToString("yyyy-MM-dd") + "&leftTicketDTO.from_station=" + from_station.station_telecode + "&leftTicketDTO.to_station=" + to_station.station_telecode + "&leftTicketDTO.ticket_type=1&randCode=");

            //Send the GET request asynchronously and retrieve the response as a string.
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            //Send the GET request
            httpResponse = await httpClient.GetAsync(requestUri);
            httpResponse.EnsureSuccessStatusCode();
            httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            return httpResponseBody;
        }

        public static async Task<String> GetFromNewAPI()
        {
            string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
            string initUrl = $"https://kyfw.12306.cn/otn/leftTicket/init?linktypeid=dc&fs=%E6%9D%AD%E5%B7%9E%E4%B8%9C,HGH&ts=%E5%AE%81%E6%B3%A2,NGH&date={todayDate}&flag=N,N,Y";

            // 创建一个 HttpClientHandler 用于管理 cookies
            using (var handler = new HttpClientHandler() { CookieContainer = new System.Net.CookieContainer() })
            using (var client = new HttpClient(handler))
            {
                // 设置模拟浏览器的请求头
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                client.DefaultRequestHeaders.Referrer = new Uri("https://kyfw.12306.cn/otn/leftTicket/init");

                // 发送请求并获取初始化页面的响应
                var initResponse = await client.GetAsync(initUrl);

                if (initResponse.IsSuccessStatusCode)
                {
                    // 如果初始化页面请求成功，可以继续获取第二个 API
                    string apiUrl = "https://kyfw.12306.cn/otn/leftTicket/queryO?leftTicketDTO.train_date=2024-12-23&leftTicketDTO.from_station=BJP&leftTicketDTO.to_station=SHH&purpose_codes=ADULT"; // 替换为实际的 API 地址
                    var apiResponse = await client.GetAsync(apiUrl);

                    if (apiResponse.IsSuccessStatusCode)
                    {
                        string apiContent = await apiResponse.Content.ReadAsStringAsync();
                        Console.WriteLine("API Response: " + apiContent);
                        return apiContent;
                    }
                    else
                    {
                        Console.WriteLine("第二个 API 请求失败，状态码：" + apiResponse.StatusCode);
                        return "error";
                    }
                }
                else
                {
                    Console.WriteLine("初始化页面请求失败，状态码：" + initResponse.StatusCode);
                    return "error";
                }
            }
        }
    }
}
