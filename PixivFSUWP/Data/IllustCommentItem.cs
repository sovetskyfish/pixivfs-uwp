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
        public int ID { get; set; }
        public string Comment { get; set; }
        public string DateTime { get; set; }
        public string UserName { get; set; }
        public string UserAccount { get; set; }
        public string AvatarUrl { get; set; }
        public int ParentCommentID { get; set; }

        public static IllustCommentItem FromJsonValue(JsonObject Source)
        {
            IllustCommentItem toret = new IllustCommentItem();
            toret.ID = (int)Source["id"].GetNumber();
            toret.Comment = Source["comment"].TryGetString();
            toret.DateTime = Source["date"].TryGetString();
            toret.UserName = Source["user"].GetObject()["name"].TryGetString();
            toret.UserAccount = Source["user"].GetObject()["account"].TryGetString();
            toret.AvatarUrl = Source["user"].GetObject()["profile_image_urls"].GetObject()["medium"].TryGetString();
            var parent = Source["parent_comment"].ToString();
            if(parent!="{}")
            {
                //有父级评论
                toret.ParentCommentID = (int)Source["parent_comment"].GetObject()["id"].GetNumber();
            }
            else
            {
                toret.ParentCommentID = -1;
            }
            return toret;
        }
    }
}
