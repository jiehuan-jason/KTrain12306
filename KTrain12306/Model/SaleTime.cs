using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTrain12306
{
    public class Root
    {
        [JsonProperty("data")]
        public List<SaleTime> Data { get; set; }
    }
    public class SaleTime
    {
        public object name { get; set; }  //Station Name
        public object value { get; set; }//Time

        public SaleTime(object name, object value)
        {
            this.name = name;
            this.value = value;
        }
    }
    public class SaleTimeUtils
    {
        async static public Task<List<SaleTime>> getSaleTimes(String name)
        {
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

            var headers = httpClient.DefaultRequestHeaders;

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

            Uri requestUri = new Uri("https://hzfw.12306.cn/zgzfw/wxcore/queryQssj?stationName=" + name);

            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                return getSaleTimeFromJson(httpResponseBody);
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            
        }

        private static List<SaleTime> getSaleTimeFromJson(String json)
        {
            var root = JsonConvert.DeserializeObject<Root>(json);
            List<SaleTime> locations = new List<SaleTime>();
            foreach (var item in root.Data)
            {
                locations.Add(new SaleTime(item.name, item.value));
            }
            return locations;
        }
    }
}
