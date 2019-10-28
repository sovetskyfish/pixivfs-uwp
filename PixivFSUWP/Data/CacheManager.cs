using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PixivFSUWP.Data
{
    //记录临时的缓存文件及其期望的名称
    public struct TempStorageFile
    {
        public StorageFile File;
        public string ExpectedName;
    }

    //用来管理缓存
    public static class CacheManager
    {
        //临时目录
        private static StorageFolder tmpFolder = ApplicationData.Current.TemporaryFolder;

        //获取临时目录，确保目录存在
        private static async Task<StorageFolder> getCacheFolderAsync()
        {
            var cacheFolder = await tmpFolder.TryGetItemAsync("Cache");
            if (cacheFolder == null) return await tmpFolder.CreateFolderAsync("Cache");
            else return cacheFolder as StorageFolder;
        }

        //清除缓存
        public static async Task ClearCacheAsync()
        {
            var cacheFolder = await tmpFolder.TryGetItemAsync("imgCache");
            if (cacheFolder != null) await cacheFolder.DeleteAsync();
        }

        //得到目录大小
        private static async Task<long> getFolderSizeAsync(StorageFolder target)
        {
            var getFileSizeTasks = from file
                                   in await target.CreateFileQuery().GetFilesAsync()
                                   select file.GetBasicPropertiesAsync().AsTask();
            var fileSizes = await Task.WhenAll(getFileSizeTasks);
            return fileSizes.Sum(i => (long)i.Size);
        }

        //得到缓存目录大小
        public static async Task<long> GetCacheSizeAsync()
            => await getFolderSizeAsync(await getCacheFolderAsync());

        //得到缓存文件
        public static async Task<StorageFile> GetCachedFileAsync(string Filename)
        {
            var cacheFolder = await getCacheFolderAsync();
            if (await cacheFolder.TryGetItemAsync(Filename) == null) return null;
            return await cacheFolder.GetFileAsync(Filename);
        }

        //删除缓存文件
        public static async Task<bool> DeleteCachedFileAsync(string Filename)
        {
            var file = await GetCachedFileAsync(Filename);
            if (file == null) return false;
            await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            return true;
        }

        //创建缓存文件
        //文件将以随机的Guid命名，完成写入时再命名为期望的文件名
        //此举有助于防止写入一半时程序非法终止而产生损坏的缓存文件
        public static async Task<TempStorageFile?> CreateCacheFileAsync(string Filename)
        {
            Guid guid;
            var test = Guid.TryParse(Filename, out guid);
            //避免使用GUID作为文件名
            if (test) return null;
            do guid = Guid.NewGuid();
            while (await GetCachedFileAsync(guid.ToString()) != null);
            var cacheFolder = await getCacheFolderAsync();
            var newFile = await cacheFolder.CreateFileAsync(guid.ToString());
            TempStorageFile result = new TempStorageFile();
            result.ExpectedName = Filename;
            result.File = newFile;
            return result;
        }

        //完成写入缓存文件
        //ForceUpdate: 是否覆盖已有的文件
        public static async Task<bool> FinishCachedFileAsync(TempStorageFile File, bool ForceUpdate = false)
        {
            if (await GetCachedFileAsync(File.ExpectedName) != null)
            {
                if (ForceUpdate) await DeleteCachedFileAsync(File.ExpectedName);
                else return false;
            }
            await File.File.MoveAsync(await getCacheFolderAsync(), File.ExpectedName);
            return true;
        }

        //清理名称为GUID的临时文件
        public static async Task ClearTempFilesAsync()
        {
            var deleteTempFilesTasks = from file
                                       in await (await getCacheFolderAsync()).CreateFileQuery().GetFilesAsync()
                                       where Guid.TryParse(file.Name, out _)
                                       select file.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask();
            await Task.WhenAll(deleteTempFilesTasks);
        }
    }
}
