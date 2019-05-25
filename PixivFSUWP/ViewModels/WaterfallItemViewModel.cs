using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixivFSUWP.Data;
using Windows.UI.Xaml.Media.Imaging;
using System.IO;
using System.ComponentModel;

namespace PixivFSUWP.ViewModels
{
    public class WaterfallItemViewModel : INotifyPropertyChanged
    {
        public int ItemId { get; private set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ImageUri { get; set; }

        private int _stars;
        public int Stars
        {
            get => _stars;
            set
            {
                _stars = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Stars)));
            }
        }

        public int Pages { get; set; }

        private bool _isbookmarked;
        public bool IsBookmarked
        {
            get => _isbookmarked;
            set
            {
                _isbookmarked = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBookmarked)));
            }
        }

        public BitmapImage ImageSource { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task LoadImageAsync()
        {
            ImageSource = await Data.OverAll.LoadImageAsync(ImageUri);
        }

        public string StarsString
        {
            get
            {
                if (IsBookmarked) return "★√" + Stars.ToString();
                return "★" + Stars.ToString();
            }
        }

        public static WaterfallItemViewModel FromItem(WaterfallItem Item)
            => new WaterfallItemViewModel()
            {
                ItemId = Item.Id,
                Title = Item.Title,
                Author = Item.Author,
                ImageUri = Item.ImageUri,
                IsBookmarked = Item.IsBookmarked,
                Stars = Item.Stars,
                Pages = Item.Pages,
                Width = Item.Width,
                Height = Item.Height
            };
    }
}
