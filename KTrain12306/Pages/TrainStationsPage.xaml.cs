using KTrain12306.Model;
using KTrain12306.Util;
using System;
using System.Collections.Generic;
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
    public sealed partial class TrainStationsPage : Page
    {
        TrainPassInfo info;
        public TrainStationsPage()
        {
            this.InitializeComponent();
        }
        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is TrainPassInfo)
            {
                info = (TrainPassInfo)e.Parameter;
                List<TrainStation> stations = new List<TrainStation>();
                LoadingRing.IsActive = true;
                await Task.Run(async () =>
                {
                    DateTime date = DateTime.ParseExact(info.Date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    stations = await TrainPassInfoUtil.BackTrainPassInfo(await TrainPassInfoUtil.GetTrainQueryNo(info.station_train_code, date), date);
                });

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    train_code.Content = info.station_train_code;

                    from_station_name.Text = info.from_station;
                    to_station_name.Text = info.to_station;
                    time.Text = stations.Last().running_time;
                    start_time.Text = stations.First().start_time;
                    arrive_time.Text = stations.Last().arrive_time;
                    if (!stations.Last().arrive_day_diff.Equals("0"))
                        add_day_display.Text = "+" + stations.Last().arrive_day_diff;
                    LoadingRing.IsActive = false;
                    StationsList.ItemsSource = stations;
                });
                

                
                
            }

        }
    }
}
