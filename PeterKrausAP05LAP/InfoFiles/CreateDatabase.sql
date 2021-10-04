CREATE DATABASE StockGamesDatabase
GO

USE StockGamesDatabase
GO

CREATE TABLE "Manufacturer"(
	"Id"			INT NOT NULL,
	"Name"			NVARCHAR(50) NOT NULL,
	"FirstName"		NVARCHAR(50),
	"LastName"		NVARCHAR(50),
	PRIMARY KEY ("Id")
);
GO

CREATE TABLE "Category"(
	"Id"			INT NOT NULL,
	"Name"			NVARCHAR(50) NOT NULL,
	"TaxRate"		DECIMAL(19,4) NOT NULL,
	PRIMARY KEY ("Id")
);
GO


CREATE TABLE "Customer"(
	"Id"			INT NOT NULL,
	"Title"			NVARCHAR(50),
	"FirstName"		NVARCHAR(50) NOT NULL,
	"LastName"		NVARCHAR(50) NOT NULL,
	"Email"			NVARCHAR(120),
	"Street"		NVARCHAR(120) NOT NULL,
	"Zip"			NVARCHAR(50),
	"City"			NVARCHAR(50),
	"PWHash"		NVARCHAR(200) NOT NULL,
	"Salt"			NVARCHAR(100) NOT NULL,
	PRIMARY KEY ("Id")
);
GO

CREATE TABLE "Order"(
	"Id"			INT NOT NULL,
	"CustomerId"	INT NOT NULL,
	"PriceTotal"	DECIMAL(19,4),
	"DateOrdered"	DATE NOT NULL,
	"Zip"			NVARCHAR(50),
	"City"			NVARCHAR(50),
	"FirstName"		NVARCHAR(50),
	"LastName"		NVARCHAR(50),
	PRIMARY KEY ("Id"),
	CONSTRAINT "FK_CustomerOrderId" FOREIGN KEY ("CustomerId")
	REFERENCES "Customer"("Id")
);
GO

CREATE TABLE "Product"(
	"Id"			INT NOT NULL,
	"ProductName"	NVARCHAR(80) NOT NULL,
	"NetUnitPrice"	DECIMAL(19,4) NOT NULL,
	"ImagePath"		NVARCHAR(200) NOT NULL,
	"Description"	NVARCHAR(200),
	"ManufactureId" INT NOT NULL,
	"CategoryId"	INT NOT NULL,
	PRIMARY KEY ("Id"),

	CONSTRAINT "FK_ManufacturedProductId" FOREIGN KEY ("ManufactureId")
	REFERENCES "Manufacturer"("Id"),
	
	CONSTRAINT "FK_CategoryProductId" FOREIGN KEY ("CategoryId")
	REFERENCES "Category"("Id")
);
GO

CREATE TABLE "OrderLine"(
	"Id"			INT NOT NULL,
	"OrderId"		INT NOT NULL,
	"ProductId"		INT NOT NULL,
	"Amount"		DECIMAL(19,4),
	"NetUnitPrice"	DECIMAL(19,4),
	"TaxRate"		DECIMAL(19,4),
	PRIMARY KEY ("Id"),

	CONSTRAINT "FK_OrderId" FOREIGN KEY ("OrderId")
	REFERENCES "Order"("Id"),
	
	CONSTRAINT "FK_ProductOrderId" FOREIGN KEY ("ProductId")
	REFERENCES "Product"("Id")
);
GO

