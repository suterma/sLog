using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace sLog.Models
{
    public class Registration
    {
        /// <summary>
        ///     Gets or sets the registration token.
        /// </summary>
        /// <value>
        ///     The registration token.
        /// </value>
        public Guid RegistrationToken { get; set; }

        /// <summary>
        ///     Gets or sets the description for this Registration.
        /// </summary>
        /// <remarks>This could be a device name or serial number.</remarks>
        /// <value>
        ///     The description.
        /// </value>
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the registration identifier.
        /// </summary>
        /// <value>
        ///     The registration identifier.
        /// </value>
        [Key]
        public int RegistrationId { get; set; }

        /// <summary>
        ///     Gets or sets the e mail address.
        /// </summary>
        /// <value>
        ///     The e mail address.
        /// </value>
        [BindRequired]
        [DataType(DataType.EmailAddress)]
        public string EMailAddress { get; set; }

        /// <summary>
        ///     Gets or sets the logs.
        /// </summary>
        /// <value>
        ///     The logs.
        /// </value>
        public ICollection<Log> Logs { get; set; }
    }
}