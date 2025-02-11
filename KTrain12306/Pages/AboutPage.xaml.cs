using System;
using System.Collections.Generic;
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

namespace KTrain12306.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        public AboutPage()
        {
            this.InitializeComponent();
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
                    Frame.Navigate(typeof(SaleTimeQueryPage));
                    break;
                case "about":
                    
                    break;
            }
        }
    }
}
