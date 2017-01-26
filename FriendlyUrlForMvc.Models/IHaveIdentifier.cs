namespace FriendlyUrlForMvc.Models {

    /// <summary>
    /// Unique identifier for entity
    /// </summary>
    public interface IHaveIdentifier {

        /// <summary>
        /// Identifier for entity
        /// </summary>
        int Id { get; set; }
    }
}