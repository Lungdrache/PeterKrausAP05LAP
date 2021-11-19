DROP DATABASE StockGamesDatabase
GO



CREATE DATABASE StockGamesDatabase
GO

USE StockGamesDatabase
GO

CREATE TABLE "Manufacturer"(
	"Id"			INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	"Name"			NVARCHAR(50) NOT NULL,
	"FirstName"		NVARCHAR(50),
	"LastName"		NVARCHAR(50)
);
GO

CREATE TABLE "Category"(
	"Id"			INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	"Name"			NVARCHAR(50) NOT NULL,
	"TaxRate"		DECIMAL(19,4) NOT NULL,
);
GO


CREATE TABLE "Customer"(
	"Id"			INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	"Title"			NVARCHAR(50),
	"FirstName"		NVARCHAR(50) NOT NULL,
	"LastName"		NVARCHAR(50) NOT NULL,
	"Email"			NVARCHAR(120),
	"Street"		NVARCHAR(120) NOT NULL,
	"Zip"			NVARCHAR(50),
	"City"			NVARCHAR(50),
	"PWHash"		NVARCHAR(200) NOT NULL,
	"Salt"			NVARCHAR(100) NOT NULL,
);
GO

CREATE TABLE "Order"(
	"Id"			INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	"CustomerId"	INT NOT NULL,
	"PriceTotal"	DECIMAL(19,4),
	"DateOrdered"	DATE NOT NULL,
	"Zip"			NVARCHAR(50),
	"City"			NVARCHAR(50),
	"FirstName"		NVARCHAR(50),
	"LastName"		NVARCHAR(50),
	CONSTRAINT "FK_CustomerOrderId" FOREIGN KEY ("CustomerId")
	REFERENCES "Customer"("Id")
);
GO

CREATE TABLE "Product"(
	"Id"			INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	"ProductName"	NVARCHAR(80) NOT NULL,
	"NetUnitPrice"	DECIMAL(19,4) NOT NULL,
	"TrailerPath"	NVARCHAR(200) NOT NULL,
	"Description"	NVARCHAR(500),
	"ManufactureId" INT NOT NULL,
	"CategoryId"	INT NOT NULL,

	CONSTRAINT "FK_ManufacturedProductId" FOREIGN KEY ("ManufactureId")
	REFERENCES "Manufacturer"("Id"),
	
	CONSTRAINT "FK_CategoryProductId" FOREIGN KEY ("CategoryId")
	REFERENCES "Category"("Id")
);
GO

CREATE TABLE "ProductImages"(
	"Id"			INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	"ProductId"		INT NOT NULL,
	"ImagePath"		NVARCHAR(200) NOT NULL

	CONSTRAINT "FK_ProductId" FOREIGN KEY ("ProductId")
	REFERENCES "Product"("Id")
);
GO

CREATE TABLE "OrderLine"(
	"Id"			INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	"OrderId"		INT NOT NULL,
	"ProductId"		INT NOT NULL,
	"IsActive"		BIT NOT NULL,
	"Amount"		DECIMAL(19,4),
	"NetUnitPrice"	DECIMAL(19,4),
	"TaxRate"		DECIMAL(19,4)

	CONSTRAINT "FK_OrderId" FOREIGN KEY ("OrderId")
	REFERENCES "Order"("Id"),
	
	CONSTRAINT "FK_ProductOrderId" FOREIGN KEY ("ProductId")
	REFERENCES "Product"("Id")
);
GO


SELECT ImagePath
FROM ProductImages
