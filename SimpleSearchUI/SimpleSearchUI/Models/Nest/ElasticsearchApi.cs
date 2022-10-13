using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleSearchUI.Models.Nest
{
    public class ElasticsearchApi
    {
        private readonly ILogger<ElasticsearchApi> _logger;

        ElasticClient ElasticClient;

        public ElasticsearchApi(ILogger<ElasticsearchApi> logger)
        {
            _logger = logger;

            var settings = new ConnectionSettings(new Uri("http://192.168.0.190:9200")).DefaultIndex("test");

            ElasticClient = new ElasticClient(settings);
        }

        public SearchRespone SearchData(string key,int pageIndex)
        {
            int startIndex = pageIndex * 10 - 10;

            var watch = Stopwatch.StartNew();

            var rs = ElasticClient.Search((SearchDescriptor<ESTest> s) => s.From(startIndex).Size(10).Query((QueryContainerDescriptor<ESTest> q) => q.MatchPhrase((MatchPhraseQueryDescriptor<ESTest> q1) => q1.Field((ESTest f) => f.Content).Query(key)))
            .Highlight((HighlightDescriptor<ESTest> h) => h.PreTags("<strong class=\"text-danger\">").PostTags("</strong>").Fields((HighlightFieldDescriptor<ESTest> fh) => fh.Field((ESTest fhf) => fhf.Content))));

            watch.Stop();

            SearchRespone result = new SearchRespone()
            {
                SearchKey = key,
                Count = rs.Total,
                PageCount = rs.Total == 0 ? 0 : rs.Total / 10 + 1,
                CurrentPageIndex = pageIndex,
                CurrentDatas = new List<SearchItem>(),
                TimeSpeed = Math.Round(watch.Elapsed.TotalSeconds, 4),
            };

            foreach (var item in rs.Hits)
            {
                var sitem = new SearchItem()
                {
                    Data = item.Source
                };

                if(item.Highlight!=null && item.Highlight.Count > 0)
                {
                    if (item.Highlight.ContainsKey("content"))
                    {
                        foreach (var item2 in item.Highlight["content"])
                        {
                            sitem.ShowContent += item2;
                        }
                    }
                }

                result.CurrentDatas.Add(sitem);
            }

            return result;
        }
    }
}
