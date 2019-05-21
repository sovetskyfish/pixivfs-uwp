using PixivCS;
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
            else
            {
                Clear();
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
                JsonObject recommendres = null;
                if (nexturl == "begin")
                    recommendres = await new PixivCS
                        .PixivAppAPI(OverAll.GlobalBaseAPI)
                        .IllustRecommended();
                else
                {
                    Uri next = new Uri(nexturl);
                    string getparam(string param) => HttpUtility.ParseQueryString(next.Query).Get(param);
                    recommendres = await new PixivCS
                        .PixivAppAPI(OverAll.GlobalBaseAPI)
                        .IllustRecommended(ContentType:
                            getparam("content_type"),
                            IncludeRankingLabel: bool.Parse(getparam("include_ranking_label")),
                            Filter: getparam("filter"),
                            MinBookmarkIDForRecentIllust: getparam("min_bookmark_id_for_recent_illust"),
                            MaxBookmarkIDForRecommended: getparam("max_bookmark_id_for_recommend"),
                            Offset: getparam("offset"),
                            IncludeRankingIllusts: bool.Parse(getparam("include_ranking_illusts")),
                            IncludePrivacyPolicy: getparam("include_privacy_policy"));
                }
                nexturl = recommendres["next_url"].TryGetString();
                foreach (var recillust in recommendres["illusts"].GetArray())
                {
                    await Task.Run(() => pause.WaitOne());
                    if (_emergencyStop)
                    {
                        _emergencyStop = false;
                        nexturl = "";
                        Clear();
                        throw new Exception();
                    }
                    Data.WaterfallItem recommendi = Data.WaterfallItem.FromJsonValue(recillust.GetObject());
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

