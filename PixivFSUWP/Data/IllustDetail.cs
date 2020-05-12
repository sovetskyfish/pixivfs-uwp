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

        public static IllustDetail FromObject(PixivCS.Objects.IllustDetail Source)
        {
            IllustDetail toret = new IllustDetail();
            toret.IllustID = (int)Source.Illust.Id;
            toret.Title = Source.Illust.Title;
            toret.Type = Source.Illust.Type;
            toret.Caption = Source.Illust.Caption;
            toret.AuthorID = (int)Source.Illust.User.Id;
            toret.Author = Source.Illust.User.Name;
            toret.AuthorAccount = Source.Illust.User.Account;
            toret.AuthorAvatarUrl = Source.Illust.User.ProfileImageUrls.Medium?.ToString() ?? "";
            toret.IsUserFollowed = Source.Illust.User.IsFollowed.HasValue ? Source.Illust.User.IsFollowed.Value : false;
            var tags = Source.Illust.Tags;
            toret.Tags = new List<string>();
            foreach (var tag in tags)
                toret.Tags.Add(tag.Name);
            var tools = Source.Illust.Tools;
            toret.Tools = new List<string>();
            foreach (var tool in tools)
                toret.Tools.Add(tool);
            toret.CreateDate = Source.Illust.CreateDate;
            toret.MediumUrl = Source.Illust.ImageUrls.SquareMedium?.ToString() ?? "";
            var pgCount = (int)Source.Illust.PageCount;
            toret.OriginalUrls = new List<string>();
            if (pgCount == 1) toret.OriginalUrls.Add(Source.Illust.MetaSinglePage.OriginalImageUrl?.ToString());
            else
            {
                var pages = Source.Illust.MetaPages;
                foreach (var page in pages)
                    toret.OriginalUrls.Add(page.ImageUrls.Original?.ToString());
            }
            toret.Width = (int)Source.Illust.Width;
            toret.Height = (int)Source.Illust.Height;
            toret.SanityLevel = (int)Source.Illust.SanityLevel;
            toret.TotalView = (int)Source.Illust.TotalView;
            toret.TotalBookmarks = (int)Source.Illust.TotalBookmarks;
            toret.IsBookmarked = Source.Illust.IsBookmarked;
            toret.TotalComments = (int)Source.Illust.TotalComments;
            return toret;
        }
    }
}
