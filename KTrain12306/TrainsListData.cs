using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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

                // 遍历 data 数组
                foreach (var item in dataArray)
                {
                    var queryLeftNewDTO = item["queryLeftNewDTO"];
                    if (queryLeftNewDTO != null)
                    {
                        result.Add(TrainInfo.GetTrainInfo(queryLeftNewDTO.ToString()));
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
    }
}
