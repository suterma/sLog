using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Data;

namespace sLog.Models
{
    /// <summary>
    /// A Model for retrieving Table names.
    /// </summary>
    public class TableNames
    {
        [BindRequired]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the table names.
        /// </summary>
        /// <value>
        /// The table names.
        /// </value>
        public IEnumerable<string> Names { get; set; }

    }
}