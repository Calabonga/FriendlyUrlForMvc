namespace FriendlyUrlForMvc.Models {

/// <summary>
/// Editable Content
/// </summary>
public class EditablePage : Audit, IHaveIdentifier {

    public int Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string Keywords { get; set; }

    public string Description { get; set; }
}
}
