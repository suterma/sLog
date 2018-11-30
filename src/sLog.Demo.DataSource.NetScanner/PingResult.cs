using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace sLog.Demo.DataSource.NetScanner
{
    /// <summary>
    /// A result of a network ping.
    /// </summary>
    public class PingResult
    {
        /// <summary>
        /// Gets or sets the target IP address as a string.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        /// <devdoc>Using the IPAddress class results in an exception when serializing to JSON, thus the string form is used here.</devdoc>
        public string Target { get; set; }

        /// <summary>
        /// Gets or sets the roundtrip time.
        /// </summary>
        /// <value>
        /// The roundtrip time.
        /// </value>
        public TimeSpan RoundtripTime { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public IPStatus Status { get; set; }
    }
}
