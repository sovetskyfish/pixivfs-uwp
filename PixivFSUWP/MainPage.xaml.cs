using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace PixivFSUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            List<ViewModels.WaterfallItemViewModel> testItems = new List<ViewModels.WaterfallItemViewModel>();
            testItems.Add(ViewModels.WaterfallItemViewModel.FromItem(new Data.WaterfallItem() { ImageUri = "http://scp-wiki.wdfiles.com/local--files/scp-002/800px-SCP002.jpg", Title = "Test" }));
            testItems.Add(ViewModels.WaterfallItemViewModel.FromItem(new Data.WaterfallItem() { ImageUri = "http://scp-wiki.wdfiles.com/local--files/scp-003/SCP-003a.jpg", Title = "Test" }));
            testItems.Add(ViewModels.WaterfallItemViewModel.FromItem(new Data.WaterfallItem() { ImageUri = "http://scp-wiki.wdfiles.com/local--files/scp-004/SCP004_door.jpg", Title = "Test" }));
            testItems.Add(ViewModels.WaterfallItemViewModel.FromItem(new Data.WaterfallItem() { ImageUri = "http://scp-wiki.wdfiles.com/local--files/scp-005/SCP-005.jpg", Title = "Test" }));
            testItems.Add(ViewModels.WaterfallItemViewModel.FromItem(new Data.WaterfallItem() { ImageUri = "http://scp-wiki.wdfiles.com/local--files/scp-002/800px-SCP002.jpg", Title = "Test" }));
            testItems.Add(ViewModels.WaterfallItemViewModel.FromItem(new Data.WaterfallItem() { ImageUri = "http://scp-wiki.wdfiles.com/local--files/scp-003/SCP-003a.jpg", Title = "Test" }));
            testItems.Add(ViewModels.WaterfallItemViewModel.FromItem(new Data.WaterfallItem() { ImageUri = "http://scp-wiki.wdfiles.com/local--files/scp-004/SCP004_door.jpg", Title = "Test" }));
            testItems.Add(ViewModels.WaterfallItemViewModel.FromItem(new Data.WaterfallItem() { ImageUri = "http://scp-wiki.wdfiles.com/local--files/scp-005/SCP-005.jpg", Title = "Test" }));
            testItems.Add(ViewModels.WaterfallItemViewModel.FromItem(new Data.WaterfallItem() { ImageUri = "http://scp-wiki.wdfiles.com/local--files/scp-002/800px-SCP002.jpg", Title = "Test" }));
            testItems.Add(ViewModels.WaterfallItemViewModel.FromItem(new Data.WaterfallItem() { ImageUri = "http://scp-wiki.wdfiles.com/local--files/scp-003/SCP-003a.jpg", Title = "Test" }));
            testItems.Add(ViewModels.WaterfallItemViewModel.FromItem(new Data.WaterfallItem() { ImageUri = "http://scp-wiki.wdfiles.com/local--files/scp-004/SCP004_door.jpg", Title = "Test" }));
            testItems.Add(ViewModels.WaterfallItemViewModel.FromItem(new Data.WaterfallItem() { ImageUri = "http://scp-wiki.wdfiles.com/local--files/scp-005/SCP-005.jpg", Title = "Test" }));
            testItems.Add(ViewModels.WaterfallItemViewModel.FromItem(new Data.WaterfallItem() { ImageUri = "http://scp-wiki.wdfiles.com/local--files/scp-002/800px-SCP002.jpg", Title = "Test" }));
            testItems.Add(ViewModels.WaterfallItemViewModel.FromItem(new Data.WaterfallItem() { ImageUri = "http://scp-wiki.wdfiles.com/local--files/scp-003/SCP-003a.jpg", Title = "Test" }));
            testItems.Add(ViewModels.WaterfallItemViewModel.FromItem(new Data.WaterfallItem() { ImageUri = "http://scp-wiki.wdfiles.com/local--files/scp-004/SCP004_door.jpg", Title = "Test" }));
            testItems.Add(ViewModels.WaterfallItemViewModel.FromItem(new Data.WaterfallItem() { ImageUri = "http://scp-wiki.wdfiles.com/local--files/scp-005/SCP-005.jpg", Title = "Test" }));
            WaterfallListView.ItemsSource = testItems;
        }

        private void AdaptiveStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {

        }
    }
}
