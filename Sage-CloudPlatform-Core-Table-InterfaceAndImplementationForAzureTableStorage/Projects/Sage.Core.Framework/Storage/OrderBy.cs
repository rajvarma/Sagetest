using System;

namespace Sage.Core.Framework.Storage
{
    public enum SortDirection
    {
        Ascending = 0, // ascending is the default value
        Descending
    }

    public class OrderBy
    {
        public String PropertyName { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
