using FSharp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivFSUWP.Data
{
    public class WaterfallItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ImageUri { get; set; }
        public int Stars { get; set; }
        public int Pages { get; set; }
        public bool IsBookmarked { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public static WaterfallItem FromJsonValue(JsonValue Source)
        {
            var toret = new WaterfallItem();
            toret.Id = Source.TryGetProperty("id").Value.AsInteger();
            toret.Title = Source.TryGetProperty("title").Value.AsString();
            toret.Author = Source.TryGetProperty("user").Value.TryGetProperty("name").Value.AsString();
            toret.ImageUri = Source.TryGetProperty("image_urls").Value.TryGetProperty("medium").Value.AsString();
            toret.Stars = Source.TryGetProperty("total_bookmarks").Value.AsInteger();
            toret.Pages = Source.TryGetProperty("page_count").Value.AsInteger();
            toret.IsBookmarked = Source.TryGetProperty("is_bookmarked").Value.AsBoolean();
            toret.Width = Source.TryGetProperty("width").Value.AsInteger();
            toret.Height = Source.TryGetProperty("height").Value.AsInteger();
            return toret;
        }
    }
}
