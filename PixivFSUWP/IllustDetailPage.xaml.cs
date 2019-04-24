using FSharp.Data;
using PixivFS;
using PixivFSCS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            iconView.Visibility = Visibility.Visible;
            iconStar.Visibility = Visibility.Visible;
            stkAuthor.Visibility = Visibility.Visible;
            txtViewStatus.Text = illust.TotalView.ToString();
            txtBookmarkStatus.Text = illust.TotalBookmarks.ToString();
            txtAuthor.Text = illust.Author;
            txtAuthorAccount.Text = string.Format("@{0}", illust.AuthorAccount);
            txtCaption.Text = (illust.Caption == "") ? "暂无简介" : illust.Caption.Replace("<br />", "\n");
            txtCommentTitle.Text = "评论";
            listComments.ItemsSource = new Data.CommentsCollection(illustID.ToString());
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
    }
}
