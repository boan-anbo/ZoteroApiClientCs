using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Flurl;
using Xunit;
using Xunit.Abstractions;
using ZoteroApiClientCs.ZoteroApiEntities;

namespace ZoteroApiClientCs
{
    public class QueryBuilder
    {
        public static string GetQuery(string url, ZoteroApiParameters param)
        {
            var result = url.SetQueryParams(param);
            return result.ToString();
        }
    }

    public class ZoteroApiParameters
    {
        [Range(0, 100)] public int limit { get; set; } = 100;

        public int start { get; set; } = 0;

        public ItemType? itemType { set; get; }
    }

    public class Tests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Tests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public  void TestGetQuery()
        {
            var result = QueryBuilder.GetQuery("Htp: fewafew", new ZoteroApiParameters()
            {
                limit = 30
            });
            _testOutputHelper.WriteLine(result);
        }
    }
}