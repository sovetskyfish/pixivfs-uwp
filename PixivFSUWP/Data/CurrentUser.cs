using FSharp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static CurrentUser FromJsonValue(JsonValue Source)
        {
            CurrentUser toret = new CurrentUser();
            toret.ID = Source.TryGetProperty("id").Value.AsInteger();
            toret.Username = Source.TryGetProperty("name").Value.AsString();
            toret.UserAccount = Source.TryGetProperty("account").Value.AsString();
            toret.Email = Source.TryGetProperty("mail_address").Value.AsString();
            toret.IsMailAuthorized= Source.TryGetProperty("is_mail_authorized").Value.AsBoolean();
            toret.IsPremium = Source.TryGetProperty("is_premium").Value.AsBoolean();
            toret.Avatar16 = Source.TryGetProperty("profile_image_urls").Value.TryGetProperty("px_16x16").Value.AsString();
            toret.Avatar50 = Source.TryGetProperty("profile_image_urls").Value.TryGetProperty("px_50x50").Value.AsString();
            toret.Avatar170 = Source.TryGetProperty("profile_image_urls").Value.TryGetProperty("px_170x170").Value.AsString();
            return toret;
        }
    }
}
