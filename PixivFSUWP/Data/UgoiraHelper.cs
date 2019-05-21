using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixivCS;
using Windows.UI.Xaml.Media.Imaging;

namespace PixivFSUWP.Data
{
    public static class UgoiraHelper
    {
        public static async Task<Ugoira> GetUgoiraAsync(string IllustID)
        {
            var res = await new PixivAppAPI(OverAll.GlobalBaseAPI).UgoiraMetadata(IllustID);
            if (res.Stringify().Contains("error")) return null;
            List<string> framefiles = new List<string>();
            Dictionary<string, int> framedelays = new Dictionary<string, int>();
            var framesarray = res["ugoira_metadata"].GetObject()["frames"].GetArray();
            foreach (var i in framesarray)
            {
                var file = i.GetObject()["file"].GetString();
                framefiles.Add(file);
                framedelays.Add(file, (int)i.GetObject()["delay"].GetNumber());
            }
            Dictionary<string, BitmapImage> frameimgs = new Dictionary<string, BitmapImage>();
            var zipurl = res["ugoira_metadata"].GetObject()["zip_urls"].GetObject()["medium"].GetString();
            using (var zipfile = await OverAll.DownloadImage(zipurl))
            {
                using (ZipArchive ziparc = new ZipArchive(zipfile, ZipArchiveMode.Read))
                {
                    foreach (var entry in ziparc.Entries)
                    {
                        var file = entry.FullName;
                        BitmapImage img = new BitmapImage();
                        using (var memStream = new MemoryStream())
                        {
                            memStream.Position = 0;
                            await entry.Open().CopyToAsync(memStream);
                            await img.SetSourceAsync(memStream.AsRandomAccessStream());
                        }
                        frameimgs.Add(file, img);
                    }
                }
            }
            Ugoira toret = new Ugoira();
            foreach (var i in framefiles)
                toret.Frames.Add(new Ugoira.Frame() { Image = frameimgs[i], Delay = framedelays[i] });
            framefiles.Clear();
            framedelays.Clear();
            frameimgs.Clear();
            return toret;
        }
    }
}
