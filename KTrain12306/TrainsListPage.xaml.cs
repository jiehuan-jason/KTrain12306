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
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class TrainsListPage : Page
    {
        private static ObservableCollection<TrainInfo> data = new ObservableCollection<TrainInfo>();

        public TrainsListPage()
        {
            this.InitializeComponent();
            train_list.ItemsSource = data;
            MainPage.setDatePickerRange(calendar);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is TrainsListData)
            {
                TrainsListData list_data = (TrainsListData)e.Parameter;
                List<TrainInfo> list = list_data.trains_list;
                title.Text = list_data.from_station.station_name + "-" + list_data.to_station.station_name;
                calendar.Date = list_data.date;
                data.Clear();
                if (list.Count != 0)
                {
                    
                    foreach(var info in list)
                    {
                        data.Add(info);
                    }
                }
                else
                {
                    Debug.WriteLine("No Trains");
                }
            }
            else
            {
                Debug.WriteLine("No Trains");
            }
                
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
