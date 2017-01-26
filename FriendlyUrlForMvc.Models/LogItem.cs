using System;

namespace FriendlyUrlForMvc.Models {

    /// <summary>
    /// Log item
    /// </summary>
    public class LogItem : IHaveIdentifier {

        /// <summary>
        /// Type of the log-item
        /// </summary>
        public LogType LogType { get; set; }

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

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Identifier for entity
        /// </summary>
        public int Id { get; set; }
    }
}
