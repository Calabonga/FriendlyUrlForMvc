using System;
using System.Collections.Generic;
using System.Linq;
using FriendlyUrlForMvc.Data;
using FriendlyUrlForMvc.Models;

namespace FriendlyUrlForMvc.Web.Infrastructure {

    /// <summary>
    /// This is a Singleton (anti-pattern) implementation for extremely fast reading from database.
    /// And caching resolved URLs
    /// </summary>
    public class FriendlyUrlProvider {

        private static readonly Lazy<FriendlyUrlProvider> Instance = new Lazy<FriendlyUrlProvider>(() => new FriendlyUrlProvider());
        private static readonly Dictionary<string, FriendlyUrl> CachedUrl = new Dictionary<string, FriendlyUrl>();

        private FriendlyUrlProvider() { }

        /// <summary>
        /// Single instance
        /// </summary>
        public static FriendlyUrlProvider Default {
            get { return Instance.Value; }
        }

        /// <summary>
        /// Returns an instance from cache or from database (if not found) of the FriendlyPage
        /// </summary>
        /// <param name="url">url name for filtering</param>
        /// <returns></returns>
        public FriendlyUrl GetPageByFriendlyUrl(string url) {
            FriendlyUrl page;

            if (CachedUrl.ContainsKey(url)) {
                return CachedUrl[url];
            }

            using (var context = new ApplicationDbContext()) {
                var urlParams = url.TrimEnd('/');
                page = context.FriendlyUrls.SingleOrDefault(x => x.Permalink.ToLower() == urlParams.ToLower());
                if (page != null) {
                    context.Entry(page).Reference(x => x.Page).Load();
                    CachedUrl.Add(url, page);
                }
            }
            return page;
        }

        /// <summary>
        /// Returns an instance from cache or from database (if not found) of the FriendlyPage
        /// </summary>
        /// <param name="id">id as parameter for filtering</param>
        /// <returns></returns>
        public FriendlyUrl GetPageByFriendlyId(int id) {
            FriendlyUrl page;
            if (CachedUrl.ContainsKey(id.ToString())) {
                return CachedUrl[id.ToString()];
            }
            using (var context = new ApplicationDbContext()) {
                page = context.FriendlyUrls.SingleOrDefault(x => x.PageId == id);
                if (page != null) {
                    context.Entry(page).Reference(x => x.Page).Load();
                    CachedUrl.Add(id.ToString(), page);
                }
            }
            return page;
        }
    }
}