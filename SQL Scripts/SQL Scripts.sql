/* Create database */
IF NOT EXISTS(SELECT *
FROM sys.databases
WHERE name = 'Taxation')
BEGIN
	CREATE DATABASE Taxation;
END
GO

/* Change to the Taxation database */
USE Taxation;
GO

/* Create user */
IF NOT EXISTS(SELECT *
FROM sys.server_principals
WHERE name = 'TaxUser')
BEGIN
	CREATE LOGIN TaxUser WITH PASSWORD=N'TaxP@ss18013', DEFAULT_DATABASE=Taxation
END

IF NOT EXISTS(SELECT *
FROM sys.database_principals
WHERE name = 'TaxUser')
BEGIN
	EXEC sp_adduser 'TaxUser', 'TaxUser', 'db_owner';
END

/* Create tables */
IF NOT EXISTS (SELECT *
FROM sysobjects
WHERE name='TaxType' AND xtype='U')
BEGIN
	CREATE TABLE TaxType
	(
		TaxTypeId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		Description VARCHAR(255) NOT NULL
	);
END

IF NOT EXISTS (SELECT *
FROM sysobjects
WHERE name='PostalCodeTax' AND xtype='U')
BEGIN
	CREATE TABLE PostalCodeTax
	(
		PostalCode VARCHAR(4) NOT NULL PRIMARY KEY,
		TaxTypeId INT NOT NULL

			CONSTRAINT FK_PostalCodeTax_TaxType FOREIGN KEY (TaxTypeId)     
    		REFERENCES dbo.TaxType (TaxTypeId)     
    		ON DELETE CASCADE    
    		ON UPDATE CASCADE
	);
END

IF NOT EXISTS (SELECT *
FROM sysobjects
WHERE name='ProgressiveTaxRate' AND xtype='U')
BEGIN
	CREATE TABLE ProgressiveTaxRate
	(
		Rate DECIMAL(10,2) NOT NULL PRIMARY KEY,
		FromAmount DECIMAL(10,2) NOT NULL,
		ToAmount DECIMAL(10,2) NOT NULL
	);
END
GO

IF NOT EXISTS (SELECT *
FROM sysobjects
WHERE name='TaxCalculation' AND xtype='U')
BEGIN
	CREATE TABLE TaxCalculation
	(
		CalculationId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		CalculatedAt DATETIME2 NOT NULL,
		PostalCode VARCHAR(4) NOT NULL,
		AnnualIncome DECIMAL(10,2) NOT NULL,
		TaxAmount DECIMAL(10,2) NOT NULL,
		RequestedBy VARCHAR(50)

			CONSTRAINT FK_TaxCalculation_PostalCodeTax FOREIGN KEY (PostalCode)     
    		REFERENCES dbo.PostalCodeTax (PostalCode)     
    		ON DELETE CASCADE    
    		ON UPDATE CASCADE
	);
END

/* Insert data */
SET IDENTITY_INSERT TaxType ON

IF NOT EXISTS (SELECT *
FROM TaxType
WHERE TaxTypeId = 1)
BEGIN
	INSERT INTO TaxType
		(TaxTypeId, Description)
	VALUES
		(1, 'Progressive')
END

IF NOT EXISTS (SELECT *
FROM TaxType
WHERE TaxTypeId = 2)
BEGIN
	INSERT INTO TaxType
		(TaxTypeId, Description)
	VALUES
		(2, 'Flat Value')
END
GO

IF NOT EXISTS (SELECT *
FROM TaxType
WHERE TaxTypeId = 3)
BEGIN
	INSERT INTO TaxType
		(TaxTypeId, Description)
	VALUES
		(3, 'Flat Rate')
END
GO

SET IDENTITY_INSERT TaxType OFF

IF NOT EXISTS (SELECT *
FROM PostalCodeTax
WHERE PostalCode = '7441')
BEGIN
	INSERT INTO PostalCodeTax
		(PostalCode, TaxTypeId)
	VALUES
		('7441', 1)
END

IF NOT EXISTS (SELECT *
FROM PostalCodeTax
WHERE PostalCode = 'A100')
BEGIN
	INSERT INTO PostalCodeTax
		(PostalCode, TaxTypeId)
	VALUES
		('A100', 2)
END
GO

IF NOT EXISTS (SELECT *
FROM PostalCodeTax
WHERE PostalCode = '7000')
BEGIN
	INSERT INTO PostalCodeTax
		(PostalCode, TaxTypeId)
	VALUES
		('7000', 3)
END
GO

IF NOT EXISTS (SELECT *
FROM PostalCodeTax
WHERE PostalCode = '1000')
BEGIN
	INSERT INTO PostalCodeTax
		(PostalCode, TaxTypeId)
	VALUES
		('1000', 1)
END

IF NOT EXISTS (SELECT *
FROM ProgressiveTaxRate
WHERE Rate = 10)
BEGIN
	INSERT INTO ProgressiveTaxRate
		(Rate, FromAmount, ToAmount)
	VALUES
		(10, 0, 8350)
END

IF NOT EXISTS (SELECT *
FROM ProgressiveTaxRate
WHERE Rate = 15)
BEGIN
	INSERT INTO ProgressiveTaxRate
		(Rate, FromAmount, ToAmount)
	VALUES
		(15, 8351, 33950)
END

IF NOT EXISTS (SELECT *
FROM ProgressiveTaxRate
WHERE Rate = 25)
BEGIN
	INSERT INTO ProgressiveTaxRate
		(Rate, FromAmount, ToAmount)
	VALUES
		(25, 33951, 82250)
END

IF NOT EXISTS (SELECT *
FROM ProgressiveTaxRate
WHERE Rate = 28)
BEGIN
	INSERT INTO ProgressiveTaxRate
		(Rate, FromAmount, ToAmount)
	VALUES
		(28, 82251, 171550)
END

IF NOT EXISTS (SELECT *
FROM ProgressiveTaxRate
WHERE Rate = 33)
BEGIN
	INSERT INTO ProgressiveTaxRate
		(Rate, FromAmount, ToAmount)
	VALUES
		(33, 171551, 372950)
END

DECLARE @decimalMax DECIMAL(10,2)
SET @decimalMax = 99999999.99;

IF NOT EXISTS (SELECT *
FROM ProgressiveTaxRate
WHERE Rate = 35)
BEGIN
	INSERT INTO ProgressiveTaxRate
		(Rate, FromAmount, ToAmount)
	VALUES
		(35, 372951, @decimalMax)
END