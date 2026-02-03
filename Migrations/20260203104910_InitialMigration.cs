using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ressource_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateTable(
            //     name: "backoffice_log_levels",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("backoffice_log_levels_pk", x => x.id);
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "backoffice_operation_types",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("backoffice_operation_types_pk", x => x.id);
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "cities",
            //     columns: table => new
            //     {
            //         id = table.Column<int>(type: "integer", nullable: false),
            //         department_code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
            //         insee_code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
            //         zip_code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
            //         name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         gps_lat = table.Column<float>(type: "real", nullable: false),
            //         gps_lng = table.Column<float>(type: "real", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("cities_pk", x => x.id);
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "departments",
            //     columns: table => new
            //     {
            //         id = table.Column<int>(type: "integer", nullable: false),
            //         region_code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
            //         code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
            //         name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("departments_pk", x => x.id);
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "email_logs",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         sent_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         content = table.Column<string>(type: "text", nullable: false),
            //         sender_email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         receiver_email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         operation_type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("email_logs_pk", x => x.id);
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "regions",
            //     columns: table => new
            //     {
            //         id = table.Column<int>(type: "integer", nullable: false),
            //         code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
            //         name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("regions_pk", x => x.id);
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "reports_types",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("reports_types_pk", x => x.id);
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "ressource_types",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("ressource_types_pk", x => x.id);
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "ressources_confidentiality_types",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("ressources_confidentiality_types_pk", x => x.id);
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "ressources_status",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("ressources_status_pk", x => x.id);
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "tags",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("tags_pk", x => x.id);
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "users_roles",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         role_label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("users_roles_pk", x => x.id);
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "backoffice_logs",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         event_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         log_content = table.Column<string>(type: "text", nullable: false),
            //         backoffice_log_level_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         backoffice_operation_type_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("backoffice_logs_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "backoffice_logs_backoffice_log_level_id_fk",
            //             column: x => x.backoffice_log_level_id,
            //             principalTable: "backoffice_log_levels",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "backoffice_logs_backoffice_operation_type_id_fk",
            //             column: x => x.backoffice_operation_type_id,
            //             principalTable: "backoffice_operation_types",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "users",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         user_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         activation_code = table.Column<Guid>(type: "uuid", nullable: true),
            //         is_active = table.Column<bool>(type: "boolean", nullable: false),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         user_role_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("users_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "users_user_role_id_fk",
            //             column: x => x.user_role_id,
            //             principalTable: "users_roles",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "addresses",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         region_id = table.Column<int>(type: "integer", nullable: false),
            //         department_id = table.Column<int>(type: "integer", nullable: false),
            //         city_id = table.Column<int>(type: "integer", nullable: false),
            //         user_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("addresses_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "addresses_city_id_fk",
            //             column: x => x.city_id,
            //             principalTable: "cities",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "addresses_department_id_fk",
            //             column: x => x.department_id,
            //             principalTable: "departments",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "addresses_region_id_fk",
            //             column: x => x.region_id,
            //             principalTable: "regions",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "addresses_user_id_fk",
            //             column: x => x.user_id,
            //             principalTable: "users",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "friends_requests",
            //     columns: table => new
            //     {
            //         user_sender_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         user_receiver_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         request_status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("friends_requests_pk", x => new { x.user_sender_id, x.user_receiver_id });
            //         table.ForeignKey(
            //             name: "friends_requests_user_receiver_id_fk",
            //             column: x => x.user_receiver_id,
            //             principalTable: "users",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "friends_requests_user_sender_id_fk",
            //             column: x => x.user_sender_id,
            //             principalTable: "users",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "logins",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         password_hash = table.Column<string>(type: "text", nullable: false),
            //         password_salt = table.Column<string>(type: "text", nullable: false),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         user_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("logins_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "logins_user_id_fk",
            //             column: x => x.user_id,
            //             principalTable: "users",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "notifications",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         marked_as_read = table.Column<bool>(type: "boolean", nullable: false),
            //         content = table.Column<string>(type: "text", nullable: false),
            //         user_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("notifications_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "notifications_user_id_fk",
            //             column: x => x.user_id,
            //             principalTable: "users",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "passwords_infos",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         attempt_count = table.Column<int>(type: "integer", nullable: false),
            //         reset_token = table.Column<Guid>(type: "uuid", nullable: true),
            //         reset_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         user_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("passwords_infos_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "passwords_infos_user_id_fk",
            //             column: x => x.user_id,
            //             principalTable: "users",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "profiles_pictures",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         image_url = table.Column<string>(type: "text", nullable: false),
            //         user_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("profiles_pictures_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "profiles_pictures_user_id_fk",
            //             column: x => x.user_id,
            //             principalTable: "users",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "refresh_tokens",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         refresh_token = table.Column<string>(type: "text", nullable: false),
            //         is_active = table.Column<bool>(type: "boolean", nullable: false),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         user_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("refresh_tokens_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "refresh_tokens_user_id_fk",
            //             column: x => x.user_id,
            //             principalTable: "users",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "ressources",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         description = table.Column<string>(type: "text", nullable: false),
            //         view_count = table.Column<long>(type: "bigint", nullable: false),
            //         thumbnail_url = table.Column<string>(type: "text", nullable: false),
            //         ressource_confidentiality_type_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         ressource_status_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         user_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         ressource_type_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("ressources_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "ressources_ressource_confidentiality_type_id_fk",
            //             column: x => x.ressource_confidentiality_type_id,
            //             principalTable: "ressources_confidentiality_types",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "ressources_ressource_status_id_fk",
            //             column: x => x.ressource_status_id,
            //             principalTable: "ressources_status",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "ressources_ressource_type_id_fk",
            //             column: x => x.ressource_type_id,
            //             principalTable: "ressource_types",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "ressources_user_id_fk",
            //             column: x => x.user_id,
            //             principalTable: "users",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "passwords_history",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         password_hash = table.Column<string>(type: "text", nullable: false),
            //         password_salt = table.Column<string>(type: "text", nullable: false),
            //         password_infos_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("passwords_history_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "passwords_history_password_infos_id_fk",
            //             column: x => x.password_infos_id,
            //             principalTable: "passwords_infos",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "articles",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         content = table.Column<string>(type: "text", nullable: false),
            //         ressource_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("articles_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "articles_ressource_id_fk",
            //             column: x => x.ressource_id,
            //             principalTable: "ressources",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "comments",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         content = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         comment_id = table.Column<Guid>(type: "uuid", nullable: true),
            //         ressource_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         user_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("comments_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "comments_comment_id_fk",
            //             column: x => x.comment_id,
            //             principalTable: "comments",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "comments_ressource_id_fk",
            //             column: x => x.ressource_id,
            //             principalTable: "ressources",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "comments_user_id_fk",
            //             column: x => x.user_id,
            //             principalTable: "users",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "events",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         is_virtual = table.Column<bool>(type: "boolean", nullable: false),
            //         date_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         date_end = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         event_link = table.Column<string>(type: "text", nullable: true),
            //         location = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         ressource_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("events_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "events_ressource_id_fk",
            //             column: x => x.ressource_id,
            //             principalTable: "ressources",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "polls",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         vote_count = table.Column<long>(type: "bigint", nullable: false),
            //         ressource_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("polls_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "polls_ressource_id_fk",
            //             column: x => x.ressource_id,
            //             principalTable: "ressources",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "quizzes",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         participation_count = table.Column<long>(type: "bigint", nullable: false),
            //         ressource_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("quizzes_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "quizzes_ressource_id_fk",
            //             column: x => x.ressource_id,
            //             principalTable: "ressources",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "reports",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         is_checked_by_moderator = table.Column<bool>(type: "boolean", nullable: false),
            //         report_type_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         user_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         ressource_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("reports_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "reports_report_type_id_fk",
            //             column: x => x.report_type_id,
            //             principalTable: "reports_types",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "reports_ressource_id_fk",
            //             column: x => x.ressource_id,
            //             principalTable: "ressources",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "reports_user_id_fk",
            //             column: x => x.user_id,
            //             principalTable: "users",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "ressource_favorite",
            //     columns: table => new
            //     {
            //         user_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         ressource_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("ressource_favorite_pk", x => new { x.user_id, x.ressource_id });
            //         table.ForeignKey(
            //             name: "ressource_favorite_ressource_id_fk",
            //             column: x => x.ressource_id,
            //             principalTable: "ressources",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "ressource_favorite_user_id_fk",
            //             column: x => x.user_id,
            //             principalTable: "users",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "ressource_like",
            //     columns: table => new
            //     {
            //         user_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         ressource_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("ressource_like_pk", x => new { x.user_id, x.ressource_id });
            //         table.ForeignKey(
            //             name: "ressource_like_ressource_id_fk",
            //             column: x => x.ressource_id,
            //             principalTable: "ressources",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "ressource_like_user_id_fk",
            //             column: x => x.user_id,
            //             principalTable: "users",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "ressource_progression",
            //     columns: table => new
            //     {
            //         ressource_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         user_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         is_aside = table.Column<bool>(type: "boolean", nullable: false),
            //         is_exploited = table.Column<bool>(type: "boolean", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("ressource_progression_pk", x => new { x.ressource_id, x.user_id });
            //         table.ForeignKey(
            //             name: "ressource_progression_ressource_id_fk",
            //             column: x => x.ressource_id,
            //             principalTable: "ressources",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "ressource_progression_user_id_fk",
            //             column: x => x.user_id,
            //             principalTable: "users",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "ressource_tag",
            //     columns: table => new
            //     {
            //         tag_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         ressource_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("ressource_tag_pk", x => new { x.tag_id, x.ressource_id });
            //         table.ForeignKey(
            //             name: "ressource_tag_ressource_id_fk",
            //             column: x => x.ressource_id,
            //             principalTable: "ressources",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "ressource_tag_tag_id_fk",
            //             column: x => x.tag_id,
            //             principalTable: "tags",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "ressources_medias",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         media_url = table.Column<string>(type: "text", nullable: false),
            //         mime_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         ressource_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("ressources_medias_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "ressources_medias_ressource_id_fk",
            //             column: x => x.ressource_id,
            //             principalTable: "ressources",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "sessions",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         id_ws = table.Column<string>(type: "text", nullable: false),
            //         status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         ressource_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("sessions_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "sessions_ressource_id_fk",
            //             column: x => x.ressource_id,
            //             principalTable: "ressources",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "event_member",
            //     columns: table => new
            //     {
            //         user_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         event_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("event_member_pk", x => new { x.user_id, x.event_id });
            //         table.ForeignKey(
            //             name: "event_member_event_id_fk",
            //             column: x => x.event_id,
            //             principalTable: "events",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "event_member_user_id_fk",
            //             column: x => x.user_id,
            //             principalTable: "users",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "polls_options",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         option = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         poll_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("polls_options_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "polls_options_poll_id_fk",
            //             column: x => x.poll_id,
            //             principalTable: "polls",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "quizzes_questions",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         question = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         possible_answers = table.Column<string>(type: "jsonb", nullable: false),
            //         correct_answer = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         quizz_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("quizzes_questions_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "quizzes_questions_quizz_id_fk",
            //             column: x => x.quizz_id,
            //             principalTable: "quizzes",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "sessions_messages",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         sent_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         content = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         user_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         session_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("sessions_messages_pk", x => x.id);
            //         table.ForeignKey(
            //             name: "sessions_messages_session_id_fk",
            //             column: x => x.session_id,
            //             principalTable: "sessions",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "sessions_messages_user_id_fk",
            //             column: x => x.user_id,
            //             principalTable: "users",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "poll_vote",
            //     columns: table => new
            //     {
            //         user_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         poll_option_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("poll_vote_pk", x => new { x.user_id, x.poll_option_id });
            //         table.ForeignKey(
            //             name: "poll_vote_poll_option_id_fk",
            //             column: x => x.poll_option_id,
            //             principalTable: "polls_options",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "poll_vote_user_id_fk",
            //             column: x => x.user_id,
            //             principalTable: "users",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "question_answer",
            //     columns: table => new
            //     {
            //         user_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         quizz_question_id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("question_answer_pk", x => new { x.user_id, x.quizz_question_id });
            //         table.ForeignKey(
            //             name: "question_answer_quizz_question_id_fk",
            //             column: x => x.quizz_question_id,
            //             principalTable: "quizzes_questions",
            //             principalColumn: "id");
            //         table.ForeignKey(
            //             name: "question_answer_user_id_fk",
            //             column: x => x.user_id,
            //             principalTable: "users",
            //             principalColumn: "id");
            //     });
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_addresses_city_id",
            //     table: "addresses",
            //     column: "city_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_addresses_department_id",
            //     table: "addresses",
            //     column: "department_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_addresses_region_id",
            //     table: "addresses",
            //     column: "region_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_addresses_user_id",
            //     table: "addresses",
            //     column: "user_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_articles_ressource_id",
            //     table: "articles",
            //     column: "ressource_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_backoffice_logs_backoffice_log_level_id",
            //     table: "backoffice_logs",
            //     column: "backoffice_log_level_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_backoffice_logs_backoffice_operation_type_id",
            //     table: "backoffice_logs",
            //     column: "backoffice_operation_type_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_comments_comment_id",
            //     table: "comments",
            //     column: "comment_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_comments_ressource_id",
            //     table: "comments",
            //     column: "ressource_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_comments_user_id",
            //     table: "comments",
            //     column: "user_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "code_idx",
            //     table: "departments",
            //     column: "code");
            //
            // migrationBuilder.CreateIndex(
            //     name: "region_code_idx",
            //     table: "departments",
            //     column: "region_code");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_event_member_event_id",
            //     table: "event_member",
            //     column: "event_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_events_ressource_id",
            //     table: "events",
            //     column: "ressource_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_friends_requests_user_receiver_id",
            //     table: "friends_requests",
            //     column: "user_receiver_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "email_idx",
            //     table: "logins",
            //     column: "email");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_logins_user_id",
            //     table: "logins",
            //     column: "user_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_notifications_user_id",
            //     table: "notifications",
            //     column: "user_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_passwords_history_password_infos_id",
            //     table: "passwords_history",
            //     column: "password_infos_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_passwords_infos_user_id",
            //     table: "passwords_infos",
            //     column: "user_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_poll_vote_poll_option_id",
            //     table: "poll_vote",
            //     column: "poll_option_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_polls_ressource_id",
            //     table: "polls",
            //     column: "ressource_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_polls_options_poll_id",
            //     table: "polls_options",
            //     column: "poll_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_profiles_pictures_user_id",
            //     table: "profiles_pictures",
            //     column: "user_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_question_answer_quizz_question_id",
            //     table: "question_answer",
            //     column: "quizz_question_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_quizzes_ressource_id",
            //     table: "quizzes",
            //     column: "ressource_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_quizzes_questions_quizz_id",
            //     table: "quizzes_questions",
            //     column: "quizz_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_refresh_tokens_user_id",
            //     table: "refresh_tokens",
            //     column: "user_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_reports_report_type_id",
            //     table: "reports",
            //     column: "report_type_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_reports_ressource_id",
            //     table: "reports",
            //     column: "ressource_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_reports_user_id",
            //     table: "reports",
            //     column: "user_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_ressource_favorite_ressource_id",
            //     table: "ressource_favorite",
            //     column: "ressource_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_ressource_like_ressource_id",
            //     table: "ressource_like",
            //     column: "ressource_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_ressource_progression_user_id",
            //     table: "ressource_progression",
            //     column: "user_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_ressource_tag_ressource_id",
            //     table: "ressource_tag",
            //     column: "ressource_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_ressources_ressource_confidentiality_type_id",
            //     table: "ressources",
            //     column: "ressource_confidentiality_type_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_ressources_ressource_status_id",
            //     table: "ressources",
            //     column: "ressource_status_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_ressources_ressource_type_id",
            //     table: "ressources",
            //     column: "ressource_type_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_ressources_user_id",
            //     table: "ressources",
            //     column: "user_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_ressources_medias_ressource_id",
            //     table: "ressources_medias",
            //     column: "ressource_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_sessions_ressource_id",
            //     table: "sessions",
            //     column: "ressource_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_sessions_messages_session_id",
            //     table: "sessions_messages",
            //     column: "session_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_sessions_messages_user_id",
            //     table: "sessions_messages",
            //     column: "user_id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_users_user_role_id",
            //     table: "users",
            //     column: "user_role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropTable(
            //     name: "addresses");
            //
            // migrationBuilder.DropTable(
            //     name: "articles");
            //
            // migrationBuilder.DropTable(
            //     name: "backoffice_logs");
            //
            // migrationBuilder.DropTable(
            //     name: "comments");
            //
            // migrationBuilder.DropTable(
            //     name: "email_logs");
            //
            // migrationBuilder.DropTable(
            //     name: "event_member");
            //
            // migrationBuilder.DropTable(
            //     name: "friends_requests");
            //
            // migrationBuilder.DropTable(
            //     name: "logins");
            //
            // migrationBuilder.DropTable(
            //     name: "notifications");
            //
            // migrationBuilder.DropTable(
            //     name: "passwords_history");
            //
            // migrationBuilder.DropTable(
            //     name: "poll_vote");
            //
            // migrationBuilder.DropTable(
            //     name: "profiles_pictures");
            //
            // migrationBuilder.DropTable(
            //     name: "question_answer");
            //
            // migrationBuilder.DropTable(
            //     name: "refresh_tokens");
            //
            // migrationBuilder.DropTable(
            //     name: "reports");
            //
            // migrationBuilder.DropTable(
            //     name: "ressource_favorite");
            //
            // migrationBuilder.DropTable(
            //     name: "ressource_like");
            //
            // migrationBuilder.DropTable(
            //     name: "ressource_progression");
            //
            // migrationBuilder.DropTable(
            //     name: "ressource_tag");
            //
            // migrationBuilder.DropTable(
            //     name: "ressources_medias");
            //
            // migrationBuilder.DropTable(
            //     name: "sessions_messages");
            //
            // migrationBuilder.DropTable(
            //     name: "cities");
            //
            // migrationBuilder.DropTable(
            //     name: "departments");
            //
            // migrationBuilder.DropTable(
            //     name: "regions");
            //
            // migrationBuilder.DropTable(
            //     name: "backoffice_log_levels");
            //
            // migrationBuilder.DropTable(
            //     name: "backoffice_operation_types");
            //
            // migrationBuilder.DropTable(
            //     name: "events");
            //
            // migrationBuilder.DropTable(
            //     name: "passwords_infos");
            //
            // migrationBuilder.DropTable(
            //     name: "polls_options");
            //
            // migrationBuilder.DropTable(
            //     name: "quizzes_questions");
            //
            // migrationBuilder.DropTable(
            //     name: "reports_types");
            //
            // migrationBuilder.DropTable(
            //     name: "tags");
            //
            // migrationBuilder.DropTable(
            //     name: "sessions");
            //
            // migrationBuilder.DropTable(
            //     name: "polls");
            //
            // migrationBuilder.DropTable(
            //     name: "quizzes");
            //
            // migrationBuilder.DropTable(
            //     name: "ressources");
            //
            // migrationBuilder.DropTable(
            //     name: "ressources_confidentiality_types");
            //
            // migrationBuilder.DropTable(
            //     name: "ressources_status");
            //
            // migrationBuilder.DropTable(
            //     name: "ressource_types");
            //
            // migrationBuilder.DropTable(
            //     name: "users");
            //
            // migrationBuilder.DropTable(
            //     name: "users_roles");
        }
    }
}
