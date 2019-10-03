using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace PixivFSUWP.Controls
{
    //似乎是我的panel弄坏了ListView，这里只能自己造一个(lll￢ω￢)
    public class WaterfallListView : ListView
    {
        bool busyLoading = false;

        public WaterfallListView() : base()
        {
            //不使用base的增量加载
            IncrementalLoadingTrigger = IncrementalLoadingTrigger.None;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var scrollViewer = GetTemplateChild("ScrollViewer") as ScrollViewer;
            scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
            _ = LoadPage();
        }

        private async Task LoadPage()
        {
            var scrollViewer = GetTemplateChild("ScrollViewer") as ScrollViewer;
            busyLoading = true;
            try
            {
                while (scrollViewer.ScrollableHeight == 0)
                    try
                    {
                        var res = await (ItemsSource as ISupportIncrementalLoading)?.LoadMoreItemsAsync(0);
                        if (res.Count == 0) return;
                    }
                    catch (InvalidOperationException)
                    {
                        return;
                    }
            }
            finally
            {
                busyLoading = false;
            }
        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (busyLoading) return;
            busyLoading = true;
            try
            {
                while ((sender as ScrollViewer).VerticalOffset >= (sender as ScrollViewer).ScrollableHeight - 500)
                {
                    try
                    {
                        await (ItemsSource as ISupportIncrementalLoading)?.LoadMoreItemsAsync(0);
                    }
                    catch (InvalidOperationException)
                    {
                        return;
                    }
                    catch { }
                }
            }
            finally
            {
                busyLoading = false;
            }
        }
    }
}
