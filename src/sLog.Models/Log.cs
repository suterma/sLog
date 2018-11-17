using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        [Key]
        public Guid LogId { get; set; }

        /// <summary>
        ///     Gets or sets the timestamp.
        /// </summary>
        /// <value>
        ///     The timestamp.
        /// </value>
        [BindRequired]
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
        [Display(Name = "MIME type")]
        [DataType(DataType.Text)]
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the registration identifier.
        /// </summary>
        /// <value>
        /// The registration identifier.
        /// </value>
        [Display(Name = "Registration")]
        [Required(ErrorMessage = "Please reference an existing registration.")]
        public int RegistrationId { get; set; }

        /// <summary>
        ///     Gets or sets the registration.
        /// </summary>
        /// <remarks>References the Registration Token, the log entry was created with.</remarks>
        /// <value>
        ///     The registration.
        /// </value>
        [Required(ErrorMessage = "Please reference an existing registration.")]
        public virtual Registration Registration { get; set; }
    }
}