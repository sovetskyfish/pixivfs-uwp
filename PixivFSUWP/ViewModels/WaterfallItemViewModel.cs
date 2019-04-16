using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixivFSUWP.Data;
using Windows.UI.Xaml.Media.Imaging;
using PixivFSCS;
using PixivFS;
using System.IO;

namespace PixivFSUWP.ViewModels
{
    public class WaterfallItemViewModel
    {
        public int ItemId { get; private set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ImageUri { get; set; }
        public int Stars { get; set; }
        public int Pages { get; set; }
        public bool IsBookmarked { get; set; }
        public BitmapImage ImageSource { get; set; }

        public async Task LoadImageAsync()
        {
            ImageSource = new BitmapImage();
            var resStream = new PixivAppAPI(OverAll.GlobalBaseAPI).csfriendly_no_auth_requests_call_stream("GET",
                ImageUri, new List<Tuple<string, string>>() { ("Referer", "https://app-api.pixiv.net/").ToTuple() })
                .ResponseStream;
            var memStream = new MemoryStream();
            await resStream.CopyToAsync(memStream);
            memStream.Position = 0;
            await ImageSource.SetSourceAsync(memStream.AsRandomAccessStream());
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
                Pages = Item.Pages
            };
    }
}
