using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace KTrain12306
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static StationInfo from_station;
        public static StationInfo to_station;
        public static StationInfo[] stations;
        public MainPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;

            calendar = setDatePickerRange(calendar);
            calendar.Date = DateTime.Now;
            from_station = new StationInfo("bji|北京|BJP|beijing|bj|2|0357|北京|||");
            to_station = new StationInfo("sha|上海|SHH|shanghai|sh|13|0712|上海|||");
            from.Content = from_station.station_name;
            to.Content = to_station.station_name;
            /*LoadingRing.IsActive = true;
            LoadingRing.Visibility = Visibility.Visible;
            button.Visibility = Visibility.Collapsed;
            webview.NavigationCompleted += WebView_NavigationCompleted;
            webview.Visibility = Visibility.Collapsed;
            //TrainInfo train = JsonConvert.DeserializeObject<TrainInfo>("{\"train_no\":\"85000K104211\",\"station_train_code\":\"K1039\",\"start_station_telecode\":\"LZJ\",\"start_station_name\":\"兰州\",\"end_station_telecode\":\"NGH\",\"end_station_name\":\"宁波\",\"from_station_telecode\":\"HZH\",\"from_station_name\":\"杭州\",\"to_station_telecode\":\"NGH\",\"to_station_name\":\"宁波\",\"start_time\":\"04:02\",\"arrive_time\":\"06:50\",\"day_difference\":\"0\",\"train_class_name\":\"\",\"lishi\":\"02:48\",\"is_support_card\":\"0\",\"control_train_day\":\"20300303\",\"start_train_date\":\"20241204\",\"seat_feature\":\"133343W3\",\"yp_ex\":\"103040W0\",\"train_seat_feature\":\"3\",\"controlled_train_flag\":\"0\",\"controlled_train_message\":\"正常车次，不受控\",\"country_flag\":\"CHN,CHN\",\"rw_num\":\"有\",\"srrb_num\":\"-1\",\"gg_num\":\"-1\",\"gr_num\":\"-1\",\"rz_num\":\"-1\",\"tz_num\":\"-1\",\"wz_num\":\"有\",\"yb_num\":\"-1\",\"yw_num\":\"有\",\"yz_num\":\"有\",\"ze_num\":\"-1\",\"zy_num\":\"-1\",\"swz_num\":\"-1\",\"wz_seat_type_code\":\"1\",\"yz_price\":\"00285\",\"rz_price\":\"--\",\"yw_price\":\"00745\",\"rw_price\":\"01125\",\"gr_price\":\"--\",\"zy_price\":\"--\",\"ze_price\":\"--\",\"tz_price\":\"--\",\"gg_price\":\"--\",\"yb_price\":\"--\",\"bxyw_num\":\"-1\",\"bxyw_price\":\"--\",\"swz_price\":\"--\",\"hbyz_num\":\"-1\",\"hbyz_price\":\"--\",\"hbyw_num\":\"-1\",\"hbyw_price\":\"--\",\"bxrz_num\":\"-1\",\"bxrz_price\":\"--\",\"tdrz_num\":\"-1\",\"tdrz_price\":\"--\",\"srrb_price\":\"--\",\"errb_num\":\"-1\",\"errb_price\":\"--\",\"yrrb_num\":\"-1\",\"yrrb_price\":\"--\",\"ydsr_num\":\"-1\",\"ydsr_price\":\"--\",\"edsr_num\":\"-1\",\"edsr_price\":\"--\",\"hbrz_num\":\"-1\",\"hbrz_price\":\"--\",\"hbrw_num\":\"-1\",\"hbrw_price\":\"--\",\"ydrz_num\":\"-1\",\"ydrz_price\":\"--\",\"edrz_num\":\"-1\",\"edrz_price\":\"--\",\"wz_price\":\"00285\"}");
            //isContent = false;
            webview.Source = new Uri("https://kyfw.12306.cn/otn/leftTicket/init?linktypeid=dc&fs=%E6%9D%AD%E5%B7%9E%E4%B8%9C,HGH&ts=%E5%AE%81%E6%B3%A2,NGH&date=2024-12-01&flag=N,N,Y");
           */

        }

        private async void getStationsInfoFromWeb()
        {
            Boolean isExist = await StationUtils.isLocalStationExist();
            if (!isExist)
                StationUtils.getStationsDataFromWeb();
        }

        private async void getStationsArray()
        {
            Boolean isExist = await StationUtils.isLocalStationExist();
            if (!isExist)
                StationUtils.getStationsDataFromWeb();
            stations = await StationUtils.getStationInfoArray();
        }

        private async void OnRequestButtonClick(object sender, RoutedEventArgs e)
        {
            //LoadingRing.IsActive = true;
            //LoadingRing.Visibility = Visibility.Visible;
            /*// 获取明天的日期
            string tomorrowDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            // 第一步：请求 www.12306.cn 获取基础的 Cookies
            string url12306 = "https://www.12306.cn";
            var client = new HttpClient();
            HttpResponseMessage response12306 = await client.GetAsync(new Uri(url12306));

            // 获取并存储 www.12306.cn 的 Cookies
            var cookies12306 = GetCookies(response12306);

            // 第二步：请求第一个 URL，并带上从 www.12306.cn 获取的 Cookies
            string url1 = $"https://kyfw.12306.cn/otn/leftTicket/init?linktypeid=dc&fs=%E6%9D%AD%E5%B7%9E%E4%B8%9C,HGH&ts=%E9%BB%84%E5%B1%B1%E5%8C%97,NYH&date={tomorrowDate}&flag=N,N,Y";
            var client1 = new HttpClient();
            AddCookiesToRequest(client1, cookies12306); // 将 www.12306.cn 的 Cookies 放入请求头
            HttpResponseMessage response1 = await client1.GetAsync(new Uri(url1));

            // 获取并存储第一个请求返回的 Cookies
            var cookies1 = GetCookies(response1);

            // 第三步：将 www.12306.cn 和 第一个请求的 Cookies 一起放入第二个请求，同时伪装成 Chrome 浏览器
            string url2 = $"https://kyfw.12306.cn/otn/leftTicket/queryO?leftTicketDTO.train_date={tomorrowDate}&leftTicketDTO.from_station=HGH&leftTicketDTO.to_station=NGH&purpose_codes=ADULT";
            var client2 = new HttpClient();
            var allCookies = cookies12306.Concat(cookies1).ToList(); // 合并两个请求的 Cookies
            AddCookiesToRequest(client2, allCookies); // 将所有 Cookies 放入第二个请求头

            // 伪装成最新版本的 Chrome 浏览器
            AddUserAgent(client2);

            // 发送第二个请求
            HttpResponseMessage response2 = await client2.GetAsync(new Uri(url2));

            // 获取第二个请求的响应内容
            string result2 = await response2.Content.ReadAsStringAsync();*/

            /*string htmlSource = await webview.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML" });
            var htmlDoc = new XmlDocument();

            try
            {
                // 将 HTML 字符串加载到 XmlDocument 中
                htmlDoc.LoadXml(htmlSource);
            }
            catch (Exception ex)
            {
                Debug.WriteLine( $"Error loading HTML: {ex.Message}");
            }

            // 查找 <pub> 标签
            var pubElements = htmlDoc.GetElementsByTagName("pre");
            // 显示返回结果
            responseTextBlock.Text = $"请求结果 2:\n{pubElements[0].InnerText}";
            LoadingRing.IsActive = false;
            LoadingRing.Visibility = Visibility.Collapsed;*/
        }

        private void NavLinksList_ItemClick(object sender, ItemClickEventArgs e)
        {
            StackPanel click = (StackPanel) e.ClickedItem;
            switch (click.Name)
            {
                case "menu":
                    splitView.IsPaneOpen = !splitView.IsPaneOpen;
                    break;
                case "home":
                    break;
                case "time":
                    //TODO
                    break;
            }
                
                    
            Debug.WriteLine(click.Name );
        }

        private void Menu_Button_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void From_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StationChoosePage),StationQueryInfo.query_label.From);
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {

        }

        private void To_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StationChoosePage), StationQueryInfo.query_label.To);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter.GetType().Equals(typeof(StationQueryInfo))){
                StationQueryInfo info = (StationQueryInfo) e.Parameter;
                if (info.status.Equals(StationQueryInfo.query_label.From)) {
                    from_station = info.station_info;
                    from.Content = from_station.station_name;
                }
                else
                {
                    to_station = info.station_info;
                    to.Content = to_station.station_name;
                }
            }
        }
        /*private void WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
{
if (args.IsSuccess)
{
// 页面加载成功，执行下一步操作
// 在此处添加加载完成后的操作逻辑
System.Diagnostics.Debug.WriteLine("页面加载完成！");
if (webview.Source.AbsoluteUri.StartsWith("https://kyfw.12306.cn/otn/leftTicket/init"))
{
webview.Source = new Uri("https://kyfw.12306.cn/otn/leftTicket/queryO?leftTicketDTO.train_date=2024-12-02&leftTicketDTO.from_station=HGH&leftTicketDTO.to_station=NGH&purpose_codes=ADULT");
button.Visibility = Visibility.Visible;
LoadingRing.IsActive = false;
LoadingRing.Visibility = Visibility.Collapsed;
}
ExecuteNextStep();
}
else
{
// 页面加载失败，进行错误处理
System.Diagnostics.Debug.WriteLine("页面加载失败！");
}
}



private async void ExecuteNextStep()
{
// 在这里执行你的后续操作，例如解析页面内容、显示按钮等
// 例如显示一个提示框，表示操作完成
content = await webview.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML" });
isContent = true;
}*/
        public static CalendarDatePicker setDatePickerRange(CalendarDatePicker calendarDatePicker)
        {
            calendarDatePicker.MinDate = DateTime.Now;
            calendarDatePicker.MaxDate = DateTime.Now.AddDays(15 - 1);
            return calendarDatePicker;
        }
    }
    
}
