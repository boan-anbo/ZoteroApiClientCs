using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ZoteroApiClientCs.Models;
using ZoteroApiClientCs.ZoteroApiEntities;

namespace ZoteroApiClientCs.Api
{
    public class ZoteroApiClient
    {
        private string _accessToken;

        private string _userId;

        private static HttpClient _http;

        private ZoteroApiUrls _zoteroApiUrls;


        public ZoteroApiClient(string accessToken, string userId)
        {
            this._accessToken = accessToken;
            this._userId = userId;
            var http = new HttpClient();
            http.DefaultRequestHeaders.Add("Authorization", "Bearer " + this._accessToken);
            _http = http;
            _zoteroApiUrls = new ZoteroApiUrls(userId);

            // serializer setting
        }

        public async Task<ZoteroApiResponseMany<ZoteroCollection>> GetUserCollections(ZoteroApiParameters p=null)
        {
            var url = _zoteroApiUrls.GetUserCollectionsUrl(p);
            return await SendRequestMany<ZoteroCollection>(url);

        }

        public async Task<ZoteroApiResponseMany<ZoteroCollection>> GetUserCollectionsAll(ZoteroApiParameters p=null)
        {
            var firstPage = await GetUserCollections(p);

            return await GetAllResults(firstPage);
        }

        public async Task<ZoteroApiResponseMany<ZoteroItem>> GetCollectionItemsAll(string collectionId,
            ZoteroApiParameters p=null)
        {
            var firstPage = await GetCollectionItems(collectionId, p);

            return await GetAllResults<ZoteroItem>(firstPage);
        }

        public async Task<ZoteroApiResponseMany<T>> GetAllResults<T>(ZoteroApiResponseMany<T> firstPage)
        {
            
            var finalResults = ZoteroApiResponseMany<T>.LoadResponseMany(firstPage.MetaData, firstPage.Data, firstPage.RequestUrl);
            var currentPage = firstPage;
            while (currentPage.HasNextPage)
            {
                Console.WriteLine("Found next page.");
                Console.WriteLine("Sleeping for 1000 before getting next page");
                Thread.Sleep(1000);
                var nextPage = await currentPage.GetNextPage();
                finalResults.Data.AddRange(nextPage.Data);
                currentPage = nextPage;
                Console.WriteLine("Next page loaded");

            }

            return finalResults;
        }

        public async Task<ZoteroApiResponseMany<ZoteroItem>> GetCollectionItems(string collectionId,
            ZoteroApiParameters p=null)
        {
            var url = _zoteroApiUrls.GetCollectionItemUrl(collectionId, p);
            return await SendRequestMany<ZoteroItem>(url);
        }
        
        public async Task<ZoteroApiResponseSingle<ZoteroCollection>> GetCollection(string colKey,
            ZoteroApiParameters p=null)
        {
            var url = _zoteroApiUrls.GetCollectionUrl(colKey, p);
            return await SendRequestSingle<ZoteroCollection>(url);
        }
        public async Task<ZoteroApiResponseSingle<ZoteroItem>> GetItem(string itemKey,
            ZoteroApiParameters p=null)
        {
            var url = _zoteroApiUrls.GetItemUrl(itemKey, p);
            return await SendRequestSingle<ZoteroItem>(url);
        }

        /// <summary>
        /// Static method to send generic requests and expect an array of entries, e.g. get collection items.
        /// </summary>
        /// <param name="url"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async static Task<ZoteroApiResponseMany<T>> SendRequestMany<T>(string url)
        {
            var response = await _http.GetAsync(url);
            return await ZoteroApiResponseMany<T>.LoadResponseMany(response);
        }
        
        /// <summary>
        /// expect a single entry, i.e. get item.
        /// </summary>
        /// <param name="url"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async static Task<ZoteroApiResponseSingle<T>> SendRequestSingle<T>(string url)
        {
            var response = await _http.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Zotero Record at {url} is not found. Response:\n${errorMessage}");
            }
            return await ZoteroApiResponseSingle<T>.LoadResponseSingle(response);
        }

    }
}