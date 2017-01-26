using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Calabonga.OperationResults;
using Calabonga.PagedListLite;
using FriendlyUrlForMvc.Data;
using FriendlyUrlForMvc.Models;

namespace FriendlyUrlForMvc.Web.Infrastructure.Services.Base {

    /// <summary>
    /// Generic service abstraction as one of the bussiness-logic layer
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class Service<TModel> where TModel : class, IHaveIdentifier {
        private readonly DbSet<TModel> _dbSet;

        protected Service(IContext context, IAppConfig config, IServiceSettings settings, ILogService logger) {
            Settings = settings;
            LogService = logger;
            AppSettings = config.Config;
            Context = context;
            _dbSet = context.Set<TModel>();
        }

        #region Properties

        /// <summary>
        /// Service ovirrided settings
        /// </summary>
        protected IServiceSettings Settings { get; }

        /// <summary>
        /// Application configuration settings
        /// </summary>
        protected CurrentAppSettings AppSettings { get; }

        /// <summary>
        /// ApplicationDbContext for current app
        /// </summary>
        protected IContext Context { get; }

        /// <summary>
        /// Logger for current service
        /// </summary>
        protected ILogService LogService { get; }

        #endregion

        /// <summary>
        /// Returns items with includes by OnDemand mode
        /// </summary>
        public IQueryable<TModel> All(params Expression<Func<TModel, object>>[] includeProperties) {
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
        public IQueryable<TModel> All(FetchMode mode = FetchMode.Simple) {
            return mode == FetchMode.Simple ? _dbSet.AsQueryable() : AttachIncludes(_dbSet);
        }

        protected virtual IQueryable<TModel> AttachIncludes(IQueryable<TModel> items) {
            return _dbSet.AsQueryable();
        }

        /// <summary>
        /// Returns filtered data
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        public virtual IQueryable<TModel> FilterData(IPagedListQueryParams queryParams) {
            return Settings.IncludeWhenPagedListRequested ? All(FetchMode.Expanded) : All();
        }

        /// <summary>
        /// Returns Delete operation result
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public OperationResult<bool> Delete(TModel item) {
            var operation = new OperationResult<bool>();
            Context.Set<TModel>().Remove(item);
            try {
                Context.SaveChanges();
                operation.Result = true;
            }
            catch (Exception exception) {
                operation.Error = exception;
                LogService.LogError(operation.Error);
            }

            return operation;
        }

        /// <summary>
        /// Returns paged colletion by pageIndex
        /// </summary>
        /// <param name="queryParams"></param>
        /// <param name="items">Items for paging</param>
        /// <returns></returns>
        public virtual IPagedList<TModel> PagedItems(IPagedListQueryParams queryParams, IQueryable<TModel> items = null) {
            var pageSizeFromSettings = Settings.PageSizeForPagedList ?? AppSettings.DefaultPagerSize;
            var pageSize = queryParams.PageSize == 0 ? pageSizeFromSettings : queryParams.PageSize;
            var all = items ?? All();
            var paged = all.TakePage(queryParams.PageIndex, pageSize);
            return paged;
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
                : _dbSet.SingleOrDefault(x => x.Id == id);
            var error = item == null ? new KeyNotFoundException(string.Format($"Could not be found entity with Id {0}", id)) : null;
            var result = new OperationResult<TModel>();
            if (item != null) {
                result.Result = item;
                return result;
            }
            result.Error = error;
            LogService.LogError(result.Error);
            return result;
        }

        #region private methods helpers


        /// <summary>
        /// Returns ViewModel wrapped to RequestResult
        /// </summary>
        /// <param name="item"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private OperationResult<TModel> CreateRequestResultAsViewModel(TModel item, Exception error = null) {
            var result = new OperationResult<TModel>();
            if (item != null) {
                result.Result = item;
            }
            result.Error = error;
            LogService.LogError(result.Error);
            return result;
        }

        #endregion
    }
}