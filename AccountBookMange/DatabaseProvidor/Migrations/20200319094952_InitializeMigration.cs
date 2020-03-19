using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseProvidor.Migrations
{
    public partial class InitializeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<long>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    delete_flag = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "credit_card",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<long>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    delete_flag = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_card", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "income",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<long>(nullable: true),
                    income_date = table.Column<string>(nullable: true),
                    income_kind = table.Column<long>(nullable: true),
                    income_price = table.Column<long>(nullable: true),
                    account_id = table.Column<long>(nullable: true),
                    comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_income", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "move",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<long>(nullable: true),
                    start_date = table.Column<string>(nullable: true),
                    pre_account_id = table.Column<long>(nullable: true),
                    next_account_id = table.Column<long>(nullable: true),
                    move_price = table.Column<long>(nullable: true),
                    comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_move", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payment",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<long>(nullable: true),
                    payment_date = table.Column<string>(nullable: true),
                    payment_price = table.Column<long>(nullable: true),
                    payment_kind = table.Column<long>(nullable: true),
                    payment_way = table.Column<long>(nullable: true),
                    account_id = table.Column<long>(nullable: true),
                    card_id = table.Column<long>(nullable: true),
                    comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(nullable: true),
                    account = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true),
                    delete_flag = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "credit_card");

            migrationBuilder.DropTable(
                name: "income");

            migrationBuilder.DropTable(
                name: "move");

            migrationBuilder.DropTable(
                name: "payment");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
