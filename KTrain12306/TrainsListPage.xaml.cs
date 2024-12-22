using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        public TrainsListPage()
        {
            this.InitializeComponent();
            train_list.ItemsSource = data;
            MainPage.setDatePickerRange(calendar);
            from_station = new StationInfo("bji|北京|BJP|beijing|bj|2|0357|北京|||");
            to_station = new StationInfo("sha|上海|SHH|shanghai|sh|13|0712|上海|||");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is TrainsListData)
            {
                TrainsListData list_data = (TrainsListData)e.Parameter;
                
                from_station = list_data.from_station;
                to_station = list_data.to_station;
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
            refresh(list_data);
        }

        private void refresh(TrainsListData list_data)
        {
            LoadingRing.IsActive = true;
            LoadingRing.Visibility = Visibility.Visible;
            List<TrainInfo> list = list_data.trains_list;
            title.Text = list_data.from_station.station_name + "-" + list_data.to_station.station_name;
            calendar.Date = list_data.date;
            data.Clear();
            if (list.Count != 0)
            {

                foreach (var info in list)
                {
                    data.Add(info);
                }
            }
            else
            {
                Debug.WriteLine("No Trains");
            }
            LoadingRing.IsActive = false;
            LoadingRing.Visibility = Visibility.Collapsed;
        }
    }
}
