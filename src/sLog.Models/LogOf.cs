using System;
using System.Linq;


namespace sLog.Models
{
    /// <summary>A log of a specific data type.</summary>
    public class LogOf<T> where T : class, new()
    {
        private T item;

        public LogOf(DateTime timeStamp, T item)
        {
            TimeStamp = timeStamp;
            this.item = item;
        }

        public LogOf()
        {
        }

        public T Item { get; set; } = new T();

        public DateTime TimeStamp { get; set; }

        public static IQueryable<LogOf<T>> Filter(IQueryable<Log> filterable)
        {
            return filterable
                .Where(log => log.ContentType.Equals(typeof(T).FullName))
                .Select(log => Create(log));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref=" LogOf<T>"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public static LogOf<T> Create(Log log)
        {
            T item = new T();
            LogOf<T> logOfitem = new LogOf<T>();
            logOfitem.TimeStamp = log.Timestamp;
            logOfitem.Item = item;

            return logOfitem;
        }


        /// <summary>
        /// Creates the specified log.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static LogOf<T> Create(Log log, T item)
        {
            LogOf<T> logOfitem = new LogOf<T>();
            logOfitem.TimeStamp = log.Timestamp;
            logOfitem.Item = item;

            return logOfitem;
        }

        /// <summary>
        /// Creates the specified log.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static LogOf<T> Create(DateTime timeStamp, T item)
        {
            LogOf<T> logOfitem = new LogOf<T>();
            logOfitem.TimeStamp = timeStamp;
            logOfitem.Item = item;

            return logOfitem;
        }







    }


}
