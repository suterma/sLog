using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sLog.Models
{
    public class Registration
    {
        /// <summary>
        ///     Gets or sets the registration token, the log entry was created with.
        /// </summary>
        /// <value>
        ///     The registration token.
        /// </value>
        public Guid RegistrationToken { get; set; }

        public string RegistrationId { get; set; }

        public string EMailAddress { get; set; }

        public ICollection<Log> Logs { get; set; }

    }
}