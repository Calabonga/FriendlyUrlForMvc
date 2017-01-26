using System;
using System.Collections.Generic;

namespace FriendlyUrlForMvc.Data.Base {
    /// <summary>
    /// DbContext operation wrapper
    /// </summary
    public class SaveChangesResult {
        private readonly List<string> _messages;

        /// <summary>
        /// The list of the messages registered while SaveChanges operation execution
        /// </summary>
        public List<string> Messages {
            get { return _messages; }
        }

        public SaveChangesResult() {
            _messages = new List<string>();
        }

        public SaveChangesResult(string message) : this() {
            AddMessage(message);
        }

        public Exception Exception { get; set; }

        public bool IsOk => Exception != null;

        public void AddMessage(string message) {
            Messages.Add($"{DateTime.Now}: {message}");
        }
    }
}