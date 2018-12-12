using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace sLog.Models
{
    public class ConnectionStringModel
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        [BindRequired]
        [DataType(DataType.Text)]
        [Display(Name = "Database Connection String")]
        public string ConnectionString { get; set; }
    }
}