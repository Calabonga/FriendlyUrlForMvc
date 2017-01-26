using System.Data.Entity;
using System.Reflection;
using FriendlyUrlForMvc.Data.Base;
using FriendlyUrlForMvc.Models;

namespace FriendlyUrlForMvc.Data {

/// <summary>
/// Main application DbContext
/// </summary>
public class ApplicationDbContext : DataContext, IContext {

        /// <summary>
        /// System logs
        /// </summary>
        public IDbSet<LogItem> Logs { get; set; }

        /// <summary>
        /// EditablePages
        /// </summary>
        public IDbSet<EditablePage> EditablePages { get; set; }

    /// <summary>
    /// SEO Friendly Urls
    /// </summary>
    public IDbSet<FriendlyUrl> FriendlyUrls { get; set; }

    #region overrides and other staff

    public static ApplicationDbContext Create() {
        return new ApplicationDbContext();
    }

    /// <summary>
    /// Maps table names, and sets up relationships between the various user entities
    /// </summary>
    /// <param name="modelBuilder"/>
    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
        modelBuilder.Configurations.AddFromAssembly(Assembly.GetAssembly(GetType()));
        base.OnModelCreating(modelBuilder);
    }

    #endregion
}
}
