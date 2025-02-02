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

namespace KTrain12306.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    
    public sealed partial class SaleTimeQueryPage : Page
    {
        private StationInfo stationInfo;
        private static ObservableCollection<SaleTime> time_data = new ObservableCollection<SaleTime>();

        public SaleTimeQueryPage()
        {
            this.InitializeComponent();
            calendar = MainPage.setDatePickerRange(calendar);
            calendar.Date = DateTime.Today;
            stationInfo = new StationInfo("bji|北京|BJP|beijing|bj|2|0357|北京|||");
            station.Content = stationInfo.station_name;
        }

        private void Station_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StationChoosePage), StationQueryInfo.query_label.None);
        }

        private void NavLinksList_ItemClick(object sender, ItemClickEventArgs e)
        {
            StackPanel click = (StackPanel)e.ClickedItem;
            switch (click.Name)
            {
                case "menu":
                    splitView.IsPaneOpen = !splitView.IsPaneOpen;
                    break;
                case "home":
                    Frame.Navigate(typeof(MainPage));
                    break;
                case "time":
                    break;
            }


            Debug.WriteLine(click.Name);
        }

        async private void Search_Click(object sender, RoutedEventArgs e)
        {
            LoadingRing.IsActive = true;
            TimeList.ItemsSource = null;

            await Task.Run(async () =>
            {
                
                var sale_time_list = await SaleTimeUtils.getSaleTimes(stationInfo.station_name);
                time_data = new ObservableCollection<SaleTime>(sale_time_list);
                
            });

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {

                // 隐藏加载圈
                LoadingRing.IsActive = false;
                TimeList.ItemsSource = time_data;
            });

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                if (e.Parameter.GetType().Equals(typeof(StationQueryInfo)))
                {
                    StationQueryInfo info = (StationQueryInfo)e.Parameter;
                    stationInfo = info.station_info;
                    station.Content = stationInfo.station_name;
                }
            }
        }
    }
}
