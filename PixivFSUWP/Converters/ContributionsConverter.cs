using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace PixivFSUWP.Converters
{
    public class ContributionsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string toret = "";
            if (value == null) return toret;
            var source = value as List<Data.Contribution>;
            source.Sort();
            foreach (var contribution in source)
            {
                switch (contribution)
                {
                    case Data.Contribution.bug:
                        toret += "🐛";
                        break;
                    case Data.Contribution.code:
                        toret += "💻";
                        break;
                    case Data.Contribution.doc:
                        toret += "📖";
                        break;
                    case Data.Contribution.idea:
                        toret += "🤔";
                        break;
                    case Data.Contribution.translation:
                        toret += "🌍";
                        break;
                    case Data.Contribution.unknown:
                        toret += "❓";
                        break;
                }
            }
            return toret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
