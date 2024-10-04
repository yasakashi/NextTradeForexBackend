using Entities.DBEntities;
using Entities.Dtos;
using Entities.Systems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using System.Data.SqlClient;

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
        public DbSet<PartnerType> PartnerTypes { get; set; }

        //public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFinancialInstrument> UserFinancialInstruments { get; set; }
        public DbSet<UserTrainingmethod> UserTrainingmethods { get; set; }
        public DbSet<UserTargetTrainer> UserTargetTrainers { get; set; }


        public DbSet<SiteMessage> SiteMessages { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        //public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<BlockedIP> BlockedIPs { get; set; }
        public DbSet<Role> Roles { get; set; }
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
        public DbSet<ReactionType> ReactionTypes { get; set; }

        public DbSet<ForexExperienceLevel> ForexExperienceLevels { get; set; }
        public DbSet<TrainingMethod> TrainingMethods { get; set; }
        public DbSet<FinancialInstrument> FinancialInstruments { get; set; }
        public DbSet<InterestForex> InterestForexs { get; set; }

        #region [ ForumMessage ]
        public DbSet<ForumMessage> ForumMessages { get; set; }
        public DbSet<ForumMessageCategory> ForumMessageCategorys { get; set; }
        public DbSet<ForumMessageReaction> ForumMessageReactions { get; set; }
        public DbSet<CommunityGroupsMessage> CommunityGroupsMessages { get; set; }

        #endregion [ ForumMessage ]
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

        public DbSet<CourseBuilder> CourseBuilders { get; set; }
        public DbSet<CourseQuestionAnswer> CourseQuestionsAnswers { get; set; }
        public DbSet<CourseLesson> CourseLessons { get; set; }
        public DbSet<CourseMeet> CourseMeets { get; set; }
        public DbSet<CourseVideo> CourseVideos { get; set; }
        public DbSet<CourseVideoFile> CourseVideoFiles { get; set; }
        public DbSet<CoursePDF> CoursePDFs { get; set; }
        public DbSet<CoursePDFFile> CoursePDFFiles { get; set; }
        public DbSet<CourseMemeber> CourseMemebers { get; set; }
        public DbSet<CourseLessonFile> CourseLessonFiles { get; set; }
        public DbSet<CourseMeetFile> CourseMeetFiles { get; set; }

        public DbSet<CourseTopic> CoursTopics { get; set; }
        #endregion [ Course ]

        #region [  TicketType ]
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        #endregion [  TicketType ]

        #region [ Wallet ]
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        #endregion [ Wallet ]

        #region [ Gallery ]
        public DbSet<CommunityGroupGallery> CommunityGroupGalleries { get; set; }
        public DbSet<CommunityGroupGalleryFile> CommunityGroupGalleryFiles { get; set; }
        public DbSet<CommunityGroupGalleryType> CommunityGroupGalleryTypes { get; set; }
        public DbSet<CommunityGroupGalleryAccessLevel> CommunityGroupGalleryAccessLevels { get; set; }
        #endregion [ Gallery ] 

        #region [ Market Puls Forex ]
        public DbSet<URLSection> URLSections { get; set; }
        public DbSet<PDFSection> PDFSections { get; set; }
        public DbSet<TechnicalBreakingNews> TechnicalBreakingNewss { get; set; }
        public DbSet<TechnicalTabs> TechnicalTabss { get; set; }
        public DbSet<NewsMainContent> NewsMainContents { get; set; }
        public DbSet<FundamentalNewsSection> FundamentalNewsSections { get; set; }
        public DbSet<FirstCountryData> FirstCountryDatas { get; set; }
        public DbSet<FlexibleBlock> FlexibleBlocks { get; set; }
        public DbSet<SecondCountryData> SecondCountryDatas { get; set; }
        public DbSet<forex> Forexs { get; set; }
        #endregion [ Market Puls Forex ]

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<ForumMessageReactionDto>(eb =>{    eb.HasNoKey();    eb.ToView("vwPageQuestionAnswer"); });

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
