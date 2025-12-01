using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EversdalPrimary.Blazor.ServerClient.Migrations
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
                name: "AdvertisementTier",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailabilityCount = table.Column<int>(type: "int", nullable: false),
                    Days = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    AdvertisementType = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_AdvertisementTier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Affiliate",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Featured = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Affiliate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgeGroup",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinAge = table.Column<int>(type: "int", nullable: false),
                    MaxAge = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_AgeGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceGroup",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ParentGroupId = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_AttendanceGroup", x => x.Id);
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
                name: "BlogPost",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    SocialShares = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Featured = table.Column<bool>(type: "bit", nullable: false),
                    Reply = table.Column<bool>(type: "bit", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentLinks = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_BlogPost", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category<ActivityGroup>",
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
                    table.PrimaryKey("PK_Category<ActivityGroup>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category<ActivityGroup>_Category<ActivityGroup>_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Category<ActivityGroup>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Category<BlogPost>",
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
                    table.PrimaryKey("PK_Category<BlogPost>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category<BlogPost>_Category<BlogPost>_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Category<BlogPost>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Category<BusinessListing>",
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
                    table.PrimaryKey("PK_Category<BusinessListing>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category<BusinessListing>_Category<BusinessListing>_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Category<BusinessListing>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Category<Event<ActivityGroup>>",
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
                    table.PrimaryKey("PK_Category<Event<ActivityGroup>>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category<Event<ActivityGroup>>_Category<Event<ActivityGroup>>_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Category<Event<ActivityGroup>>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Category<Event<Category<ActivityGroup>>>",
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
                    table.PrimaryKey("PK_Category<Event<Category<ActivityGroup>>>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category<Event<Category<ActivityGroup>>>_Category<Event<Category<ActivityGroup>>>_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Category<Event<Category<ActivityGroup>>>",
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
                name: "Coupon",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountPercentage = table.Column<double>(type: "float", nullable: false),
                    DeActivationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Coupon", x => x.Id);
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
                name: "Donation",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsAnonymous = table.Column<bool>(type: "bit", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    AgreeToTerms = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Donation", x => x.Id);
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
                name: "Invoice",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InvoiceNr = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_Invoice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListingTier",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    AllowServiceAndProductListing = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_ListingTier", x => x.Id);
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
                name: "Parent",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParentIdNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WalletId = table.Column<int>(type: "int", nullable: true),
                    WalletUserId = table.Column<int>(type: "int", nullable: true),
                    RequireConsent = table.Column<bool>(type: "bit", nullable: false),
                    ReceiveNotifications = table.Column<bool>(type: "bit", nullable: false),
                    ReceiveMessages = table.Column<bool>(type: "bit", nullable: false),
                    RecieveEmails = table.Column<bool>(type: "bit", nullable: false),
                    EmergencyMedicalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalAidProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalAidPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalAidNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalAidMainMember = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalAidMainMemberIdNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryPhysicianName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryPhysicianContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Parent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiptNr = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    PaymentReference = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    PaymentResult = table.Column<int>(type: "int", nullable: false),
                    PaymentResultDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Payment", x => x.Id);
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
                name: "SchoolGrade",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_SchoolGrade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeverityScale",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SeverityScale", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShoppingCartId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ShoppingCartItemType = table.Column<int>(type: "int", nullable: false),
                    LodgingId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackageId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UniquePartnerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qty = table.Column<double>(type: "float", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Adults = table.Column<int>(type: "int", nullable: false),
                    KidsGroup1 = table.Column<int>(type: "int", nullable: false),
                    KidsGroup2 = table.Column<int>(type: "int", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sku = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceExcl = table.Column<double>(type: "float", nullable: false),
                    Vat = table.Column<double>(type: "float", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    Commission = table.Column<double>(type: "float", nullable: false),
                    PriceIncl = table.Column<double>(type: "float", nullable: false),
                    PriceOld = table.Column<double>(type: "float", nullable: true),
                    RateId = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_ShoppingCartItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItem_ShoppingCartItem_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ShoppingCartItem",
                        principalColumn: "Id");
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
                name: "Comment<BlogPost>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ParentCommentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_Comment<BlogPost>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment<BlogPost>_BlogPost_EntityId",
                        column: x => x.EntityId,
                        principalTable: "BlogPost",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comment<BlogPost>_Comment<BlogPost>_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "Comment<BlogPost>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityCategory<BlogPost>",
                columns: table => new
                {
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_EntityCategory<BlogPost>", x => new { x.EntityId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_EntityCategory<BlogPost>_BlogPost_EntityId",
                        column: x => x.EntityId,
                        principalTable: "BlogPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityCategory<BlogPost>_Category<BlogPost>_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category<BlogPost>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartCoupon",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShoppingCartId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CouponId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_ShoppingCartCoupon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCartCoupon_Coupon_CouponId",
                        column: x => x.CouponId,
                        principalTable: "Coupon",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<AdvertisementTier, string>",
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
                    table.PrimaryKey("PK_EntityDocument<AdvertisementTier, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<AdvertisementTier, string>_AdvertisementTier_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AdvertisementTier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<AdvertisementTier, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Affiliate, string>",
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
                    table.PrimaryKey("PK_EntityDocument<Affiliate, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Affiliate, string>_Affiliate_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Affiliate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Affiliate, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<BlogPost, string>",
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
                    table.PrimaryKey("PK_EntityDocument<BlogPost, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<BlogPost, string>_BlogPost_EntityId",
                        column: x => x.EntityId,
                        principalTable: "BlogPost",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<BlogPost, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Category<ActivityGroup>, string>",
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
                    table.PrimaryKey("PK_EntityDocument<Category<ActivityGroup>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Category<ActivityGroup>, string>_Category<ActivityGroup>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<ActivityGroup>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Category<ActivityGroup>, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Category<BlogPost>, string>",
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
                    table.PrimaryKey("PK_EntityDocument<Category<BlogPost>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Category<BlogPost>, string>_Category<BlogPost>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<BlogPost>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Category<BlogPost>, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Category<BusinessListing>, string>",
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
                    table.PrimaryKey("PK_EntityDocument<Category<BusinessListing>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Category<BusinessListing>, string>_Category<BusinessListing>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<BusinessListing>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Category<BusinessListing>, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Category<Event<ActivityGroup>>, string>",
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
                    table.PrimaryKey("PK_EntityDocument<Category<Event<ActivityGroup>>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Category<Event<ActivityGroup>>, string>_Category<Event<ActivityGroup>>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<Event<ActivityGroup>>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Category<Event<ActivityGroup>>, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Category<Event<Category<ActivityGroup>>>, string>",
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
                    table.PrimaryKey("PK_EntityDocument<Category<Event<Category<ActivityGroup>>>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Category<Event<Category<ActivityGroup>>>, string>_Category<Event<Category<ActivityGroup>>>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<Event<Category<ActivityGroup>>>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Category<Event<Category<ActivityGroup>>>, string>_Document_DocumentId",
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
                name: "EntityImage<AdvertisementTier, string>",
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
                    table.PrimaryKey("PK_EntityImage<AdvertisementTier, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<AdvertisementTier, string>_AdvertisementTier_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AdvertisementTier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<AdvertisementTier, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Affiliate, string>",
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
                    table.PrimaryKey("PK_EntityImage<Affiliate, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Affiliate, string>_Affiliate_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Affiliate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityImage<Affiliate, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<BlogPost, string>",
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
                    table.PrimaryKey("PK_EntityImage<BlogPost, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<BlogPost, string>_BlogPost_EntityId",
                        column: x => x.EntityId,
                        principalTable: "BlogPost",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<BlogPost, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Category<ActivityGroup>, string>",
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
                    table.PrimaryKey("PK_EntityImage<Category<ActivityGroup>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Category<ActivityGroup>, string>_Category<ActivityGroup>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<ActivityGroup>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Category<ActivityGroup>, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Category<BlogPost>, string>",
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
                    table.PrimaryKey("PK_EntityImage<Category<BlogPost>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Category<BlogPost>, string>_Category<BlogPost>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<BlogPost>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Category<BlogPost>, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Category<BusinessListing>, string>",
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
                    table.PrimaryKey("PK_EntityImage<Category<BusinessListing>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Category<BusinessListing>, string>_Category<BusinessListing>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<BusinessListing>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Category<BusinessListing>, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Category<Event<ActivityGroup>>, string>",
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
                    table.PrimaryKey("PK_EntityImage<Category<Event<ActivityGroup>>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Category<Event<ActivityGroup>>, string>_Category<Event<ActivityGroup>>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<Event<ActivityGroup>>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Category<Event<ActivityGroup>>, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Category<Event<Category<ActivityGroup>>>, string>",
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
                    table.PrimaryKey("PK_EntityImage<Category<Event<Category<ActivityGroup>>>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Category<Event<Category<ActivityGroup>>>, string>_Category<Event<Category<ActivityGroup>>>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<Event<Category<ActivityGroup>>>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Category<Event<Category<ActivityGroup>>>, string>_Image_ImageId",
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
                name: "EntityDocument<ListingTier, string>",
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
                    table.PrimaryKey("PK_EntityDocument<ListingTier, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<ListingTier, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<ListingTier, string>_ListingTier_EntityId",
                        column: x => x.EntityId,
                        principalTable: "ListingTier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<ListingTier, string>",
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
                    table.PrimaryKey("PK_EntityImage<ListingTier, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<ListingTier, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<ListingTier, string>_ListingTier_EntityId",
                        column: x => x.EntityId,
                        principalTable: "ListingTier",
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
                name: "Address<Parent>",
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
                    table.PrimaryKey("PK_Address<Parent>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address<Parent>_Parent_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Parent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactNumber<Parent>",
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
                    table.PrimaryKey("PK_ContactNumber<Parent>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactNumber<Parent>_Parent_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Parent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailAddress<Parent>",
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
                    table.PrimaryKey("PK_EmailAddress<Parent>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailAddress<Parent>_Parent_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Parent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Parent, string>",
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
                    table.PrimaryKey("PK_EntityDocument<Parent, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Parent, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Parent, string>_Parent_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Parent",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Parent, string>",
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
                    table.PrimaryKey("PK_EntityImage<Parent, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Parent, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Parent, string>_Parent_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Parent",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ParentEmergencyContact",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Relationship = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    PrimaryPhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SecondaryPhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_ParentEmergencyContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentEmergencyContact_Parent_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "InvoiceDetail",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Qty = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    VatRate = table.Column<double>(type: "float", nullable: false),
                    ProfitPercentage = table.Column<double>(type: "float", nullable: false),
                    CustomerDiscountPercentage = table.Column<double>(type: "float", nullable: false),
                    ProductDiscountPercentage = table.Column<double>(type: "float", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InvoiceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_InvoiceDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceDetail_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceDetail_Product_ProductId",
                        column: x => x.ProductId,
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
                name: "SchoolClass",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutoCreateChatGroup = table.Column<bool>(type: "bit", nullable: false),
                    GradeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_SchoolClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolClass_SchoolGrade_GradeId",
                        column: x => x.GradeId,
                        principalTable: "SchoolGrade",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DisciplinaryAction",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeverityScaleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_DisciplinaryAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisciplinaryAction_SeverityScale_SeverityScaleId",
                        column: x => x.SeverityScaleId,
                        principalTable: "SeverityScale",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartItemMetadata",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShoppingCartItemId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_ShoppingCartItemMetadata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItemMetadata_ShoppingCartItem_ShoppingCartItemId",
                        column: x => x.ShoppingCartItemId,
                        principalTable: "ShoppingCartItem",
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
                name: "Advertisement",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AdvertisementType = table.Column<int>(type: "int", nullable: false),
                    SetupCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Available = table.Column<int>(type: "int", nullable: false),
                    AdvertisementTierId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_Advertisement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Advertisement_AdvertisementTier_AdvertisementTierId",
                        column: x => x.AdvertisementTierId,
                        principalTable: "AdvertisementTier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Advertisement_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id");
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
                name: "BusinessListing",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Heading = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slogan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActiveUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ListingTierId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_BusinessListing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessListing_ListingTier_ListingTierId",
                        column: x => x.ListingTierId,
                        principalTable: "ListingTier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BusinessListing_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id");
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
                name: "EntityVideo<AdvertisementTier, string>",
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
                    table.PrimaryKey("PK_EntityVideo<AdvertisementTier, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<AdvertisementTier, string>_AdvertisementTier_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AdvertisementTier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<AdvertisementTier, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Affiliate, string>",
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
                    table.PrimaryKey("PK_EntityVideo<Affiliate, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Affiliate, string>_Affiliate_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Affiliate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Affiliate, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<BlogPost, string>",
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
                    table.PrimaryKey("PK_EntityVideo<BlogPost, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<BlogPost, string>_BlogPost_EntityId",
                        column: x => x.EntityId,
                        principalTable: "BlogPost",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<BlogPost, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Category<ActivityGroup>, string>",
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
                    table.PrimaryKey("PK_EntityVideo<Category<ActivityGroup>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Category<ActivityGroup>, string>_Category<ActivityGroup>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<ActivityGroup>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Category<ActivityGroup>, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Category<BlogPost>, string>",
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
                    table.PrimaryKey("PK_EntityVideo<Category<BlogPost>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Category<BlogPost>, string>_Category<BlogPost>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<BlogPost>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Category<BlogPost>, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Category<BusinessListing>, string>",
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
                    table.PrimaryKey("PK_EntityVideo<Category<BusinessListing>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Category<BusinessListing>, string>_Category<BusinessListing>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<BusinessListing>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Category<BusinessListing>, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Category<Event<ActivityGroup>>, string>",
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
                    table.PrimaryKey("PK_EntityVideo<Category<Event<ActivityGroup>>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Category<Event<ActivityGroup>>, string>_Category<Event<ActivityGroup>>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<Event<ActivityGroup>>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Category<Event<ActivityGroup>>, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Category<Event<Category<ActivityGroup>>>, string>",
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
                    table.PrimaryKey("PK_EntityVideo<Category<Event<Category<ActivityGroup>>>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Category<Event<Category<ActivityGroup>>>, string>_Category<Event<Category<ActivityGroup>>>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<Event<Category<ActivityGroup>>>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Category<Event<Category<ActivityGroup>>>, string>_Video_VideoId",
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
                name: "EntityVideo<ListingTier, string>",
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
                    table.PrimaryKey("PK_EntityVideo<ListingTier, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<ListingTier, string>_ListingTier_EntityId",
                        column: x => x.EntityId,
                        principalTable: "ListingTier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<ListingTier, string>_Video_VideoId",
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
                name: "EntityVideo<Parent, string>",
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
                    table.PrimaryKey("PK_EntityVideo<Parent, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Parent, string>_Parent_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Parent",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Parent, string>_Video_VideoId",
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
                name: "Learner",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChildGuid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalAidParentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    SchoolUid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequireConsentFromAllParents = table.Column<bool>(type: "bit", nullable: false),
                    ReceiveNotifications = table.Column<bool>(type: "bit", nullable: false),
                    ReceiveMessages = table.Column<bool>(type: "bit", nullable: false),
                    RecieveEmails = table.Column<bool>(type: "bit", nullable: false),
                    SchoolGradeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SchoolClassId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_Learner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Learner_Parent_MedicalAidParentId",
                        column: x => x.MedicalAidParentId,
                        principalTable: "Parent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Learner_SchoolClass_SchoolClassId",
                        column: x => x.SchoolClassId,
                        principalTable: "SchoolClass",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Learner_SchoolGrade_SchoolGradeId",
                        column: x => x.SchoolGradeId,
                        principalTable: "SchoolGrade",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MessageType = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpenedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeviceTokensNotified = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificationSubscriptionsNotified = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolClassId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_SchoolClass_SchoolClassId",
                        column: x => x.SchoolClassId,
                        principalTable: "SchoolClass",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiveNotifications = table.Column<bool>(type: "bit", nullable: false),
                    ReceiveMessages = table.Column<bool>(type: "bit", nullable: false),
                    RecieveEmails = table.Column<bool>(type: "bit", nullable: false),
                    GradeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SchoolClassId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_Teacher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teacher_SchoolClass_SchoolClassId",
                        column: x => x.SchoolClassId,
                        principalTable: "SchoolClass",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Teacher_SchoolGrade_GradeId",
                        column: x => x.GradeId,
                        principalTable: "SchoolGrade",
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
                name: "EntityDocument<Advertisement, string>",
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
                    table.PrimaryKey("PK_EntityDocument<Advertisement, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Advertisement, string>_Advertisement_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Advertisement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Advertisement, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Advertisement, string>",
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
                    table.PrimaryKey("PK_EntityImage<Advertisement, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Advertisement, string>_Advertisement_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Advertisement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Advertisement, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Advertisement, string>",
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
                    table.PrimaryKey("PK_EntityVideo<Advertisement, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Advertisement, string>_Advertisement_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Advertisement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Advertisement, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityCategory<BusinessListing>",
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
                    table.PrimaryKey("PK_EntityCategory<BusinessListing>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityCategory<BusinessListing>_BusinessListing_EntityId",
                        column: x => x.EntityId,
                        principalTable: "BusinessListing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityCategory<BusinessListing>_Category<BusinessListing>_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category<BusinessListing>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<BusinessListing, string>",
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
                    table.PrimaryKey("PK_EntityDocument<BusinessListing, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<BusinessListing, string>_BusinessListing_EntityId",
                        column: x => x.EntityId,
                        principalTable: "BusinessListing",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<BusinessListing, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<BusinessListing, string>",
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
                    table.PrimaryKey("PK_EntityImage<BusinessListing, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<BusinessListing, string>_BusinessListing_EntityId",
                        column: x => x.EntityId,
                        principalTable: "BusinessListing",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<BusinessListing, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<BusinessListing, string>",
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
                    table.PrimaryKey("PK_EntityVideo<BusinessListing, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<BusinessListing, string>_BusinessListing_EntityId",
                        column: x => x.EntityId,
                        principalTable: "BusinessListing",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<BusinessListing, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ListingProduct",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ListingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_ListingProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingProduct_BusinessListing_ListingId",
                        column: x => x.ListingId,
                        principalTable: "BusinessListing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListingService",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ListingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_ListingService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingService_BusinessListing_ListingId",
                        column: x => x.ListingId,
                        principalTable: "BusinessListing",
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
                name: "Appointment",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RecurrenceRule = table.Column<int>(type: "int", nullable: false),
                    RecurrenceFormula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Heading = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AudienceType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FullDayEvent = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Appointment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointment_UserInfo_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BlogEntryView",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BlogPostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_BlogEntryView", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogEntryView_BlogPost_BlogPostId",
                        column: x => x.BlogPostId,
                        principalTable: "BlogPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogEntryView_UserInfo_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "SalesOrder",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderNr = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserInfoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    DeliveryMethod = table.Column<int>(type: "int", nullable: false),
                    ShippingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstimatedShippingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShippingReference = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrackingNr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ShippingTotal = table.Column<double>(type: "float", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    CouponDiscount = table.Column<double>(type: "float", nullable: false),
                    InvoiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_SalesOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrder_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrder_UserInfo_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "AttendanceRecord",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LearnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_AttendanceRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceRecord_AttendanceGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "AttendanceGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceRecord_Learner_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactNumber<Learner>",
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
                    table.PrimaryKey("PK_ContactNumber<Learner>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactNumber<Learner>_Learner_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Learner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisciplinaryIncident",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LearnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisciplinaryActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SeverityScore = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_DisciplinaryIncident", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisciplinaryIncident_DisciplinaryAction_DisciplinaryActionId",
                        column: x => x.DisciplinaryActionId,
                        principalTable: "DisciplinaryAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DisciplinaryIncident_Learner_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailAddress<Learner>",
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
                    table.PrimaryKey("PK_EmailAddress<Learner>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailAddress<Learner>_Learner_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Learner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Learner, string>",
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
                    table.PrimaryKey("PK_EntityDocument<Learner, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Learner, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Learner, string>_Learner_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Learner",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Learner, string>",
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
                    table.PrimaryKey("PK_EntityImage<Learner, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Learner, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Learner, string>_Learner_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Learner",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Learner, string>",
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
                    table.PrimaryKey("PK_EntityVideo<Learner, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Learner, string>_Learner_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Learner",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Learner, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LearnerParent",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LearnerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ParentConsentRequired = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_LearnerParent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearnerParent_Learner_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearnerParent_Parent_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityGroup",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutoCreateChatGroup = table.Column<bool>(type: "bit", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    AgeGroupId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_ActivityGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityGroup_AgeGroup_AgeGroupId",
                        column: x => x.AgeGroupId,
                        principalTable: "AgeGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActivityGroup_Teacher_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teacher",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Address<Teacher>",
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
                    table.PrimaryKey("PK_Address<Teacher>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address<Teacher>_Teacher_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Teacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactNumber<Teacher>",
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
                    table.PrimaryKey("PK_ContactNumber<Teacher>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactNumber<Teacher>_Teacher_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Teacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailAddress<Teacher>",
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
                    table.PrimaryKey("PK_EmailAddress<Teacher>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailAddress<Teacher>_Teacher_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Teacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Teacher, string>",
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
                    table.PrimaryKey("PK_EntityDocument<Teacher, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Teacher, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Teacher, string>_Teacher_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Teacher",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Teacher, string>",
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
                    table.PrimaryKey("PK_EntityImage<Teacher, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Teacher, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Teacher, string>_Teacher_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Teacher",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Teacher, string>",
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
                    table.PrimaryKey("PK_EntityVideo<Teacher, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Teacher, string>_Teacher_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Teacher",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Teacher, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<ListingProduct, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ListingServiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_EntityDocument<ListingProduct, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<ListingProduct, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<ListingProduct, string>_ListingProduct_EntityId",
                        column: x => x.EntityId,
                        principalTable: "ListingProduct",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<ListingProduct, string>_ListingService_ListingServiceId",
                        column: x => x.ListingServiceId,
                        principalTable: "ListingService",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<ListingProduct, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Selector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ListingServiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_EntityImage<ListingProduct, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<ListingProduct, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<ListingProduct, string>_ListingProduct_EntityId",
                        column: x => x.EntityId,
                        principalTable: "ListingProduct",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<ListingProduct, string>_ListingService_ListingServiceId",
                        column: x => x.ListingServiceId,
                        principalTable: "ListingService",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<ListingProduct, string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ListingServiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_EntityVideo<ListingProduct, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<ListingProduct, string>_ListingProduct_EntityId",
                        column: x => x.EntityId,
                        principalTable: "ListingProduct",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<ListingProduct, string>_ListingService_ListingServiceId",
                        column: x => x.ListingServiceId,
                        principalTable: "ListingService",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<ListingProduct, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppointmentRoleInvite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_AppointmentRoleInvite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentRoleInvite_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentRoleInvite_UserRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentUserInvite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_AppointmentUserInvite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentUserInvite_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentUserInvite_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Address<SalesOrder>",
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
                    table.PrimaryKey("PK_Address<SalesOrder>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address<SalesOrder>_SalesOrder_EntityId",
                        column: x => x.EntityId,
                        principalTable: "SalesOrder",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalesOrderDetail",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Processed = table.Column<bool>(type: "bit", nullable: false),
                    Qty = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    VatRate = table.Column<double>(type: "float", nullable: false),
                    ProfitPercentage = table.Column<double>(type: "float", nullable: false),
                    CustomerDiscountPercentage = table.Column<double>(type: "float", nullable: false),
                    ResellerDiscountPercentage = table.Column<double>(type: "float", nullable: false),
                    ResellerProfitPercentage = table.Column<double>(type: "float", nullable: false),
                    ProductDiscount = table.Column<double>(type: "float", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SalesOrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_SalesOrderDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrderDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrderDetail_SalesOrder_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalTable: "SalesOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrderPayment",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AmmountAllocated = table.Column<double>(type: "float", nullable: false),
                    SalesOrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_SalesOrderPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrderPayment_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesOrderPayment_SalesOrder_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalTable: "SalesOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityGroupTeamMember",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActivityGroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LearnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_ActivityGroupTeamMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityGroupTeamMember_ActivityGroup_ActivityGroupId",
                        column: x => x.ActivityGroupId,
                        principalTable: "ActivityGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityGroupTeamMember_Learner_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityCategory<ActivityGroup>",
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
                    table.PrimaryKey("PK_EntityCategory<ActivityGroup>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityCategory<ActivityGroup>_ActivityGroup_EntityId",
                        column: x => x.EntityId,
                        principalTable: "ActivityGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityCategory<ActivityGroup>_Category<ActivityGroup>_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category<ActivityGroup>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<ActivityGroup, string>",
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
                    table.PrimaryKey("PK_EntityDocument<ActivityGroup, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<ActivityGroup, string>_ActivityGroup_EntityId",
                        column: x => x.EntityId,
                        principalTable: "ActivityGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<ActivityGroup, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<ActivityGroup, string>",
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
                    table.PrimaryKey("PK_EntityImage<ActivityGroup, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<ActivityGroup, string>_ActivityGroup_EntityId",
                        column: x => x.EntityId,
                        principalTable: "ActivityGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<ActivityGroup, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<ActivityGroup, string>",
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
                    table.PrimaryKey("PK_EntityVideo<ActivityGroup, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<ActivityGroup, string>_ActivityGroup_EntityId",
                        column: x => x.EntityId,
                        principalTable: "ActivityGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<ActivityGroup, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalesOrderDetailMetaData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SalesOrderDetailId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_SalesOrderDetailMetaData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrderDetailMetaData_SalesOrderDetail_SalesOrderDetailId",
                        column: x => x.SalesOrderDetailId,
                        principalTable: "SalesOrderDetail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Address<Event<ActivityGroup>>",
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
                    table.PrimaryKey("PK_Address<Event<ActivityGroup>>", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Event<ActivityGroup>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RecurrenceRule = table.Column<int>(type: "int", nullable: false),
                    Heading = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Featured = table.Column<bool>(type: "bit", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LocationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    GoogleMapLink = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Lat = table.Column<double>(type: "float", nullable: true),
                    Lng = table.Column<double>(type: "float", nullable: true),
                    Published = table.Column<bool>(type: "bit", nullable: true),
                    HomeEvent = table.Column<bool>(type: "bit", nullable: true),
                    TransportConsentRequired = table.Column<bool>(type: "bit", nullable: true),
                    AttendanceConsentRequired = table.Column<bool>(type: "bit", nullable: true),
                    TakeAttendance = table.Column<bool>(type: "bit", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    DocumentLinks = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
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
                    table.PrimaryKey("PK_Event<ActivityGroup>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Event<ActivityGroup>_ActivityGroup_EntityId",
                        column: x => x.EntityId,
                        principalTable: "ActivityGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Event<ActivityGroup>_Address<Event<ActivityGroup>>_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Address<Event<ActivityGroup>>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContactNumber<Event<ActivityGroup>>",
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
                    table.PrimaryKey("PK_ContactNumber<Event<ActivityGroup>>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactNumber<Event<ActivityGroup>>_Event<ActivityGroup>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Event<ActivityGroup>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityCategory<Event<ActivityGroup>>",
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
                    table.PrimaryKey("PK_EntityCategory<Event<ActivityGroup>>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityCategory<Event<ActivityGroup>>_Category<Event<ActivityGroup>>_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category<Event<ActivityGroup>>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityCategory<Event<ActivityGroup>>_Event<ActivityGroup>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Event<ActivityGroup>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Event<ActivityGroup>, string>",
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
                    table.PrimaryKey("PK_EntityDocument<Event<ActivityGroup>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Event<ActivityGroup>, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Event<ActivityGroup>, string>_Event<ActivityGroup>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Event<ActivityGroup>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Event<ActivityGroup>, string>",
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
                    table.PrimaryKey("PK_EntityImage<Event<ActivityGroup>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Event<ActivityGroup>, string>_Event<ActivityGroup>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Event<ActivityGroup>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Event<ActivityGroup>, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Event<ActivityGroup>, string>",
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
                    table.PrimaryKey("PK_EntityVideo<Event<ActivityGroup>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Event<ActivityGroup>, string>_Event<ActivityGroup>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Event<ActivityGroup>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Event<ActivityGroup>, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Address<Event<Category<ActivityGroup>>>",
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
                    table.PrimaryKey("PK_Address<Event<Category<ActivityGroup>>>", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Event<Category<ActivityGroup>>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RecurrenceRule = table.Column<int>(type: "int", nullable: false),
                    Heading = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Featured = table.Column<bool>(type: "bit", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LocationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    GoogleMapLink = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Lat = table.Column<double>(type: "float", nullable: true),
                    Lng = table.Column<double>(type: "float", nullable: true),
                    Published = table.Column<bool>(type: "bit", nullable: true),
                    HomeEvent = table.Column<bool>(type: "bit", nullable: true),
                    TransportConsentRequired = table.Column<bool>(type: "bit", nullable: true),
                    AttendanceConsentRequired = table.Column<bool>(type: "bit", nullable: true),
                    TakeAttendance = table.Column<bool>(type: "bit", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    DocumentLinks = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
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
                    table.PrimaryKey("PK_Event<Category<ActivityGroup>>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Event<Category<ActivityGroup>>_Address<Event<Category<ActivityGroup>>>_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Address<Event<Category<ActivityGroup>>>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Event<Category<ActivityGroup>>_Category<ActivityGroup>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Category<ActivityGroup>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContactNumber<Event<Category<ActivityGroup>>>",
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
                    table.PrimaryKey("PK_ContactNumber<Event<Category<ActivityGroup>>>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactNumber<Event<Category<ActivityGroup>>>_Event<Category<ActivityGroup>>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Event<Category<ActivityGroup>>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityCategory<Event<Category<ActivityGroup>>>",
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
                    table.PrimaryKey("PK_EntityCategory<Event<Category<ActivityGroup>>>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityCategory<Event<Category<ActivityGroup>>>_Category<Event<Category<ActivityGroup>>>_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category<Event<Category<ActivityGroup>>>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityCategory<Event<Category<ActivityGroup>>>_Event<Category<ActivityGroup>>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Event<Category<ActivityGroup>>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Event<Category<ActivityGroup>>, string>",
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
                    table.PrimaryKey("PK_EntityDocument<Event<Category<ActivityGroup>>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Event<Category<ActivityGroup>>, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Event<Category<ActivityGroup>>, string>_Event<Category<ActivityGroup>>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Event<Category<ActivityGroup>>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Event<Category<ActivityGroup>>, string>",
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
                    table.PrimaryKey("PK_EntityImage<Event<Category<ActivityGroup>>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Event<Category<ActivityGroup>>, string>_Event<Category<ActivityGroup>>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Event<Category<ActivityGroup>>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Event<Category<ActivityGroup>>, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Event<Category<ActivityGroup>>, string>",
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
                    table.PrimaryKey("PK_EntityVideo<Event<Category<ActivityGroup>>, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Event<Category<ActivityGroup>>, string>_Event<Category<ActivityGroup>>_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Event<Category<ActivityGroup>>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Event<Category<ActivityGroup>>, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EventRegistration",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attendees = table.Column<int>(type: "int", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventCategoryActivityGroupId = table.Column<string>(name: "Event<Category<ActivityGroup>>Id", type: "nvarchar(450)", nullable: true),
                    EventId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_EventRegistration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventRegistration_Event<ActivityGroup>_EventId1",
                        column: x => x.EventId1,
                        principalTable: "Event<ActivityGroup>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EventRegistration_Event<Category<ActivityGroup>>_Event<Category<ActivityGroup>>Id",
                        column: x => x.EventCategoryActivityGroupId,
                        principalTable: "Event<Category<ActivityGroup>>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReadTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentLinks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageType = table.Column<int>(type: "int", nullable: false),
                    Public = table.Column<bool>(type: "bit", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityGroupId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LearnerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SchoolEventCategoryActivityGroupId = table.Column<string>(name: "SchoolEvent<Category<ActivityGroup>>Id", type: "nvarchar(450)", nullable: true),
                    SchoolEventId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_ActivityGroup_ActivityGroupId",
                        column: x => x.ActivityGroupId,
                        principalTable: "ActivityGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_Event<ActivityGroup>_SchoolEventId1",
                        column: x => x.SchoolEventId1,
                        principalTable: "Event<ActivityGroup>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_Event<Category<ActivityGroup>>_SchoolEvent<Category<ActivityGroup>>Id",
                        column: x => x.SchoolEventCategoryActivityGroupId,
                        principalTable: "Event<Category<ActivityGroup>>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_Learner_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learner",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ParticipatingActivityGroup",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActivityGroupId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EventId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SchoolEventActivityGroupId = table.Column<string>(name: "SchoolEvent<ActivityGroup>Id", type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_ParticipatingActivityGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParticipatingActivityGroup_ActivityGroup_ActivityGroupId",
                        column: x => x.ActivityGroupId,
                        principalTable: "ActivityGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParticipatingActivityGroup_Event<ActivityGroup>_SchoolEvent<ActivityGroup>Id",
                        column: x => x.SchoolEventActivityGroupId,
                        principalTable: "Event<ActivityGroup>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParticipatingActivityGroup_Event<Category<ActivityGroup>>_EventId",
                        column: x => x.EventId,
                        principalTable: "Event<Category<ActivityGroup>>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ParticipatingActivityGroupCategory",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActivityGroupCategoryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ActivityCategoryParentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SchoolEventActivityGroupId = table.Column<string>(name: "SchoolEvent<ActivityGroup>Id", type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_ParticipatingActivityGroupCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParticipatingActivityGroupCategory_Category<ActivityGroup>_ActivityGroupCategoryId",
                        column: x => x.ActivityGroupCategoryId,
                        principalTable: "Category<ActivityGroup>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParticipatingActivityGroupCategory_Event<ActivityGroup>_SchoolEvent<ActivityGroup>Id",
                        column: x => x.SchoolEventActivityGroupId,
                        principalTable: "Event<ActivityGroup>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParticipatingActivityGroupCategory_Event<Category<ActivityGroup>>_EventId",
                        column: x => x.EventId,
                        principalTable: "Event<Category<ActivityGroup>>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SchoolEventTicketType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EventId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    QuantityAvailable = table.Column<int>(type: "int", nullable: false),
                    QuantitySold = table.Column<int>(type: "int", nullable: false),
                    SchoolEventCategoryActivityGroupId = table.Column<string>(name: "SchoolEvent<Category<ActivityGroup>>Id", type: "nvarchar(450)", nullable: true),
                    SchoolEventId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_SchoolEventTicketType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolEventTicketType_Event<ActivityGroup>_SchoolEventId1",
                        column: x => x.SchoolEventId1,
                        principalTable: "Event<ActivityGroup>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SchoolEventTicketType_Event<Category<ActivityGroup>>_SchoolEvent<Category<ActivityGroup>>Id",
                        column: x => x.SchoolEventCategoryActivityGroupId,
                        principalTable: "Event<Category<ActivityGroup>>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SchoolEventViews",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SchoolEventId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SchoolEventId2 = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_SchoolEventViews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolEventViews_Event<ActivityGroup>_SchoolEventId",
                        column: x => x.SchoolEventId,
                        principalTable: "Event<ActivityGroup>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolEventViews_Event<Category<ActivityGroup>>_SchoolEventId2",
                        column: x => x.SchoolEventId2,
                        principalTable: "Event<Category<ActivityGroup>>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SchoolEventViews_UserInfo_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventDay",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EventRegistrationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EventCategoryActivityGroupId = table.Column<string>(name: "Event<Category<ActivityGroup>>Id", type: "nvarchar(450)", nullable: true),
                    EventId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_EventDay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventDay_Event<ActivityGroup>_EventId1",
                        column: x => x.EventId1,
                        principalTable: "Event<ActivityGroup>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EventDay_Event<Category<ActivityGroup>>_Event<Category<ActivityGroup>>Id",
                        column: x => x.EventCategoryActivityGroupId,
                        principalTable: "Event<Category<ActivityGroup>>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EventDay_EventRegistration_EventRegistrationId",
                        column: x => x.EventRegistrationId,
                        principalTable: "EventRegistration",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument<Message, string>",
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
                    table.PrimaryKey("PK_EntityDocument<Message, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument<Message, string>_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityDocument<Message, string>_Message_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Message",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityImage<Message, string>",
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
                    table.PrimaryKey("PK_EntityImage<Message, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityImage<Message, string>_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityImage<Message, string>_Message_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Message",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntityVideo<Message, string>",
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
                    table.PrimaryKey("PK_EntityVideo<Message, string>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityVideo<Message, string>_Message_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Message",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntityVideo<Message, string>_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ParentPermission",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConsentType = table.Column<int>(type: "int", nullable: false),
                    ConsentDirection = table.Column<int>(type: "int", nullable: true),
                    Granted = table.Column<bool>(type: "bit", nullable: false),
                    ParentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LearnerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EventId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ParticipatingActivityGroupId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SchoolEventActivityGroupId = table.Column<string>(name: "SchoolEvent<ActivityGroup>Id", type: "nvarchar(450)", nullable: true),
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
                    table.PrimaryKey("PK_ParentPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentPermission_Event<ActivityGroup>_SchoolEvent<ActivityGroup>Id",
                        column: x => x.SchoolEventActivityGroupId,
                        principalTable: "Event<ActivityGroup>",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParentPermission_Event<Category<ActivityGroup>>_EventId",
                        column: x => x.EventId,
                        principalTable: "Event<Category<ActivityGroup>>",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentPermission_Learner_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentPermission_Parent_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentPermission_ParticipatingActivityGroup_ParticipatingActivityGroupId",
                        column: x => x.ParticipatingActivityGroupId,
                        principalTable: "ParticipatingActivityGroup",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ParticipatingActitivityGroupTeamMember",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParticipatingActitivityGroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeamMemberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_ParticipatingActitivityGroupTeamMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParticipatingActitivityGroupTeamMember_Learner_TeamMemberId",
                        column: x => x.TeamMemberId,
                        principalTable: "Learner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParticipatingActitivityGroupTeamMember_ParticipatingActivityGroup_ParticipatingActitivityGroupId",
                        column: x => x.ParticipatingActitivityGroupId,
                        principalTable: "ParticipatingActivityGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityGroup_AgeGroupId",
                table: "ActivityGroup",
                column: "AgeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityGroup_TeacherId",
                table: "ActivityGroup",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityGroupTeamMember_ActivityGroupId",
                table: "ActivityGroupTeamMember",
                column: "ActivityGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityGroupTeamMember_LearnerId",
                table: "ActivityGroupTeamMember",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Address<Event<ActivityGroup>>_EntityId",
                table: "Address<Event<ActivityGroup>>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Address<Event<Category<ActivityGroup>>>_EntityId",
                table: "Address<Event<Category<ActivityGroup>>>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Address<Parent>_EntityId",
                table: "Address<Parent>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Address<SalesOrder>_EntityId",
                table: "Address<SalesOrder>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Address<Teacher>_EntityId",
                table: "Address<Teacher>",
                column: "EntityId",
                unique: true,
                filter: "[EntityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Address<UserInfo>_EntityId",
                table: "Address<UserInfo>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_AdvertisementTierId",
                table: "Advertisement",
                column: "AdvertisementTierId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_UserId",
                table: "Advertisement",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_UserId",
                table: "Appointment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentRoleInvite_AppointmentId",
                table: "AppointmentRoleInvite",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentRoleInvite_RoleId",
                table: "AppointmentRoleInvite",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentUserInvite_AppointmentId",
                table: "AppointmentUserInvite",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentUserInvite_UserId",
                table: "AppointmentUserInvite",
                column: "UserId");

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
                name: "IX_AttendanceRecord_GroupId",
                table: "AttendanceRecord",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecord_LearnerId",
                table: "AttendanceRecord",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogEntryView_BlogPostId",
                table: "BlogEntryView",
                column: "BlogPostId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogEntryView_UserId",
                table: "BlogEntryView",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessListing_ListingTierId",
                table: "BusinessListing",
                column: "ListingTierId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessListing_UserId",
                table: "BusinessListing",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Category<ActivityGroup>_ParentCategoryId",
                table: "Category<ActivityGroup>",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Category<BlogPost>_ParentCategoryId",
                table: "Category<BlogPost>",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Category<BusinessListing>_ParentCategoryId",
                table: "Category<BusinessListing>",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Category<Event<ActivityGroup>>_ParentCategoryId",
                table: "Category<Event<ActivityGroup>>",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Category<Event<Category<ActivityGroup>>>_ParentCategoryId",
                table: "Category<Event<Category<ActivityGroup>>>",
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
                name: "IX_Comment<BlogPost>_EntityId",
                table: "Comment<BlogPost>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment<BlogPost>_ParentCommentId",
                table: "Comment<BlogPost>",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactNumber<Event<ActivityGroup>>_EntityId",
                table: "ContactNumber<Event<ActivityGroup>>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactNumber<Event<Category<ActivityGroup>>>_EntityId",
                table: "ContactNumber<Event<Category<ActivityGroup>>>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactNumber<Learner>_EntityId",
                table: "ContactNumber<Learner>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactNumber<Parent>_EntityId",
                table: "ContactNumber<Parent>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactNumber<Teacher>_EntityId",
                table: "ContactNumber<Teacher>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactNumber<UserInfo>_EntityId",
                table: "ContactNumber<UserInfo>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_DisciplinaryAction_SeverityScaleId",
                table: "DisciplinaryAction",
                column: "SeverityScaleId");

            migrationBuilder.CreateIndex(
                name: "IX_DisciplinaryIncident_DisciplinaryActionId",
                table: "DisciplinaryIncident",
                column: "DisciplinaryActionId");

            migrationBuilder.CreateIndex(
                name: "IX_DisciplinaryIncident_LearnerId",
                table: "DisciplinaryIncident",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAddress<Learner>_EntityId",
                table: "EmailAddress<Learner>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAddress<Parent>_EntityId",
                table: "EmailAddress<Parent>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAddress<Teacher>_EntityId",
                table: "EmailAddress<Teacher>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAddress<UserInfo>_EntityId",
                table: "EmailAddress<UserInfo>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCategory<ActivityGroup>_CategoryId",
                table: "EntityCategory<ActivityGroup>",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCategory<ActivityGroup>_EntityId",
                table: "EntityCategory<ActivityGroup>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCategory<BlogPost>_CategoryId",
                table: "EntityCategory<BlogPost>",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCategory<BusinessListing>_CategoryId",
                table: "EntityCategory<BusinessListing>",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCategory<BusinessListing>_EntityId",
                table: "EntityCategory<BusinessListing>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCategory<Event<ActivityGroup>>_CategoryId",
                table: "EntityCategory<Event<ActivityGroup>>",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCategory<Event<ActivityGroup>>_EntityId",
                table: "EntityCategory<Event<ActivityGroup>>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCategory<Event<Category<ActivityGroup>>>_CategoryId",
                table: "EntityCategory<Event<Category<ActivityGroup>>>",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCategory<Event<Category<ActivityGroup>>>_EntityId",
                table: "EntityCategory<Event<Category<ActivityGroup>>>",
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
                name: "IX_EntityDocument<ActivityGroup, string>_DocumentId",
                table: "EntityDocument<ActivityGroup, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<ActivityGroup, string>_EntityId",
                table: "EntityDocument<ActivityGroup, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Advertisement, string>_DocumentId",
                table: "EntityDocument<Advertisement, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Advertisement, string>_EntityId",
                table: "EntityDocument<Advertisement, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<AdvertisementTier, string>_DocumentId",
                table: "EntityDocument<AdvertisementTier, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<AdvertisementTier, string>_EntityId",
                table: "EntityDocument<AdvertisementTier, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Affiliate, string>_DocumentId",
                table: "EntityDocument<Affiliate, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Affiliate, string>_EntityId",
                table: "EntityDocument<Affiliate, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<BlogPost, string>_DocumentId",
                table: "EntityDocument<BlogPost, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<BlogPost, string>_EntityId",
                table: "EntityDocument<BlogPost, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<BusinessListing, string>_DocumentId",
                table: "EntityDocument<BusinessListing, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<BusinessListing, string>_EntityId",
                table: "EntityDocument<BusinessListing, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Category<ActivityGroup>, string>_DocumentId",
                table: "EntityDocument<Category<ActivityGroup>, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Category<ActivityGroup>, string>_EntityId",
                table: "EntityDocument<Category<ActivityGroup>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Category<BlogPost>, string>_DocumentId",
                table: "EntityDocument<Category<BlogPost>, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Category<BlogPost>, string>_EntityId",
                table: "EntityDocument<Category<BlogPost>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Category<BusinessListing>, string>_DocumentId",
                table: "EntityDocument<Category<BusinessListing>, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Category<BusinessListing>, string>_EntityId",
                table: "EntityDocument<Category<BusinessListing>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Category<Event<ActivityGroup>>, string>_DocumentId",
                table: "EntityDocument<Category<Event<ActivityGroup>>, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Category<Event<ActivityGroup>>, string>_EntityId",
                table: "EntityDocument<Category<Event<ActivityGroup>>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Category<Event<Category<ActivityGroup>>>, string>_DocumentId",
                table: "EntityDocument<Category<Event<Category<ActivityGroup>>>, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Category<Event<Category<ActivityGroup>>>, string>_EntityId",
                table: "EntityDocument<Category<Event<Category<ActivityGroup>>>, string>",
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
                name: "IX_EntityDocument<Event<ActivityGroup>, string>_DocumentId",
                table: "EntityDocument<Event<ActivityGroup>, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Event<ActivityGroup>, string>_EntityId",
                table: "EntityDocument<Event<ActivityGroup>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Event<Category<ActivityGroup>>, string>_DocumentId",
                table: "EntityDocument<Event<Category<ActivityGroup>>, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Event<Category<ActivityGroup>>, string>_EntityId",
                table: "EntityDocument<Event<Category<ActivityGroup>>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Learner, string>_DocumentId",
                table: "EntityDocument<Learner, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Learner, string>_EntityId",
                table: "EntityDocument<Learner, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<ListingProduct, string>_DocumentId",
                table: "EntityDocument<ListingProduct, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<ListingProduct, string>_EntityId",
                table: "EntityDocument<ListingProduct, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<ListingProduct, string>_ListingServiceId",
                table: "EntityDocument<ListingProduct, string>",
                column: "ListingServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<ListingTier, string>_DocumentId",
                table: "EntityDocument<ListingTier, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<ListingTier, string>_EntityId",
                table: "EntityDocument<ListingTier, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Message, string>_DocumentId",
                table: "EntityDocument<Message, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Message, string>_EntityId",
                table: "EntityDocument<Message, string>",
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
                name: "IX_EntityDocument<Parent, string>_DocumentId",
                table: "EntityDocument<Parent, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Parent, string>_EntityId",
                table: "EntityDocument<Parent, string>",
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
                name: "IX_EntityDocument<ServiceTier, string>_DocumentId",
                table: "EntityDocument<ServiceTier, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<ServiceTier, string>_EntityId",
                table: "EntityDocument<ServiceTier, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Teacher, string>_DocumentId",
                table: "EntityDocument<Teacher, string>",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument<Teacher, string>_EntityId",
                table: "EntityDocument<Teacher, string>",
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
                name: "IX_EntityImage<ActivityGroup, string>_EntityId",
                table: "EntityImage<ActivityGroup, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<ActivityGroup, string>_ImageId",
                table: "EntityImage<ActivityGroup, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Advertisement, string>_EntityId",
                table: "EntityImage<Advertisement, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Advertisement, string>_ImageId",
                table: "EntityImage<Advertisement, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<AdvertisementTier, string>_EntityId",
                table: "EntityImage<AdvertisementTier, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<AdvertisementTier, string>_ImageId",
                table: "EntityImage<AdvertisementTier, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Affiliate, string>_EntityId",
                table: "EntityImage<Affiliate, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Affiliate, string>_ImageId",
                table: "EntityImage<Affiliate, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<BlogPost, string>_EntityId",
                table: "EntityImage<BlogPost, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<BlogPost, string>_ImageId",
                table: "EntityImage<BlogPost, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<BusinessListing, string>_EntityId",
                table: "EntityImage<BusinessListing, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<BusinessListing, string>_ImageId",
                table: "EntityImage<BusinessListing, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Category<ActivityGroup>, string>_EntityId",
                table: "EntityImage<Category<ActivityGroup>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Category<ActivityGroup>, string>_ImageId",
                table: "EntityImage<Category<ActivityGroup>, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Category<BlogPost>, string>_EntityId",
                table: "EntityImage<Category<BlogPost>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Category<BlogPost>, string>_ImageId",
                table: "EntityImage<Category<BlogPost>, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Category<BusinessListing>, string>_EntityId",
                table: "EntityImage<Category<BusinessListing>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Category<BusinessListing>, string>_ImageId",
                table: "EntityImage<Category<BusinessListing>, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Category<Event<ActivityGroup>>, string>_EntityId",
                table: "EntityImage<Category<Event<ActivityGroup>>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Category<Event<ActivityGroup>>, string>_ImageId",
                table: "EntityImage<Category<Event<ActivityGroup>>, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Category<Event<Category<ActivityGroup>>>, string>_EntityId",
                table: "EntityImage<Category<Event<Category<ActivityGroup>>>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Category<Event<Category<ActivityGroup>>>, string>_ImageId",
                table: "EntityImage<Category<Event<Category<ActivityGroup>>>, string>",
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
                name: "IX_EntityImage<Event<ActivityGroup>, string>_EntityId",
                table: "EntityImage<Event<ActivityGroup>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Event<ActivityGroup>, string>_ImageId",
                table: "EntityImage<Event<ActivityGroup>, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Event<Category<ActivityGroup>>, string>_EntityId",
                table: "EntityImage<Event<Category<ActivityGroup>>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Event<Category<ActivityGroup>>, string>_ImageId",
                table: "EntityImage<Event<Category<ActivityGroup>>, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Learner, string>_EntityId",
                table: "EntityImage<Learner, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Learner, string>_ImageId",
                table: "EntityImage<Learner, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<ListingProduct, string>_EntityId",
                table: "EntityImage<ListingProduct, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<ListingProduct, string>_ImageId",
                table: "EntityImage<ListingProduct, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<ListingProduct, string>_ListingServiceId",
                table: "EntityImage<ListingProduct, string>",
                column: "ListingServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<ListingTier, string>_EntityId",
                table: "EntityImage<ListingTier, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<ListingTier, string>_ImageId",
                table: "EntityImage<ListingTier, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Message, string>_EntityId",
                table: "EntityImage<Message, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Message, string>_ImageId",
                table: "EntityImage<Message, string>",
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
                name: "IX_EntityImage<Parent, string>_EntityId",
                table: "EntityImage<Parent, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Parent, string>_ImageId",
                table: "EntityImage<Parent, string>",
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
                name: "IX_EntityImage<ServiceTier, string>_EntityId",
                table: "EntityImage<ServiceTier, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<ServiceTier, string>_ImageId",
                table: "EntityImage<ServiceTier, string>",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Teacher, string>_EntityId",
                table: "EntityImage<Teacher, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityImage<Teacher, string>_ImageId",
                table: "EntityImage<Teacher, string>",
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
                name: "IX_EntityVideo<ActivityGroup, string>_EntityId",
                table: "EntityVideo<ActivityGroup, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<ActivityGroup, string>_VideoId",
                table: "EntityVideo<ActivityGroup, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Advertisement, string>_EntityId",
                table: "EntityVideo<Advertisement, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Advertisement, string>_VideoId",
                table: "EntityVideo<Advertisement, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<AdvertisementTier, string>_EntityId",
                table: "EntityVideo<AdvertisementTier, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<AdvertisementTier, string>_VideoId",
                table: "EntityVideo<AdvertisementTier, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Affiliate, string>_EntityId",
                table: "EntityVideo<Affiliate, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Affiliate, string>_VideoId",
                table: "EntityVideo<Affiliate, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<BlogPost, string>_EntityId",
                table: "EntityVideo<BlogPost, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<BlogPost, string>_VideoId",
                table: "EntityVideo<BlogPost, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<BusinessListing, string>_EntityId",
                table: "EntityVideo<BusinessListing, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<BusinessListing, string>_VideoId",
                table: "EntityVideo<BusinessListing, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Category<ActivityGroup>, string>_EntityId",
                table: "EntityVideo<Category<ActivityGroup>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Category<ActivityGroup>, string>_VideoId",
                table: "EntityVideo<Category<ActivityGroup>, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Category<BlogPost>, string>_EntityId",
                table: "EntityVideo<Category<BlogPost>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Category<BlogPost>, string>_VideoId",
                table: "EntityVideo<Category<BlogPost>, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Category<BusinessListing>, string>_EntityId",
                table: "EntityVideo<Category<BusinessListing>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Category<BusinessListing>, string>_VideoId",
                table: "EntityVideo<Category<BusinessListing>, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Category<Event<ActivityGroup>>, string>_EntityId",
                table: "EntityVideo<Category<Event<ActivityGroup>>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Category<Event<ActivityGroup>>, string>_VideoId",
                table: "EntityVideo<Category<Event<ActivityGroup>>, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Category<Event<Category<ActivityGroup>>>, string>_EntityId",
                table: "EntityVideo<Category<Event<Category<ActivityGroup>>>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Category<Event<Category<ActivityGroup>>>, string>_VideoId",
                table: "EntityVideo<Category<Event<Category<ActivityGroup>>>, string>",
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
                name: "IX_EntityVideo<Event<ActivityGroup>, string>_EntityId",
                table: "EntityVideo<Event<ActivityGroup>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Event<ActivityGroup>, string>_VideoId",
                table: "EntityVideo<Event<ActivityGroup>, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Event<Category<ActivityGroup>>, string>_EntityId",
                table: "EntityVideo<Event<Category<ActivityGroup>>, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Event<Category<ActivityGroup>>, string>_VideoId",
                table: "EntityVideo<Event<Category<ActivityGroup>>, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Learner, string>_EntityId",
                table: "EntityVideo<Learner, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Learner, string>_VideoId",
                table: "EntityVideo<Learner, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<ListingProduct, string>_EntityId",
                table: "EntityVideo<ListingProduct, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<ListingProduct, string>_ListingServiceId",
                table: "EntityVideo<ListingProduct, string>",
                column: "ListingServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<ListingProduct, string>_VideoId",
                table: "EntityVideo<ListingProduct, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<ListingTier, string>_EntityId",
                table: "EntityVideo<ListingTier, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<ListingTier, string>_VideoId",
                table: "EntityVideo<ListingTier, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Message, string>_EntityId",
                table: "EntityVideo<Message, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Message, string>_VideoId",
                table: "EntityVideo<Message, string>",
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
                name: "IX_EntityVideo<Parent, string>_EntityId",
                table: "EntityVideo<Parent, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Parent, string>_VideoId",
                table: "EntityVideo<Parent, string>",
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
                name: "IX_EntityVideo<ServiceTier, string>_EntityId",
                table: "EntityVideo<ServiceTier, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<ServiceTier, string>_VideoId",
                table: "EntityVideo<ServiceTier, string>",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Teacher, string>_EntityId",
                table: "EntityVideo<Teacher, string>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityVideo<Teacher, string>_VideoId",
                table: "EntityVideo<Teacher, string>",
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
                name: "IX_Event<ActivityGroup>_EntityId",
                table: "Event<ActivityGroup>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Event<ActivityGroup>_LocationId",
                table: "Event<ActivityGroup>",
                column: "LocationId",
                unique: true,
                filter: "[LocationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Event<Category<ActivityGroup>>_EntityId",
                table: "Event<Category<ActivityGroup>>",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Event<Category<ActivityGroup>>_LocationId",
                table: "Event<Category<ActivityGroup>>",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_EventDay_Event<Category<ActivityGroup>>Id",
                table: "EventDay",
                column: "Event<Category<ActivityGroup>>Id");

            migrationBuilder.CreateIndex(
                name: "IX_EventDay_EventId1",
                table: "EventDay",
                column: "EventId1");

            migrationBuilder.CreateIndex(
                name: "IX_EventDay_EventRegistrationId",
                table: "EventDay",
                column: "EventRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistration_Event<Category<ActivityGroup>>Id",
                table: "EventRegistration",
                column: "Event<Category<ActivityGroup>>Id");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistration_EventId1",
                table: "EventRegistration",
                column: "EventId1");

            migrationBuilder.CreateIndex(
                name: "IX_Follower_UserFollowingId",
                table: "Follower",
                column: "UserFollowingId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetail_InvoiceId",
                table: "InvoiceDetail",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetail_ProductId",
                table: "InvoiceDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Learner_MedicalAidParentId",
                table: "Learner",
                column: "MedicalAidParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Learner_SchoolClassId",
                table: "Learner",
                column: "SchoolClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Learner_SchoolGradeId",
                table: "Learner",
                column: "SchoolGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_LearnerParent_LearnerId",
                table: "LearnerParent",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_LearnerParent_ParentId",
                table: "LearnerParent",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingProduct_ListingId",
                table: "ListingProduct",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingService_ListingId",
                table: "ListingService",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ActivityGroupId",
                table: "Message",
                column: "ActivityGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_LearnerId",
                table: "Message",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SchoolEvent<Category<ActivityGroup>>Id",
                table: "Message",
                column: "SchoolEvent<Category<ActivityGroup>>Id");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SchoolEventId1",
                table: "Message",
                column: "SchoolEventId1");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_SchoolClassId",
                table: "Notification",
                column: "SchoolClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentEmergencyContact_ParentId",
                table: "ParentEmergencyContact",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentPermission_EventId",
                table: "ParentPermission",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentPermission_LearnerId",
                table: "ParentPermission",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentPermission_ParentId",
                table: "ParentPermission",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentPermission_ParticipatingActivityGroupId",
                table: "ParentPermission",
                column: "ParticipatingActivityGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentPermission_SchoolEvent<ActivityGroup>Id",
                table: "ParentPermission",
                column: "SchoolEvent<ActivityGroup>Id");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipatingActitivityGroupTeamMember_ParticipatingActitivityGroupId",
                table: "ParticipatingActitivityGroupTeamMember",
                column: "ParticipatingActitivityGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipatingActitivityGroupTeamMember_TeamMemberId",
                table: "ParticipatingActitivityGroupTeamMember",
                column: "TeamMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipatingActivityGroup_ActivityGroupId",
                table: "ParticipatingActivityGroup",
                column: "ActivityGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipatingActivityGroup_EventId",
                table: "ParticipatingActivityGroup",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipatingActivityGroup_SchoolEvent<ActivityGroup>Id",
                table: "ParticipatingActivityGroup",
                column: "SchoolEvent<ActivityGroup>Id");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipatingActivityGroupCategory_ActivityGroupCategoryId",
                table: "ParticipatingActivityGroupCategory",
                column: "ActivityGroupCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipatingActivityGroupCategory_EventId",
                table: "ParticipatingActivityGroupCategory",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipatingActivityGroupCategory_SchoolEvent<ActivityGroup>Id",
                table: "ParticipatingActivityGroupCategory",
                column: "SchoolEvent<ActivityGroup>Id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_VariantParentId",
                table: "Product",
                column: "VariantParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMetadata_ProductId",
                table: "ProductMetadata",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "Identity",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrder_InvoiceId",
                table: "SalesOrder",
                column: "InvoiceId",
                unique: true,
                filter: "[InvoiceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrder_UserInfoId",
                table: "SalesOrder",
                column: "UserInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderDetail_ProductId",
                table: "SalesOrderDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderDetail_SalesOrderId",
                table: "SalesOrderDetail",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderDetailMetaData_SalesOrderDetailId",
                table: "SalesOrderDetailMetaData",
                column: "SalesOrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderPayment_PaymentId",
                table: "SalesOrderPayment",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderPayment_SalesOrderId",
                table: "SalesOrderPayment",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolClass_GradeId",
                table: "SchoolClass",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolEventTicketType_SchoolEvent<Category<ActivityGroup>>Id",
                table: "SchoolEventTicketType",
                column: "SchoolEvent<Category<ActivityGroup>>Id");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolEventTicketType_SchoolEventId1",
                table: "SchoolEventTicketType",
                column: "SchoolEventId1");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolEventViews_SchoolEventId",
                table: "SchoolEventViews",
                column: "SchoolEventId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolEventViews_SchoolEventId2",
                table: "SchoolEventViews",
                column: "SchoolEventId2");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolEventViews_UserId",
                table: "SchoolEventViews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTier_RoleId",
                table: "ServiceTier",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartCoupon_CouponId",
                table: "ShoppingCartCoupon",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItem_ParentId",
                table: "ShoppingCartItem",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItemMetadata_ShoppingCartItemId",
                table: "ShoppingCartItemMetadata",
                column: "ShoppingCartItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_GradeId",
                table: "Teacher",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_SchoolClassId",
                table: "Teacher",
                column: "SchoolClassId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Address<Event<ActivityGroup>>_Event<ActivityGroup>_EntityId",
                table: "Address<Event<ActivityGroup>>",
                column: "EntityId",
                principalTable: "Event<ActivityGroup>",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Address<Event<Category<ActivityGroup>>>_Event<Category<ActivityGroup>>_EntityId",
                table: "Address<Event<Category<ActivityGroup>>>",
                column: "EntityId",
                principalTable: "Event<Category<ActivityGroup>>",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityGroup_AgeGroup_AgeGroupId",
                table: "ActivityGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityGroup_Teacher_TeacherId",
                table: "ActivityGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_Event<ActivityGroup>_ActivityGroup_EntityId",
                table: "Event<ActivityGroup>");

            migrationBuilder.DropForeignKey(
                name: "FK_Address<Event<ActivityGroup>>_Event<ActivityGroup>_EntityId",
                table: "Address<Event<ActivityGroup>>");

            migrationBuilder.DropForeignKey(
                name: "FK_Address<Event<Category<ActivityGroup>>>_Event<Category<ActivityGroup>>_EntityId",
                table: "Address<Event<Category<ActivityGroup>>>");

            migrationBuilder.DropTable(
                name: "ActivityGroupTeamMember");

            migrationBuilder.DropTable(
                name: "Address<Parent>");

            migrationBuilder.DropTable(
                name: "Address<SalesOrder>");

            migrationBuilder.DropTable(
                name: "Address<Teacher>");

            migrationBuilder.DropTable(
                name: "Address<UserInfo>");

            migrationBuilder.DropTable(
                name: "AppointmentRoleInvite");

            migrationBuilder.DropTable(
                name: "AppointmentUserInvite");

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
                name: "AttendanceRecord");

            migrationBuilder.DropTable(
                name: "AuditTrails");

            migrationBuilder.DropTable(
                name: "BlogEntryView");

            migrationBuilder.DropTable(
                name: "Comment<BlogPost>");

            migrationBuilder.DropTable(
                name: "ContactNumber<Event<ActivityGroup>>");

            migrationBuilder.DropTable(
                name: "ContactNumber<Event<Category<ActivityGroup>>>");

            migrationBuilder.DropTable(
                name: "ContactNumber<Learner>");

            migrationBuilder.DropTable(
                name: "ContactNumber<Parent>");

            migrationBuilder.DropTable(
                name: "ContactNumber<Teacher>");

            migrationBuilder.DropTable(
                name: "ContactNumber<UserInfo>");

            migrationBuilder.DropTable(
                name: "DeviceToken");

            migrationBuilder.DropTable(
                name: "DisciplinaryIncident");

            migrationBuilder.DropTable(
                name: "Donation");

            migrationBuilder.DropTable(
                name: "EmailAddress<Learner>");

            migrationBuilder.DropTable(
                name: "EmailAddress<Parent>");

            migrationBuilder.DropTable(
                name: "EmailAddress<Teacher>");

            migrationBuilder.DropTable(
                name: "EmailAddress<UserInfo>");

            migrationBuilder.DropTable(
                name: "EntityCategory<ActivityGroup>");

            migrationBuilder.DropTable(
                name: "EntityCategory<BlogPost>");

            migrationBuilder.DropTable(
                name: "EntityCategory<BusinessListing>");

            migrationBuilder.DropTable(
                name: "EntityCategory<Event<ActivityGroup>>");

            migrationBuilder.DropTable(
                name: "EntityCategory<Event<Category<ActivityGroup>>>");

            migrationBuilder.DropTable(
                name: "EntityCategory<OfferedService>");

            migrationBuilder.DropTable(
                name: "EntityCategory<Product>");

            migrationBuilder.DropTable(
                name: "EntityDocument<ActivityGroup, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Advertisement, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<AdvertisementTier, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Affiliate, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<BlogPost, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<BusinessListing, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Category<ActivityGroup>, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Category<BlogPost>, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Category<BusinessListing>, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Category<Event<ActivityGroup>>, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Category<Event<Category<ActivityGroup>>>, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Category<OfferedService>, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Category<Product>, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Event<ActivityGroup>, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Event<Category<ActivityGroup>>, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Learner, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<ListingProduct, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<ListingTier, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Message, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<OfferedService, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Parent, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Product, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<ServiceTier, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<Teacher, string>");

            migrationBuilder.DropTable(
                name: "EntityDocument<UserInfo, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<ActivityGroup, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Advertisement, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<AdvertisementTier, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Affiliate, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<BlogPost, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<BusinessListing, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Category<ActivityGroup>, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Category<BlogPost>, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Category<BusinessListing>, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Category<Event<ActivityGroup>>, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Category<Event<Category<ActivityGroup>>>, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Category<OfferedService>, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Category<Product>, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Event<ActivityGroup>, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Event<Category<ActivityGroup>>, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Learner, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<ListingProduct, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<ListingTier, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Message, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<OfferedService, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Parent, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Product, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<ServiceTier, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<Teacher, string>");

            migrationBuilder.DropTable(
                name: "EntityImage<UserInfo, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<ActivityGroup, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Advertisement, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<AdvertisementTier, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Affiliate, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<BlogPost, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<BusinessListing, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Category<ActivityGroup>, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Category<BlogPost>, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Category<BusinessListing>, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Category<Event<ActivityGroup>>, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Category<Event<Category<ActivityGroup>>>, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Category<OfferedService>, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Category<Product>, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Event<ActivityGroup>, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Event<Category<ActivityGroup>>, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Learner, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<ListingProduct, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<ListingTier, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Message, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<OfferedService, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Parent, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Product, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<ServiceTier, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<Teacher, string>");

            migrationBuilder.DropTable(
                name: "EntityVideo<UserInfo, string>");

            migrationBuilder.DropTable(
                name: "EventDay");

            migrationBuilder.DropTable(
                name: "Follower");

            migrationBuilder.DropTable(
                name: "InvoiceDetail");

            migrationBuilder.DropTable(
                name: "LearnerParent");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "ParentEmergencyContact");

            migrationBuilder.DropTable(
                name: "ParentPermission");

            migrationBuilder.DropTable(
                name: "ParticipatingActitivityGroupTeamMember");

            migrationBuilder.DropTable(
                name: "ParticipatingActivityGroupCategory");

            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropTable(
                name: "ProductMetadata");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "SalesOrderDetailMetaData");

            migrationBuilder.DropTable(
                name: "SalesOrderPayment");

            migrationBuilder.DropTable(
                name: "SchoolEventTicketType");

            migrationBuilder.DropTable(
                name: "SchoolEventViews");

            migrationBuilder.DropTable(
                name: "ShoppingCartCoupon");

            migrationBuilder.DropTable(
                name: "ShoppingCartItemMetadata");

            migrationBuilder.DropTable(
                name: "TierService");

            migrationBuilder.DropTable(
                name: "UserAppSettings");

            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "AttendanceGroup");

            migrationBuilder.DropTable(
                name: "DisciplinaryAction");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Advertisement");

            migrationBuilder.DropTable(
                name: "Affiliate");

            migrationBuilder.DropTable(
                name: "BlogPost");

            migrationBuilder.DropTable(
                name: "Category<BlogPost>");

            migrationBuilder.DropTable(
                name: "Category<BusinessListing>");

            migrationBuilder.DropTable(
                name: "Category<Event<ActivityGroup>>");

            migrationBuilder.DropTable(
                name: "Category<Event<Category<ActivityGroup>>>");

            migrationBuilder.DropTable(
                name: "Category<OfferedService>");

            migrationBuilder.DropTable(
                name: "Category<Product>");

            migrationBuilder.DropTable(
                name: "ListingProduct");

            migrationBuilder.DropTable(
                name: "ListingService");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Video");

            migrationBuilder.DropTable(
                name: "EventRegistration");

            migrationBuilder.DropTable(
                name: "ParticipatingActivityGroup");

            migrationBuilder.DropTable(
                name: "SalesOrderDetail");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Coupon");

            migrationBuilder.DropTable(
                name: "ShoppingCartItem");

            migrationBuilder.DropTable(
                name: "OfferedService");

            migrationBuilder.DropTable(
                name: "ServiceTier");

            migrationBuilder.DropTable(
                name: "SeverityScale");

            migrationBuilder.DropTable(
                name: "AdvertisementTier");

            migrationBuilder.DropTable(
                name: "BusinessListing");

            migrationBuilder.DropTable(
                name: "Learner");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "SalesOrder");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ListingTier");

            migrationBuilder.DropTable(
                name: "Parent");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "UserInfo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "AgeGroup");

            migrationBuilder.DropTable(
                name: "Teacher");

            migrationBuilder.DropTable(
                name: "SchoolClass");

            migrationBuilder.DropTable(
                name: "SchoolGrade");

            migrationBuilder.DropTable(
                name: "ActivityGroup");

            migrationBuilder.DropTable(
                name: "Event<ActivityGroup>");

            migrationBuilder.DropTable(
                name: "Address<Event<ActivityGroup>>");

            migrationBuilder.DropTable(
                name: "Event<Category<ActivityGroup>>");

            migrationBuilder.DropTable(
                name: "Address<Event<Category<ActivityGroup>>>");

            migrationBuilder.DropTable(
                name: "Category<ActivityGroup>");
        }
    }
}
