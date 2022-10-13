using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSearchUI.Models.Nest
{
    /// <summary>
    /// ElasticSearch 索引对应的对应类
    /// </summary>
    public class ESTest
    {
        public string Content { get; set; }
        public ESMeta Meta { get; set; }
        public ESFile File { get; set; }
        public ESPath Path { get; set; }
    }

    public class ESMeta
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Format { get; set; } 
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Date { get; set; }
    }

    public class ESFile
    {
        public string Extension { get; set; }
        public string Content_type { get; set; }
        public DateTime Created { get; set; }
        public DateTime Last_modified { get; set; }
        public DateTime Last_accessed { get; set; }
        public DateTime Indexing_date { get; set; }
        public long Filesize { get; set; }
        public string Filename { get; set; }
        public string Url { get; set; }
    }

    public class ESPath
    {
        public string Root { get; set; }
        public string Virtual { get; set; }
        public string Real { get; set; }
    }

}
