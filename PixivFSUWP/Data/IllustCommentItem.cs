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

        public static IllustCommentItem FromObject(PixivCS.Objects.Comment Source)
        {
            IllustCommentItem toret = new IllustCommentItem();
            toret.ID = (int)Source.Id;
            toret.Comment = Source.CommentComment;
            toret.DateTime = Source.Date;
            toret.UserName = Source.User.Name;
            toret.UserAccount = Source.User.Account;
            toret.AvatarUrl = Source.User.ProfileImageUrls.Medium?.ToString() ?? "";
            if (Source.ParentComment.CommentComment != null)
            {
                //有父级评论
                toret.ParentCommentID = (int)Source.ParentComment.Id;
            }
            else
            {
                toret.ParentCommentID = -1;
            }
            return toret;
        }
    }
}
