using PixivFSUWP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivFSUWP.ViewModels
{
    public class ContributorViewModel
    {
        public string AvatarUrl { get; set; }
        public string DisplayName { get; set; }
        public string Account { get; set; }
        public string ProfileUrl { get; set; }
        public List<Contribution> Contributions { get; set; }

        public static ContributorViewModel FromItem(Contributor Item)
        {
            return new ContributorViewModel()
            {
                AvatarUrl = Item.AvatarUrl + "&s=45",
                DisplayName = Item.DisplayName,
                Account = "@" + Item.Account,
                ProfileUrl = Item.ProfileUrl,
                Contributions = Item.Contributions
            };
        }
    }
}
