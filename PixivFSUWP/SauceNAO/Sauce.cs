using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivFSUWP.SauceNao {
  class Sauce {
    public string Title, AuthorName;
    public int     SauceId, AuthorId;
    public string Url {
      get { return GetUrl(); }
    }

    protected Sauce(string title, int sauceId, string authorName, int authorId) {
      Title      = title;
      SauceId    = sauceId;
      AuthorName = authorName;
      AuthorId   = authorId;
    }

    public override string ToString() {
      return String.Format("Title: {0}\nSauce ID: {1}\nAuthor: {2} (#{3})", Title, SauceId, AuthorName, AuthorId);
    }

    protected virtual string GetUrl() {
      throw new NotImplementedException();
    }

    public static bool IsTheRightAdapterFor(dynamic data) {
      throw new NotImplementedException();
    }
  }
}
