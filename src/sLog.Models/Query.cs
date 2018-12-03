using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Data;

namespace sLog.Models
{
    public class Query
    {
        [BindRequired]
        public string ConnectionString { get; set; }

        [BindRequired]
        public string SelectCommand { get; set; }

        /// <summary>
        ///     Gets or sets the resulting data set from the query.
        /// </summary>
        /// <value>
        ///     The data set.
        /// </value>
        [BindNever]
        public DataTable DataSet { get; set; }

        /// <summary>
        ///     Gets or sets the resulting data set from the query.
        /// </summary>
        /// <value>
        ///     The data set.
        /// </value>
                 [BindNever]
        public DataTable TableSchema { get; set; }
    }
}