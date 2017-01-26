using System.ComponentModel.DataAnnotations;
using FriendlyUrlForMvc.Models;

namespace FriendlyUrlForMvc.Web.Models {
    /// <summary>
    /// EditablePage ViewModel for Updating
    /// </summary>
    public class EditablePageUpdateViewModel : IHaveIdentifier {

        public int Id { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public string Keywords { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
