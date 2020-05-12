using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
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
            toret.ID = (int)user["id"].GetNumber();
            toret.Name = user["name"].TryGetString();
            toret.Account = user["account"].TryGetString();
            toret.AvatarUrl = user["profile_image_urls"].GetObject()["medium"].TryGetString();
            toret.Comment = user["comment"].TryGetString();
            toret.IsFollowed = user["is_followed"].GetBoolean();
            var profile = Source["profile"].GetObject();
            toret.WebPage = profile["webpage"].TryGetString();
            toret.Gender = profile["gender"].TryGetString();
            toret.Birth = profile["birth"].TryGetString();
            toret.BirthDay = profile["birth_day"].TryGetString();
            toret.BirthYear = (int)profile["birth_year"].GetNumber();
            toret.Region = profile["region"].TryGetString();
            toret.AddressID = (int)profile["address_id"].GetNumber();
            toret.CountryCode = profile["country_code"].TryGetString();
            toret.Job = profile["job"].TryGetString();
            toret.JobID = (int)profile["job_id"].GetNumber();
            toret.TotalFollowUsers = (int)profile["total_follow_users"].GetNumber();
            toret.TotalMyPixivUsers = (int)profile["total_mypixiv_users"].GetNumber();
            toret.TotalIllusts = (int)profile["total_illusts"].GetNumber();
            toret.TotalManga = (int)profile["total_manga"].GetNumber();
            toret.TotalNovels = (int)profile["total_novels"].GetNumber();
            toret.TotalIllustBookmarksPublic = (int)profile["total_illust_bookmarks_public"].GetNumber();
            toret.TotalIllustSeries = (int)profile["total_illust_series"].GetNumber();
            toret.TotalNovelSeries = (int)profile["total_novel_series"].GetNumber();
            toret.BackgroundImage = profile["background_image_url"].TryGetString();
            toret.TwitterAccount = profile["twitter_account"].TryGetString();
            toret.TwitterUrl = profile["twitter_url"].TryGetString();
            toret.PawooUrl = profile["pawoo_url"].TryGetString();
            toret.IsPremium = profile["is_premium"].GetBoolean();
            toret.IsUsingCustomProfileImage = profile["is_using_custom_profile_image"].GetBoolean();
            var profile_publicity = Source["profile_publicity"].GetObject();
            toret.GenderPublicity = profile_publicity["gender"].TryGetString();
            toret.RegionPublicity = profile_publicity["region"].TryGetString();
            toret.BirthDayPublicity = profile_publicity["birth_day"].TryGetString();
            toret.BirthYearPublicity = profile_publicity["birth_year"].TryGetString();
            toret.JobPublicity = profile_publicity["job"].TryGetString();
            toret.Pawoo = profile_publicity["pawoo"].GetBoolean();
            var workspace = Source["workspace"].GetObject();
            toret.PC = workspace["pc"].TryGetString();
            toret.Monitor = workspace["monitor"].TryGetString();
            toret.Tool = workspace["tool"].TryGetString();
            toret.Scanner = workspace["scanner"].TryGetString();
            toret.Tablet = workspace["tablet"].TryGetString();
            toret.Mouse = workspace["mouse"].TryGetString();
            toret.Printer = workspace["printer"].TryGetString();
            toret.Desktop = workspace["desktop"].TryGetString();
            toret.Music = workspace["music"].TryGetString();
            toret.Desk = workspace["desk"].TryGetString();
            toret.Chair = workspace["chair"].TryGetString();
            toret.WorkspaceComment = workspace["comment"].TryGetString();
            toret.WorkspaceImageUrl = workspace["workspace_image_url"].TryGetString();
            return toret;
        }

        public static UserDetail FromObject(PixivCS.Objects.UserDetail Source)
        {
            UserDetail toret = new UserDetail();
            var user = Source.User;
            toret.ID = (int)user.Id;
            toret.Name = user.Name;
            toret.Account = user.Account;
            toret.AvatarUrl = user.ProfileImageUrls.Medium?.ToString() ?? "";
            toret.Comment = user.Comment;
            toret.IsFollowed = user.IsFollowed.HasValue ? user.IsFollowed.Value : false;
            var profile = Source.Profile;
            toret.WebPage = profile.Webpage?.ToString() ?? "";
            toret.Gender = profile.Gender;
            toret.Birth = profile.Birth;
            toret.BirthDay = profile.BirthDay;
            toret.BirthYear = (int)profile.BirthYear;
            toret.Region = profile.Region;
            toret.AddressID = (int)profile.AddressId;
            toret.CountryCode = profile.CountryCode;
            toret.Job = profile.Job;
            toret.JobID = (int)profile.JobId;
            toret.TotalFollowUsers = (int)profile.TotalFollowUsers;
            toret.TotalMyPixivUsers = (int)profile.TotalMypixivUsers;
            toret.TotalIllusts = (int)profile.TotalIllusts;
            toret.TotalManga = (int)profile.TotalManga;
            toret.TotalNovels = (int)profile.TotalNovels;
            toret.TotalIllustBookmarksPublic = (int)profile.TotalIllustBookmarksPublic;
            toret.TotalIllustSeries = (int)profile.TotalIllustSeries;
            toret.TotalNovelSeries = (int)profile.TotalNovelSeries;
            toret.BackgroundImage = profile.BackgroundImageUrl?.ToString() ?? "";
            toret.TwitterAccount = profile.TwitterAccount;
            toret.TwitterUrl = profile.TwitterUrl?.ToString() ?? "";
            toret.PawooUrl = profile.PawooUrl?.ToString() ?? "";
            toret.IsPremium = profile.IsPremium;
            toret.IsUsingCustomProfileImage = profile.IsUsingCustomProfileImage;
            var profile_publicity = Source.ProfilePublicity;
            toret.GenderPublicity = profile_publicity.Gender;
            toret.RegionPublicity = profile_publicity.Region;
            toret.BirthDayPublicity = profile_publicity.BirthDay;
            toret.BirthYearPublicity = profile_publicity.BirthYear;
            toret.JobPublicity = profile_publicity.Job;
            toret.Pawoo = profile_publicity.Pawoo;
            var workspace = Source.Workspace;
            toret.PC = workspace.Pc;
            toret.Monitor = workspace.Monitor;
            toret.Tool = workspace.Tool;
            toret.Scanner = workspace.Scanner;
            toret.Tablet = workspace.Tablet;
            toret.Mouse = workspace.Mouse;
            toret.Printer = workspace.Printer;
            toret.Desktop = workspace.Desktop;
            toret.Music = workspace.Music;
            toret.Desk = workspace.Desk;
            toret.Chair = workspace.Chair;
            toret.WorkspaceComment = workspace.Comment;
            toret.WorkspaceImageUrl = workspace.WorkspaceImageUrl?.ToString() ?? "";
            return toret;
        }
    }
}
