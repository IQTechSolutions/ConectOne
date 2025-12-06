using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KiburiOnline.Blazor.ServerClient.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.CreateTable(
                name: "Amenity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IconClass = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditTrails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffectedColumns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookingTermsDescriptionTemplate",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingTermsDescriptionTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CancellationTermsTemplate",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancellationTermsTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category<Lodging>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Featured = table.Column<bool>(type: "bit", nullable: false),
                    WebTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayCategoryInMainManu = table.Column<bool>(type: "bit", nullable: false),
                    DisplayAsSliderItem = table.Column<bool>(type: "bit", nullable: false),
                    Slogan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubSlogan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentCategoryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category<Lodging>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category<Lodging>_Category<Lodging>_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Category<Lodging>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Category<OfferedService>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Featured = table.Column<bool>(type: "bit", nullable: false),
                    WebTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayCategoryInMainManu = table.Column<bool>(type: "bit", nullable: false),
                    DisplayAsSliderItem = table.Column<bool>(type: "bit", nullable: false),
                    Slogan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubSlogan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentCategoryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category<OfferedService>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category<OfferedService>_Category<OfferedService>_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Category<OfferedService>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Category<Product>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Featured = table.Column<bool>(type: "bit", nullable: false),
                    WebTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayCategoryInMainManu = table.Column<bool>(type: "bit", nullable: false),
                    DisplayAsSliderItem = table.Column<bool>(type: "bit", nullable: false),
                    Slogan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubSlogan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentCategoryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category<Product>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category<Product>_Category<Product>_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Category<Product>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactType = table.Column<int>(type: "int", nullable: false),
                    Featured = table.Column<bool>(type: "bit", nullable: false),
                    Selector = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomVariableTag",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomVariableTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DayTourActivityTemplate",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    GuestType = table.Column<int>(type: "int", nullable: false),
                    DisplayInOverview = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayTourActivityTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Destination",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    OnlineDescription = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lat = table.Column<double>(type: "float", nullable: false),
                    Lng = table.Column<double>(type: "float", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destination", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceToken",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceTokenContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    RelativePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralInformationTemplate",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Information = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralInformationTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gift",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    GuestType = table.Column<int>(type: "int", nullable: false),
                    TimeDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    GiftType = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gift", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GolfCourse",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    OnlineDescription = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lat = table.Column<double>(type: "float", nullable: false),
                    Lng = table.Column<double>(type: "float", nullable: false),
                    CourseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Carts = table.Column<bool>(type: "bit", nullable: false),
                    Caddies = table.Column<bool>(type: "bit", nullable: false),
                    GolfClubs = table.Column<bool>(type: "bit", nullable: false),
                    Halfway = table.Column<bool>(type: "bit", nullable: false),
                    DesignedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ranking = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GolfCourse", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Featured = table.Column<bool>(type: "bit", nullable: false),
                    ImageType = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    RelativePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LodgingListingRequest",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LodgingListingRequestId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lat = table.Column<double>(type: "float", nullable: false),
                    Lng = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LodgingListingRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LodgingType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LodgingType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfferedService",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    BillingFrequency = table.Column<int>(type: "int", nullable: false),
                    ServiceFrequency = table.Column<int>(type: "int", nullable: false),
                    PriceTableItem = table.Column<bool>(type: "bit", nullable: false),
                    Featured = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    DoNotDisplayInCatalogs = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferedService", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubTotalExcl = table.Column<double>(type: "float", nullable: false),
                    Vat = table.Column<double>(type: "float", nullable: false),
                    SubTotalIncl = table.Column<double>(type: "float", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    TotalDue = table.Column<double>(type: "float", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentExclusionTemplate",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentExclusionTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SKU = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    ShopOwnerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    Featured = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VariantParentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Product_VariantParentId",
                        column: x => x.VariantParentId,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Restaurant",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShortDescriptionTemplate",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortDescriptionTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TermsAndConditionsTemplate",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermsAndConditionsTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotAvailableForRegistrationSelection = table.Column<bool>(type: "bit", nullable: false),
                    AdministrativeRole = table.Column<bool>(type: "bit", nullable: false),
                    CannotDelete = table.Column<bool>(type: "bit", nullable: false),
                    AdvertiseRegistration = table.Column<bool>(type: "bit", nullable: false),
                    AdvertiseOnlyToMembers = table.Column<bool>(type: "bit", nullable: false),
                    ProductManager = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ParentRoleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_UserRoles_ParentRoleId",
                        column: x => x.ParentRoleId,
                        principalSchema: "Identity",
                        principalTable: "UserRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PrivacyAndUsageTermsAcceptedTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Licenses = table.Column<int>(type: "int", nullable: false),
                    IsConnected = table.Column<bool>(type: "bit", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RegistrationStatus = table.Column<int>(type: "int", nullable: false),
                    ReasonForRejection = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VacationContactUsInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationContactUsInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VacationTitleTemplate",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VacationTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationTitleTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Video",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    RelativePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Video", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AverageTemperature",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Month = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AvgHigh = table.Column<double>(type: "float", nullable: false),
                    AvgLow = table.Column<double>(type: "float", nullable: false),
                    AreaId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AverageTemperature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AverageTemperature_Area_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Area",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VacationHost",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Suburb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lat = table.Column<double>(type: "float", nullable: false),
                    Lng = table.Column<double>(type: "float", nullable: false),
                    Zoom = table.Column<int>(type: "int", nullable: false),
                    AreaId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RepresentativeTitle = table.Column<int>(type: "int", nullable: false),
                    RepresentativeName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepresentativeSurname2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepresentativePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepresentativeEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepresentativeBio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationHost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationHost_Area_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Area",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MeetAndGreetTemplate",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetAndGreetTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeetAndGreetTemplate_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Category<Lodging>, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityDocument<Category<Lodging>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Category<Lodging>, string>_Category<Lodging>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<Lodging>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Category<Lodging>, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Category<OfferedService>, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityDocument<Category<OfferedService>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Category<OfferedService>, string>_Category<OfferedService>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<OfferedService>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Category<OfferedService>, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Category<Product>, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityDocument<Category<Product>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Category<Product>, string>_Category<Product>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<Product>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Category<Product>, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Contact, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityDocument<Contact, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Contact, string>_Contact_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Contact",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Contact, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Destination, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityDocument<Destination, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Destination, string>_Destination_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Destination",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Destination, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Gift, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityDocument<Gift, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Gift, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Gift, string>_Gift_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Gift",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<GolfCourse, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityDocument<GolfCourse, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<GolfCourse, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<GolfCourse, string>_GolfCourse_EntityId",
                        column: x => x.EntityId,
                        principalTable: "GolfCourse",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GolfCourseDestination",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GolfCourseId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DestinationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GolfCourseDestination", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GolfCourseDestination_Destination_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destination",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GolfCourseDestination_GolfCourse_GolfCourseId",
                        column: x => x.GolfCourseId,
                        principalTable: "GolfCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Category<Lodging>, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Selector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityImage<Category<Lodging>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Category<Lodging>, string>_Category<Lodging>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<Lodging>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Category<Lodging>, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Category<OfferedService>, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Selector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityImage<Category<OfferedService>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Category<OfferedService>, string>_Category<OfferedService>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<OfferedService>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Category<OfferedService>, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Category<Product>, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Selector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityImage<Category<Product>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Category<Product>, string>_Category<Product>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<Product>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Category<Product>, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Contact, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Selector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityImage<Contact, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Contact, string>_Contact_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityImage<Contact, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Destination, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Selector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityImage<Destination, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Destination, string>_Destination_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Destination",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityImage<Destination, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Gift, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Selector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityImage<Gift, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Gift, string>_Gift_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Gift",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Gift, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<GolfCourse, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Selector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityImage<GolfCourse, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<GolfCourse, string>_GolfCourse_EntityId",
                        column: x => x.EntityId,
                        principalTable: "GolfCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityImage<GolfCourse, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Lodging",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UniquePartnerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnlineDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoomInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Teaser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facilities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attractions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Grading = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepositPolicy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChildPolicy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LowestGuestAgeCutOff = table.Column<int>(type: "int", nullable: false),
                    MiddleGuestAgeCutOff = table.Column<int>(type: "int", nullable: false),
                    HighestGuestAgeCutOff = table.Column<int>(type: "int", nullable: false),
                    DefaultRateScheme = table.Column<int>(type: "int", nullable: false),
                    DefaultCommissionPercentage = table.Column<double>(type: "float", nullable: false),
                    DefaultMarkupPercentage = table.Column<double>(type: "float", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    Contacts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CellNr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaKeys = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeteDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreaInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Suburb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lat = table.Column<double>(type: "float", nullable: false),
                    Lng = table.Column<double>(type: "float", nullable: false),
                    Zoom = table.Column<int>(type: "int", nullable: false),
                    Directions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProvinceId = table.Column<int>(type: "int", nullable: true),
                    SingleRoomType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SingleRoomMealPlanType = table.Column<int>(type: "int", nullable: true),
                    DoubleRoomType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoubleRoomMealPlanType = table.Column<int>(type: "int", nullable: true),
                    TwinRoomType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwinRoomMealPlanType = table.Column<int>(type: "int", nullable: true),
                    CheckInTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LodgingTypeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CountryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lodging", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lodging_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lodging_LodgingType_LodgingTypeId",
                        column: x => x.LodgingTypeId,
                        principalTable: "LodgingType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityCategory<OfferedService>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityCategory<OfferedService>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityCategory<OfferedService>_Category<OfferedService>_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category<OfferedService>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityCategory<OfferedService>_OfferedService_EntityId",
                        column: x => x.EntityId,
                        principalTable: "OfferedService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<OfferedService, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityDocument<OfferedService, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<OfferedService, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<OfferedService, string>_OfferedService_EntityId",
                        column: x => x.EntityId,
                        principalTable: "OfferedService",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<OfferedService, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Selector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityImage<OfferedService, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<OfferedService, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<OfferedService, string>_OfferedService_EntityId",
                        column: x => x.EntityId,
                        principalTable: "OfferedService",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityCategory<Product>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityCategory<Product>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityCategory<Product>_Category<Product>_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category<Product>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityCategory<Product>_Product_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Product, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityDocument<Product, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Product, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Product, string>_Product_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Product, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Selector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityImage<Product, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Product, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Product, string>_Product_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Price",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiscountEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CostExcl = table.Column<double>(type: "float", nullable: false),
                    ShippingAmount = table.Column<double>(type: "float", nullable: false),
                    ContactForPrice = table.Column<bool>(type: "bit", nullable: false),
                    Vatable = table.Column<bool>(type: "bit", nullable: false),
                    DiscountPercentage = table.Column<double>(type: "float", nullable: false),
                    SellingPrice = table.Column<double>(type: "float", nullable: false),
                    RewardPoints = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Price", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Price_Product_Id",
                        column: x => x.Id,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductMetadata",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMetadata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductMetadata_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealAdditionTemplate",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GuestType = table.Column<int>(type: "int", nullable: false),
                    MealType = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    RestaurantId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealAdditionTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealAdditionTemplate_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_UserRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "UserRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceTier",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Default = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceTier_UserRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserPasskeys",
                columns: table => new
                {
                    CredentialId = table.Column<byte[]>(type: "varbinary(1024)", maxLength: 1024, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserPasskeys", x => x.CredentialId);
                    table.ForeignKey(
                        name: "FK_AspNetUserPasskeys_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_UserRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Follower",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserFollowingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserBeingFollowedId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follower", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Follower_Users_UserFollowingId",
                        column: x => x.UserFollowingId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UniqueUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityNr = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    MaritalStatus = table.Column<int>(type: "int", nullable: false),
                    MoodStatus = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VatNr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInfo_Users_Id",
                        column: x => x.Id,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Category<Lodging>, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityVideo<Category<Lodging>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Category<Lodging>, string>_Category<Lodging>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<Lodging>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Category<Lodging>, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Category<OfferedService>, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityVideo<Category<OfferedService>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Category<OfferedService>, string>_Category<OfferedService>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<OfferedService>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Category<OfferedService>, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Category<Product>, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityVideo<Category<Product>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Category<Product>, string>_Category<Product>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<Product>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Category<Product>, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Contact, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityVideo<Contact, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Contact, string>_Contact_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Contact",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Contact, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Destination, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityVideo<Destination, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Destination, string>_Destination_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Destination",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Destination, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Gift, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityVideo<Gift, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Gift, string>_Gift_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Gift",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Gift, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<GolfCourse, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityVideo<GolfCourse, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<GolfCourse, string>_GolfCourse_EntityId",
                        column: x => x.EntityId,
                        principalTable: "GolfCourse",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<GolfCourse, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<OfferedService, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityVideo<OfferedService, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<OfferedService, string>_OfferedService_EntityId",
                        column: x => x.EntityId,
                        principalTable: "OfferedService",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<OfferedService, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Product, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityVideo<Product, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Product, string>_Product_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Product, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<VacationHost, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityDocument<VacationHost, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<VacationHost, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<VacationHost, string>_VacationHost_EntityId",
                        column: x => x.EntityId,
                        principalTable: "VacationHost",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<VacationHost, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Selector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityImage<VacationHost, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<VacationHost, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<VacationHost, string>_VacationHost_EntityId",
                        column: x => x.EntityId,
                        principalTable: "VacationHost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<VacationHost, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityVideo<VacationHost, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<VacationHost, string>_VacationHost_EntityId",
                        column: x => x.EntityId,
                        principalTable: "VacationHost",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<VacationHost, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Vacation",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReferenceNr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvailabilityCutOffDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Nights = table.Column<int>(type: "int", nullable: false),
                    RoomCount = table.Column<int>(type: "int", nullable: false),
                    RoomCountFoc = table.Column<int>(type: "int", nullable: false),
                    CurrencySymbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxBookingCount = table.Column<int>(type: "int", nullable: false),
                    GeneralInclusionsSummaryInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MealsAndActivitiesInclusionsSummaryInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransportAndFlightsInclusionsSummaryInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GolfRoundSummaryInclusionInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccommodationInclusionSummaryInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsExtension = table.Column<bool>(type: "bit", nullable: false),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    MeetAndGreetTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    MeetAndGreetTemplateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VacationTitleTemplateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BookingTermsTemplateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CancellationTermsTemplateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TermsAndConditionsTemplateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PaymentExclusionTemplateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ShortDescriptionTemplateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    GeneralInformationTemplateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VacationHostId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vacation_BookingTermsDescriptionTemplate_BookingTermsTemplateId",
                        column: x => x.BookingTermsTemplateId,
                        principalTable: "BookingTermsDescriptionTemplate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vacation_CancellationTermsTemplate_CancellationTermsTemplateId",
                        column: x => x.CancellationTermsTemplateId,
                        principalTable: "CancellationTermsTemplate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vacation_GeneralInformationTemplate_GeneralInformationTemplateId",
                        column: x => x.GeneralInformationTemplateId,
                        principalTable: "GeneralInformationTemplate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vacation_MeetAndGreetTemplate_MeetAndGreetTemplateId",
                        column: x => x.MeetAndGreetTemplateId,
                        principalTable: "MeetAndGreetTemplate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vacation_PaymentExclusionTemplate_PaymentExclusionTemplateId",
                        column: x => x.PaymentExclusionTemplateId,
                        principalTable: "PaymentExclusionTemplate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vacation_ShortDescriptionTemplate_ShortDescriptionTemplateId",
                        column: x => x.ShortDescriptionTemplateId,
                        principalTable: "ShortDescriptionTemplate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vacation_TermsAndConditionsTemplate_TermsAndConditionsTemplateId",
                        column: x => x.TermsAndConditionsTemplateId,
                        principalTable: "TermsAndConditionsTemplate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vacation_VacationHost_VacationHostId",
                        column: x => x.VacationHostId,
                        principalTable: "VacationHost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vacation_VacationTitleTemplate_VacationTitleTemplateId",
                        column: x => x.VacationTitleTemplateId,
                        principalTable: "VacationTitleTemplate",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Airport",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Airport_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CancellationRule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DaysBeforeBookingThatCancellationIsAvailable = table.Column<int>(type: "int", nullable: false),
                    CancellationFormualaType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CancellationFormualaValue = table.Column<double>(type: "float", nullable: false),
                    LodgingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancellationRule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CancellationRule_Lodging_LodgingId",
                        column: x => x.LodgingId,
                        principalTable: "Lodging",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityCategory<Lodging>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityCategory<Lodging>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityCategory<Lodging>_Category<Lodging>_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category<Lodging>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityCategory<Lodging>_Lodging_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Lodging",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Lodging, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityDocument<Lodging, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Lodging, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Lodging, string>_Lodging_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Lodging",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Lodging, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Selector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityImage<Lodging, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Lodging, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Lodging, string>_Lodging_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Lodging",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Lodging, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityVideo<Lodging, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Lodging, string>_Lodging_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Lodging",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Lodging, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LodgingDestination",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LodgingId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DestinationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LodgingId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LodgingDestination", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LodgingDestination_Destination_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destination",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LodgingDestination_Lodging_LodgingId",
                        column: x => x.LodgingId,
                        principalTable: "Lodging",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LodgingDestination_Lodging_LodgingId1",
                        column: x => x.LodgingId1,
                        principalTable: "Lodging",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LodgingPackage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    AvailablePartnerUid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialRateId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LodgingId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LodgingPackage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LodgingPackage_Lodging_LodgingId",
                        column: x => x.LodgingId,
                        principalTable: "Lodging",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LodgingSettings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApiPartner = table.Column<int>(type: "int", nullable: true),
                    UniquePartnerId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Featured = table.Column<bool>(type: "bit", nullable: false),
                    AllowBookings = table.Column<bool>(type: "bit", nullable: false),
                    AllowLiveBookings = table.Column<bool>(type: "bit", nullable: false),
                    MinAdvanceBookingDays = table.Column<int>(type: "int", nullable: false),
                    AllowSameDay = table.Column<bool>(type: "bit", nullable: false),
                    CutOffTime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    OneNightStayRefundable = table.Column<bool>(type: "bit", nullable: false),
                    ShowCellPhoneNumber = table.Column<bool>(type: "bit", nullable: false),
                    AllowSmoking = table.Column<bool>(type: "bit", nullable: false),
                    AllowMultipleMealPlans = table.Column<bool>(type: "bit", nullable: false),
                    VatRegistered = table.Column<bool>(type: "bit", nullable: false),
                    VatNr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CheckInTime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CheckoutTime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AllowPets = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Parking = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Wifi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    WifiCost = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LodgingSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LodgingSettings_Lodging_Id",
                        column: x => x.Id,
                        principalTable: "Lodging",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    MarkupPercentage = table.Column<double>(type: "float", nullable: false),
                    Commission = table.Column<double>(type: "float", nullable: false),
                    Features = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Terms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Featured = table.Column<bool>(type: "bit", nullable: false),
                    LodgingId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voucher_Lodging_LodgingId",
                        column: x => x.LodgingId,
                        principalTable: "Lodging",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<ServiceTier, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityDocument<ServiceTier, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<ServiceTier, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<ServiceTier, string>_ServiceTier_EntityId",
                        column: x => x.EntityId,
                        principalTable: "ServiceTier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<ServiceTier, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Selector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityImage<ServiceTier, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<ServiceTier, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<ServiceTier, string>_ServiceTier_EntityId",
                        column: x => x.EntityId,
                        principalTable: "ServiceTier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<ServiceTier, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityVideo<ServiceTier, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<ServiceTier, string>_ServiceTier_EntityId",
                        column: x => x.EntityId,
                        principalTable: "ServiceTier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<ServiceTier, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TierService",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ServiceTierId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OfferedServiceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TierService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TierService_OfferedService_OfferedServiceId",
                        column: x => x.OfferedServiceId,
                        principalTable: "OfferedService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TierService_ServiceTier_ServiceTierId",
                        column: x => x.ServiceTierId,
                        principalTable: "ServiceTier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Address<UserInfo>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    GoogleMapLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Complex = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StreetNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    StreetName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Suburb = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    City = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Province = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Latitude = table.Column<double>(type: "float", maxLength: 255, nullable: false),
                    Longitude = table.Column<double>(type: "float", maxLength: 255, nullable: false),
                    RouteId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Default = table.Column<bool>(type: "bit", nullable: false),
                    AddressType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address<UserInfo>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address<UserInfo>_UserInfo_EntityId",
                        column: x => x.EntityId,
                        principalTable: "UserInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContactNumber<UserInfo>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    InternationalCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    AreaCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Default = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactNumber<UserInfo>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactNumber<UserInfo>_UserInfo_EntityId",
                        column: x => x.EntityId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailAddress<UserInfo>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Default = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAddress<UserInfo>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailAddress<UserInfo>_UserInfo_EntityId",
                        column: x => x.EntityId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<UserInfo, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityDocument<UserInfo, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<UserInfo, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<UserInfo, string>_UserInfo_EntityId",
                        column: x => x.EntityId,
                        principalTable: "UserInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<UserInfo, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Selector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityImage<UserInfo, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<UserInfo, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<UserInfo, string>_UserInfo_EntityId",
                        column: x => x.EntityId,
                        principalTable: "UserInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<UserInfo, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityVideo<UserInfo, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<UserInfo, string>_UserInfo_EntityId",
                        column: x => x.EntityId,
                        principalTable: "UserInfo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<UserInfo, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserAppSettings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShowJobTitle = table.Column<bool>(type: "bit", nullable: false),
                    ShowPhoneNr = table.Column<bool>(type: "bit", nullable: false),
                    ShowEmailAddress = table.Column<bool>(type: "bit", nullable: false),
                    ReceiveNotifications = table.Column<bool>(type: "bit", nullable: false),
                    ReceiveNewsletters = table.Column<bool>(type: "bit", nullable: false),
                    ReceiveMessages = table.Column<bool>(type: "bit", nullable: false),
                    ReceiveEmails = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAppSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAppSettings_UserInfo_Id",
                        column: x => x.Id,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DayTourActivity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DayNr = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    DayTourActivityTemplateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VacationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayTourActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayTourActivity_DayTourActivityTemplate_DayTourActivityTemplateId",
                        column: x => x.DayTourActivityTemplateId,
                        principalTable: "DayTourActivityTemplate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DayTourActivity_Vacation_VacationId",
                        column: x => x.VacationId,
                        principalTable: "Vacation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Vacation, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityDocument<Vacation, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Vacation, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Vacation, string>_Vacation_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Vacation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Vacation, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Selector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityImage<Vacation, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Vacation, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Vacation, string>_Vacation_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Vacation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Vacation, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityVideo<Vacation, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Vacation, string>_Vacation_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Vacation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Vacation, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GolferPackage",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DayNr = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Carts = table.Column<bool>(type: "bit", nullable: false),
                    Caddies = table.Column<bool>(type: "bit", nullable: false),
                    Halfway = table.Column<bool>(type: "bit", nullable: false),
                    GolfCourseId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VacationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GolferPackage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GolferPackage_GolfCourse_GolfCourseId",
                        column: x => x.GolfCourseId,
                        principalTable: "GolfCourse",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GolferPackage_Vacation_VacationId",
                        column: x => x.VacationId,
                        principalTable: "Vacation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItineraryEntryItemTemplate",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DayNr = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VacationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItineraryEntryItemTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItineraryEntryItemTemplate_Vacation_VacationId",
                        column: x => x.VacationId,
                        principalTable: "Vacation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealAddition",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DayNr = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    IntervalInclusion = table.Column<bool>(type: "bit", nullable: false),
                    MealAdditionTemplateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VacationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RestaurantId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealAddition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealAddition_MealAdditionTemplate_MealAdditionTemplateId",
                        column: x => x.MealAdditionTemplateId,
                        principalTable: "MealAdditionTemplate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MealAddition_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MealAddition_Vacation_VacationId",
                        column: x => x.VacationId,
                        principalTable: "Vacation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentRule",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MonthsBeforeVacationStart = table.Column<int>(type: "int", nullable: false),
                    RuleType = table.Column<int>(type: "int", nullable: false),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    VacationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentRule_Vacation_VacationId",
                        column: x => x.VacationId,
                        principalTable: "Vacation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review<Vacation>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review<Vacation>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Review<Vacation>_Vacation_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Vacation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoomGift",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DayNr = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    GiftId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VacationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomGift", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomGift_Gift_GiftId",
                        column: x => x.GiftId,
                        principalTable: "Gift",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoomGift_Vacation_VacationId",
                        column: x => x.VacationId,
                        principalTable: "Vacation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VacationContact",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VacationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContactId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationContact_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VacationContact_Vacation_VacationId",
                        column: x => x.VacationId,
                        principalTable: "Vacation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VacationExtensionAddition",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParentVacationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ExtensioId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationExtensionAddition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationExtensionAddition_Vacation_ExtensioId",
                        column: x => x.ExtensioId,
                        principalTable: "Vacation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VacationExtensionAddition_Vacation_ParentVacationId",
                        column: x => x.ParentVacationId,
                        principalTable: "Vacation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VacationInclusionDisplayTypeInformation",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VacationInclusionDisplayType = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    ColumnSelection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VacationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationInclusionDisplayTypeInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationInclusionDisplayTypeInformation_Vacation_VacationId",
                        column: x => x.VacationId,
                        principalTable: "Vacation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VacationInterval",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SortOrderNr = table.Column<int>(type: "int", nullable: false),
                    StartDayNr = table.Column<int>(type: "int", nullable: false),
                    NightCount = table.Column<int>(type: "int", nullable: false),
                    CheckInTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CheckOutTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    RoomType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MealPlan = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VacationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LodgingId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DestinationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationInterval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationInterval_Destination_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destination",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VacationInterval_Lodging_LodgingId",
                        column: x => x.LodgingId,
                        principalTable: "Lodging",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VacationInterval_Vacation_VacationId",
                        column: x => x.VacationId,
                        principalTable: "Vacation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VacationPrice",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Selector = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    VacationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationPrice_Vacation_VacationId",
                        column: x => x.VacationId,
                        principalTable: "Vacation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VacationPriceGroup",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VacationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationPriceGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationPriceGroup_Vacation_VacationId",
                        column: x => x.VacationId,
                        principalTable: "Vacation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flight",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartureDayNr = table.Column<int>(type: "int", nullable: true),
                    ArrivalDayNr = table.Column<int>(type: "int", nullable: false),
                    DepartureTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    ArrivalTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    DepartureAirportId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ArrivalAirportId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FlightNumber = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ConfirmationNumber = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DepartureMovementTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    DepartureMovementDestination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartureMovementMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartureMovementNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VacationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flight", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flight_Airport_ArrivalAirportId",
                        column: x => x.ArrivalAirportId,
                        principalTable: "Airport",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Flight_Airport_DepartureAirportId",
                        column: x => x.DepartureAirportId,
                        principalTable: "Airport",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Flight_Vacation_VacationId",
                        column: x => x.VacationId,
                        principalTable: "Vacation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LodgingService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Supplier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UniqueId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommPerc = table.Column<double>(type: "float", nullable: true),
                    Display = table.Column<bool>(type: "bit", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: true),
                    RatePeriodStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RatePeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MarkupPerc = table.Column<double>(type: "float", nullable: true),
                    CurrentRate = table.Column<double>(type: "float", nullable: true),
                    RoomsAvailable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoomRateTypeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPackage = table.Column<bool>(type: "bit", nullable: true),
                    RateCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailablePartnerUid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Includes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Excludes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoomInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChildPolicy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CancellationPolicy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UseLocalData = table.Column<bool>(type: "bit", nullable: true),
                    PackageId = table.Column<int>(type: "int", nullable: true),
                    LodgingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LodgingService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LodgingService_LodgingPackage_PackageId",
                        column: x => x.PackageId,
                        principalTable: "LodgingPackage",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LodgingService_Lodging_LodgingId",
                        column: x => x.LodgingId,
                        principalTable: "Lodging",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartnerRoomTypeId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BedCount = table.Column<int>(type: "int", nullable: false),
                    RoomCount = table.Column<int>(type: "int", nullable: false),
                    MaxOccupancy = table.Column<int>(type: "int", nullable: false),
                    MaxAdults = table.Column<int>(type: "int", nullable: false),
                    FirstChildStaysFree = table.Column<bool>(type: "bit", nullable: false),
                    BookingTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CancellationPolicy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChildPolicyDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultBedTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultMealPlanId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RateScheme = table.Column<int>(type: "int", nullable: true),
                    Commision = table.Column<double>(type: "float", nullable: false),
                    MarkUp = table.Column<double>(type: "float", nullable: false),
                    SpecialRate = table.Column<double>(type: "float", nullable: false),
                    VoucherRate = table.Column<double>(type: "float", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: true),
                    VoucherId = table.Column<int>(type: "int", nullable: true),
                    LodgingId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Room_LodgingPackage_PackageId",
                        column: x => x.PackageId,
                        principalTable: "LodgingPackage",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Room_Lodging_LodgingId",
                        column: x => x.LodgingId,
                        principalTable: "Lodging",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Room_Voucher_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Voucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Review<Vacation>, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityDocument<Review<Vacation>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Review<Vacation>, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Review<Vacation>, string>_Review<Vacation>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Review<Vacation>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Review<Vacation>, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Selector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityImage<Review<Vacation>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Review<Vacation>, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Review<Vacation>, string>_Review<Vacation>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Review<Vacation>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Review<Vacation>, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityVideo<Review<Vacation>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Review<Vacation>, string>_Review<Vacation>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Review<Vacation>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Review<Vacation>, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VacationReview",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VacationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationReview_Review<Vacation>_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Review<Vacation>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VacationReview_Vacation_VacationId",
                        column: x => x.VacationId,
                        principalTable: "Vacation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Itinerary",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ItineraryType = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VacationIntervalId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itinerary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Itinerary_VacationInterval_VacationIntervalId",
                        column: x => x.VacationIntervalId,
                        principalTable: "VacationInterval",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChildAgeParams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<double>(type: "float", nullable: true),
                    UniqueNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildAgeParams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChildAgeParams_LodgingService_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "LodgingService",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SingleRoom = table.Column<bool>(type: "bit", nullable: true),
                    SingleRoomRate = table.Column<double>(type: "float", nullable: true),
                    RateCodeSingle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasValueSingle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoubleRoom = table.Column<bool>(type: "bit", nullable: true),
                    DoubleRoomRate = table.Column<double>(type: "float", nullable: true),
                    RateCodeDouble = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasValueDouble = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailableDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TpnUid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rates_LodgingService_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "LodgingService",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AmenityItem<Lodging, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AmenityId = table.Column<int>(type: "int", nullable: false),
                    LodgingId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmenityItem<Lodging, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AmenityItem<Lodging, string>_Amenity_AmenityId",
                        column: x => x.AmenityId,
                        principalTable: "Amenity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AmenityItem<Lodging, string>_Lodging_LodgingId",
                        column: x => x.LodgingId,
                        principalTable: "Lodging",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AmenityItem<Lodging, string>_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BedType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PartnerBedTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BedCount = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    RoomId1 = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BedType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BedType_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BedType_Room_RoomId1",
                        column: x => x.RoomId1,
                        principalTable: "Room",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingReferenceNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contacts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Directions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentInsturctions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoomQty = table.Column<double>(type: "float", nullable: false),
                    RateId = table.Column<int>(type: "int", nullable: false),
                    RateDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adults = table.Column<int>(type: "int", nullable: false),
                    Children = table.Column<int>(type: "int", nullable: false),
                    Infants = table.Column<int>(type: "int", nullable: false),
                    CancellationId = table.Column<int>(type: "int", nullable: false),
                    BookingStatus = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    PackageId = table.Column<int>(type: "int", nullable: true),
                    LodgingId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OrderNr = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booking_LodgingPackage_PackageId",
                        column: x => x.PackageId,
                        principalTable: "LodgingPackage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_Lodging_LodgingId",
                        column: x => x.LodgingId,
                        principalTable: "Lodging",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Booking_Order_OrderNr",
                        column: x => x.OrderNr,
                        principalTable: "Order",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Booking_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Booking_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChildPolicyRule",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MinAge = table.Column<int>(type: "int", nullable: false),
                    MaxAge = table.Column<int>(type: "int", nullable: false),
                    Allowed = table.Column<bool>(type: "bit", nullable: false),
                    UseSpecialRate = table.Column<bool>(type: "bit", nullable: false),
                    ChildPolicyFormualaType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChildPolicyFormualaValue = table.Column<double>(type: "float", nullable: false),
                    CustomDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    RoomId1 = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildPolicyRule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChildPolicyRule_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChildPolicyRule_Room_RoomId1",
                        column: x => x.RoomId1,
                        principalTable: "Room",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Room, int>",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityDocument<Room, int>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Room, int>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Room, int>_Room_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Room, int>",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Selector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityImage<Room, int>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Room, int>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Room, int>_Room_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Room, int>",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityVideo<Room, int>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Room, int>_Room_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Room, int>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FeaturedImage",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    LodgingId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageType = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeaturedImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeaturedImage_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealPlan",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    RateId = table.Column<int>(type: "int", nullable: false),
                    PartnerMealPlanId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Default = table.Column<bool>(type: "bit", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    OriginalRate = table.Column<double>(type: "float", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealPlan_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceAmenity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceAmenity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceAmenity_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserVoucher",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoucherId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVoucher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserVoucher_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserVoucher_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserVoucher_Voucher_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Voucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItineraryItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItineraryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItineraryItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItineraryItem_Itinerary_ItineraryId",
                        column: x => x.ItineraryId,
                        principalTable: "Itinerary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address<UserInfo>_EntityId",
                table: "Address<UserInfo>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Airport_CityId",
                table: "Airport",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_AmenityItem<Lodging, string>_AmenityId",
                table: "AmenityItem<Lodging, string>",
                column: "AmenityId");

            migrationBuilder.CreateIndex(
                name: "IX_AmenityItem<Lodging, string>_LodgingId",
                table: "AmenityItem<Lodging, string>",
                column: "LodgingId");

            migrationBuilder.CreateIndex(
                name: "IX_AmenityItem<Lodging, string>_RoomId",
                table: "AmenityItem<Lodging, string>",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserPasskeys_UserId",
                table: "AspNetUserPasskeys",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AverageTemperature_AreaId",
                table: "AverageTemperature",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_BedType_RoomId",
                table: "BedType",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_BedType_RoomId1",
                table: "BedType",
                column: "RoomId1");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_LodgingId",
                table: "Booking",
                column: "LodgingId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_OrderNr",
                table: "Booking",
                column: "OrderNr");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_PackageId",
                table: "Booking",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_RoomId",
                table: "Booking",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_UserId",
                table: "Booking",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationRule_LodgingId",
                table: "CancellationRule",
                column: "LodgingId");

            migrationBuilder.CreateIndex(
                name: "IX_Category<Lodging>_ParentCategoryId",
                table: "Category<Lodging>",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Category<OfferedService>_ParentCategoryId",
                table: "Category<OfferedService>",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Category<Product>_ParentCategoryId",
                table: "Category<Product>",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildAgeParams_ServiceId",
                table: "ChildAgeParams",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildPolicyRule_RoomId",
                table: "ChildPolicyRule",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildPolicyRule_RoomId1",
                table: "ChildPolicyRule",
                column: "RoomId1");

            migrationBuilder.CreateIndex(
                name: "IX_City_CountryId",
                table: "City",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactNumber<UserInfo>_EntityId",
                table: "ContactNumber<UserInfo>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_DayTourActivity_DayTourActivityTemplateId",
                table: "DayTourActivity",
                column: "DayTourActivityTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_DayTourActivity_VacationId",
                table: "DayTourActivity",
                column: "VacationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAddress<UserInfo>_EntityId",
                table: "EmailAddress<UserInfo>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCategory<Lodging>_CategoryId",
                table: "EntityCategory<Lodging>",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCategory<Lodging>_EntityId",
                table: "EntityCategory<Lodging>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCategory<OfferedService>_CategoryId",
                table: "EntityCategory<OfferedService>",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCategory<OfferedService>_EntityId",
                table: "EntityCategory<OfferedService>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCategory<Product>_CategoryId",
                table: "EntityCategory<Product>",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCategory<Product>_EntityId",
                table: "EntityCategory<Product>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Category<Lodging>, string>_DocumentId",
                table: "EntityDocument<Category<Lodging>, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Category<Lodging>, string>_EntityId",
                table: "EntityDocument<Category<Lodging>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Category<OfferedService>, string>_DocumentId",
                table: "EntityDocument<Category<OfferedService>, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Category<OfferedService>, string>_EntityId",
                table: "EntityDocument<Category<OfferedService>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Category<Product>, string>_DocumentId",
                table: "EntityDocument<Category<Product>, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Category<Product>, string>_EntityId",
                table: "EntityDocument<Category<Product>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Contact, string>_DocumentId",
                table: "EntityDocument<Contact, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Contact, string>_EntityId",
                table: "EntityDocument<Contact, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Destination, string>_DocumentId",
                table: "EntityDocument<Destination, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Destination, string>_EntityId",
                table: "EntityDocument<Destination, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Gift, string>_DocumentId",
                table: "EntityDocument<Gift, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Gift, string>_EntityId",
                table: "EntityDocument<Gift, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<GolfCourse, string>_DocumentId",
                table: "EntityDocument<GolfCourse, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<GolfCourse, string>_EntityId",
                table: "EntityDocument<GolfCourse, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Lodging, string>_DocumentId",
                table: "EntityDocument<Lodging, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Lodging, string>_EntityId",
                table: "EntityDocument<Lodging, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<OfferedService, string>_DocumentId",
                table: "EntityDocument<OfferedService, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<OfferedService, string>_EntityId",
                table: "EntityDocument<OfferedService, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Product, string>_DocumentId",
                table: "EntityDocument<Product, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Product, string>_EntityId",
                table: "EntityDocument<Product, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Review<Vacation>, string>_DocumentId",
                table: "EntityDocument<Review<Vacation>, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Review<Vacation>, string>_EntityId",
                table: "EntityDocument<Review<Vacation>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Room, int>_DocumentId",
                table: "EntityDocument<Room, int>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Room, int>_EntityId",
                table: "EntityDocument<Room, int>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<ServiceTier, string>_DocumentId",
                table: "EntityDocument<ServiceTier, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<ServiceTier, string>_EntityId",
                table: "EntityDocument<ServiceTier, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<UserInfo, string>_DocumentId",
                table: "EntityDocument<UserInfo, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<UserInfo, string>_EntityId",
                table: "EntityDocument<UserInfo, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Vacation, string>_DocumentId",
                table: "EntityDocument<Vacation, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Vacation, string>_EntityId",
                table: "EntityDocument<Vacation, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<VacationHost, string>_DocumentId",
                table: "EntityDocument<VacationHost, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<VacationHost, string>_EntityId",
                table: "EntityDocument<VacationHost, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Category<Lodging>, string>_EntityId",
                table: "EntityImage<Category<Lodging>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Category<Lodging>, string>_ImageId",
                table: "EntityImage<Category<Lodging>, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Category<OfferedService>, string>_EntityId",
                table: "EntityImage<Category<OfferedService>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Category<OfferedService>, string>_ImageId",
                table: "EntityImage<Category<OfferedService>, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Category<Product>, string>_EntityId",
                table: "EntityImage<Category<Product>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Category<Product>, string>_ImageId",
                table: "EntityImage<Category<Product>, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Contact, string>_EntityId",
                table: "EntityImage<Contact, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Contact, string>_ImageId",
                table: "EntityImage<Contact, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Destination, string>_EntityId",
                table: "EntityImage<Destination, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Destination, string>_ImageId",
                table: "EntityImage<Destination, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Gift, string>_EntityId",
                table: "EntityImage<Gift, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Gift, string>_ImageId",
                table: "EntityImage<Gift, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<GolfCourse, string>_EntityId",
                table: "EntityImage<GolfCourse, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<GolfCourse, string>_ImageId",
                table: "EntityImage<GolfCourse, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Lodging, string>_EntityId",
                table: "EntityImage<Lodging, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Lodging, string>_ImageId",
                table: "EntityImage<Lodging, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<OfferedService, string>_EntityId",
                table: "EntityImage<OfferedService, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<OfferedService, string>_ImageId",
                table: "EntityImage<OfferedService, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Product, string>_EntityId",
                table: "EntityImage<Product, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Product, string>_ImageId",
                table: "EntityImage<Product, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Review<Vacation>, string>_EntityId",
                table: "EntityImage<Review<Vacation>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Review<Vacation>, string>_ImageId",
                table: "EntityImage<Review<Vacation>, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Room, int>_EntityId",
                table: "EntityImage<Room, int>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Room, int>_ImageId",
                table: "EntityImage<Room, int>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<ServiceTier, string>_EntityId",
                table: "EntityImage<ServiceTier, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<ServiceTier, string>_ImageId",
                table: "EntityImage<ServiceTier, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<UserInfo, string>_EntityId",
                table: "EntityImage<UserInfo, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<UserInfo, string>_ImageId",
                table: "EntityImage<UserInfo, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Vacation, string>_EntityId",
                table: "EntityImage<Vacation, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Vacation, string>_ImageId",
                table: "EntityImage<Vacation, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<VacationHost, string>_EntityId",
                table: "EntityImage<VacationHost, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<VacationHost, string>_ImageId",
                table: "EntityImage<VacationHost, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Category<Lodging>, string>_EntityId",
                table: "EntityVideo<Category<Lodging>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Category<Lodging>, string>_VideoId",
                table: "EntityVideo<Category<Lodging>, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Category<OfferedService>, string>_EntityId",
                table: "EntityVideo<Category<OfferedService>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Category<OfferedService>, string>_VideoId",
                table: "EntityVideo<Category<OfferedService>, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Category<Product>, string>_EntityId",
                table: "EntityVideo<Category<Product>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Category<Product>, string>_VideoId",
                table: "EntityVideo<Category<Product>, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Contact, string>_EntityId",
                table: "EntityVideo<Contact, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Contact, string>_VideoId",
                table: "EntityVideo<Contact, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Destination, string>_EntityId",
                table: "EntityVideo<Destination, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Destination, string>_VideoId",
                table: "EntityVideo<Destination, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Gift, string>_EntityId",
                table: "EntityVideo<Gift, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Gift, string>_VideoId",
                table: "EntityVideo<Gift, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<GolfCourse, string>_EntityId",
                table: "EntityVideo<GolfCourse, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<GolfCourse, string>_VideoId",
                table: "EntityVideo<GolfCourse, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Lodging, string>_EntityId",
                table: "EntityVideo<Lodging, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Lodging, string>_VideoId",
                table: "EntityVideo<Lodging, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<OfferedService, string>_EntityId",
                table: "EntityVideo<OfferedService, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<OfferedService, string>_VideoId",
                table: "EntityVideo<OfferedService, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Product, string>_EntityId",
                table: "EntityVideo<Product, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Product, string>_VideoId",
                table: "EntityVideo<Product, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Review<Vacation>, string>_EntityId",
                table: "EntityVideo<Review<Vacation>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Review<Vacation>, string>_VideoId",
                table: "EntityVideo<Review<Vacation>, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Room, int>_EntityId",
                table: "EntityVideo<Room, int>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Room, int>_VideoId",
                table: "EntityVideo<Room, int>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<ServiceTier, string>_EntityId",
                table: "EntityVideo<ServiceTier, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<ServiceTier, string>_VideoId",
                table: "EntityVideo<ServiceTier, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<UserInfo, string>_EntityId",
                table: "EntityVideo<UserInfo, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<UserInfo, string>_VideoId",
                table: "EntityVideo<UserInfo, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Vacation, string>_EntityId",
                table: "EntityVideo<Vacation, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Vacation, string>_VideoId",
                table: "EntityVideo<Vacation, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<VacationHost, string>_EntityId",
                table: "EntityVideo<VacationHost, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<VacationHost, string>_VideoId",
                table: "EntityVideo<VacationHost, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_FeaturedImage_RoomId",
                table: "FeaturedImage",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_ArrivalAirportId",
                table: "Flight",
                column: "ArrivalAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_DepartureAirportId",
                table: "Flight",
                column: "DepartureAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_VacationId",
                table: "Flight",
                column: "VacationId");

            migrationBuilder.CreateIndex(
                name: "IX_Follower_UserFollowingId",
                table: "Follower",
                column: "UserFollowingId");

            migrationBuilder.CreateIndex(
                name: "IX_GolfCourseDestination_DestinationId",
                table: "GolfCourseDestination",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_GolfCourseDestination_GolfCourseId",
                table: "GolfCourseDestination",
                column: "GolfCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_GolferPackage_GolfCourseId",
                table: "GolferPackage",
                column: "GolfCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_GolferPackage_VacationId",
                table: "GolferPackage",
                column: "VacationId");

            migrationBuilder.CreateIndex(
                name: "IX_Itinerary_VacationIntervalId",
                table: "Itinerary",
                column: "VacationIntervalId");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryEntryItemTemplate_VacationId",
                table: "ItineraryEntryItemTemplate",
                column: "VacationId");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryItem_ItineraryId",
                table: "ItineraryItem",
                column: "ItineraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Lodging_CountryId",
                table: "Lodging",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Lodging_LodgingTypeId",
                table: "Lodging",
                column: "LodgingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LodgingDestination_DestinationId",
                table: "LodgingDestination",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_LodgingDestination_LodgingId",
                table: "LodgingDestination",
                column: "LodgingId");

            migrationBuilder.CreateIndex(
                name: "IX_LodgingDestination_LodgingId1",
                table: "LodgingDestination",
                column: "LodgingId1");

            migrationBuilder.CreateIndex(
                name: "IX_LodgingPackage_LodgingId",
                table: "LodgingPackage",
                column: "LodgingId");

            migrationBuilder.CreateIndex(
                name: "IX_LodgingService_LodgingId",
                table: "LodgingService",
                column: "LodgingId");

            migrationBuilder.CreateIndex(
                name: "IX_LodgingService_PackageId",
                table: "LodgingService",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_MealAddition_MealAdditionTemplateId",
                table: "MealAddition",
                column: "MealAdditionTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_MealAddition_RestaurantId",
                table: "MealAddition",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_MealAddition_VacationId",
                table: "MealAddition",
                column: "VacationId");

            migrationBuilder.CreateIndex(
                name: "IX_MealAdditionTemplate_RestaurantId",
                table: "MealAdditionTemplate",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlan_RoomId",
                table: "MealPlan",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetAndGreetTemplate_ContactId",
                table: "MeetAndGreetTemplate",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRule_VacationId",
                table: "PaymentRule",
                column: "VacationId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_VariantParentId",
                table: "Product",
                column: "VariantParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMetadata_ProductId",
                table: "ProductMetadata",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_ServiceId",
                table: "Rates",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Review<Vacation>_EntityId",
                table: "Review<Vacation>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "Identity",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_LodgingId",
                table: "Room",
                column: "LodgingId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_PackageId",
                table: "Room",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_VoucherId",
                table: "Room",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomGift_GiftId",
                table: "RoomGift",
                column: "GiftId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomGift_VacationId",
                table: "RoomGift",
                column: "VacationId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAmenity_RoomId",
                table: "ServiceAmenity",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTier_RoleId",
                table: "ServiceTier",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TierService_OfferedServiceId",
                table: "TierService",
                column: "OfferedServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TierService_ServiceTierId",
                table: "TierService",
                column: "ServiceTierId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_ParentRoleId",
                schema: "Identity",
                table: "UserRoles",
                column: "ParentRoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Identity",
                table: "UserRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Identity",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Identity",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserVoucher_OrderId",
                table: "UserVoucher",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVoucher_RoomId",
                table: "UserVoucher",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVoucher_VoucherId",
                table: "UserVoucher",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacation_BookingTermsTemplateId",
                table: "Vacation",
                column: "BookingTermsTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacation_CancellationTermsTemplateId",
                table: "Vacation",
                column: "CancellationTermsTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacation_GeneralInformationTemplateId",
                table: "Vacation",
                column: "GeneralInformationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacation_MeetAndGreetTemplateId",
                table: "Vacation",
                column: "MeetAndGreetTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacation_PaymentExclusionTemplateId",
                table: "Vacation",
                column: "PaymentExclusionTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacation_ShortDescriptionTemplateId",
                table: "Vacation",
                column: "ShortDescriptionTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacation_Slug",
                table: "Vacation",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Vacation_TermsAndConditionsTemplateId",
                table: "Vacation",
                column: "TermsAndConditionsTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacation_VacationHostId",
                table: "Vacation",
                column: "VacationHostId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacation_VacationTitleTemplateId",
                table: "Vacation",
                column: "VacationTitleTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationContact_ContactId",
                table: "VacationContact",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationContact_VacationId",
                table: "VacationContact",
                column: "VacationId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationExtensionAddition_ExtensioId",
                table: "VacationExtensionAddition",
                column: "ExtensioId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationExtensionAddition_ParentVacationId",
                table: "VacationExtensionAddition",
                column: "ParentVacationId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationHost_AreaId",
                table: "VacationHost",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationHost_Name",
                table: "VacationHost",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VacationInclusionDisplayTypeInformation_VacationId",
                table: "VacationInclusionDisplayTypeInformation",
                column: "VacationId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationInterval_DestinationId",
                table: "VacationInterval",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationInterval_LodgingId",
                table: "VacationInterval",
                column: "LodgingId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationInterval_VacationId",
                table: "VacationInterval",
                column: "VacationId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationPrice_VacationId",
                table: "VacationPrice",
                column: "VacationId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationPriceGroup_VacationId",
                table: "VacationPriceGroup",
                column: "VacationId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationReview_ReviewId",
                table: "VacationReview",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationReview_VacationId",
                table: "VacationReview",
                column: "VacationId");

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_LodgingId",
                table: "Voucher",
                column: "LodgingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address<UserInfo>");

            migrationBuilder.DropTable(
                name: "AmenityItem<Lodging, string>");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserPasskeys");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditTrails");

            migrationBuilder.DropTable(
                name: "AverageTemperature");

            migrationBuilder.DropTable(
                name: "BedType");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "CancellationRule");

            migrationBuilder.DropTable(
                name: "ChildAgeParams");

            migrationBuilder.DropTable(
                name: "ChildPolicyRule");

            migrationBuilder.DropTable(
                name: "ContactNumber<UserInfo>");

            migrationBuilder.DropTable(
                name: "CustomVariableTag");

            migrationBuilder.DropTable(
                name: "DayTourActivity");

            migrationBuilder.DropTable(
                name: "DeviceToken");

            migrationBuilder.DropTable(
                name: "EmailAddress<UserInfo>");

            migrationBuilder.DropTable(
                name: "EntityCategory<Lodging>");

            migrationBuilder.DropTable(
                name: "EntityCategory<OfferedService>");

            migrationBuilder.DropTable(
                name: "EntityCategory<Product>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Category<Lodging>, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Category<OfferedService>, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Category<Product>, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Contact, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Destination, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Gift, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<GolfCourse, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Lodging, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<OfferedService, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Product, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Review<Vacation>, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Room, int>");

            migrationBuilder.DropTable(
                name: "EntityDocument<ServiceTier, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<UserInfo, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Vacation, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<VacationHost, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Category<Lodging>, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Category<OfferedService>, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Category<Product>, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Contact, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Destination, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Gift, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<GolfCourse, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Lodging, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<OfferedService, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Product, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Review<Vacation>, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Room, int>");

            migrationBuilder.DropTable(
                name: "EntityImage<ServiceTier, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<UserInfo, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Vacation, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<VacationHost, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Category<Lodging>, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Category<OfferedService>, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Category<Product>, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Contact, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Destination, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Gift, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<GolfCourse, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Lodging, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<OfferedService, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Product, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Review<Vacation>, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Room, int>");

            migrationBuilder.DropTable(
                name: "EntityVideo<ServiceTier, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<UserInfo, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Vacation, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<VacationHost, string>");

            migrationBuilder.DropTable(
                name: "FeaturedImage");

            migrationBuilder.DropTable(
                name: "Flight");

            migrationBuilder.DropTable(
                name: "Follower");

            migrationBuilder.DropTable(
                name: "GolfCourseDestination");

            migrationBuilder.DropTable(
                name: "GolferPackage");

            migrationBuilder.DropTable(
                name: "ItineraryEntryItemTemplate");

            migrationBuilder.DropTable(
                name: "ItineraryItem");

            migrationBuilder.DropTable(
                name: "LodgingDestination");

            migrationBuilder.DropTable(
                name: "LodgingListingRequest");

            migrationBuilder.DropTable(
                name: "LodgingSettings");

            migrationBuilder.DropTable(
                name: "MealAddition");

            migrationBuilder.DropTable(
                name: "MealPlan");

            migrationBuilder.DropTable(
                name: "PaymentRule");

            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropTable(
                name: "ProductMetadata");

            migrationBuilder.DropTable(
                name: "Rates");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "RoomGift");

            migrationBuilder.DropTable(
                name: "ServiceAmenity");

            migrationBuilder.DropTable(
                name: "TierService");

            migrationBuilder.DropTable(
                name: "UserAppSettings");

            migrationBuilder.DropTable(
                name: "UserVoucher");

            migrationBuilder.DropTable(
                name: "VacationContact");

            migrationBuilder.DropTable(
                name: "VacationContactUsInfo");

            migrationBuilder.DropTable(
                name: "VacationExtensionAddition");

            migrationBuilder.DropTable(
                name: "VacationInclusionDisplayTypeInformation");

            migrationBuilder.DropTable(
                name: "VacationPrice");

            migrationBuilder.DropTable(
                name: "VacationPriceGroup");

            migrationBuilder.DropTable(
                name: "VacationReview");

            migrationBuilder.DropTable(
                name: "Amenity");

            migrationBuilder.DropTable(
                name: "DayTourActivityTemplate");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Category<Lodging>");

            migrationBuilder.DropTable(
                name: "Category<OfferedService>");

            migrationBuilder.DropTable(
                name: "Category<Product>");

            migrationBuilder.DropTable(
                name: "Video");

            migrationBuilder.DropTable(
                name: "Airport");

            migrationBuilder.DropTable(
                name: "GolfCourse");

            migrationBuilder.DropTable(
                name: "Itinerary");

            migrationBuilder.DropTable(
                name: "MealAdditionTemplate");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "LodgingService");

            migrationBuilder.DropTable(
                name: "Gift");

            migrationBuilder.DropTable(
                name: "OfferedService");

            migrationBuilder.DropTable(
                name: "ServiceTier");

            migrationBuilder.DropTable(
                name: "UserInfo");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "Review<Vacation>");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "VacationInterval");

            migrationBuilder.DropTable(
                name: "Restaurant");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "LodgingPackage");

            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.DropTable(
                name: "Destination");

            migrationBuilder.DropTable(
                name: "Vacation");

            migrationBuilder.DropTable(
                name: "Lodging");

            migrationBuilder.DropTable(
                name: "BookingTermsDescriptionTemplate");

            migrationBuilder.DropTable(
                name: "CancellationTermsTemplate");

            migrationBuilder.DropTable(
                name: "GeneralInformationTemplate");

            migrationBuilder.DropTable(
                name: "MeetAndGreetTemplate");

            migrationBuilder.DropTable(
                name: "PaymentExclusionTemplate");

            migrationBuilder.DropTable(
                name: "ShortDescriptionTemplate");

            migrationBuilder.DropTable(
                name: "TermsAndConditionsTemplate");

            migrationBuilder.DropTable(
                name: "VacationHost");

            migrationBuilder.DropTable(
                name: "VacationTitleTemplate");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "LodgingType");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "Area");
        }
    }
}
