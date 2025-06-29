using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionWithoutIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogImages_Blogs_BlogId",
                table: "BlogImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Users_UserId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Courses_CourseId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_CenterImages_Centers_CenterId",
                table: "CenterImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Centers_Users_UserId",
                table: "Centers");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Centers_CenterId",
                table: "Courses");

            migrationBuilder.CreateTable(
         name: "Transaction",
         columns: table => new
         {
             Id = table.Column<long>(type: "bigint", nullable: false), // ❗ Không có Identity
             CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
             Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
             BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
         },
         constraints: table =>
         {
             table.PrimaryKey("PK_Transaction", x => x.Id);
             table.ForeignKey(
                 name: "FK_Transaction_Bookings_BookingId",
                 column: x => x.BookingId,
                 principalTable: "Bookings",
                 principalColumn: "Id",
                 onDelete: ReferentialAction.NoAction);
         });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BookingId",
                table: "Transaction",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogImages_Blogs_BlogId",
                table: "BlogImages",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Users_UserId",
                table: "Blogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Courses_CourseId",
                table: "Bookings",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_CenterImages_Centers_CenterId",
                table: "CenterImages",
                column: "CenterId",
                principalTable: "Centers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Centers_Users_UserId",
                table: "Centers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Centers_CenterId",
                table: "Courses",
                column: "CenterId",
                principalTable: "Centers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogImages_Blogs_BlogId",
                table: "BlogImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Users_UserId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Courses_CourseId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_CenterImages_Centers_CenterId",
                table: "CenterImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Centers_Users_UserId",
                table: "Centers");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Centers_CenterId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogImages_Blogs_BlogId",
                table: "BlogImages",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Users_UserId",
                table: "Blogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Courses_CourseId",
                table: "Bookings",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CenterImages_Centers_CenterId",
                table: "CenterImages",
                column: "CenterId",
                principalTable: "Centers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Centers_Users_UserId",
                table: "Centers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Centers_CenterId",
                table: "Courses",
                column: "CenterId",
                principalTable: "Centers",
                principalColumn: "Id");
        }
    }
}
