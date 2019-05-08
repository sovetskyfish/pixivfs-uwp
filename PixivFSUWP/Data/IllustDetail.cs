using FSharp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivFSUWP.Data
{
    public class IllustDetail
    {
        public int IllustID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Caption { get; set; }
        public int AuthorID { get; set; }
        public string Author { get; set; }
        public string AuthorAccount { get; set; }
        public string AuthorAvatarUrl { get; set; }
        public bool IsUserFollowed { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Tools { get; set; }
        public string CreateDate { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int SanityLevel { get; set; }
        public string MediumUrl { get; set; }
        public List<string> OriginalUrls { get; set; }
        public int TotalView { get; set; }
        public int TotalBookmarks { get; set; }
        public int TotalComments { get; set; }
        public bool IsBookmarked { get; set; }

        public static IllustDetail FromJsonValue(JsonValue Source)
        {
            IllustDetail toret = new IllustDetail();
            toret.IllustID = Source.TryGetProperty("illust").Value.TryGetProperty("id").Value.AsInteger();
            toret.Title = Source.TryGetProperty("illust").Value.TryGetProperty("title").Value.AsString();
            toret.Type = Source.TryGetProperty("illust").Value.TryGetProperty("type").Value.AsString();
            toret.Caption = Source.TryGetProperty("illust").Value.TryGetProperty("caption").Value.AsString();
            toret.AuthorID = Source.TryGetProperty("illust").Value.TryGetProperty("user").Value.TryGetProperty("id").Value.AsInteger();
            toret.Author = Source.TryGetProperty("illust").Value.TryGetProperty("user").Value.TryGetProperty("name").Value.AsString();
            toret.AuthorAccount = Source.TryGetProperty("illust").Value.TryGetProperty("user").Value.TryGetProperty("account").Value.AsString();
            toret.AuthorAvatarUrl = Source.TryGetProperty("illust").Value.TryGetProperty("user").Value.TryGetProperty("profile_image_urls").Value.TryGetProperty("medium").Value.AsString();
            toret.IsUserFollowed = Source.TryGetProperty("illust").Value.TryGetProperty("user").Value.TryGetProperty("is_followed").Value.AsBoolean();
            var tags = Source.TryGetProperty("illust").Value.TryGetProperty("tags").Value.AsArray();
            toret.Tags = new List<string>();
            foreach (var tag in tags)
                toret.Tags.Add(tag.TryGetProperty("name").Value.AsString());
            var tools = Source.TryGetProperty("illust").Value.TryGetProperty("tools").Value.AsArray();
            toret.Tools = new List<string>();
            foreach (var tool in tools)
                toret.Tools.Add(tool.AsString());
            toret.CreateDate = Source.TryGetProperty("illust").Value.TryGetProperty("create_date").Value.AsString();
            toret.MediumUrl = Source.TryGetProperty("illust").Value.TryGetProperty("image_urls").Value.TryGetProperty("square_medium").Value.AsString();
            var pgCount = Source.TryGetProperty("illust").Value.TryGetProperty("page_count").Value.AsInteger();
            toret.OriginalUrls = new List<string>();
            if (pgCount == 1) toret.OriginalUrls.Add(Source.TryGetProperty("illust").Value.TryGetProperty("meta_single_page").Value.TryGetProperty("original_image_url").Value.AsString());
            else
            {
                var pages = Source.TryGetProperty("illust").Value.TryGetProperty("meta_pages").Value.AsArray();
                foreach (var page in pages)
                    toret.OriginalUrls.Add(page.TryGetProperty("image_urls").Value.TryGetProperty("original").Value.AsString());
            }
            toret.Width = Source.TryGetProperty("illust").Value.TryGetProperty("width").Value.AsInteger();
            toret.Height = Source.TryGetProperty("illust").Value.TryGetProperty("height").Value.AsInteger();
            toret.SanityLevel = Source.TryGetProperty("illust").Value.TryGetProperty("sanity_level").Value.AsInteger();
            toret.TotalView = Source.TryGetProperty("illust").Value.TryGetProperty("total_view").Value.AsInteger();
            toret.TotalBookmarks = Source.TryGetProperty("illust").Value.TryGetProperty("total_bookmarks").Value.AsInteger();
            toret.IsBookmarked = Source.TryGetProperty("illust").Value.TryGetProperty("is_bookmarked").Value.AsBoolean();
            toret.TotalComments = Source.TryGetProperty("illust").Value.TryGetProperty("total_comments").Value.AsInteger();
            return toret;
        }
    }
}
