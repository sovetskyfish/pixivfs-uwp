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
        public static WaterfallItem FromJsonValue(JsonValue Source)
        {
            var toret = new WaterfallItem();
            toret.Id = Source.Item("id").AsInteger();
            toret.Title = Source.Item("title").AsString();
            toret.Author = Source.Item("user").Item("name").AsString();
            toret.ImageUri = Source.Item("image_urls").Item("medium").AsString();
            toret.Stars = Source.Item("total_bookmarks").AsInteger();
            toret.Pages = Source.Item("page_count").AsInteger();
            toret.IsBookmarked = Source.Item("is_bookmarked").AsBoolean();
            return toret;
        }
    }
}
