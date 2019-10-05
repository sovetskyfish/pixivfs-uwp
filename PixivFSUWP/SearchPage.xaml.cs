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
using Windows.UI.Xaml.Navigation;
using PixivCS;
using Windows.UI.Xaml.Media.Imaging;
using PixivFSUWP.Interfaces;
using static PixivFSUWP.Data.OverAll;
// 额外内容
using PixivFSUWP.SauceNao;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PixivFSUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchPage : Page, IGoBackFlag
    {
        string lastWord = null;
        int lastIndex1 = -1;
        int lastIndex2 = -1;
        int lastIndex3 = -1;

        public SearchPage()
        {
            this.InitializeComponent();
            _ = loadContents();
        }

        private bool _backflag { get; set; } = false;

        public void SetBackFlag(bool value)
        {
            _backflag = value;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            (resultFrame.Content as SearchResultPage)?.ItemsSource?.StopLoading();
            if (!_backflag)
            {
                Data.Backstack.Default.Push(typeof(SearchPage), null);
                ((Frame.Parent as Grid)?.Parent as MainPage)?.UpdateNavButtonState();
            }
        }

        async Task<List<ViewModels.TagViewModel>> getTrendingTags()
        {
            try
            {
                var res = await new PixivAppAPI(Data.OverAll.GlobalBaseAPI).TrendingTagsIllust();
                var array = res["trend_tags"].GetArray();
                List<ViewModels.TagViewModel> toret = new List<ViewModels.TagViewModel>();
                foreach (var i in array)
                    toret.Add(new ViewModels.TagViewModel() { Tag = i.GetObject()["tag"].GetString() });
                return toret;
            }
            catch
            {
                return null;
            }
        }

        async Task loadContents()
        {
            var tags = await getTrendingTags();
            progressRing.IsActive = false;
            progressRing.Visibility = Visibility.Collapsed;
            stkMain.Visibility = Visibility.Visible;
            panelTags.ItemsSource = tags;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ((Frame.Parent as Grid)?.Parent as MainPage)?.SelectNavPlaceholder(GetResourceString("SearchPagePlain"));
        }

        public async Task ShowSearch()
        {
            if (grdSearchPanel.Visibility == Visibility.Collapsed)
            {
                searchProgressRing.Visibility = Visibility.Collapsed;
                searchProgressRing.IsActive = false;
                grdSearchPanel.Visibility = Visibility.Visible;
                stkMain.Visibility = Visibility.Collapsed;
                storyShow.Begin();
                await Task.Delay(200);
            }
            else stkMain.Visibility = Visibility.Collapsed;
            progressRing.IsActive = true;
            progressRing.Visibility = Visibility.Visible;
            (panelTags.ItemsSource as List<ViewModels.TagViewModel>).Clear();
            panelTags.ItemsSource = null;
            await loadContents();
        }

        private async void TxtWord_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(txtWord.Text)) return;
            if (txtWord.Text.Trim() != lastWord || cbSearchTarget.SelectedIndex != lastIndex1 ||
                cbSort.SelectedIndex != lastIndex2 || cbDuration.SelectedIndex != lastIndex3)
            {
                if (resultFrame.Content != null)
                    (resultFrame.Content as SearchResultPage).ItemsSource.CollectionChanged -= ItemsSource_CollectionChanged;
                var param = new SearchResultPage.SearchParam()
                {
                    Word = txtWord.Text.Trim()
                };
                switch (cbSearchTarget.SelectedIndex)
                {
                    case 0:
                        param.SearchTarget = "partial_match_for_tags";
                        break;
                    case 1:
                        param.SearchTarget = "exact_match_for_tags";
                        break;
                    case 2:
                        param.SearchTarget = "title_and_caption";
                        break;
                }
                switch (cbSort.SelectedIndex)
                {
                    case 0:
                        param.Sort = "date_desc";
                        break;
                    case 1:
                        param.Sort = "date_asc";
                        break;
                }
                switch (cbDuration.SelectedIndex)
                {
                    case 0:
                        param.Duration = null;
                        break;
                    case 1:
                        param.Duration = "within_last_day";
                        break;
                    case 2:
                        param.Duration = "within_last_week";
                        break;
                    case 3:
                        param.Duration = "within_last_month";
                        break;
                }
                resultFrame.Navigate(typeof(SearchResultPage), param);
                (resultFrame.Content as SearchResultPage).ItemsSource.CollectionChanged += ItemsSource_CollectionChanged;
            }
            storyFade.Begin();
            await Task.Delay(200);
            grdSearchPanel.Visibility = Visibility.Collapsed;
            if (txtWord.Text.Trim() != lastWord || cbSearchTarget.SelectedIndex != lastIndex1 ||
                cbSort.SelectedIndex != lastIndex2 || cbDuration.SelectedIndex != lastIndex3)
            {
                lastWord = txtWord.Text.Trim();
                lastIndex1 = cbSearchTarget.SelectedIndex;
                lastIndex2 = cbSort.SelectedIndex;
                lastIndex3 = cbDuration.SelectedIndex;
                searchProgressRing.IsActive = true;
                searchProgressRing.Visibility = Visibility.Visible;
            }
        }

        private void ItemsSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            searchProgressRing.Visibility = Visibility.Collapsed;
            searchProgressRing.IsActive = false;
        }

        private void BtnTag_Click(object sender, RoutedEventArgs e)
        {
            txtWord.Text = (sender as Button).Tag as string;
            cbSearchTarget.SelectedIndex = 1;
            cbSort.SelectedIndex = 0;
            cbDuration.SelectedIndex = 0;
            TxtWord_QuerySubmitted(null, null);
        }

        private async void btnSauceNAO_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SauceNAOPage));
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string SAUCENAO_API_KEY = localSettings.Values["SauceNAOAPI"] as string;//读取设置项
            string IMGUR_API_KEY = localSettings.Values["ImgurAPI"] as string;
            if (SAUCENAO_API_KEY == null)
            {
                // "未设置SauceNAO API"
                Frame.Navigate(typeof(SettingsPage));
                return;
                // SAUCENAO_API_KEY = "默认API";
            }
            else if (SAUCENAO_API_KEY.Length == 0)
            {
                // "未设置SauceNAO API"
                Frame.Navigate(typeof(SettingsPage));
                return;
                // SAUCENAO_API_KEY = "默认API";
            }
            if (IMGUR_API_KEY == null)
            {
                // "未设置Imger API"
                Frame.Navigate(typeof(SettingsPage));
                return;
                // IMGUR_API_KEY = "默认API";
            }
            else if (IMGUR_API_KEY.Length == 0)
            {
                // "未设置Imger API"
                Frame.Navigate(typeof(SettingsPage));
                return;
                // IMGUR_API_KEY = "默认API";
            }
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file == null)
            {
                Frame.GoBack();
                return;
            }
            byte[] IMAGE_PATH = await StorageFileExt.AsByteArray(file);

            Action<byte[]> action = IMAGE_BYTES =>
            {
                Console.WriteLine("==== 以图搜源 ====");
                string image = Imgur.Upload(IMAGE_BYTES, IMGUR_API_KEY);
                List<Result> results = new SauceNao.SauceNao(SAUCENAO_API_KEY).Request(image, null);
                // 这里是调试输出查询结果的内容..
                //results.RemoveAll(result => !result.HasRecognizableSauce());
                //foreach (Result result in results)
                //{
                //    System.Diagnostics.Debug.WriteLine(result.ToString() + "\n");
                //}
                System.Diagnostics.Debug.WriteLine("Pixiv ID = " + results[0].Response.SauceId.ToString());
                Frame.Navigate(typeof(IllustDetailPage), results[0].Response.SauceId);
            };
            action.Invoke(IMAGE_PATH);
        }

        private void BtnGoTo_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GoToPIDPage));
        }
    }
    static class StorageFileExt
    {
        /// <summary>
        /// 将文件转换为字节数组
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static async Task<byte[]> AsByteArray(this Windows.Storage.StorageFile file)
        {
            Windows.Storage.Streams.IRandomAccessStream fileStream =
                await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            var reader = new Windows.Storage.Streams.DataReader(fileStream.GetInputStreamAt(0));
            await reader.LoadAsync((uint)fileStream.Size);
            byte[] pixels = new byte[fileStream.Size];
            reader.ReadBytes(pixels);
            return pixels;
        }
    }
}
