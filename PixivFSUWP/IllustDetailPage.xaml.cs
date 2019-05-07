using FSharp.Data;
using PixivFS;
using PixivFSCS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PixivFSUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class IllustDetailPage : Page
    {
        int illustID;
        Data.IllustDetail illust;

        Task<WriteableBitmap> loadingTask = null;

        public IllustDetailPage()
        {
            this.InitializeComponent();
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var request = args.Request;
            request.Data.SetText(string.Format("Pixiv作品\n{0} by {1}\n" +
                "网页链接：https://www.pixiv.net/member_illust.php?mode=medium&illust_id={2}\n" +
                "PixivFSUWP：pixiv://illust?id={2}", illust.Title, illust.Author, illustID));
            request.Data.Properties.Title = string.Format("分享：{0}", illust.Title);
            request.Data.Properties.Description = "该图片页面的链接将被分享";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ((Frame.Parent as Grid)?.Parent as MainPage)?.SelectNavPlaceholder("详情");
            illustID = (int)e.Parameter;
            base.OnNavigatedTo(e);
            _ = loadContent();
        }

        private async Task loadContent()
        {
            txtTitle.Text = "加载中";
            iconView.Visibility = Visibility.Collapsed;
            iconStar.Visibility = Visibility.Collapsed;
            stkAuthor.Visibility = Visibility.Collapsed;
            var res = await Task.Run(() =>
                new PixivAppAPI(Data.OverAll.GlobalBaseAPI)
                .csfriendly_illust_detail(illustID.ToString()));
            ImageList.ItemsSource = new ObservableCollection<ViewModels.ImageItemViewModel>();
            illust = Data.IllustDetail.FromJsonValue(res);
            imgAuthor.ImageSource = await Data.OverAll.LoadImageAsync(illust.AuthorAvatarUrl);
            txtTitle.Text = illust.Title;
            btnBookmark.IsChecked = illust.IsBookmarked;
            btnFollow.IsChecked = illust.IsUserFollowed;
            txtBtnBookmark.Text = illust.IsBookmarked ? "已收藏" : "未收藏";
            txtBtnFollow.Text = illust.IsUserFollowed ? "已关注" : "未关注";
            iconView.Visibility = Visibility.Visible;
            iconStar.Visibility = Visibility.Visible;
            stkAuthor.Visibility = Visibility.Visible;
            txtViewStatus.Text = illust.TotalView.ToString();
            txtBookmarkStatus.Text = illust.TotalBookmarks.ToString();
            txtAuthor.Text = illust.Author;
            txtAuthorAccount.Text = string.Format("@{0}", illust.AuthorAccount);
            txtCaption.Text = (illust.Caption == "") ? "暂无简介" : Regex.Replace(illust.Caption.Replace("<br />", "\n"), "<[^>]+>", "");
            txtCommentTitle.Text = "评论";
            listComments.ItemsSource = new Data.CommentsCollection(illustID.ToString());
            var dataUri = await Data.OverAll.GetDataUri(illust.MediumUrl);
            await Data.OverAll.GenerateActivityAsync(illust.Title, string.Format("by {0}", illust.Author),
                new Uri(string.Format("pixiv://illust?id={0}", illustID)), illustID.ToString());
            int counter = 0;
            foreach (var i in illust.OriginalUrls)
            {
                txtLoadingStatus.Text = string.Format("正在加载第 {0} 张，共 {1} 张", ++counter, illust.OriginalUrls.Count);
                loadingTask = Data.OverAll.LoadImageAsync(i, 1, 1);
                (ImageList.ItemsSource as ObservableCollection<ViewModels.ImageItemViewModel>)
                    .Add(new ViewModels.ImageItemViewModel()
                    {
                        ImageSource = await Data.OverAll.LoadImageAsync(i, 1, 1)
                    });
            }
            txtLoadingStatus.Text = string.Format("共 {0} 张作品，点击或触摸查看大图", illust.OriginalUrls.Count);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ImageList.MaxHeight = Frame.ActualHeight - 265;
        }

        private async void ImageList_ItemClick(object sender, ItemClickEventArgs e)
        {

            var Item = e.ClickedItem as ViewModels.ImageItemViewModel;
            await Data.OverAll.ShowNewWindow(typeof(BigImage), new Data.BigImageDetail()
            {
                Title = illust.Title,
                Width = Item.ImageSource.PixelWidth,
                Height = Item.ImageSource.PixelHeight,
                Author = illust.Author,
                Image = await Data.OverAll.ImageToBytes(Item.ImageSource)
            });
        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if ((sender as ScrollViewer).VerticalOffset >= (sender as ScrollViewer).ScrollableHeight - 500)
            {
                try
                {
                    await (listComments.ItemsSource as ISupportIncrementalLoading)?.LoadMoreItemsAsync(0);
                }
                catch { }
            }
        }

        private async void BtnBookmark_Click(object sender, RoutedEventArgs e)
        {
            var btnSender = sender as ToggleButton;
            btnSender.IsEnabled = false;
            if (btnSender.IsChecked == true)
            {
                btnSender.IsChecked = false;
                //进行关注
                txtBtnBookmark.Text = "请求中";
                var res = await Task.Run(() =>
                {
                    try
                    {
                        new PixivAppAPI(Data.OverAll.GlobalBaseAPI)
                            .csfriendly_illust_bookmark_add(illustID.ToString());
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                });
                if (res)
                {
                    btnSender.IsChecked = true;
                    txtBtnBookmark.Text = "已收藏";
                    txtBookmarkStatus.Text = illust.IsBookmarked ? illust.TotalBookmarks.ToString() : (illust.TotalBookmarks + 1).ToString();
                }
                btnSender.IsEnabled = true;
            }
            else
            {
                btnSender.IsChecked = true;
                //取消关注
                txtBtnBookmark.Text = "请求中";
                var res = await Task.Run(() =>
                {
                    try
                    {
                        new PixivAppAPI(Data.OverAll.GlobalBaseAPI)
                            .csfriendly_illust_bookmark_delete(illustID.ToString());
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                });
                if (res)
                {
                    btnSender.IsChecked = false;
                    txtBtnBookmark.Text = "未收藏";
                    txtBookmarkStatus.Text = illust.IsBookmarked ? (illust.TotalBookmarks - 1).ToString() : illust.TotalBookmarks.ToString();
                }
                btnSender.IsEnabled = true;
            }
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
                var res = await Task.Run(() =>
                {
                    try
                    {
                        new PixivAppAPI(Data.OverAll.GlobalBaseAPI)
                            .csfriendly_user_follow_add(illust.AuthorID.ToString());
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                });
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
                var res = await Task.Run(() =>
                {
                    try
                    {
                        new PixivAppAPI(Data.OverAll.GlobalBaseAPI)
                            .csfriendly_user_follow_delete(illust.AuthorID.ToString());
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                });
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
            copyToClipboard(string.Format("https://www.pixiv.net/member_illust.php?mode=medium&illust_id={0}", illustID));
            btnShareFlyout.Hide();
        }

        private void BtnAppLink_Click(object sender, RoutedEventArgs e)
        {
            copyToClipboard(string.Format("pixiv://illust?id={0}", illustID));
            btnShareFlyout.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserDetailPage), illust.AuthorID);
        }
    }
}
