using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivFSUWP.Data
{
    public class CommentAvatarLoader
    {
        private bool isBusy = false;
        private bool emergencyStop = false;
        private int currentIndex = 0;
        private ObservableCollection<ViewModels.CommentViewModel> collection;

        public CommentAvatarLoader(ObservableCollection<ViewModels.CommentViewModel> Collection) => collection = Collection;

        public async Task LoadAvatars()
        {
            if (isBusy) return;
            isBusy = true;
            try
            {
                while (currentIndex + 1 <= collection.Count)
                {
                    if (emergencyStop)
                    {
                        collection = null;
                        return;
                    }
                    await collection[currentIndex].LoadAvatarAsync();
                    if (collection[currentIndex].ChildrenComments != null)
                    {
                        foreach (var child in collection[currentIndex].ChildrenComments)
                        {
                            await child.LoadAvatarAsync();
                        }
                    }
                    currentIndex++;
                }
            }
            finally
            {
                isBusy = false;
            }
        }

        public void EmergencyStop() => emergencyStop = true;
    }
}
