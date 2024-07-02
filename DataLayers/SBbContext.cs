using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Entities.Systems;
using Entities.DBEntities;
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
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }

        //public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }        
        //public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<BlockedIP> BlockedIPs { get; set; }        
        public DbSet<Role> Roles{ get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<CommunityGroup> CommunityGroups { get; set; }
        public DbSet<SignalChannel> SignalChannels { get; set; }


        public DbSet<MessageAttachement> MessageAttachements { get; set; }
        public DbSet<CommunityGroupMember> CommunityGroupMembers { get; set; }
        
        //public DbSet<SubCategoryGroup> SubCategoryGroups { get; set; }
        //public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<GroupType> GroupTypes { get; set; }
        
        public DbSet<ForexExperienceLevel> ForexExperienceLevels { get; set; }
        public DbSet<ForumMessage> ForumMessages { get; set; }
        public DbSet<AnalysisType> AnalysisTypes { get; set; }
        public DbSet<PositionType> PositionTypes { get; set; }
        public DbSet<MarketCycle> MarketCycles { get; set; }
        public DbSet<InstrumentType> Instruments { get; set; }
        public DbSet<EntryPointType> EntryPoints { get; set; }
        public DbSet<SignalFileAttachment> SignalFileAttachments { get; set; }
        public DbSet<Signal> Signals { get; set; }

        #region [ Course ]
        public DbSet<CourseLevelType> CourseLevelTypes { get; set; }
        public DbSet<CourseType> CourseTypes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseLesson> CourseLessons { get; set; }
        public DbSet<CourseLessonFile> CourseLessonFiles { get; set; }
        #endregion [ Course ]

        #region [  TicketType ]
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        #endregion [  TicketType ]

        #region [ Wallet ]
        public DbSet<FinancialInstrument> FinancialInstruments { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        #endregion [ Wallet ]

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // ToDo : To Be Fixed Later...
            //builder.UseEncryption(_provider);


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
