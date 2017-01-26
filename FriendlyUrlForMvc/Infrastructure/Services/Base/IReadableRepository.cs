using System;
using System.Linq;
using System.Linq.Expressions;
using Calabonga.OperationResults;
using Calabonga.PagedListLite;
using FriendlyUrlForMvc.Data;

namespace FriendlyUrlForMvc.Web.Infrastructure.Services.Base {

    /// <summary>
    /// Generic service abstraction for read only operation with entity
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TQueryParams"></typeparam>
    public interface IReadableRepository<TModel, in TQueryParams>
        where TQueryParams : IPagedListQueryParams {


        /// <summary>
        /// Service ovirrided settings
        /// </summary>
        IServiceSettings Settings { get; }

        /// <summary>
        /// ApplicationDbContext for current app
        /// </summary>
        IContext Context { get; }

        /// <summary>
        /// Returns paged colletion by pageIndex
        /// </summary>
        /// <param name="queryParams"></param>
        /// <param name="orderPredecat"></param>
        /// <param name="items">Items for paging</param>
        /// <param name="fetchMode"></param>
        /// <returns></returns>
        OperationResult<PagedList<TResult>> PagedResult<TSortType, TResult>(TQueryParams queryParams, Expression<Func<TModel, TSortType>> orderPredecat, IQueryable<TModel> items, FetchMode fetchMode);

        /// <summary>
        /// Returns paged colletion by pageIndex
        /// </summary>
        /// <param name="queryParams"></param>
        /// <param name="orderPredecat"></param>
        /// <returns></returns>
        OperationResult<PagedList<TResult>> PagedResult<TSortType, TResult>(TQueryParams queryParams, Expression<Func<TModel, TSortType>> orderPredecat);


        /// <summary>
        /// Returns paged colletion by pageIndex
        /// </summary>
        /// <typeparam name="TSortType"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        OperationResult<PagedList<TResult>> PagedResult<TSortType, TResult>(TQueryParams queryParams);

        /// <summary>
        /// Returns paged colletion by pageIndex
        /// </summary>
        /// <typeparam name="TSortType"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryParams"></param>
        /// <param name="fetchMode"></param>
        /// <returns></returns>
        OperationResult<PagedList<TResult>> PagedResult<TSortType, TResult>(TQueryParams queryParams, FetchMode fetchMode);

        /// <summary>
        /// Returns request result with Model of the entity this service
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        OperationResult<TModel> GetById(int id, bool includes = false);


        /// <summary>
        /// Filter data by <see cref="TQueryParams"/>
        /// </summary>
        /// <param name="queryParams"></param>
        /// <param name="mode">Include or not navigation properties</param>
        /// <returns></returns>
        IQueryable<TModel> FilterData(TQueryParams queryParams, FetchMode mode = FetchMode.Simple);
    }
}