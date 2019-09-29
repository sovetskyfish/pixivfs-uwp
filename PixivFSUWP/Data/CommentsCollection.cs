using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace PixivFSUWP.Data
{
    public class CommentsCollection : ObservableCollection<ViewModels.CommentViewModel>, ISupportIncrementalLoading
    {
        string nexturl = "begin";
        bool _busy = false;
        bool _emergencyStop = false;
        EventWaitHandle pause = new ManualResetEvent(true);
        readonly string illustid;
        List<ViewModels.CommentViewModel> ChildrenComments = new List<ViewModels.CommentViewModel>();
        public CommentAvatarLoader AvatarLoader;

        public CommentsCollection(string IllustID)
        {
            illustid = IllustID;
            AvatarLoader = new CommentAvatarLoader(this);
        }

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
                JsonObject commentres = null;
                try
                {
                    if (nexturl == "begin")
                        commentres = await new PixivCS
                            .PixivAppAPI(OverAll.GlobalBaseAPI)
                            .IllustComments(illustid, IncludeTotalComments: true);
                    else
                    {
                        Uri next = new Uri(nexturl);
                        string getparam(string param) => HttpUtility.ParseQueryString(next.Query).Get(param);
                        commentres = await new PixivCS
                            .PixivAppAPI(OverAll.GlobalBaseAPI)
                            .IllustComments(illustid, getparam("offset"), bool.Parse(getparam("include_total_comments")));
                    }
                }
                catch
                {
                    return toret;
                }
                nexturl = commentres["next_url"].TryGetString();
                foreach (var recillust in commentres["comments"].GetArray())
                {
                    await Task.Run(() => pause.WaitOne());
                    if (_emergencyStop)
                    {
                        nexturl = "";
                        Clear();
                        return new LoadMoreItemsResult() { Count = 0 };
                    }
                    Data.IllustCommentItem recommendi = Data.IllustCommentItem.FromJsonValue(recillust.GetObject());
                    var recommendmodel = ViewModels.CommentViewModel.FromItem(recommendi);
                    //查找是否存在子回复
                    var children = from item
                                   in ChildrenComments
                                   where item.ParentID == recommendmodel.ID
                                   select item;
                    children = children.ToList();
                    if (children.Count() > 0)
                    {
                        //存在子回复
                        recommendmodel.ChildrenComments = new ObservableCollection<ViewModels.CommentViewModel>();
                        foreach (var child in children)
                        {
                            if (child.ChildrenComments != null)
                            {
                                foreach (var childschild in child.ChildrenComments)
                                {
                                    childschild.Comment = string.Format("Re: {0}: {1}",
                                        child.UserName, childschild.Comment);
                                    recommendmodel.ChildrenComments.Add(childschild);
                                }
                                child.ChildrenComments.Clear();
                                child.ChildrenComments = null;
                                GC.Collect();
                            }
                            recommendmodel.ChildrenComments.Insert(0, child);
                            ChildrenComments.Remove(child);
                        }
                    }
                    //检查自己是不是子回复
                    if (recommendmodel.ParentID != -1)
                    {
                        //自己也是子回复
                        ChildrenComments.Add(recommendmodel);
                    }
                    else
                    {
                        //自己并非子回复
                        Add(recommendmodel);
                        toret.Count++;
                    }
                }
                return toret;
            }
            finally
            {
                _busy = false;
                _ = AvatarLoader.LoadAvatars();
                if (_emergencyStop)
                {
                    AvatarLoader.EmergencyStop();
                    nexturl = "";
                    Clear();
                    GC.Collect();
                }
            }
        }
    }
}
