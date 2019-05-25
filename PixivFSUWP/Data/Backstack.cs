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
        public static Backstack Default;

        public bool CanBack
        {
            get => base.Count > 0;
        }

        public void Push(Type Page, object Parameter)
        {
            base.Push((Page, Parameter));
        }

        public void Back(Frame Frame)
        {
            if (!CanBack) throw new InvalidOperationException();
            (var page, var param) = base.Pop();
            Frame.Navigate(page, param);
        }
    }

    public static class FrameBackstackExtended
    {
        public static void Back(this Frame source) => Backstack.Default.Back(source);
    }
}
