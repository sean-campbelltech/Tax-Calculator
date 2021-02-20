/* Create database */
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Taxation')
BEGIN
	CREATE DATABASE Taxation;
END
GO

/* Change to the Taxation database */
USE Taxation;
GO

/* Create user */

IF NOT EXISTS(SELECT * FROM sys.server_principals WHERE name = 'TaxUser')
BEGIN
	CREATE LOGIN TaxUser WITH PASSWORD=N'TaxP@ss18013', DEFAULT_DATABASE=Taxation
END

IF NOT EXISTS(SELECT * FROM sys.database_principals WHERE name = 'TaxUser')
BEGIN
	EXEC sp_adduser 'TaxUser', 'TaxUser', 'db_owner';  
END

/* Create tables */

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TaxType' AND xtype='U')
BEGIN
	CREATE TABLE TaxType (
    		TaxTypeId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    		Description VARCHAR(255) NOT NULL
	);
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PostalCodeTax' AND xtype='U')
BEGIN
	CREATE TABLE PostalCodeTax (
    		PostalCode VARCHAR(4) NOT NULL PRIMARY KEY,
	 	TaxTypeId INT NOT NULL
    
    	CONSTRAINT FK_PostalCodeTax_TaxType FOREIGN KEY (TaxTypeId)     
    		REFERENCES dbo.TaxType (TaxTypeId)     
    		ON DELETE CASCADE    
    		ON UPDATE CASCADE
);
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TaxCalculation' AND xtype='U')
BEGIN
	CREATE TABLE TaxCalculation (
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

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ProgressiveTaxRate' AND xtype='U')
BEGIN
	CREATE TABLE ProgressiveTaxRate (
    		Rate DECIMAL(10,2) NOT NULL PRIMARY KEY,
	    	FromAmount DECIMAL(10,2) NOT NULL,
	    	ToAmount DECIMAL(10,2) NOT NULL
);
END
GO

/* Insert data */
