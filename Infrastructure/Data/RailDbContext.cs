using Core.Entities;
using Core.Entities.AppMessages;
using Core.Entities.Compensation;
using Core.Entities.Configuration;
using Core.Entities.ContentPages;
using Core.Entities.Fares;
using Core.Entities.Forms;
using Core.Entities.ManagmentLogger;
using Core.Entities.Messenger;
using Core.Entities.MotUpdates;
using Core.Entities.MyTravel;
using Core.Entities.Notifications;
using Core.Entities.PriceEngine;
using Core.Entities.Push;
using Core.Entities.SpeakerTimer;
using Core.Entities.Stations;
using Core.Entities.Surveys;
using Core.Entities.Translation;
using Core.Entities.VersionCatalog;
using Core.Entities.Vouchers;
using Infrastructure.Data.Config;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Infrastructure.Data;

public class RailDbContext : DbContext
{
    public RailDbContext(DbContextOptions<RailDbContext> options) : base(options)
    {
    }

    public DbSet<ContentPages> ContentPages { get; set; }
    public DbSet<RailCalendar> RailCalendar { get; set; }
    public DbSet<Station> Stations { get; set; }
    public DbSet<Metropolin> Metropolins { get; set; }
    public DbSet<TrainTimeTable> TrainTimeTables { get; set; }
    public DbSet<TrainStationsTimeTable> TrainStationsTimeTables { get; set; }
    public DbSet<DailyTrainsTimeTable> DailyTrainsTimeTables { get; set; }
    public DbSet<DailyTrainStationsTimeTable> DailyTrainStationsTimeTables { get; set; }
    public DbSet<TrainVoucher> TrainVouchers { get; set; }
    public DbSet<TrainStationVoucher> TrainStationVouchers { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Synonym> Synonyms { get; set; }
    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<SystemType> SystemTypes { get; set; }
    public DbSet<PushRegistration> PushRegistrations { get; set; }
    public DbSet<PushNotification> PushNotifications { get; set; }
    public DbSet<SentMessageOld> SentMessagesOld { get; set; }
    public DbSet<PushRouting> PushRouting { get; set; }
    public DbSet<PushNotificationsLog> PushNotificationsLog { get; set; }
    public DbSet<Compensation> Compensations { get; set; }
    public DbSet<Translation> Translations { get; set; }
    public DbSet<URLTranslation> URLTranslations { get; set; }
    public DbSet<AutomationNotification> AutomationNotifications { get; set; }
    public DbSet<NotificationType> NotificationTypes { get; set; }
    public DbSet<RailSchedual> RailScheduals { get; set; }
    public DbSet<VersionCatalog> VersionCatalog { get; set; }
    public DbSet<PageType> PageTypes { get; set; }
    public DbSet<PopUpMessages> PopUpMessages { get; set; }
    public DbSet<TrainWarning> TrainWarnings { get; set; }
    public DbSet<WarningType> WarningType { get; set; }
    public DbSet<NotificationEvent> NotificationEvents { get; set; }
    public DbSet<Otp> Otps { get; set; }
    public DbSet<VoucherErrorCode> VoucherErrorCode { get; set; }
    public DbSet<PushRoutingWeekSchedual> PushRoutingWeekScheduals { get; set; }
    public DbSet<PushNotificationsByDate> PushNotificationsByDate { get; set; }
    public DbSet<PushNotificationsByWeekDay> PushNotificationsByWeekDay { get; set; }
    public DbSet<SurveysData> SurveysData { get; set; }
    public DbSet<SurveysResults> SurveysResults { get; set; }
    public DbSet<ConfigurationParameter> ConfigurationParameter { get; set; }
    public DbSet<EttCodes> EttCodes { get; set; }
    public DbSet<ProfileCodes> ProfileCodes { get; set; }
    public DbSet<FaresVersions> FaresVersions { get; set; }
    public DbSet<StationInfo> StationInfo { get; set; }
    public DbSet<StationImage> StationImage { get; set; }
    public DbSet<StationImageComplete> StationImageComplete { get; set; }
    public DbSet<ParkingCosts> ParkingCosts { get; set; }
    public DbSet<StationInfoTranslation> StationInfoTranslation { get; set; }
    public DbSet<StationGate> StationGate { get; set; }
    public DbSet<StationGateServices> StationGateServices { get; set; }
    public DbSet<StationServices> StationServices { get; set; }
    public DbSet<StationActivityTemplatesTypes> StationActivityTemplatesTypes { get; set; }
    public DbSet<StationActivityHoursTemplates> StationActivityHoursTemplates { get; set; }
    public DbSet<StationActivityHoursTemplatesLine> StationActivityHoursTemplatesLine { get; set; }
    public DbSet<StationGateActivityHours> StationGateActivityHours { get; set; }
    public DbSet<StationGateActivityHoursLines> StationGateActivityHoursLines { get; set; }
    public DbSet<FormsIdThrees> FormsIdThrees { get; set; }
    public DbSet<ExemptionNotices> ExemptionNotices { get; set; }
    public DbSet<MailingList> MailingList { get; set; }
    public DbSet<Meetings> Meetings { get; set; }
    public DbSet<SingleSupplier> SingleSupplier { get; set; }
    public DbSet<Tenders> Tenders { get; set; }
    public DbSet<TenderTypes> TenderTypes { get; set; }
    public DbSet<SentMessages> SentMessages { get; set; }
    public DbSet<Message> Message { get; set; }
    public DbSet<MotConvertion> MotConvertion { get; set; }
    public DbSet<TenderDocuments> TenderDocuments { get; set; }
    public DbSet<RavKav> RavKav { get; set; }
    public DbSet<PlanningAndRates> PlanningAndRates { get; set; }
    public DbSet<TendersUpdates> TendersUpdates { get; set; }
    public DbSet<ManagmentSystemObjects> ManagmentSystemObjects { get; set; }
    public DbSet<ManagmentLog> ManagmentLog { get; set; }
          public DbSet<ClosedStationsAndLines> ClosedStationsAndLines { get; set; }
    public DbSet<InquireTransactions> InquireTransactions { get; set; }
    public DbSet<TrainLocation> TrainLocation { get; set; }



    public DbSet<Profiles> Profiles { get; set; }
    public DbSet<PriceNotes> PriceNotes { get; set; }

    public DbSet<BackgroundImage> BackgroundImage { get; set; }

       public DbSet<EmployeeDataFV> EmployeeDataFV { get; set; }
    public DbSet<TrainDataFV> TrainDataFV { get; set; }
    public DbSet<HandicapDataFV> HandicapDataFV { get; set; }
    public DbSet<EngineDataFV> EngineDataFV { get; set; }
    public DbSet<WagonDataFV> WagonDataFV { get; set; }
    public DbSet<VisaTimesDataFV> VisaTimesDataFV { get; set; }
    public DbSet<StationDataFV> StationDataFV { get; set; }
    public DbSet<Profile_Filtering> Profile_Filtering { get; set; }
    public DbSet<Participant> Participant { get; set; }
    public DbSet<Discussion> Discussion { get; set; }
    public DbSet<TimerUser> TimerUser { get; set; }
    public DbSet<TimerRole> TimerRole { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.ApplyConfiguration(new RailCalendarConfiguration());
        _ = modelBuilder.ApplyConfiguration(new StationConfiguration()).Entity<Station>().ToTable(tb => tb.HasTrigger("station_versioning_trigger"));
        _ = modelBuilder.ApplyConfiguration(new MetropolinConfiguration());
        _ = modelBuilder.ApplyConfiguration(new TrainTimeTableConfiguration());
        _ = modelBuilder.ApplyConfiguration(new TrainStationsTimeTableConfiguration());
        _ = modelBuilder.ApplyConfiguration(new DailyTrainsTimeTableConfiguration());
        _ = modelBuilder.ApplyConfiguration(new DailyTrainStationsTimeTableConfiguration());
        _ = modelBuilder.ApplyConfiguration(new TrainVoucherConfiguration());
        _ = modelBuilder.ApplyConfiguration(new TrainStationVoucherConfiguration());
        _ = modelBuilder.ApplyConfiguration(new SynonymConfiguration());
        _ = modelBuilder.ApplyConfiguration(new LanguageConfiguration());
        _ = modelBuilder.ApplyConfiguration(new ConfigurationConfiguration());
        _ = modelBuilder.ApplyConfiguration(new SystemTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new PushRegistrationConfiguration());
        _ = modelBuilder.ApplyConfiguration(new PushNotificationConfiguration());
        _ = modelBuilder.ApplyConfiguration(new SentMessageOldConfiguration());
        _ = modelBuilder.ApplyConfiguration(new PushRoutingConfiguration());
        _ = modelBuilder.ApplyConfiguration(new PushNotificationsLogConfiguration());
        _ = modelBuilder.ApplyConfiguration(new CompensationConfiguration());
        _ = modelBuilder.ApplyConfiguration(new TranslationConfiguration()).Entity<Translation>().ToTable(tb => tb.HasTrigger("translations_versioning_trigger"));
        _ = modelBuilder.ApplyConfiguration(new URLTranslationConfiguration()).Entity<URLTranslation>().ToTable(tb => tb.HasTrigger("url_translations_versioning_trigger"));
        _ = modelBuilder.ApplyConfiguration(new AutomationNotificationConfiguration());
        _ = modelBuilder.ApplyConfiguration(new NotificationTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new RailSchedualConfiguration());
        _ = modelBuilder.ApplyConfiguration(new VersionCatalogConfiguration());
        _ = modelBuilder.ApplyConfiguration(new PageTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new PopUpMessagesConfiguration()).Entity<PopUpMessages>().ToTable(tb => tb.HasTrigger("popupmessages_versioning_trigger"));
        _ = modelBuilder.ApplyConfiguration(new TrainWarningConfiguration());
        _ = modelBuilder.ApplyConfiguration(new WarningTypeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new NotificationEventConfiguration());
        _ = modelBuilder.ApplyConfiguration(new OtpConfiguration());
        _ = modelBuilder.ApplyConfiguration(new VoucherErrorCodeConfiguration());
        _ = modelBuilder.ApplyConfiguration(new PushRoutingWeekSchedualConfiguration());
        _ = modelBuilder.ApplyConfiguration(new PushNotificationsByDateConfiguration());
        _ = modelBuilder.ApplyConfiguration(new PushNotificationsByWeekDayConfiguration());
        _ = modelBuilder.ApplyConfiguration(new ContentPagesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new SurveysConfiguration());
        _ = modelBuilder.ApplyConfiguration(new SurveysResultsConfiguration());
        _ = modelBuilder.ApplyConfiguration(new ConfigurationParameterConfiguration()).Entity<ConfigurationParameter>().ToTable(tb => tb.HasTrigger("configuration_versioning_trigger"));
        _ = modelBuilder.ApplyConfiguration(new EttCodesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new ProfileCodesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new FaresVersionsConfiguration());
        _ = modelBuilder.ApplyConfiguration(new StationInfoConfiguration());
        _ = modelBuilder.ApplyConfiguration(new StationImageConfiguration());
        _ = modelBuilder.ApplyConfiguration(new StationImageCompleteConfiguration());
        _ = modelBuilder.ApplyConfiguration(new StationInfoTranslationConfiguration());
        _ = modelBuilder.ApplyConfiguration(new StationGateConfiguration());
        _ = modelBuilder.ApplyConfiguration(new StationGateServicesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new StationServicesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new StationActivityTemplatesTypesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new StationActivityHoursTemplatesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new StationActivityHoursTemplatesLineConfiguration());
        _ = modelBuilder.ApplyConfiguration(new StationGateActivityHoursConfiguration());
        _ = modelBuilder.ApplyConfiguration(new StationGateActivityHoursLinesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new FormsIdThreesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new ExemptionNoticesConfiguration()).Entity<ExemptionNotices>().ToTable(tb => { tb.HasTrigger("ExemptionNotices_insert_trigger"); tb.HasTrigger("ExemptionNotices_update_trigger"); });
        _ = modelBuilder.ApplyConfiguration(new MailingListConfiguration());
        _ = modelBuilder.ApplyConfiguration(new MeetingsConfiguration());
        _ = modelBuilder.ApplyConfiguration(new SingleSupplierConfiguration()).Entity<SingleSupplier>().ToTable(tb => { tb.HasTrigger("SingleSupplier_insert_trigger"); tb.HasTrigger("SingleSupplier_update_trigger"); });
        _ = modelBuilder.ApplyConfiguration(new TenderConfiguration()).Entity<Tenders>().ToTable(tb => { tb.HasTrigger("tenders_insert_trigger"); tb.HasTrigger("tenders_update_trigger"); }); ;
        _ = modelBuilder.ApplyConfiguration(new TypesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new SentMessagesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new MessageConfiguration());
        _ = modelBuilder.ApplyConfiguration(new TenderDocumentsConfiguration());
        _ = modelBuilder.ApplyConfiguration(new RavKavConfiguration());
        _ = modelBuilder.ApplyConfiguration(new PlanningAndRatesConfiguration()).Entity<PlanningAndRates>().ToTable(tb => { tb.HasTrigger("Planning_insert_trigger"); tb.HasTrigger("planning_update_trigger"); });
        _ = modelBuilder.ApplyConfiguration(new TendersUpdatesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new ManagmentSystemObjectsConfiguration());
        _ = modelBuilder.ApplyConfiguration(new ManagmentLogConfiguration());
        _ = modelBuilder.ApplyConfiguration(new ClosedStationsAndLinesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new InquireTransactionsConfiguration());
        _ = modelBuilder.ApplyConfiguration(new ProfilesConfiguration());
        _ = modelBuilder.ApplyConfiguration(new PriceNotesConfiguration());

               _ = modelBuilder.ApplyConfiguration(new EmployeeDataFVConfiguration());
        _ = modelBuilder.ApplyConfiguration(new TrainDataFVConfiguration());
        _ = modelBuilder.ApplyConfiguration(new HandicapDataFVConfiguration());
        _ = modelBuilder.ApplyConfiguration(new EngineDataFVConfiguration());
        _ = modelBuilder.ApplyConfiguration(new WagonDataFVConfiguration());
        _ = modelBuilder.ApplyConfiguration(new VisaTimesDataFVConfiguration());
        _ = modelBuilder.ApplyConfiguration(new StationDataFVConfiguration());
        _ = modelBuilder.ApplyConfiguration(new TrainLocationConfiguration());
        _ = modelBuilder.ApplyConfiguration(new Profile_FilteringConfiguration());
        _ = modelBuilder.ApplyConfiguration(new BackgroundConfiguration()).Entity<BackgroundImage>().ToTable(tb => tb.HasTrigger("backgroundImage_versioning_trigger"));
        _ = modelBuilder.ApplyConfiguration(new ParticipantConfiguration());
        _ = modelBuilder.ApplyConfiguration(new DiscussionConfiguration());
        _ = modelBuilder.ApplyConfiguration(new TimerUserConfiguration());
        _ = modelBuilder.ApplyConfiguration(new TimerRoleConfiguration());
    }
}

