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
