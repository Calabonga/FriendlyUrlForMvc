using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Web.Http.ModelBinding;
using Calabonga.OperationResults;

namespace FriendlyUrlForMvc.Web.Infrastructure.QueryParams {

    [ModelBinder(typeof(QueryParamsModelBinder<PagedListQueryParams>))]
    public class PagedListQueryParams : PagedListQueryParamsBase {

        public PagedListQueryParams() {
            SortDefinitions = new Collection<SortDefinition>();
        }

        public ICollection<SortDefinition> SortDefinitions { get; set; }
    }

    public class SortDefinition {

        public string Key { get; set; }

        public ListSortDirection SortDirection { get; set; }
    }
}