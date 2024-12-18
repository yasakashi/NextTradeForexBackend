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
        public DbSet<CourseStatus> CourseStatuses { get; set; }
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

        public DbSet<CourseBuilderCourse> CourseBuilderCourses { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<CourseBuilderMeeting> CourseBuilderMeetings { get; set; }
        public DbSet<CourseBuildeVideoPdfUrl> CourseBuildeVideoPdfUrls { get; set; }
        public DbSet<CourseBuilderTopic> CourseBuilderTopics { get; set; }
        public DbSet<CourseBuilderLesson> CourseBuilderLessons { get; set; }
        public DbSet<CourseBuilderLessonFile> CourseBuilderLessonFiles { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<CourseBuilderQuiz> CourseBuilderQuizs { get; set; }
        public DbSet<QuertionType> QuertionTypes { get; set; }

        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<UserLesson> UserLessons { get; set; }

        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOption { get; set; }
        public DbSet<AdvancedSetting> AdvancedSettings { get; set; }



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
        public DbSet<QuizFeedbackMode> QuizFeedbackModes { get; set; }
        public DbSet<QuizQuestionOrder> QuizQuestionOrders { get; set; }
        public DbSet<QuizQuestionLayout> QuizQuestionLayouts { get; set; }
        public DbSet<CourseCustomIntroVideoType> CourseCustomIntroVideoTypes { get; set; }
        public DbSet<Podcast> Podcasts { get; set; }
        public DbSet<PodcastCategory> PodcastCategories { get; set; }
        public DbSet<Webinar> Webinars { get; set; }
        public DbSet<WebinarCategory> WebinarCategories { get; set; }


        #endregion [ Course ]

        #region [ LearnToTrade ]
        public DbSet<TopicType> TopicTypes { get; set; }
        public DbSet<TopicStatus> TopicStatuses { get; set; }
        public DbSet<PDFPageRenderSize> PDFPageRenderSizes { get; set; }
        public DbSet<HardPage> HardPages { get; set; }
        public DbSet<DisplayMode> DisplayModes { get; set; }
        public DbSet<BookSourceType> BookSourceTypes { get; set; }
        public DbSet<AutoEnableSound> AutoEnableSounds { get; set; }
        public DbSet<EnableDownload> EnableDownloads { get; set; }
        public DbSet<LearnToTradeTopic> LearnToTradeTopics { get; set; }
        public DbSet<NewBook> NewBooks { get; set; }

        public DbSet<NewBookCategory> NewBookCategories { get; set; }
        public DbSet<NewBookPageImage> NewBookPageImages { get; set; }
        public DbSet<ForumFormat> ForumFormats { get; set; }
        public DbSet<PageSize> PageSizes { get; set; }
        public DbSet<EnableAutoplayAutomatically> EnableAutoplayAutomaticallies{ get; set; }
        public DbSet<EnableAutoplay> EnableAutoplayes { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<ControlsPosition> ControlsPositions { get; set; }
        public DbSet<SinglePageMode> SinglePageModes { get; set; }
        public DbSet<PageMode> PageModes { get; set; }

        public DbSet<Video> Videos { get; set; }
        public DbSet<VideoCategory> VideoCategories { get; set; }
        public DbSet<VideoSubtitle> VideoSubtitles { get; set; }

        #endregion [ LearnToTrade ]

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
        public DbSet<Forex> Forexs { get; set; }

        #endregion [ Market Puls Forex ]

        #region [  Market Puls Indices ]
        public DbSet<Indice> Indices { get; set; }
        public DbSet<Indice_FundamentalNewsSection> IndiceFundamentalNewsSections { get; set; }
        public DbSet<Indice_NewsMainContent> IndiceNewsMainContents { get; set; }
        public DbSet<Indice_TechnicalTabs> IndiceTechnicalTabss { get; set; }
        public DbSet<Indice_TechnicalTabs_TechnicalBreakingNews> IndiceTechnicalTabsTechnicalBreakingNewss { get; set; }
        public DbSet<Indices_PDFSection> IndicePDFSections { get; set; }
        public DbSet<Indice_URLSection> IndiceURLSections { get; set; }
        public DbSet<Indice_ListedExchange> IndiceListedExchanges { get; set; }
        public DbSet<Indice_RelatedInstument> IndiceRelatedInstuments { get; set; }
        public DbSet<Indice_SectorRepresented> IndiceSectorRepresenteds { get; set; }
        public DbSet<Indice_ChildIndice> IndiceChildIndices { get; set; }
        public DbSet<Indice_AlternateIndice> IndiceAlternateIndices { get; set; }

        #endregion [ Market Puls Indices ]

        #region [ Market Puls Cominidity ]
        public DbSet<Comodities_FlexibleBlock> ComoditiyFlexibleBlocks { get; set; }
        public DbSet<Comodity> Comodities { get; set; }
        public DbSet<ComoditiesCountriesData> ComoditiesCountriesDatas { get; set; }
        public DbSet<ComoditiesFirstCountryDataCountriesData> ComoditiesFirstCountryDataCountriesDatas { get; set; }
        public DbSet<ComoditiesSecondCountryDataCountriesData> ComoditiesSecondCountryDataCountriesDatas { get; set; }
        public DbSet<Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection> Comodities_fundamentalandtechnicaltabsection_fundamentalnewssections { get; set; }
        public DbSet<Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontent> Comodities_fundamentalandtechnicaltabsection_fundamentalnewssection_newsmaincontents { get; set; }
        public DbSet<Comodities_Fundamentalandtechnicaltabsection_TechnicalTabs> Comodities_Fundamentalandtechnicaltabsection_TechnicalTabses { get; set; }
        public DbSet<Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNews> Comodities_FundamentalandTechnicalTabSection_TechnicalTabs_TechnicalBreakingNewses { get; set; }
        public DbSet<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSection> Comodities_FundamentalandTechnicalTabSection_RelatedResorces_PDFSections { get; set; }
        public DbSet<Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSection> Comodities_FundamentalandTechnicalTabSection_RelatedResorces_URLSections { get; set; }

        #endregion   [ Market Puls Cominidity ]

        #region [ Market Puls Crypto ]
        public DbSet<Crypto> Cryptos { get; set; }
        public DbSet<Crypto_FlexibleBlock> CryptoFlexibleBlocks { get; set; }
        public DbSet<CryptoCountryData> CryptoCountryDatas { get; set; }
        public DbSet<CryptoCountriesData> CryptoCountriesDatas { get; set; }
        public DbSet<Crypto_FundamentalandNewsSection> CryptoFundamentalandNewsSections { get; set; }
        public DbSet<Crypto_NewsMainContent> CryptoNewsMainContents { get; set; }
        public DbSet<Crypto_TechnicalTab> CryptoTechnicalTabs { get; set; }
        public DbSet<Crypto_TechnicalBreakingNews> CryptoTechnicalBreakingNewss { get; set; }
        public DbSet<Crypto_FundamentalandNewsSection_RelatedReSorces_PDFSection> CryptoFundamentalandNewsSection_RelatedReSorces_PDFSections { get; set; }
        public DbSet<Crypto_FundamentalandNewsSection_RelatedReSorces_URLSection> CryptoFundamentalandNewsSection_RelatedReSorces_URLSections { get; set; }
        #endregion [ Market Puls Crypto ]

        #region [ Market Puls Stock ]
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockSecondCountryData> StockSecondCountryDatas { get; set; }
        public DbSet<StockFirstCountryData> StockFirstCountryDatas { get; set; }
        public DbSet<StockCountriesData> StockCountriesDatas { get; set; }
        public DbSet<StockFlexibleBlock> StockFlexibleBlocks { get; set; }
        public DbSet<StockManagementTeam> StockManagementTeams { get; set; }
        public DbSet<StockInductryFocus> StockInductryFocuss { get; set; }
        public DbSet<StockProductAndService> StockProductAndServices { get; set; }
        public DbSet<StockURLSection> StockURLSections { get; set; }
        public DbSet<StockPDFSection> StockPDFSections { get; set; }
        public DbSet<StockTechnicalTab> StockTechnicalTabs { get; set; }
        public DbSet<StockNewsMainContent> StockNewsMainContents { get; set; }
        public DbSet<StockTechnicalBreakingNews> StockTechnicalBreakingNewss { get; set; }
        public DbSet<StockFundametalNewsSection> StockFundametalNewsSections { get; set; }
        #endregion [ Market Puls Stock ]

        #region [ Market Puls Forex Chart ]
        public DbSet<ForexChartURLSection> ForexChartURLSections { get; set; }
        public DbSet<ForexChartPDFSection> ForexChartPDFSections { get; set; }
        public DbSet<ForexChartTechnicalBreakingNews> ForexChartTechnicalBreakingNewss { get; set; }
        public DbSet<ForexChartTechnicalTab> ForexChartTechnicalTabs { get; set; }
        public DbSet<ForexChartNewsMainContent> ForexChartNewsMainContents { get; set; }
        public DbSet<ForexChartFundamentalNewsSection> ForexChartFundamentalNewsSections { get; set; }
        public DbSet<ForexChartSecondCountryData> ForexChartSecondCountryDatas { get; set; }
        public DbSet<ForexChartFirstCountryData> ForexChartFirstCountryDatas { get; set; }
        public DbSet<ForexChartCountriesData> ForexChartCountriesDatas { get; set; }        
        public DbSet<ForexChartFlexibleBlock> ForexChartFlexibleBlocks { get; set; }
        public DbSet<ForexChart> ForexCharts { get; set; }

        #endregion [ Market Puls Forex Chart ]

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
