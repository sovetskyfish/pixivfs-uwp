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

        public IllustDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ((Frame.Parent as Grid)?.Parent as MainPage)?.SelectNavPlaceholder("详情");
            illustID = (int)e.Parameter;
            base.OnNavigatedTo(e);
            ImageList.MaxHeight = Frame.ActualHeight - 150;
            txtTitle.Text = "加载中";
            _ = loadContent();
        }

        private async Task loadContent()
        {
            var res = await Task.Run(() =>
                new PixivAppAPI(Data.OverAll.GlobalBaseAPI)
                .csfriendly_illust_detail(illustID.ToString()));
            ImageList.ItemsSource = new ObservableCollection<ViewModels.ImageItemViewModel>();
            illust = Data.IllustDetail.FromJsonValue(res);
            txtTitle.Text = illust.Title;
            int counter = 0;
            foreach (var i in illust.OriginalUrls)
            {
                txtLoadingStatus.Text = string.Format("正在加载第{0}张，共{1}张", ++counter, illust.OriginalUrls.Count);
                (ImageList.ItemsSource as ObservableCollection<ViewModels.ImageItemViewModel>)
                    .Add(new ViewModels.ImageItemViewModel()
                    {
                        ImageSource = await Data.OverAll.LoadImageAsync(i)
                    });
            }
            txtLoadingStatus.Text = string.Format("共{0}张作品", illust.OriginalUrls.Count);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ImageList.MaxHeight = Frame.ActualHeight - 150;
        }
    }
}
