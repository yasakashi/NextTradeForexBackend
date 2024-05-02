using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Entities.Systems;
using Entities.DBEntities;

namespace DataLayers
{
    public class LogSBbContext : DbContext
    {
        public LogSBbContext(DbContextOptions<LogSBbContext> options) : base(options)
        {
        }
        public DbSet<LogType> LogTypes { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }

        public DbSet<LoginLog> LoginLogs { get; set; }

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        #endregion

        public SqlConnection GetSqlConnection()
        {
            var adoConnStr = this.Database.GetDbConnection().ConnectionString;
            return new SqlConnection(adoConnStr);

        }
    }
}
