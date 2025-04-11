using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECOM.Infrastructure.Persistence.Main.Migrations
{
    /// <inheritdoc />
    public partial class InitialMainDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationClaim",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ClaimValue = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationClaim", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    BucketName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt_Utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastUpdatedAt_Utc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LanguageComponent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ComponentName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageComponent_LanguageComponent_ParentId",
                        column: x => x.ParentId,
                        principalTable: "LanguageComponent",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationRoleClaim",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRoleClaim", x => new { x.RoleId, x.ClaimId });
                    table.ForeignKey(
                        name: "FK_ApplicationRoleClaim_ApplicationClaim_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "ApplicationClaim",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationRoleClaim_ApplicationRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ApplicationRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NormalizedFullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    SecretKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TimeZoneId = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Currency = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false),
                    Language = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: true),
                    VerifyFailedCount = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    LockedReason = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LockoutEndDate_Utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvatarId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt_Utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastUpdatedAt_Utc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUser_FileEntity_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "FileEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AvatarId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Language_FileEntity_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "FileEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "LanguageKey",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageKey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageKey_LanguageComponent_LanguageComponentId",
                        column: x => x.LanguageComponentId,
                        principalTable: "LanguageComponent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserClaim",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserClaim", x => new { x.UserId, x.ClaimId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserClaim_ApplicationClaim_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "ApplicationClaim",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserClaim_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserLogin",
                columns: table => new
                {
                    Provider = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserLogin", x => new { x.Provider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_ApplicationUserLogin_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserRole",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserRole_ApplicationRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ApplicationRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserRole_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserToken",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TokenName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TokenValue = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    TokenExpiredAt_Utc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserToken", x => new { x.UserId, x.Provider, x.TokenName });
                    table.ForeignKey(
                        name: "FK_ApplicationUserToken_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageTranslationEntity",
                columns: table => new
                {
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageTranslationEntity", x => new { x.LanguageId, x.EntityName, x.EntityId, x.FieldName });
                    table.ForeignKey(
                        name: "FK_LanguageTranslationEntity_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageTranslation",
                columns: table => new
                {
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageKeyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageTranslation", x => new { x.LanguageId, x.LanguageKeyId });
                    table.ForeignKey(
                        name: "FK_LanguageTranslation_LanguageKey_LanguageKeyId",
                        column: x => x.LanguageKeyId,
                        principalTable: "LanguageKey",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageTranslation_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoleClaim_ClaimId",
                table: "ApplicationRoleClaim",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_AvatarId",
                table: "ApplicationUser",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserClaim_ClaimId",
                table: "ApplicationUserClaim",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserLogin_UserId",
                table: "ApplicationUserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserRole_RoleId",
                table: "ApplicationUserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Language_AvatarId",
                table: "Language",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_Language_Code",
                table: "Language",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LanguageComponent_ComponentName",
                table: "LanguageComponent",
                column: "ComponentName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LanguageComponent_ParentId",
                table: "LanguageComponent",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageKey_LanguageComponentId",
                table: "LanguageKey",
                column: "LanguageComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageTranslation_LanguageKeyId",
                table: "LanguageTranslation",
                column: "LanguageKeyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationRoleClaim");

            migrationBuilder.DropTable(
                name: "ApplicationUserClaim");

            migrationBuilder.DropTable(
                name: "ApplicationUserLogin");

            migrationBuilder.DropTable(
                name: "ApplicationUserRole");

            migrationBuilder.DropTable(
                name: "ApplicationUserToken");

            migrationBuilder.DropTable(
                name: "LanguageTranslation");

            migrationBuilder.DropTable(
                name: "LanguageTranslationEntity");

            migrationBuilder.DropTable(
                name: "ApplicationClaim");

            migrationBuilder.DropTable(
                name: "ApplicationRole");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropTable(
                name: "LanguageKey");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "LanguageComponent");

            migrationBuilder.DropTable(
                name: "FileEntity");
        }
    }
}
