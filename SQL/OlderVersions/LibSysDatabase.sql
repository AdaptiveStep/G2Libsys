------------------------------------------------
	--------- G2 SYSTEMS ---------------------------
		------------------------------------------------
		-----SKAPAT AV HARIZ@KTH.se 2020  --------------
		---- NEWTONS YRKESHÖGSKOLA        --------------
		---- PROJEKTARBETE TILL AGIL KURS --------------
		------------------------------------------------
		------------------------------------------------

	------------------------------------------------------------------------
		-----KVAR ATT GÖRA------------------------------------------------------

		-- Ta bort "Batch Resetter" och använd "IF EXISTS...Replace" när tabeller definieras istället

		------------------------------------------------------------------------
	------------------------------------------------------------------------

------------------------------------------------------------------------
--- SQL RESET ------------------------------------------------
	------------------------------------------------------------------------



		USE [master]
		GO
		ALTER DATABASE [G2LibSys] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
		GO
		USE [master]
		GO
		DROP DATABASE [G2LibSys]
		GO
		----------- WARNING

		--Skapas en helt ny Tom G2Systems Databas.
		GO 
		USE [master]
		GO
		Create Database G2LibSys
		GO
		USE G2LibSys
		GO

------------------------------------------------------------------------
--- SQL DEFINITIONSFILE ------------------------------------------------
------------------------------------------------------------------------

CREATE TABLE Library(
	ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL, 
	Name VARCHAR(50) NOT NULL DEFAULT 'UNNAMED',

	--Nycklar:
	-- 
	-- Beräknade attribut nedan
	--TotalCash INT NOT NULL, (SUM aggregat av alla betalda ordrar)

	--Referensattribut nedan
	-- 
	--Extra Begränsningar nedan
	--
	);

CREATE TABLE Session(
	ID INT NOT NULL, 		--Candidatekey
	[User] INT ,
	Loggedinat DATETIME,
	Loggedin BIT DEFAULT 0;
	LastUsed DATETIME,
	]
CREATE TABLE Users(
	ID INT IDENTITY(1,1) NOT NULL,                                      --Candidatekey
	
	LoanID INT NOT NULL FOREIGN KEY REFERENCES Cards(ID) UNIQUE, 		--Candidatekey
	Email VARCHAR(20) NOT NULL UNIQUE,									--Candidatekey
	[Password] VARCHAR(20) NOT NULL DEFAULT ROUND(RAND() * 100000, 0), 
	
	Firstname VARCHAR(20) ,
	Lastname VARCHAR(20) ,
	Rights INT NOT NULL DEFAULT 3 FOREIGN KEY REFERENCES Rights(ID),

	Activated BIT NOT NULL DEFAULT 1
	
	--MORE 
		-- Nycklar 
			--
		-- Beräknade attribut nedan
		--
		-- Referensattribut nedan
		-- Extra Begränsningar nedan
		
		--#Todo
				-- Check( 
				-- 		Logged in implies Activated
				-- 	  	)
		--
		);
CREATE TABLE Rights(
	ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Name VARCHAR(50) NOT NULL DEFAULT 'UNNAMED',
	
	--Nycklar:

	--Beräknade attribut nedan
	--
	-- Referensattribut nedan
	-- Extra Begränsningar nedan
	--
	); 
CREATE TABLE Loans(
	ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	LoanedAt DATE NOT NULL DEFAULT CurrentTime,          --- CurrentTime() ? 
	ObjectID int ForeignKey LibraryObjects(ID)
	--Beräknade attribut för potentiell extra-view
		

	-- Referensattribut nedan		
		
		-- 
	-- Extra Begränsningar nedan
		--Validerar
		-- CONSTRAINT CHK_ORDERS_OrderValidering   
	 --    	CHECK 
	 --    		(
		-- 			(PickedUp 	=0 or (Paid=1 AND Canceled =0 AND Returned=0))
		-- 			AND
		-- 			(Paid 		=0 or (Canceled=0) )
		-- 			AND
		-- 			(Canceled 	=0 OR (PickedUp=0 AND Returned=0) )
		-- 			AND
		-- 			(Returned 	=0 OR (PickedUp=0) )
									
		-- 		)
	);

CREATE TABLE LibraryObjects(
	ID INT IDENTITY(1,1) PRIMARY KEY,

	Title VARCHAR(50) NOT NULL DEFAULT 'UNNAMED', 
	[Description] varchar(500) NOT NULL DEFAULT 'No Description',
	Author varchar(500) DEFAULT 'No Description',
	Length INT DEFAULT 0,
	Narrator  varchar(500),
	Publisher varchar(500),
	Director  varchar(500),
	ISBN  varchar(500),
	Pages INT  DEFAULT 30,
	Available BIT NOT NULL DEFAULT 1,
	Category INT NOT NULL DEFAULT 1 FOREIGN KEY Categories(ID),
	AttachedFiles INT FOREIGN KEY TableWithFiles(ID),
	Purchaseprice FLOAT DEFAULT 300   				--Hur mycket biblioteket betalar
	);
CREATE TABLE Categories(
	ID INT IDENTITY(1,1) PRIMARY KEY,
	Name
	)
CREATE TABLE TableWithFiles(
	ID INT IDENTITY(1,1) PRIMARY KEY,
	file BINARY NOT NULL,
	)
------------------------------------------------------------------------
--- SQL INITIAL BATCH INSERTS ------------------------------------------
------------------------------------------------------------------------

	-- Users	
		SET IDENTITY_INSERT Users on
			INSERT INTO Users (ID,Username, [Password]) VALUES (1, 'Admin', 123);
			INSERT INTO Users (ID,Username) VALUES (2, 'Johan121');
			INSERT INTO Users (ID,Username) VALUES (3, 'PetraMede');
			INSERT INTO Users (ID,Username) VALUES (4, 'Birgit');
			INSERT INTO Users (ID,Username) VALUES (5, 'Pizza');
		SET IDENTITY_INSERT Users OFF
	GO


a
