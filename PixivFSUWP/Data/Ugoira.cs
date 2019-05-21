using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;

namespace PixivFSUWP.Data
{
    public class Ugoira : IDisposable
    {
        public struct Frame
        {
            public SoftwareBitmap Image;
            public int Delay;
        }

        public List<Frame> Frames { get; } = new List<Frame>();

        public void Dispose()
        {
            foreach(var i in Frames)
            {
                i.Image.Dispose();
            }
            Frames.Clear();
        }
    }
}
