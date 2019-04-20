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
        public string AuthorAvatarUrl { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Tools { get; set; }
        public string CreateDate { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int SanityLevel { get; set; }
        public List<string> OriginalUrls { get; set; }
        public int TotalView { get; set; }
        public int TotalBookmarks { get; set; }
        public int TotalComments { get; set; }
        public bool IsBookmarked { get; set; }

        public static IllustDetail FromJsonValue(JsonValue Source)
        {
            IllustDetail toret = new IllustDetail();
            toret.IllustID = Source.Item("illust").Item("id").AsInteger();
            toret.Title = Source.Item("illust").Item("title").AsString();
            toret.Type = Source.Item("illust").Item("type").AsString();
            toret.Caption = Source.Item("illust").Item("caption").AsString();
            toret.AuthorID = Source.Item("illust").Item("user").Item("id").AsInteger();
            toret.Author = Source.Item("illust").Item("user").Item("name").AsString();
            toret.AuthorAvatarUrl = Source.Item("illust").Item("user").Item("profile_image_urls").Item("medium").AsString();
            var tags = Source.Item("illust").Item("tags").AsArray();
            toret.Tags = new List<string>();
            foreach (var tag in tags)
                toret.Tags.Add(tag.Item("name").AsString());
            var tools = Source.Item("illust").Item("tools").AsArray();
            toret.Tools = new List<string>();
            foreach (var tool in tools)
                toret.Tools.Add(tool.AsString());
            toret.CreateDate = Source.Item("illust").Item("create_date").AsString();
            var pgCount = Source.Item("illust").Item("page_count").AsInteger();
            toret.OriginalUrls = new List<string>();
            if (pgCount == 1) toret.OriginalUrls.Add(Source.Item("illust").Item("meta_single_page").Item("original_image_url").AsString());
            else
            {
                var pages = Source.Item("illust").Item("meta_pages").AsArray();
                foreach (var page in pages)
                    toret.OriginalUrls.Add(page.Item("image_urls").Item("original").AsString());
            }
            toret.Width = Source.Item("illust").Item("width").AsInteger();
            toret.Height = Source.Item("illust").Item("height").AsInteger();
            toret.SanityLevel = Source.Item("illust").Item("sanity_level").AsInteger();
            toret.TotalView = Source.Item("illust").Item("total_view").AsInteger();
            toret.TotalBookmarks = Source.Item("illust").Item("total_bookmarks").AsInteger();
            toret.IsBookmarked = Source.Item("illust").Item("is_bookmarked").AsBoolean();
            toret.TotalComments = Source.Item("illust").Item("total_comments").AsInteger();
            return toret;
        }
    }
}
