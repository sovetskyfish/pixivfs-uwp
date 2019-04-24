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
    public class RecommendIllustsCollection : ObservableCollection<ViewModels.WaterfallItemViewModel>, ISupportIncrementalLoading
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
            try
            {
                if (!HasMoreItems) return new LoadMoreItemsResult() { Count = 0 };
                LoadMoreItemsResult toret = new LoadMoreItemsResult() { Count = 0 };
                JsonValue recommendres = null;
                if (nexturl == "begin")
                    recommendres = await Task.Run(() => new PixivFS
                        .PixivAppAPI(OverAll.GlobalBaseAPI)
                        .csfriendly_illust_recommended());
                else
                {
                    Uri next = new Uri(nexturl);
                    string getparam(string param) => HttpUtility.ParseQueryString(next.Query).Get(param);
                    recommendres = await Task.Run(() => new PixivFS
                        .PixivAppAPI(OverAll.GlobalBaseAPI)
                        .csfriendly_illust_recommended(content_type:
                            getparam("content_type"),
                            include_ranking_label: bool.Parse(getparam("include_ranking_label")),
                            filter: getparam("filter"),
                            min_bookmark_id_for_recent_illust: getparam("min_bookmark_id_for_recent_illust"),
                            max_bookmark_id_for_recommend: getparam("max_bookmark_id_for_recommend"),
                            offset: getparam("offset"),
                            include_ranking_illusts: bool.Parse(getparam("include_ranking_illusts")),
                            include_privacy_policy: getparam("include_privacy_policy")));
                }
                nexturl = recommendres.TryGetProperty("next_url").Value.AsString();
                foreach (var recillust in recommendres.TryGetProperty("illusts").Value.AsArray())
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

