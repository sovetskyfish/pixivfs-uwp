using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

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

        public static IllustDetail FromJsonValue(JsonObject Source)
        {
            IllustDetail toret = new IllustDetail();
            toret.IllustID = Convert.ToInt32(Source["illust"].GetObject()["id"].GetString());
            toret.Title = Source["illust"].GetObject()["title"].GetString();
            toret.Type = Source["illust"].GetObject()["type"].GetString();
            toret.Caption = Source["illust"].GetObject()["caption"].GetString();
            toret.AuthorID = Convert.ToInt32(Source["illust"].GetObject()["user"].GetObject()["id"].GetString());
            toret.Author = Source["illust"].GetObject()["user"].GetObject()["name"].GetString();
            toret.AuthorAccount = Source["illust"].GetObject()["user"].GetObject()["account"].GetString();
            toret.AuthorAvatarUrl = Source["illust"].GetObject()["user"].GetObject()["profile_image_urls"].GetObject()["medium"].GetString();
            toret.IsUserFollowed = Source["illust"].GetObject()["user"].GetObject()["is_followed"].GetBoolean();
            var tags = Source["illust"].GetObject()["tags"].GetArray();
            toret.Tags = new List<string>();
            foreach (var tag in tags)
                toret.Tags.Add(tag.GetObject()["name"].GetString());
            var tools = Source["illust"].GetObject()["tools"].GetArray();
            toret.Tools = new List<string>();
            foreach (var tool in tools)
                toret.Tools.Add(tool.GetString());
            toret.CreateDate = Source["illust"].GetObject()["create_date"].GetString();
            toret.MediumUrl = Source["illust"].GetObject()["image_urls"].GetObject()["square_medium"].GetString();
            var pgCount = Convert.ToInt32(Source["illust"].GetObject()["page_count"].GetString());
            toret.OriginalUrls = new List<string>();
            if (pgCount == 1) toret.OriginalUrls.Add(Source["illust"].GetObject()["meta_single_page"].GetObject()["original_image_url"].GetString());
            else
            {
                var pages = Source["illust"].GetObject()["meta_pages"].GetArray();
                foreach (var page in pages)
                    toret.OriginalUrls.Add(page.GetObject()["image_urls"].GetObject()["original"].GetString());
            }
            toret.Width = Convert.ToInt32(Source["illust"].GetObject()["width"].GetString());
            toret.Height = Convert.ToInt32(Source["illust"].GetObject()["height"].GetString());
            toret.SanityLevel = Convert.ToInt32(Source["illust"].GetObject()["sanity_level"].GetString());
            toret.TotalView = Convert.ToInt32(Source["illust"].GetObject()["total_view"].GetString());
            toret.TotalBookmarks = Convert.ToInt32(Source["illust"].GetObject()["total_bookmarks"].GetString());
            toret.IsBookmarked = Source["illust"].GetObject()["is_bookmarked"].GetBoolean();
            toret.TotalComments = Convert.ToInt32(Source["illust"].GetObject()["total_comments"].GetString());
            return toret;
        }
    }
}
