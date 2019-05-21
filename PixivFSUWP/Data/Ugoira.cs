using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace PixivFSUWP.Data
{
    public class Ugoira : IDisposable
    {
        public struct Frame
        {
            public BitmapImage Image;
            public int Delay;
        }

        public List<Frame> Frames { get; } = new List<Frame>();

        public void Dispose()
        {
            Frames.Clear();
        }
    }
}
