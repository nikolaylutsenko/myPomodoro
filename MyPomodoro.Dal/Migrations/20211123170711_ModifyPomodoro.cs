using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPomodoro.Dal.Migrations
{
    public partial class ModifyPomodoro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Pomodoros",
                newName: "PomodoroDate");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "StartTime",
                table: "Pomodoros",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "EndTime",
                table: "Pomodoros",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PomodoroDate",
                table: "Pomodoros",
                newName: "Time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Pomodoros",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(TimeOnly),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "Pomodoros",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(TimeOnly),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
