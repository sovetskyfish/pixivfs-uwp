using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace PixivFSUWP.Data
{
    public class IllustCommentItem
    {
        public string Comment { get; set; }
        public string DateTime { get; set; }
        public string UserName { get; set; }
        public string UserAccount { get; set; }
        public string AvatarUrl { get; set; }

        public static IllustCommentItem FromJsonValue(JsonObject Source)
        {
            IllustCommentItem toret = new IllustCommentItem();
            toret.Comment = Source["comment"].GetString();
            toret.DateTime = Source["date"].GetString();
            toret.UserName = Source["user"].GetObject()["name"].GetString();
            toret.UserAccount = Source["user"].GetObject()["account"].GetString();
            toret.AvatarUrl = Source["user"].GetObject()["profile_image_urls"].GetObject()["medium"].GetString();
            return toret;
        }
    }
}
