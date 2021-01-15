using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SiteManager.V4.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppLog",
                columns: table => new
                {
                    AppLogId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LogDateTime = table.Column<DateTime>(type: "timestamp(7)", nullable: false),
                    LogLevel = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<string>(type: "text", nullable: true),
                    EventId = table.Column<string>(type: "text", nullable: true),
                    StateInfo = table.Column<string>(type: "text", nullable: true),
                    LogMessage = table.Column<string>(type: "text", nullable: true),
                    LogException = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "varchar(255)", nullable: true),
                    JsonData = table.Column<string>(type: "json", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLog", x => x.AppLogId);
                });

            migrationBuilder.CreateTable(
                name: "AppPermissionType",
                columns: table => new
                {
                    AppPermissionTypeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    PermissionType = table.Column<string>(type: "varchar(25)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPermissionType", x => x.AppPermissionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AppResource",
                columns: table => new
                {
                    AppResourceId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    ResourceRoute = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppResource", x => x.AppResourceId);
                });

            migrationBuilder.CreateTable(
                name: "AppRole",
                columns: table => new
                {
                    AppRoleId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRole", x => x.AppRoleId);
                });

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    AppUserId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.AppUserId);
                });

            migrationBuilder.CreateTable(
                name: "DeviceCodes",
                columns: table => new
                {
                    UserCode = table.Column<string>(maxLength: 200, nullable: false),
                    DeviceCode = table.Column<string>(maxLength: 200, nullable: false),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: false),
                    Data = table.Column<string>(maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceCodes", x => x.UserCode);
                });

            migrationBuilder.CreateTable(
                name: "PersistedGrants",
                columns: table => new
                {
                    Key = table.Column<string>(maxLength: 200, nullable: false),
                    Type = table.Column<string>(maxLength: 50, nullable: false),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: true),
                    Data = table.Column<string>(maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedGrants", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "TodoLists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    Colour = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRoleResource",
                columns: table => new
                {
                    AppRoleResourceId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    AppRoleId = table.Column<int>(type: "int", nullable: false),
                    AppResourceId = table.Column<int>(type: "int", nullable: false),
                    AppPermissionTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleResource", x => x.AppRoleResourceId);
                    table.ForeignKey(
                        name: "AppResource_AppPermissionType_FK",
                        column: x => x.AppPermissionTypeId,
                        principalTable: "AppPermissionType",
                        principalColumn: "AppPermissionTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "AppResource_AppRoleResouce_FK",
                        column: x => x.AppResourceId,
                        principalTable: "AppResource",
                        principalColumn: "AppResourceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppRoleClaim",
                columns: table => new
                {
                    AppRoleClaimId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppRoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleClaim", x => x.AppRoleClaimId);
                    table.ForeignKey(
                        name: "FK_AppRoleClaim_AppRole_AppRoleId",
                        column: x => x.AppRoleId,
                        principalTable: "AppRole",
                        principalColumn: "AppRoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserClaim",
                columns: table => new
                {
                    AppUserClaimId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppUserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserClaim", x => x.AppUserClaimId);
                    table.ForeignKey(
                        name: "FK_AppUserClaim_AppUser_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    AppUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AppUserLogin_AppUser_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRole",
                columns: table => new
                {
                    AppUserId = table.Column<int>(nullable: false),
                    AppRoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRole", x => new { x.AppUserId, x.AppRoleId });
                    table.ForeignKey(
                        name: "FK_AppUserRole_AppRole_AppRoleId",
                        column: x => x.AppRoleId,
                        principalTable: "AppRole",
                        principalColumn: "AppRoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserRole_AppUser_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserToken",
                columns: table => new
                {
                    AppUserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserToken", x => new { x.AppUserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AppUserToken_AppUser_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TodoItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    ListId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    Note = table.Column<string>(nullable: true),
                    Done = table.Column<bool>(nullable: false),
                    Reminder = table.Column<DateTime>(nullable: true),
                    Priority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoItems_TodoLists_ListId",
                        column: x => x.ListId,
                        principalTable: "TodoLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AppRole",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppRoleClaim_AppRoleId",
                table: "AppRoleClaim",
                column: "AppRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppRoleResource_AppPermissionTypeId",
                table: "AppRoleResource",
                column: "AppPermissionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppRoleResource_AppResourceId",
                table: "AppRoleResource",
                column: "AppResourceId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AppUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AppUser",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserClaim_AppUserId",
                table: "AppUserClaim",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserLogin_AppUserId",
                table: "AppUserLogin",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRole_AppRoleId",
                table: "AppUserRole",
                column: "AppRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCodes_DeviceCode",
                table: "DeviceCodes",
                column: "DeviceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCodes_Expiration",
                table: "DeviceCodes",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_Expiration",
                table: "PersistedGrants",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_SubjectId_ClientId_Type",
                table: "PersistedGrants",
                columns: new[] { "SubjectId", "ClientId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_ListId",
                table: "TodoItems",
                column: "ListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppLog");

            migrationBuilder.DropTable(
                name: "AppRoleClaim");

            migrationBuilder.DropTable(
                name: "AppRoleResource");

            migrationBuilder.DropTable(
                name: "AppUserClaim");

            migrationBuilder.DropTable(
                name: "AppUserLogin");

            migrationBuilder.DropTable(
                name: "AppUserRole");

            migrationBuilder.DropTable(
                name: "AppUserToken");

            migrationBuilder.DropTable(
                name: "DeviceCodes");

            migrationBuilder.DropTable(
                name: "PersistedGrants");

            migrationBuilder.DropTable(
                name: "TodoItems");

            migrationBuilder.DropTable(
                name: "AppPermissionType");

            migrationBuilder.DropTable(
                name: "AppResource");

            migrationBuilder.DropTable(
                name: "AppRole");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropTable(
                name: "TodoLists");
        }
    }
}
