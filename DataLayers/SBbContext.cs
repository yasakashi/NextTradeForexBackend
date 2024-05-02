using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Entities.Systems;
using Entities.DBEntities;

namespace DataLayers
{
    public class SBbContext : DbContext
    {
        private readonly KeyInfo _keyInfo;
        private readonly IEncryptionProvider _provider;

        public SBbContext(DbContextOptions<SBbContext> options) : base(options)
        {
            _keyInfo = new KeyInfo("15KLO2soJhvBwz09kBrMlShkaL40vUSGaqr/WBu3+Va=",
"kh3fn+orShicGDMLksabAQ==");
            _provider = new AesProvider(_keyInfo.Key, _keyInfo.Iv);
        }
        public DbSet<SystemMessage> SystemMessages { get; set; }
        public DbSet<LogType> LogTypes { get; set; }
        //public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<OutSystem> OutSystems { get; set; }
        public DbSet<OutRelationWallet> OutRelationWallets { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }        
        //public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<BlockedIP> BlockedIPs { get; set; }        
        public DbSet<Role> Roles{ get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Person> People { get; set; }

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.UseEncryption(_provider);


            /*
              modelBuilder
            .Entity<PageQuestionAnswer>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("vwPageQuestionAnswer");
            });
            modelBuilder
            .Entity<UserAnswerQuestion>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("vwGetUserAnswerQuestion");
            });
            modelBuilder
            .Entity<ExamAnswerReportDto>(eb =>
            {
                eb.HasNoKey();
            });
            modelBuilder
            .Entity<AcceptedLottotyReportDto>(eb =>
            {
                eb.HasNoKey();
            });


                        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }

             */
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
