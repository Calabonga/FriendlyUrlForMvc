using System;

namespace FriendlyUrlForMvc.Models {

    /// <summary>
    ///  Audition data
    /// </summary>
    public interface IHaveAudit {

        DateTime CreatedAt { get; set; }

        string CreatedBy { get; set; }

        DateTime UpdatedAt { get; set; }

        string UpdatedBy { get; set; }
    }
}