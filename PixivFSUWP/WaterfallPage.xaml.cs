using System;
using FSharp.Data;
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
using PixivFSCS;

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
            Following
        }

        ListContent listContent;

        ObservableCollection<ViewModels.WaterfallItemViewModel> listItems = new ObservableCollection<ViewModels.WaterfallItemViewModel>();

        public WaterfallPage()
        {
            this.InitializeComponent();
            WaterfallListView.ItemsSource = listItems;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is ListContent) listContent = (ListContent)e.Parameter;
            _ = LoadImages();
        }

        public async Task LoadImages()
        {
            switch (listContent)
            {
                case ListContent.Recommend:
                    var recommendres = await Task.Run(() => new PixivFS
                        .PixivAppAPI(Data.OverAll.GlobalBaseAPI)
                        .csfriendly_illust_recommended());
                    foreach (var recillust in recommendres.Item("illusts").AsArray())
                    {
                        Data.WaterfallItem recommendi = Data.WaterfallItem.FromJsonValue(recillust);
                        var recommendmodel = ViewModels.WaterfallItemViewModel.FromItem(recommendi);
                        await recommendmodel.LoadImageAsync();
                        listItems.Add(recommendmodel);
                    }
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
            (((Parent as Frame)?.Parent as Grid)?.Parent as MainPage)?.SelectNavPlaceholder("详情");
        }
    }
}
