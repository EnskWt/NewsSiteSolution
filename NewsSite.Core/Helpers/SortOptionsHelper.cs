using NewsSite.Core.Enums.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.Helpers
{
    public static class SortOptionsHelper
    {
        public static Dictionary<SortAttributes, string> GetSortByOptions()
        {
            return new Dictionary<SortAttributes, string>
            {
                { SortAttributes.Date, "Newest" },
                { SortAttributes.Views, "Most viewed" }
            };
        }
    }
}
