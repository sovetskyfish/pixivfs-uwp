using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using System.Web;
using Windows.Data.Json;

namespace PixivFSUWP.Data.Collections
{
    public class FollowingIllustsCollection : ObservableCollection<ViewModels.WaterfallItemViewModel>, ISupportIncrementalLoading
    {
        string nexturl = "begin";
        bool _busy = false;
        bool _emergencyStop = false;
        EventWaitHandle pause = new ManualResetEvent(true);

        public bool HasMoreItems
        {
            get => !string.IsNullOrEmpty(nexturl);
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (_busy)
                throw new InvalidOperationException("Only one operation in flight at a time");
            _busy = true;
            return AsyncInfo.Run((c) => LoadMoreItemsAsync(c, count));
        }

        public void StopLoading()
        {
            _emergencyStop = true;
            if (_busy)
            {
                ResumeLoading();
            }
            else
            {
                Clear();
                GC.Collect();
            }
        }

        public void PauseLoading()
        {
            pause.Reset();
        }

        public void ResumeLoading()
        {
            pause.Set();
        }

        protected async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken c, uint count)
        {
            try
            {
                if (!HasMoreItems) return new LoadMoreItemsResult() { Count = 0 };
                LoadMoreItemsResult toret = new LoadMoreItemsResult() { Count = 0 };
                JsonObject followingres = null;
                try
                {
                    if (nexturl == "begin")
                        followingres = await new PixivCS
                            .PixivAppAPI(OverAll.GlobalBaseAPI)
                            .IllustFollow();
                    else
                    {
                        Uri next = new Uri(nexturl);
                        string getparam(string param) => HttpUtility.ParseQueryString(next.Query).Get(param);
                        followingres = await new PixivCS
                            .PixivAppAPI(OverAll.GlobalBaseAPI)
                            .IllustFollow(getparam("restrict"), getparam("offset"));
                    }
                }
                catch
                {
                    return toret;
                }
                nexturl = followingres["next_url"].TryGetString();
                foreach (var recillust in followingres["illusts"].GetArray())
                {
                    await Task.Run(() => pause.WaitOne());
                    if (_emergencyStop)
                    {
                        nexturl = "";
                        Clear();
                        return new LoadMoreItemsResult() { Count = 0 };
                    }
                    WaterfallItem recommendi = WaterfallItem.FromJsonValue(recillust.GetObject());
                    var recommendmodel = ViewModels.WaterfallItemViewModel.FromItem(recommendi);
                    await recommendmodel.LoadImageAsync();
                    Add(recommendmodel);
                    toret.Count++;
                }
                return toret;
            }
            finally
            {
                _busy = false;
                if (_emergencyStop)
                {
                    nexturl = "";
                    Clear();
                    GC.Collect();
                }
            }
        }
    }
}

