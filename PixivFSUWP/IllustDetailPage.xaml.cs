using PixivCS;
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
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using AdaptiveCards;
using Microsoft.Toolkit.Uwp.Helpers;
using System.Threading;
using Lumia.Imaging;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

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
        IBuffer buffer;

        Data.Ugoira ugoira;

        bool _emergencyStop = false;
        bool _busy = false;
        bool _playing = true;

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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _emergencyStop = true;
            ugoiraPlayer.Source = null;
            if (!_busy)
                (ImageList.ItemsSource as ObservableCollection<ViewModels.ImageItemViewModel>)?.Clear();
            GC.Collect();
            base.OnNavigatedFrom(e);
        }

        private async Task loadContent()
        {
            try
            {
                _busy = true;
                txtTitle.Text = "加载中";
                iconView.Visibility = Visibility.Collapsed;
                iconStar.Visibility = Visibility.Collapsed;
                stkAuthor.Visibility = Visibility.Collapsed;
                var res = await new PixivAppAPI(Data.OverAll.GlobalBaseAPI)
                    .IllustDetail(illustID.ToString());
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
                txtLoadingStatus.Text = "正在创建时间线";
                AdaptiveCard card = new AdaptiveCard("1.1");
                card.Body.Add(new AdaptiveTextBlock()
                {
                    Text = illust.Title,
                    Weight = AdaptiveTextWeight.Bolder,
                    Size = AdaptiveTextSize.Large,
                    Wrap = true,
                    MaxLines = 3
                });
                card.Body.Add(new AdaptiveTextBlock()
                {
                    Text = string.Format("by {0}", illust.Author),
                    Size = AdaptiveTextSize.Default,
                    Wrap = true,
                    MaxLines = 3
                });
                var build = SystemInformation.OperatingSystemVersion.Build;
                if (build >= 18362)
                {
                    var data = await Data.OverAll.GetDataUri(illust.MediumUrl);
                    card.BackgroundImage = new AdaptiveBackgroundImage(new Uri(data));
                }
                await Data.OverAll.GenerateActivityAsync(illust.Title, card, new Uri(string.Format("pixiv://illust?id={0}", illustID)), illustID.ToString());
                if (illust.Type == "ugoira")
                {
                    ugoiraPlayer.Visibility = Visibility.Visible;
                    ImageList.Visibility = Visibility.Collapsed;
                    btnPlay.IsEnabled = false;
                    btnPlay.Visibility = Visibility.Visible;
                    btnSaveGif.IsEnabled = false;
                    btnSaveGif.Visibility = Visibility.Visible;
                    txtLoadingStatus.Text = "正在加载预览";
                    ugoiraPlayer.Source = await Data.OverAll.LoadImageAsync(illust.OriginalUrls[0]);
                    txtLoadingStatus.Text = "正在下载动态剪影";
                    if (_emergencyStop)
                    {
                        return;
                    }
                    ugoira = await Data.UgoiraHelper.GetUgoiraAsync(illust.IllustID.ToString());
                    txtLoadingStatus.Text = "正在生成 Gif";
                    await playUgoira();
                    txtLoadingStatus.Text = "正在播放动态剪影";
                    btnPlay.IsEnabled = true;
                    btnSaveGif.IsEnabled = true;
                }
                else
                {
                    int counter = 0;
                    foreach (var i in illust.OriginalUrls)
                    {
                        if (_emergencyStop)
                        {
                            return;
                        }
                        txtLoadingStatus.Text = string.Format("正在加载第 {0} 张，共 {1} 张", ++counter, illust.OriginalUrls.Count);
                        (ImageList.ItemsSource as ObservableCollection<ViewModels.ImageItemViewModel>)
                            .Add(new ViewModels.ImageItemViewModel()
                            {
                                ImageSource = await Data.OverAll.LoadImageAsync(i, 1, 1)
                            });
                    }
                    txtLoadingStatus.Text = string.Format("共 {0} 张作品，点击或触摸查看大图", illust.OriginalUrls.Count);
                }
            }
            finally
            {
                _busy = false;
                if (_emergencyStop)
                {
                    (ImageList.ItemsSource as ObservableCollection<ViewModels.ImageItemViewModel>)?.Clear();
                    GC.Collect();
                }
            }
        }

        //当场生成Gif
        async Task playUgoira()
        {
            try
            {
                using (GifRenderer renderer = new GifRenderer())
                {
                    renderer.Duration = ugoira.Frames[0].Delay;
                    renderer.Size = new Size(ugoira.Frames[0].Image.PixelWidth, ugoira.Frames[0].Image.PixelHeight);
                    List<IImageProvider> sources = new List<IImageProvider>();
                    foreach (var i in ugoira.Frames)
                        sources.Add(new SoftwareBitmapImageSource(i.Image));
                    renderer.Sources = sources;
                    buffer = await renderer.RenderAsync();
                }
                var gif = new BitmapImage();
                await gif.SetSourceAsync(buffer.AsStream().AsRandomAccessStream());
                ugoiraPlayer.Source = gif;
            }
            finally
            {
                ugoira.Dispose();
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ImageList.MaxHeight = Frame.ActualHeight - 265;
            ugoiraPlayer.MaxHeight = Frame.ActualHeight - 265;
            ugoiraPlayer.Height = Frame.ActualHeight - 265;
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
                bool res;
                try
                {
                    await new PixivAppAPI(Data.OverAll.GlobalBaseAPI)
                        .IllustBookmarkAdd(illustID.ToString());
                    res = true;
                }
                catch
                {
                    res = false;
                }
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
                bool res;
                try
                {
                    await new PixivAppAPI(Data.OverAll.GlobalBaseAPI)
                        .IllustBookmarkDelete(illustID.ToString());
                    res = true;
                }
                catch
                {
                    res = false;
                }
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
                bool res;
                try
                {
                    await new PixivAppAPI(Data.OverAll.GlobalBaseAPI)
                        .UserFollowAdd(illust.AuthorID.ToString());
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
                        .UserFollowDelete(illust.AuthorID.ToString());
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

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (_playing)
            {
                (ugoiraPlayer.Source as BitmapImage).Stop();
                txtPlay.Text = "播放";
                iconPlay.Glyph = "";
            }
            else
            {
                (ugoiraPlayer.Source as BitmapImage).Play();
                txtPlay.Text = "停止";
                iconPlay.Glyph = "";
            }
            _playing = !_playing;
        }

        private async void BtnSaveGif_Click(object sender, RoutedEventArgs e)
        {
            await saveImage();
        }

        async Task saveImage()
        {
            FileSavePicker picker = new FileSavePicker();
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeChoices.Add("Gif文件", new List<string>() { ".gif" });
            picker.SuggestedFileName = illust.Title;
            var file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await stream.WriteAsync(buffer);
                }
                var updateStatus = await CachedFileManager.CompleteUpdatesAsync(file);
                if (updateStatus != FileUpdateStatus.Complete)
                {
                    var messageDialog = new MessageDialog("剪影保存失败");
                    messageDialog.Commands.Add(new UICommand("重试", async (a) => { await saveImage(); }));
                    messageDialog.Commands.Add(new UICommand("放弃"));
                    messageDialog.DefaultCommandIndex = 0;
                    messageDialog.CancelCommandIndex = 1;
                    await messageDialog.ShowAsync();
                }
                else
                {
                    var messageDialog = new MessageDialog("剪影已保存");
                    messageDialog.Commands.Add(new UICommand("好的"));
                    messageDialog.DefaultCommandIndex = 0;
                    await messageDialog.ShowAsync();
                }
            }
        }
    }
}
