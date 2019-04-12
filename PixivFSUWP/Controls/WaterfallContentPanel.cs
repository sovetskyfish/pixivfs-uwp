using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PixivFSUWP.Controls
{
    public class WaterfallContentPanel : Panel
    {
        //此属性决定瀑布流列数
        public static readonly DependencyProperty ColumsProperty =
            DependencyProperty.Register("Colums", typeof(int),
                typeof(WaterfallContentPanel), new PropertyMetadata(2,
                    (DepObj, e) =>
                    {
                        (DepObj as WaterfallContentPanel).InvalidateMeasure();
                        (DepObj as WaterfallContentPanel).InvalidateArrange();
                    }));

        public int Colums
        {
            get => (int)GetValue(ColumsProperty);
            set => SetValue(ColumsProperty, value);
        }

        //此属性决定项目间隔
        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register("ItemMargin", typeof(double),
                typeof(WaterfallContentPanel), new PropertyMetadata(0,
                    (DepObj, e) =>
                    {
                        (DepObj as WaterfallContentPanel).InvalidateMeasure();
                        (DepObj as WaterfallContentPanel).InvalidateArrange();
                    }));

        public double ItemMargin
        {
            get => (double)GetValue(ItemMarginProperty);
            set => SetValue(ItemMarginProperty, value);
        }

        //测量panel需要的空间
        //宽度填满，高度进行计算
        protected override Size MeasureOverride(Size availableSize)
        {
            Size toret = new Size();
            List<double> heights = new List<double>(Colums);
            toret.Width = availableSize.Width;
            double itemwidth = (availableSize.Width - ItemMargin * (Colums - 1)) / Colums;
            foreach (var i in Children)
            {
                i.Measure(new Size(itemwidth, double.PositiveInfinity));
                heights[heights.IndexOf(heights.Min())] += ItemMargin + i.DesiredSize.Height;
            }
            toret.Height = heights.Max();
            return toret;
        }

        //排版，不改变大小
        protected override Size ArrangeOverride(Size finalSize)
        {
            List<double> Xs = new List<double>();
            List<double> Ys = new List<double>();
            for (int i = 0; i < Colums; i++)
            {
                Xs.Add(i * (DesiredSize.Width + ItemMargin) / Colums);
                Ys.Add(0);
            }
            foreach (var i in Children)
            {
                var minC = Xs.IndexOf(Xs.Min());
                i.Arrange(new Rect(Xs[minC], Ys[minC], i.DesiredSize.Width, i.DesiredSize.Height));
                Ys[minC] += i.DesiredSize.Height + ItemMargin;
            }
            return finalSize;
        }
    }
}
