using System.Collections.Generic;

namespace Paginator.EntityFrameworkCore
{
    /// <summary>
    /// Represents a pagination result object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResult<T> 
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PagedResult()
        {
            Items = new List<T>();
        }
        /// <summary>
        /// Current page in pagination
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Total number of items in every page as per
        /// pagination request. Defaults to 10.
        /// </summary>
        public int ItemsPerPage { get; set; }
        /// <summary>
        /// Number of pages used to paginate collection with
        /// each page containing (x) items per page
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// Number of items matching your pagination request
        /// </summary>
        public int TotalItems { get; set; }
        /// <summary>
        /// Collection containing items in the current page.
        /// </summary>
        public IList<T> Items { get; set; }

        /// <summary>
        /// Calculates &amp; returns the hashcode of the current object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return System.HashCode.Combine(Page, ItemsPerPage, TotalPages, TotalItems, Items);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Page: {0:N0} Perpage: {1:N0} Totalpages: {2:N0} TotalItems: {3:N0}", Page, ItemsPerPage, TotalPages, TotalItems);
        }
    }
}