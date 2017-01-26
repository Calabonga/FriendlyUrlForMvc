using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Calabonga.OperationResults;
using FriendlyUrlForMvc.Data;
using FriendlyUrlForMvc.Models;

namespace FriendlyUrlForMvc.Web.Infrastructure.Services.Base {

    /// <summary>
    /// Base class for entity update and create operations
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TUpdateModel"></typeparam>
    /// <typeparam name="TCreateModel"></typeparam>
    /// <typeparam name="TQueryParams"></typeparam>
    public abstract class WritableRepositoryBase<TModel, TUpdateModel, TCreateModel, TQueryParams> : ReadableRepositoryBase<TModel, TQueryParams>, IWritableRepository<TModel, TUpdateModel, TCreateModel, TQueryParams>
        where TQueryParams : IPagedListQueryParams
        where TModel : class, IHaveIdentifier
        where TUpdateModel : class, IHaveIdentifier
        where TCreateModel : class {

        public WritableRepositoryBase(IContext context, IAppConfig config, IMapper mapper, IServiceSettings settings, ILogService logger)
            : base(context, config, mapper, settings, logger) {
        }

        /// <summary>
        /// Returns OpertaionResult with just added entity wrapped to ViewModel
        /// </summary>
        /// <param name="model">CreateViewModel with data for new object</param>
        /// <param name="funcBeforeAddExecuted"></param>
        /// <param name="actionAfterSaveChanges"></param>
        /// <returns></returns>
        public OperationResult<TModel> Add(TCreateModel model, Func<TModel, TCreateModel, OperationResult<TModel>> funcBeforeAddExecuted, Action<TModel> actionAfterSaveChanges) {
            var operation = new OperationResult<TModel>();
            var entity = Mapper.Map<TModel>(model);
            var result = funcBeforeAddExecuted?.Invoke(entity, model);
            if (result == null || result.Ok) {
                Context.Set<TModel>().Add(entity);
                try {
                    Context.SaveChanges();
                    actionAfterSaveChanges?.Invoke(entity);
                    operation.Result = entity;
                }
                catch (Exception exception) {
                    operation.AddError(ExceptionHelper.GetMessages(exception));
                    operation.Error = exception;
                    LogService.LogError(operation.Error);
                }
            }
            else {
                if (!result.Ok) {
                    return result;
                }
            }
            return operation;
        }

        public OperationResult<TUpdateModel> GetEditById(int id) {
            var result = new OperationResult<TUpdateModel>();
            var item = All(FetchMode.Expanded).SingleOrDefault(x => x.Id == id);
            if (item == null) {
                result.Error = new KeyNotFoundException($"Entity not found by id {id}");
                LogService.LogError(result.Error);
                return result;
            }

            result.Result = Mapper.Map<TUpdateModel>(item);
            return result;
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

        public OperationResult<TModel> Update(TUpdateModel model, FetchMode fetchMode = FetchMode.Simple) {
            var item = All(fetchMode).SingleOrDefault(x => x.Id == model.Id);
            Mapper.Map(model, item);
            var result = OperationResult.CreateResult<TModel>();
            try {
                Context.SaveChanges();
                result.Result = item;
            }
            catch (Exception exception) {
                LogService.LogError(exception);
                result.Error = exception;
                result.AddError(ExceptionHelper.GetMessages(exception));
            }

            return result;
        }
    }
}