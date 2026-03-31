using Microsoft.EntityFrameworkCore;
using Ressource_API.Features.Addresses.Models;
using Ressource_API.Features.Articles.Models;
using Ressource_API.Features.BackofficeLogLevels.Models;
using Ressource_API.Features.BackofficeLogs.Models;
using Ressource_API.Features.BackofficeOperationTypes.Models;
using Ressource_API.Features.Cities.Models;
using Ressource_API.Features.Comments.Models;
using Ressource_API.Features.Departments.Models;
using Ressource_API.Features.EmailLogs.Models;
using Ressource_API.Features.Events.Models;
using Ressource_API.Features.FriendsRequests.Models;
using Ressource_API.Features.Logins.Models;
using Ressource_API.Features.Notifications.Models;
using Ressource_API.Features.PasswordHistories.Models;
using Ressource_API.Features.PasswordInfos.Models;
using Ressource_API.Features.PollOptions.Models;
using Ressource_API.Features.Polls.Models;
using Ressource_API.Features.ProfilePictures.Models;
using Ressource_API.Features.Quizzes.Models;
using Ressource_API.Features.QuizzQuestions.Models;
using Ressource_API.Features.RefreshTokens.Models;
using Ressource_API.Features.Regions.Models;
using Ressource_API.Features.Reports.Models;
using Ressource_API.Features.ReportTypes.Models;
using Ressource_API.Features.RessourceConfidentialityTypes.Models;
using Ressource_API.Features.RessourceMedias.Models;
using Ressource_API.Features.RessourceProgressions.Models;
using Ressource_API.Features.Ressources.Models;
using Ressource_API.Features.RessourceStatuses.Models;
using Ressource_API.Features.RessourceTypes.Models;
using Ressource_API.Features.SessionMessages.Models;
using Ressource_API.Features.Sessions.Models;
using Ressource_API.Features.Tags.Models;
using Ressource_API.Features.UserRoles.Models;
using Ressource_API.Features.Users.Models;

