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
        public string BirthYear { get; set; }
        public string Region { get; set; }
        public int AddressID { get; set; }
        public int CountryCode { get; set; }
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
    }
}
