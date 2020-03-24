using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseProvidor.Migrations
{
    public partial class InitializeSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    table.ForeignKey(
                        name: "FK_account_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    table.ForeignKey(
                        name: "FK_credit_card_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    comment = table.Column<string>(nullable: true),
                    account_id1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_income", x => x.id);
                    table.ForeignKey(
                        name: "FK_income_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_income_account_account_id1",
                        column: x => x.account_id1,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "move",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<long>(nullable: true),
                    start_date = table.Column<string>(nullable: true),
                    end_date = table.Column<string>(nullable: true),
                    pre_account_id = table.Column<long>(nullable: true),
                    next_account_id = table.Column<long>(nullable: true),
                    move_price = table.Column<long>(nullable: true),
                    comment = table.Column<string>(nullable: true),
                    pre_account_id1 = table.Column<long>(nullable: true),
                    next_account_id1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_move", x => x.id);
                    table.ForeignKey(
                        name: "FK_move_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_move_account_next_account_id1",
                        column: x => x.next_account_id1,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_move_account_pre_account_id1",
                        column: x => x.pre_account_id1,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                    comment = table.Column<string>(nullable: true),
                    account_id1 = table.Column<long>(nullable: true),
                    card_id1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment", x => x.id);
                    table.ForeignKey(
                        name: "FK_payment_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_payment_account_account_id1",
                        column: x => x.account_id1,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_payment_credit_card_card_id1",
                        column: x => x.card_id1,
                        principalTable: "credit_card",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_user_id",
                table: "account",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_credit_card_user_id",
                table: "credit_card",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_income_user_id",
                table: "income",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_income_account_id1",
                table: "income",
                column: "account_id1");

            migrationBuilder.CreateIndex(
                name: "IX_move_user_id",
                table: "move",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_move_next_account_id1",
                table: "move",
                column: "next_account_id1");

            migrationBuilder.CreateIndex(
                name: "IX_move_pre_account_id1",
                table: "move",
                column: "pre_account_id1");

            migrationBuilder.CreateIndex(
                name: "IX_payment_user_id",
                table: "payment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_account_id1",
                table: "payment",
                column: "account_id1");

            migrationBuilder.CreateIndex(
                name: "IX_payment_card_id1",
                table: "payment",
                column: "card_id1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "income");

            migrationBuilder.DropTable(
                name: "move");

            migrationBuilder.DropTable(
                name: "payment");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "credit_card");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
