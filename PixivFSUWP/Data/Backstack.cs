using PixivFSUWP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace PixivFSUWP.Data
{
    public class Backstack : Stack<(Type, object)>
    {
        public static Backstack Default { get; } = new Backstack();

        public bool CanBack
        {
            get => base.Count > 0;
        }

        public void Push(Type Page, object Parameter)
        {
            base.Push((Page, Parameter));
        }

        public bool Back(Frame Frame)
        {
            if (!CanBack) throw new InvalidOperationException();
            (var page, var param) = base.Pop();
            return Frame.Navigate(page, param);
        }
    }

    public static class FrameBackstackExtended
    {
        public static bool Back(this Frame source)
        {
            (source.Content as IGoBackFlag).SetBackFlag(true);
            return Backstack.Default.Back(source);
        }
    }
}
