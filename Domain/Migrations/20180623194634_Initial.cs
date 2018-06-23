using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionAnticipations",
                columns: table => new
                {
                    TransactionAnticipationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SolicitationDate = table.Column<DateTime>(nullable: false),
                    AnalysisDate = table.Column<DateTime>(nullable: false),
                    AnticipationResult = table.Column<bool>(nullable: false),
                    TotalTransactionValue = table.Column<decimal>(nullable: false),
                    TotalPassThroughValue = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionAnticipations", x => x.TransactionAnticipationID);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    PassThroughDate = table.Column<DateTime>(nullable: false),
                    FlConfirmation = table.Column<bool>(nullable: false),
                    TransactionValue = table.Column<decimal>(nullable: false),
                    PassThroughValue = table.Column<decimal>(nullable: false),
                    InstalmentQuantity = table.Column<int>(nullable: false),
                    TransactionAnticipationID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_Transactions_TransactionAnticipations_TransactionAnticipationID",
                        column: x => x.TransactionAnticipationID,
                        principalTable: "TransactionAnticipations",
                        principalColumn: "TransactionAnticipationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionAnticipationID",
                table: "Transactions",
                column: "TransactionAnticipationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "TransactionAnticipations");
        }
    }
}
