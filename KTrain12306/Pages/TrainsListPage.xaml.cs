using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace KTrain12306
{
    public class ListIndexExistsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is List<SeatData> list && int.TryParse(parameter.ToString(), out int index))
            {
                return index >= 0 && index < list.Count ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class RemarkToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // 1. 类型验证
            if (value is String)
            {
                String remark = (String)value;
                // 2. 字符串比较（忽略大小写）
                bool isMatch = string.Equals(remark, "预订", StringComparison.OrdinalIgnoreCase);

                // 3. 返回可见性结果
                return isMatch ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException(); // 单向绑定无需反向转换
        }
    }
    public class ListNameItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var items = value as List<SeatData>;
            if (items == null) return string.Empty;

            if (int.TryParse(parameter.ToString(), out int index) && index >= 0 && index < items.Count)
            {
                return items[index].name;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ListTicketsItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var items = value as List<SeatData>;
            if (items == null) return string.Empty;

            if (int.TryParse(parameter.ToString(), out int index) && index >= 0 && index < items.Count)
            {
                return items[index].num;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class TrainsListPage : Page
    {
        private static ObservableCollection<TrainInfo> data = new ObservableCollection<TrainInfo>();

        StationInfo from_station;
        StationInfo to_station;

        DateTime date;

        public TrainsListPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            train_list.ItemsSource = data;
            MainPage.setDatePickerRange(calendar);
            from_station = new StationInfo("bji|北京|BJP|beijing|bj|2|0357|北京|||");
            to_station = new StationInfo("sha|上海|SHH|shanghai|sh|13|0712|上海|||");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.NavigationMode == NavigationMode.New && e.Parameter is TrainsListData)
            {
                TrainsListData list_data = (TrainsListData)e.Parameter;
                
                from_station = list_data.from_station;
                to_station = list_data.to_station;

                date = list_data.date;
                refresh(list_data);
            }
            else
            {
                Debug.WriteLine("No Trains");
            }
                
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            var list_data = await TrainsListData.init(from_station, to_station, calendar.Date.Value.DateTime);
            date = calendar.Date.Value.DateTime;
            refresh(list_data);
        }

        private async void refresh(TrainsListData list_data)
        {
            // 在主线程启动加载圈
            LoadingRing.IsActive = true;
            LoadingRing.Visibility = Visibility.Visible;

            await Task.Run(() =>
            {
                List<TrainInfo> list = list_data.trains_list;

                // 使用 Dispatcher 更新 UI
                Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    title.Text = list_data.from_station.station_name + "-" + list_data.to_station.station_name;
                    calendar.Date = date;
                    data.Clear();
                }).AsTask().Wait();

                

                if (list.Count != 0)
                {
                    foreach (var info in list)
                    {
                        // 使用 Dispatcher 来确保修改 UI 控件的操作在主线程执行
                        Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            data.Add(info);
                        }).AsTask().Wait();
                    }
                }
                else
                {
                    // 同样使用 Dispatcher 来更新 UI
                    Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        Debug.WriteLine("No Trains");
                    }).AsTask().Wait();
                }

                // 在后台线程处理完毕后关闭加载圈
                Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    LoadingRing.IsActive = false;
                    LoadingRing.Visibility = Visibility.Collapsed;
                }).AsTask().Wait();
            });
        }



        private void Train_list_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(TrainInfoPage), e.ClickedItem);
        }
    }
}
