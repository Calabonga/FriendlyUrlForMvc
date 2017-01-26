using System.Data.Entity;
using FriendlyUrlForMvc.Data.Base;
using FriendlyUrlForMvc.Models;

namespace FriendlyUrlForMvc.Data {

public interface IContext : IDbContext {

    /// <summary>
    /// System logs
    /// </summary>
    IDbSet<LogItem> Logs { get; set; }

    /// <summary>
    /// SEO Friendly Urls
    /// </summary>
    IDbSet<FriendlyUrl> FriendlyUrls { get; set; }

    /// <summary>
    /// Экспонаты музея
    /// </summary>
    IDbSet<EditablePage> EditablePages { get; set; }
}
}
