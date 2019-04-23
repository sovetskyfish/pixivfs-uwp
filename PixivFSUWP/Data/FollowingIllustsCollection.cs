using PixivFSCS;
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
using FSharp.Data;
using System.Web;

namespace PixivFSUWP.Data
{
    public class FollowingIllustsCollection : ObservableCollection<ViewModels.WaterfallItemViewModel>, ISupportIncrementalLoading
    {
        string nexturl = "begin";
        bool _busy = false;
        bool _emergencyStop = false;
        EventWaitHandle pause = new ManualResetEvent(true);

        public bool HasMoreItems
        {
            get => nexturl != "";
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
            if (_busy)
            {
                _emergencyStop = true;
                ResumeLoading();
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
            if (!HasMoreItems) return new LoadMoreItemsResult() { Count = 0 };
            try
            {
                LoadMoreItemsResult toret = new LoadMoreItemsResult() { Count = 0 };
                JsonValue recommendres = null;
                if (nexturl == "begin")
                    recommendres = await Task.Run(() => new PixivFS
                        .PixivAppAPI(OverAll.GlobalBaseAPI)
                        .csfriendly_illust_follow());
                else
                {
                    Uri next = new Uri(nexturl);
                    string getparam(string param) => HttpUtility.ParseQueryString(next.Query).Get(param);
                    recommendres = await Task.Run(() => new PixivFS
                        .PixivAppAPI(OverAll.GlobalBaseAPI)
                        .csfriendly_illust_follow(getparam("restrict"), getparam("offset")));
                }
                nexturl = recommendres.Item("next_url").AsString();
                foreach (var recillust in recommendres.Item("illusts").AsArray())
                {
                    if (_emergencyStop)
                    {
                        _emergencyStop = false;
                        return toret;
                    }
                    await Task.Run(() => pause.WaitOne());
                    Data.WaterfallItem recommendi = Data.WaterfallItem.FromJsonValue(recillust);
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
            }
        }
    }
}

