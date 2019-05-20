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
using System.Threading.Tasks;
using PixivCS;
using PixivFSUWP.Data;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PixivFSUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class UserDetailPage : Page
    {
        UserIllustsCollection itemsSource;
        int userid = 0;
        Data.UserDetail detail;
        public UserDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ((Frame.Parent as Grid)?.Parent as MainPage)?.SelectNavPlaceholder("用户");
            userid = (int)e.Parameter;
            itemsSource = new UserIllustsCollection(userid.ToString());
            illustsList.ItemsSource = itemsSource;
            base.OnNavigatedTo(e);
            _ = loadContents();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            itemsSource?.StopLoading();
            itemsSource = null;
            base.OnNavigatedFrom(e);
        }

        async Task loadContents()
        {
            var res = await new PixivAppAPI(Data.OverAll.GlobalBaseAPI)
                    .UserDetail(userid.ToString());
            detail = Data.UserDetail.FromJsomValue(res);
            string _getText(string input) => (input == "") ? "保密" : input;
            txtUsername.Text = detail.Name;
            txtAuthor.Text = detail.Name;
            txtAccount.Text = "@" + detail.Account;
            txtAuthorAccount.Text = txtAccount.Text;
            txtWebPage.Text = (detail.WebPage == "") ? "无/保密" : detail.WebPage;
            if (detail.Gender == "") txtGender.Text = "保密";
            else txtGender.Text = (detail.Gender == "male") ? "男" : "女";
            txtBirthday.Text = _getText(detail.BirthDay);
            txtRegion.Text = _getText(detail.Region);
            txtJob.Text = _getText(detail.Job);
            string _getHW(string input) => (input == "") ? "未知" : input;
            txtPC.Text = _getHW(detail.PC);
            txtMonitor.Text = _getHW(detail.Monitor);
            txtTool.Text = _getHW(detail.Tool);
            txtScanner.Text = _getHW(detail.Scanner);
            txtTablet.Text = _getHW(detail.Tablet);
            txtMouse.Text = _getHW(detail.Mouse);
            txtPrinter.Text = _getHW(detail.Printer);
            txtDesktop.Text = _getHW(detail.Desktop);
            txtMusic.Text = _getHW(detail.Music);
            txtDesk.Text = _getHW(detail.Desk);
            txtChair.Text = _getHW(detail.Chair);
            txtBtnFollow.Text = detail.IsFollowed ? "已关注" : "未关注";
            btnFollow.IsChecked = detail.IsFollowed;
            imgAvatar.ImageSource = await Data.OverAll.LoadImageAsync(detail.AvatarUrl);
            imgAuthor.ImageSource = imgAvatar.ImageSource;
            _ = loadPage();
        }

        async Task loadPage()
        {
            while (scrollViewer.ScrollableHeight == 0)
                try
                {
                    await (itemsSource as ISupportIncrementalLoading)?.LoadMoreItemsAsync(0);
                }
                catch (InvalidOperationException)
                {
                    return;
                }
        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight - 500)
            {
                try
                {
                    await (itemsSource as ISupportIncrementalLoading)?.LoadMoreItemsAsync(0);
                }
                catch { }
            }
        }

        private void WaterfallContent_Loaded(object sender, RoutedEventArgs e)
        {
            if (ActualWidth < 700) (sender as Controls.WaterfallContentPanel).Colums = 3;
            else if (ActualWidth < 900) (sender as Controls.WaterfallContentPanel).Colums = 4;
            else if (ActualWidth < 1100) (sender as Controls.WaterfallContentPanel).Colums = 5;
            else (sender as Controls.WaterfallContentPanel).Colums = 6;
        }

        private void IllustsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(IllustDetailPage),
                (e.ClickedItem as ViewModels
                .WaterfallItemViewModel).ItemId);
        }

        private async void BtnWorks_Click(object sender, RoutedEventArgs e)
        {
            grdUserButton.Visibility = Visibility.Visible;
            storyFade.Begin();
            await Task.Delay(TimeSpan.FromMilliseconds(200));
            grdDetail.Visibility = Visibility.Collapsed;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            grdDetail.Visibility = Visibility.Visible;
            storyShow.Begin();
            await Task.Delay(TimeSpan.FromMilliseconds(200));
            grdUserButton.Visibility = Visibility.Collapsed;
        }

        private async void BtnFollow_Click(object sender, RoutedEventArgs e)
        {
            var btnSender = sender as ToggleButton;
            btnSender.IsEnabled = false;
            if (btnSender.IsChecked == true)
            {
                btnSender.IsChecked = false;
                //进行关注
                txtBtnFollow.Text = "请求中";
                bool res;
                try
                {
                    await new PixivAppAPI(Data.OverAll.GlobalBaseAPI)
                        .UserFollowAdd(detail.ID.ToString());
                    res = true;
                }
                catch
                {
                    res = false;
                }
                if (res)
                {
                    btnSender.IsChecked = true;
                    txtBtnFollow.Text = "已关注";
                }
                btnSender.IsEnabled = true;
            }
            else
            {
                btnSender.IsChecked = true;
                //取消关注
                txtBtnFollow.Text = "请求中";
                bool res;
                try
                {
                    await new PixivAppAPI(Data.OverAll.GlobalBaseAPI)
                        .UserFollowDelete(detail.ID.ToString());
                    res = true;
                }
                catch
                {
                    res = false;
                }
                if (res)
                {
                    btnSender.IsChecked = false;
                    txtBtnFollow.Text = "未关注";
                }
                btnSender.IsEnabled = true;
            }
        }
    }
}
