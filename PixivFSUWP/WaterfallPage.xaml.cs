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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PixivFSUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class WaterfallPage : Page
    {
        public enum ListContent
        {
            Recommend,
            Bookmark,
            Following,
            Ranking
        }

        ListContent listContent;

        public WaterfallPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is ListContent) listContent = (ListContent)e.Parameter;
            switch (listContent)
            {
                case ListContent.Recommend:
                    WaterfallListView.ItemsSource = Data.OverAll.RecommendList;
                    Data.OverAll.RecommendList.ResumeLoading();
                    break;
                case ListContent.Bookmark:
                    WaterfallListView.ItemsSource = Data.OverAll.BookmarkList;
                    Data.OverAll.BookmarkList.ResumeLoading();
                    break;
                case ListContent.Following:
                    WaterfallListView.ItemsSource = Data.OverAll.FollowingList;
                    Data.OverAll.FollowingList.ResumeLoading();
                    break;
                case ListContent.Ranking:
                    WaterfallListView.ItemsSource = Data.OverAll.RankingList;
                    Data.OverAll.RankingList.ResumeLoading();
                    break;
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            switch (listContent)
            {
                case ListContent.Recommend:
                    Data.OverAll.RecommendList.PauseLoading();
                    break;
                case ListContent.Bookmark:
                    Data.OverAll.BookmarkList.PauseLoading();
                    break;
                case ListContent.Following:
                    Data.OverAll.FollowingList.PauseLoading();
                    break;
                case ListContent.Ranking:
                    Data.OverAll.RankingList.PauseLoading();
                    break;
            }
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

        ViewModels.WaterfallItemViewModel tapped = null;

        private void WaterfallListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ListView listView = (ListView)sender;
            tapped = e.OriginalSource as ViewModels.WaterfallItemViewModel;
            quickActions.ShowAt(listView, e.GetPosition(listView));
        }

        private void WaterfallListView_Holding(object sender, HoldingRoutedEventArgs e)
        {
            ListView listView = (ListView)sender;
            tapped = e.OriginalSource as ViewModels.WaterfallItemViewModel;
            quickActions.ShowAt(listView, e.GetPosition(listView));
        }
    }
}
