using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

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

        public static UserDetail FromJsomValue(JsonObject Source)
        {
            UserDetail toret = new UserDetail();
            var user = Source["user"].GetObject();
            toret.ID = Convert.ToInt32(user["id"].GetString());
            toret.Name = user["name"].GetString();
            toret.Account = user["account"].GetString();
            toret.AvatarUrl = user["profile_image_urls"].GetObject()["medium"].GetString();
            toret.Comment = user["comment"].GetString();
            toret.IsFollowed = user["is_followed"].GetBoolean();
            var profile = Source["profile"].GetObject();
            toret.WebPage = profile["webpage"].GetString();
            toret.Gender = profile["gender"].GetString();
            toret.Birth = profile["birth"].GetString();
            toret.BirthDay = profile["birth_day"].GetString();
            toret.BirthYear = Convert.ToInt32(profile["birth_year"].GetString());
            toret.Region = profile["region"].GetString();
            toret.AddressID = Convert.ToInt32(profile["address_id"].GetString());
            toret.CountryCode = profile["country_code"].GetString();
            toret.Job = profile["job"].GetString();
            toret.JobID = Convert.ToInt32(profile["job_id"].GetString());
            toret.TotalFollowUsers = Convert.ToInt32(profile["total_follow_users"].GetString());
            toret.TotalMyPixivUsers = Convert.ToInt32(profile["total_mypixiv_users"].GetString());
            toret.TotalIllusts = Convert.ToInt32(profile["total_illusts"].GetString());
            toret.TotalManga = Convert.ToInt32(profile["total_manga"].GetString());
            toret.TotalNovels = Convert.ToInt32(profile["total_novels"].GetString());
            toret.TotalIllustBookmarksPublic = Convert.ToInt32(profile["total_illust_bookmarks_public"].GetString());
            toret.TotalIllustSeries = Convert.ToInt32(profile["total_illust_series"].GetString());
            toret.TotalNovelSeries = Convert.ToInt32(profile["total_novel_series"].GetString());
            toret.BackgroundImage = profile["background_image_url"].GetString();
            toret.TwitterAccount = profile["twitter_account"].GetString();
            toret.TwitterUrl = profile["twitter_url"].GetString();
            toret.PawooUrl = profile["pawoo_url"].GetString();
            toret.IsPremium = profile["is_premium"].GetBoolean();
            toret.IsUsingCustomProfileImage = profile["is_using_custom_profile_image"].GetBoolean();
            var profile_publicity = Source["profile_publicity"].GetObject();
            toret.GenderPublicity = profile_publicity["gender"].GetString();
            toret.RegionPublicity = profile_publicity["region"].GetString();
            toret.BirthDayPublicity = profile_publicity["birth_day"].GetString();
            toret.BirthYearPublicity = profile_publicity["birth_year"].GetString();
            toret.JobPublicity = profile_publicity["job"].GetString();
            toret.Pawoo = profile_publicity["pawoo"].GetBoolean();
            var workspace = Source["workspace"].GetObject();
            toret.PC = workspace["pc"].GetString();
            toret.Monitor = workspace["monitor"].GetString();
            toret.Tool = workspace["tool"].GetString();
            toret.Scanner = workspace["scanner"].GetString();
            toret.Tablet = workspace["tablet"].GetString();
            toret.Mouse = workspace["mouse"].GetString();
            toret.Printer = workspace["printer"].GetString();
            toret.Desktop = workspace["desktop"].GetString();
            toret.Music = workspace["music"].GetString();
            toret.Desk = workspace["desk"].GetString();
            toret.Chair = workspace["chair"].GetString();
            toret.WorkspaceComment = workspace["comment"].GetString();
            toret.WorkspaceImageUrl = workspace["workspace_image_url"].GetString();
            return toret;
        }
    }
}
