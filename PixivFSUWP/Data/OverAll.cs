using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixivFS;

namespace PixivFSUWP.Data
{
    public static class OverAll
    {
        public static PixivBaseAPI GlobalBaseAPI = new PixivBaseAPI();
        public const string passwordResource = "PixivFSUWPPassword";
        public const string refreshTokenResource = "PixivFSUWPRefreshToken";

        public static RecommendIllustsCollection RecommendList { get; private set; } = new RecommendIllustsCollection();

        public static void RefreshRecommendList()
        {
            RecommendList.StopLoading();
            RecommendList.Clear();
            RecommendList = new RecommendIllustsCollection();
        }
    }
}
