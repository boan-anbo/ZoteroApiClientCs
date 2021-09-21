using MyNamespace;
using ZoteroApiClientCs.Api;

namespace ZoteroApiClientCs
{
    public class ZoteroApiUrls
    {
        private string _userId;

        private string _apiRoot;

        public ZoteroApiUrls(string userId)
        {
            this._userId = userId;

            _apiRoot = EndPoints.BaseUrl + "/" + EndPoints.Users + "/" + _userId + "/";
        }

        public string GetUserCollectionsUrl(ZoteroApiParameters p)
        {
            var url = _apiRoot + EndPoints.Collections;
            return AttachParams(url, p);
        }

        private string _GetCollectionItemUrl(string collectionId)
        {
            return _apiRoot + EndPoints.Collections + '/' + collectionId + '/' + "items";
        }

        public string GetItemUrl(string itemKey, ZoteroApiParameters p)
        {
            var url = _apiRoot + EndPoints.Items + '/' + itemKey;
            return AttachParams(url, p);
        }
        
        public string GetCollectionUrl(string itemKey, ZoteroApiParameters p)
        {
            var url = _apiRoot + EndPoints.Collections + '/' + itemKey;
            return AttachParams(url, p);
        }

        public string GetCollectionItemUrl(string collectionId, ZoteroApiParameters p = null)
        {
            var url = _GetCollectionItemUrl(collectionId);
            return AttachParams(url, p);
        }

        /// <summary>
        /// Every method return url with params attached.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private string AttachParams(string url, ZoteroApiParameters p)
        {
            if (p == null)
            {
                /// Provide a default parameters if the user did not provide one
                p = new ZoteroApiParameters();
            }

            return QueryBuilder.GetQuery(url, p);
        }

    }
}