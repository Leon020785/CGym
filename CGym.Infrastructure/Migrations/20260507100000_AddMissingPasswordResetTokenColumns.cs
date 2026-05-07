using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingPasswordResetTokenColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"SET @exist := (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'Users' AND COLUMN_NAME = 'PasswordResetToken'); SET @sql := IF(@exist = 0, 'ALTER TABLE Users ADD COLUMN PasswordResetToken longtext CHARACTER SET utf8mb4 NULL', 'SELECT 1'); PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;");

            migrationBuilder.Sql(@"SET @exist := (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'Users' AND COLUMN_NAME = 'PasswordResetTokenExpiry'); SET @sql := IF(@exist = 0, 'ALTER TABLE Users ADD COLUMN PasswordResetTokenExpiry datetime(6) NULL', 'SELECT 1'); PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetTokenExpiry",
                table: "Users");
        }
    }
}
