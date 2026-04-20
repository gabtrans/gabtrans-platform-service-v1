using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;


namespace GabTrans.Infrastructure.Data;

public partial class GabTransContext : DbContext
{
    public GabTransContext()
    {
    }

    public GabTransContext(DbContextOptions<GabTransContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountCurrency> AccountCurrencies { get; set; }

    public virtual DbSet<AccountProvider> AccountProviders { get; set; }

    public virtual DbSet<AccountRequest> AccountRequests { get; set; }

    public virtual DbSet<AccountType> AccountTypes { get; set; }

    public virtual DbSet<AppCredential> AppCredentials { get; set; }

    public virtual DbSet<ApplicationSetting> ApplicationSettings { get; set; }

    public virtual DbSet<ApprovalStatus> ApprovalStatuses { get; set; }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<Audit> Audits { get; set; }

    public virtual DbSet<AuthCredential> AuthCredentials { get; set; }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<BankAccountType> BankAccountTypes { get; set; }

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<BusinessRole> BusinessRoles { get; set; }

    public virtual DbSet<BusinessTeam> BusinessTeams { get; set; }

    public virtual DbSet<BusinessType> BusinessTypes { get; set; }

    public virtual DbSet<Channel> Channels { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Continent> Continents { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<CryptoTrade> CryptoTrades { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Deposit> Deposits { get; set; }

    public virtual DbSet<Dispute> Disputes { get; set; }

    public virtual DbSet<DocumentType> DocumentTypes { get; set; }

    public virtual DbSet<EmploymentStatus> EmploymentStatuses { get; set; }

    public virtual DbSet<Fee> Fees { get; set; }

    public virtual DbSet<FxRate> FxRates { get; set; }

    public virtual DbSet<FxRateAudit> FxRateAudits { get; set; }

    public virtual DbSet<FxRateLog> FxRateLogs { get; set; }

    public virtual DbSet<FxTransaction> FxTransactions { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<GremitAccount> GremitAccounts { get; set; }

    public virtual DbSet<IdentityType> IdentityTypes { get; set; }

    public virtual DbSet<Industry> Industries { get; set; }

    public virtual DbSet<Integration> Integrations { get; set; }

    public virtual DbSet<Invitation> Invitations { get; set; }

    public virtual DbSet<Kyc> Kycs { get; set; }

    public virtual DbSet<KycRequest> KycRequests { get; set; }

    public virtual DbSet<KycVerification> KycVerifications { get; set; }

    public virtual DbSet<Limit> Limits { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<MaritalStatus> MaritalStatuses { get; set; }

    public virtual DbSet<MessageCredential> MessageCredentials { get; set; }

    public virtual DbSet<ApplicationModule> Modules { get; set; }

    public virtual DbSet<ApplicationModuleAction> ModuleActions { get; set; }

    public virtual DbSet<Network> Networks { get; set; }

    public virtual DbSet<Occupation> Occupations { get; set; }

    public virtual DbSet<OneTimePassword> OneTimePasswords { get; set; }

    public virtual DbSet<OtpCategory> OtpCategories { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<PaymentReason> PaymentReasons { get; set; }

    public virtual DbSet<PendingDeposit> PendingDeposits { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<PlatformTransaction> PlatformTransactions { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SerialNumber> SerialNumbers { get; set; }

    public virtual DbSet<Settlement> Settlements { get; set; }

    public virtual DbSet<SourceOfFund> SourceOfFunds { get; set; }

    public virtual DbSet<Stage> Stages { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<TransactionPin> TransactionPins { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    public virtual DbSet<Transfer> Transfers { get; set; }

    public virtual DbSet<TransferProvider> TransferProviders { get; set; }

    public virtual DbSet<TransferRecipient> TransferRecipients { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserToken> UserTokens { get; set; }

    public virtual DbSet<VirtualAccount> VirtualAccounts { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    public virtual DbSet<WebHook> WebHooks { get; set; }

    [NotMapped]
    public virtual DbSet<AuditDetails> AuditDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseNpgsql(StaticData.ConnectionStrings!);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("accounts_pkey");

            entity.ToTable("accounts", "gabtrans");

            entity.HasIndex(e => e.Id, "idx_accounts_deposits");

            entity.HasIndex(e => e.Id, "idx_settlements_accounts");

            entity.HasIndex(e => e.Id, "idx_transfers_accounts");

            entity.HasIndex(e => e.Id, "idx_virtual_accounts_accounts");

            entity.HasIndex(e => e.Id, "idx_wallets_accounts");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Assets)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("assets");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("name");
            entity.Property(e => e.Provider)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("provider");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasDefaultValueSql("'pending'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Type)
                .HasMaxLength(45)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Uuid)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("uuid");

            entity.HasOne(d => d.User).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_users_accounts");
        });

        modelBuilder.Entity<AccountCurrency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("account_currencies_pkey");

            entity.ToTable("account_currencies", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Country)
                .HasMaxLength(5)
                .HasColumnName("country");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .HasMaxLength(5)
                .HasColumnName("currency");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .HasDefaultValueSql("'active'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<AccountProvider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("account_providers_pkey");

            entity.ToTable("account_providers", "gabtrans");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(45)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<AccountRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("account_requests_pkey");

            entity.ToTable("account_requests", "gabtrans");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasDefaultValueSql("'new'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Currency)
            .HasMaxLength(5)
            .HasColumnName("currency");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
        });

        modelBuilder.Entity<AccountType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("account_types_pkey");

            entity.ToTable("account_types", "gabtrans");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(45)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<AppCredential>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("app_credentials_pkey");

            entity.ToTable("app_credentials", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("'0'::\"bit\"")
                .HasColumnType("bit(1)")
                .HasColumnName("active");
            entity.Property(e => e.AppId)
                .HasMaxLength(150)
                .HasColumnName("app_id");
            entity.Property(e => e.AppKey)
                .HasMaxLength(100)
                .HasColumnName("app_key");
            entity.Property(e => e.LastLogin)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_login");
            entity.Property(e => e.Token)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("token");
        });

        modelBuilder.Entity<ApplicationSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("application_settings_pkey");

            entity.ToTable("application_settings", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Value)
                .HasMaxLength(500)
                .HasColumnName("value");
        });

        modelBuilder.Entity<ApprovalStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("approval_statuses_pkey");

            entity.ToTable("approval_statuses", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("assets_pkey");

            entity.ToTable("assets", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Audit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("audits_pkey");

            entity.ToTable("audits", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Browser)
                .HasMaxLength(100)
                .HasColumnName("browser");
            entity.Property(e => e.ChannelId).HasColumnName("channel_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("description");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(45)
                .HasColumnName("ip_address");
            entity.Property(e => e.ModuleActionId).HasColumnName("module_action_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<AuthCredential>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("auth_credentials_pkey");

            entity.ToTable("auth_credentials", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AppId)
                .HasMaxLength(150)
                .HasColumnName("app_id");
            entity.Property(e => e.AppKey)
                .HasMaxLength(100)
                .HasColumnName("app_key");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValueSql("'active'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Token)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("token");
        });

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("banks_pkey");

            entity.ToTable("banks", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("code");
            entity.Property(e => e.Country)
                .HasMaxLength(45)
                .HasColumnName("country");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<BankAccountType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bank_account_types_pkey");

            entity.ToTable("bank_account_types", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("businesses_pkey");

            entity.ToTable("businesses", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AdditionalIndustry)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("additional_industry");
            entity.Property(e => e.Address1).HasColumnName("address1");
            entity.Property(e => e.Address2).HasColumnName("address2");
            entity.Property(e => e.Agreement)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("agreement");
            entity.Property(e => e.BankStatement)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("bank_statement");
            entity.Property(e => e.City)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("city");
            entity.Property(e => e.CountriesOfOperation)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("countries_of_operation");
            entity.Property(e => e.Country)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("country");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CurrencyNeeded)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("currency_needed");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.FormationDocument)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("formation_document");
            entity.Property(e => e.Identifier)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("identifier");
            entity.Property(e => e.IncorporationDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("incorporation_date");
            entity.Property(e => e.MailingAddress1)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("mailing_address1");
            entity.Property(e => e.MailingAddress2)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("mailing_address2");
            entity.Property(e => e.MailingCity)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("mailing_city");
            entity.Property(e => e.MailingCountry)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("mailing_country");
            entity.Property(e => e.MailingPostalCode)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("mailing_postal_code");
            entity.Property(e => e.MailingState)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("mailing_state");
            entity.Property(e => e.MainIndustry)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("main_industry");
            entity.Property(e => e.MonthlyConversionVolumeDigitalAssets)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("monthly_conversion_volume_digital_assets");
            entity.Property(e => e.MonthlyConversionVolumeFiat)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("monthly_conversion_volume_fiat");
            entity.Property(e => e.MonthlyLocalPaymentVolume)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("monthly_local_payment_volume");
            entity.Property(e => e.MonthlyRevenue)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("monthly_revenue");
            entity.Property(e => e.MonthlySwiftVolume)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("monthly_swift_volume");
            entity.Property(e => e.Naics)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("naics");
            entity.Property(e => e.NaicsDescription)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("naics_description");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("postal_code");
            entity.Property(e => e.ProofOfOwnership)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("proof_of_ownership");
            entity.Property(e => e.ProofOfRegistration)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("proof_of_registration");
            entity.Property(e => e.State)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("state");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasDefaultValueSql("'active'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.DataUploaded)
             .HasDefaultValue(false)
             .HasColumnName("data_uploaded");
            entity.Property(e => e.DocumentUploaded)
          .HasDefaultValue(false)
          .HasColumnName("document_uploaded");
            entity.Property(e => e.TaxDocument)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("tax_document");
            entity.Property(e => e.TaxId)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("tax_id");
            entity.Property(e => e.TradeName)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("trade_name");
            entity.Property(e => e.Type)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("type");
            entity.Property(e => e.UpdateAddress)
                .HasDefaultValue(false)
                .HasColumnName("update_address");
            entity.Property(e => e.UpdateDocument)
                .HasDefaultValue(false)
                .HasColumnName("update_document");
            entity.Property(e => e.UpdateInformation)
                .HasDefaultValue(false)
                .HasColumnName("update_information");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Uuid)
                .HasMaxLength(300)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("uuid");
            entity.Property(e => e.Website).HasColumnName("website");

            entity.HasOne(d => d.User).WithMany(p => p.Businesses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_users_businesses");
        });

        modelBuilder.Entity<BusinessRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("business_roles_pkey");

            entity.ToTable("business_roles", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<BusinessTeam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("business_teams_pkey");

            entity.ToTable("business_teams", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.BusinessId).HasColumnName("business_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Uuid)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("uuid");
        });

        modelBuilder.Entity<BusinessType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("business_types_pkey");

            entity.ToTable("business_types", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(450)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Channel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("channels_pkey");

            entity.ToTable("channels", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasColumnType("bit(1)")
                .HasColumnName("active");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cities_pkey");

            entity.ToTable("cities", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(3)
                .HasDefaultValueSql("''::bpchar")
                .IsFixedLength()
                .HasColumnName("country_code");
            entity.Property(e => e.District)
                .HasMaxLength(20)
                .HasDefaultValueSql("''::bpchar")
                .IsFixedLength()
                .HasColumnName("district");
            entity.Property(e => e.Name)
                .HasMaxLength(35)
                .HasDefaultValueSql("''::bpchar")
                .IsFixedLength()
                .HasColumnName("name");
        });

        modelBuilder.Entity<Continent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("continents_pkey");

            entity.ToTable("continents", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(45)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("countries_pkey");

            entity.ToTable("countries", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(5)
                .HasColumnName("code");
            entity.Property(e => e.Code2)
                .HasMaxLength(3)
                .HasColumnName("code2");
            entity.Property(e => e.ContinentCode)
                .HasMaxLength(45)
                .HasColumnName("continent_code");
            entity.Property(e => e.Currency)
                .HasMaxLength(5)
                .HasColumnName("currency");
            entity.Property(e => e.Flag)
                .HasMaxLength(150)
                .HasColumnName("flag");
            entity.Property(e => e.Name)
                .HasMaxLength(60)
                .HasColumnName("name");
            entity.Property(e => e.Region)
                .HasMaxLength(45)
                .HasColumnName("region");
        });

        modelBuilder.Entity<CryptoTrade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("crypto_trades_pkey");

            entity.ToTable("crypto_trades", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.FromAmount)
                .HasPrecision(18, 2)
                .HasColumnName("from_amount");
            entity.Property(e => e.FromAsset)
                .HasMaxLength(45)
                .HasColumnName("from_asset");
            entity.Property(e => e.FromNetwork)
                .HasMaxLength(45)
                .HasColumnName("from_network");
            entity.Property(e => e.Reference)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("reference");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasDefaultValueSql("'pending'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.ToAmount)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("NULL::numeric")
                .HasColumnName("to_amount");
            entity.Property(e => e.ToAsset)
                .HasMaxLength(45)
                .HasColumnName("to_asset");
            entity.Property(e => e.ToNetwork)
                .HasMaxLength(45)
                .HasColumnName("to_network");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("currencies_pkey");

            entity.ToTable("currencies", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(45)
                .HasColumnName("code");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(45)
                .HasColumnName("country_code");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasDefaultValueSql("'active'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Symbol)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("symbol");
        });

        modelBuilder.Entity<Deposit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("deposits_pkey");

            entity.ToTable("deposits", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .HasMaxLength(45)
                .HasColumnName("currency");
            entity.Property(e => e.Fee)
                .HasPrecision(18, 2)
                .HasColumnName("fee");
            entity.Property(e => e.GatewayPostBack).HasColumnName("gateway_post_back");
            entity.Property(e => e.GatewayReference)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("gateway_reference");
            entity.Property(e => e.GatewayResponse).HasColumnName("gateway_response");
            entity.Property(e => e.Narration).HasColumnName("narration");
            entity.Property(e => e.PayerAccountName)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("payer_account_name");
            entity.Property(e => e.PayerAccountNumber)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("payer_account_number");
            entity.Property(e => e.PayerBank)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("payer_bank");
            entity.Property(e => e.PayerCountry)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("payer_country");
            entity.Property(e => e.Reference)
                .HasMaxLength(50)
                .HasColumnName("reference");
            entity.Property(e => e.ResponseMessage)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("response_message");
            entity.Property(e => e.SettledAmount)
                .HasPrecision(18, 2)
                .HasColumnName("settled_amount");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasDefaultValueSql("'pending'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Suspicious)
                .HasDefaultValue(false)
                .HasColumnName("suspicious");
            entity.Property(e => e.Type)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Account).WithMany(p => p.Deposits)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_accounts_deposits");
        });

        modelBuilder.Entity<Dispute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("disputes_pkey");

            entity.ToTable("disputes", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Reference)
                .HasMaxLength(100)
                .HasColumnName("reference");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasDefaultValueSql("'open'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Type)
                .HasMaxLength(45)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<DocumentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("document_types_pkey");

            entity.ToTable("document_types", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<EmploymentStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employment_statuses_pkey");

            entity.ToTable("employment_statuses", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Fee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fees_pkey");

            entity.ToTable("fees", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CappedValue)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("capped_value");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .HasColumnName("currency");
            entity.Property(e => e.Rate)
                .HasPrecision(18, 3)
                .HasColumnName("rate");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(50)
                .HasColumnName("transaction_type");
            entity.Property(e => e.MethodType)
             .HasMaxLength(50)
               .HasDefaultValueSql("'local'")
             .HasColumnName("method_type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<FxRate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fx_rates_pkey");

            entity.ToTable("fx_rates", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AccountId)
                .HasDefaultValueSql("'0'::bigint")
                .HasColumnName("account_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.FriendlyDisplayAmount)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("NULL::numeric")
                .HasColumnName("friendly_display_amount");
            entity.Property(e => e.FromCurrency)
                .HasMaxLength(50)
                .HasColumnName("from_currency");
            entity.Property(e => e.Rate)
                .HasPrecision(18, 10)
                .HasColumnName("rate");
            entity.Property(e => e.RateFromProvider)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("0.0000")
                .HasColumnName("rate_from_provider");
            entity.Property(e => e.RateMarkUp)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("0.0000")
                .HasColumnName("rate_mark_up");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'active'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.ToCurrency)
                .HasMaxLength(50)
                .HasColumnName("to_currency");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<FxRateAudit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fx_rate_audits_pkey");

            entity.ToTable("fx_rate_audits", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.BaseCurrency)
                .HasMaxLength(45)
                .HasColumnName("base_currency");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.FromCurrency)
                .HasMaxLength(50)
                .HasColumnName("from_currency");
            entity.Property(e => e.Rate)
                .HasMaxLength(191)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("rate");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'active'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.TargetCurrency)
                .HasMaxLength(45)
                .HasColumnName("target_currency");
            entity.Property(e => e.ToCurrency)
                .HasMaxLength(50)
                .HasColumnName("to_currency");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(191)
                .HasColumnName("updated_by");
        });

        modelBuilder.Entity<FxRateLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fx_rate_logs_pkey");

            entity.ToTable("fx_rate_logs", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.FriendlyRate)
                .HasPrecision(20, 10)
                .HasDefaultValueSql("0.0000000000")
                .HasColumnName("friendly_rate");
            entity.Property(e => e.FromAmount)
                .HasPrecision(20, 10)
                .HasDefaultValueSql("0.0000000000")
                .HasColumnName("from_amount");
            entity.Property(e => e.FromCurrency)
                .HasMaxLength(5)
                .HasColumnName("from_currency");
            entity.Property(e => e.Rate)
                .HasPrecision(20, 10)
                .HasDefaultValueSql("0.0000000000")
                .HasColumnName("rate");
            entity.Property(e => e.RateToken)
                .HasMaxLength(100)
                .HasColumnName("rate_token");
            entity.Property(e => e.ToAmount)
                .HasPrecision(20, 10)
                .HasDefaultValueSql("0.0000000000")
                .HasColumnName("to_amount");
            entity.Property(e => e.ToCurrency)
                .HasMaxLength(5)
                .HasColumnName("to_currency");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.ValidityInSeconds)
                .HasDefaultValue(60)
                .HasColumnName("validity_in_seconds");
        });

        modelBuilder.Entity<FxTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fx_transactions_pkey");

            entity.ToTable("fx_transactions", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Comment)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("comment");
            entity.Property(e => e.ConversionDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("conversion_date");
            entity.Property(e => e.CounterRate)
                .HasPrecision(20, 10)
                .HasDefaultValueSql("0.0000000000")
                .HasColumnName("counter_rate");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.FromAmount)
                .HasPrecision(20, 10)
                .HasDefaultValueSql("0.0000000000")
                .HasColumnName("from_amount");
            entity.Property(e => e.FromCurrency)
                .HasMaxLength(5)
                .HasColumnName("from_currency");
            entity.Property(e => e.RateToken)
                .HasMaxLength(100)
                .HasColumnName("rate_token");
            entity.Property(e => e.Reference)
                .HasMaxLength(50)
                .HasColumnName("reference");
            entity.Property(e => e.ShortReferenceId)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("short_reference_id");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValueSql("'pending'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.ToAmount)
                .HasPrecision(20, 10)
                .HasColumnName("to_amount");
            entity.Property(e => e.ToCurrency)
                .HasMaxLength(5)
                .HasColumnName("to_currency");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

            modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genders_pkey");

            entity.ToTable("genders", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<GremitAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("gremit_applications_pkey");

            entity.ToTable("gremit_accounts", "gabtrans");

            entity.Property(e => e.Id)
               .UseIdentityAlwaysColumn()
               .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Country)
                .HasMaxLength(10)
                .HasDefaultValueSql("'NG'")
                .HasColumnName("country");
            entity.Property(e => e.Status)
             .HasDefaultValueSql("'active'")
             .HasColumnName("status");
            entity.Property(e => e.Guid)
                .HasMaxLength(100)
                .HasColumnName("guid");
            entity.Property(e => e.DeliveryMethod)
               .HasMaxLength(50)
               .HasColumnName("delivery_method");
            entity.Property(e => e.Location)
                .HasMaxLength(20)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<GRemitBank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("gremit_banks_pkey");

            entity.ToTable("gremit_banks", "gabtrans");

            entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn()
            .HasColumnName("id");
            entity.Property(e => e.Country)
                .HasMaxLength(10)
                .HasColumnName("country");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("code");
            entity.Property(e => e.Name)
               .HasMaxLength(100)
               .HasColumnName("name");
            entity.Property(e => e.GenericCode)
                .HasMaxLength(100)
                .HasColumnName("generic_code");
        });

        modelBuilder.Entity<IdentityType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("identity_types_pkey");

            entity.ToTable("identity_types", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(45)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Industry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("industries_pkey");

            entity.ToTable("industries", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Integration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("integrations_pkey");

            entity.ToTable("integrations", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.BaseUrl)
                .HasMaxLength(150)
                .HasColumnName("base_url");
            entity.Property(e => e.CallBackUrl)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("call_back_url");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("password");
            entity.Property(e => e.PostBackUrl)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("post_back_url");
            entity.Property(e => e.PublicKey)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("public_key");
            entity.Property(e => e.SecretKey)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("secret_key");
            entity.Property(e => e.UserName)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("user_name");
        });

        modelBuilder.Entity<Invitation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("invitations_pkey");

            entity.ToTable("invitations", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.BusinessId).HasColumnName("business_id");
            entity.Property(e => e.Completed)
                .HasDefaultValue(false)
                .HasColumnName("completed");
            entity.Property(e => e.DateCreated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_created");
            entity.Property(e => e.DateUpdated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_updated");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(500)
                .HasColumnName("email_address");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.SecretToken)
                .HasMaxLength(150)
                .HasColumnName("secret_token");
        });

        modelBuilder.Entity<Kyc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("kycs_pkey");

            entity.ToTable("kycs", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Address1)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("address1");
            entity.Property(e => e.Address2)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("address2");
            entity.Property(e => e.AnnualIncome)
                .HasMaxLength(50)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("annual_income");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.BankStatement)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("bank_statement");
            entity.Property(e => e.BusinessDetails)
                .HasMaxLength(50)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("business_details");
            entity.Property(e => e.Citizenship)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("citizenship");
            entity.Property(e => e.City)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("country");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DateApproved)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_approved");
            entity.Property(e => e.DateCompleted)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_completed");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_of_birth");
            entity.Property(e => e.DateVerified)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_verified");
            entity.Property(e => e.Employer)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("employer");
            entity.Property(e => e.EmployerCountry)
                .HasMaxLength(50)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("employer_country");
            entity.Property(e => e.EmployerState)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("employer_state");
            entity.Property(e => e.EmploymentStatus)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("employment_status");
            entity.Property(e => e.Gender)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("gender");
            entity.Property(e => e.HasPin)
                .HasDefaultValue(false)
                .HasColumnName("has_pin");
            entity.Property(e => e.IdentityDocumentBack)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("identity_document_back");
            entity.Property(e => e.IdentityDocumentFront)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("identity_document_front");
            entity.Property(e => e.IdentityExpiryDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("identity_expiry_date");
            entity.Property(e => e.IdentityIssueDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("identity_issue_date");
            entity.Property(e => e.IdentityNumber)
                .HasMaxLength(50)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("identity_number");
            entity.Property(e => e.IdentityType)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("identity_type");
            entity.Property(e => e.IncomeCountry)
                .HasMaxLength(50)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("income_country");
            entity.Property(e => e.IncomeRange)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("income_range");
            entity.Property(e => e.IncomeSource)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("income_source");
            entity.Property(e => e.IncomeState)
                .HasMaxLength(50)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("income_state");
            entity.Property(e => e.Industry)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("industry");
            entity.Property(e => e.IsSigner)
                .HasDefaultValue(false)
                .HasColumnName("is_signer");
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("marital_status");
            entity.Property(e => e.Occupation)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("occupation");
            entity.Property(e => e.OccupationDescription).HasColumnName("occupation_description");
            entity.Property(e => e.OnboardedRepresentatives)
                .HasDefaultValue(false)
                .HasColumnName("onboarded_representatives");
            entity.Property(e => e.Outcome)
                .HasMaxLength(45)
                .HasDefaultValueSql("'pending'::character varying")
                .HasColumnName("outcome");
            entity.Property(e => e.OwnershipPercentage)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("ownership_percentage");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("postal_code");
            entity.Property(e => e.ProofOfAddress)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("proof_of_address");
            entity.Property(e => e.ReasonForRejection)
                .HasMaxLength(250)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("reason_for_rejection");
            entity.Property(e => e.ResidentialState)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("residential_state");
            entity.Property(e => e.Role)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("role");
            entity.Property(e => e.Selfie)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("selfie");
            entity.Property(e => e.SourceOfFund)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("source_of_fund");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasDefaultValueSql("'pending'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.TaxDocument)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("tax_document");
            entity.Property(e => e.TaxNumber)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("tax_number");
            entity.Property(e => e.Title)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("type");
            entity.Property(e => e.UpdateEmployment)
                .HasDefaultValue(false)
                .HasColumnName("update_employment");
            entity.Property(e => e.UpdateIdentity)
                .HasDefaultValue(false)
                .HasColumnName("update_identity");
            entity.Property(e => e.UpdatePersonal)
                .HasDefaultValue(false)
                .HasColumnName("update_personal");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Uuid)
                .HasMaxLength(300)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("uuid");
            entity.Property(e => e.Verified)
                .HasDefaultValue(false)
                .HasColumnName("verified");
            entity.Property(e => e.DataUploaded)
               .HasDefaultValue(false)
               .HasColumnName("data_uploaded");
            entity.Property(e => e.DocumentUploaded)
           .HasDefaultValue(false)
           .HasColumnName("document_uploaded");
            entity.Property(e => e.VerifyEmail)
                .HasDefaultValue(false)
                .HasColumnName("verify_email");
            entity.Property(e => e.WealthSource)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("wealth_source");
            entity.Property(e => e.WealthSourceDescription).HasColumnName("wealth_source_description");

            entity.HasOne(d => d.User).WithMany(p => p.Kycs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_kycs_users");
        });

        modelBuilder.Entity<KycRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("kyc_requests_pkey");

            entity.ToTable("kyc_requests", "gabtrans");

            entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn()
            .HasColumnName("id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ApprovedBy)
            .HasDefaultValueSql("'0'")
            .HasColumnName("approved_by");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasDefaultValueSql("'new'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<KycVerification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("kyc_verifications_pkey");

            entity.ToTable("kyc_verifications", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Reference)
                .HasMaxLength(100)
                .HasColumnName("reference");
            entity.Property(e => e.Response).HasColumnName("response");
            entity.Property(e => e.SessionId)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("session_id");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasDefaultValueSql("'pending'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<Limit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("limits_pkey");

            entity.ToTable("limits", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.AccountType)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("account_type");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .HasMaxLength(45)
                .HasColumnName("currency");
            entity.Property(e => e.DailyCount).HasColumnName("daily_count");
            entity.Property(e => e.DailyCumulative)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("NULL::numeric")
                .HasColumnName("daily_cumulative");
            entity.Property(e => e.SingleCumulative)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("NULL::numeric")
                .HasColumnName("single_cumulative");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("transaction_type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("logins_pkey");

            entity.ToTable("logins", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Attempts)
                .HasDefaultValueSql("'1'::bigint")
                .HasColumnName("attempts");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("ip_address");
            entity.Property(e => e.LastAccessed)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_accessed");
            entity.Property(e => e.RefreshToken).HasColumnName("refresh_token");
            entity.Property(e => e.RefreshTokenExpiryTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("refresh_token_expiry_time");
            entity.Property(e => e.SessionToken).HasColumnName("session_token");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Logins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_logins_users");
        });

        modelBuilder.Entity<MaritalStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("marital_statuses_pkey");

            entity.ToTable("marital_statuses", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<MessageCredential>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("message_credentials_pkey");

            entity.ToTable("message_credentials", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("display_name");
            entity.Property(e => e.Encryption)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("encryption");
            entity.Property(e => e.Host)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("host");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("password");
            entity.Property(e => e.Port)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("port");
            entity.Property(e => e.Sender)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("sender");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("username");
        });

        modelBuilder.Entity<ApplicationModule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("modules_pkey");

            entity.ToTable("modules", "gabtrans");

            entity.HasIndex(e => e.Id, "idx_module_actions_modules");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasMaxLength(10)
                .HasDefaultValueSql("'active'::character varying")
                .HasColumnName("active");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ApplicationModuleAction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("module_actions_pkey");

            entity.ToTable("module_actions", "gabtrans");

            entity.HasIndex(e => e.Id, "idx_permissions_module_actions");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.ModuleId).HasColumnName("module_id");
            entity.Property(e => e.UserAction)
                .HasMaxLength(100)
                .HasColumnName("user_action");

            entity.HasOne(d => d.Module).WithMany(p => p.ModuleActions)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_module_actions_modules");
        });

        modelBuilder.Entity<Network>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("networks_pkey");

            entity.ToTable("networks", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Occupation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("occupations_pkey");

            entity.ToTable("occupations", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .HasColumnName("name");
        });

        modelBuilder.Entity<OneTimePassword>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("one_time_passwords_pkey");

            entity.ToTable("one_time_passwords", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiredAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expired_at");
            entity.Property(e => e.OtpCategoryId).HasColumnName("otp_category_id");
            entity.Property(e => e.Token)
                .HasMaxLength(45)
                .HasColumnName("token");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.Used)
                .HasDefaultValue(false)
                .HasColumnName("used");
            entity.Property(e => e.UsedOn)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("used_on");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.OtpCategory).WithMany(p => p.OneTimePasswords)
                .HasForeignKey(d => d.OtpCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_one_time_passwords_otp_categories");

            entity.HasOne(d => d.User).WithMany(p => p.OneTimePasswords)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_one_time_passwords_users");
        });

        modelBuilder.Entity<OtpCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("otp_categories_pkey");

            entity.ToTable("otp_categories", "gabtrans");

            entity.HasIndex(e => e.Id, "idx_one_time_passwords_otp_categories");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payment_methods_pkey");

            entity.ToTable("payment_methods", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PaymentReason>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payment_reasons_pkey");

            entity.ToTable("payment_reasons", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PendingDeposit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pending_deposits_pkey");

            entity.ToTable("pending_deposits", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AllData).HasColumnName("all_data");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.BankCode)
                .HasMaxLength(15)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("bank_code");
            entity.Property(e => e.AccountUuid)
               .HasMaxLength(150)
               .HasDefaultValueSql("NULL::character varying")
               .HasColumnName("account_uuid");
            entity.Property(e => e.WalletUuid)
       .HasMaxLength(150)
       .HasDefaultValueSql("NULL::character varying")
       .HasColumnName("wallet_uuid");
            entity.Property(e => e.BankName)
                .HasMaxLength(100)
                .HasColumnName("bank_name");
            entity.Property(e => e.BeneficiaryAccount)
                .HasMaxLength(15)
                .HasColumnName("beneficiary_account");
            entity.Property(e => e.BeneficiaryAccountName)
                .HasMaxLength(100)
                .HasColumnName("beneficiary_account_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .HasMaxLength(5)
                .HasColumnName("currency");
            entity.Property(e => e.Gateway).HasColumnName("gateway");
            entity.Property(e => e.Narration).HasColumnName("narration");
            entity.Property(e => e.PaymentReference)
                .HasMaxLength(200)
                .HasColumnName("payment_reference");
            entity.Property(e => e.RetryCount).HasColumnName("retry_count");
            entity.Property(e => e.SenderAccountName)
                .HasMaxLength(100)
                .HasColumnName("sender_account_name");
            entity.Property(e => e.SenderAccountNumber)
                .HasMaxLength(100)
                .HasColumnName("sender_account_number");
            entity.Property(e => e.SessionId)
                .HasMaxLength(300)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("session_id");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .HasDefaultValueSql("'pending'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("permissions_pkey");

            entity.ToTable("permissions", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.ModuleActionId).HasColumnName("module_action_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.ModuleAction).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.ModuleActionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_permissions_module_actions");

            entity.HasOne(d => d.Role).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_permissions_roles");
        });

        modelBuilder.Entity<PlatformTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("platform_transactions_pkey");

            entity.ToTable("platform_transactions", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Reference)
                    .HasMaxLength(50)
                    .HasColumnName("reference");
            entity.Property(e => e.Gateway)
                .HasMaxLength(50)
                .HasColumnName("gateway");
            entity.Property(e => e.Request)
                .HasColumnName("request");
            entity.Property(e => e.Response)
                .HasColumnName("response");
            entity.Property(e => e.Status)
                    .HasMaxLength(15)
                      .HasColumnName("status");
            entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("timestamp without time zone")
            .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("timestamp without time zone")
            .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("provinces_pkey");

            entity.ToTable("provinces", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(100)
                .HasColumnName("country_code");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("regions_pkey");

            entity.ToTable("regions", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles", "gabtrans");

            entity.HasIndex(e => e.Id, "idx_permissions_roles");

            entity.HasIndex(e => e.Id, "idx_user_roles_roles");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValueSql("'active'::character varying")
                .HasColumnName("status");
        });

        modelBuilder.Entity<SerialNumber>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("serial_numbers_pkey");

            entity.ToTable("serial_numbers", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.LastCount).HasColumnName("last_count");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Settlement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("settlements_pkey");

            entity.ToTable("settlements", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .HasMaxLength(45)
                .HasColumnName("currency");
            entity.Property(e => e.CurrentBalance)
                .HasPrecision(18, 2)
                .HasColumnName("current_balance");
            entity.Property(e => e.DebitCreditIndicator)
                .HasMaxLength(10)
                .HasColumnName("debit_credit_indicator");
            entity.Property(e => e.Note)
                .HasMaxLength(550)
                .HasColumnName("note");
            entity.Property(e => e.PreviousBalance)
                .HasPrecision(18, 2)
                .HasColumnName("previous_balance");
            entity.Property(e => e.Reference)
                .HasMaxLength(100)
                .HasColumnName("reference");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasColumnName("status");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.WalletId).HasColumnName("wallet_id");

            entity.HasOne(d => d.Account).WithMany(p => p.Settlements)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_settlements_accounts");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Settlements)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_settlements_wallets");
        });

        modelBuilder.Entity<SourceOfFund>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("source_of_funds_pkey");

            entity.ToTable("source_of_funds", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Stage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("stages_pkey");

            entity.ToTable("stages", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("states_pkey");

            entity.ToTable("states", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("code");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(24)
                .HasColumnName("country_code");
            entity.Property(e => e.Name)
                .HasMaxLength(191)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("statuses_pkey");

            entity.ToTable("statuses", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TransactionPin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transaction_pins_pkey");

            entity.ToTable("transaction_pins", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.NewPin)
                .HasMaxLength(500)
                .HasColumnName("new_pin");
            entity.Property(e => e.OldPin)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("old_pin");
            entity.Property(e => e.Trials).HasColumnName("trials");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.TransactionPins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_transaction_pins_users");
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transaction_types_pkey");

            entity.ToTable("transaction_types", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transfers_pkey");

            entity.ToTable("transfers", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("amount");
            entity.Property(e => e.AmountPaid)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("amount_paid");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .HasMaxLength(45)
                .HasColumnName("currency");
            entity.Property(e => e.FailureReason).HasColumnName("failure_reason");
            entity.Property(e => e.Fee)
                .HasPrecision(18, 2)
                .HasColumnName("fee");
            entity.Property(e => e.Gateway)
                .HasMaxLength(45)
                .HasColumnName("gateway");
            entity.Property(e => e.GatewayReference)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("gateway_reference");
            entity.Property(e => e.GatewayRequest).HasColumnName("gateway_request");
            entity.Property(e => e.GatewayResponse).HasColumnName("gateway_response");
            entity.Property(e => e.QueryStatusResponse).HasColumnName("query_status_response");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("payment_method");
            entity.Property(e => e.PostBackResponse).HasColumnName("post_back_response");
            entity.Property(e => e.ProcessingStatus)
                .HasMaxLength(45)
                .HasDefaultValueSql("'pending'::character varying")
                .HasColumnName("processing_status");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.Reference)
                .HasMaxLength(50)
                .HasColumnName("reference");
            entity.Property(e => e.SourceCurrency)
                .HasMaxLength(50)
                .HasColumnName("source_currency");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasDefaultValueSql("'pending'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.TransferRecipientId).HasColumnName("transfer_recipient_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Account).WithMany(p => p.Transfers)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_transfers_accounts");
        });

        modelBuilder.Entity<TransferProvider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transfer_providers_pkey");

            entity.ToTable("transfer_providers", "gabtrans");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Currency).HasColumnName("currency");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasDefaultValueSql("'active'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.Provider).HasColumnName("provider");
        });

        modelBuilder.Entity<TransferRecipient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transfer_recipients_pkey");

            entity.ToTable("transfer_recipients", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.AccountName)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("account_name");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("account_number");
            entity.Property(e => e.AccountRoutingType)
                .HasMaxLength(200)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("account_routing_type");
            entity.Property(e => e.BankAccountType)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("bank_account_type");
            entity.Property(e => e.BankBranch)
                .HasMaxLength(250)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("bank_branch");
            entity.Property(e => e.BankCity)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("bank_city");
            entity.Property(e => e.BankCountry)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("bank_country");
            entity.Property(e => e.BankName)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("bank_name");
            entity.Property(e => e.BankPostalCode)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("bank_postal_code");
            entity.Property(e => e.BankState)
                .HasMaxLength(200)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("bank_state");
            entity.Property(e => e.BankStreetAddress)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("bank_street_address");
            entity.Property(e => e.City)
                .HasMaxLength(200)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("country");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("currency");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_of_birth");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("email");
            entity.Property(e => e.IntermediaryBankCountry)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("intermediary_bank_country");
            entity.Property(e => e.IntermediaryBankName)
                .HasMaxLength(200)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("intermediary_bank_name");
            entity.Property(e => e.IntermediaryCity)
                .HasMaxLength(200)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("intermediary_city");
            entity.Property(e => e.IntermediaryPostalCode)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("intermediary_postal_code");
            entity.Property(e => e.IntermediaryRoutingCode)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("intermediary_routing_code");
            entity.Property(e => e.IntermediaryState)
                .HasMaxLength(200)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("intermediary_state");
            entity.Property(e => e.IntermediaryStreet1)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("intermediary_street1");
            entity.Property(e => e.IntermediaryStreet2)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("intermediary_street2");
            entity.Property(e => e.InternationalBankName)
                .HasMaxLength(200)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("international_bank_name");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("name");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("payment_method");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("phone_number");
            entity.Property(e => e.PostCode)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("post_code");
            entity.Property(e => e.RoutingNumber)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("routing_number");
            entity.Property(e => e.State)
                .HasMaxLength(250)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("state");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasDefaultValueSql("'active'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.StreetAddress)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("street_address");
            entity.Property(e => e.SwiftCode)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("swift_code");
            entity.Property(e => e.Type)
                .HasMaxLength(45)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.Uuid)
                .HasMaxLength(300)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("uuid");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users", "gabtrans");

            entity.HasIndex(e => e.Id, "idx_kycs_users");

            entity.HasIndex(e => e.Id, "idx_logins_users");

            entity.HasIndex(e => e.Id, "idx_one_time_passwords_users");

            entity.HasIndex(e => e.Id, "idx_transaction_pins_users");

            entity.HasIndex(e => e.Id, "idx_user_roles_users");

            entity.HasIndex(e => e.Id, "idx_user_tokens_users");

            entity.HasIndex(e => e.Id, "idx_users_accounts");

            entity.HasIndex(e => e.Id, "idx_users_businesses");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeviceId)
                .HasMaxLength(150)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("device_id");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(150)
                .HasColumnName("email_address");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("first_name");
            entity.Property(e => e.LastLogin)
                .HasMaxLength(50)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("last_login");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("last_name");
            entity.Property(e => e.LockedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("locked_at");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("middle_name");
            entity.Property(e => e.OldPassword).HasColumnName("old_password");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("phone_number");
            entity.Property(e => e.Status)
                .HasMaxLength(40)
                .HasDefaultValueSql("'active'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_roles_pkey");

            entity.ToTable("user_roles", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_roles_roles");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_roles_users");
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_tokens_pkey");

            entity.ToTable("user_tokens", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(false)
                .HasColumnName("active");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.UniqueToken)
                .HasMaxLength(500)
                .HasColumnName("unique_token");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_tokens_users");
        });

        modelBuilder.Entity<VirtualAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("virtual_accounts_pkey");

            entity.ToTable("virtual_accounts", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AccountHolderName)
                .HasMaxLength(500)
                .HasColumnName("account_holder_name");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .HasColumnName("account_number");
            entity.Property(e => e.BankCity)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("bank_city");
            entity.Property(e => e.BankName)
                .HasMaxLength(250)
                .HasColumnName("bank_name");
            entity.Property(e => e.BankPostalCode)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("bank_postal_code");
            entity.Property(e => e.BankState)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("bank_state");
            entity.Property(e => e.BankStreet1)
                .HasMaxLength(200)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("bank_street1");
            entity.Property(e => e.BankStreet2)
                .HasMaxLength(200)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("bank_street2");
            entity.Property(e => e.Country)
                .HasMaxLength(45)
                .HasColumnName("country");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .HasMaxLength(45)
                .HasColumnName("currency");
            entity.Property(e => e.ReferenceCode)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("reference_code");
            entity.Property(e => e.RoutingNumber)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("routing_number");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasDefaultValueSql("'active'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.SwiftCode)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("swift_code");
            entity.Property(e => e.Type)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Account).WithMany(p => p.VirtualAccounts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_virtual_accounts_accounts");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("wallets_pkey");

            entity.ToTable("wallets", "gabtrans");

            entity.HasIndex(e => e.Id, "idx_settlements_wallets");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("address");
            entity.Property(e => e.Asset)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("asset");
            entity.Property(e => e.Balance)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("balance");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .HasMaxLength(45)
                .HasColumnName("currency");
            entity.Property(e => e.Network)
                .HasMaxLength(45)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("network");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasDefaultValueSql("'inactive'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.Uuid)
                .HasMaxLength(500)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("uuid");

            entity.HasOne(d => d.Account).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_wallets_accounts");
        });

        modelBuilder.Entity<WebHook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("web_hooks_pkey");

            entity.ToTable("web_hooks", "gabtrans");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Provider)
                .HasMaxLength(45)
                .HasColumnName("provider");
            entity.Property(e => e.ReferenceNumber)
                .HasMaxLength(100)
                .HasColumnName("reference_number");
            entity.Property(e => e.Request).HasColumnName("request");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasColumnName("status");
        });

        modelBuilder.Entity<AuditDetails>(entity =>
        {
            entity.HasNoKey();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
