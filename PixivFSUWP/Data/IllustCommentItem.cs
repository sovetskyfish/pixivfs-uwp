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
            toret.Comment = Source.TryGetProperty("comment").Value.AsString();
            toret.DateTime = Source.TryGetProperty("date").Value.AsString();
            toret.UserName = Source.TryGetProperty("user").Value.TryGetProperty("name").Value.AsString();
            toret.UserAccount = Source.TryGetProperty("user").Value.TryGetProperty("account").Value.AsString();
            toret.AvatarUrl = Source.TryGetProperty("user").Value.TryGetProperty("profile_image_urls").Value.TryGetProperty("medium").Value.AsString();
            return toret;
        }
    }
}
