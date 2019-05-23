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
            panelTags.ItemsSource = tags;
            progressRing.IsActive = false;
            progressRing.Visibility = Visibility.Collapsed;
            stkMain.Visibility = Visibility.Visible;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ((Frame.Parent as Grid)?.Parent as MainPage)?.SelectNavPlaceholder("搜索");
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

        public async Task ShowSearch()
        {
            grdSearchPanel.Visibility = Visibility.Visible;
            stkMain.Visibility = Visibility.Collapsed;
            progressRing.IsActive = true;
            progressRing.Visibility = Visibility.Visible;
            await loadContents();
        }
    }
}
