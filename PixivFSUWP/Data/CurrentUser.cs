
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace PixivFSUWP.Data
{
    public class CurrentUser
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string UserAccount { get; set; }
        public string Email { get; set; }
        public bool IsMailAuthorized { get; set; }
        public bool IsPremium { get; set; }
        public string Avatar16 { get; set; }
        public string Avatar50 { get; set; }
        public string Avatar170 { get; set; }

        public static CurrentUser FromJsonValue(JsonObject Source)
        {
            CurrentUser toret = new CurrentUser();
            toret.ID = Convert.ToInt32(Source["id"].TryGetString());
            toret.Username = Source["name"].TryGetString();
            toret.UserAccount = Source["account"].TryGetString();
            toret.Email = Source["mail_address"].TryGetString();
            toret.IsMailAuthorized = Source["is_mail_authorized"].GetBoolean();
            toret.IsPremium = Source["is_premium"].GetBoolean();
            toret.Avatar16 = Source["profile_image_urls"].GetObject()["px_16x16"].TryGetString();
            toret.Avatar50 = Source["profile_image_urls"].GetObject()["px_50x50"].TryGetString();
            toret.Avatar170 = Source["profile_image_urls"].GetObject()["px_170x170"].TryGetString();
            return toret;
        }

        public static CurrentUser FromObject(PixivCS.Objects.ResponseUser Source)
        {
            CurrentUser toret = new CurrentUser();
            toret.ID = Convert.ToInt32(Source.Id);
            toret.Username = Source.Name;
            toret.UserAccount = Source.Account;
            toret.Email = Source.MailAddress;
            toret.IsMailAuthorized = Source.IsMailAuthorized;
            toret.IsPremium = Source.IsPremium;
            toret.Avatar16 = Source.ProfileImageUrls.Px16X16?.ToString();
            toret.Avatar50 = Source.ProfileImageUrls.Px50X50?.ToString();
            toret.Avatar170 = Source.ProfileImageUrls.Px170X170?.ToString();
            return toret;
        }
    }
}