namespace Ressource_API.Common.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<BackofficeLog> BackofficeLogs { get; set; }

    public virtual DbSet<BackofficeLogLevel> BackofficeLogLevels { get; set; }

    public virtual DbSet<BackofficeOperationType> BackofficeOperationTypes { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<EmailLog> EmailLogs { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<FriendsRequest> FriendsRequests { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<PasswordHistory> PasswordsHistories { get; set; }

    public virtual DbSet<PasswordInfo> PasswordsInfos { get; set; }

    public virtual DbSet<Poll> Polls { get; set; }

    public virtual DbSet<PollOption> PollsOptions { get; set; }

    public virtual DbSet<ProfilePicture> ProfilesPictures { get; set; }

    public virtual DbSet<Quizz> Quizzes { get; set; }

    public virtual DbSet<QuizzQuestion> QuizzesQuestions { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<ReportType> ReportsTypes { get; set; }

    public virtual DbSet<Ressource> Ressources { get; set; }

    public virtual DbSet<RessourceProgression> RessourceProgressions { get; set; }

    public virtual DbSet<RessourceType> RessourceTypes { get; set; }

    public virtual DbSet<RessourceConfidentialityType> RessourcesConfidentialityTypes { get; set; }

    public virtual DbSet<RessourceMedia> RessourcesMedias { get; set; }

    public virtual DbSet<RessourceStatus> RessourcesStatuses { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<SessionMessage> SessionsMessages { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UsersRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("addresses_pk");

            entity.ToTable("addresses");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.DeletionTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("deletion_time");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.RegionId).HasColumnName("region_id");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.City).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("addresses_city_id_fk");

            entity.HasOne(d => d.Department).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("addresses_department_id_fk");

            entity.HasOne(d => d.Region).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("addresses_region_id_fk");

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("addresses_user_id_fk");
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("articles_pk");

            entity.ToTable("articles");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.RessourceId).HasColumnName("ressource_id");

            entity.HasOne(d => d.Ressource).WithMany(p => p.Articles)
                .HasForeignKey(d => d.RessourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("articles_ressource_id_fk");
        });

        modelBuilder.Entity<BackofficeLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("backoffice_logs_pk");

            entity.ToTable("backoffice_logs");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BackofficeLogLevelId).HasColumnName("backoffice_log_level_id");
            entity.Property(e => e.BackofficeOperationTypeId).HasColumnName("backoffice_operation_type_id");
            entity.Property(e => e.EventTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("event_time");
            entity.Property(e => e.LogContent).HasColumnName("log_content");

            entity.HasOne(d => d.BackofficeLogLevel).WithMany(p => p.BackofficeLogs)
                .HasForeignKey(d => d.BackofficeLogLevelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("backoffice_logs_backoffice_log_level_id_fk");

            entity.HasOne(d => d.BackofficeOperationType).WithMany(p => p.BackofficeLogs)
                .HasForeignKey(d => d.BackofficeOperationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("backoffice_logs_backoffice_operation_type_id_fk");
        });

        modelBuilder.Entity<BackofficeLogLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("backoffice_log_levels_pk");

            entity.ToTable("backoffice_log_levels");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Label)
                .HasMaxLength(50)
                .HasColumnName("label");
        });

        modelBuilder.Entity<BackofficeOperationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("backoffice_operation_types_pk");

            entity.ToTable("backoffice_operation_types");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Label)
                .HasMaxLength(50)
                .HasColumnName("label");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cities_pk");

            entity.ToTable("cities");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DepartmentCode)
                .HasMaxLength(3)
                .HasColumnName("department_code");
            entity.Property(e => e.GpsLat).HasColumnName("gps_lat");
            entity.Property(e => e.GpsLng).HasColumnName("gps_lng");
            entity.Property(e => e.InseeCode)
                .HasMaxLength(5)
                .HasColumnName("insee_code");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(5)
                .HasColumnName("zip_code");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comments_pk");

            entity.ToTable("comments");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.Content)
                .HasMaxLength(255)
                .HasColumnName("content");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.DeletionTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("deletion_time");
            entity.Property(e => e.RessourceId).HasColumnName("ressource_id");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.ParentComment).WithMany(p => p.Replies)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("comments_comment_id_fk");

            entity.HasOne(d => d.Ressource).WithMany(p => p.Comments)
                .HasForeignKey(d => d.RessourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comments_ressource_id_fk");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comments_user_id_fk");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("departments_pk");

            entity.ToTable("departments");

            entity.HasIndex(e => e.Code, "code_idx");

            entity.HasIndex(e => e.RegionCode, "region_code_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(3)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(3)
                .HasColumnName("region_code");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
        });

        modelBuilder.Entity<EmailLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("email_logs_pk");

            entity.ToTable("email_logs");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.OperationType)
                .HasMaxLength(255)
                .HasColumnName("operation_type");
            entity.Property(e => e.ReceiverEmail)
                .HasMaxLength(100)
                .HasColumnName("receiver_email");
            entity.Property(e => e.SenderEmail)
                .HasMaxLength(100)
                .HasColumnName("sender_email");
            entity.Property(e => e.SentTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("sent_time");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("events_pk");

            entity.ToTable("events");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DateEnd)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("date_end");
            entity.Property(e => e.DateStart)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("date_start");
            entity.Property(e => e.EventLink).HasColumnName("event_link");
            entity.Property(e => e.IsVirtual).HasColumnName("is_virtual");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.RessourceId).HasColumnName("ressource_id");

            entity.HasOne(d => d.Ressource).WithMany()
                .HasForeignKey(d => d.RessourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("events_ressource_id_fk");
        });

        modelBuilder.Entity<FriendsRequest>(entity =>
        {
            entity.HasKey(e => new { e.UserSenderId, e.UserReceiverId }).HasName("friends_requests_pk");

            entity.ToTable("friends_requests");

            entity.Property(e => e.UserSenderId).HasColumnName("user_sender_id");
            entity.Property(e => e.UserReceiverId).HasColumnName("user_receiver_id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.DeletionTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("deletion_time");
            entity.Property(e => e.RequestStatus)
                .HasMaxLength(255)
                .HasColumnName("request_status");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");

            entity.HasOne(d => d.UserReceiver).WithMany(p => p.ReceivedFriendRequests)
                .HasForeignKey(d => d.UserReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("friends_requests_user_receiver_id_fk");

            entity.HasOne(d => d.UserSender).WithMany(p => p.SentFriendRequests)
                .HasForeignKey(d => d.UserSenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("friends_requests_user_sender_id_fk");
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("logins_pk");

            entity.ToTable("logins");

            entity.HasIndex(e => e.Email, "email_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.DeletionTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("deletion_time");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Logins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("logins_user_id_fk");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("notifications_pk");

            entity.ToTable("notifications");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.MarkedAsRead).HasColumnName("marked_as_read");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("notifications_user_id_fk");
        });

        modelBuilder.Entity<PasswordHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("passwords_history_pk");

            entity.ToTable("passwords_history");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.DeletionTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("deletion_time");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.PasswordInfosId).HasColumnName("password_infos_id");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");

            entity.HasOne(d => d.PasswordInfos).WithMany(p => p.PasswordsHistories)
                .HasForeignKey(d => d.PasswordInfosId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("passwords_history_password_infos_id_fk");
        });

        modelBuilder.Entity<PasswordInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("passwords_infos_pk");

            entity.ToTable("passwords_infos");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AttemptCount).HasColumnName("attempt_count");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.DeletionTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("deletion_time");
            entity.Property(e => e.ResetDate)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("reset_date");
            entity.Property(e => e.ResetToken).HasColumnName("reset_token");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.PasswordsInfos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("passwords_infos_user_id_fk");
        });

        modelBuilder.Entity<Poll>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("polls_pk");

            entity.ToTable("polls");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.RessourceId).HasColumnName("ressource_id");
            entity.Property(e => e.VoteCount).HasColumnName("vote_count");

            entity.HasOne(d => d.Ressource).WithMany()
                .HasForeignKey(d => d.RessourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("polls_ressource_id_fk");
        });

        modelBuilder.Entity<PollOption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("polls_options_pk");

            entity.ToTable("polls_options");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.DeletionTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("deletion_time");
            entity.Property(e => e.Option)
                .HasMaxLength(255)
                .HasColumnName("option");
            entity.Property(e => e.PollId).HasColumnName("poll_id");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");

            entity.HasOne(d => d.Poll).WithMany(p => p.PollsOptions)
                .HasForeignKey(d => d.PollId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("polls_options_poll_id_fk");
        });

        modelBuilder.Entity<ProfilePicture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("profiles_pictures_pk");

            entity.ToTable("profiles_pictures");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.DeletionTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("deletion_time");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.ProfilesPictures)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("profiles_pictures_user_id_fk");
        });

        modelBuilder.Entity<Quizz>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("quizzes_pk");

            entity.ToTable("quizzes");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ParticipationCount).HasColumnName("participation_count");
            entity.Property(e => e.RessourceId).HasColumnName("ressource_id");

            entity.HasOne(d => d.Ressource).WithMany(p => p.Quizzes)
                .HasForeignKey(d => d.RessourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("quizzes_ressource_id_fk");
        });

        modelBuilder.Entity<QuizzQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("quizzes_questions_pk");

            entity.ToTable("quizzes_questions");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CorrectAnswer)
                .HasMaxLength(255)
                .HasColumnName("correct_answer");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.DeletionTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("deletion_time");
            entity.Property(e => e.PossibleAnswers)
                .HasColumnType("jsonb")
                .HasColumnName("possible_answers");
            entity.Property(e => e.Question)
                .HasMaxLength(255)
                .HasColumnName("question");
            entity.Property(e => e.QuizzId).HasColumnName("quizz_id");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");

            entity.HasOne(d => d.Quizz).WithMany(p => p.QuizzesQuestions)
                .HasForeignKey(d => d.QuizzId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("quizzes_questions_quizz_id_fk");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("refresh_tokens_pk");

            entity.HasIndex(e => e.Token, "token_hash_IDX");

            entity.ToTable("refresh_tokens");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.ExpirationTime).HasColumnName("expiration_time");
            entity.Property(e => e.Token).HasColumnName("refresh_token");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("refresh_tokens_user_id_fk");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("regions_pk");

            entity.ToTable("regions");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(3)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reports_pk");

            entity.ToTable("reports");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.IsCheckedByModerator).HasColumnName("is_checked_by_moderator");
            entity.Property(e => e.ReportTypeId).HasColumnName("report_type_id");
            entity.Property(e => e.RessourceId).HasColumnName("ressource_id");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.ReportType).WithMany(p => p.Reports)
                .HasForeignKey(d => d.ReportTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reports_report_type_id_fk");

            entity.HasOne(d => d.Ressource).WithMany(p => p.Reports)
                .HasForeignKey(d => d.RessourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reports_ressource_id_fk");

            entity.HasOne(d => d.User).WithMany(p => p.Reports)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reports_user_id_fk");
        });

        modelBuilder.Entity<ReportType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reports_types_pk");

            entity.ToTable("reports_types");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.DeletionTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("deletion_time");
            entity.Property(e => e.Label)
                .HasMaxLength(50)
                .HasColumnName("label");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");
        });

        modelBuilder.Entity<Ressource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ressources_pk");

            entity.ToTable("ressources");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.DeletionTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("deletion_time");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.RessourceConfidentialityTypeId).HasColumnName("ressource_confidentiality_type_id");
            entity.Property(e => e.RessourceStatusId).HasColumnName("ressource_status_id");
            entity.Property(e => e.RessourceTypeId).HasColumnName("ressource_type_id");
            entity.Property(e => e.ThumbnailId).HasColumnName("thumbnail_id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ViewCount).HasColumnName("view_count");

            entity.HasOne(d => d.RessourceConfidentialityType).WithMany()
                .HasForeignKey(d => d.RessourceConfidentialityTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ressources_ressource_confidentiality_type_id_fk");

            entity.HasOne(d => d.RessourceStatus).WithMany()
                .HasForeignKey(d => d.RessourceStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ressources_ressource_status_id_fk");

            entity.HasOne(d => d.RessourceType).WithMany()
                .HasForeignKey(d => d.RessourceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ressources_ressource_type_id_fk");

            entity.HasOne(d => d.User).WithMany(p => p.AuthoredRessources)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ressources_user_id_fk");

            entity.HasOne(d => d.Thumbnail).WithMany()
                .HasForeignKey(d => d.ThumbnailId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("ressources_thumbnail_id_fk");
        });

        modelBuilder.Entity<RessourceProgression>(entity =>
        {
            entity.HasKey(e => new { e.RessourceId, e.UserId }).HasName("ressource_progression_pk");

            entity.ToTable("ressource_progression");

            entity.Property(e => e.RessourceId).HasColumnName("ressource_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.IsAside).HasColumnName("is_aside");
            entity.Property(e => e.IsExploited).HasColumnName("is_exploited");

            entity.HasOne(d => d.Ressource).WithMany(p => p.RessourceProgressions)
                .HasForeignKey(d => d.RessourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ressource_progression_ressource_id_fk");

            entity.HasOne(d => d.User).WithMany(p => p.RessourceProgressions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ressource_progression_user_id_fk");
        });

        modelBuilder.Entity<RessourceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ressource_types_pk");

            entity.ToTable("ressource_types");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.DeletionTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("deletion_time");
            entity.Property(e => e.Label)
                .HasMaxLength(50)
                .HasColumnName("label");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");
        });

        modelBuilder.Entity<RessourceConfidentialityType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ressources_confidentiality_types_pk");

            entity.ToTable("ressources_confidentiality_types");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.DeletionTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("deletion_time");
            entity.Property(e => e.Label)
                .HasMaxLength(50)
                .HasColumnName("label");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");
        });

        modelBuilder.Entity<RessourceMedia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ressources_medias_pk");

            entity.ToTable("ressources_medias");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.DeletionTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("deletion_time");
            entity.Property(e => e.MediaUrl).HasColumnName("media_url");
            entity.Property(e => e.MimeType)
                .HasMaxLength(50)
                .HasColumnName("mime_type");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");
        });

        modelBuilder.Entity<RessourceStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ressources_status_pk");

            entity.ToTable("ressources_status");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.DeletionTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("deletion_time");
            entity.Property(e => e.Label)
                .HasMaxLength(50)
                .HasColumnName("label");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sessions_pk");

            entity.ToTable("sessions");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.IdWs).HasColumnName("id_ws");
            entity.Property(e => e.RessourceId).HasColumnName("ressource_id");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");

            entity.HasOne(d => d.Ressource).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.RessourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sessions_ressource_id_fk");
        });

        modelBuilder.Entity<SessionMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sessions_messages_pk");

            entity.ToTable("sessions_messages");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Content)
                .HasMaxLength(255)
                .HasColumnName("content");
            entity.Property(e => e.SentTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("sent_time");
            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Session).WithMany(p => p.SessionsMessages)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sessions_messages_session_id_fk");

            entity.HasOne(d => d.User).WithMany(p => p.SessionsMessages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sessions_messages_user_id_fk");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tags_pk");

            entity.ToTable("tags");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.DeletionTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("deletion_time");
            entity.Property(e => e.Label)
                .HasMaxLength(50)
                .HasColumnName("label");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");

            entity.HasMany<Ressource>().WithMany(p => p.Tags)
                .UsingEntity<Dictionary<string, object>>(
                    "RessourceTag",
                    r => r.HasOne<Ressource>().WithMany()
                        .HasForeignKey("RessourceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("ressource_tag_ressource_id_fk"),
                    l => l.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("ressource_tag_tag_id_fk"),
                    j =>
                    {
                        j.HasKey("TagId", "RessourceId").HasName("ressource_tag_pk");
                        j.ToTable("ressource_tag");
                        j.IndexerProperty<Guid>("TagId").HasColumnName("tag_id");
                        j.IndexerProperty<Guid>("RessourceId").HasColumnName("ressource_id");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pk");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ActivationCode).HasColumnName("activation_code");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.DeletionTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("deletion_time");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .HasColumnName("user_name");
            entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");

            entity.HasOne(d => d.UserRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_user_role_id_fk");

            entity.HasMany(d => d.Events).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "EventMember",
                    r => r.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("event_member_event_id_fk"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("event_member_user_id_fk"),
                    j =>
                    {
                        j.HasKey("UserId", "EventId").HasName("event_member_pk");
                        j.ToTable("event_member");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<Guid>("EventId").HasColumnName("event_id");
                    });

            entity.HasMany(d => d.PollOptions).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "PollVote",
                    r => r.HasOne<PollOption>().WithMany()
                        .HasForeignKey("PollOptionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("poll_vote_poll_option_id_fk"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("poll_vote_user_id_fk"),
                    j =>
                    {
                        j.HasKey("UserId", "PollOptionId").HasName("poll_vote_pk");
                        j.ToTable("poll_vote");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<Guid>("PollOptionId").HasColumnName("poll_option_id");
                    });

            entity.HasMany(d => d.QuizzQuestions).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "QuestionAnswer",
                    r => r.HasOne<QuizzQuestion>().WithMany()
                        .HasForeignKey("QuizzQuestionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("question_answer_quizz_question_id_fk"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("question_answer_user_id_fk"),
                    j =>
                    {
                        j.HasKey("UserId", "QuizzQuestionId").HasName("question_answer_pk");
                        j.ToTable("question_answer");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<Guid>("QuizzQuestionId").HasColumnName("quizz_question_id");
                    });

            entity.HasMany(d => d.LikedRessources).WithMany(p => p.LikedByUsers)
                .UsingEntity<Dictionary<string, object>>(
                    "RessourceLike",
                    r => r.HasOne<Ressource>().WithMany()
                        .HasForeignKey("RessourceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("ressource_like_ressource_id_fk"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("ressource_like_user_id_fk"),
                    j =>
                    {
                        j.HasKey("UserId", "RessourceId").HasName("ressource_like_pk");
                        j.ToTable("ressource_like");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<Guid>("RessourceId").HasColumnName("ressource_id");
                    });

            entity.HasMany(d => d.FavoritedRessources).WithMany(p => p.FavoritedByUsers)
                .UsingEntity<Dictionary<string, object>>(
                    "RessourceFavorite",
                    r => r.HasOne<Ressource>().WithMany()
                        .HasForeignKey("RessourceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("ressource_favorite_ressource_id_fk"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("ressource_favorite_user_id_fk"),
                    j =>
                    {
                        j.HasKey("UserId", "RessourceId").HasName("ressource_favorite_pk");
                        j.ToTable("ressource_favorite");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<Guid>("RessourceId").HasColumnName("ressource_id");
                    });
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_roles_pk");

            entity.ToTable("users_roles");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.DeletionTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("deletion_time");
            entity.Property(e => e.RoleLabel)
                .HasMaxLength(50)
                .HasColumnName("role_label");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("update_time");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}