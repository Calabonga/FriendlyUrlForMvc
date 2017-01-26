using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.PagedListLite;
using FriendlyUrlForMvc.Data;
using FriendlyUrlForMvc.Models;

namespace FriendlyUrlForMvc.Web.Infrastructure.Services.Base {

    /// <summary>
    /// Generic service abstraction as one of the bussiness-logic layer
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TQueryParams"></typeparam>
    public abstract class ReadableRepositoryBase<TModel, TQueryParams> : IReadableRepository<TModel, TQueryParams>
        where TModel : class, IHaveIdentifier
        where TQueryParams : IPagedListQueryParams {

        protected ReadableRepositoryBase(IContext context, IAppConfig config, IMapper mapper, IServiceSettings settings, ILogService logger) {
            Settings = settings;
            LogService = logger;
            Mapper = mapper;
            AppSettings = config.Config;
            Context = context;
        }

        #region Properties

        /// <summary>
        /// Service ovirrided settings
        /// </summary>
        public IServiceSettings Settings { get; }

        /// <summary>
        /// Application configuration settings
        /// </summary>
        protected CurrentAppSettings AppSettings { get; }

        /// <summary>
        /// Instance of the IMapper
        /// </summary>
        protected IMapper Mapper { get; }

        /// <summary>
        /// ApplicationDbContext for current app
        /// </summary>
        public IContext Context { get; }

        protected ILogService LogService { get; }

        #endregion

        /// <summary>
        /// In this place you can attach navigation properties
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        protected virtual IQueryable<TModel> AttachIncludes(IQueryable<TModel> items) {
            return items;
        }

        /// <summary>
        /// Returns filtered data
        /// </summary>
        /// <param name="queryParams"></param>
        /// <param name="fetchMode">include or not navigation properties</param>
        /// <returns></returns>
        public virtual IQueryable<TModel> FilterData(TQueryParams queryParams, FetchMode fetchMode = FetchMode.Simple) {
            return All(fetchMode);
        }

        /// <summary>
        /// Returns Model of the entity this service
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual OperationResult<TModel> GetById(int id, bool includes = false) {
            var item = includes
                ? All(FetchMode.Expanded).SingleOrDefault(x => x.Id == id)
                : Context.Set<TModel>().SingleOrDefault(x => x.Id == id);
            var error = item == null ? new KeyNotFoundException($"Could not found entit{0}") : null;
            return CreateRequestResultAsModel(item, error);
        }

        /// <summary>
        /// Returns paged colletion by pageIndex
        /// </summary>
        /// <param name="queryParams"></param>
        /// <param name="orderPredecat"></param>
        /// <param name="items">Items for paging</param>
        /// <param name="fetchMode">include or no navigation properties</param>
        /// <returns></returns>
        public virtual OperationResult<PagedList<TResult>> PagedResult<TSortType, TResult>(TQueryParams queryParams, Expression<Func<TModel, TSortType>> orderPredecat, IQueryable<TModel> items, FetchMode fetchMode) {
            return PagedOperationResult<TSortType, TResult>(queryParams, orderPredecat, items, fetchMode);
        }

        /// <summary>
        /// Returns paged colletion by pageIndex
        /// </summary>
        /// <param name="queryParams"></param>
        /// <param name="items">Items for paging</param>
        /// <param name="fetchMode">include or no navigation properties</param>
        /// <returns></returns>
        public virtual OperationResult<PagedList<TResult>> PagedResult<TSortType, TResult>(TQueryParams queryParams, IQueryable<TModel> items, FetchMode fetchMode = FetchMode.Simple) {
            return PagedOperationResult<TSortType, TResult>(queryParams, null, items, fetchMode);
        }

        /// <summary>
        /// Returns paged colletion by pageIndex
        /// </summary>
        /// <param name="queryParams"></param>
        /// <param name="fetchMode">include or no navigation properties</param>
        /// <returns></returns>
        public virtual OperationResult<PagedList<TResult>> PagedResult<TSortType, TResult>(TQueryParams queryParams, FetchMode fetchMode) {
            return PagedOperationResult<TSortType, TResult>(queryParams, null, null, fetchMode);
        }

        /// <summary>
        /// Returns paged colletion by pageIndex. FetchMode Simple uses.
        /// </summary>
        /// <param name="queryParams"></param>
        /// <param name="orderPredecat"></param>
        /// <returns></returns>
        public virtual OperationResult<PagedList<TResult>> PagedResult<TSortType, TResult>(TQueryParams queryParams, Expression<Func<TModel, TSortType>> orderPredecat) {
            return PagedOperationResult<TSortType, TResult>(queryParams, orderPredecat);
        }

        /// <summary>
        /// Returns paged colletion by pageIndex. FetchMode Simple uses.
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        public virtual OperationResult<PagedList<TResult>> PagedResult<TSortType, TResult>(TQueryParams queryParams) {
            return PagedOperationResult<TSortType, TResult>(queryParams);
        }

        #region private methods helpers

        /// <summary>
        /// Returns paged colletion by pageIndex
        /// </summary>
        /// <param name="queryParams"></param>
        /// <param name="orderPredecat"></param>
        /// <param name="items">Items for paging</param>
        /// <param name="fetchMode">include or no navigation properties</param>
        /// <returns></returns>
        private OperationResult<PagedList<TResult>> PagedOperationResult<TSortType, TResult>(TQueryParams queryParams, Expression<Func<TModel, TSortType>> orderPredecat = null, IQueryable<TModel> items = null, FetchMode fetchMode = FetchMode.Simple) {
            var pageSizeFromSettings = Settings.PageSizeForPagedList ?? AppSettings.DefaultPagerSize;
            var pageSize = queryParams.PageSize == 0 ? pageSizeFromSettings : queryParams.PageSize;
            var all = items ?? FilterData(queryParams, fetchMode);
            if (orderPredecat != null) {
                var pagedDefaultSort = all.AsNoTracking().TakePage(queryParams.PageIndex, pageSize, SortDirection.Descending, orderPredecat);
                if (typeof(TModel) == typeof(TResult)) {
                    return OperationResult.CreateResult((PagedList<TResult>)pagedDefaultSort);
                }
                var pagedDefaultSortMapped = Mapper.Map<PagedList<TResult>>(pagedDefaultSort);
                return OperationResult.CreateResult(pagedDefaultSortMapped);
            }

            var pagedCustomSort = all.AsNoTracking().TakePage(queryParams.PageIndex, pageSize);
            if (typeof(TModel) == typeof(TResult)) {
                return OperationResult.CreateResult((PagedList<TResult>)pagedCustomSort);
            }
            var pagedCustomSortMapped = Mapper.Map<PagedList<TResult>>(pagedCustomSort);
            return OperationResult.CreateResult(pagedCustomSortMapped);
        }

        /// <summary>
        /// Returns items with includes by OnDemand mode
        /// </summary>
        protected IQueryable<TModel> All(params Expression<Func<TModel, object>>[] includeProperties) {
            if (includeProperties == null) {
                var exception = new ArgumentNullException(nameof(includeProperties));
                LogService.LogError(exception);
                throw exception;
            }
            return includeProperties.Aggregate(All().AsQueryable(), (current, includeProperty) => current.Include(includeProperty));
        }

        /// <summary>
        /// Returns items by selected mode with/without Includes of entities
        /// </summary>
        protected IQueryable<TModel> All(FetchMode mode = FetchMode.Simple) {
            return mode == FetchMode.Simple ? Context.Set<TModel>() : AttachIncludes(Context.Set<TModel>());
        }

        /// <summary>
        /// Returns Model wrapped to RequestResult
        /// </summary>
        /// <param name="item"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        protected virtual OperationResult<TModel> CreateRequestResultAsModel(TModel item, Exception error = null) {
            var result = new OperationResult<TModel>();
            if (item != null) {
                result.Result = item;
                return result;
            }
            if (error != null) {
                result.Error = error;
                LogService.LogError(result.Error);
                result.AddError(ExceptionHelper.GetMessages(error));
            }
            return result;
        }
        #endregion

    }
}