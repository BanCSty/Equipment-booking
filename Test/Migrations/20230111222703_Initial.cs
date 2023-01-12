using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    idMain = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    AmountOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    NameOrder = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.idMain);
                });

            migrationBuilder.CreateTable(
                name: "serviceObjects",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_serviceObjects", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "idMain", "AmountOrder", "ID", "NameOrder" },
                values: new object[] { 1, 2, "0f8fad5b-d9cb-469f-a165-70867728950e", "Blade" });

            migrationBuilder.InsertData(
                table: "serviceObjects",
                columns: new[] { "ID", "Amount", "Name" },
                values: new object[] { "0f8fad5b-d9cb-469f-a165-70867728950e", 15, "Blade" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "serviceObjects");
        }
    }
}
