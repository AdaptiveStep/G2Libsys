--------------------------------------------------------
	----------------------------------------------------
	--------- G2 SYSTEMS -------------------------------
		------------------------------------------------
		-----SKAPAT AV HARIZ@KTH.se 2020  --------------
		---- NEWTONS YRKESHÖGSKOLA        --------------
		---- PROJEKTARBETE TILL AGIL KURS --------------
		------------------------------------------------

		-----KVAR ATT GÖRA------------------------------
		--lätta saker
		-- Fixa syntax överallt

		--Svåra saker		
		-- Ta bort "Batch Resetter" och använd 
		-- "IF EXISTS...Replace" när tabeller 
		-- definieras istället
---------------------------------------------------------
--- SQL RESET -------------------------------------------
	-----------------------------------------------------



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
	);
	--STANDARD INSERTS
		SET IDENTITY_INSERT Library on
			INSERT INTO Library (ID,Name) VALUES (1, 'StadsBibliotekAB');
			SET IDENTITY_INSERT Library OFF
			GO

CREATE TABLE Cards(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,                                      --Candidatekey
	ActivationDate DATETIME NOT NULL DEFAULT SYSDATETIME(),
	ValidUntil AS DATEADD(year, 1, ActivationDate),
	Activated BIT NOT NULL DEFAULT 1,
	CardNumber AS ID * 2867585817934888 % 8687931395129801 --Calculated with (mod p) . Inverse is 7654321234567890
	);
	--STANDARD INSERTS
		SET IDENTITY_INSERT Cards on
			INSERT INTO Cards 
				(ID) 
				VALUES 
				(1),
				(2),
				(3);
			SET IDENTITY_INSERT Cards OFF
			GO

CREATE TABLE Rights(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY ,
	[Name] VARCHAR(50) 	 NOT NULL DEFAULT 'UNNAMED',
	); 
	--STANDARD INSERTS
		SET IDENTITY_INSERT Rights on
			INSERT INTO Rights(ID,[Name]) 
			VALUES 
				(1,'Bibliotekschef'),
				(2,'Lokalspersonal'),
				(3,'Användare'),
				(4,'Avstängd'),
				(5,'Städerska')
			SET IDENTITY_INSERT Rights OFF
			GO

CREATE TABLE Users(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY ,                  		--Candidatekey
	
	[Card] INT FOREIGN KEY REFERENCES Cards(ID) UNIQUE, 				--Candidatekey
	Email VARCHAR(20) 	   NOT NULL UNIQUE,								--Candidatekey
	[Password] VARCHAR(20) NOT NULL DEFAULT ROUND(RAND() * 100000, 0), 
	Firstname VARCHAR(20)  NOT NULL DEFAULT 'UNNAMED',
	Lastname VARCHAR(20)   NOT NULL DEFAULT 'UNNAMED',
	Rights INT 			   NOT NULL FOREIGN KEY REFERENCES Rights(ID),
	LoggedIn bit 		   NOT NULL DEFAULT 0, 
	)
	GO

	--STANDARD INSERTS
		SET IDENTITY_INSERT 	Users on
			INSERT INTO 		Users (ID,[Card],[Email], Firstname, Lastname, Rights) 
					VALUES 
					(1,1, 'Johan@johan.com'		,'johan'	,'schwartz'    	,1),		--Does ID really have to be inserted? 
					(2,2, 'Petra@petra.com'	  	,'Petra'	,'Mede'   	 	,2),
					(3,3, 'Joppan@johanna.com'	,'Joan'		,'Sacrebleu'	,3);
			SET IDENTITY_INSERT Users OFF
			GO

-- PROTOTYPE SCHEMA

	-- CREATE TABLE Loans(
	-- 	ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	-- 	LoanedAt DATE NOT NULL,          --- CurrentTime() ? 
	-- 	ObjectID int Foreign Key LibraryObjects(ID)

	-- 	);

	-- CREATE TABLE LibraryObjects(
	-- 	ID INT IDENTITY(1,1) PRIMARY KEY,

	-- 	Title VARCHAR(50) NOT NULL DEFAULT 'UNNAMED', 
	-- 		[Description] varchar(500) DEFAULT 'No Description',
			
	-- 		MaxDays INT NOT NULL DEFAULT 30, --Max days to loan
	-- 		Loanable BIT NOT NULL DEFAULT 1,
	-- 		Library INT NOT NULL DEFAULT 1 FOREIGN KEY Libraries(ID),
	-- 		AttachedFiles INT FOREIGN KEY TableWithFiles(ID),

	-- 		Purchaseprice FLOAT DEFAULT 300,
	-- 		ProductionYear DATETIME DEFAULT YEAR(curdate()),
	-- 	);

	-- CREATE TABLE BookBases(
	-- 	ID INT IDENTITY(1,1) NOT NULL,    --Candidatekey
	-- 	Author VARCHAR(50) NOT NULL DEFAULT 'UNNAMED',
	-- 	DeweyDec NOT NULL DEFAULT 'UNNAMED',
	-- 	Publisher VARCHAR(50)  DEFAULT 'UNNAMED'
	-- 	);

	-- --- REDUNDANT?
	-- CREATE TABLE Authorings(
	-- 	author
	-- 	bookID INT FOREIGN KEY AudioBooks(ID)
	-- 	);

	-- CREATE TABLE Books(
	-- 	LibraryObjectID INT FOREIGN KEY LibraryObjects(ID) PRIMARY KEY,     --Candidatekey
	-- 	BookBaseID INT FOREIGN KEY BookBases(ID) UNIQUE,     				--Candidatekey

	-- 	ISBN INT NOT NULL DEFAULT 30,
	-- 	Pages INT NOT NULL DEFAULT 30,
	-- 	);

	-- CREATE TABLE Ebooks(
	-- 	ID INT FOREIGN KEY LibraryObjects(ID),    --Candidatekey
	-- 	ISBN INT NOT NULL DEFAULT 30,
	-- 	Pages INT NOT NULL DEFAULT 30,
	-- 	);

	-- CREATE TABLE Audiobooks(
	-- 	ID INT FOREIGN KEY LibraryObjects(ID),
	-- 	ISBN INT NOT NULL DEFAULT 30,
	-- 	Length INT NOT NULL DEFAULT 60,          -- IN MINUTES
	-- 	Narrator VARCHAR(50) NOT NULL DEFAULT 'UNNAMED'
	-- 	);

	-- CREATE TABLE Movies(
	-- 	ID INT FOREIGN KEY LibraryObjects(ID),    --Candidatekey
	-- 	Author VARCHAR(50) NOT NULL DEFAULT 'UNNAMED', 
	-- 	ISBN INT NOT NULL DEFAULT 30,
	-- 	Pages INT NOT NULL DEFAULT 30,
	-- 	);

	-- CREATE TABLE Events(
	-- 	ID INT FOREIGN KEY LibraryObjects(ID),    --Candidatekey
	-- 	Author VARCHAR(50) NOT NULL DEFAULT 'UNNAMED', 
	-- 	ISBN INT NOT NULL DEFAULT 30,
	-- 	Pages INT NOT NULL DEFAULT 30,
	-- 	);

	-- CREATE TABLE TableWithFiles(
	-- 	ID INT IDENTITY(1,1) NOT NULL,    --Candidatekey
	-- 	File Binary NOT NULL,
	-- 	);

------------------------------------------------------------------------
--- OTHER SQL BATCH INSERTS ------------------------------------------
------------------------------------------------------------------------