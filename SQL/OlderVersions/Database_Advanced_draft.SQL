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
		--lätta saker
		-- Fixa syntax överallt

		--Svåra saker		
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
CREATE TABLE Users(
	ID INT IDENTITY(1,1) NOT NULL,                                      --Candidatekey
	
	LoanID INT NOT NULL FOREIGN KEY REFERENCES Cards(ID) UNIQUE, 		--Candidatekey
	Email VARCHAR(20) NOT NULL UNIQUE,									--Candidatekey
	[Password] VARCHAR(20) NOT NULL DEFAULT ROUND(RAND() * 100000, 0), 
	
	Firstname VARCHAR(20) ,
	Lastname VARCHAR(20) ,
	Rights INT NOT NULL DEFAULT 3 FOREIGN KEY REFERENCES Rights(ID),

	LoggedIn bit NOT NULL DEFAULT 0, 
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

	);

CREATE TABLE LibraryObjects(
	ID INT IDENTITY(1,1) PRIMARY KEY,

	Title VARCHAR(50) NOT NULL DEFAULT 'UNNAMED', 
		[Description] varchar(500) DEFAULT 'No Description',
		
		MaxDays INT NOT NULL DEFAULT 30, --Max days to loan
		Loanable BIT NOT NULL DEFAULT 1,
		Library INT NOT NULL DEFAULT 1 FOREIGN KEY Libraries(ID),
		AttachedFiles INT FOREIGN KEY TableWithFiles(ID),

		Purchaseprice FLOAT DEFAULT 300,
		ProductionYear DATETIME DEFAULT YEAR(curdate()),
	);

CREATE TABLE BookBases(
	ID INT IDENTITY(1,1) NOT NULL,    --Candidatekey
	Author VARCHAR(50) NOT NULL DEFAULT 'UNNAMED',
	DeweyDec NOT NULL DEFAULT 'UNNAMED',
	Publisher VARCHAR(50)  DEFAULT 'UNNAMED'
	);

CREATE TABLE Authorings(
	author
	bookID INT FOREIGN KEY AudioBooks(ID)
	);

CREATE TABLE Books(
	LibraryObjectID INT FOREIGN KEY LibraryObjects(ID) PRIMARY KEY,
	BookBaseID INT FOREIGN KEY BookBases(ID) UNIQUE,

	ISBN INT NOT NULL DEFAULT 30,
	Pages INT NOT NULL DEFAULT 30,
	);

CREATE TABLE Ebooks(
	ID INT FOREIGN KEY LibraryObjects(ID),
	ISBN INT NOT NULL DEFAULT 30,
	Pages INT NOT NULL DEFAULT 30,
	);

CREATE TABLE Audiobooks(
	ID INT FOREIGN KEY LibraryObjects(ID),
	ISBN INT NOT NULL DEFAULT 30,
	Length INT NOT NULL DEFAULT 60,          -- IN MINUTES
	Narrator VARCHAR(50) NOT NULL DEFAULT 'UNNAMED'
	);

CREATE TABLE Movies(
	ID INT FOREIGN KEY LibraryObjects(ID),
	Author VARCHAR(50) NOT NULL DEFAULT 'UNNAMED', 
	ISBN INT NOT NULL DEFAULT 30,
	Pages INT NOT NULL DEFAULT 30,
	);

CREATE TABLE Events(
	ID INT FOREIGN KEY LibraryObjects(ID),
	Author VARCHAR(50) NOT NULL DEFAULT 'UNNAMED', 
	ISBN INT NOT NULL DEFAULT 30,
	Pages INT NOT NULL DEFAULT 30,
	);

CREATE TABLE TableWithFiles(
	ID INT IDENTITY(1,1) NOT NULL,    --Candidatekey
	File Binary NOT NULL,
	);
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
