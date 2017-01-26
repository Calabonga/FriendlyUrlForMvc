using System.Linq;
using AutoMapper;
using Calabonga.PagedListLite;

namespace FriendlyUrlForMvc.Web.Infrastructure.Mapper.Base {

    /// <summary>
    /// Generic converter for IPagedList collections
    /// </summary>
    /// <typeparam name="TMapFrom"></typeparam>
    /// <typeparam name="TMapTo"></typeparam>
    public class PagedListConverter<TMapFrom, TMapTo> : ITypeConverter<PagedList<TMapFrom>, PagedList<TMapTo>> {

        /// <summary>Performs conversion from source to destination type</summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        /// <returns>Destination object</returns>
        public PagedList<TMapTo> Convert(PagedList<TMapFrom> source, PagedList<TMapTo> destination, ResolutionContext context) {
            if (source == null) return null;
            var vm = source.Items.Select(m => context.Mapper.Map<TMapFrom, TMapTo>(m)).ToList();
            return new PagedList<TMapTo>(vm, source.PageIndex, source.PageSize, source.TotalCount);
        }
    }
}