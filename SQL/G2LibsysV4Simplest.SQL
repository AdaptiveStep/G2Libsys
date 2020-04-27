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
		-- Views for reports
			--WHich views? View for 
			--Lägg in deweydecimal tabell
		-- Dummy Reservations

		--Svåra saker		
		-- Ta bort "Batch Resetter" och använd 
		-- "IF EXISTS...Replace" när tabeller 
		-- definieras istället
		-- History Tables och triggers till User/object/Reservations
		-- Find candidate keys
		-- Find dependencies  , normalize more.
---------------------------------------------------------
--- SQL RESET -------------------------------------------
	-----------------------------------------------------



		USE [master]
		GO
		ALTER DATABASE [TESTDB55] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
		GO
		USE [master]
		GO
		DROP DATABASE [TESTDB55]
		GO
		----------- WARNING

		--Skapas en helt ny Tom G2Systems Databas.
		GO 
		USE [master]
		GO
		Create Database [TESTDB55]
		GO
		USE [TESTDB55]
		GO

------------------------------------------------------------------------
--- SQL DEFINITIONSFILE ------------------------------------------------
------------------------------------------------------------------------

CREATE TABLE Libraries(
	ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL, 
	[Name] VARCHAR(50) NOT NULL DEFAULT 'UNNAMED',
	);
	--STANDARD INSERTS
		SET IDENTITY_INSERT [Libraries] on
			INSERT INTO Libraries (ID,Name) VALUES (1, 'StadsBibliotekAB');
			SET IDENTITY_INSERT [Libraries] OFF
			GO

CREATE TABLE UserTypes(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY ,
	[Name] VARCHAR(50) 	 NOT NULL DEFAULT 'UNNAMED',
	); 
	--STANDARD INSERTS
		SET IDENTITY_INSERT [Usertypes] on
			INSERT INTO UserTypes(ID,[Name]) 
			VALUES 
				(1,'Bibliotekschef'),
				(2,'Lokalspersonal'),
				(3,'Användare'),
				(4,'Avstängd'),
				(5,'Städerska')
			SET IDENTITY_INSERT [Usertypes] OFF
			GO

CREATE TABLE Users(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY ,                  		--Candidatekey
	LoanID AS (ID+100) * 2867585817934888 % 8687931395129801, 			--Calculated with (mod p) . Inverse is 7654321234567890 . Always unique due to being Finite Field.

	[Email] VARCHAR(20) 	   NOT NULL UNIQUE,								--Candidatekey
	[Password] VARCHAR(20) NOT NULL DEFAULT ROUND(RAND() * 100000, 0), 
	Firstname VARCHAR(20)  NOT NULL DEFAULT 'UNNAMED',
	Lastname VARCHAR(20)   NOT NULL DEFAULT 'UNNAMED',
	UserType INT 			   NOT NULL FOREIGN KEY REFERENCES UserTypes(ID),
	LoggedIn bit 		   NOT NULL DEFAULT 0, 
	);
	GO
	--STANDARD INSERTS
		SET IDENTITY_INSERT 	Users on
			INSERT INTO 		Users (ID,[Email], Firstname, Lastname, UserType) 
					VALUES 
					(1, 'Johan@johan.com'		,'johan'	,'schwartz'    	,1),		--Does ID really have to be inserted? 
					(2, 'Petra@petra.com'	  	,'Petra'	,'Mede'   	 	,2),
					(3, 'Joppan@johanna.com'	,'Joan'		,'Sacrebleu'	,3);
			SET IDENTITY_INSERT Users OFF
			GO

CREATE TABLE Cards(
	ID 				INT IDENTITY(1,1) 	NOT NULL PRIMARY KEY,                                      	--Candidatekey
	ActivationDate 	DATETIME 			NOT NULL DEFAULT SYSDATETIME(),
	ValidUntil AS	DATEADD(year, 1, ActivationDate),
	Activated 		BIT 				NOT NULL DEFAULT 1,
	[Owner] 		INT 			   	NOT NULL FOREIGN KEY REFERENCES Users(ID), 					--Many2One

	--Calculated Column
	CardNumber AS ID * 2867585817934888 % 8687931395129801 --Calculated with (mod p) . Inverse is 7654321234567890
	);
	--STANDARD INSERTS
		SET IDENTITY_INSERT Cards on
			INSERT INTO Cards 
				(ID,[Owner]) 
				VALUES 
				(1,1),
				(2,2),
				(3,3);
			SET IDENTITY_INSERT Cards OFF
			GO

