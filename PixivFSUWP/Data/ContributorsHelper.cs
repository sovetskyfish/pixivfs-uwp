using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Windows.Data.Json;

namespace PixivFSUWP.Data
{
    public static class ContributorsHelper
    {
        public async static Task<List<Contributor>> GetContributorsAsync()
        {
            try
            {
                List<Contributor> toret = new List<Contributor>();
                HttpClient client = new HttpClient();
                var res = await client.GetAsync("https://raw.githubusercontent.com/tobiichiamane/pixivfs-uwp/master/.all-contributorsrc");
                var json = JsonObject.Parse(await res.Content.ReadAsStringAsync());
                var array = json["contributors"].GetArray();
                foreach (var i in array)
                    toret.Add(Contributor.FromJsonValue(i.GetObject()));
                return toret;
            }
            catch
            {
                return null;
            }
        }
    }
}
