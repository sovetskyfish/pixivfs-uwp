using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace PixivFSUWP.ViewModels
{
    public class CommentViewModel
    {
        public string Comment { get; set; }
        public string UserName { get; set; }
        public string UserAccount { get; set; }
        private string _dateTime { get; set; }
        public string AvatarUrl { get; set; }
        //public BitmapImage Avatar { get; set; }
        public string DateTime
        {
            get => DateTimeOffset.Parse(_dateTime).LocalDateTime.ToString();
        }

        //public async Task LoadAvatarAsync()
        //    => Avatar = await Data.OverAll.LoadImageAsync(AvatarUrl);

        public static CommentViewModel FromItem(Data.IllustCommentItem Item)
        {
            return new CommentViewModel()
            {
                Comment = Item.Comment,
                UserName = Item.UserName,
                UserAccount = "@" + Item.UserAccount,
                _dateTime = Item.DateTime,
                AvatarUrl = Item.AvatarUrl
            };
        }
    }
}
