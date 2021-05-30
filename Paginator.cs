using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Paginator.EntityFrameworkCore
{
    /// <summary>
    /// Static class containing pagination methods.
    /// </summary>
    public static class Paginator
    {
        internal const int DEF_PAGE = 1;
        internal const int DEF_PERPAGE = 10;
        internal const bool DEF_SKIPCOUNT = false;

        /// <summary>
        /// Asynchronously lists items in the pagination request.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="page">Page</param>
        /// <param name="perpage">Items per page.</param>
        /// <param name="skipCount">Specify whether to omit running a count operation on your query againt the data store.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation which you can await.</returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="OperationCanceledException"/>
        public static Task<PagedResult<TEntity>> PaginateAsync<TEntity>(this IQueryable<TEntity> query,
            int page = DEF_PAGE, int perpage = DEF_PERPAGE, bool skipCount = DEF_SKIPCOUNT, CancellationToken token = default)
            where TEntity : class
            => query.ProcessPaginationAsync(page, perpage, skipCount, token);
        /// <summary>
        /// Lists items in the sequence and only returns the specified number.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="page">Page</param>
        /// <param name="perpage">Items per page.</param>
        /// <param name="skipCount">Specify whether to omit running a count operation on your query againt the data store.</param>
        /// <returns>A <see cref="PagedResult{TEntity}"/> response object.</returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        public static PagedResult<TEntity> Paginate<TEntity>(this IQueryable<TEntity> query,
            int page = DEF_PAGE, int perpage = DEF_PERPAGE, bool skipCount = DEF_SKIPCOUNT)
            where TEntity : class
            => query.ProcessPagination(page, perpage, skipCount);

        internal static async Task<PagedResult<TEntity>> ProcessPaginationAsync<TEntity>(this IQueryable<TEntity> query,
                    int page = DEF_PAGE, int perpage = DEF_PERPAGE, bool skipCount = DEF_SKIPCOUNT, CancellationToken token = default)
        {
            ValidateParams_IfInvalid_Throw(page, perpage);

            int total = 0;
            var list = new List<TEntity>();

            token.ThrowIfCancellationRequested();

            if (!skipCount)
                total = await query.CountEntitiesAsync(token);

            if (skipCount || (!skipCount && total > 0))
                list = await query.Skip((page - 1) * perpage).Take(perpage).ToListAsync(token);
            
            if (skipCount)
                total = list.Count;

            return new PagedResult<TEntity>()
            {
                Page = page,
                ItemsPerPage = perpage,
                TotalItems = total,
                TotalPages = CalculateTotalPages(total, perpage),
                Items = list
            };
        }
        internal static PagedResult<TEntity> ProcessPagination<TEntity>(this IQueryable<TEntity> query,
                    int page = DEF_PAGE, int perpage = DEF_PERPAGE, bool skipCount = DEF_SKIPCOUNT)
        {
            ValidateParams_IfInvalid_Throw(page, perpage);

            int total = 0;
            var list = new List<TEntity>();

            if (!skipCount)
                total = query.Count();

            if (skipCount || (!skipCount && total > 0))
                list = query.Skip((page - 1) * perpage).Take(perpage).ToList();

            if (skipCount)
                total = list.Count;

            return new PagedResult<TEntity>()
            {
                Page = page,
                ItemsPerPage = perpage,
                TotalItems = total,
                TotalPages = CalculateTotalPages(total, perpage),
                Items = list
            };
        }

        internal static void ValidateParams_IfInvalid_Throw(int page, int perpage)
        {
            if (page <= 0)
                throw new ArgumentException("Page parameter must be greater than zero.", nameof(page));

            if (perpage < 0)
                throw new ArgumentException("Per-page parameter must be 0 or greater than 0.", nameof(perpage));

            return;
        }
        internal static Task<int> CountEntitiesAsync<TEntity>(this IQueryable<TEntity> query, CancellationToken token = default)
        {
            return query.CountAsync(token);
        }
        internal static int CalculateTotalPages(int totalItems, int perpage)
        {
            int ans = 0;
            if (perpage >= 1)
            {
                ans = totalItems / perpage;
                ans += (totalItems % perpage) > 0 ? 1 : 0;
            }
            return ans;
        }
    }
}