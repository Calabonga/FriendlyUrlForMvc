using System;

namespace FriendlyUrlForMvc.Web.Infrastructure.Dto {

    /// <summary>
    /// Data Transfer Object for LogItem
    /// </summary>
    public class LogItemDto {
    
        /// <summary>
        /// Identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Type of the log-item
        /// </summary>
        public string LogType { get; set; }

        /// <summary>
        /// Log message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Creation Date
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Operation Exception Message
        /// </summary>
        public string StackTrace { get; set; }
    }
}
