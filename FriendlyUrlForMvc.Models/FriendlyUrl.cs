using System.ComponentModel.DataAnnotations;

namespace FriendlyUrlForMvc.Models {

/// <summary>
/// Friendly Url for SEO Management
/// </summary>
public class FriendlyUrl {

    public int Id { get; set; }

    [RegularExpression(@"^[a-z0-9-]+$")]
    [Display(Name = "SEO friendly url: only lowercase, number and dash (-) character allowed")]
    public string Permalink { get; set; }

    public string ControllerName { get; set; }

    public string ActionName { get; set; }

    public int? PageId { get; set; }

    public virtual EditablePage Page { get; set; }
}
}
