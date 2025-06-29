using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class hashdata : Migration
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

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "CreatedAt", "Email", "FullName", "IsActive", "IsDeleted", "Password", "Phone", "Role", "UpdatedAt" },
                values: new object[] { new Guid("d5407a97-9267-4ff4-b01a-fc3de7ff29ac"), "123 Lê Lợi, Quận 1, TP.HCM", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@center.vn", "Nguyễn Thành Vinh", true, false, "IBvOJFjwClQTDGlcqNFlgxmzIgbUla3xdYR7V71KQVE=", "0911222333", "Center", new DateTime(2025, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Centers",
                columns: new[] { "Id", "Capacity", "CreatedAt", "CurrentPatientCount", "DirectorName", "Email", "EstablishedDate", "HotLine", "Image", "IsActive", "IsDeleted", "Location", "Name", "Notes", "Type", "UpdatedAt", "UserId" },
                values: new object[] { new Guid("6c187f68-4e52-4e3e-9a2b-b42976f347a2"), 120, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, "Nguyễn Thành Vinh", "contact@quitcenter.vn", new DateTime(2020, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "18001111", "https://sdmntprwestus.oaiusercontent.com/files/00000000-7d08-6230-9129-91414b5e179d/raw?se=2025-06-29T05%3A01%3A47Z&sp=r&sv=2024-08-04&sr=b&scid=f7e18e3f-756e-5b33-85e5-a5db4150eae2&skoid=61180a4f-34a9-42b7-b76d-9ca47d89946d&sktid=a48cca56-e6da-484e-a814-9c849652bcb3&skt=2025-06-28T20%3A11%3A59Z&ske=2025-06-29T20%3A11%3A59Z&sks=b&skv=2024-08-04&sig=UfMiXKvlWY90aTQgza5mAqazs1pjTpapXKj1ZXTpBnI%3D", true, false, "123 Lê Lợi, Quận 1, TP.HCM", "Trung tâm Cai nghiện Thuốc lá Quận 1", "Chuyên hỗ trợ tư vấn và điều trị nghiện thuốc lá bằng các liệu pháp tâm lý và y khoa.", "Public", new DateTime(2025, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("d5407a97-9267-4ff4-b01a-fc3de7ff29ac") });

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

            migrationBuilder.DeleteData(
                table: "Centers",
                keyColumn: "Id",
                keyValue: new Guid("6c187f68-4e52-4e3e-9a2b-b42976f347a2"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d5407a97-9267-4ff4-b01a-fc3de7ff29ac"));

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
