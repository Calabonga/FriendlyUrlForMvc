using System;

namespace FriendlyUrlForMvc.Models {
    /// <summary>
    /// Properties for Audition
    /// </summary>
    public abstract class Audit : IHaveAudit {

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}