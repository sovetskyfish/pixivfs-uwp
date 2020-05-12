using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixivCS;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;

namespace PixivFSUWP.Data
{
    public static class UgoiraHelper
    {
        public static async Task<Ugoira> GetUgoiraAsync(string IllustID)
        {
            List<string> framefiles = new List<string>();
            Dictionary<string, int> framedelays = new Dictionary<string, int>();
            Dictionary<string, SoftwareBitmap> frameimgs = new Dictionary<string, SoftwareBitmap>();
            try
            {
                var res = await new PixivAppAPI(OverAll.GlobalBaseAPI).GetUgoiraMetadataAsync(IllustID);
                if (res.UgoiraMetadataUgoiraMetadata == null) return null;
                var framesarray = res.UgoiraMetadataUgoiraMetadata.Frames;
                foreach (var i in framesarray)
                {
                    var file = i.File;
                    framefiles.Add(file);
                    framedelays.Add(file, (int)i.Delay);
                }
                var zipurl = res.UgoiraMetadataUgoiraMetadata.ZipUrls.Medium?.ToString();
                using (var zipfile = await OverAll.DownloadImage(zipurl))
                {
                    using (ZipArchive ziparc = new ZipArchive(zipfile, ZipArchiveMode.Read))
                    {
                        foreach (var entry in ziparc.Entries)
                        {
                            var file = entry.FullName;
                            using (var memStream = new MemoryStream())
                            {
                                await entry.Open().CopyToAsync(memStream);
                                memStream.Position = 0;
                                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(memStream.AsRandomAccessStream());
                                frameimgs.Add(file, await decoder.GetSoftwareBitmapAsync());
                            }
                        }
                    }
                }
                Ugoira toret = new Ugoira();
                foreach (var i in framefiles)
                    toret.Frames.Add(new Ugoira.Frame() { Image = frameimgs[i], Delay = framedelays[i] });
                return toret;
            }
            finally
            {
                framefiles.Clear();
                framedelays.Clear();
                frameimgs.Clear();
            }
        }
    }
}
