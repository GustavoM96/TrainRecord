using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TeacherStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeacherStudent",
                columns: table =>
                    new
                    {
                        Id = table.Column<Guid>(type: "TEXT", nullable: false),
                        TeacherId = table.Column<Guid>(type: "TEXT", nullable: false),
                        StudentId = table.Column<Guid>(type: "TEXT", nullable: false),
                        CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                        CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                        LastModifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                        LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherStudent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherStudent_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_TeacherStudent_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "TeacherStudent");
        }
    }
}
