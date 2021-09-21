using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using MyNamespace;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using ZoteroApiClientCs.Api;
using ZoteroApiClientCs.Models;
using ZoteroApiClientCs.Utils;
using ZoteroApiClientCs.ZoteroApiEntities;

namespace ZoteroApiClientCs
{
    public class ClientTest
    {
        private readonly ITestOutputHelper _out;
        private static readonly HttpClient http = new HttpClient();
        private static ZoteroApiClient _zoteroApiClient;

        public ClientTest(ITestOutputHelper @out)
        {
            _out = @out;


            var baseUrl = "https://api.zotero.org";

            var accessToken = "ilAvaQxJPS7h2NQYp8kE3BnQ";
            var user = "1826983";
            _zoteroApiClient = new Api.ZoteroApiClient(accessToken, user);
        }

        [Fact]
        public async void TestClient()
        {
            var accessToken = "ilAvaQxJPS7h2NQYp8kE3BnQ";
            var baseUrl = "https://api.zotero.org";
            var user = "/users/1826983";
            var collections = "collections";
            var url = baseUrl + user + "/" + collections;
            var testUrl = "https://api.zotero.org/users/1826983/collections";
            Assert.Equal(url, testUrl);
            _out.WriteLine("https://api.zotero.org/users/1826983/collections");
            http.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var response = await http.GetAsync(url);
            _out.WriteLine(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async void Get_Collections()
        {
            var results = await _zoteroApiClient.GetUserCollectionsAll(new ZoteroApiParameters());
            Assert.NotEmpty(results.Data);
            DisplayResult(results);
        }

        private void DisplayResult<T>(ZoteroApiResponseMany<T> result)
        {
            _out.WriteLine($"Request {result.RequestUrl}");
            _out.WriteLine(result.MetaData.Prev ?? "No Prev");
            _out.WriteLine(result.MetaData.Next ?? "No Next");
            _out.WriteLine(result.MetaData.Last ?? "No Last");
            // _out.WriteLine(results.MetaData.Links);
            _out.WriteLine(result.MetaData.Alternate ?? "No Alternate");
            _out.WriteLine(result.MetaData.TotalResults?.ToString());
            _out.WriteLine($"Actual Data size {result.Data.Count}");

            foreach (var col in result.Data)
            {
                var jsonString = JsonConvert.SerializeObject(col, Formatting.Indented);
                _out.WriteLine(jsonString);
            }
        }

        [Fact]
        private async void Get_Item()
        {
            var itemKey = "TM229X9T";
            var item = await _zoteroApiClient.GetItem(itemKey);
            Assert.NotNull(item.Data);
            Assert.Equal(itemKey, item.Data.Key);
        }

        [Fact]
        private async System.Threading.Tasks.Task Get_Collection()
        {
            var wrongColKey = "TM229X9T";
            await Assert.ThrowsAsync<Exception>(() => _zoteroApiClient.GetCollection(wrongColKey));
            var correctColKey = "YV7NLJLV";
            var col = await _zoteroApiClient.GetCollection(correctColKey);
            Assert.NotNull(col.Data);
            Assert.Equal(correctColKey, col.Data.Data.Key);
        }

        [Fact]
        public async void Get_Collection_Items()
        {
            var results = await _zoteroApiClient.GetCollectionItemsAll("KI7ELGWY");

            DisplayResult(results);

            // TUrn to the next page;
            // results = await results.GetNextPage();
        }
    }
}