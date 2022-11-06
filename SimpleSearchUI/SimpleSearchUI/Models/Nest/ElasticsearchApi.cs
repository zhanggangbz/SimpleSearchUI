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

            var settings = new ConnectionSettings(new Uri("http://192.168.0.190:9200"));

            ElasticClient = new ElasticClient(settings);
        }

        public SearchRespone SearchData(string key,int pageIndex,string indexName)
        {
            int startIndex = pageIndex * 10 - 10;

            var watch = Stopwatch.StartNew();

            var kps = key.Split(' ');

            QueryContainer wholeWordQuery = null;

            foreach (var k1 in kps)
            {
                if (!string.IsNullOrWhiteSpace(k1))
                {
                    wholeWordQuery |= new MatchPhraseQuery() { Field = "content", Query = k1 };
                    wholeWordQuery |= new MatchPhraseQuery() { Field = "file.filename.text", Query = k1 };
                }
            }

            var rs = ElasticClient.Search<ESTest>(s => s
                .Query(q => wholeWordQuery).From(startIndex)
                .Index(indexName)
                .Size(10)
                .Highlight(h => h
                    .PreTags("<strong class=\"text-danger\">")
                    .PostTags("</strong>")
                    .Fields(fh => fh.Field(fw => fw.Content), fh => fh.Field(fw => fw.File.filename.text))
                )
            );

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

                sitem.ShowFileName = item.Source.File.Filename;

                if (item.Highlight!=null && item.Highlight.Count > 0)
                {
                    if (item.Highlight.ContainsKey("content"))
                    {
                        foreach (var item2 in item.Highlight["content"])
                        {
                            sitem.ShowContent += item2;
                        }
                    }

                    if (item.Highlight.ContainsKey("file.filename.text"))
                    {
                        foreach (var k1 in kps)
                        {
                            if (!string.IsNullOrWhiteSpace(k1))
                            {
                                sitem.ShowFileName = sitem.ShowFileName.Replace(k1,$"<strong class=\"text-danger\">{k1}</strong>");
                            }
                        }
                    }
                }
                result.CurrentDatas.Add(sitem);
            }

            return result;
        }

        public List<string> GetAllIndexName()
        {
            List<string> list = new List<string>();
            var rs = ElasticClient.Cat.Indices();

            if(rs != null && rs.Records!=null)
            {
                foreach(var item in rs.Records)
                {
                    if(item!=null && !item.Index.Contains('.') && !item.Index.Contains("_folder"))
                    {
                        list.Add(item.Index);
                    }
                }
            }

            return list;
        }
    }
}
