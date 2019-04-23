using FSharp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivFSUWP.Data
{
    public class IllustCommentItem
    {
        public string Comment { get; set; }
        public string DateTime { get; set; }
        public string UserName { get; set; }
        public string UserAccount { get; set; }
        public string AvatarUrl { get; set; }

        public static IllustCommentItem FromJsonValue(JsonValue Source)
        {
            IllustCommentItem toret = new IllustCommentItem();
            toret.Comment = Source.Item("comment").AsString();
            toret.DateTime = Source.Item("date").AsString();
            toret.UserName = Source.Item("user").Item("name").AsString();
            toret.UserAccount = Source.Item("user").Item("account").AsString();
            toret.AvatarUrl = Source.Item("user").Item("profile_image_urls").Item("medium").AsString();
            return toret;
        }
    }
}
