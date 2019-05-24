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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PixivFSUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        public SearchPage()
        {
            this.InitializeComponent();
            _ = loadContents();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            (resultFrame.Content as SearchResultPage)?.ItemsSource?.StopLoading();
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
            ((Frame.Parent as Grid)?.Parent as MainPage)?.SelectNavPlaceholder("搜索");
        }

        public async Task ShowSearch()
        {
            if (grdSearchPanel.Visibility == Visibility.Collapsed)
            {
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
            if (resultFrame.Content != null)
                (resultFrame.Content as SearchResultPage).ItemsSource.CollectionChanged -= ItemsSource_CollectionChanged;
            resultFrame.Navigate(typeof(SearchResultPage),
                new SearchResultPage.SearchParam()
                {
                    Word = txtWord.Text.Trim()
                });
            (resultFrame.Content as SearchResultPage).ItemsSource.CollectionChanged += ItemsSource_CollectionChanged;
            storyFade.Begin();
            await Task.Delay(200);
            grdSearchPanel.Visibility = Visibility.Collapsed;
            searchProgressRing.IsActive = true;
            searchProgressRing.Visibility = Visibility.Visible;
        }

        private void ItemsSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            searchProgressRing.Visibility = Visibility.Collapsed;
            searchProgressRing.IsActive = false;
        }
    }
}
