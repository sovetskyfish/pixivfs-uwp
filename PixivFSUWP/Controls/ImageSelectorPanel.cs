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
    public class ImageSelectorPanel : Panel
    {
        //此属性决定项目间隔
        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register("ItemMargin", typeof(double),
                typeof(ImageSelectorPanel), new PropertyMetadata((double)0,
                    (DepObj, e) =>
                    {
                        (DepObj as ImageSelectorPanel).InvalidateMeasure();
                        (DepObj as ImageSelectorPanel).InvalidateArrange();
                    }));

        public double ItemMargin
        {
            get => (double)GetValue(ItemMarginProperty);
            set => SetValue(ItemMarginProperty, value);
        }

        //此属性决定左端间距
        public static readonly DependencyProperty LeftOffsetProperty =
            DependencyProperty.Register("LeftOffset", typeof(double),
                typeof(ImageSelectorPanel), new PropertyMetadata((double)0,
                    (DepObj, e) =>
                    {
                        (DepObj as ImageSelectorPanel).InvalidateMeasure();
                        (DepObj as ImageSelectorPanel).InvalidateArrange();
                    }));

        public double LeftOffset
        {
            get => (double)GetValue(LeftOffsetProperty);
            set => SetValue(LeftOffsetProperty, value);
        }

        //此属性决定右端间距
        public static readonly DependencyProperty RightOffsetProperty =
            DependencyProperty.Register("RightOffset", typeof(double),
                typeof(ImageSelectorPanel), new PropertyMetadata((double)0,
                    (DepObj, e) =>
                    {
                        (DepObj as ImageSelectorPanel).InvalidateMeasure();
                        (DepObj as ImageSelectorPanel).InvalidateArrange();
                    }));

        public double RightOffset
        {
            get => (double)GetValue(RightOffsetProperty);
            set => SetValue(RightOffsetProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size toret = new Size();
            toret.Height = availableSize.Height;
            toret.Width = LeftOffset + ItemMargin * (Children.Count - 1) + RightOffset;
            foreach (var i in Children)
            {
                i.Measure(new Size(double.PositiveInfinity, toret.Height));
                toret.Width += i.DesiredSize.Width;
            }
            return toret;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double xOffset = LeftOffset;
            foreach (var i in Children)
            {
                i.Arrange(new Rect(xOffset, 0, i.DesiredSize.Width, i.DesiredSize.Height));
                xOffset += i.DesiredSize.Width + ItemMargin;
            }
            return finalSize;
        }
    }
}
