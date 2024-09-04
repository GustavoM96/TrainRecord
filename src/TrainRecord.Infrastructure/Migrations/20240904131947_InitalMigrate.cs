using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitalMigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase().Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "Activities",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false),
                        Name = table.Column<string>(type: "varchar(255)", nullable: false),
                        CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                        CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                        LastModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                        LastModifiedBy = table.Column<string>(type: "longtext", nullable: true),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Activities", x => x.Id);
                    }
                )
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "Users",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false),
                        Email = table.Column<string>(type: "varchar(255)", nullable: false),
                        Password = table.Column<string>(type: "longtext", nullable: false),
                        FirstName = table.Column<string>(type: "longtext", nullable: false),
                        LastName = table.Column<string>(type: "longtext", nullable: false),
                        Role = table.Column<int>(type: "int", nullable: false),
                        CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                        CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                        LastModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                        LastModifiedBy = table.Column<string>(type: "longtext", nullable: true),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Users", x => x.Id);
                    }
                )
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "TeacherStudent",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false),
                        TeacherId = table.Column<Guid>(type: "char(36)", nullable: false),
                        StudentId = table.Column<Guid>(type: "char(36)", nullable: false),
                        CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                        CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                        LastModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                        LastModifiedBy = table.Column<string>(type: "longtext", nullable: true),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_TeacherStudent", x => x.Id);
                        table.ForeignKey(
                            name: "FK_TeacherStudent_Users_StudentId",
                            column: x => x.StudentId,
                            principalTable: "Users",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict
                        );
                        table.ForeignKey(
                            name: "FK_TeacherStudent_Users_TeacherId",
                            column: x => x.TeacherId,
                            principalTable: "Users",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict
                        );
                    }
                )
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "UserActivities",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "char(36)", nullable: false),
                        UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                        TeacherId = table.Column<Guid>(type: "char(36)", nullable: true),
                        ActivityId = table.Column<Guid>(type: "char(36)", nullable: false),
                        Weight = table.Column<int>(type: "int", nullable: false),
                        Repetition = table.Column<int>(type: "int", nullable: false),
                        Serie = table.Column<int>(type: "int", nullable: false),
                        Time = table.Column<TimeOnly>(type: "time", nullable: true),
                        TrainGroup = table.Column<string>(type: "longtext", nullable: true),
                        TrainName = table.Column<string>(type: "longtext", nullable: true),
                        CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                        CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                        LastModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                        LastModifiedBy = table.Column<string>(type: "longtext", nullable: true),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_UserActivities", x => x.Id);
                        table.ForeignKey(
                            name: "FK_UserActivities_Activities_ActivityId",
                            column: x => x.ActivityId,
                            principalTable: "Activities",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict
                        );
                        table.ForeignKey(
                            name: "FK_UserActivities_Users_UserId",
                            column: x => x.UserId,
                            principalTable: "Users",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict
                        );
                    }
                )
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_Name",
                table: "Activities",
                column: "Name",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_TeacherStudent_StudentId",
                table: "TeacherStudent",
                column: "StudentId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_TeacherStudent_TeacherId",
                table: "TeacherStudent",
                column: "TeacherId"
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

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "TeacherStudent");

            migrationBuilder.DropTable(name: "UserActivities");

            migrationBuilder.DropTable(name: "Activities");

            migrationBuilder.DropTable(name: "Users");
        }
    }
}
