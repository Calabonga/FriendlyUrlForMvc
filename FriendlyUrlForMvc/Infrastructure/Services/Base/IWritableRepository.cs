using System;
using Calabonga.OperationResults;
using FriendlyUrlForMvc.Models;

namespace FriendlyUrlForMvc.Web.Infrastructure.Services.Base {
    /// <summary>
    /// Generic service abstraction for writable view model operation with entity
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TUpdateModel"></typeparam>
    /// <typeparam name="TCreateModel"></typeparam>
    /// <typeparam name="TQueryParams"></typeparam>
    public interface IWritableRepository<TModel, TUpdateModel, TCreateModel, in TQueryParams> : IReadableRepository<TModel, TQueryParams>
        where TModel : class, IHaveIdentifier
        where TUpdateModel : class, IHaveIdentifier
        where TCreateModel : class
        where TQueryParams : IPagedListQueryParams {
        /// <summary>
        /// Returns OpertaionResult with just added entity wrapped to ViewModel
        /// </summary>
        /// <param name="model">CreateViewModel with data for new object</param>
        /// <param name="funcBeforeAddExecuted"></param>
        /// <param name="actionAfterSaveChanges"></param>
        /// <returns></returns>
        OperationResult<TModel> Add(TCreateModel model, Func<TModel, TCreateModel, OperationResult<TModel>> funcBeforeAddExecuted, Action<TModel> actionAfterSaveChanges);

        /// <summary>
        /// Returns OperationResult with flag about action success
        /// </summary>
        /// <param name="model">ViewModel with date for update entity</param>
        /// <param name="fetchMode">include navigation properties mode</param>
        /// <returns></returns>
        OperationResult<TModel> Update(TUpdateModel model, FetchMode fetchMode = FetchMode.Simple);

        /// <summary>
        /// Retuns viewModel for editing view for entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OperationResult<TUpdateModel> GetEditById(int id);

        /// <summary>
        /// Returns Delete operation result
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        OperationResult<bool> Delete(TModel item);
    }
}