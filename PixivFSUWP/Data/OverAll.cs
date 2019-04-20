using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixivFS;
using PixivFSCS;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

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

        public static async Task<BitmapImage> LoadImageAsync(string Uri)
        {
            var toret = new BitmapImage();
            var resStream = await Task.Run(() => new PixivAppAPI(GlobalBaseAPI).csfriendly_no_auth_requests_call_stream("GET",
                  Uri, new List<Tuple<string, string>>() { ("Referer", "https://app-api.pixiv.net/").ToTuple() })
                  .ResponseStream);
            var memStream = new MemoryStream();
            await resStream.CopyToAsync(memStream);
            memStream.Position = 0;
            await toret.SetSourceAsync(memStream.AsRandomAccessStream());
            return toret;
        }
    }
}
