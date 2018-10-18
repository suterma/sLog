using System;
using System.Collections.Generic;

namespace sLog.Models
{
    /// <summary>
    ///     A log entry.
    /// </summary>
    public class Log
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public Guid LogId { get; set; }

        /// <summary>
        ///     Gets or sets the timestamp.
        /// </summary>
        /// <value>
        ///     The timestamp.
        /// </value>
        public DateTime Timestamp { get; set; }

        /// <summary>
        ///     Gets or sets the data.
        /// </summary>
        /// <value>
        ///     The data.
        /// </value>
        public string Data { get; set; }

        /// <summary>
        ///     Gets or sets the MIME type of the data.
        /// </summary>
        /// <remarks>See https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types</remarks>
        /// <value>
        ///     The MIME type of the data.
        /// </value>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the registration.
        /// </summary>
        /// <value>
        /// The registration.
        /// </value>
        public Registration Registration { get; set; }
    }
}