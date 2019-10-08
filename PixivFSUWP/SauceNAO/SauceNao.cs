using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PixivFSUWP.SauceNao {
  class SauceNao {
    private static string ENDPOINT = "https://saucenao.com/search.php";
    private        string ApiKey;

    public SauceNao(string apiKey) {
      this.ApiKey = apiKey;
    }

    public List<Result> Request(string url) {
      return Request(url, null);
    }

    public List<Result> Request(string url, WebProxy proxy) {
      WebClient webClient = new WebClient();
      webClient.Proxy     = proxy;

      webClient.QueryString.Add("db",          "999");
      webClient.QueryString.Add("output_type", "2");
      webClient.QueryString.Add("numres",      "16");
      webClient.QueryString.Add("api_key",     ApiKey);
      webClient.QueryString.Add("url",         url);

      List<Result> results = new List<Result>();

      try {
        string  response = webClient.DownloadString(ENDPOINT);
        dynamic dynObj   = JsonConvert.DeserializeObject(response);
        foreach(var result in dynObj.results) {
          results.Add(new Result(result.header, result.data));
        }
      }
      catch (Exception e) {
        Console.WriteLine(e.Message);
      }

      return results;
    }
  }
}
