using KTrain12306.Model;
using KTrain12306.Pages;
using KTrain12306.Util;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class TrainInfoPage : Page
    {
        TrainInfo info;
        public TrainInfoPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is TrainInfo)
            {
                info = (TrainInfo)e.Parameter;

                train_code.Content = info.StationTrainCode;

                from_station_name.Text = info.from_station_name;
                to_station_name.Text = info.to_station_name;
                time.Text = info.lishi;
                start_time.Text = info.StartTime;
                arrive_time.Text = info.ArriveTime;
                add_day_display.Text = info.add_day_display;
                PriceList.ItemsSource = info.SeatDatas;
            }

        }

        async private void Train_code_Click(object sender, RoutedEventArgs e)
        {
            var trains = await TrainPassInfoUtil.SearchAndBackTrainPassInfos(info.StationTrainCode,info.date);
            var train = trains.FirstOrDefault(item => item.station_train_code == info.StationTrainCode);
            Frame.Navigate(typeof(TrainStationsPage), train);

        }
    }
}
