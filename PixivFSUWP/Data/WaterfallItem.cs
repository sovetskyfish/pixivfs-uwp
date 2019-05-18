using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

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
        public static WaterfallItem FromJsonValue(JsonObject Source)
        {
            var toret = new WaterfallItem();
            toret.Id = Convert.ToInt32(Source["id"].GetString());
            toret.Title = Source["title"].GetString();
            toret.Author = Source["user"].GetObject()["name"].GetString();
            toret.ImageUri = Source["image_urls"].GetObject()["medium"].GetString();
            toret.Stars = Convert.ToInt32(Source["total_bookmarks"].GetString());
            toret.Pages = Convert.ToInt32(Source["page_count"].GetString());
            toret.IsBookmarked = Source["is_bookmarked"].GetBoolean();
            toret.Width = Convert.ToInt32(Source["width"].GetString());
            toret.Height = Convert.ToInt32(Source["height"].GetString());
            return toret;
        }
    }
}
