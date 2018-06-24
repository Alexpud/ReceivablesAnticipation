using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShopKeepers",
                columns: table => new
                {
                    ShopKeeperID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopKeepers", x => x.ShopKeeperID);
                });

            migrationBuilder.CreateTable(
                name: "TransactionAnticipations",
                columns: table => new
                {
                    TransactionAnticipationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SolicitationDate = table.Column<DateTime>(nullable: false),
                    AnalysisDate = table.Column<DateTime>(nullable: true),
                    AnticipationResult = table.Column<bool>(nullable: true),
                    TotalTransactionValue = table.Column<decimal>(nullable: false),
                    TotalPassThroughValue = table.Column<decimal>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ShopKeeperID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionAnticipations", x => x.TransactionAnticipationID);
                    table.ForeignKey(
                        name: "FK_TransactionAnticipations_ShopKeepers_ShopKeeperID",
                        column: x => x.ShopKeeperID,
                        principalTable: "ShopKeepers",
                        principalColumn: "ShopKeeperID",
                        onDelete: ReferentialAction.Cascade);
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
                    TransactionAnticipationID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_Transactions_TransactionAnticipations_TransactionAnticipationID",
                        column: x => x.TransactionAnticipationID,
                        principalTable: "TransactionAnticipations",
                        principalColumn: "TransactionAnticipationID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionAnticipations_ShopKeeperID",
                table: "TransactionAnticipations",
                column: "ShopKeeperID");

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

            migrationBuilder.DropTable(
                name: "ShopKeepers");
        }
    }
}
