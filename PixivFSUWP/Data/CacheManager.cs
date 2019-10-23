using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PixivFSUWP.Data
{
    //用来管理缓存
    public static class CacheManager
    {
        //临时目录
        private static StorageFolder tmpFolder = ApplicationData.Current.TemporaryFolder;

        //获取临时目录，确保目录存在
        private static async Task<StorageFolder> getCacheFolder()
        {
            var cacheFolder = await tmpFolder.TryGetItemAsync("Cache");
            if (cacheFolder == null) return await tmpFolder.CreateFolderAsync("Cache");
            else return cacheFolder as StorageFolder;
        }

        //清除缓存
        public static async Task ClearCache()
        {
            var cacheFolder = await tmpFolder.TryGetItemAsync("imgCache");
            if (cacheFolder != null) await cacheFolder.DeleteAsync();
        }

        //得到目录大小
        private static async Task<long> getFolderSize(StorageFolder target)
        {
            var getFileSizeTasks = from file
                                   in await target.CreateFileQuery().GetFilesAsync()
                                   select file.GetBasicPropertiesAsync().AsTask();
            var fileSizes = await Task.WhenAll(getFileSizeTasks);
            var getFolderSizetTasks = from folder
                                      in await target.CreateFolderQuery().GetFoldersAsync()
                                      select getFolderSize(folder);
            var folderSizes = await Task.WhenAll(getFolderSizetTasks);
            return fileSizes.Sum(i => (long)i.Size) + folderSizes.Sum();
        }

        //得到缓存目录大小
        public static async Task<long> GetCacheSize()
            => await getFolderSize(await getCacheFolder());
    }
}
