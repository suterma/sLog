using System;
using System.Collections.Generic;
using System.Text;

namespace sLog.Models
{
    public class Query
    {
        public string ConnectionString { get; set; }

        public string SelectCommand { get; set; }

        public ICollection<string> Rows { get; set; } = new List<string>();
    }
}

