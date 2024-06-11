using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceElectronicQueue.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    IdOrganization = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    UniqueKey = table.Column<string>(type: "text", nullable: true),
                    Logo = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.IdOrganization);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRole = table.Column<Guid>(type: "uuid", nullable: false),
                    Amplua = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRole);
                });

            migrationBuilder.CreateTable(
                name: "BranchOffices",
                columns: table => new
                {
                    IdBranchOffice = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Addres = table.Column<string>(type: "text", nullable: false),
                    UniqueLink = table.Column<string>(type: "text", nullable: true),
                    IdOrganization = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchOffices", x => x.IdBranchOffice);
                    table.ForeignKey(
                        name: "FK_BranchOffices_Organizations_IdOrganization",
                        column: x => x.IdOrganization,
                        principalTable: "Organizations",
                        principalColumn: "IdOrganization",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdUser = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Patronymic = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    IdOrganization = table.Column<Guid>(type: "uuid", nullable: false),
                    IdRole = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IdUser);
                    table.ForeignKey(
                        name: "FK_Users_Organizations_IdOrganization",
                        column: x => x.IdOrganization,
                        principalTable: "Organizations",
                        principalColumn: "IdOrganization",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Roles_IdRole",
                        column: x => x.IdRole,
                        principalTable: "Roles",
                        principalColumn: "IdRole",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    IdServices = table.Column<Guid>(type: "uuid", nullable: false),
                    NumberService = table.Column<int>(type: "integer", nullable: false),
                    Service = table.Column<string>(type: "text", nullable: false),
                    IdBranchOffice = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.IdServices);
                    table.ForeignKey(
                        name: "FK_Services_BranchOffices_IdBranchOffice",
                        column: x => x.IdBranchOffice,
                        principalTable: "BranchOffices",
                        principalColumn: "IdBranchOffice",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElectronicQueues",
                columns: table => new
                {
                    IdElectronicQueue = table.Column<Guid>(type: "uuid", nullable: false),
                    NumberInQueue = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    DateAndTimeStatus = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdServices = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectronicQueues", x => x.IdElectronicQueue);
                    table.ForeignKey(
                        name: "FK_ElectronicQueues_Services_IdServices",
                        column: x => x.IdServices,
                        principalTable: "Services",
                        principalColumn: "IdServices",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchOffices_IdOrganization",
                table: "BranchOffices",
                column: "IdOrganization");

            migrationBuilder.CreateIndex(
                name: "IX_ElectronicQueues_IdServices",
                table: "ElectronicQueues",
                column: "IdServices");

            migrationBuilder.CreateIndex(
                name: "IX_Services_IdBranchOffice",
                table: "Services",
                column: "IdBranchOffice");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdOrganization",
                table: "Users",
                column: "IdOrganization");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdRole",
                table: "Users",
                column: "IdRole");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElectronicQueues");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "BranchOffices");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
