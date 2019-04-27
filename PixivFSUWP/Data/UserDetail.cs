using FSharp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivFSUWP.Data
{
    public class UserDetail
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string AvatarUrl { get; set; }
        public string Comment { get; set; }
        public bool IsFollowed { get; set; }
        public string WebPage { get; set; }
        public string Gender { get; set; }
        public string Birth { get; set; }
        public string BirthDay { get; set; }
        public int BirthYear { get; set; }
        public string Region { get; set; }
        public int AddressID { get; set; }
        public string CountryCode { get; set; }
        public string Job { get; set; }
        public int JobID { get; set; }
        public int TotalFollowUsers { get; set; }
        public int TotalMyPixivUsers { get; set; }
        public int TotalIllusts { get; set; }
        public int TotalManga { get; set; }
        public int TotalNovels { get; set; }
        public int TotalIllustBookmarksPublic { get; set; }
        public int TotalIllustSeries { get; set; }
        public int TotalNovelSeries { get; set; }
        public string BackgroundImage { get; set; }
        public string TwitterAccount { get; set; }
        public string TwitterUrl { get; set; }
        public string PawooUrl { get; set; }
        public bool IsPremium { get; set; }
        public bool IsUsingCustomProfileImage { get; set; }
        public string GenderPublicity { get; set; }
        public string RegionPublicity { get; set; }
        public string BirthDayPublicity { get; set; }
        public string BirthYearPublicity { get; set; }
        public string JobPublicity { get; set; }
        public bool Pawoo { get; set; }
        public string PC { get; set; }
        public string Monitor { get; set; }
        public string Tool { get; set; }
        public string Scanner { get; set; }
        public string Tablet { get; set; }
        public string Mouse { get; set; }
        public string Printer { get; set; }
        public string Desktop { get; set; }
        public string Music { get; set; }
        public string Desk { get; set; }
        public string Chair { get; set; }
        public string WorkspaceComment { get; set; }
        public string WorkspaceImageUrl { get; set; }

        public static UserDetail FromJsomValue(JsonValue Source)
        {
            UserDetail toret = new UserDetail();
            var user = Source.TryGetProperty("user").Value;
            toret.ID = user.TryGetProperty("id").Value.AsInteger();
            toret.Name = user.TryGetProperty("name").Value.AsString();
            toret.Account = user.TryGetProperty("account").Value.AsString();
            toret.AvatarUrl = user.TryGetProperty("profile_image_urls").Value.TryGetProperty("medium").Value.AsString();
            toret.Comment = user.TryGetProperty("comment").Value.AsString();
            toret.IsFollowed = user.TryGetProperty("is_followed").Value.AsBoolean();
            var profile = Source.TryGetProperty("profile").Value;
            toret.WebPage = profile.TryGetProperty("webpage").Value.AsString();
            toret.Gender = profile.TryGetProperty("gender").Value.AsString();
            toret.Birth = profile.TryGetProperty("birth").Value.AsString();
            toret.BirthDay = profile.TryGetProperty("birth_day").Value.AsString();
            toret.BirthYear = profile.TryGetProperty("birth_year").Value.AsInteger();
            toret.Region = profile.TryGetProperty("region").Value.AsString();
            toret.AddressID = profile.TryGetProperty("address_id").Value.AsInteger();
            toret.CountryCode = profile.TryGetProperty("country_code").Value.AsString();
            toret.Job = profile.TryGetProperty("job").Value.AsString();
            toret.JobID = profile.TryGetProperty("job_id").Value.AsInteger();
            toret.TotalFollowUsers = profile.TryGetProperty("total_follow_users").Value.AsInteger();
            toret.TotalMyPixivUsers = profile.TryGetProperty("total_mypixiv_users").Value.AsInteger();
            toret.TotalIllusts = profile.TryGetProperty("total_illusts").Value.AsInteger();
            toret.TotalManga = profile.TryGetProperty("total_manga").Value.AsInteger();
            toret.TotalNovels = profile.TryGetProperty("total_novels").Value.AsInteger();
            toret.TotalIllustBookmarksPublic = profile.TryGetProperty("total_illust_bookmarks_public").Value.AsInteger();
            toret.TotalIllustSeries = profile.TryGetProperty("total_illust_series").Value.AsInteger();
            toret.TotalNovelSeries = profile.TryGetProperty("total_novel_series").Value.AsInteger();
            toret.BackgroundImage = profile.TryGetProperty("background_image_url").Value.AsString();
            toret.TwitterAccount = profile.TryGetProperty("twitter_account").Value.AsString();
            toret.TwitterUrl = profile.TryGetProperty("twitter_url").Value.AsString();
            toret.PawooUrl = profile.TryGetProperty("pawoo_url").Value.AsString();
            toret.IsPremium = profile.TryGetProperty("is_premium").Value.AsBoolean();
            toret.IsUsingCustomProfileImage = profile.TryGetProperty("is_using_custom_profile_image").Value.AsBoolean();
            var profile_publicity = Source.TryGetProperty("profile_publicity").Value;
            toret.GenderPublicity = profile_publicity.TryGetProperty("gender").Value.AsString();
            toret.RegionPublicity = profile_publicity.TryGetProperty("region").Value.AsString();
            toret.BirthDayPublicity = profile_publicity.TryGetProperty("birth_day").Value.AsString();
            toret.BirthYearPublicity = profile_publicity.TryGetProperty("birth_year").Value.AsString();
            toret.JobPublicity = profile_publicity.TryGetProperty("job").Value.AsString();
            toret.Pawoo = profile_publicity.TryGetProperty("pawoo").Value.AsBoolean();
            var workspace = Source.TryGetProperty("workspace").Value;
            toret.PC = workspace.TryGetProperty("pc").Value.AsString();
            toret.Monitor = workspace.TryGetProperty("monitor").Value.AsString();
            toret.Tool = workspace.TryGetProperty("tool").Value.AsString();
            toret.Scanner = workspace.TryGetProperty("scanner").Value.AsString();
            toret.Tablet = workspace.TryGetProperty("tablet").Value.AsString();
            toret.Mouse = workspace.TryGetProperty("mouse").Value.AsString();
            toret.Printer = workspace.TryGetProperty("printer").Value.AsString();
            toret.Desktop = workspace.TryGetProperty("desktop").Value.AsString();
            toret.Music = workspace.TryGetProperty("music").Value.AsString();
            toret.Desk = workspace.TryGetProperty("desk").Value.AsString();
            toret.Chair = workspace.TryGetProperty("chair").Value.AsString();
            toret.WorkspaceComment = workspace.TryGetProperty("comment").Value.AsString();
            toret.WorkspaceImageUrl = workspace.TryGetProperty("workspace_image_url").Value.AsString();
            return toret;
        }
    }
}
