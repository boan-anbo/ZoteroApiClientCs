using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MyNamespace;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ZoteroApiClientCs.Api;
using ZoteroApiClientCs.Utils;
using ZoteroApiClientCs.ZoteroApiEntities;

namespace ZoteroApiClientCs.Models
{
    public abstract class ZoteroApiResponse<T>
    {
        public ZoteroResponseHeaderReader MetaData;


        public string RequestUrl;

        public bool HasNextPage = false;

        public ZoteroApiResponse(ZoteroResponseHeaderReader metaData, string requestUrl)
        {
            MetaData = metaData;

            RequestUrl = requestUrl;
            LoadMetaData(metaData);
        }




        private void LoadMetaData(ZoteroResponseHeaderReader metaData)
        {
            if (metaData.Next != null)
            {
                HasNextPage = true;
            }
        }

    }
}