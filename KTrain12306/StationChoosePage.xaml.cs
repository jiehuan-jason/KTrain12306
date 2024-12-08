using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class StationChoosePage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<StationInfo> _stationInfos = new ObservableCollection<StationInfo>();
        private StationQueryInfo.query_label label;

        public ObservableCollection<StationInfo> StationInfos
        {
            get => _stationInfos;
            set
            {
                if (_stationInfos != value)
                {
                    _stationInfos = value;
                    OnPropertyChanged(nameof(StationInfos));
                }
            }
        }
        public StationChoosePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter.GetType().Equals(typeof(StationQueryInfo.query_label)))
            {
                label = (StationQueryInfo.query_label)e.Parameter;
            }
        }

        private void Station_list_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), new StationQueryInfo((StationInfo)e.ClickedItem, label));
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            LoadingRing.IsActive = true;
            LoadingRing.Visibility = Visibility.Visible;
            
            StationInfos.Clear();
            var all_station_list = await StationUtils.getStationInfoArray();
            string input = search_input.Text.Trim();

            // 判断输入是中文还是英文
            bool isChinese = Regex.IsMatch(input, @"[\u4e00-\u9fa5]");

            // 根据输入类型选择字段匹配
            var search_list = from station_obj in all_station_list
                              where isChinese
                                  ? station_obj.station_name.Contains(input)
                                  : station_obj.station_pinyin.Contains(input.ToLower())
                              select station_obj;
            StationInfos = new ObservableCollection<StationInfo>();
            foreach(var station_info in search_list)
            {
                StationInfos.Add(station_info);
            }
            LoadingRing.IsActive = false;
            LoadingRing.Visibility = Visibility.Collapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
