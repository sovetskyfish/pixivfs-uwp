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
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Provider;
using PixivFSUWP.Interfaces;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PixivFSUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class UserDetailPage : Page, IGoBackFlag
    {
        UserIllustsCollection itemsSource;
        int userid = 0;
        Data.UserDetail detail;
        public UserDetailPage()
        {
            this.InitializeComponent();
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }

        private bool _backflag { get; set; } = false;

        public void SetBackFlag(bool value)
        {
            _backflag = value;
        }

        private void ItemsSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            btnWorks.IsEnabled = true;
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var request = args.Request;
            request.Data.SetText(string.Format("Pixiv用户\n{0}\n" +
                "网页链接：https://www.pixiv.net/member.php?id={1}\n" +
                "PixivFSUWP：pixiv://user?id={1}", detail.Name, detail.ID));
            request.Data.Properties.Title = string.Format("画师：{0}", detail.Name);
            request.Data.Properties.Description = "该用户页面的链接将被分享";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ((Frame.Parent as Grid)?.Parent as MainPage)?.SelectNavPlaceholder("用户");
            userid = (int)e.Parameter;
            itemsSource = new UserIllustsCollection(userid.ToString());
            itemsSource.CollectionChanged += ItemsSource_CollectionChanged;
            WaterfallListView.ItemsSource = itemsSource;
            base.OnNavigatedTo(e);
            _ = loadContents();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            itemsSource?.StopLoading();
            itemsSource = null;
            base.OnNavigatedFrom(e);
            if (!_backflag)
            {
                Data.Backstack.Default.Push(typeof(UserDetailPage), userid);
                ((Frame.Parent as Grid)?.Parent as MainPage)?.UpdateNavButtonState();
            }
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
        }

        private void WaterfallContent_Loaded(object sender, RoutedEventArgs e)
        {
            if (ActualWidth < 700) (sender as Controls.WaterfallContentPanel).Colums = 3;
            else if (ActualWidth < 900) (sender as Controls.WaterfallContentPanel).Colums = 4;
            else if (ActualWidth < 1100) (sender as Controls.WaterfallContentPanel).Colums = 5;
            else (sender as Controls.WaterfallContentPanel).Colums = 6;
        }

        private void WaterfallListView_ItemClick(object sender, ItemClickEventArgs e)
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

        private void BtnShare_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        void copyToClipboard(string content)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(content);
            Clipboard.SetContent(dataPackage);
        }

        private void BtnLink_Click(object sender, RoutedEventArgs e)
        {
            copyToClipboard(string.Format("https://www.pixiv.net/member.php?id={0}", detail.ID));
            btnShareFlyout.Hide();
        }

        private void BtnAppLink_Click(object sender, RoutedEventArgs e)
        {
            copyToClipboard(string.Format("pixiv://user?id={0}", detail.ID));
            btnShareFlyout.Hide();
        }

        ViewModels.WaterfallItemViewModel tapped = null;

        private void WaterfallListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ListView listView = (ListView)sender;
            tapped = ((FrameworkElement)e.OriginalSource).DataContext as ViewModels.WaterfallItemViewModel;
            quickStar.Text = (tapped.IsBookmarked) ? "删除收藏" : "快速收藏";
            quickStar.IsEnabled = tapped.Title != null;
            quickActions.ShowAt(listView, e.GetPosition(listView));
        }

        private void WaterfallListView_Holding(object sender, HoldingRoutedEventArgs e)
        {
            ListView listView = (ListView)sender;
            tapped = ((FrameworkElement)e.OriginalSource).DataContext as ViewModels.WaterfallItemViewModel;
            quickStar.Text = (tapped.IsBookmarked) ? "删除收藏" : "快速收藏";
            quickStar.IsEnabled = tapped.Title != null;
            quickActions.ShowAt(listView, e.GetPosition(listView));
        }

        private async void QuickStar_Click(object sender, RoutedEventArgs e)
        {
            if (tapped == null) return;
            var i = tapped;
            var title = i.Title;
            try
            {
                //用Title作标识，表明任务是否在执行
                i.Title = null;
                if (i.IsBookmarked)
                {
                    bool res;
                    try
                    {
                        await new PixivAppAPI(Data.OverAll.GlobalBaseAPI)
                            .IllustBookmarkDelete(i.ItemId.ToString());
                        res = true;
                    }
                    catch
                    {
                        res = false;
                    }
                    i.Title = title;
                    if (res)
                    {
                        i.IsBookmarked = false;
                        i.Stars--;
                        i.NotifyChange("StarsString");
                        i.NotifyChange("IsBookmarked");
                        await ((Frame.Parent as Grid)?.Parent as MainPage)?.
                            ShowTip(string.Format("作品「{0}」已删除收藏", title));
                    }
                    else
                    {
                        await ((Frame.Parent as Grid)?.Parent as MainPage)?.
                            ShowTip(string.Format("作品「{0}」删除收藏失败", title));
                    }
                }
                else
                {
                    bool res;
                    try
                    {
                        await new PixivAppAPI(Data.OverAll.GlobalBaseAPI)
                            .IllustBookmarkAdd(i.ItemId.ToString());
                        res = true;
                    }
                    catch
                    {
                        res = false;
                    }
                    i.Title = title;
                    if (res)
                    {
                        i.IsBookmarked = true;
                        i.Stars++;
                        i.NotifyChange("StarsString");
                        i.NotifyChange("IsBookmarked");
                        await ((Frame.Parent as Grid)?.Parent as MainPage)?.
                            ShowTip(string.Format("作品「{0}」已收藏", title));
                    }
                    else
                    {
                        await ((Frame.Parent as Grid)?.Parent as MainPage)?.
                            ShowTip(string.Format("作品「{0}」收藏失败", title));
                    }
                }
            }
            finally
            {
                //确保出错时数据不被破坏
                i.Title = title;
            }
        }

        private async void QuickSave_Click(object sender, RoutedEventArgs e)
        {
            if (tapped == null) return;
            var i = tapped;
            FileSavePicker picker = new FileSavePicker();
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeChoices.Add("图片文件", new List<string>() { ".png" });
            picker.SuggestedFileName = i.Title;
            var file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                var res = await new PixivAppAPI(Data.OverAll.GlobalBaseAPI)
                    .IllustDetail(i.ItemId.ToString());
                var illust = Data.IllustDetail.FromJsonValue(res);
                using (var imgstream = await Data.OverAll.DownloadImage(illust.OriginalUrls[0]))
                {
                    using (var filestream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        await imgstream.CopyToAsync(filestream.AsStream());
                    }
                }
                var updateStatus = await CachedFileManager.CompleteUpdatesAsync(file);
                if (updateStatus == FileUpdateStatus.Complete)
                    await ((Frame.Parent as Grid)?.Parent as MainPage)?.
                            ShowTip(string.Format("作品「{0}」已保存", i.Title));
                else
                    await ((Frame.Parent as Grid)?.Parent as MainPage)?.
                            ShowTip(string.Format("作品「{0}」保存失败", i.Title));
            }
        }
    }
}
