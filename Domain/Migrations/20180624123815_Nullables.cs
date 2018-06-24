using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class Nullables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionAnticipations_TransactionAnticipationID",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionAnticipationID",
                table: "Transactions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<bool>(
                name: "AnticipationResult",
                table: "TransactionAnticipations",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<DateTime>(
                name: "AnalysisDate",
                table: "TransactionAnticipations",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "TransactionAnticipations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionAnticipations_TransactionAnticipationID",
                table: "Transactions",
                column: "TransactionAnticipationID",
                principalTable: "TransactionAnticipations",
                principalColumn: "TransactionAnticipationID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionAnticipations_TransactionAnticipationID",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TransactionAnticipations");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionAnticipationID",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AnticipationResult",
                table: "TransactionAnticipations",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AnalysisDate",
                table: "TransactionAnticipations",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionAnticipations_TransactionAnticipationID",
                table: "Transactions",
                column: "TransactionAnticipationID",
                principalTable: "TransactionAnticipations",
                principalColumn: "TransactionAnticipationID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
