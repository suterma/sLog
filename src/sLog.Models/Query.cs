using System.Data;

namespace sLog.Models
{
    public class Query
    {
        public string ConnectionString { get; set; }

        public string SelectCommand { get; set; }

        /// <summary>
        ///     Gets or sets the resulting data set from the query.
        /// </summary>
        /// <value>
        ///     The data set.
        /// </value>
        public DataTable DataSet { get; set; }

        /// <summary>
        ///     Gets or sets the resulting data set from the query.
        /// </summary>
        /// <value>
        ///     The data set.
        /// </value>
        public DataTable TableSchema { get; set; }
    }
}