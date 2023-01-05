using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Activities",
            columns: table =>
                new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(
                        type: "DATETIME",
                        nullable: false,
                        defaultValueSql: "getdate()"
                    ),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(
                        type: "DATETIME",
                        nullable: true,
                        defaultValueSql: "getdate()"
                    ),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
            constraints: table =>
            {
                table.PrimaryKey("PK_Activities", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table =>
                new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(
                        type: "DATETIME",
                        nullable: false,
                        defaultValueSql: "getdate()"
                    ),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(
                        type: "DATETIME",
                        nullable: true,
                        defaultValueSql: "getdate()"
                    ),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "UserActivities",
            columns: table =>
                new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ActivityId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Weight = table.Column<int>(type: "INTEGER", nullable: false),
                    Repetition = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(
                        type: "DATETIME",
                        nullable: false,
                        defaultValueSql: "getdate()"
                    ),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(
                        type: "DATETIME",
                        nullable: true,
                        defaultValueSql: "getdate()"
                    ),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserActivities", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserActivities_Activities_ActivityId",
                    column: x => x.ActivityId,
                    principalTable: "Activities",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
                table.ForeignKey(
                    name: "FK_UserActivities_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateIndex(
            name: "IX_UserActivities_ActivityId",
            table: "UserActivities",
            column: "ActivityId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_UserActivities_UserId",
            table: "UserActivities",
            column: "UserId"
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "UserActivities");

        migrationBuilder.DropTable(name: "Activities");

        migrationBuilder.DropTable(name: "Users");
    }
}