-- PROTOTYPE SCHEMA

	--Relationship USER-2-OBJECT ; M2M
	CREATE TABLE Categories(
		ID				INT IDENTITY(1,1)	NOT NULL PRIMARY KEY,					--Candidatekey
		Name 			VARCHAR(50) 		NOT NULL DEFAULT 'UNNAMED' UNIQUE, 		--Candidatekey
		[Description] 	varchar(500) 		DEFAULT 'No Description',
		);

	--STANDARD INSERTS
		SET IDENTITY_INSERT 	Categories on
			INSERT INTO 		Categories (ID,[Name], Description) 
					VALUES 
					(1, 'Papers Bok','Alla typer av böcker'							),		--Does ID really have to be inserted? 
					(2, 'Ebok'	  	,'Eböcker som går att ladda ner.'				),
					(3, 'Ljudbok'	,'Ljudböcker som går att lyssna på'				),
					(4, 'Film'		,'Filmer som går att låna eller ladda ner'		),
					(5, 'Seminarie'	,'Kunskapsseminarier som går att boka sig till'	);
			SET IDENTITY_INSERT Categories OFF
			GO

	CREATE TABLE LibraryObjects(
		ID 				INT 		IDENTITY(1,1) 				PRIMARY KEY,		--Candidatekey
		Title 			VARCHAR(50) NOT NULL 	DEFAULT 'UNNAMED', 
		[Description] 	varchar(500) 			DEFAULT 'No Description',
		ISBN 			BIGINT 		NOT NULL 	DEFAULT 0,
		Publisher		VARCHAR(100) 			DEFAULT 'UNNAMED',
		DeweyDecimal	INT 		NOT NULL 	DEFAULT 0,					 						--NEEDS CHECK 0-999 
		PurchasePrice 	FLOAT 					DEFAULT 300,
		Category 		INT 		NOT NULL 	DEFAULT 1 		FOREIGN KEY REFERENCES Categories(ID),

		-- META attributes -- Must be here due to the 1..1 -> 1..1 relationship. Cannot be in separate Table.
		-- Solution: Can be replaced with "history tables and history-triggers".
		[Library] 		INT 	 	NOT NULL DEFAULT 1 			FOREIGN KEY REFERENCES Libraries(ID),
		[AddedBy] 		INT 	 	NOT NULL DEFAULT 1 			FOREIGN KEY REFERENCES Users(ID),
		[LastEdited] 	DATETIME 	NOT NULL DEFAULT SYSDATETIME(),
		[DateAdded] 	DATETIME 	NOT NULL DEFAULT SYSDATETIME()
		--Constraints
		);
	--SAMPLE INSERTS
	SET IDENTITY_INSERT 	LibraryObjects on
		INSERT INTO 		LibraryObjects (ID,[Title], [Description], ISBN, Publisher, DeweyDecimal, PurchasePrice, Category) 
				VALUES 
				(1, 'Harry Potter'							,'Bra skick och bok för ungdomar'			,7599999999999,'BonniersBöcker'	 	,435	,350, 1),		--Does ID really have to be inserted? 
				(2, 'Sagan om Ringen'						,'Bra skick och bok för ungdomar. Poppis'	,9999229999999,'Penguin Classics'	,100	,350, 1),
				(3, 'Sagan om Ringen - Bilbo Ljudboken!'	,'Våran första ljudbok i lager'				,9974324423999,'Coola Förlaget'		,415	,350, 3),
				(4, 'Game of Thrones Ebook!'				,'Från tittande till lyssnande!'			,9979999423999,'Amazon'				,215	,350, 2),
				(5, 'Eva mår bra! Ljudblogbok'				,'Bra skick och ljudbok för alla!'			,9999992343999,'Evas Förlag'		,500	,350, 3),
				(6, 'Tjäna pengar med flumkurser!'			,'Otroligt seminarie om modern utbildning!' ,9999999423999,'StadsBiblioteketAB'	,19999	,350, 5),
				(7, 'The Hulk'								,'Senaste filmen från hollywood!'			,9996439994999,'WarnerBrothers'		,200	,350, 4),
				(8, 'Starta Eget'							,'Den bästa seminarien om starta eget.'		,9999999423999,'StadsBiblioteketAB'	,600	,350, 5);
		SET IDENTITY_INSERT LibraryObjects OFF
		GO


	--Relationship USER-2-OBJECT ; M2M
	CREATE TABLE ReservationsBy(
		ID				INT IDENTITY(1,1)	NOT NULL PRIMARY KEY,
		ReservationDate DATE				NOT NULL DEFAULT SYSDATETIME(),   
		ObjectID		INT FOREIGN KEY REFERENCES LibraryObjects(ID),
		Card			INT FOREIGN KEY REFERENCES Cards(ID),

		--if the user has returned a book, or visited a seminar. Or in any other way fulfilled his deal of the contract. 
		Fulfilled BIT NOT NULL DEFAULT 0
		--Computed columns
		--ExpirationDate
		--isLate

		);

	--Contains all the attachments , .pdfs etc..
	CREATE TABLE FilesTable(
		ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,				--Candidatekey
		FilePayload Binary NOT NULL UNIQUE						--Candidatekey	
		);
	CREATE TABLE TableWithFiles(
		ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,				--Candidatekey
		
		--CandidateKey Together
		ObjectID INT FOREIGN KEY REFERENCES LibraryObjects(ID),
		FileID INT FOREIGN KEY REFERENCES FilesTable(ID),
		--
		CONSTRAINT UC_ObjectFileAttachment UNIQUE (ObjectID,FileID)
		);

------------------------------------------------------------------------
--- OTHER SQL BATCH INSERTS ------------------------------------------
------------------------------------------------------------------------