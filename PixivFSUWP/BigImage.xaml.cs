using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PixivFSUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BigImage : Page
    {
        Data.BigImageDetail parameter;
        public BigImage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            parameter = e.Parameter as Data.BigImageDetail;
            //CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonForegroundColor = Colors.White;
            view.TitleBar.ButtonForegroundColor = Colors.Black;
            view.TitleBar.ButtonInactiveForegroundColor = Colors.Gray;
            view.Title = string.Format("{0} by {1} - {2}x{3}",
                parameter.Title, parameter.Author,
                parameter.Width, parameter.Height);
            mainImg.Source = await Data.OverAll.BytesToImage(parameter.Image, parameter.Width, parameter.Height);
            base.OnNavigatedTo(e);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mainImg.MaxHeight = Frame.ActualHeight;
            mainImg.MaxWidth = Frame.ActualWidth;
        }
    }
}
