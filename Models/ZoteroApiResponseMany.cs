using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ZoteroApiClientCs.Api;
using ZoteroApiClientCs.Utils;
using ZoteroApiClientCs.ZoteroApiEntities;

namespace ZoteroApiClientCs.Models
{
    public class ZoteroApiResponseMany<T> : ZoteroApiResponse<T>
    {
        /// <summary>
        /// The main difference between Many and Single.
        /// </summary>
        public List<ZoteroReturnData<T>> Data;

        public ZoteroApiResponseMany(ZoteroResponseHeaderReader metaData, List<ZoteroReturnData<T>> data,
            string requestUrl) : base(metaData, requestUrl)
        {
            Data = data;
        }

        /// <summary>
        /// Static constructor to follow the factory pattern because of the need to asynchronously load the json from the response content.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static async Task<ZoteroApiResponseMany<T>> LoadResponseMany(HttpResponseMessage response)
        {
            var meta = new ZoteroResponseHeaderReader(response);

            var stringBody = await response.Content.ReadAsStringAsync();

            var setting = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var data = JsonConvert.DeserializeObject<List<ZoteroReturnData<T>>>(stringBody, setting);

            return new ZoteroApiResponseMany<T>(meta, data, response.RequestMessage.RequestUri.ToString());
        }

        /// <summary>
        /// Another constructor for already parsed metadata, i.e. when loading multiple pages when the first page has already provided the header info.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="data"></param>
        /// <param name="requestUrl"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ZoteroApiResponseMany<T> LoadResponseMany<T>(ZoteroResponseHeaderReader metadata,
            List<ZoteroReturnData<T>> data, string requestUrl)
        {
            return new ZoteroApiResponseMany<T>(metadata, data, requestUrl);
        }

        public async Task<ZoteroApiResponseMany<T>> GetNextPage()
        {
            if (!HasNextPage)
            {
                throw new Exception("No Next Page");
            }

            return await ZoteroApiClient.SendRequestMany<T>(MetaData.Next);
        }
    }
}