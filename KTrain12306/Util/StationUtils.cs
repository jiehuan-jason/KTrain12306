using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace KTrain12306
{
    class StationUtils
    {
        public StationUtils()
        {
        }
        public static async void getStationsDataFromWeb()
        {
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            Uri requestUri = new Uri("https://kyfw.12306.cn/otn/resources/js/framework/station_name.js");

            //Send the GET request asynchronously and retrieve the response as a string.
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync("stations.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, httpResponseBody);
        }
        public static async Task<String> getStationsDataLocal()
        {
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.GetFileAsync("stations.txt");
            String data = await FileIO.ReadTextAsync(file);
            return data;
        }
        public static async Task<Boolean> isLocalStationExist()
        {
            string fileName = "stations.txt";
            //如果是File，则括号内的转换类型应该为(StorageFile)，并返回为 StorageFile
            StorageFile retFile = (StorageFile)await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName);
            if (retFile == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static async Task<StationInfo[]> getStationInfoArray(){
            String content = await getStationsDataLocal();
            String[] station_raw = content.Split('@');
            StationInfo[] stations_info = new StationInfo[station_raw.Length-1];
            for(int i = 1; i < station_raw.Length; i++)
            {
                stations_info[i-1] = new StationInfo(station_raw[i]);
            }
            return stations_info;
        }
    }
}
