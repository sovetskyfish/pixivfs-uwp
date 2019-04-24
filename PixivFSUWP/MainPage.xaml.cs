using PixivFSUWP.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace PixivFSUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            NavControl.SelectedItem = NavControl.MenuItems[0];
            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.ButtonForegroundColor = Colors.Black;
            view.TitleBar.ButtonInactiveForegroundColor = Colors.Gray;
            view.Title = "";
            txtUsername.Text = OverAll.currentUser.Username;
        }

        private async void NavControl_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                SelectNavPlaceholder("设置");
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                switch (sender.MenuItems.IndexOf(args.SelectedItem))
                {
                    case 0:
                        OverAll.RefreshRecommendList();
                        ContentFrame.Navigate(typeof(WaterfallPage), WaterfallPage.ListContent.Recommend);
                        NavPlaceholder.IsEnabled = false;
                        await Task.Delay(TimeSpan.FromMilliseconds(350));
                        NavSeparator.Visibility = Visibility.Collapsed;
                        NavPlaceholder.Visibility = Visibility.Collapsed;
                        break;
                    case 1:
                        OverAll.RefreshBookmarkList();
                        ContentFrame.Navigate(typeof(WaterfallPage), WaterfallPage.ListContent.Bookmark);
                        NavPlaceholder.IsEnabled = false;
                        await Task.Delay(TimeSpan.FromMilliseconds(350));
                        NavSeparator.Visibility = Visibility.Collapsed;
                        NavPlaceholder.Visibility = Visibility.Collapsed;
                        break;
                    case 2:
                        OverAll.RefreshFollowingList();
                        ContentFrame.Navigate(typeof(WaterfallPage), WaterfallPage.ListContent.Following);
                        NavPlaceholder.IsEnabled = false;
                        await Task.Delay(TimeSpan.FromMilliseconds(350));
                        NavSeparator.Visibility = Visibility.Collapsed;
                        NavPlaceholder.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }

        private void NavSelect(int index)
            => NavControl.SelectedItem = NavControl.MenuItems[index];

        public async void SelectNavPlaceholder(string title)
        {
            NavPlaceholder.IsEnabled = true;
            NavPlaceholder.Content = title;
            NavSeparator.Visibility = Visibility.Visible;
            NavPlaceholder.Visibility = Visibility.Visible;
            await Task.Delay(TimeSpan.FromMilliseconds(10));
            NavSelect(4);
        }
    }
}
