using SimpleSearchUI.Models.Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleSearchUI.Models
{
    public class SearchRespone
    {
        public string SearchKey { get; set; }

        public long Count { get; set; }

        public long PageCount { get; set; }

        public long CurrentPageIndex { get; set; }

        public List<SearchItem> CurrentDatas { get; set; }

        public double TimeSpeed { get; set; }
    }

    public class SearchItem
    {
        public string ShowContent { get; set; }

        public ESTest Data { get; set; }
    }
}
