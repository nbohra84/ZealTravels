using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ZealTravel.Domain.Data.Entities;

namespace ZealTravelWebsite.Infrastructure.Context;

public partial class ZealdbNContext : DbContext
{
    public ZealdbNContext()
    {
    }

    public ZealdbNContext(DbContextOptions<ZealdbNContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AddGroupFare> AddGroupFares { get; set; }

    public virtual DbSet<AddGroupFdFare> AddGroupFdFares { get; set; }

    public virtual DbSet<AirExtraCommission> AirExtraCommissions { get; set; }

    public virtual DbSet<AirMarkupCreateGroupSa> AirMarkupCreateGroupSas { get; set; }

    public virtual DbSet<AirMarkupDRuleSa> AirMarkupDRuleSas { get; set; }

    public virtual DbSet<AirMarkupIRuleSa> AirMarkupIRuleSas { get; set; }

    public virtual DbSet<AirMarkupSetCompanySa> AirMarkupSetCompanySas { get; set; }

    public virtual DbSet<AirOffline> AirOfflines { get; set; }

    public virtual DbSet<AirOfflineSupplier> AirOfflineSuppliers { get; set; }

    public virtual DbSet<AirPriceTypeCommissionGroup> AirPriceTypeCommissionGroups { get; set; }

    public virtual DbSet<AirPriceTypeCommissionGroupCommission> AirPriceTypeCommissionGroupCommissions { get; set; }

    public virtual DbSet<AirPriceTypeCommissionGroupCompany> AirPriceTypeCommissionGroupCompanies { get; set; }

    public virtual DbSet<AirlineBaggage> AirlineBaggages { get; set; }

    public virtual DbSet<AirlineBaggageDetail> AirlineBaggageDetails { get; set; }

    public virtual DbSet<AirlineBaggageDetail1> AirlineBaggageDetails1 { get; set; }

    public virtual DbSet<AirlineCallCenter> AirlineCallCenters { get; set; }

    public virtual DbSet<AirlineCancellationReschedulingChargeDetail> AirlineCancellationReschedulingChargeDetails { get; set; }

    public virtual DbSet<AirlineManualChargeAirline> AirlineManualChargeAirlines { get; set; }

    public virtual DbSet<AirlinePnrMakeDay> AirlinePnrMakeDays { get; set; }

    public virtual DbSet<AirlineSsrList> AirlineSsrLists { get; set; }

    public virtual DbSet<AirlinesSector> AirlinesSectors { get; set; }

    public virtual DbSet<AirportCity> AirportCities { get; set; }

    public virtual DbSet<BankCodeList> BankCodeLists { get; set; }

    public virtual DbSet<BankDetail> BankDetails { get; set; }

    public virtual DbSet<BillingTrigg> BillingTriggs { get; set; }

    public virtual DbSet<BookingAirlineLogForPg> BookingAirlineLogForPgs { get; set; }

    public virtual DbSet<BookingCustomerLog> BookingCustomerLogs { get; set; }

    public virtual DbSet<BookingHotelLogForPg> BookingHotelLogForPgs { get; set; }

    public virtual DbSet<BookingsMailSmsDetail> BookingsMailSmsDetails { get; set; }

    public virtual DbSet<CarEntriesAirport> CarEntriesAirports { get; set; }

    public virtual DbSet<CarEntriesLocal> CarEntriesLocals { get; set; }

    public virtual DbSet<CarEntriesOutstation> CarEntriesOutstations { get; set; }

    public virtual DbSet<CarEntry> CarEntries { get; set; }

    public virtual DbSet<CarrierDetail> CarrierDetails { get; set; }

    public virtual DbSet<CommissionDApi> CommissionDApis { get; set; }

    public virtual DbSet<CommissionGroupCompany> CommissionGroupCompanies { get; set; }

    public virtual DbSet<CommissionGroupOnApi> CommissionGroupOnApis { get; set; }

    public virtual DbSet<CommissionGroupOnD> CommissionGroupOnDs { get; set; }

    public virtual DbSet<CommissionGroupOnI> CommissionGroupOnIs { get; set; }

    public virtual DbSet<CommissionIApi> CommissionIApis { get; set; }

    public virtual DbSet<CommissionOnCarrier> CommissionOnCarriers { get; set; }

    public virtual DbSet<CompanyBalanceTransactionDetail> CompanyBalanceTransactionDetails { get; set; }

    public virtual DbSet<CompanyBalanceTransactionDetailEvent> CompanyBalanceTransactionDetailEvents { get; set; }

    public virtual DbSet<CompanyBalanceTransactionHistoryDetail> CompanyBalanceTransactionHistoryDetails { get; set; }

    public virtual DbSet<CompanyBookingsCfee> CompanyBookingsCfees { get; set; }

    public virtual DbSet<CompanyFareDetailAirline> CompanyFareDetailAirlines { get; set; }

    public virtual DbSet<CompanyFareDetailSegmentAirline> CompanyFareDetailSegmentAirlines { get; set; }

    public virtual DbSet<CompanyFlightDetailAirline> CompanyFlightDetailAirlines { get; set; }

    public virtual DbSet<CompanyFlightGstDetail> CompanyFlightGstDetails { get; set; }

    public virtual DbSet<CompanyFlightOwnPnr> CompanyFlightOwnPnrs { get; set; }

    public virtual DbSet<CompanyFlightRejectDetailAirline> CompanyFlightRejectDetailAirlines { get; set; }

    public virtual DbSet<CompanyFlightSegmentDetailAirline> CompanyFlightSegmentDetailAirlines { get; set; }

    public virtual DbSet<CompanyFlightSegmentRuleDetailAirline> CompanyFlightSegmentRuleDetailAirlines { get; set; }

    public virtual DbSet<CompanyMarkupDRule> CompanyMarkupDRules { get; set; }

    public virtual DbSet<CompanyMarkupIRule> CompanyMarkupIRules { get; set; }

    public virtual DbSet<CompanyPaxDetailAirline> CompanyPaxDetailAirlines { get; set; }

    public virtual DbSet<CompanyPaxSegmentCancellationDetailAirline> CompanyPaxSegmentCancellationDetailAirlines { get; set; }

    public virtual DbSet<CompanyPaxSegmentCancellationDetailAirlineRefund> CompanyPaxSegmentCancellationDetailAirlineRefunds { get; set; }

    public virtual DbSet<CompanyPaxSegmentCancellationDetailAirlineRefundSub> CompanyPaxSegmentCancellationDetailAirlineRefundSubs { get; set; }

    public virtual DbSet<CompanyPaxSegmentCancellationDetailAirlineRemark> CompanyPaxSegmentCancellationDetailAirlineRemarks { get; set; }

    public virtual DbSet<CompanyPaxSegmentDetailAirline> CompanyPaxSegmentDetailAirlines { get; set; }

    public virtual DbSet<CompanyPaxSegmentRescheduleDetailAirline> CompanyPaxSegmentRescheduleDetailAirlines { get; set; }

    public virtual DbSet<CompanyPaymentDetail> CompanyPaymentDetails { get; set; }

    public virtual DbSet<CompanyProductServiceTaxDetail> CompanyProductServiceTaxDetails { get; set; }

    public virtual DbSet<CompanyProductTdsDetail> CompanyProductTdsDetails { get; set; }

    public virtual DbSet<CompanyProfile> CompanyProfiles { get; set; }

    public virtual DbSet<CompanyPromoDetail> CompanyPromoDetails { get; set; }

    public virtual DbSet<CompanyRegister> CompanyRegisters { get; set; }

    public virtual DbSet<CompanyRegisterAttachment> CompanyRegisterAttachments { get; set; }

    public virtual DbSet<CompanyRegisterCorporateDetail> CompanyRegisterCorporateDetails { get; set; }

    public virtual DbSet<CompanyRegisterCorporateUser> CompanyRegisterCorporateUsers { get; set; }

    public virtual DbSet<CompanyRegisterCorporateUsersLimit> CompanyRegisterCorporateUsersLimits { get; set; }

    public virtual DbSet<CompanyRegisterGst> CompanyRegisterGsts { get; set; }

    public virtual DbSet<CompanyRegisterProduct> CompanyRegisterProducts { get; set; }

    public virtual DbSet<CompanyRegisterProductAdded> CompanyRegisterProductAddeds { get; set; }

    public virtual DbSet<CompanyRoleModuleProcuctRegisterOperationManagement> CompanyRoleModuleProcuctRegisterOperationManagements { get; set; }

    public virtual DbSet<CompanyTransactionDetail> CompanyTransactionDetails { get; set; }

    public virtual DbSet<CompanyTransactionLedgerEvent> CompanyTransactionLedgerEvents { get; set; }

    public virtual DbSet<CorporateRole> CorporateRoles { get; set; }

    public virtual DbSet<CountryMaster> CountryMasters { get; set; }

    public virtual DbSet<CurrencyDatum> CurrencyData { get; set; }

    public virtual DbSet<CustomerFareDetailAirline1> CustomerFareDetailAirline1s { get; set; }

    public virtual DbSet<CustomerFareDetailHotel> CustomerFareDetailHotels { get; set; }

    public virtual DbSet<CustomerRefund> CustomerRefunds { get; set; }

    public virtual DbSet<DealDetail> DealDetails { get; set; }

    public virtual DbSet<DealValidity> DealValidities { get; set; }

    public virtual DbSet<DealVendor> DealVendors { get; set; }

    public virtual DbSet<DeparHotelAdd> DeparHotelAdds { get; set; }

    public virtual DbSet<DeparHotelAddFacility> DeparHotelAddFacilities { get; set; }

    public virtual DbSet<DeparHotelAddImage> DeparHotelAddImages { get; set; }

    public virtual DbSet<DeparHotelAddMetum> DeparHotelAddMeta { get; set; }

    public virtual DbSet<DeparHotelAddPolicy> DeparHotelAddPolicies { get; set; }

    public virtual DbSet<DeparHotelAddRoom> DeparHotelAddRooms { get; set; }

    public virtual DbSet<DeparHotelAddRoomsPrice> DeparHotelAddRoomsPrices { get; set; }

    public virtual DbSet<DeparHotelAmenity> DeparHotelAmenities { get; set; }

    public virtual DbSet<DeparHotelFacility> DeparHotelFacilities { get; set; }

    public virtual DbSet<DeparHotelInclusion> DeparHotelInclusions { get; set; }

    public virtual DbSet<DeparHotelInstruction> DeparHotelInstructions { get; set; }

    public virtual DbSet<DeparHotelRole> DeparHotelRoles { get; set; }

    public virtual DbSet<DeparHotelRoomType> DeparHotelRoomTypes { get; set; }

    public virtual DbSet<DeparHotelType> DeparHotelTypes { get; set; }

    public virtual DbSet<DeparHotelentryDetail> DeparHotelentryDetails { get; set; }

    public virtual DbSet<DeparHotelentryDetailDescription> DeparHotelentryDetailDescriptions { get; set; }

    public virtual DbSet<DeparHotelentryPassengerDetail> DeparHotelentryPassengerDetails { get; set; }

    public virtual DbSet<DeparHotelentryRoomDetail> DeparHotelentryRoomDetails { get; set; }

    public virtual DbSet<DepartureFare> DepartureFares { get; set; }

    public virtual DbSet<DepartureFaresBook> DepartureFaresBooks { get; set; }

    public virtual DbSet<DepartureFaresPassengerBook> DepartureFaresPassengerBooks { get; set; }

    public virtual DbSet<DepartureRole> DepartureRoles { get; set; }

    public virtual DbSet<DmcCountryCityList> DmcCountryCityLists { get; set; }

    public virtual DbSet<FdagentRole> FdagentRoles { get; set; }

    public virtual DbSet<FlightOwnSegmentsPnr> FlightOwnSegmentsPnrs { get; set; }

    public virtual DbSet<FlightSearchCache> FlightSearchCaches { get; set; }

    public virtual DbSet<FlightSearchCacheMaster> FlightSearchCacheMasters { get; set; }

    public virtual DbSet<GoFirstSsr> GoFirstSsrs { get; set; }

    public virtual DbSet<GroupCommission> GroupCommissions { get; set; }

    public virtual DbSet<GroupCommissionDRule> GroupCommissionDRules { get; set; }

    public virtual DbSet<GroupCommissionDRuleCN> GroupCommissionDRuleCNs { get; set; }

    public virtual DbSet<GroupCommissionDRuleSa> GroupCommissionDRuleSas { get; set; }

    public virtual DbSet<GroupCommissionIRule> GroupCommissionIRules { get; set; }

    public virtual DbSet<GroupCommissionIRuleCN> GroupCommissionIRuleCNs { get; set; }

    public virtual DbSet<GroupCommissionIRuleSa> GroupCommissionIRuleSas { get; set; }

    public virtual DbSet<GroupCommissionPriceType> GroupCommissionPriceTypes { get; set; }

    public virtual DbSet<GroupCommissionSa> GroupCommissionSas { get; set; }

    public virtual DbSet<GroupCompanyCommission> GroupCompanyCommissions { get; set; }

    public virtual DbSet<GroupCompanyCommissionSa> GroupCompanyCommissionSas { get; set; }

    public virtual DbSet<GstState> GstStates { get; set; }

    public virtual DbSet<GstStateCity> GstStateCities { get; set; }

    public virtual DbSet<HelpFile> HelpFiles { get; set; }

    public virtual DbSet<HelpFileDescription> HelpFileDescriptions { get; set; }

    public virtual DbSet<HolidaysQuery> HolidaysQueries { get; set; }

    public virtual DbSet<HotelCallCenter> HotelCallCenters { get; set; }

    public virtual DbSet<HotelCityList> HotelCityLists { get; set; }

    public virtual DbSet<HotelCityStaticList> HotelCityStaticLists { get; set; }

    public virtual DbSet<HotelCommission> HotelCommissions { get; set; }

    public virtual DbSet<HotelCommissionSa> HotelCommissionSas { get; set; }

    public virtual DbSet<HotelCountryList> HotelCountryLists { get; set; }

    public virtual DbSet<HotelDetail> HotelDetails { get; set; }

    public virtual DbSet<HotelMarkup> HotelMarkups { get; set; }

    public virtual DbSet<HotelMarkupCommC> HotelMarkupCommCs { get; set; }

    public virtual DbSet<HotelPassengerDetail> HotelPassengerDetails { get; set; }

    public virtual DbSet<HotelRejectDetail> HotelRejectDetails { get; set; }

    public virtual DbSet<HotelSegmentDetail> HotelSegmentDetails { get; set; }

    public virtual DbSet<HtmlFilght> HtmlFilghts { get; set; }

    public virtual DbSet<HtmlProduct> HtmlProducts { get; set; }

    public virtual DbSet<HtmlProductsImage> HtmlProductsImages { get; set; }

    public virtual DbSet<HtmlProductsQuery> HtmlProductsQueries { get; set; }

    public virtual DbSet<LogCompanyBalanceTransactionDetail> LogCompanyBalanceTransactionDetails { get; set; }

    public virtual DbSet<LogCompanyBalanceTransactionHistoryDetail> LogCompanyBalanceTransactionHistoryDetails { get; set; }

    public virtual DbSet<LogCompanyFlightDetailAirline> LogCompanyFlightDetailAirlines { get; set; }

    public virtual DbSet<LogCompanyFlightSegmentDetailAirline> LogCompanyFlightSegmentDetailAirlines { get; set; }

    public virtual DbSet<LogCompanyPaxDetailAirline> LogCompanyPaxDetailAirlines { get; set; }

    public virtual DbSet<LogCompanyPaxSegmentCancellationDetailAirline> LogCompanyPaxSegmentCancellationDetailAirlines { get; set; }

    public virtual DbSet<LogCompanyPaxSegmentCancellationDetailAirlineRefund> LogCompanyPaxSegmentCancellationDetailAirlineRefunds { get; set; }

    public virtual DbSet<LogCompanyPaxSegmentCancellationDetailAirlineRefundSub> LogCompanyPaxSegmentCancellationDetailAirlineRefundSubs { get; set; }

    public virtual DbSet<LogCompanyPaxSegmentDetailAirline> LogCompanyPaxSegmentDetailAirlines { get; set; }

    public virtual DbSet<LogCompanyPaymentDetail> LogCompanyPaymentDetails { get; set; }

    public virtual DbSet<LogCompanyRegister> LogCompanyRegisters { get; set; }

    public virtual DbSet<LogCompanyTransactionDetail> LogCompanyTransactionDetails { get; set; }

    public virtual DbSet<LogPaymentGatewayLogger> LogPaymentGatewayLoggers { get; set; }

    public virtual DbSet<Logger> Loggers { get; set; }

    public virtual DbSet<LoggerApi> LoggerApis { get; set; }

    public virtual DbSet<LoggerHotelApi> LoggerHotelApis { get; set; }

    public virtual DbSet<LoggerLogin> LoggerLogins { get; set; }

    public virtual DbSet<LoggerSearch> LoggerSearches { get; set; }

    public virtual DbSet<LoginOtp> LoginOtps { get; set; }

    public virtual DbSet<MailTemplateSender> MailTemplateSenders { get; set; }

    public virtual DbSet<MailTemplateSenderSa> MailTemplateSenderSas { get; set; }

    public virtual DbSet<MasterAirline> MasterAirlines { get; set; }

    public virtual DbSet<NoticeCompany> NoticeCompanies { get; set; }

    public virtual DbSet<NoticeStaff> NoticeStaffs { get; set; }

    public virtual DbSet<OpinionLog> OpinionLogs { get; set; }

    public virtual DbSet<PaymentGatewayCardCharge> PaymentGatewayCardCharges { get; set; }

    public virtual DbSet<PaymentGatewayCardChargesDetail> PaymentGatewayCardChargesDetails { get; set; }

    public virtual DbSet<PaymentGatewayCardChargesWaiverLimit> PaymentGatewayCardChargesWaiverLimits { get; set; }

    public virtual DbSet<PaymentGatewayCardName> PaymentGatewayCardNames { get; set; }

    public virtual DbSet<PaymentGatewayLogger> PaymentGatewayLoggers { get; set; }

    public virtual DbSet<PaymentGatewayManagement> PaymentGatewayManagements { get; set; }

    public virtual DbSet<PkgAdd> PkgAdds { get; set; }

    public virtual DbSet<PkgAddCity> PkgAddCities { get; set; }

    public virtual DbSet<PkgAddDepartureCity> PkgAddDepartureCities { get; set; }

    public virtual DbSet<PkgAddKeyword> PkgAddKeywords { get; set; }

    public virtual DbSet<PkgAddMetaInfo> PkgAddMetaInfos { get; set; }

    public virtual DbSet<PkgAddPolicy> PkgAddPolicies { get; set; }

    public virtual DbSet<PkgAddTransfer> PkgAddTransfers { get; set; }

    public virtual DbSet<PkgBilling> PkgBillings { get; set; }

    public virtual DbSet<PkgDestination> PkgDestinations { get; set; }

    public virtual DbSet<PkgDocument> PkgDocuments { get; set; }

    public virtual DbSet<PkgExclusion> PkgExclusions { get; set; }

    public virtual DbSet<PkgHotelCategory> PkgHotelCategories { get; set; }

    public virtual DbSet<PkgInclusion> PkgInclusions { get; set; }

    public virtual DbSet<PkgPackageRange> PkgPackageRanges { get; set; }

    public virtual DbSet<PkgPassenger> PkgPassengers { get; set; }

    public virtual DbSet<PkgPayment> PkgPayments { get; set; }

    public virtual DbSet<PkgPaymentoption> PkgPaymentoptions { get; set; }

    public virtual DbSet<PkgQuery> PkgQueries { get; set; }

    public virtual DbSet<PkgQueryReply> PkgQueryReplies { get; set; }

    public virtual DbSet<PkgQueryReplyVendor> PkgQueryReplyVendors { get; set; }

    public virtual DbSet<PkgStatus> PkgStatuses { get; set; }

    public virtual DbSet<PkgTheme> PkgThemes { get; set; }

    public virtual DbSet<PkgVendor> PkgVendors { get; set; }

    public virtual DbSet<PkgVendorType> PkgVendorTypes { get; set; }

    public virtual DbSet<PkgWorkingOn> PkgWorkingOns { get; set; }

    public virtual DbSet<PreferredAirline> PreferredAirlines { get; set; }

    public virtual DbSet<QueryGroupFare> QueryGroupFares { get; set; }

    public virtual DbSet<RoleModuleManagement> RoleModuleManagements { get; set; }

    public virtual DbSet<RoleModuleProcuctManagement> RoleModuleProcuctManagements { get; set; }

    public virtual DbSet<RoleModuleProcuctRegisterManagement> RoleModuleProcuctRegisterManagements { get; set; }

    public virtual DbSet<SmsDetail> SmsDetails { get; set; }

    public virtual DbSet<SmsRunTime> SmsRunTimes { get; set; }

    public virtual DbSet<SmsTemplate> SmsTemplates { get; set; }

    public virtual DbSet<SmtpDetail> SmtpDetails { get; set; }

    public virtual DbSet<SpicejetSsr> SpicejetSsrs { get; set; }

    public virtual DbSet<StCityAirport> StCityAirports { get; set; }

    public virtual DbSet<StaffCategory> StaffCategories { get; set; }

    public virtual DbSet<StaffRole> StaffRoles { get; set; }

    public virtual DbSet<SupplierApiAirline> SupplierApiAirlines { get; set; }

    public virtual DbSet<SupplierCommissionDRule> SupplierCommissionDRules { get; set; }

    public virtual DbSet<SupplierCommissionIRule> SupplierCommissionIRules { get; set; }

    public virtual DbSet<SupplierDetail> SupplierDetails { get; set; }

    public virtual DbSet<SupplierDetailAmadeusAirline> SupplierDetailAmadeusAirlines { get; set; }

    public virtual DbSet<SupplierDetailApiAirline> SupplierDetailApiAirlines { get; set; }

    public virtual DbSet<SupplierDetailApiHotel> SupplierDetailApiHotels { get; set; }

    public virtual DbSet<SupplierDetailGalileoAirline> SupplierDetailGalileoAirlines { get; set; }

    public virtual DbSet<SupplierDetailLccAirline> SupplierDetailLccAirlines { get; set; }

    public virtual DbSet<SupplierProductDetail> SupplierProductDetails { get; set; }

    public virtual DbSet<SupplierProductStatus> SupplierProductStatuses { get; set; }

    public virtual DbSet<SupplierRefidSeries> SupplierRefidSeries { get; set; }

    public virtual DbSet<Table1> Table1s { get; set; }

    public virtual DbSet<TiAirportCity> TiAirportCities { get; set; }

    public virtual DbSet<TiFlightActiveSupplier> TiFlightActiveSuppliers { get; set; }

    public virtual DbSet<ToursExclusion> ToursExclusions { get; set; }

    public virtual DbSet<ToursInclusion> ToursInclusions { get; set; }

    public virtual DbSet<ToursInvoice> ToursInvoices { get; set; }

    public virtual DbSet<ToursInvoiceDetail> ToursInvoiceDetails { get; set; }

    public virtual DbSet<ToursInvoicePassenger> ToursInvoicePassengers { get; set; }

    public virtual DbSet<ToursList> ToursLists { get; set; }

    public virtual DbSet<ToursListCommVat> ToursListCommVats { get; set; }

    public virtual DbSet<ToursListContact> ToursListContacts { get; set; }

    public virtual DbSet<ToursListDaysdescription> ToursListDaysdescriptions { get; set; }

    public virtual DbSet<ToursListExclusion> ToursListExclusions { get; set; }

    public virtual DbSet<ToursListImage> ToursListImages { get; set; }

    public virtual DbSet<ToursListInclusion> ToursListInclusions { get; set; }

    public virtual DbSet<ToursListLocation> ToursListLocations { get; set; }

    public virtual DbSet<ToursListMap> ToursListMaps { get; set; }

    public virtual DbSet<ToursListMetatag> ToursListMetatags { get; set; }

    public virtual DbSet<ToursListPolicy> ToursListPolicies { get; set; }

    public virtual DbSet<ToursListRelated> ToursListRelateds { get; set; }

    public virtual DbSet<ToursListReview> ToursListReviews { get; set; }

    public virtual DbSet<ToursPaymentoption> ToursPaymentoptions { get; set; }

    public virtual DbSet<ToursRole> ToursRoles { get; set; }

    public virtual DbSet<ToursTourtype> ToursTourtypes { get; set; }

    public virtual DbSet<UapiCcDetail> UapiCcDetails { get; set; }

    public virtual DbSet<UapiFormOfPayment> UapiFormOfPayments { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    public virtual DbSet<VVisaCategory> VVisaCategories { get; set; }

    public virtual DbSet<VVisaChargeCategory> VVisaChargeCategories { get; set; }

    public virtual DbSet<VVisaEntry> VVisaEntries { get; set; }

    public virtual DbSet<VVisaFee> VVisaFees { get; set; }

    public virtual DbSet<VVisaFile> VVisaFiles { get; set; }

    public virtual DbSet<VVisaOrder> VVisaOrders { get; set; }

    public virtual DbSet<VVisaRule> VVisaRules { get; set; }

    public virtual DbSet<WhitelabelDetail> WhitelabelDetails { get; set; }

    public virtual DbSet<WhitelabelRole> WhitelabelRoles { get; set; }

    public virtual DbSet<WhitelabelSupport> WhitelabelSupports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AddGroupFare>(entity =>
        {
            entity.ToTable("Add_Group_Fares");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AdultBasic).HasDefaultValue(0);
            entity.Property(e => e.AdultTax).HasDefaultValue(0);
            entity.Property(e => e.AdultYq).HasDefaultValue(0);
            entity.Property(e => e.ArrivalTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Baggage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Cabin)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CabinBaggage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.ChildBasic).HasDefaultValue(0);
            entity.Property(e => e.ChildTax).HasDefaultValue(0);
            entity.Property(e => e.ChildYq).HasDefaultValue(0);
            entity.Property(e => e.ClassOfService)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.DepartureDate)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.DepartureTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Destination)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FlightNumber)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.InfantBasic).HasDefaultValue(0);
            entity.Property(e => e.InfantTax).HasDefaultValue(0);
            entity.Property(e => e.Origin)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.PriceType)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.RefundType)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.RuleTarrif)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SoldSeat).HasDefaultValue(0);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.Stops).HasDefaultValue(0);
            entity.Property(e => e.Supplier)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalSeat).HasDefaultValue(0);
        });

        modelBuilder.Entity<AddGroupFdFare>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Add_Group_FD_Fares");

            entity.Property(e => e.AdminCharge).HasDefaultValue(0);
            entity.Property(e => e.AdultBasic).HasDefaultValue(0);
            entity.Property(e => e.AdultTax).HasDefaultValue(0);
            entity.Property(e => e.AdultYq).HasDefaultValue(0);
            entity.Property(e => e.ArrivalTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Baggage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BaggageDetail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Cabin)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CabinBaggage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.ChildBasic).HasDefaultValue(0);
            entity.Property(e => e.ChildTax).HasDefaultValue(0);
            entity.Property(e => e.ChildYq).HasDefaultValue(0);
            entity.Property(e => e.ClassOfService)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.DepartureDate)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.DepartureTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Destination)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FlightNumber)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.InfantBasic).HasDefaultValue(0);
            entity.Property(e => e.InfantTax).HasDefaultValue(0);
            entity.Property(e => e.Origin)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.PriceType)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.RefundType)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.RuleTarrif)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SoldSeat).HasDefaultValue(0);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.Stops).HasDefaultValue(0);
            entity.Property(e => e.Supplier)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalSeat).HasDefaultValue(0);
        });

        modelBuilder.Entity<AirExtraCommission>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Air_ExtraCommission");

            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.ExtraCommission).HasDefaultValue(0);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID");
        });

        modelBuilder.Entity<AirMarkupCreateGroupSa>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Air_Markup_Create_Group_SA");

            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GroupId)
                .HasComputedColumnSql("([ID]+(100))", false)
                .HasColumnName("GroupID");
            entity.Property(e => e.GroupName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.RegistrationGroup).HasDefaultValue(false);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AirMarkupDRuleSa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Group_Markup_D_Rule_SA");

            entity.ToTable("Air_Markup_D_Rule_SA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.GroupId)
                .HasDefaultValue(0)
                .HasColumnName("GroupID");
            entity.Property(e => e.NormalMarkup).HasDefaultValue(0);
            entity.Property(e => e.SpecialMarkup).HasDefaultValue(0);
        });

        modelBuilder.Entity<AirMarkupIRuleSa>(entity =>
        {
            entity.ToTable("Air_Markup_I_Rule_SA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.GroupId)
                .HasDefaultValue(0)
                .HasColumnName("GroupID");
            entity.Property(e => e.NormalMarkup).HasDefaultValue(0);
            entity.Property(e => e.SpecialMarkup).HasDefaultValue(0);
        });

        modelBuilder.Entity<AirMarkupSetCompanySa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Air_Company_Markup_SA");

            entity.ToTable("Air_Markup_Set_Company_SA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.GroupId)
                .HasDefaultValue(0)
                .HasColumnName("GroupID");
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AirOffline>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("AirOffline");

            entity.Property(e => e.Adt).HasDefaultValue(0);
            entity.Property(e => e.AdultBasicInbound).HasDefaultValue(0);
            entity.Property(e => e.AdultBasicOutbound).HasDefaultValue(0);
            entity.Property(e => e.AdultTaxInbound).HasDefaultValue(0);
            entity.Property(e => e.AdultTaxOutbound).HasDefaultValue(0);
            entity.Property(e => e.AdultTotalInbound).HasDefaultValue(0);
            entity.Property(e => e.AdultTotalOutbound).HasDefaultValue(0);
            entity.Property(e => e.AdultYqInbound).HasDefaultValue(0);
            entity.Property(e => e.AdultYqOutbound).HasDefaultValue(0);
            entity.Property(e => e.ArrDateInbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ArrDateOutbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ArrTimeInbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ArrTimeOutbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ArrivalAirportInbound)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ArrivalAirportOutbound)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ArrivalStationInbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ArrivalStationOutbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BookingRef).HasComputedColumnSql("([id]+(1000000))", false);
            entity.Property(e => e.BookingStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CarrierCodeInbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CarrierCodeOutbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Chd).HasDefaultValue(0);
            entity.Property(e => e.ChildBasicInbound).HasDefaultValue(0);
            entity.Property(e => e.ChildBasicOutbound).HasDefaultValue(0);
            entity.Property(e => e.ChildTaxInbound).HasDefaultValue(0);
            entity.Property(e => e.ChildTaxOutbound).HasDefaultValue(0);
            entity.Property(e => e.ChildTotalInbound).HasDefaultValue(0);
            entity.Property(e => e.ChildTotalOutbound).HasDefaultValue(0);
            entity.Property(e => e.ChildYqInbound).HasDefaultValue(0);
            entity.Property(e => e.ChildYqOutbound).HasDefaultValue(0);
            entity.Property(e => e.ClassInbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ClassOutbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.DepDateInbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DepDateOutbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DepTimeInbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DepTimeOutbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DepartureAirportInbound)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DepartureAirportOutbound)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DepartureStationInbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DepartureStationOutbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Destination)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Dob)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FirstName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FlightNumberInbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FlightNumberOutbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Inf).HasDefaultValue(0);
            entity.Property(e => e.InfantBasicInbound).HasDefaultValue(0);
            entity.Property(e => e.InfantBasicOutbound).HasDefaultValue(0);
            entity.Property(e => e.InfantTaxInbound).HasDefaultValue(0);
            entity.Property(e => e.InfantTaxOutbound).HasDefaultValue(0);
            entity.Property(e => e.InfantTotalInbound).HasDefaultValue(0);
            entity.Property(e => e.InfantTotalOutbound).HasDefaultValue(0);
            entity.Property(e => e.IsBilled).HasDefaultValue(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Origin)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PaxType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PnrInbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PnrOutbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.StopsInbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StopsOutbound)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Supplier)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TicketNumber)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalFare).HasDefaultValue(0);
            entity.Property(e => e.Trip)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AirOfflineSupplier>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("AirOfflineSupplier");

            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Supplier)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AirPriceTypeCommissionGroup>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Air_PriceTypeCommission_Group");

            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GroupId)
                .HasComputedColumnSql("([id]+(100))", false)
                .HasColumnName("GroupID");
            entity.Property(e => e.GroupName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.RegistrationGroup).HasDefaultValue(false);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AirPriceTypeCommissionGroupCommission>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Air_PriceTypeCommission_Group_Commission");

            entity.Property(e => e.Basic).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Cb)
                .HasDefaultValue(0)
                .HasColumnName("CB");
            entity.Property(e => e.GroupId)
                .HasDefaultValue(0)
                .HasColumnName("GroupID");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Pnr)
                .HasDefaultValue(false)
                .HasColumnName("PNR");
            entity.Property(e => e.PriceType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Sf)
                .HasDefaultValue(0)
                .HasColumnName("SF");
            entity.Property(e => e.Show).HasDefaultValue(false);
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.Yq).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<AirPriceTypeCommissionGroupCompany>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Air_PriceTypeCommission_Group_Company");

            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.GroupId)
                .HasDefaultValue(0)
                .HasColumnName("GroupID");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AirlineBaggage>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CabinBaggage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CheckInBaggage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Destination)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.DestinationCountry)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.GdsCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsHandBaggage).HasDefaultValue(false);
            entity.Property(e => e.SourceCountry)
                .HasMaxLength(2)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AirlineBaggageDetail>(entity =>
        {
            entity.ToTable("Airline_Baggage_Detail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CabinBaggage)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Cabin_Baggage");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CheckInBaggage)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Check_in_Baggage");
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AirlineBaggageDetail1>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Airline_Baggage_Details");

            entity.Property(e => e.Cabin)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Carrier)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.CheckIn)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Destination)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AirlineCallCenter>(entity =>
        {
            entity.HasKey(e => e.BookingRef);

            entity.ToTable("Airline_CallCenter");

            entity.Property(e => e.BookingRef).ValueGeneratedNever();
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StaffID");
            entity.Property(e => e.Status).HasDefaultValue(false);
        });

        modelBuilder.Entity<AirlineCancellationReschedulingChargeDetail>(entity =>
        {
            entity.ToTable("Airline_Cancellation_Rescheduling_Charge_Detail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CancellationFee)
                .HasDefaultValue(0)
                .HasColumnName("Cancellation_Fee");
            entity.Property(e => e.CancellationRemark)
                .HasColumnType("text")
                .HasColumnName("Cancellation_Remark");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.ReBookingFee)
                .HasDefaultValue(0)
                .HasColumnName("Re_Booking_Fee");
            entity.Property(e => e.ReBookingRemark)
                .HasColumnType("text")
                .HasColumnName("Re_Booking_Remark");
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AirlineManualChargeAirline>(entity =>
        {
            entity.ToTable("Airline_Manual_Charge_Airline");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ChargeAmount).HasDefaultValue(0);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AirlinePnrMakeDay>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.PnrDays).HasDefaultValue(0);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AirlineSsrList>(entity =>
        {
            entity.ToTable("Airline_SSR_List");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AdditionalRule)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FeeCode)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ProductClass)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Refundable)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.SsrCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.SsrType)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AirlinesSector>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ArrivalStation)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.DepartureStation)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        modelBuilder.Entity<AirportCity>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AirportCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AirportName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AltCityName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CityCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CityName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CountryCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CountryName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Orderlist).HasDefaultValue(false);
        });

        modelBuilder.Entity<BankCodeList>(entity =>
        {
            entity.ToTable("BankCodeList");

            entity.Property(e => e.BankCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BankName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BankDetail>(entity =>
        {
            entity.HasKey(e => e.AccountNo);

            entity.ToTable("Bank_Detail");

            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.B2b)
                .HasDefaultValue(false)
                .HasColumnName("B2B");
            entity.Property(e => e.B2b2b)
                .HasDefaultValue(false)
                .HasColumnName("B2B2B");
            entity.Property(e => e.B2b2c)
                .HasDefaultValue(false)
                .HasColumnName("B2B2C");
            entity.Property(e => e.B2c)
                .HasDefaultValue(false)
                .HasColumnName("B2C");
            entity.Property(e => e.BankCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Bank_Code");
            entity.Property(e => e.BankLogo)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Bank_Logo");
            entity.Property(e => e.BankLogoCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Bank_Logo_Code");
            entity.Property(e => e.BankName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Bank_Name");
            entity.Property(e => e.BranchName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Branch_Name");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.D2b)
                .HasDefaultValue(false)
                .HasColumnName("D2B");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Status).HasDefaultValue(false);
        });

        modelBuilder.Entity<BillingTrigg>(entity =>
        {
            entity.ToTable("Billing_Trigg");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookingType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("AIRLINE");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsBilled).HasDefaultValue(false);
            entity.Property(e => e.Status).HasDefaultValue(false);
        });

        modelBuilder.Entity<BookingAirlineLogForPg>(entity =>
        {
            entity.ToTable("BookingAirlineLogForPG");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AvailabilityResponse)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("CompanyID");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("INR");
            entity.Property(e => e.CurrencyValue)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsCombi).HasDefaultValue(false);
            entity.Property(e => e.IsMc)
                .HasDefaultValue(false)
                .HasColumnName("IsMC");
            entity.Property(e => e.IsRt)
                .HasDefaultValue(false)
                .HasColumnName("IsRT");
            entity.Property(e => e.PassengerResponse)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.PaymentId)
                .HasDefaultValue(0)
                .HasColumnName("PaymentID");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.RefIdI)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("RefID_I");
            entity.Property(e => e.RefIdO)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("RefID_O");
            entity.Property(e => e.SearchId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SearchID");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<BookingCustomerLog>(entity =>
        {
            entity.ToTable("BookingCustomerLog");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.AdminId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("AdminID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Host)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Ip)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BookingHotelLogForPg>(entity =>
        {
            entity.ToTable("BookingHotelLogForPG");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("INR");
            entity.Property(e => e.CurrencyValue)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.HotelSearchRequest)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Hotelblock)
                .IsUnicode(false)
                .HasColumnName("hotelblock");
            entity.Property(e => e.Hoteldata)
                .IsUnicode(false)
                .HasColumnName("hoteldata");
            entity.Property(e => e.Hotelinfo)
                .IsUnicode(false)
                .HasColumnName("hotelinfo");
            entity.Property(e => e.Paxdetail)
                .IsUnicode(false)
                .HasColumnName("paxdetail");
            entity.Property(e => e.PaymentId)
                .HasDefaultValue(0)
                .HasColumnName("PaymentID");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ResultIndex)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoomRef)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SearchId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SearchID");
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StaffID");
        });

        modelBuilder.Entity<BookingsMailSmsDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Bookings_Mail_Sms_Detail");

            entity.Property(e => e.BookingType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EventTime).HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
        });

        modelBuilder.Entity<CarEntriesAirport>(entity =>
        {
            entity.ToTable("Car_Entries_Airport");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Dropto).HasDefaultValue(false);
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Pickup).HasDefaultValue(false);
            entity.Property(e => e.PickupDate).HasColumnName("Pickup_date");
            entity.Property(e => e.Refid).HasDefaultValue(0);
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Tostation)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CarEntriesLocal>(entity =>
        {
            entity.ToTable("Car_Entries_Local");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DropoffTime)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Dropoff_time");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.PickupDate).HasColumnName("Pickup_date");
            entity.Property(e => e.PickupTime)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Pickup_time");
            entity.Property(e => e.Refid).HasDefaultValue(0);
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CarEntriesOutstation>(entity =>
        {
            entity.ToTable("Car_Entries_Outstation");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CityFrom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("city_from");
            entity.Property(e => e.CityTo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("city_to");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.FromDate).HasColumnName("from_date");
            entity.Property(e => e.Refid).HasDefaultValue(0);
            entity.Property(e => e.ToDate).HasColumnName("to_date");
            entity.Property(e => e.Trip)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CarEntry>(entity =>
        {
            entity.ToTable("Car_Entries");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Contact)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Eventtime)
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Host)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsAirport)
                .HasDefaultValue(false)
                .HasColumnName("isAirport");
            entity.Property(e => e.IsLocal)
                .HasDefaultValue(false)
                .HasColumnName("isLocal");
            entity.Property(e => e.IsOutstation)
                .HasDefaultValue(false)
                .HasColumnName("isOutstation");
            entity.Property(e => e.NoOfPassenger).HasDefaultValue(0);
            entity.Property(e => e.Refid).HasComputedColumnSql("([ID]+(100))", false);
        });

        modelBuilder.Entity<CarrierDetail>(entity =>
        {
            entity.ToTable("CarrierDetail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AirlineContact)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CarrierName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Iatacode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("IATACode");
            entity.Property(e => e.IsLcc)
                .HasDefaultValue(false)
                .HasColumnName("isLCC");
        });

        modelBuilder.Entity<CommissionDApi>(entity =>
        {
            entity.ToTable("Commission_D_api");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.PnrStatus).HasDefaultValue(false);
            entity.Property(e => e.Sf)
                .HasDefaultValue(0)
                .HasColumnName("SF");
            entity.Property(e => e.Value)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<CommissionGroupCompany>(entity =>
        {
            entity.ToTable("Commission_Group_Company");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.GroupId)
                .HasDefaultValue(0)
                .HasColumnName("GroupID");
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CommissionGroupOnApi>(entity =>
        {
            entity.ToTable("Commission_Group_on_api");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GroupId)
                .HasComputedColumnSql("([id]+(100))", false)
                .HasColumnName("GroupID");
            entity.Property(e => e.GroupName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationGroup).HasDefaultValue(false);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CommissionGroupOnD>(entity =>
        {
            entity.ToTable("Commission_Group_on_D");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.GroupId)
                .HasDefaultValue(0)
                .HasColumnName("GroupID");
            entity.Property(e => e.PnrStatus).HasDefaultValue(false);
            entity.Property(e => e.Sf)
                .HasDefaultValue(0)
                .HasColumnName("SF");
            entity.Property(e => e.Value)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<CommissionGroupOnI>(entity =>
        {
            entity.ToTable("Commission_Group_on_I");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.GroupId)
                .HasDefaultValue(0)
                .HasColumnName("GroupID");
            entity.Property(e => e.PnrStatus).HasDefaultValue(false);
            entity.Property(e => e.Sf)
                .HasDefaultValue(0)
                .HasColumnName("SF");
            entity.Property(e => e.Value)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<CommissionIApi>(entity =>
        {
            entity.ToTable("Commission_I_api");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.PnrStatus).HasDefaultValue(false);
            entity.Property(e => e.Sf)
                .HasDefaultValue(0)
                .HasColumnName("SF");
            entity.Property(e => e.Value)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<CommissionOnCarrier>(entity =>
        {
            entity.HasKey(e => e.CarrierCode);

            entity.ToTable("Commission_On_Carrier");

            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.IsDomestic)
                .HasDefaultValue(false)
                .HasColumnName("isDomestic");
        });

        modelBuilder.Entity<CompanyBalanceTransactionDetail>(entity =>
        {
            entity.ToTable("Company_Balance_Transaction_Detail", tb =>
                {
                    tb.HasTrigger("Trigg_Company_Balance_Transaction_Detail");
                    tb.HasTrigger("Trigg_Company_Balance_Transaction_Detail_updated");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AvailableBalance)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Available_Balance");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.CreditAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Credit_Amount");
            entity.Property(e => e.CreditBalance)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Credit_Balance");
            entity.Property(e => e.CreditTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CreditUser).HasDefaultValue(false);
            entity.Property(e => e.EventId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("EventID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OutstandingBalance)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Outstanding_Balance");
            entity.Property(e => e.TemporaryBalance)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Temporary_Balance");
            entity.Property(e => e.TemporaryBalanceTime)
                .HasColumnType("datetime")
                .HasColumnName("Temporary_Balance_Time");
        });

        modelBuilder.Entity<CompanyBalanceTransactionDetailEvent>(entity =>
        {
            entity.HasKey(e => e.EventId);

            entity.ToTable("Company_Balance_Transaction_Detail_Events");

            entity.Property(e => e.EventId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EventID");
            entity.Property(e => e.EventName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.EventType)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
        });

        modelBuilder.Entity<CompanyBalanceTransactionHistoryDetail>(entity =>
        {
            entity.ToTable("Company_Balance_Transaction_History_Detail", tb =>
                {
                    tb.HasTrigger("Trigg_Company_Balance_Transaction_History_Detail_Delete");
                    tb.HasTrigger("Trigg_Company_Balance_Transaction_History_Detail_insert");
                    tb.HasTrigger("Trigg_Company_Balance_Transaction_History_Detail_update");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AfterTransactionAvailableBalance)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("After_Transaction_Available_balance");
            entity.Property(e => e.AfterTransactionTemporaryBalance)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("After_Transaction_Temporary_Balance");
            entity.Property(e => e.BeforeTransactionAvailableBalance)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Before_Transaction_Available_Balance");
            entity.Property(e => e.BeforeTransactionTemporaryBalance)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Before_Transaction_Temporary_Balance");
            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.EventId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EventID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OutStandingBalance)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("OutStanding_Balance");
            entity.Property(e => e.Remark).IsUnicode(false);
            entity.Property(e => e.TransType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TransactionAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Transaction_Amount");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CompanyBookingsCfee>(entity =>
        {
            entity.ToTable("Company_Bookings_Cfee");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookingType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CfeeCu)
                .HasDefaultValue(0)
                .HasColumnName("Cfee_cu");
            entity.Property(e => e.CfeeSa)
                .HasDefaultValue(0)
                .HasColumnName("Cfee_sa");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Iswallet).HasDefaultValue(false);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CompanyFareDetailAirline>(entity =>
        {
            entity.ToTable("Company_Fare_Detail_Airline");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Conn)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PriceType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.TotalBaggage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalBasic).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalBasicDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalBasic_deal");
            entity.Property(e => e.TotalCbDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalCB_deal");
            entity.Property(e => e.TotalCommission).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalFare).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalMarkup).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalMeal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalPromoDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalPromo_deal");
            entity.Property(e => e.TotalQueue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalSeat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalServiceFeeDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalServiceFee_deal");
            entity.Property(e => e.TotalServiceTax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalTax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalTds).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalYq)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalYqDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalYQ_deal");
        });

        modelBuilder.Entity<CompanyFareDetailSegmentAirline>(entity =>
        {
            entity.ToTable("Company_Fare_Detail_Segment_Airline");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Audf)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("AUDF");
            entity.Property(e => e.Baggage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Basic).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BasicDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Basic_Deal");
            entity.Property(e => e.BasicDeal1)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Basic_Deal1");
            entity.Property(e => e.CbDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Cb_Deal");
            entity.Property(e => e.CbDeal1)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Cb_Deal1");
            entity.Property(e => e.Cess).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Conn)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Cute).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Ex).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Gst).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Import).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Import1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Markup).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Markup1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Meal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NoOfPassenger).HasColumnName("No_Of_Passenger");
            entity.Property(e => e.PaxType)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.PromoDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Promo_Deal");
            entity.Property(e => e.PromoDeal1)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Promo_Deal1");
            entity.Property(e => e.Psf).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Seat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ServiceFee)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Service_Fee");
            entity.Property(e => e.ServiceFee1)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Service_Fee1");
            entity.Property(e => e.ServiceTax).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ServiceTax1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tds).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Tds1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tf)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TF");
            entity.Property(e => e.Udf).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Yq).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.YqDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Yq_Deal");
            entity.Property(e => e.YqDeal1)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Yq_Deal1");
        });

        modelBuilder.Entity<CompanyFlightDetailAirline>(entity =>
        {
            entity.ToTable("Company_Flight_Detail_Airline", tb =>
                {
                    tb.HasTrigger("Log_Company_Flight_Detail_Airline_DEL");
                    tb.HasTrigger("Log_Company_Flight_Detail_Airline_INS");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Adt).HasDefaultValue(0);
            entity.Property(e => e.AgMarkupA)
                .HasDefaultValue(0)
                .HasColumnName("AG_Markup_A");
            entity.Property(e => e.AgMarkupD)
                .HasDefaultValue(0)
                .HasColumnName("AG_Markup_D");
            entity.Property(e => e.AirlinePnrA)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Airline_PNR_A");
            entity.Property(e => e.AirlinePnrD)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Airline_PNR_D");
            entity.Property(e => e.BookingRef).HasComputedColumnSql("([ID]+(100))", false);
            entity.Property(e => e.CarrierCodeA)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("CarrierCode_A");
            entity.Property(e => e.CarrierCodeD)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("CarrierCode_D");
            entity.Property(e => e.Chd).HasDefaultValue(0);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.DepartureDateA)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("DepartureDate_A");
            entity.Property(e => e.DepartureDateD)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("DepartureDate_D");
            entity.Property(e => e.Destination)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Fdmarkup)
                .HasDefaultValue(0)
                .HasColumnName("FDMarkup");
            entity.Property(e => e.GdsPnrA)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("GDS_PNR_A");
            entity.Property(e => e.GdsPnrD)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("GDS_PNR_D");
            entity.Property(e => e.Inf).HasDefaultValue(0);
            entity.Property(e => e.IsCancelRequested).HasDefaultValue(false);
            entity.Property(e => e.IsCanceled).HasDefaultValue(false);
            entity.Property(e => e.IsCanceledRejected).HasDefaultValue(false);
            entity.Property(e => e.IsImport).HasDefaultValue(false);
            entity.Property(e => e.IsOffline).HasDefaultValue(false);
            entity.Property(e => e.IsRejected).HasDefaultValue(false);
            entity.Property(e => e.IsRescheduled).HasDefaultValue(false);
            entity.Property(e => e.IsUpdated).HasDefaultValue(false);
            entity.Property(e => e.MakerId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MakerID");
            entity.Property(e => e.Origin)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.PriceTypeA)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("PriceType_A");
            entity.Property(e => e.PriceTypeD)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("PriceType_D");
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StaffID");
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SupplierID");
            entity.Property(e => e.UniversalLocatorCodeA)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("UniversalLocatorCode_A");

            entity.Property(e => e.UniversalLocatorCodeD)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("UniversalLocatorCode_D");

            entity.Property(e => e.SupplierIdA)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SupplierID_A");
            entity.Property(e => e.SupplierIdD)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SupplierID_D");
            entity.Property(e => e.TotalBaggage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalBasic).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalBasicDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalBasic_deal");
            entity.Property(e => e.TotalBasicDeal1)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalBasic_deal1");
            entity.Property(e => e.TotalCbDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalCB_deal");
            entity.Property(e => e.TotalCbDeal1)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalCB_deal1");
            entity.Property(e => e.TotalCommission).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalCommission1)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalFare).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalFare1)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalImport).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalMarkup).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalMarkup1)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalMeal).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TotalPromoDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalPromo_deal");
            entity.Property(e => e.TotalPromoDeal1)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalPromo_deal1");
            entity.Property(e => e.TotalSeat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalServiceFeeDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalServiceFee_deal");
            entity.Property(e => e.TotalServiceTax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalTax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalTds).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalTds1)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalYq)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalYqDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalYQ_deal");
            entity.Property(e => e.TotalYqDeal1)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalYQ_deal1");
            entity.Property(e => e.Totalcfee)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Trip)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CompanyFlightGstDetail>(entity =>
        {
            entity.ToTable("Company_Flight_Gst_Detail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GstcompanyAddress)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("GSTCompanyAddress");
            entity.Property(e => e.GstcompanyContactNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GSTCompanyContactNumber");
            entity.Property(e => e.GstcompanyEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("GSTCompanyEmail");
            entity.Property(e => e.GstcompanyName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("GSTCompanyName");
            entity.Property(e => e.Gstnumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GSTNumber");
        });

        modelBuilder.Entity<CompanyFlightOwnPnr>(entity =>
        {
            entity.ToTable("Company_Flight_Own_Pnr");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FltType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Pnr)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PNR");
        });

        modelBuilder.Entity<CompanyFlightRejectDetailAirline>(entity =>
        {
            entity.ToTable("Company_Flight_Reject_Detail_Airline");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RejectDetail)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CompanyFlightSegmentDetailAirline>(entity =>
        {
            entity.ToTable("Company_Flight_Segment_Detail_Airline", tb =>
                {
                    tb.HasTrigger("Log_Company_Flight_Segment_Detail_Airline_DEL");
                    tb.HasTrigger("Log_Company_Flight_Segment_Detail_Airline_INS");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AirlinePnr)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Airline_PNR");
            entity.Property(e => e.ArrivalDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ArrivalStation)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.ArrivalTerminal)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ArrivalTime)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BaggageDetail)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Cabin)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.ClassOfService)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.ConnOrder)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.DepartureDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DepartureStation)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.DepartureTerminal)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DepartureTime)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Destination)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FareBasisCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FareRule).HasColumnType("text");
            entity.Property(e => e.FareType)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.FlightNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FlightSegmentId)
                .HasComputedColumnSql("(([ID]+[BookingRef])+(10))", false)
                .HasColumnName("Flight_SegmentID");
            entity.Property(e => e.GdsPnr)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("GDS_PNR");
            entity.Property(e => e.Origin)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.ProductClass)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RefundType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RuleTarrif)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ViaName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<CompanyFlightSegmentRuleDetailAirline>(entity =>
        {
            entity.ToTable("Company_Flight_Segment_Rule_Detail_Airline");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ApiSearchId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("API_SearchID");
            entity.Property(e => e.ApiTraceId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("API_TraceID");
            entity.Property(e => e.BaggageDetail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Conn)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.EquipmentType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FareRule).HasColumnType("ntext");
            entity.Property(e => e.FareRuledb).HasColumnType("ntext");
            entity.Property(e => e.IsPriceChanged).HasDefaultValue(false);
            entity.Property(e => e.IsTimeChanged).HasDefaultValue(false);
            entity.Property(e => e.PriceType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SearchId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SearchID");
        });

        modelBuilder.Entity<CompanyMarkupDRule>(entity =>
        {
            entity.ToTable("Company_Markup_D_Rule");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Basic).HasDefaultValue(false);
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Fixed).HasDefaultValue(false);
            entity.Property(e => e.IValue)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("iValue");
            entity.Property(e => e.Perce).HasDefaultValue(false);
            entity.Property(e => e.Total).HasDefaultValue(false);
            entity.Property(e => e.Yq).HasDefaultValue(false);
        });

        modelBuilder.Entity<CompanyMarkupIRule>(entity =>
        {
            entity.ToTable("Company_Markup_I_Rule");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Basic).HasDefaultValue(false);
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Fixed).HasDefaultValue(false);
            entity.Property(e => e.IValue)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("iValue");
            entity.Property(e => e.Perce).HasDefaultValue(false);
            entity.Property(e => e.Total).HasDefaultValue(false);
            entity.Property(e => e.Yq).HasDefaultValue(false);
        });

        modelBuilder.Entity<CompanyPaxDetailAirline>(entity =>
        {
            entity.ToTable("Company_Pax_Detail_Airline", tb =>
                {
                    tb.HasTrigger("Trig_Company_Pax_Detail_Airline_delete");
                    tb.HasTrigger("Trig_Company_Pax_Detail_Airline_update");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Dob)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Ffn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FFN");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Last_Name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Middle_Name");
            entity.Property(e => e.MobileNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaxSegmentId)
                .HasComputedColumnSql("(([ID]+[BookingRef])+(10))", false)
                .HasColumnName("Pax_SegmentID");
            entity.Property(e => e.PaxType)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.PpNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PpexpirayDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PPExpirayDate");
            entity.Property(e => e.PpissueDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PPIssueDate");
            entity.Property(e => e.TicketNo)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TourCode)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CompanyPaxSegmentCancellationDetailAirline>(entity =>
        {
            entity.ToTable("Company_Pax_Segment_Cancellation_Detail_Airline", tb =>
                {
                    tb.HasTrigger("trigg_Company_Pax_Segment_Cancellation_Detail_Airline_delete");
                    tb.HasTrigger("trigg_Company_Pax_Segment_Cancellation_Detail_Airline_update");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CPaxSegmentId).HasColumnName("C_Pax_SegmentID");
            entity.Property(e => e.CanceledBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CancellationReason)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CancellationType)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Destination)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FltType)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Ip)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsPartial).HasDefaultValue(false);
            entity.Property(e => e.Origin)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.PaxSegmentId).HasColumnName("Pax_SegmentID");
            entity.Property(e => e.Pnr)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("PNR");
            entity.Property(e => e.Remark).IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.TicketNo)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CompanyPaxSegmentCancellationDetailAirlineRefund>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Company_Pax_Segment_Cancellation_Detail_Airline_Refund", tb =>
                {
                    tb.HasTrigger("trigg_Company_Pax_Segment_Cancellation_Detail_Airline_Refund_delete");
                    tb.HasTrigger("trigg_Company_Pax_Segment_Cancellation_Detail_Airline_Refund_update");
                });

            entity.Property(e => e.AirlineCharges)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Baggage)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Basic)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.CPaxSegmentId).HasColumnName("C_Pax_SegmentID");
            entity.Property(e => e.Commission)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Gst)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HideAirlineCharges)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Hide_AirlineCharges");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.IsBill).HasDefaultValue(false);
            entity.Property(e => e.Meal)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NetRefundAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Net_Refund_Amount");
            entity.Property(e => e.OurCharges)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaxSegmentId)
                .HasDefaultValue(0)
                .HasColumnName("Pax_SegmentID");
            entity.Property(e => e.PayEventTime)
                .HasColumnType("datetime")
                .HasColumnName("Pay_EventTime");
            entity.Property(e => e.PayStaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Pay_StaffID");
            entity.Property(e => e.RefundType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Reply)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Seat)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ServiceFee)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StaffID");
            entity.Property(e => e.Taxes)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tds)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VerifyEventTime)
                .HasColumnType("datetime")
                .HasColumnName("Verify_EventTime");
            entity.Property(e => e.VerifyStaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Verify_StaffID");
            entity.Property(e => e.VerifyStatus)
                .HasDefaultValue(0)
                .HasColumnName("Verify_Status");
            entity.Property(e => e.Yq)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<CompanyPaxSegmentCancellationDetailAirlineRefundSub>(entity =>
        {
            entity.ToTable("Company_Pax_Segment_Cancellation_Detail_Airline_Refund_Sub", tb =>
                {
                    tb.HasTrigger("trigg_Company_Pax_Segment_Cancellation_Detail_Airline_Refund_Sub_delete");
                    tb.HasTrigger("trigg_Company_Pax_Segment_Cancellation_Detail_Airline_Refund_Sub_update");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AirlineCharges)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Baggage)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Basic)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.CPaxSegmentId).HasColumnName("C_Pax_SegmentID");
            entity.Property(e => e.Commission)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyCancelCharges)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Gst)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Markup)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("markup");
            entity.Property(e => e.Meal)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NetRefundAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Net_Refund_Amount");
            entity.Property(e => e.OurCharges)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaxSegmentId)
                .HasDefaultValue(0)
                .HasColumnName("Pax_SegmentID");
            entity.Property(e => e.PayEventTime)
                .HasColumnType("datetime")
                .HasColumnName("Pay_EventTime");
            entity.Property(e => e.PayStaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Pay_StaffID");
            entity.Property(e => e.RefundType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Reply)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Seat)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ServiceFee)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StaffID");
            entity.Property(e => e.Taxes)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tds)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VerifyEventTime)
                .HasColumnType("datetime")
                .HasColumnName("Verify_EventTime");
            entity.Property(e => e.VerifyStaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Verify_StaffID");
            entity.Property(e => e.VerifyStatus)
                .HasDefaultValue(0)
                .HasColumnName("Verify_Status");
            entity.Property(e => e.Yq)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<CompanyPaxSegmentCancellationDetailAirlineRemark>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Company_Pax_Segment_Cancellation_Detail_Airline_Remark");

            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Remark).IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StaffID");
        });

        modelBuilder.Entity<CompanyPaxSegmentDetailAirline>(entity =>
        {
            entity.ToTable("Company_Pax_Segment_Detail_Airline", tb =>
                {
                    tb.HasTrigger("trigg_Company_Pax_Segment_Detail_Airline_delete");
                    tb.HasTrigger("trigg_Company_Pax_Segment_Detail_Airline_update");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ChargeAmount)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Charge_Amount");
            entity.Property(e => e.ChargeCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ChargeDescription)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ChargeType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Conn)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaxSegmentId).HasColumnName("Pax_SegmentID");
        });

        modelBuilder.Entity<CompanyPaxSegmentRescheduleDetailAirline>(entity =>
        {
            entity.ToTable("Company_Pax_Segment_Reschedule_Detail_Airline");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.DepartureDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Destination)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FlightNumber)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.FltType)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Origin)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.PaxSegmentId).HasColumnName("Pax_SegmentID");
            entity.Property(e => e.Pnr)
                .HasMaxLength(50)
                .HasColumnName("PNR");
            entity.Property(e => e.Remark)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RescheduleType)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.TiketNo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CompanyPaymentDetail>(entity =>
        {
            entity.ToTable("Company_Payment_Detail", tb =>
                {
                    tb.HasTrigger("Trigg_Company_Payment_Detail_Delete");
                    tb.HasTrigger("Trigg_Company_Payment_Detail_update");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Amount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BankName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.ChequeNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Cheque_No");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.EntryBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EventTimeEnd).HasColumnType("datetime");
            entity.Property(e => e.IsBilled).HasDefaultValue(false);
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.PaymentRef).HasComputedColumnSql("([ID]+(1000))", false);
            entity.Property(e => e.PaymentType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReferenceNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Remark).IsUnicode(false);
            entity.Property(e => e.RemarkEnd)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("StaffID");
            entity.Property(e => e.Surcharge)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UtrNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UTR_No");
        });

        modelBuilder.Entity<CompanyProductServiceTaxDetail>(entity =>
        {
            entity.ToTable("Company_Product_ServiceTax_Detail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.FlightServiceTax)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Flight_ServiceTax");
            entity.Property(e => e.HotelServiceTax)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Hotel_ServiceTax");
        });

        modelBuilder.Entity<CompanyProductTdsDetail>(entity =>
        {
            entity.ToTable("Company_Product_TDS_Detail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.FlightTds)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Flight_TDS");
            entity.Property(e => e.HotelTds)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Hotel_TDS");
        });

        modelBuilder.Entity<CompanyProfile>(entity =>
        {
            entity.ToTable("Company_Profiles");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Notification)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<CompanyPromoDetail>(entity =>
        {
            entity.ToTable("Company_Promo_Detail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PromoDeal).HasDefaultValue(0);
            entity.Property(e => e.PromoStatus).HasDefaultValue(false);
            entity.Property(e => e.Promocode)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<CompanyRegister>(entity =>
        {
            entity.HasKey(e => e.Email);

            entity.ToTable("Company_Register", tb =>
                {
                    tb.HasTrigger("Company_Register_Trigg");
                    tb.HasTrigger("Log_Company_Register_DEL");
                    tb.HasTrigger("Log_Company_Register_INS");
                });

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AbtaNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ABTA_No");
            entity.Property(e => e.AccessStatus)
                .HasDefaultValue(false)
                .HasColumnName("Access_Status");
            entity.Property(e => e.AccountId)
                .HasComputedColumnSql("([ID]+(100))", false)
                .HasColumnName("AccountID");
            entity.Property(e => e.ActiveStatus)
                .HasDefaultValue(false)
                .HasColumnName("Active_Status");
            entity.Property(e => e.Address)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.AdminId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("AdminID");
            entity.Property(e => e.AnnualTurnover)
                .HasDefaultValue(0)
                .HasColumnName("Annual_Turnover");
            entity.Property(e => e.AtolNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ATOL_No");
            entity.Property(e => e.BusinessType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Business_Type");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CnpjNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CNPJ_No");
            entity.Property(e => e.CompanyEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Company_Email");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(84)
                .IsUnicode(false)
                .HasComputedColumnSql("(case when [UserType]='ST' then 'ST-'+CONVERT([varchar](30),[ID]+(100),(0)) when [UserType]='B2C' then 'C-'+CONVERT([varchar](30),[ID]+(100),(0)) when [UserType]='B2B' then 'A-'+CONVERT([varchar](30),[ID]+(100),(0)) when [UserType]='B2B2ST' then ([AdminID]+'-ST-')+CONVERT([varchar](30),[ID]+(100),(0)) when [UserType]='B2B2C' then ([AdminID]+'-C-')+CONVERT([varchar](30),[ID]+(100),(0)) when [UserType]='B2B2B' then ([AdminID]+'-SA-')+CONVERT([varchar](30),[ID]+(100),(0)) when [UserType]='B2B2B2ST' then ([AdminID]+'-ST-')+CONVERT([varchar](30),[ID]+(100),(0)) when [UserType]='A2D' then 'D-'+CONVERT([varchar](30),[ID]+(100),(0)) else 'AD-'+CONVERT([varchar](30),[ID]+(100),(0)) end)", false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.CompanyLogo)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CompanyMobile)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Company_Mobile");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CompanyPhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Company_PhoneNo");
            entity.Property(e => e.Consolidators)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CorporateAgent).HasDefaultValue(false);
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DirectDebitAgent)
                .HasDefaultValue(false)
                .HasColumnName("Direct_Debit_Agent");
            entity.Property(e => e.DistributorAgent).HasDefaultValue(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FaxNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fdfares)
                .HasDefaultValue(false)
                .HasColumnName("FDfares");
            entity.Property(e => e.Fdmarkup)
                .HasDefaultValue(0)
                .HasColumnName("FDMarkup");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gst)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("GST");
            entity.Property(e => e.GstName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("GST_Name");
            entity.Property(e => e.GtgNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GTG_No");
            entity.Property(e => e.Host)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.IataMemberSinceYrs)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IATA_member_since_yrs");
            entity.Property(e => e.IataNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IATA_No");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IP");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LicenseNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("License_No");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MonthlyBookingVolume)
                .HasDefaultValue(0)
                .HasColumnName("Monthly_Booking_Volume");
            entity.Property(e => e.NtnNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NTN_No");
            entity.Property(e => e.OfficeSpace)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Office_Space");
            entity.Property(e => e.PanName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Pan_Name");
            entity.Property(e => e.PanNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Pan_No");
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode).HasDefaultValue(0);
            entity.Property(e => e.Pwd)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Reference)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ReferredBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.SafiCharge)
                .HasDefaultValue(false)
                .HasColumnName("Safi_Charge");
            entity.Property(e => e.SelfBilling)
                .HasDefaultValue(false)
                .HasColumnName("Self_billing");
            entity.Property(e => e.ServiceTaxNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Service_TaxNo");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StaxNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TanNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Tan_No");
            entity.Property(e => e.TdsAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("TDS_Amount");
            entity.Property(e => e.TdsExemption)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("TDS_exemption");
            entity.Property(e => e.Title)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.TotalBranches)
                .HasDefaultValue(0)
                .HasColumnName("Total_Branches");
            entity.Property(e => e.TotalEmployee)
                .HasDefaultValue(0)
                .HasColumnName("Total_Employee");
            entity.Property(e => e.TpinNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TPIN_No");
            entity.Property(e => e.TtaNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TTA_No");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VatNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VAT_No");
            entity.Property(e => e.WhitelabelAgent).HasDefaultValue(false);
            entity.Property(e => e.YrsInBusiness)
                .HasDefaultValue(0)
                .HasColumnName("Yrs_in_business");
        });

        modelBuilder.Entity<CompanyRegisterAttachment>(entity =>
        {
            entity.ToTable("Company_Register_Attachment");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AadharCard)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Bill)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Dl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("DL");
            entity.Property(e => e.Pan)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Passport)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Photo)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.VoterId)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("VoterID");
        });

        modelBuilder.Entity<CompanyRegisterCorporateDetail>(entity =>
        {
            entity.ToTable("Company_Register_Corporate_Detail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccountEmail1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Account_Email1");
            entity.Property(e => e.AccountEmail2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Account_Email2");
            entity.Property(e => e.AccountMobile1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Account_Mobile1");
            entity.Property(e => e.AccountMobile2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Account_Mobile2");
            entity.Property(e => e.AccountName1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Account_Name1");
            entity.Property(e => e.AccountName2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Account_Name2");
            entity.Property(e => e.AccountPhone1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Account_Phone1");
            entity.Property(e => e.AccountPhone2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Account_Phone2");
            entity.Property(e => e.AuthorityEmail1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Authority_Email1");
            entity.Property(e => e.AuthorityEmail2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Authority_Email2");
            entity.Property(e => e.AuthorityMobile1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Authority_Mobile1");
            entity.Property(e => e.AuthorityMobile2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Authority_Mobile2");
            entity.Property(e => e.AuthorityName1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Authority_Name1");
            entity.Property(e => e.AuthorityName2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Authority_Name2");
            entity.Property(e => e.AuthorityPhone1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Authority_Phone1");
            entity.Property(e => e.AuthorityPhone2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Authority_Phone2");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.ContactEmail1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Contact_Email1");
            entity.Property(e => e.ContactEmail2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Contact_Email2");
            entity.Property(e => e.ContactMobile1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Contact_Mobile1");
            entity.Property(e => e.ContactMobile2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Contact_Mobile2");
            entity.Property(e => e.ContactName1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Contact_Name1");
            entity.Property(e => e.ContactName2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Contact_Name2");
            entity.Property(e => e.ContactPhone1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Contact_Phone1");
            entity.Property(e => e.ContactPhone2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Contact_Phone2");
            entity.Property(e => e.ManagementEmail1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Management_Email1");
            entity.Property(e => e.ManagementEmail2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Management_Email2");
            entity.Property(e => e.ManagementMobile1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Management_Mobile1");
            entity.Property(e => e.ManagementMobile2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Management_Mobile2");
            entity.Property(e => e.ManagementName1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Management_Name1");
            entity.Property(e => e.ManagementName2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Management_Name2");
            entity.Property(e => e.ManagementPhone1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Management_Phone1");
            entity.Property(e => e.ManagementPhone2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Management_Phone2");
        });

        modelBuilder.Entity<CompanyRegisterCorporateUser>(entity =>
        {
            entity.ToTable("Company_Register_Corporate_Users");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
        });

        modelBuilder.Entity<CompanyRegisterCorporateUsersLimit>(entity =>
        {
            entity.ToTable("Company_Register_Corporate_Users_Limits");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Limit).HasDefaultValue(0);
        });

        modelBuilder.Entity<CompanyRegisterGst>(entity =>
        {
            entity.ToTable("Company_Register_Gst");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GstcompanyAddress)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("GSTCompanyAddress");
            entity.Property(e => e.GstcompanyContactNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("GSTCompanyContactNumber");
            entity.Property(e => e.GstcompanyEmail)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("GSTCompanyEmail");
            entity.Property(e => e.GstcompanyName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("GSTCompanyName");
            entity.Property(e => e.Gstnumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GSTNumber");
        });

        modelBuilder.Entity<CompanyRegisterProduct>(entity =>
        {
            entity.ToTable("Company_Register_Product");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AirCalender).HasDefaultValue(true);
            entity.Property(e => e.Bus).HasDefaultValue(false);
            entity.Property(e => e.Car).HasDefaultValue(true);
            entity.Property(e => e.Cc)
                .HasDefaultValue(true)
                .HasColumnName("CC");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Cruise).HasDefaultValue(false);
            entity.Property(e => e.Customer).HasDefaultValue(false);
            entity.Property(e => e.Dc)
                .HasDefaultValue(true)
                .HasColumnName("DC");
            entity.Property(e => e.Dmt)
                .HasDefaultValue(false)
                .HasColumnName("DMT");
            entity.Property(e => e.Flight).HasDefaultValue(true);
            entity.Property(e => e.FxdDepartures)
                .HasDefaultValue(false)
                .HasColumnName("Fxd_Departures");
            entity.Property(e => e.Gdsterminal)
                .HasDefaultValue(false)
                .HasColumnName("GDSTerminal");
            entity.Property(e => e.Groups).HasDefaultValue(true);
            entity.Property(e => e.Holiday).HasDefaultValue(true);
            entity.Property(e => e.Hotel).HasDefaultValue(true);
            entity.Property(e => e.HotelDepartures)
                .HasDefaultValue(false)
                .HasColumnName("Hotel_Departures");
            entity.Property(e => e.ImportPnr)
                .HasDefaultValue(false)
                .HasColumnName("Import_PNR");
            entity.Property(e => e.Insurance).HasDefaultValue(false);
            entity.Property(e => e.Netbanking).HasDefaultValue(true);
            entity.Property(e => e.OfflineBooking)
                .HasDefaultValue(false)
                .HasColumnName("Offline_Booking");
            entity.Property(e => e.Package).HasDefaultValue(false);
            entity.Property(e => e.Prepaid).HasDefaultValue(false);
            entity.Property(e => e.Railway).HasDefaultValue(false);
            entity.Property(e => e.Recharge).HasDefaultValue(true);
            entity.Property(e => e.Staff).HasDefaultValue(false);
            entity.Property(e => e.SubCompany).HasDefaultValue(false);
            entity.Property(e => e.Tour).HasDefaultValue(false);
            entity.Property(e => e.Upi)
                .HasDefaultValue(true)
                .HasColumnName("UPI");
            entity.Property(e => e.Visa).HasDefaultValue(true);
            entity.Property(e => e.Wallet).HasDefaultValue(false);
        });

        modelBuilder.Entity<CompanyRegisterProductAdded>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Company_Register_Product_Added");

            entity.Property(e => e.AirDepartures).HasDefaultValue(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.HotelDepartures).HasDefaultValue(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Tours).HasDefaultValue(false);
        });

        modelBuilder.Entity<CompanyRoleModuleProcuctRegisterOperationManagement>(entity =>
        {
            entity.ToTable("Company_Role_Module_Procuct_Register_Operation_Management");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Edit).HasDefaultValue(false);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ProductStatus).HasDefaultValue(true);
            entity.Property(e => e.Show).HasDefaultValue(false);
        });

        modelBuilder.Entity<CompanyTransactionDetail>(entity =>
        {
            entity.ToTable("Company_Transaction_Detail", tb =>
                {
                    tb.HasTrigger("Trigg_Company_Transaction_Detail_Delete");
                    tb.HasTrigger("Trigg_Company_Transaction_Detail_Insert");
                    tb.HasTrigger("Trigg_Company_Transaction_Detail_update");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Balance)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Credit)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Debit)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EventId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EventID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsAirline).HasDefaultValue(false);
            entity.Property(e => e.IsAirlineOff).HasDefaultValue(false);
            entity.Property(e => e.IsHotel).HasDefaultValue(false);
            entity.Property(e => e.IsHotelOff).HasDefaultValue(false);
            entity.Property(e => e.PaxSegmentId)
                .HasDefaultValue(0)
                .HasColumnName("PaxSegmentID");
            entity.Property(e => e.PaymentId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PaymentID");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remark)
                .HasMaxLength(900)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CompanyTransactionLedgerEvent>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Company_Transaction_Ledger_Events");

            entity.Property(e => e.EventId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EventID");
            entity.Property(e => e.EventName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        modelBuilder.Entity<CorporateRole>(entity =>
        {
            entity.ToTable("CorporateRole");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Edit).HasDefaultValue(false);
            entity.Property(e => e.ProductId)
                .HasDefaultValue(0)
                .HasColumnName("ProductID");
            entity.Property(e => e.Show).HasDefaultValue(false);
        });

        modelBuilder.Entity<CountryMaster>(entity =>
        {
            entity.HasKey(e => e.CountryId);

            entity.ToTable("CountryMaster");

            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CountryName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CurrencyDatum>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Value)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<CustomerFareDetailAirline1>(entity =>
        {
            entity.ToTable("Customer_Fare_Detail_Airline1");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.CompanyCredit)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyDebit)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("CompanyID");
            entity.Property(e => e.CompanyTds)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CustomerCredit)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CustomerDebit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Markup).HasDefaultValue(0);
        });

        modelBuilder.Entity<CustomerFareDetailHotel>(entity =>
        {
            entity.ToTable("Customer_Fare_Detail_Hotel");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyCredit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyDebit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.CompanyTds)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CustomerCredit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CustomerDebit).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<CustomerRefund>(entity =>
        {
            entity.ToTable("Customer_Refunds");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Passengerid).HasDefaultValue(0);
            entity.Property(e => e.Pgcharges)
                .HasDefaultValue(0)
                .HasColumnName("PGcharges");
            entity.Property(e => e.RefundAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Refund_Amount");
            entity.Property(e => e.RefundEventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RefundStatus).HasDefaultValue(false);
        });

        modelBuilder.Entity<DealDetail>(entity =>
        {
            entity.ToTable("Deal_Detail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Business)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Economy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Iata)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IATA");
            entity.Property(e => e.VendorId).HasColumnName("VendorID");
        });

        modelBuilder.Entity<DealValidity>(entity =>
        {
            entity.ToTable("Deal_Validity");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Validity)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DealVendor>(entity =>
        {
            entity.ToTable("Deal_Vendor", tb => tb.HasTrigger("Deal_Vendor_Trigg"));

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.VendorDetail)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.VendorId)
                .HasComputedColumnSql("([ID]+(100))", false)
                .HasColumnName("VendorID");
            entity.Property(e => e.VendorName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DeparHotelAdd>(entity =>
        {
            entity.ToTable("Depar_Hotel_Add");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddressOnMap)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("address_on_map");
            entity.Property(e => e.Attractions)
                .IsUnicode(false)
                .HasColumnName("attractions");
            entity.Property(e => e.Commission)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("commission");
            entity.Property(e => e.Commissiontype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("commissiontype");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.FromDate).HasColumnName("from_date");
            entity.Property(e => e.HotelAddress)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("hotel_address");
            entity.Property(e => e.HotelContact)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("hotel_contact");
            entity.Property(e => e.HotelDescription)
                .IsUnicode(false)
                .HasColumnName("hotel_description");
            entity.Property(e => e.HotelEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("hotel_email");
            entity.Property(e => e.HotelMobile)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("hotel_mobile");
            entity.Property(e => e.HotelName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("hotel_name");
            entity.Property(e => e.HotelNearPoint)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("hotel_near_point");
            entity.Property(e => e.HotelPhone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("hotel_phone");
            entity.Property(e => e.HotelShortDescription)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("hotel_short_description");
            entity.Property(e => e.HotelType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("hotel_type");
            entity.Property(e => e.HotelWebiste)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("hotel_webiste");
            entity.Property(e => e.Latitude)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("latitude");
            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.Longitude)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("longitude");
            entity.Property(e => e.Refid)
                .HasComputedColumnSql("([id]+(100000))", true)
                .HasColumnName("refid");
            entity.Property(e => e.SpecialInstructions)
                .IsUnicode(false)
                .HasColumnName("special_instructions");
            entity.Property(e => e.Stars).HasDefaultValue(0);
            entity.Property(e => e.Status)
                .HasDefaultValue(false)
                .HasColumnName("status");
            entity.Property(e => e.ToDate).HasColumnName("to_date");
            entity.Property(e => e.Vat)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("vat");
            entity.Property(e => e.Vattype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("vattype");
        });

        modelBuilder.Entity<DeparHotelAddFacility>(entity =>
        {
            entity.ToTable("Depar_Hotel_Add_Facilities");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Facilities)
                .IsUnicode(false)
                .HasColumnName("facilities");
            entity.Property(e => e.Refid)
                .HasDefaultValue(0)
                .HasColumnName("refid");
        });

        modelBuilder.Entity<DeparHotelAddImage>(entity =>
        {
            entity.ToTable("Depar_Hotel_Add_Images");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Images)
                .IsUnicode(false)
                .HasColumnName("images");
            entity.Property(e => e.ImagesOrder)
                .HasDefaultValue(0)
                .HasColumnName("images_order");
            entity.Property(e => e.Refid)
                .HasDefaultValue(0)
                .HasColumnName("refid");
        });

        modelBuilder.Entity<DeparHotelAddMetum>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Depar_Hotel_Add_meta");

            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.MetaDescription)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("meta_description");
            entity.Property(e => e.MetaKeywords)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("meta_keywords");
            entity.Property(e => e.MetaTitle)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("meta_title");
            entity.Property(e => e.Refid)
                .HasDefaultValue(0)
                .HasColumnName("refid");
        });

        modelBuilder.Entity<DeparHotelAddPolicy>(entity =>
        {
            entity.ToTable("Depar_Hotel_Add_Policy");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CancellationPolicy)
                .IsUnicode(false)
                .HasColumnName("cancellation_policy");
            entity.Property(e => e.Checkin)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("checkin");
            entity.Property(e => e.Checkout)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("checkout");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.PaymentOptions)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("payment_options");
            entity.Property(e => e.PolicyTerms)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("policy_terms");
            entity.Property(e => e.Refid)
                .HasDefaultValue(0)
                .HasColumnName("refid");
        });

        modelBuilder.Entity<DeparHotelAddRoom>(entity =>
        {
            entity.ToTable("Depar_Hotel_Add_Rooms");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adults)
                .HasDefaultValue(0)
                .HasColumnName("adults");
            entity.Property(e => e.Amenities)
                .IsUnicode(false)
                .HasColumnName("amenities");
            entity.Property(e => e.Childs)
                .HasDefaultValue(0)
                .HasColumnName("childs");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.ExtraBeds)
                .HasDefaultValue(0)
                .HasColumnName("extra_beds");
            entity.Property(e => e.Inclusion)
                .IsUnicode(false)
                .HasColumnName("inclusion");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0)
                .HasColumnName("quantity");
            entity.Property(e => e.Refid)
                .HasDefaultValue(0)
                .HasColumnName("refid");
            entity.Property(e => e.RoomDescription)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("room_description");
            entity.Property(e => e.RoomShortDescription)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("room_short_description");
            entity.Property(e => e.RoomType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("room_type");
            entity.Property(e => e.Roomid).HasColumnName("roomid");
            entity.Property(e => e.Services)
                .IsUnicode(false)
                .HasColumnName("services");
            entity.Property(e => e.Status)
                .HasDefaultValue(false)
                .HasColumnName("status");
        });

        modelBuilder.Entity<DeparHotelAddRoomsPrice>(entity =>
        {
            entity.ToTable("Depar_Hotel_Add_Rooms_Prices");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adults)
                .HasDefaultValue(0)
                .HasColumnName("adults");
            entity.Property(e => e.Childs)
                .HasDefaultValue(0)
                .HasColumnName("childs");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.ExtraBeds)
                .HasDefaultValue(0)
                .HasColumnName("extra_beds");
            entity.Property(e => e.ExtraBedsChgs)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("extra_beds_chgs");
            entity.Property(e => e.FriChgs)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("fri_chgs");
            entity.Property(e => e.FromDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("from_date");
            entity.Property(e => e.MonChgs)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("mon_chgs");
            entity.Property(e => e.Priceid).HasColumnName("priceid");
            entity.Property(e => e.Refid)
                .HasDefaultValue(0)
                .HasColumnName("refid");
            entity.Property(e => e.Roomid)
                .HasDefaultValue(0)
                .HasColumnName("roomid");
            entity.Property(e => e.SatChgs)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("sat_chgs");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.SunChgs)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("sun_chgs");
            entity.Property(e => e.ThuChgs)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("thu_chgs");
            entity.Property(e => e.ToDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("to_date");
            entity.Property(e => e.TueChgs)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tue_chgs");
            entity.Property(e => e.WedChgs)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("wed_chgs");
        });

        modelBuilder.Entity<DeparHotelAmenity>(entity =>
        {
            entity.ToTable("Depar_Hotel_Amenities");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amenity)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("amenity");
        });

        modelBuilder.Entity<DeparHotelFacility>(entity =>
        {
            entity.ToTable("Depar_Hotel_Facilities");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Facilities)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("facilities");
        });

        modelBuilder.Entity<DeparHotelInclusion>(entity =>
        {
            entity.ToTable("Depar_Hotel_Inclusions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Inclusion)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("inclusion");
        });

        modelBuilder.Entity<DeparHotelInstruction>(entity =>
        {
            entity.ToTable("Depar_Hotel_Instructions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Instructions)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("instructions");
        });

        modelBuilder.Entity<DeparHotelRole>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Depar_Hotel_Role");

            entity.Property(e => e.Edit).HasDefaultValue(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.ProductId)
                .HasDefaultValue(0)
                .HasColumnName("ProductID");
            entity.Property(e => e.Show).HasDefaultValue(false);
        });

        modelBuilder.Entity<DeparHotelRoomType>(entity =>
        {
            entity.ToTable("Depar_Hotel_Room_Types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoomTypes)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("room_types");
        });

        modelBuilder.Entity<DeparHotelType>(entity =>
        {
            entity.ToTable("Depar_Hotel_Types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.HotelType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("hotel_type");
        });

        modelBuilder.Entity<DeparHotelentryDetail>(entity =>
        {
            entity.ToTable("Depar_Hotelentry_Detail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AddressLine1)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Adt).HasDefaultValue(0);
            entity.Property(e => e.BookingRef).HasComputedColumnSql("([id]+(1000))", false);
            entity.Property(e => e.Chd).HasDefaultValue(0);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Commission)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("commission");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.ConfirmationNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CountryCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.HotelName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.IsRejected).HasDefaultValue(false);
            entity.Property(e => e.IsUpdated).HasDefaultValue(false);
            entity.Property(e => e.LastCancellationDate).HasColumnType("datetime");
            entity.Property(e => e.Latitude)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Longitude)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NoOfRooms).HasDefaultValue(0);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StaffID");
            entity.Property(e => e.StarRating)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID");
            entity.Property(e => e.TotalFare).HasDefaultValue(0);
            entity.Property(e => e.Vat)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("vat");
        });

        modelBuilder.Entity<DeparHotelentryDetailDescription>(entity =>
        {
          
            entity.HasKey(e => e.BookingRef);
            entity.Property(e => e.BookingRef)
                .ValueGeneratedOnAdd() 
                .HasColumnName("booking_ref");


            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");

            entity.Property(e => e.Amenities)
                .IsUnicode(false)
                .HasColumnName("amenities");
            entity.Property(e => e.Amenity)
                .IsUnicode(false)
                .HasColumnName("amenity");
            entity.Property(e => e.Attractions)
                .IsUnicode(false)
                .HasColumnName("attractions");
            entity.Property(e => e.CancellationPolicy)
                .IsUnicode(false)
                .HasColumnName("cancellation_policy");
            entity.Property(e => e.Commission)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("commission");
            entity.Property(e => e.Commissiontype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("commissiontype");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Eventtime)
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Facilities)
                .IsUnicode(false)
                .HasColumnName("facilities");
            entity.Property(e => e.HotelContact)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("hotel_contact");
            entity.Property(e => e.HotelDescription)
                .IsUnicode(false)
                .HasColumnName("hotel_description");
            entity.Property(e => e.HotelEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("hotel_email");
            entity.Property(e => e.HotelMobile)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("hotel_mobile");
            entity.Property(e => e.HotelNearPoint)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("hotel_near_point");
            entity.Property(e => e.HotelPhone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("hotel_phone");
            entity.Property(e => e.HotelShortDescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("hotel_short_description");
            entity.Property(e => e.HotelType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("hotel_type");
            entity.Property(e => e.HotelWebiste)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("hotel_webiste");
            entity.Property(e => e.Inclusion)
                .IsUnicode(false)
                .HasColumnName("inclusion");
            entity.Property(e => e.PolicyTerms)
                .IsUnicode(false)
                .HasColumnName("policy_terms");
            entity.Property(e => e.Services)
                .IsUnicode(false)
                .HasColumnName("services");
            entity.Property(e => e.SpecialInstructions)
                .IsUnicode(false)
                .HasColumnName("special_instructions");
            entity.Property(e => e.Vat)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("vat");
            entity.Property(e => e.Vattype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("vattype");
        });

        modelBuilder.Entity<DeparHotelentryPassengerDetail>(entity =>
        {
       
            entity.HasKey(e => e.BookingRef);

      
            entity.ToTable("Depar_Hotelentry_Passenger_Detail");

            
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");

            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);

          
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd() 
                .HasColumnName("ID");

            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);

            
            entity.Property(e => e.BookingRef)
                .ValueGeneratedNever() 
                .HasColumnName("booking_ref");
        });

        modelBuilder.Entity<DeparHotelentryRoomDetail>(entity =>
        {
            entity.HasKey(e => e.BookingRef);

            entity.ToTable("Depar_Hotelentry_Room_Detail");

            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");

            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");

           
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd() 
                .HasColumnName("id");

         
            entity.Property(e => e.BookingRef)
                .ValueGeneratedNever() 
                .HasColumnName("booking_ref");

            entity.Property(e => e.RoomDescription)
                .IsUnicode(false)
                .HasColumnName("room_description");

            entity.Property(e => e.RoomShortDescription)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("room_short_description");

            entity.Property(e => e.RoomType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("room_type");
        });


        modelBuilder.Entity<DepartureFare>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ArrivalStation)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.ArrivalTerminal)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.DepartureArrivalDate)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.DepartureArrivalTime)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.DepartureCarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.DepartureDate)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.DepartureStation)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.DepartureTerminal)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.DepartureTime)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.EventTime).HasColumnType("datetime");
            entity.Property(e => e.FareType)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.ImgShow).HasDefaultValue(true);
            entity.Property(e => e.Pnr)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("PNR");
            entity.Property(e => e.RefId)
                .HasComputedColumnSql("([ID]+(1000))", false)
                .HasColumnName("RefID");
            entity.Property(e => e.ReturnArrivalDate)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.ReturnArrivalTime)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.ReturnCarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.ReturnDate)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.ReturnStops).HasDefaultValue(0);
            entity.Property(e => e.ReturnTime)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID");
        });

        modelBuilder.Entity<DepartureFaresBook>(entity =>
        {
            entity.ToTable("DepartureFares_book");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ArrivalStation)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.ArrivalTerminal)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.BookingRef).HasComputedColumnSql("([ID]+(1000))", false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.DepartureArrivalDate)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.DepartureArrivalTime)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.DepartureCarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.DepartureDate)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.DepartureStation)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.DepartureStops).HasDefaultValue(0);
            entity.Property(e => e.DepartureTerminal)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.DepartureTime)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Fare).HasDefaultValue(0);
            entity.Property(e => e.Gst).HasDefaultValue(0);
            entity.Property(e => e.Markup).HasDefaultValue(0);
            entity.Property(e => e.NoOfPassengers).HasDefaultValue(0);
            entity.Property(e => e.Pnr)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("PNR");
            entity.Property(e => e.RefId)
                .HasDefaultValue(0)
                .HasColumnName("RefID");
            entity.Property(e => e.Reject).HasDefaultValue(false);
            entity.Property(e => e.ReturnArrivalDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ReturnArrivalTime)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ReturnCarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ReturnDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ReturnStops).HasDefaultValue(0);
            entity.Property(e => e.ReturnTime)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID");
            entity.Property(e => e.TotalFare).HasDefaultValue(0);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<DepartureFaresPassengerBook>(entity =>
        {
            entity.ToTable("DepartureFares_Passenger_book");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Age).HasDefaultValue(0);
            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("CompanyID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Title)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<DepartureRole>(entity =>
        {
            entity.ToTable("DepartureRole");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
        });

        modelBuilder.Entity<DmcCountryCityList>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DmcCountryCityList");

            entity.Property(e => e.CityName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CountryName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        modelBuilder.Entity<FdagentRole>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("FDAgentRole");

            entity.Property(e => e.Edit).HasDefaultValue(true);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.ProductId)
                .HasDefaultValue(0)
                .HasColumnName("ProductID");
            entity.Property(e => e.Show).HasDefaultValue(true);
        });

        modelBuilder.Entity<FlightOwnSegmentsPnr>(entity =>
        {
            entity.ToTable("Flight_Own_Segments_Pnr");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FltType)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.PnrAirline)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Pnr_Airline");
            entity.Property(e => e.PnrGalileo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Pnr_Galileo");
        });

        modelBuilder.Entity<FlightSearchCache>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Flight_Search_Cache");

            entity.Property(e => e.Createdon)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdon");
            entity.Property(e => e.FareRule).HasColumnType("ntext");
            entity.Property(e => e.Fidx).HasColumnName("FIdx");
            entity.Property(e => e.Flight).HasColumnType("ntext");
            entity.Property(e => e.Gds)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Segment)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FlightSearchCacheMaster>(entity =>
        {
            entity.HasKey(e => e.CacheId).HasName("PK_FlightSearchCacheMaster");

            entity.ToTable("Flight_Search_Cache_Master");

            entity.Property(e => e.CompanyId)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Guid)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifyOn).HasColumnType("datetime");
            entity.Property(e => e.SearchRequest).HasColumnType("ntext");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<GoFirstSsr>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("GoFirstSSR");

            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.IsMoreThan10Hour).HasDefaultValue(false);
            entity.Property(e => e.ProductClass)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Ssrtype)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("SSRType");
        });

        modelBuilder.Entity<GroupCommission>(entity =>
        {
            entity.ToTable("Group_Commission");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GroupId)
                .HasComputedColumnSql("([ID]+(100))", false)
                .HasColumnName("GroupID");
            entity.Property(e => e.GroupName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationGroup).HasDefaultValue(false);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<GroupCommissionDRule>(entity =>
        {
            entity.ToTable("Group_Commission_D_Rule");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AutoPnr)
                .HasDefaultValue(false)
                .HasColumnName("Auto_PNR");
            entity.Property(e => e.AutoTkt)
                .HasDefaultValue(false)
                .HasColumnName("Auto_Tkt");
            entity.Property(e => e.Basic)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("BASIC");
            entity.Property(e => e.BasicIata)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Basic_Iata");
            entity.Property(e => e.BookingClass)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.BookingClassNotValid)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("BookingClass_notValid");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Cb)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("CB");
            entity.Property(e => e.GroupId)
                .HasDefaultValue(0)
                .HasColumnName("GroupID");
            entity.Property(e => e.Priority).HasDefaultValue(0);
            entity.Property(e => e.Promo)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("PROMO");
            entity.Property(e => e.Sf)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("SF");
            entity.Property(e => e.SplFare).HasDefaultValue(false);
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SupplierID");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Supplier_Name");
            entity.Property(e => e.Yq)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("YQ");
            entity.Property(e => e.YqIata)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("YQ_Iata");
        });

        modelBuilder.Entity<GroupCommissionDRuleCN>(entity =>
        {
            entity.ToTable("Group_Commission_D_Rule_C_N");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Cb)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("CB");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Markup).HasDefaultValue(0);
        });

        modelBuilder.Entity<GroupCommissionDRuleSa>(entity =>
        {
            entity.ToTable("Group_Commission_D_Rule_SA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Commission)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.GroupId)
                .HasDefaultValue(0)
                .HasColumnName("GroupID");
        });

        modelBuilder.Entity<GroupCommissionIRule>(entity =>
        {
            entity.ToTable("Group_Commission_I_Rule");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AutoPnr)
                .HasDefaultValue(false)
                .HasColumnName("Auto_PNR");
            entity.Property(e => e.AutoTkt)
                .HasDefaultValue(false)
                .HasColumnName("Auto_Tkt");
            entity.Property(e => e.Basic)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("BASIC");
            entity.Property(e => e.BasicIata)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Basic_Iata");
            entity.Property(e => e.BookingClass)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.BookingClassNotValid)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("BookingClass_notValid");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Cb)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("CB");
            entity.Property(e => e.DestinationCountry)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.GroupId)
                .HasDefaultValue(0)
                .HasColumnName("GroupID");
            entity.Property(e => e.OriginCountry)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Priority).HasDefaultValue(0);
            entity.Property(e => e.Promo)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("PROMO");
            entity.Property(e => e.Sf)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("SF");
            entity.Property(e => e.SplFare).HasDefaultValue(false);
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SupplierID");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Supplier_Name");
            entity.Property(e => e.Yq)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("YQ");
            entity.Property(e => e.YqIata)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("YQ_Iata");
        });

        modelBuilder.Entity<GroupCommissionIRuleCN>(entity =>
        {
            entity.ToTable("Group_Commission_I_Rule_C_N");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Cb)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("CB");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Markup).HasDefaultValue(0);
        });

        modelBuilder.Entity<GroupCommissionIRuleSa>(entity =>
        {
            entity.ToTable("Group_Commission_I_Rule_SA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Commission)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.GroupId)
                .HasDefaultValue(0)
                .HasColumnName("GroupID");
        });

        modelBuilder.Entity<GroupCommissionPriceType>(entity =>
        {
            entity.ToTable("Group_Commission_PriceType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Basic)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Cb)
                .HasDefaultValue(0)
                .HasColumnName("CB");
            entity.Property(e => e.Pnr).HasDefaultValue(false);
            entity.Property(e => e.PriceType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Sf)
                .HasDefaultValue(0)
                .HasColumnName("SF");
            entity.Property(e => e.Show).HasDefaultValue(true);
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.Supplierid)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Yq)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<GroupCommissionSa>(entity =>
        {
            entity.ToTable("Group_Commission_SA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GroupId)
                .HasComputedColumnSql("([ID]+(100))", false)
                .HasColumnName("GroupID");
            entity.Property(e => e.GroupName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationGroup).HasDefaultValue(false);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<GroupCompanyCommission>(entity =>
        {
            entity.ToTable("Group_Company_Commission");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.GroupId)
                .HasDefaultValue(0)
                .HasColumnName("GroupID");
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<GroupCompanyCommissionSa>(entity =>
        {
            entity.ToTable("Group_Company_Commission_SA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.GroupId)
                .HasDefaultValue(0)
                .HasColumnName("GroupID");
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<GstState>(entity =>
        {
            entity.ToTable("Gst_States");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GstCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StateCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Ut)
                .HasDefaultValue(false)
                .HasColumnName("UT");
        });

        modelBuilder.Entity<GstStateCity>(entity =>
        {
            entity.ToTable("Gst_State_City");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StateCode)
                .HasMaxLength(2)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HelpFile>(entity =>
        {
            entity.ToTable("Help_file");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Heading)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Pageid)
                .HasComputedColumnSql("([id]+(1000))", false)
                .HasColumnName("pageid");
            entity.Property(e => e.Pagename)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HelpFileDescription>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Help_file_description");

            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.IsHeader).HasDefaultValue(false);
            entity.Property(e => e.Orderby)
                .HasDefaultValue(0)
                .HasColumnName("orderby");
            entity.Property(e => e.Pageid)
                .HasDefaultValue(0)
                .HasColumnName("pageid");
        });

        modelBuilder.Entity<HolidaysQuery>(entity =>
        {
            entity.ToTable("Holidays_Query");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.ContactPerson)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Destination)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remark)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(false);
        });

        modelBuilder.Entity<HotelCallCenter>(entity =>
        {
            entity.ToTable("Hotel_CallCenter");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StaffID");
            entity.Property(e => e.Status).HasDefaultValue(false);
        });

        modelBuilder.Entity<HotelCityList>(entity =>
        {
            entity.ToTable("Hotel_CityList");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CityCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CityName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(1);
        });

        modelBuilder.Entity<HotelCityStaticList>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Hotel_CityStaticList");

            entity.Property(e => e.CityId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CityName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        modelBuilder.Entity<HotelCommission>(entity =>
        {
            entity.ToTable("Hotel_Commission");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Commission)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PnrStatus).HasDefaultValue(false);
            entity.Property(e => e.ServiceCharge)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<HotelCommissionSa>(entity =>
        {
            entity.ToTable("Hotel_Commission_SA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Commission)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("CompanyID");
        });

        modelBuilder.Entity<HotelCountryList>(entity =>
        {
            entity.HasKey(e => e.CountryCode);

            entity.ToTable("Hotel_CountryList");

            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.CountryName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
        });

        modelBuilder.Entity<HotelDetail>(entity =>
        {
            entity.ToTable("Hotel_Detail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AddressLine1)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.BookingId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BookingRef).HasComputedColumnSql("([ID]+(100))", false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Commission).HasDefaultValue(0);
            entity.Property(e => e.CommissionSa)
                .HasDefaultValue(0)
                .HasColumnName("Commission_SA");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.ConfirmationNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CountryCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Destination)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.HotelBookingStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HotelName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.HotelPolicyDetail).IsUnicode(false);
            entity.Property(e => e.InfoSource)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceAmount).HasDefaultValue(0);
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsCancelRequested).HasDefaultValue(false);
            entity.Property(e => e.IsCanceled).HasDefaultValue(false);
            entity.Property(e => e.IsCanceledRejected).HasDefaultValue(false);
            entity.Property(e => e.IsCancellationPolicyChanged).HasDefaultValue(false);
            entity.Property(e => e.IsHotelPolicyChanged).HasDefaultValue(false);
            entity.Property(e => e.IsPriceChanged).HasDefaultValue(false);
            entity.Property(e => e.IsRejected).HasDefaultValue(false);
            entity.Property(e => e.IsRescheduled).HasDefaultValue(false);
            entity.Property(e => e.IsUnderCancellationAllowed).HasDefaultValue(false);
            entity.Property(e => e.IsUpdated).HasDefaultValue(false);
            entity.Property(e => e.LastCancellationDate).HasColumnType("datetime");
            entity.Property(e => e.Latitude)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Longitude)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MakerId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MakerID");
            entity.Property(e => e.Markup).HasDefaultValue(0);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.ServiceCharge).HasDefaultValue(0);
            entity.Property(e => e.SpecialRequest).IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StaffID");
            entity.Property(e => e.StarRating)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID");
            entity.Property(e => e.Tds).HasDefaultValue(0);
            entity.Property(e => e.TdsSa)
                .HasDefaultValue(0)
                .HasColumnName("Tds_SA");
            entity.Property(e => e.TotalFare).HasDefaultValue(0);
            entity.Property(e => e.Totalcfee).HasDefaultValue(0);
            entity.Property(e => e.VoucherStatus).HasDefaultValue(false);
        });

        modelBuilder.Entity<HotelMarkup>(entity =>
        {
            entity.ToTable("Hotel_Markup");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.IsFixed).HasDefaultValue(false);
            entity.Property(e => e.IsPercent).HasDefaultValue(false);
            entity.Property(e => e.Markup).HasDefaultValue(0);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("D");
        });

        modelBuilder.Entity<HotelMarkupCommC>(entity =>
        {
            entity.ToTable("Hotel_Markup_Comm_C");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cb)
                .HasDefaultValue(0)
                .HasColumnName("CB");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.IsFixed).HasDefaultValue(false);
            entity.Property(e => e.IsPercent).HasDefaultValue(false);
            entity.Property(e => e.Markup).HasDefaultValue(0);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("D");
        });

        modelBuilder.Entity<HotelPassengerDetail>(entity =>
        {
            entity.ToTable("Hotel_Passenger_Detail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Age).HasDefaultValue(0);
            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GstcompanyAddress)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("GSTCompanyAddress");
            entity.Property(e => e.GstcompanyContactNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GSTCompanyContactNumber");
            entity.Property(e => e.GstcompanyEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("GSTCompanyEmail");
            entity.Property(e => e.GstcompanyName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("GSTCompanyName");
            entity.Property(e => e.Gstnumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GSTNumber");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PassportExpDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PassportIssueDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PassportNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaxId)
                .HasDefaultValue(0)
                .HasColumnName("PaxID");
            entity.Property(e => e.PaxSegmentId)
                .HasComputedColumnSql("([ID]+[BookingRef])", false)
                .HasColumnName("Pax_SegmentID");
            entity.Property(e => e.PaxType)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.RoomNo).HasDefaultValue(0);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HotelRejectDetail>(entity =>
        {
            entity.ToTable("Hotel_Reject_Detail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RejectDetail)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HotelSegmentDetail>(entity =>
        {
            entity.ToTable("Hotel_Segment_Detail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AgentCommission)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AgentMarkUp)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amenities).IsUnicode(false);
            entity.Property(e => e.Amenity).IsUnicode(false);
            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.CancellationPolicies).IsUnicode(false);
            entity.Property(e => e.CancellationPolicy).IsUnicode(false);
            entity.Property(e => e.ChildCharge)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.DayRates).IsUnicode(false);
            entity.Property(e => e.Discount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ExtraGuestCharge)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HotelSegmentId)
                .HasComputedColumnSql("(([ID]+[BookingRef])+(10))", false)
                .HasColumnName("Hotel_SegmentID");
            entity.Property(e => e.HotelSupplements).IsUnicode(false);
            entity.Property(e => e.Inclusion).IsUnicode(false);
            entity.Property(e => e.LastCancellationDate).HasColumnType("datetime");
            entity.Property(e => e.Markup).HasDefaultValue(0);
            entity.Property(e => e.OfferedPrice)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OfferedPriceRoundedOff).HasDefaultValue(0);
            entity.Property(e => e.OtherCharges)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PublishedPrice)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PublishedPriceRoundedOff).HasDefaultValue(0);
            entity.Property(e => e.RatePlan)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RatePlanCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RequireAllPaxDetails).HasDefaultValue(false);
            entity.Property(e => e.RoomPrice)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RoomPromotion).IsUnicode(false);
            entity.Property(e => e.RoomTypeCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoomTypeName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ServiceCharge).HasDefaultValue(0);
            entity.Property(e => e.ServiceTax)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SmokingPreference)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SupplierPrice)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Tax)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tds)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TDS");
            entity.Property(e => e.TotalFare).HasDefaultValue(0);
            entity.Property(e => e.TotalGstamount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalGSTAmount");
        });

        modelBuilder.Entity<HtmlFilght>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Destination)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("destination");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("sector");
            entity.Property(e => e.Source)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("source");
        });

        modelBuilder.Entity<HtmlProduct>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Description1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description3)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.HeadImageLocation)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.OrderNo).HasDefaultValue(0);
            entity.Property(e => e.PageName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductHeading)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Productid).HasComputedColumnSql("([id]+(100))", false);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Star).HasDefaultValue(0);
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<HtmlProductsImage>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HtmlProductsQuery>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("HtmlProductsQuery");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Eventtime).HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LogCompanyBalanceTransactionDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Log_Company_Balance_Transaction_Detail");

            entity.Property(e => e.AvailableBalance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Available_Balance");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.CreditAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Credit_Amount");
            entity.Property(e => e.CreditBalance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Credit_Balance");
            entity.Property(e => e.EventId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EventID");
            entity.Property(e => e.EventTime).HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.OutstandingBalance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Outstanding_Balance");
            entity.Property(e => e.TemporaryBalance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Temporary_Balance");
            entity.Property(e => e.TemporaryBalanceTime)
                .HasColumnType("datetime")
                .HasColumnName("Temporary_Balance_Time");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UpdateType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LogCompanyBalanceTransactionHistoryDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Log_Company_Balance_Transaction_History_Detail");

            entity.Property(e => e.AfterTransactionAvailableBalance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("After_Transaction_Available_balance");
            entity.Property(e => e.AfterTransactionTemporaryBalance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("After_Transaction_Temporary_Balance");
            entity.Property(e => e.BeforeTransactionAvailableBalance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Before_Transaction_Available_Balance");
            entity.Property(e => e.BeforeTransactionTemporaryBalance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Before_Transaction_Temporary_Balance");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.EventId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EventID");
            entity.Property(e => e.EventTime).HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.OutStandingBalance)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("OutStanding_Balance");
            entity.Property(e => e.Remark).IsUnicode(false);
            entity.Property(e => e.TransType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TransactionAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Transaction_Amount");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UpdateType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LogCompanyFlightDetailAirline>(entity =>
        {
            entity.ToTable("Log_Company_Flight_Detail_Airline");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AirlinePnrA)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Airline_PNR_A");
            entity.Property(e => e.AirlinePnrD)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Airline_PNR_D");
            entity.Property(e => e.CarrierCodeA)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("CarrierCode_A");
            entity.Property(e => e.CarrierCodeD)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("CarrierCode_D");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.DepartureDateA)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DepartureDate_A");
            entity.Property(e => e.DepartureDateD)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DepartureDate_D");
            entity.Property(e => e.Destination)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.EventTime).HasColumnType("datetime");
            entity.Property(e => e.GdsPnrA)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GDS_PNR_A");
            entity.Property(e => e.GdsPnrD)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GDS_PNR_D");
            entity.Property(e => e.MakerId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MakerID");
            entity.Property(e => e.Origin)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StaffID");
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID");
            entity.Property(e => e.SupplierIdA)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID_A");
            entity.Property(e => e.SupplierIdD)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID_D");
            entity.Property(e => e.TotalBaggage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalBasic).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalBasicDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalBasic_deal");
            entity.Property(e => e.TotalCbDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalCB_deal");
            entity.Property(e => e.TotalCommission).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalFare).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalImport).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalMarkup).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalMeal).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TotalPromoDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalPromo_deal");
            entity.Property(e => e.TotalSeat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalServiceFeeDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalServiceFee_deal");
            entity.Property(e => e.TotalServiceTax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalTax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalTds).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalYq).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalYqDeal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TotalYQ_deal");
            entity.Property(e => e.Trip)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<LogCompanyFlightSegmentDetailAirline>(entity =>
        {
            entity.ToTable("Log_Company_Flight_Segment_Detail_Airline");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AirlinePnr)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Airline_PNR");
            entity.Property(e => e.ArrivalDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ArrivalStation)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.ArrivalTerminal)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ArrivalTime)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Cabin)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.ClassOfService)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.ConnOrder)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.DepartureDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DepartureStation)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.DepartureTerminal)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DepartureTime)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Destination)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.EventTime).HasColumnType("datetime");
            entity.Property(e => e.FareBasisCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FareType)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.FlightNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FlightSegmentId).HasColumnName("Flight_SegmentID");
            entity.Property(e => e.GdsPnr)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GDS_PNR");
            entity.Property(e => e.Origin)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.ProductClass)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RefundType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ViaName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LogCompanyPaxDetailAirline>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Log_Company_Pax_Detail_Airline");

            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Dob)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EventTime).HasColumnType("datetime");
            entity.Property(e => e.Ffn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FFN");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("First_Name");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Last_Name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Middle_Name");
            entity.Property(e => e.MobileNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaxSegmentId).HasColumnName("Pax_SegmentID");
            entity.Property(e => e.PaxType)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.PpNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PpexpirayDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PPExpirayDate");
            entity.Property(e => e.PpissueDate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PPIssueDate");
            entity.Property(e => e.TicketNo).IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TourCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UpdateType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LogCompanyPaxSegmentCancellationDetailAirline>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Log_Company_Pax_Segment_Cancellation_Detail_Airline");

            entity.Property(e => e.CPaxSegmentId).HasColumnName("C_Pax_SegmentID");
            entity.Property(e => e.CanceledBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CancellationReason)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CancellationType)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Destination)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.EventTime).HasColumnType("datetime");
            entity.Property(e => e.FltType)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Ip)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Origin)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.PaxSegmentId).HasColumnName("Pax_SegmentID");
            entity.Property(e => e.Pnr)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("PNR");
            entity.Property(e => e.Remark).IsUnicode(false);
            entity.Property(e => e.TicketNo)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UpdateType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LogCompanyPaxSegmentCancellationDetailAirlineRefund>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Log_Company_Pax_Segment_Cancellation_Detail_Airline_Refund");

            entity.Property(e => e.AirlineCharges).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Baggage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Basic).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CPaxSegmentId).HasColumnName("C_Pax_SegmentID");
            entity.Property(e => e.Commission).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.EventTime).HasColumnType("datetime");
            entity.Property(e => e.Gst).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Meal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NetRefundAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Net_Refund_Amount");
            entity.Property(e => e.OurCharges).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaxSegmentId).HasColumnName("Pax_SegmentID");
            entity.Property(e => e.PayEventTime)
                .HasColumnType("datetime")
                .HasColumnName("Pay_EventTime");
            entity.Property(e => e.PayStaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Pay_StaffID");
            entity.Property(e => e.RefundType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Reply)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Seat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ServiceFee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StaffID");
            entity.Property(e => e.Taxes).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tds).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UpdateType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VerifyEventTime)
                .HasColumnType("datetime")
                .HasColumnName("Verify_EventTime");
            entity.Property(e => e.VerifyStaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Verify_StaffID");
            entity.Property(e => e.VerifyStatus).HasColumnName("Verify_Status");
            entity.Property(e => e.Yq).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<LogCompanyPaxSegmentCancellationDetailAirlineRefundSub>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Log_Company_Pax_Segment_Cancellation_Detail_Airline_Refund_Sub");

            entity.Property(e => e.AirlineCharges).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Baggage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Basic).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CPaxSegmentId).HasColumnName("C_Pax_SegmentID");
            entity.Property(e => e.Commission).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyCancelCharges).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.EventTime).HasColumnType("datetime");
            entity.Property(e => e.Gst).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Markup)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("markup");
            entity.Property(e => e.Meal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NetRefundAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Net_Refund_Amount");
            entity.Property(e => e.OurCharges).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaxSegmentId).HasColumnName("Pax_SegmentID");
            entity.Property(e => e.PayEventTime)
                .HasColumnType("datetime")
                .HasColumnName("Pay_EventTime");
            entity.Property(e => e.PayStaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Pay_StaffID");
            entity.Property(e => e.RefundType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Reply)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Seat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ServiceFee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StaffID");
            entity.Property(e => e.Taxes).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Tds).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UpdateType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VerifyEventTime)
                .HasColumnType("datetime")
                .HasColumnName("Verify_EventTime");
            entity.Property(e => e.VerifyStaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Verify_StaffID");
            entity.Property(e => e.VerifyStatus).HasColumnName("Verify_Status");
            entity.Property(e => e.Yq).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<LogCompanyPaxSegmentDetailAirline>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Log_Company_Pax_Segment_Detail_Airline");

            entity.Property(e => e.ChargeAmount)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Charge_Amount");
            entity.Property(e => e.ChargeCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ChargeDescription)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ChargeType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Conn)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.EventTime).HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.PaxSegmentId).HasColumnName("Pax_SegmentID");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UpdateType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LogCompanyPaymentDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Log_Company_Payment_Detail");

            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BankName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ChequeNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Cheque_No");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.EntryBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EventTime).HasColumnType("datetime");
            entity.Property(e => e.EventTimeEnd).HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReferenceNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remark).IsUnicode(false);
            entity.Property(e => e.RemarkEnd).IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StaffID");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UpdateType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UtrNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UTR_No");
        });

        modelBuilder.Entity<LogCompanyRegister>(entity =>
        {
            entity.ToTable("Log_Company_Register");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AbtaNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ABTA_No");
            entity.Property(e => e.AccessStatus).HasColumnName("Access_Status");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.ActiveStatus).HasColumnName("Active_Status");
            entity.Property(e => e.Address)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.AdminId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("AdminID");
            entity.Property(e => e.AnnualTurnover).HasColumnName("Annual_Turnover");
            entity.Property(e => e.AtolNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ATOL_No");
            entity.Property(e => e.BusinessType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Business_Type");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CnpjNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CNPJ_No");
            entity.Property(e => e.CompanyEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Company_Email");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.CompanyLogo)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CompanyMobile)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Company_Mobile");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CompanyPhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Company_PhoneNo");
            entity.Property(e => e.Consolidators)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DirectDebitAgent).HasColumnName("Direct_Debit_Agent");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EventTime).HasColumnType("datetime");
            entity.Property(e => e.FaxNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gst)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("GST");
            entity.Property(e => e.GtgNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GTG_No");
            entity.Property(e => e.Host)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.IataMemberSinceYrs)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IATA_member_since_yrs");
            entity.Property(e => e.IataNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IATA_No");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IP");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LicenseNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("License_No");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MonthlyBookingVolume).HasColumnName("Monthly_Booking_Volume");
            entity.Property(e => e.NtnNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NTN_No");
            entity.Property(e => e.OfficeSpace)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Office_Space");
            entity.Property(e => e.PanName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Pan_Name");
            entity.Property(e => e.PanNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Pan_No");
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Pwd)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Reference)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ReferredBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.SafiCharge).HasColumnName("Safi_Charge");
            entity.Property(e => e.SelfBilling).HasColumnName("Self_billing");
            entity.Property(e => e.ServiceTaxNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Service_TaxNo");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StaxNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TanNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Tan_No");
            entity.Property(e => e.TdsAmount)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("TDS_Amount");
            entity.Property(e => e.TdsExemption)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("TDS_exemption");
            entity.Property(e => e.Title)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.TotalBranches).HasColumnName("Total_Branches");
            entity.Property(e => e.TotalEmployee).HasColumnName("Total_Employee");
            entity.Property(e => e.TpinNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TPIN_No");
            entity.Property(e => e.TtaNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TTA_No");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.UserType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VatNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VAT_No");
            entity.Property(e => e.YrsInBusiness).HasColumnName("Yrs_in_business");
        });

        modelBuilder.Entity<LogCompanyTransactionDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Log_Company_Transaction_Detail");

            entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Credit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Debit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EventId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EventID");
            entity.Property(e => e.EventTime).HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.PaxSegmentId).HasColumnName("PaxSegmentID");
            entity.Property(e => e.PaymentId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PaymentID");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remark)
                .HasMaxLength(900)
                .IsUnicode(false);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UpdateType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LogPaymentGatewayLogger>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Log_Payment_Gateway_Logger");

            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CardName)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Card_Name");
            entity.Property(e => e.CardType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EventTime).HasColumnType("datetime");
            entity.Property(e => e.Host)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IP");
            entity.Property(e => e.IsManualEvent).HasColumnType("datetime");
            entity.Property(e => e.MerchantCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Merchant_Code");
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Pgresponse)
                .IsUnicode(false)
                .HasColumnName("PGresponse");
            entity.Property(e => e.PgresponseCode).HasColumnName("PGresponseCode");
            entity.Property(e => e.RefundAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RequestRemark)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ResponseEventTime).HasColumnType("datetime");
            entity.Property(e => e.ResponseRemark)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Surcharge).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SurchargeAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UpdateType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Logger>(entity =>
        {
            entity.ToTable("Logger");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.ErrorMessage).IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Host)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Location).IsUnicode(false);
            entity.Property(e => e.MethodName).IsUnicode(false);
            entity.Property(e => e.SearchCriteria).IsUnicode(false);
            entity.Property(e => e.SearchId)
                .IsUnicode(false)
                .HasColumnName("SearchID");
        });

        modelBuilder.Entity<LoggerApi>(entity =>
        {
            entity.ToTable("LoggerAPI");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.CompanyId)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Conn)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MethodName).IsUnicode(false);
            entity.Property(e => e.PassengerRequest).IsUnicode(false);
            entity.Property(e => e.Request).IsUnicode(false);
            entity.Property(e => e.Response).IsUnicode(false);
            entity.Property(e => e.SearchCriteria).IsUnicode(false);
            entity.Property(e => e.SearchId)
                .IsUnicode(false)
                .HasColumnName("SearchID");
        });

        modelBuilder.Entity<LoggerHotelApi>(entity =>
        {
            entity.ToTable("LoggerHotelAPI");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.CompanyId)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Conn)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MethodName).IsUnicode(false);
            entity.Property(e => e.PassengerRequest).IsUnicode(false);
            entity.Property(e => e.Request).IsUnicode(false);
            entity.Property(e => e.Response).IsUnicode(false);
            entity.Property(e => e.SearchCriteria).IsUnicode(false);
            entity.Property(e => e.SearchId)
                .IsUnicode(false)
                .HasColumnName("SearchID");
        });

        modelBuilder.Entity<LoggerLogin>(entity =>
        {
            entity.ToTable("LoggerLogin");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Host)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InEvent)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Ip)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IP");
            entity.Property(e => e.LoginStatus).HasDefaultValue(false);
            entity.Property(e => e.OutEvent).HasColumnType("datetime");
            entity.Property(e => e.Pwd)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LoggerSearch>(entity =>
        {
            entity.ToTable("LoggerSearch");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.CompanyId)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("CompanyID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Host)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Location).IsUnicode(false);
            entity.Property(e => e.Place)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Remark).IsUnicode(false);
            entity.Property(e => e.Remark2)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.SearchId)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SearchID");
            entity.Property(e => e.StaffId)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("StaffID");
            entity.Property(e => e.Status).IsUnicode(false);
        });

        modelBuilder.Entity<LoginOtp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LoginOTP");

            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Otp)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("UserID");
        });

        modelBuilder.Entity<MailTemplateSender>(entity =>
        {
            entity.ToTable("Mail_Template_Sender");

            entity.Property(e => e.BccAddress)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("BCC_Address");
            entity.Property(e => e.CcAddress)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("CC_Address");
            entity.Property(e => e.EventTime).HasColumnType("datetime");
            entity.Property(e => e.Footer).HasDefaultValue(false);
            entity.Property(e => e.FromAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("From_Address");
            entity.Property(e => e.Header).HasDefaultValue(false);
            entity.Property(e => e.MailContent)
                .IsUnicode(false)
                .HasColumnName("Mail_Content");
            entity.Property(e => e.MailType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Mail_Type");
            entity.Property(e => e.Subject)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MailTemplateSenderSa>(entity =>
        {
            entity.ToTable("Mail_Template_Sender_SA");

            entity.Property(e => e.BccAddress)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("BCC_Address");
            entity.Property(e => e.CcAddress)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("CC_Address");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("CompanyID");
            entity.Property(e => e.Footer)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Header)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.SmsSenderId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SMS_SenderId");
        });

        modelBuilder.Entity<MasterAirline>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Master_Airline");

            entity.Property(e => e.AirlineName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PrefixCode)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NoticeCompany>(entity =>
        {
            entity.ToTable("Notice_Company");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IPriority)
                .HasDefaultValue(0)
                .HasColumnName("iPriority");
            entity.Property(e => e.Link)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.Subject)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NoticeStaff>(entity =>
        {
            entity.ToTable("Notice_Staff");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IPriority).HasColumnName("iPriority");
            entity.Property(e => e.Link)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.Subject)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OpinionLog>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("opinion_logs");

            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Rating).HasDefaultValue(0);
            entity.Property(e => e.Refid)
                .HasComputedColumnSql("([id]+(100))", false)
                .HasColumnName("refid");
            entity.Property(e => e.Review)
                .IsUnicode(false)
                .HasColumnName("review");
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.TitleReview)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PaymentGatewayCardCharge>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Payment_Gateway_Card_Charges");

            entity.Property(e => e.CardType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Charges).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        modelBuilder.Entity<PaymentGatewayCardChargesDetail>(entity =>
        {
            entity.ToTable("Payment_Gateway_Card_Charges_Detail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.B2b)
                .HasDefaultValue(false)
                .HasColumnName("B2B");
            entity.Property(e => e.B2b2b)
                .HasDefaultValue(false)
                .HasColumnName("B2B2B");
            entity.Property(e => e.B2b2c)
                .HasDefaultValue(false)
                .HasColumnName("B2B2C");
            entity.Property(e => e.B2c)
                .HasDefaultValue(false)
                .HasColumnName("B2C");
            entity.Property(e => e.CardType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Card_Type");
            entity.Property(e => e.Charges)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.D2b)
                .HasDefaultValue(false)
                .HasColumnName("D2B");
            entity.Property(e => e.Fixed).HasDefaultValue(false);
            entity.Property(e => e.MerchantCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Merchant_Code");
            entity.Property(e => e.Percnt).HasDefaultValue(false);
            entity.Property(e => e.Status).HasDefaultValue(false);
        });

        modelBuilder.Entity<PaymentGatewayCardChargesWaiverLimit>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Payment_Gateway_Card_Charges_Waiver_Limit");

            entity.Property(e => e.CardType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        modelBuilder.Entity<PaymentGatewayCardName>(entity =>
        {
            entity.ToTable("Payment_Gateway_Card_Name");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CardName)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Card_Name");
            entity.Property(e => e.CardType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Card_Type");
            entity.Property(e => e.Charges)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Fixed).HasDefaultValue(false);
            entity.Property(e => e.MerchantCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Merchant_Code");
            entity.Property(e => e.Percnt).HasDefaultValue(false);
        });

        modelBuilder.Entity<PaymentGatewayLogger>(entity =>
        {
            entity.ToTable("Payment_Gateway_Logger", tb =>
                {
                    tb.HasTrigger("Trig_Payment_Gateway_Logger_Delete");
                    tb.HasTrigger("Trig_Payment_Gateway_Logger_Update");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Amount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BookingRef).HasDefaultValue(0);
            entity.Property(e => e.CardName)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Card_Name");
            entity.Property(e => e.CardType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Host)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IP");
            entity.Property(e => e.IsBilled).HasDefaultValue(false);
            entity.Property(e => e.IsManualEvent)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MerchantCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Merchant_Code");
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentId)
                .HasComputedColumnSql("([ID]+(100000))", false)
                .HasColumnName("PaymentID");
            entity.Property(e => e.Pgresponse)
                .IsUnicode(false)
                .HasColumnName("PGresponse");
            entity.Property(e => e.PgresponseCode)
                .HasDefaultValue(-1)
                .HasColumnName("PGresponseCode");
            entity.Property(e => e.RefundAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Refunded).HasDefaultValue(false);
            entity.Property(e => e.RequestRemark)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.RequestStatus).HasDefaultValue(false);
            entity.Property(e => e.ResponseEventTime).HasColumnType("datetime");
            entity.Property(e => e.ResponseRemark)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ResponseStatus).HasDefaultValue(false);
            entity.Property(e => e.Surcharge)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SurchargeAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Updated).HasDefaultValue(false);
        });

        modelBuilder.Entity<PaymentGatewayManagement>(entity =>
        {
            entity.ToTable("Payment_Gateway_Management", tb => tb.HasTrigger("Payment_Gateway_Management_Trigg"));

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.IsCredit).HasDefaultValue(false);
            entity.Property(e => e.IsDebit).HasDefaultValue(false);
            entity.Property(e => e.IsFix).HasDefaultValue(false);
            entity.Property(e => e.IsNetbanking).HasDefaultValue(false);
            entity.Property(e => e.IsUpi).HasDefaultValue(false);
            entity.Property(e => e.MerchantCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Merchant_Code");
            entity.Property(e => e.MerchantKey)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Merchant_Key");
            entity.Property(e => e.MerchantMid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Merchant_MID");
            entity.Property(e => e.PaymentGatewayName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Payment_Gateway_Name");
            entity.Property(e => e.PaymentUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("PaymentURL");
        });

        modelBuilder.Entity<PkgAdd>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PkgAdd");

            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.DepartureDates)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("departure_dates");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Exclusions)
                .IsUnicode(false)
                .HasColumnName("exclusions");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Inclusions)
                .IsUnicode(false)
                .HasColumnName("inclusions");
            entity.Property(e => e.InstalmentPaymentOptions)
                .IsUnicode(false)
                .HasColumnName("instalment_payment_options");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Noofnights)
                .HasDefaultValue(0)
                .HasColumnName("noofnights");
            entity.Property(e => e.PackageShowInHomePage)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("package_show_in_home_page");
            entity.Property(e => e.Refid)
                .HasComputedColumnSql("([id]+(100))", false)
                .HasColumnName("refid");
            entity.Property(e => e.Starrating)
                .HasDefaultValue(0)
                .HasColumnName("starrating");
            entity.Property(e => e.StartingPrice)
                .HasDefaultValue(0)
                .HasColumnName("starting_price");
            entity.Property(e => e.StartingPriceNonresident)
                .HasDefaultValue(0)
                .HasColumnName("starting_price_nonresident");
            entity.Property(e => e.Themes)
                .IsUnicode(false)
                .HasColumnName("themes");
            entity.Property(e => e.Trip)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("trip");
            entity.Property(e => e.Url)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("url");
            entity.Property(e => e.ValidityTripEnd).HasColumnName("validity_trip_end");
            entity.Property(e => e.ValidityTripStart).HasColumnName("validity_trip_start");
        });

        modelBuilder.Entity<PkgAddCity>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Attraction)
                .IsUnicode(false)
                .HasColumnName("attraction");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.Day)
                .HasDefaultValue(0)
                .HasColumnName("day");
            entity.Property(e => e.HotelAddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("hotel_address");
            entity.Property(e => e.HotelName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("hotel_name");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Itinerary)
                .IsUnicode(false)
                .HasColumnName("itinerary");
            entity.Property(e => e.MealPlan)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("meal_plan");
            entity.Property(e => e.Night)
                .HasDefaultValue(0)
                .HasColumnName("night");
            entity.Property(e => e.Refid)
                .HasDefaultValue(0)
                .HasColumnName("refid");
            entity.Property(e => e.SightseeingSeen)
                .IsUnicode(false)
                .HasColumnName("sightseeing_seen");
        });

        modelBuilder.Entity<PkgAddDepartureCity>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.CountryName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("countryName");
            entity.Property(e => e.Countrycode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("countrycode");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Refid).HasColumnName("refid");
        });

        modelBuilder.Entity<PkgAddKeyword>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Keywords)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("keywords");
            entity.Property(e => e.Refid)
                .HasDefaultValue(0)
                .HasColumnName("refid");
        });

        modelBuilder.Entity<PkgAddMetaInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PkgAddMetaInfo");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.MetaDescription).IsUnicode(false);
            entity.Property(e => e.MetaKeyword)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.MetaTitle)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Refid).HasColumnName("refid");
        });

        modelBuilder.Entity<PkgAddPolicy>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PkgAddPolicy");

            entity.Property(e => e.Cancellation)
                .IsUnicode(false)
                .HasColumnName("cancellation");
            entity.Property(e => e.DestinationInfo)
                .IsUnicode(false)
                .HasColumnName("destination_info");
            entity.Property(e => e.GroupDepartureInfo)
                .IsUnicode(false)
                .HasColumnName("group_departure_info");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Payment)
                .IsUnicode(false)
                .HasColumnName("payment");
            entity.Property(e => e.Refid)
                .HasDefaultValue(0)
                .HasColumnName("refid");
        });

        modelBuilder.Entity<PkgAddTransfer>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PkgAddTransfer");

            entity.Property(e => e.Class)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Custom).IsUnicode(false);
            entity.Property(e => e.Day).HasColumnName("day");
            entity.Property(e => e.Destiation)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Number)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Origin)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Refid).HasColumnName("refid");
            entity.Property(e => e.Segmentno)
                .HasDefaultValue(0)
                .HasColumnName("segmentno");
            entity.Property(e => e.TransferType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PkgBilling>(entity =>
        {
            entity.HasKey(e => e.QueryRef);

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd() 
                .HasColumnName("ID");

           
            entity.Property(e => e.QueryRef)
                .ValueGeneratedNever()
                .HasColumnName("Query_Ref");

            
            entity.Property(e => e.CustomerExtra)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Customer_Extra");

            entity.Property(e => e.CustomerFlightAmout)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Customer_Flight_Amout");

            entity.Property(e => e.CustomerHotelAmout)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Customer_Hotel_Amout");

            entity.Property(e => e.CustomerInsuranceAmout)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Customer_Insurance_Amout");

            entity.Property(e => e.CustomerPassportAmout)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Customer_Passport_Amout");

            entity.Property(e => e.CustomerSightSeenAmout)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Customer_SightSeen_Amout");

            entity.Property(e => e.CustomerTransferringAmout)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Customer_Transferring_Amout");

            entity.Property(e => e.CustomerVisaAmout)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Customer_Visa_Amout");

            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.Property(e => e.ExtraDetail)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Extra_Detail");

            entity.Property(e => e.ExtraGst)
                .HasDefaultValue(false)
                .HasColumnName("Extra_GST");

            entity.Property(e => e.Flight)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.FlightGst)
                .HasDefaultValue(false)
                .HasColumnName("Flight_GST");

            entity.Property(e => e.FlightNameNumberInbound)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Flight_Name_Number_Inbound");

            entity.Property(e => e.FlightNameNumberOutbound)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Flight_Name_Number_Outbound");

            entity.Property(e => e.Hotel)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.HotelGst)
                .HasDefaultValue(false)
                .HasColumnName("Hotel_GST");

            entity.Property(e => e.Insurance)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.InsuranceGst)
                .HasDefaultValue(false)
                .HasColumnName("Insurance_GST");

            entity.Property(e => e.Passport)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.PassportGst)
                .HasDefaultValue(false)
                .HasColumnName("Passport_GST");

            entity.Property(e => e.Remark)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.Property(e => e.SightSeen)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.SightSeenGst)
                .HasDefaultValue(false)
                .HasColumnName("SightSeen_GST");

            entity.Property(e => e.Transferring)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.TransferringGst)
                .HasDefaultValue(false)
                .HasColumnName("Transferring_GST");

            entity.Property(e => e.UpdateBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.VendorExtra)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Vendor_Extra");

            entity.Property(e => e.VendorFlightAmout)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Vendor_Flight_Amout");

            entity.Property(e => e.VendorFlightId)
                .HasColumnName("Vendor_Flight_ID");

            entity.Property(e => e.VendorHotelAmout)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Vendor_Hotel_Amout");

            entity.Property(e => e.VendorHotelId)
                .HasDefaultValue(0)
                .HasColumnName("Vendor_Hotel_ID");

            entity.Property(e => e.VendorInsuranceAmout)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Vendor_Insurance_Amout");

            entity.Property(e => e.VendorInsuranceId)
                .HasColumnName("Vendor_Insurance_ID");

            entity.Property(e => e.VendorPassportAmout)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Vendor_Passport_Amout");

            entity.Property(e => e.VendorPassportId)
                .HasColumnName("Vendor_Passport_ID");

            entity.Property(e => e.VendorSightSeenAmout)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Vendor_SightSeen_Amout");

            entity.Property(e => e.VendorSightSeenId)
                .HasColumnName("Vendor_SightSeen_ID");

            entity.Property(e => e.VendorTransferringAmout)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Vendor_Transferring_Amout");

            entity.Property(e => e.VendorTransferringId)
                .HasColumnName("Vendor_Transferring_ID");

            entity.Property(e => e.VendorVisaAmout)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Vendor_Visa_Amout");

            entity.Property(e => e.VendorVisaId)
                .HasColumnName("Vendor_Visa_ID");

            entity.Property(e => e.Visa)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.VisaGst)
                .HasDefaultValue(false)
                .HasColumnName("Visa_GST");
        });


        modelBuilder.Entity<PkgDestination>(entity =>
        {
            entity.ToTable("Pkg_Destinations");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Destination)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DestinationDetail).IsUnicode(false);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PkgDocument>(entity =>
        {
            entity.ToTable("Pkg_Documents");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DocumentType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Documents)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.QueryRef).HasDefaultValue(0);
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StaffID");
        });

        modelBuilder.Entity<PkgExclusion>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Exclusionid)
                .HasComputedColumnSql("([id]+(100))", false)
                .HasColumnName("exclusionid");
            entity.Property(e => e.Exclusions)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("exclusions");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        modelBuilder.Entity<PkgHotelCategory>(entity =>
        {
            entity.ToTable("Pkg_HotelCategory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PkgInclusion>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Inclusionid)
                .HasComputedColumnSql("([id]+(100))", false)
                .HasColumnName("inclusionid");
            entity.Property(e => e.Inclusions)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("inclusions");
        });

        modelBuilder.Entity<PkgPackageRange>(entity =>
        {
            entity.ToTable("Pkg_PackageRange");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Range)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PkgPassenger>(entity =>
        {
            entity.ToTable("Pkg_Passenger");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Age).HasDefaultValue(0);
            entity.Property(e => e.PassengerName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PaxType)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.QueryRef).HasDefaultValue(0);
        });

        modelBuilder.Entity<PkgPayment>(entity =>
        {
            entity.ToTable("Pkg_Payments");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentBank)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PaymentDocument)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.PaymentRef)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PaymentRemark)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.PaymentType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.QueryRef).HasDefaultValue(0);
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StaffID");
        });

        modelBuilder.Entity<PkgPaymentoption>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Paymentid)
                .HasComputedColumnSql("([id]+(100))", false)
                .HasColumnName("paymentid");
            entity.Property(e => e.Paymentoptions)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))")
                .HasColumnName("paymentoptions");
        });

        modelBuilder.Entity<PkgQuery>(entity =>
        {
            entity.ToTable("Pkg_Query", tb =>
                {
                    tb.HasTrigger("log_Pkg_Query");
                    tb.HasTrigger("log_delete_Pkg_Query");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Adt).HasDefaultValue(0);
            entity.Property(e => e.Baggage).HasDefaultValue(false);
            entity.Property(e => e.Chd).HasDefaultValue(0);
            entity.Property(e => e.ContactPerson)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Contact_Person");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Cruise).HasDefaultValue(false);
            entity.Property(e => e.Destination)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DestinationDetail)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.DirectBilling).HasDefaultValue(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Flight).HasDefaultValue(false);
            entity.Property(e => e.Hotel).HasDefaultValue(false);
            entity.Property(e => e.HotelCategory).IsUnicode(false);
            entity.Property(e => e.Inf).HasDefaultValue(0);
            entity.Property(e => e.Insurance).HasDefaultValue(false);
            entity.Property(e => e.IsConfirmed).HasDefaultValue(false);
            entity.Property(e => e.IsRejected).HasDefaultValue(false);
            entity.Property(e => e.Meals).HasDefaultValue(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))");
            entity.Property(e => e.Package).HasDefaultValue(false);
            entity.Property(e => e.Passport).HasDefaultValue(false);
            entity.Property(e => e.QueryRef).HasComputedColumnSql("([ID]+(1000))", false);
            entity.Property(e => e.Range)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.RejectRemark)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Remark).IsUnicode(false);
            entity.Property(e => e.ReplyStatus).HasDefaultValue(0);
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.SiteSeen).HasDefaultValue(false);
            entity.Property(e => e.Source)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SourceArrivalDate).HasColumnName("Source_ArrivalDate");
            entity.Property(e => e.SourceDepartureDate).HasColumnName("Source_DepartureDate");
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StaffID");
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.Transferring).HasDefaultValue(false);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.Visa).HasDefaultValue(false);
        });

        modelBuilder.Entity<PkgQueryReply>(entity =>
        {
            entity.ToTable("Pkg_Query_Reply");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.QueryRef).HasDefaultValue(0);
            entity.Property(e => e.Reply)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ReplyRef).HasComputedColumnSql("([QueryRef]+[ID])", false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("StaffID");
            entity.Property(e => e.Status).HasDefaultValue(0);
            entity.Property(e => e.Vendor)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.WorkingOn)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<PkgQueryReplyVendor>(entity =>
        {
            entity.ToTable("Pkg_Query_Reply_Vendor");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Vendor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VendorTypeId).HasColumnName("VendorTypeID");
        });

        modelBuilder.Entity<PkgStatus>(entity =>
        {
            entity.ToTable("Pkg_Status");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PkgTheme>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Themes)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("themes");
            entity.Property(e => e.Themesid)
                .HasComputedColumnSql("([id]+(100))", false)
                .HasColumnName("themesid");
        });

        modelBuilder.Entity<PkgVendor>(entity =>
        {
            entity.ToTable("Pkg_Vendor");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Destination).IsUnicode(false);
            entity.Property(e => e.VendorAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.VendorDetail)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.VendorEmail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VendorMobile)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VendorName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.VendorPhone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VendorType).IsUnicode(false);
        });

        modelBuilder.Entity<PkgVendorType>(entity =>
        {
            entity.ToTable("Pkg_VendorType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.VendorType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<PkgWorkingOn>(entity =>
        {
            entity.ToTable("Pkg_WorkingOn");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.WorkingId).HasColumnName("WorkingID");
            entity.Property(e => e.WorkingName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PreferredAirline>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Airlinesname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("airlinesname");
            entity.Property(e => e.Code)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("code");
        });

        modelBuilder.Entity<QueryGroupFare>(entity =>
        {
            entity.ToTable("Query_GroupFare");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ArrivalDate)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ArrivalStation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("AD-101")
                .HasColumnName("CompanyID");
            entity.Property(e => e.ContactPerson)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DepartureDate)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DepartureStation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EventTimeResponse)
                .HasColumnType("datetime")
                .HasColumnName("EventTime_Response");
            entity.Property(e => e.GroupId)
                .HasComputedColumnSql("([ID]+(1000))", false)
                .HasColumnName("GroupID");
            entity.Property(e => e.MobileNo)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.NoOfPassengers).HasDefaultValue(0);
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RemarksResponse)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Remarks_Response");
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(0);
            entity.Property(e => e.Trip)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RoleModuleManagement>(entity =>
        {
            entity.ToTable("Role_Module_Management");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ModuleId)
                .HasComputedColumnSql("([ID]+(100))", false)
                .HasColumnName("ModuleID");
            entity.Property(e => e.ModuleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RoleModuleProcuctManagement>(entity =>
        {
            entity.ToTable("Role_Module_Procuct_Management");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IsB2c)
                .HasDefaultValue(false)
                .HasColumnName("IsB2C");
            entity.Property(e => e.ModuleId).HasColumnName("ModuleID");
            entity.Property(e => e.PageName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ProductId)
                .HasComputedColumnSql("([ID]+(100))", false)
                .HasColumnName("ProductID");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(false);
        });

        modelBuilder.Entity<RoleModuleProcuctRegisterManagement>(entity =>
        {
            entity.ToTable("Role_Module_Procuct_Register_Management");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Edit).HasDefaultValue(true);
            entity.Property(e => e.ProductId)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("ProductID");
            entity.Property(e => e.Show).HasDefaultValue(true);
            entity.Property(e => e.UserType)
                .HasMaxLength(9)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SmsDetail>(entity =>
        {
            entity.ToTable("SMS_Detail");

            entity.Property(e => e.BalanceUrl)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("BalanceURL");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.SmsUrl)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SmsURL");
            entity.Property(e => e.SourceId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<SmsRunTime>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SMS_RunTime");

            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.SmsText)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("Sms_Text");
            entity.Property(e => e.Smsid)
                .HasComputedColumnSql("([id]+(1000))", false)
                .HasColumnName("smsid");
            entity.Property(e => e.Status).HasDefaultValue(false);
        });

        modelBuilder.Entity<SmsTemplate>(entity =>
        {
            entity.ToTable("SMS_Template");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Entityid)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EventName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Event_Name");
            entity.Property(e => e.ProductName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Product_Name");
            entity.Property(e => e.SenderId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SenderID");
            entity.Property(e => e.SmsMessage)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("SMS_Message");
            entity.Property(e => e.Tempid)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("User_Type");
        });

        modelBuilder.Entity<SmtpDetail>(entity =>
        {
            entity.ToTable("SMTP_Detail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Authentication).HasDefaultValue(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ServerPort)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Server_Port");
            entity.Property(e => e.SmtpServer)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SMTP_Server");
            entity.Property(e => e.SmtpType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SMTP_Type");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SpicejetSsr>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SpicejetSSR");

            entity.Property(e => e.Code)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Sector)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Ssrtype)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("SSRtype");
            entity.Property(e => e.T03011130).HasColumnName("T0301_1130");
            entity.Property(e => e.T11311500).HasColumnName("T1131_1500");
            entity.Property(e => e.T15011900).HasColumnName("T1501_1900");
            entity.Property(e => e.T19012300).HasColumnName("T1901_2300");
            entity.Property(e => e.T23010300).HasColumnName("T2301_0300");
        });

        modelBuilder.Entity<StCityAirport>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("st_CityAirport");

            entity.Property(e => e.AirportCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("airport_code");
            entity.Property(e => e.AirportName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("airport_name");
            entity.Property(e => e.CityName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("city_name");
        });

        modelBuilder.Entity<StaffCategory>(entity =>
        {
            entity.ToTable("StaffCategory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CategoryId)
                .HasComputedColumnSql("([ID]+(100))", false)
                .HasColumnName("CategoryID");
            entity.Property(e => e.IsAdmin).HasDefaultValue(false);
            entity.Property(e => e.IsB2b)
                .HasDefaultValue(false)
                .HasColumnName("IsB2B");
            entity.Property(e => e.IsCorporate).HasDefaultValue(false);
            entity.Property(e => e.StaffType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StaffRole>(entity =>
        {
            entity.ToTable("StaffRole");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
        });

        modelBuilder.Entity<SupplierApiAirline>(entity =>
        {
            entity.ToTable("Supplier_API_Airlines");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID");
        });

        modelBuilder.Entity<SupplierCommissionDRule>(entity =>
        {
            entity.ToTable("Supplier_Commission_D_Rule");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AutoPnr)
                .HasDefaultValue(false)
                .HasColumnName("Auto_PNR");
            entity.Property(e => e.AutoTkt)
                .HasDefaultValue(false)
                .HasColumnName("Auto_Tkt");
            entity.Property(e => e.Basic)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("BASIC");
            entity.Property(e => e.BookingClass).IsUnicode(false);
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Cb)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("CB");
            entity.Property(e => e.Priority).HasDefaultValue(0);
            entity.Property(e => e.Promo)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("PROMO");
            entity.Property(e => e.Sf)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("SF");
            entity.Property(e => e.SplFare).HasDefaultValue(false);
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID");
            entity.Property(e => e.Yq)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("YQ");
        });

        modelBuilder.Entity<SupplierCommissionIRule>(entity =>
        {
            entity.ToTable("Supplier_Commission_I_Rule");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AutoPnr)
                .HasDefaultValue(false)
                .HasColumnName("Auto_PNR");
            entity.Property(e => e.AutoTkt)
                .HasDefaultValue(false)
                .HasColumnName("Auto_Tkt");
            entity.Property(e => e.Basic)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("BASIC");
            entity.Property(e => e.BookingClass).IsUnicode(false);
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Cb)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("CB");
            entity.Property(e => e.DestinationCountry).IsUnicode(false);
            entity.Property(e => e.OriginCountry).IsUnicode(false);
            entity.Property(e => e.Priority).HasDefaultValue(0);
            entity.Property(e => e.Promo)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("PROMO");
            entity.Property(e => e.Sf)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("SF");
            entity.Property(e => e.SplFare).HasDefaultValue(false);
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID");
            entity.Property(e => e.Yq)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("YQ");
        });

        modelBuilder.Entity<SupplierDetail>(entity =>
        {
            entity.ToTable("Supplier_Detail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OwnSupplier).HasDefaultValue(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Product)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Supplier_Name");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");
        });

        modelBuilder.Entity<SupplierDetailAmadeusAirline>(entity =>
        {
            entity.ToTable("Supplier_Detail_Amadeus_Airline");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Corporateid)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OfficeId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("OfficeID");
            entity.Property(e => e.Originator)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PwdLength).HasDefaultValue(0);
            entity.Property(e => e.SellCity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Url)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Wsap)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SupplierDetailApiAirline>(entity =>
        {
            entity.ToTable("Supplier_Detail_API_Airline");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("UserID");
        });

        modelBuilder.Entity<SupplierDetailApiHotel>(entity =>
        {
            entity.ToTable("Supplier_Detail_API_Hotel");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("UserID");
        });

        modelBuilder.Entity<SupplierDetailGalileoAirline>(entity =>
        {
            entity.ToTable("Supplier_Detail_Galileo_Airline");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Hap)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("HAP");
            entity.Property(e => e.ImportQueue)
                .HasDefaultValue(0)
                .HasColumnName("Import_Queue");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Pcc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PCC");
            entity.Property(e => e.SoapUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Soap_Url");
            entity.Property(e => e.TargetBranch)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TicketIfFareGaurantee).HasDefaultValue(false);
            entity.Property(e => e.TktdQueue)
                .HasDefaultValue(0)
                .HasColumnName("Tktd_Queue");
            entity.Property(e => e.Userid)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SupplierDetailLccAirline>(entity =>
        {
            entity.ToTable("Supplier_Detail_Lcc_Airline");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AgentDomain)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.AgentId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("AgentID");
            entity.Property(e => e.AgentUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("AgentURL");
            entity.Property(e => e.BookingUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("BookingURL");
            entity.Property(e => e.CarrierCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ContentUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("ContentURL");
            entity.Property(e => e.ContractVersion).HasDefaultValue(0);
            entity.Property(e => e.CorporateCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.FareUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("FareURL");
            entity.Property(e => e.LocationCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.LoginId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("LoginID");
            entity.Property(e => e.LookupUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("LookupURL");
            entity.Property(e => e.OperationUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("OperationURL");
            entity.Property(e => e.OrganizationCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.PromoCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Pwd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.SessionUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SessionURL");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Supplier_Code");
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SupplierID");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Supplier_Name");
            entity.Property(e => e.TargetBranch)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SupplierProductDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Supplier_Product_Detail", tb => tb.HasTrigger("Supplier_Product_Status_Trigg"));

            entity.Property(e => e.CityName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("City_Name");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.FareType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Fare_Type");
            entity.Property(e => e.Flight).HasDefaultValue(false);
            entity.Property(e => e.Hotel).HasDefaultValue(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Phone_Number");
            entity.Property(e => e.Remarks)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Supplier_Code");
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Supplier_Name");
            entity.Property(e => e.SupplierType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Supplier_Type");
        });

        modelBuilder.Entity<SupplierProductStatus>(entity =>
        {
            entity.ToTable("Supplier_Product_Status");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.B2b)
                .HasDefaultValue(false)
                .HasColumnName("B2B");
            entity.Property(e => e.B2c)
                .HasDefaultValue(false)
                .HasColumnName("B2C");
            entity.Property(e => e.ImportPnr)
                .HasDefaultValue(false)
                .HasColumnName("Import_PNR");
            entity.Property(e => e.Int)
                .HasDefaultValue(false)
                .HasColumnName("INT");
            entity.Property(e => e.MultiCity)
                .HasDefaultValue(false)
                .HasColumnName("Multi_City");
            entity.Property(e => e.Pcc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Pnr)
                .HasDefaultValue(false)
                .HasColumnName("PNR");
            entity.Property(e => e.Product)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Rt)
                .HasDefaultValue(false)
                .HasColumnName("RT");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Supplier_Code");
            entity.Property(e => e.SupplierId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SupplierID");
            entity.Property(e => e.Ticketting).HasDefaultValue(false);
        });

        modelBuilder.Entity<SupplierRefidSeries>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Refid).HasDefaultValue(0);
            entity.Property(e => e.Supplier)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ToRefid)
                .HasDefaultValue(0)
                .HasColumnName("toRefid");
        });

        modelBuilder.Entity<Table1>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Table_1");

            entity.Property(e => e.Pricet)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PRICEt");
        });

        modelBuilder.Entity<TiAirportCity>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TI_AirportCities");

            entity.Property(e => e.AirportCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AirportName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AltCityName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CityCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CityName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CountryCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CountryName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiFlightActiveSupplier>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TI_FlightActiveSupplier");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CODE");
            entity.Property(e => e.CompanyNationality)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Productcode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PRODUCTCODE");
            entity.Property(e => e.Productsupplierid)
                .HasDefaultValue(0)
                .HasColumnName("PRODUCTSUPPLIERID");
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.Supplierid)
                .HasDefaultValue(0)
                .HasColumnName("SUPPLIERID");
            entity.Property(e => e.Suppliertimeout)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SUPPLIERTIMEOUT");
        });

        modelBuilder.Entity<ToursExclusion>(entity =>
        {
            entity.ToTable("tours_exclusions");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Exclusions)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ToursInclusion>(entity =>
        {
            entity.ToTable("tours_inclusions");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Inclusions)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ToursInvoice>(entity =>
        {
            entity.ToTable("tours_invoice");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AdultsPrice).HasColumnName("adults_price");
            entity.Property(e => e.AdultsQuantity).HasColumnName("adults_quantity");
            entity.Property(e => e.BookingRef).HasComputedColumnSql("([ID]+(100))", false);
            entity.Property(e => e.ChildsPrice).HasColumnName("childs_price");
            entity.Property(e => e.ChildsQuantity).HasColumnName("childs_quantity");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FromDate)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("from_date");
            entity.Property(e => e.InfantsPrice).HasColumnName("infants_price");
            entity.Property(e => e.InfantsQuantity).HasColumnName("infants_quantity");
            entity.Property(e => e.ShortDescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("short_description");
            entity.Property(e => e.ShortOverview)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("short_overview");
            entity.Property(e => e.ToDate)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("to_date");
            entity.Property(e => e.TourCountry)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("tour_country");
            entity.Property(e => e.TourCurrency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("tour_currency");
            entity.Property(e => e.TourDays).HasColumnName("tour_days");
            entity.Property(e => e.TourName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tour_name");
            entity.Property(e => e.TourNightDays).HasColumnName("tour_night_days");
            entity.Property(e => e.TourStar).HasColumnName("tour_star");
            entity.Property(e => e.Tourid).HasColumnName("tourid");
        });

        modelBuilder.Entity<ToursInvoiceDetail>(entity =>
        {
            entity.ToTable("tours_invoice_details");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CommVatCommission)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("commVat_commission");
            entity.Property(e => e.CommVatCommissiontype)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("commVat_commissiontype");
            entity.Property(e => e.CommVatVat)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("commVat_vat");
            entity.Property(e => e.CommVatVattype)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("commVat_vattype");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.ContactAddress)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("contact_address");
            entity.Property(e => e.ContactEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contact_email");
            entity.Property(e => e.ContactMobile)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contact_mobile");
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contact_phone");
            entity.Property(e => e.ContactWebsite)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("contact_website");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Exclusions)
                .IsUnicode(false)
                .HasColumnName("exclusions");
            entity.Property(e => e.Inclusions)
                .IsUnicode(false)
                .HasColumnName("inclusions");
            entity.Property(e => e.Locations)
                .IsUnicode(false)
                .HasColumnName("locations");
            entity.Property(e => e.Policy)
                .IsUnicode(false)
                .HasColumnName("policy");
            entity.Property(e => e.Tnc)
                .IsUnicode(false)
                .HasColumnName("tnc");
            entity.Property(e => e.TourDescription)
                .IsUnicode(false)
                .HasColumnName("tour_description");
        });

        modelBuilder.Entity<ToursInvoicePassenger>(entity =>
        {
            entity.ToTable("tours_invoice_passenger");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PassengerName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Passenger_Name");
            entity.Property(e => e.PaxType)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.PpExpiraryDate)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PpIssueDate)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PpNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ToursList>(entity =>
        {
            entity.ToTable("tours_list");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AdultsPrice)
                .HasDefaultValue(0)
                .HasColumnName("adults_price");
            entity.Property(e => e.AdultsQuantity)
                .HasDefaultValue(0)
                .HasColumnName("adults_quantity");
            entity.Property(e => e.ChildsPrice)
                .HasDefaultValue(0)
                .HasColumnName("childs_price");
            entity.Property(e => e.ChildsQuantity)
                .HasDefaultValue(0)
                .HasColumnName("childs_quantity");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("CompanyID");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Featured)
                .HasDefaultValue(false)
                .HasColumnName("featured");
            entity.Property(e => e.FromDate)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("from_date");
            entity.Property(e => e.InfantsPrice)
                .HasDefaultValue(0)
                .HasColumnName("infants_price");
            entity.Property(e => e.InfantsQuantity)
                .HasDefaultValue(0)
                .HasColumnName("infants_quantity");
            entity.Property(e => e.Rating).HasDefaultValue(0);
            entity.Property(e => e.ShortDescription)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("short_description");
            entity.Property(e => e.ShortOverview)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("short_overview");
            entity.Property(e => e.Status)
                .HasDefaultValue(false)
                .HasColumnName("status");
            entity.Property(e => e.ToDate)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("to_date");
            entity.Property(e => e.TourCountry)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("tour_country");
            entity.Property(e => e.TourCurrency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("INR")
                .HasColumnName("tour_currency");
            entity.Property(e => e.TourDays)
                .HasDefaultValue(0)
                .HasColumnName("tour_days");
            entity.Property(e => e.TourDescription)
                .HasDefaultValue("")
                .HasColumnType("text")
                .HasColumnName("tour_description");
            entity.Property(e => e.TourName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tour_name");
            entity.Property(e => e.TourNightDays)
                .HasDefaultValue(0)
                .HasColumnName("tour_night_days");
            entity.Property(e => e.TourStar)
                .HasDefaultValue(0)
                .HasColumnName("tour_star");
            entity.Property(e => e.TourType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("tour_type");
            entity.Property(e => e.Tourid)
                .HasComputedColumnSql("([ID]+(1000))", false)
                .HasColumnName("tourid");
            entity.Property(e => e.Tourorder)
                .HasDefaultValue(0)
                .HasColumnName("tourorder");
        });

        modelBuilder.Entity<ToursListCommVat>(entity =>
        {
            entity.ToTable("tours_list_commVat");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Commission)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("commission");
            entity.Property(e => e.Commissiontype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("commissiontype");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Tourid)
                .HasDefaultValue(0)
                .HasColumnName("tourid");
            entity.Property(e => e.Vat)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("vat");
            entity.Property(e => e.Vattype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("vattype");
        });

        modelBuilder.Entity<ToursListContact>(entity =>
        {
            entity.ToTable("tours_list_contact");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mobile");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Tourid)
                .HasDefaultValue(0)
                .HasColumnName("tourid");
            entity.Property(e => e.Website)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("website");
        });

        modelBuilder.Entity<ToursListDaysdescription>(entity =>
        {
            entity.ToTable("tours_list_daysdescription");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.City)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.CityDetail)
                .IsUnicode(false)
                .HasColumnName("city_detail");
            entity.Property(e => e.Day)
                .HasDefaultValue(0)
                .HasColumnName("day");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.HotelAddress)
                .IsUnicode(false)
                .HasColumnName("hotel_address");
            entity.Property(e => e.HotelExtraDetail)
                .IsUnicode(false)
                .HasColumnName("hotel_extra_detail");
            entity.Property(e => e.HotelName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("hotel_name");
            entity.Property(e => e.TourDescription)
                .IsUnicode(false)
                .HasColumnName("tour_description");
            entity.Property(e => e.Tourid)
                .HasDefaultValue(0)
                .HasColumnName("tourid");
        });

        modelBuilder.Entity<ToursListExclusion>(entity =>
        {
            entity.ToTable("tours_list_exclusions");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Exclusions)
                .IsUnicode(false)
                .HasColumnName("exclusions");
            entity.Property(e => e.Tourid)
                .HasDefaultValue(0)
                .HasColumnName("tourid");
        });

        modelBuilder.Entity<ToursListImage>(entity =>
        {
            entity.ToTable("tours_list_images");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Images)
                .IsUnicode(false)
                .HasColumnName("images");
            entity.Property(e => e.ImagesOrder)
                .HasDefaultValue(0)
                .HasColumnName("images_order");
            entity.Property(e => e.Tourid)
                .HasDefaultValue(0)
                .HasColumnName("tourid");
        });

        modelBuilder.Entity<ToursListInclusion>(entity =>
        {
            entity.ToTable("tours_list_inclusions");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Inclusions)
                .IsUnicode(false)
                .HasColumnName("inclusions");
            entity.Property(e => e.Tourid)
                .HasDefaultValue(0)
                .HasColumnName("tourid");
        });

        modelBuilder.Entity<ToursListLocation>(entity =>
        {
            entity.ToTable("tours_list_locations");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Locations)
                .IsUnicode(false)
                .HasColumnName("locations");
            entity.Property(e => e.Tourid)
                .HasDefaultValue(0)
                .HasColumnName("tourid");
        });

        modelBuilder.Entity<ToursListMap>(entity =>
        {
            entity.ToTable("tours_list_map");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AddressMap)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("address_map");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Latitude)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("longitude");
            entity.Property(e => e.Tourid)
                .HasDefaultValue(0)
                .HasColumnName("tourid");
        });

        modelBuilder.Entity<ToursListMetatag>(entity =>
        {
            entity.ToTable("tours_list_metatag");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descriptions)
                .HasColumnType("text")
                .HasColumnName("descriptions");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Keywords)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("keywords");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.Tourid)
                .HasDefaultValue(0)
                .HasColumnName("tourid");
        });

        modelBuilder.Entity<ToursListPolicy>(entity =>
        {
            entity.ToTable("tours_list_policy");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Policy)
                .IsUnicode(false)
                .HasColumnName("policy");
            entity.Property(e => e.Tnc)
                .IsUnicode(false)
                .HasColumnName("tnc");
            entity.Property(e => e.Tourid)
                .HasDefaultValue(0)
                .HasColumnName("tourid");
        });

        modelBuilder.Entity<ToursListRelated>(entity =>
        {
            entity.ToTable("tours_list_related");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("eventtime");
            entity.Property(e => e.Relatedtour)
                .IsUnicode(false)
                .HasColumnName("relatedtour");
            entity.Property(e => e.Tourid)
                .HasDefaultValue(0)
                .HasColumnName("tourid");
        });

        modelBuilder.Entity<ToursListReview>(entity =>
        {
            entity.ToTable("tours_list_reviews");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Rating).HasDefaultValue(0);
            entity.Property(e => e.Review).IsUnicode(false);
            entity.Property(e => e.Tourid).HasColumnName("tourid");
        });

        modelBuilder.Entity<ToursPaymentoption>(entity =>
        {
            entity.ToTable("tours_paymentoptions");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ToursRole>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("toursRole");

            entity.Property(e => e.Edit).HasDefaultValue(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.ProductId)
                .HasDefaultValue(0)
                .HasColumnName("ProductID");
            entity.Property(e => e.Show).HasDefaultValue(false);
        });

        modelBuilder.Entity<ToursTourtype>(entity =>
        {
            entity.ToTable("tours_tourtypes");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Tourtype)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UapiCcDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("UAPI_CC_Details");

            entity.Property(e => e.AddressName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BankCountryCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.BankName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Carriers)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Cvv)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("CVV");
            entity.Property(e => e.ExpDate)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Number)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Street)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UapiFormOfPayment>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("UAPI_FormOfPayment");

            entity.Property(e => e.Fop)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FOP");
            entity.Property(e => e.Id).HasColumnName("id");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.UserDescription)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("User_Description");
            entity.Property(e => e.UserType1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserType");
        });

        modelBuilder.Entity<VVisaCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("vVisaCategory");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.VisaCategory)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.VisaCategoryName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VVisaChargeCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("vVisaChargeCategory");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.VisaChargeCategory)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.VisaChargeName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VVisaEntry>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("vVisaEntry");

            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FromVisaCountry)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.FromVisaState)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.ToVisaCountry)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.VisaCategory)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Visaid).HasComputedColumnSql("([id]+(100))", false);
        });

        modelBuilder.Entity<VVisaFee>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("vVisaFees");

            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Remark)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Servicefee)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Vfsfee)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VisaCategory)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.VisaChargeCategory)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Visafee)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Visaid).HasDefaultValue(0);
        });

        modelBuilder.Entity<VVisaFile>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("vVisaFiles");

            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FileName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FilePath)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Fileid).HasComputedColumnSql("([id]+(1000))", false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.VisaCategory)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Visaid).HasDefaultValue(0);
        });

        modelBuilder.Entity<VVisaOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("vVisaOrder");

            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FilePath)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FromVisaCountry)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.FromVisaState)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.LandLineNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Orderid).HasComputedColumnSql("([id]+(1000))", false);
            entity.Property(e => e.PassportAddress)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.PassportName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PassportNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ToVisaCountry)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.VisaCategory)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.VisaChargeCategory)
                .HasMaxLength(2)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VVisaRule>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("vVisaRules");

            entity.Property(e => e.Eventtime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Rules).HasColumnType("text");
            entity.Property(e => e.VisaCategory)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Visaid).HasDefaultValue(0);
        });

        modelBuilder.Entity<WhitelabelDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Whitelabel_Detail");

            entity.Property(e => e.AgencyPrintB2c)
                .HasDefaultValue(true)
                .HasColumnName("Agency_Print_B2C");
            entity.Property(e => e.B2b)
                .HasDefaultValue(false)
                .HasColumnName("b2b");
            entity.Property(e => e.B2c)
                .HasDefaultValue(false)
                .HasColumnName("b2c");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.EventTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Host)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.HostDirectory)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.IsWorking).HasDefaultValue(true);
            entity.Property(e => e.OwnPg)
                .HasDefaultValue(false)
                .HasColumnName("Own_PG");
            entity.Property(e => e.RunOwnGateway)
                .HasDefaultValue(false)
                .HasColumnName("Run_Own_Gateway");
        });

        modelBuilder.Entity<WhitelabelRole>(entity =>
        {
            entity.ToTable("WhitelabelRole");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Edit).HasDefaultValue(false);
            entity.Property(e => e.IsB2b)
                .HasDefaultValue(true)
                .HasColumnName("IsB2B");
            entity.Property(e => e.IsB2c)
                .HasDefaultValue(false)
                .HasColumnName("IsB2C");
            entity.Property(e => e.ProductId)
                .HasDefaultValue(0)
                .HasColumnName("ProductID");
            entity.Property(e => e.Show).HasDefaultValue(false);
        });

        modelBuilder.Entity<WhitelabelSupport>(entity =>
        {
            entity.ToTable("Whitelabel_Support");

            entity.Property(e => e.CompanyId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.SocialLinkDesc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.SocialUrl)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
