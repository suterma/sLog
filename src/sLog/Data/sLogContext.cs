using Microsoft.EntityFrameworkCore;

namespace sLog.Models
{
    /// <summary>
    ///     The context for sLog.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// <devdoc>
    ///     See
    ///     https://docs.microsoft.com/en-us/ef/core/get-started/aspnetcore/new-db?tabs=visual-studio#create-the-database
    ///     for guidance on creating the database.
    ///     <para>
    ///         Use
    ///         "EntityFrameworkCore\Add-Migration InitialCreate" and "EntityFrameworkCore\Update-Database" to create a new
    ///         database from scratch.
    ///     </para>
    /// </devdoc>
    public class sLogContext : DbContext
    {
        public sLogContext(DbContextOptions<sLogContext> options)
            : base(options)
        {
        }

        public DbSet<Registration> Registration { get; set; }
    }
}