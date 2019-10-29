using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace PixivFSUWP.Data
{
    public enum Contribution
    {
        doc, translation, code, bug, idea, unknown
    }

    public class Contributor
    {
        public string AvatarUrl { get; set; }
        public string DisplayName { get; set; }
        public string Account { get; set; }
        public string ProfileUrl { get; set; }
        public List<Contribution> Contributions { get; set; }

        //由字符串返回Contribution枚举
        private static Contribution stringToContribution(string source)
        {
            switch (source)
            {
                case "doc":
                    return Contribution.doc;
                case "translation":
                    return Contribution.translation;
                case "code":
                    return Contribution.code;
                case "bug":
                    return Contribution.bug;
                case "ideas":
                    return Contribution.idea;
                default:
                    return Contribution.unknown;
            }
        }

        public static Contributor FromJsonValue(JsonObject Source)
        {
            Contributor toret = new Contributor();
            toret.Account = Source["login"].TryGetString();
            toret.DisplayName = Source["name"].TryGetString();
            toret.AvatarUrl = Source["avatar_url"].TryGetString();
            toret.ProfileUrl = Source["profile"].TryGetString();
            toret.Contributions = new List<Contribution>();
            var contributions = Source["contributions"].GetArray();
            foreach (var contribution in contributions)
                toret.Contributions.Add(stringToContribution(contribution.TryGetString()));
            return toret;
        }
    }
}
