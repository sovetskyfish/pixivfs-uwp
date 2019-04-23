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
        bool _locked = false;
        Data.BigImageDetail parameter;
        public BigImage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            parameter = e.Parameter as Data.BigImageDetail;
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonForegroundColor = Colors.White;
            view.TitleBar.ButtonForegroundColor = Colors.Black;
            view.TitleBar.ButtonInactiveForegroundColor = Colors.Gray;
            view.Title = string.Format("{0} by {1} - {2}x{3}",
                parameter.Title, parameter.Author,
                parameter.Width, parameter.Height);
            txtTitle.Text = view.Title;
            mainImg.Source = await Data.OverAll.BytesToImage(parameter.Image, parameter.Width, parameter.Height);
            base.OnNavigatedTo(e);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_locked) return;
            gridImg.MaxHeight = scrollRoot.ActualHeight;
            gridImg.MaxWidth = scrollRoot.ActualWidth;
            gridImg.MinHeight = scrollRoot.ActualHeight;
            gridImg.MinWidth = scrollRoot.ActualWidth;
        }

        void lockView()
        {
            _locked = true;
            gridImg.MaxHeight = gridImg.ActualHeight;
            gridImg.MaxWidth = gridImg.ActualWidth;
            gridImg.MinHeight = gridImg.ActualHeight;
            gridImg.MinWidth = gridImg.ActualWidth;
            mainImg.Opacity = 0.3;
            paper.MinHeight = mainImg.ActualHeight;
            paper.MinWidth = mainImg.ActualWidth;
            paper.MaxHeight = mainImg.ActualHeight;
            paper.MaxWidth = mainImg.ActualWidth;
            paper.Height = mainImg.ActualHeight;
            paper.Width = mainImg.ActualWidth;
            mainCanvas.MinHeight = mainImg.ActualHeight;
            mainCanvas.MinWidth = mainImg.ActualWidth;
            mainCanvas.MaxHeight = mainImg.ActualHeight;
            mainCanvas.MaxWidth = mainImg.ActualWidth;
            mainCanvas.Height = mainImg.ActualHeight;
            mainCanvas.Width = mainImg.ActualWidth;
        }

        void unlockView()
        {
            _locked = false;
            mainImg.Opacity = 1;
            gridImg.Height = scrollRoot.ActualHeight;
            gridImg.Width = scrollRoot.ActualWidth;
            gridImg.MaxHeight = scrollRoot.ActualHeight;
            gridImg.MaxWidth = scrollRoot.ActualWidth;
            gridImg.MinHeight = scrollRoot.ActualHeight;
            gridImg.MinWidth = scrollRoot.ActualWidth;
        }

        private void BtnDraw_Checked(object sender, RoutedEventArgs e)
        {
            mainCanvas.Visibility = Visibility.Visible;
            inkToolbar.Visibility = Visibility.Visible;
            paper.Visibility = Visibility.Visible;
            lockView();
        }

        private void BtnDraw_Unchecked(object sender, RoutedEventArgs e)
        {
            mainCanvas.Visibility = Visibility.Collapsed;
            inkToolbar.Visibility = Visibility.Collapsed;
            paper.Visibility = Visibility.Collapsed;
            mainCanvas.InkPresenter.StrokeContainer.Clear();
            unlockView();
        }
    }
}
