using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;

namespace FriendlyUrlForMvc.Web.Infrastructure.QueryParams {

    /// <summary>
    /// Generic Model binder for paged collection query.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryParamsModelBinder<T> : IModelBinder {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext) {
            var val = bindingContext.ValueProvider.GetValue("qp");
            if (val != null) {
                var model = JsonConvert.DeserializeObject<T>(val.AttemptedValue);
                bindingContext.Model = model;
                return true;
            }
            return false;
        }
    }
}