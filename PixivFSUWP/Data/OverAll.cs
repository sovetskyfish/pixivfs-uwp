using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using PixivFS;
using PixivFSCS;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.UserActivities;
using Windows.Security.Credentials;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using AdaptiveCards;
using Windows.UI.Shell;

namespace PixivFSUWP.Data
{
    public static class OverAll
    {
        public static Uri AppUri = null;
        public static PixivBaseAPI GlobalBaseAPI = new PixivBaseAPI();
        public const string passwordResource = "PixivFSUWPPassword";
        public const string refreshTokenResource = "PixivFSUWPRefreshToken";
        public static CurrentUser currentUser = null;
        public static RecommendIllustsCollection RecommendList { get; private set; } = new RecommendIllustsCollection();
        public static BookmarkIllustsCollection BookmarkList { get; set; }
        public static FollowingIllustsCollection FollowingList { get; private set; } = new FollowingIllustsCollection();

        public static void RefreshRecommendList()
        {
            RecommendList.StopLoading();
            RecommendList = new RecommendIllustsCollection();
        }

        public static void RefreshBookmarkList()
        {
            BookmarkList.StopLoading();
            BookmarkList = new BookmarkIllustsCollection();
        }

        public static void RefreshFollowingList()
        {
            FollowingList.StopLoading();
            FollowingList = new FollowingIllustsCollection();
        }

        public static async Task<MemoryStream> DownloadImage(string Uri)
        {
            var resStream = await Task.Run(() => new PixivAppAPI(GlobalBaseAPI).csfriendly_no_auth_requests_call_stream("GET",
                  Uri, new List<Tuple<string, string>>() { ("Referer", "https://app-api.pixiv.net/").ToTuple() })
                  .ResponseStream);
            var memStream = new MemoryStream();
            await resStream.CopyToAsync(memStream);
            memStream.Position = 0;
            return memStream;
        }

        public static async Task<string> GetDataUri(string Uri)
        {
            return string.Format("data:image/png;base64,{0}", Convert.ToBase64String((await DownloadImage(Uri)).ToArray()));
        }

        public static async Task<BitmapImage> LoadImageAsync(string Uri)
        {
            var toret = new BitmapImage();
            var memStream = await DownloadImage(Uri);
            await toret.SetSourceAsync(memStream.AsRandomAccessStream());
            memStream.Dispose();
            return toret;
        }

        public static async Task<WriteableBitmap> LoadImageAsync(string Uri, int Width, int Height)
        {
            var toret = new WriteableBitmap(Width, Height);
            var memStream = await DownloadImage(Uri);
            await toret.SetSourceAsync(memStream.AsRandomAccessStream());
            memStream.Dispose();
            return toret;
        }

        public static async Task<byte[]> ImageToBytes(WriteableBitmap Source)
        {
            byte[] toret;
            using (var stream = Source.PixelBuffer.AsStream())
            {
                toret = new byte[stream.Length];
                await stream.ReadAsync(toret, 0, toret.Length);
            }
            return toret;
        }

        public static async Task<WriteableBitmap> BytesToImage(byte[] Source, int Width, int Height)
        {
            WriteableBitmap toret = new WriteableBitmap(Width, Height);
            using (var stream = toret.PixelBuffer.AsStream())
            {
                await stream.WriteAsync(Source, 0, Source.Length);
            }
            return toret;
        }

        //展示一个新的窗口
        public static async Task ShowNewWindow(Type Page, object Parameter)
        {
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(Page, Parameter);
                Window.Current.Content = frame;
                Window.Current.Activate();
                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }

        //从Vault中获取身份信息
        //此版本只储存一个，未来可以储存多到20个
        public static PasswordCredential GetCredentialFromLocker(string resourceName)
        {
            PasswordCredential credential = null;
            var vault = new PasswordVault();
            try
            {
                var credentialList = vault.FindAllByResource(resourceName);
                if (credentialList.Count > 0) credential = credentialList[0];
            }
            catch { }
            return credential;
        }

        //时间线支持
        public static async Task GenerateActivityAsync(string DisplayText, AdaptiveCard Card, Uri ActivationUri, string ActivityID)
        {
            UserActivityChannel channel = UserActivityChannel.GetDefault();
            UserActivity userActivity = await channel.GetOrCreateUserActivityAsync(ActivityID);
            userActivity.VisualElements.DisplayText = DisplayText;
            userActivity.VisualElements.Content = AdaptiveCardBuilder.CreateAdaptiveCardFromJson(Card.ToJson());
            userActivity.ActivationUri = ActivationUri;
            await userActivity.SaveAsync();
        }
    }
}
