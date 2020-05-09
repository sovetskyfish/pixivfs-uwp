using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace PixivFSUWP.Data
{
    public class BigImageDetail
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Image { get; set; }
        public StorageFile ImageCache { get; set; }
    }
}
