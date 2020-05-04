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
		-- Fix the search features

--FIXA LÄNGRE EMAILS VARCHAR(100)
		--Svåra saker		
		-- Ta bort "Batch Resetter" och använd 
		-- "IF EXISTS...Replace" när tabeller 
		-- definieras istället
		-- History Tables och triggers till User/object/Reservations
		-- Find candidate keys
		-- Find dependencies  , normalize more.

---------------------------------------------------------
--- SQL RESET -----------WARNING--------------WARNING----
	-----------------------------------------------------
	----------- WARNING -----------WARNING---------------


	--Throws .

	--Disconnect everyone else
	USE master;
	GO
	IF EXISTS (SELECT name FROM master.sys.databases WHERE name = N'G2Libsys')
		BEGIN
		
		ALTER DATABASE [G2Libsys]
			SET SINGLE_USER
			WITH ROLLBACK IMMEDIATE;
			
		ALTER DATABASE [G2Libsys]
			SET READ_ONLY;
		END
	GO

	--Delete the database if it exists. 
	USE [master]
	GO
		DROP DATABASE IF EXISTS [G2Libsys]
		GO
		------------------------

	--Create a completelly new empty database . 
	USE [master]
	GO
		Create Database [G2Libsys]
		GO
		USE [G2Libsys]
		GO

	--Enables database to other users again.
	ALTER DATABASE [G2Libsys]
	SET MULTI_USER;
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

	[Email] VARCHAR(20) 	   NOT NULL DEFAULT 'UNNAMED',
	[Password] VARCHAR(20) NOT NULL DEFAULT ROUND(RAND() * 100000, 0), 
	Firstname VARCHAR(20)  NOT NULL DEFAULT 'UNNAMED',
	Lastname VARCHAR(20)   NOT NULL DEFAULT 'UNNAMED',
	UserType INT 			   NOT NULL FOREIGN KEY REFERENCES UserTypes(ID),
	LoggedIn bit 		   NOT NULL DEFAULT 0, 
	);
	GO
	--STANDARD INSERTS
		SET IDENTITY_INSERT 	Users on
			INSERT INTO 		Users (ID,[Email],[Password], Firstname, Lastname, UserType) 
					VALUES 
					(1, 'Admin@johan.com'	,123		,'johan'	,'schwartz'    	,1),		--Does ID really have to be inserted? 
					(2, 'Petra@petra.com'	,123	  	,'Petra'	,'Mede'   	 	,2),
					(3, 'Joppan@johanna.com',123		,'Joan'		,'Sacrebleu'	,3);
			SET IDENTITY_INSERT Users OFF
			GO

CREATE TABLE Cards(
	ID 				INT IDENTITY(1,1) 	NOT NULL PRIMARY KEY,                                      	--Candidatekey
	ActivationDate 	DATETIME 			NOT NULL DEFAULT SYSDATETIME(),
	ValidUntil AS	DATEADD(year, 1, ActivationDate),
	Activated 		BIT 				NOT NULL DEFAULT 1,
	[Owner] 		INT 			   	NOT NULL FOREIGN KEY REFERENCES Users(ID), 					--Many2One

	--Calculated Column
	CardNumber AS FORMAT(ID * 2867585817934888 % 8687931395129801, '0000-0000-0000-0000') --Calculated with (mod p) . Inverse is 7654321234567890
															-- BUG: behöver REPLICATE('0',16-LEN(CardNumber)) + CardNumber
															--   Detta kommer sätta göra så att 153 blir 00000000000000153
															-- använd RTRIM(CardNumber) för att konvertera till VARCHAR.
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
		Category 		INT 		 			DEFAULT 1 		FOREIGN KEY REFERENCES Categories(ID) ON DELETE SET NULL,

		ActivityDate DATETIME 					DEFAULT NULL,

		-- META attributes -- Must be here due to the 1..1 -> 1..1 relationship. Cannot be in separate Table.
		-- Solution: Can be replaced with "history tables and history-triggers".
		[Library] 		INT 				 DEFAULT 1 			FOREIGN KEY REFERENCES Libraries(ID) ON DELETE SET NULL,
		[AddedBy] 		INT 	 	NOT NULL DEFAULT 1 			FOREIGN KEY REFERENCES Users(ID),
		[LastEdited] 	DATETIME 	NOT NULL DEFAULT SYSDATETIME(),
		[DateAdded] 	DATETIME 	NOT NULL DEFAULT SYSDATETIME()
		--Constraints
		);
	--SAMPLE INSERTS
	SET IDENTITY_INSERT 	LibraryObjects on
		INSERT INTO 		LibraryObjects (ID,[Title], [Description], ISBN, Publisher, DeweyDecimal, PurchasePrice, Category, ActivityDate) 
				VALUES 
				(1, 'Harry Potter'							,'Bra skick och bok för ungdomar'			,7599999999999,'BonniersBöcker'	 	,435	,350, 1,SYSDATETIME()),		--Does ID really have to be inserted? 
				(2, 'Sagan om Ringen'						,'Bra skick och bok för ungdomar. Poppis'	,9999229999999,'Penguin Classics'	,100	,350, 1,SYSDATETIME()),
				(3, 'Sagan om Ringen - Bilbo Ljudboken!'	,'Våran första ljudbok i lager'				,9974324423999,'Coola Förlaget'		,415	,350, 3,SYSDATETIME()),
				(4, 'Game of Thrones Ebook!'				,'Från tittande till lyssnande!'			,9979999423999,'Amazon'				,215	,350, 2,SYSDATETIME()),
				(5, 'Eva mår bra! Ljudblogbok'				,'Bra skick och ljudbok för alla!'			,9999992343999,'Evas Förlag'		,500	,350, 3,SYSDATETIME()),
				(6, 'Tjäna pengar med flumkurser!'			,'Otroligt seminarie om modern utbildning!' ,9999999423999,'StadsBiblioteketAB'	,19999	,350, 5, '2020-06-01'),
				(7, 'The Hulk'								,'Senaste filmen från hollywood!'			,9996439994999,'WarnerBrothers'		,200	,350, 4,SYSDATETIME()),
				(8, 'Starta Eget'							,'Den bästa seminarien om starta eget.'		,9999999423999,'StadsBiblioteketAB'	,600	,350, 5,'2020-06-11');
		SET IDENTITY_INSERT LibraryObjects OFF
		GO

	--Relationship Card-2-OBJECT ; M2M
	CREATE TABLE ReservationsBy(
		ID				INT IDENTITY(1,1)	NOT NULL PRIMARY KEY,
		ReservationDate DATE				NOT NULL DEFAULT SYSDATETIME(),   
		ObjectID		INT 				NOT NULL FOREIGN KEY REFERENCES LibraryObjects(ID)  ON DELETE CASCADE,
		Card			INT 				NOT NULL FOREIGN KEY REFERENCES Cards(ID)  ON DELETE CASCADE,

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
	GO
------------------------------------------------------------------------
--- SQL DATA -----------------------------------------------------------
------------------------------------------------------------------------

--DEWEY TABLES USED BY OBJECT
	-- CREATE TABLE DeweyDecimal(
	-- 	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY ,
	-- 	[classname] VARCHAR(30) NOT NULL,
	-- 	[sectionname] VARCHAR(30) NOT NULL,
	-- 	[divisionname] VARCHAR(30) NOT NULL
	-- 	); 


	-- 	--STANDARD INSERTS
	-- 		SET IDENTITY_INSERT [Usertypes] on
	-- 			INSERT INTO UserTypes([classname],[sectionname],[divisionname]) 
	-- 			VALUES 
	-- 				(001,'coolclass', 'coolsection','cooldivision'),
	-- 				(002,'Lokalspersonal'),
	-- 				(003,'Användare'),
	-- 				(004,'Avstängd'),
	-- 				(005,'Städerska')
	-- 			SET IDENTITY_INSERT [Usertypes] OFF
	-- 			GO

--- SQL PROCEDURES 	----------------------------------------------------
------------------------------------------------------------------------


---------------simple users ---------------------

--Add User
CREATE PROC usp_insert_users
    @Email varchar(20),
    @Password varchar(20),
    @Firstname varchar(20),
    @Lastname varchar(20),
    @UserType int,
    @NewID int Output
    as
    begin
        insert into users (Email, [Password], Firstname, Lastname, UserType)
        values (@Email, @Password, @Firstname, @Lastname, @UserType);
        select @NewID = SCOPE_IDENTITY();
    end
    GO

--Delete user
CREATE PROC usp_delete_users
    @ID int = null
    AS
    begin
		IF (@ID IS NOT NULL)	--Only delete user if there is a userID provided
   	    	BEGIN
			DELETE FROM Users WHERE ID=@ID; 		--Might still crash if there is no user with this userid
			END
    END
    GO

---------------Usertype ---------------------
--Change usertype for user

	--Överflödig
	-- CREATE PROC usp_changeUsertype_users
	--     @UserID int = null,
	--     @UsertypeID int = null
	--     AS
	--     BEGIN
	--    	    IF (@UserID IS NOT NULL and @UsertypeID is NOT NULL)
	--    	    	BEGIN
	-- 				UPDATE Users
	-- 				SET Usertype = @UsertypeID
	-- 				WHERE ID=@UserID; 
	-- 			END
	--     END
	--     GO

--Delete Usertype
CREATE PROC usp_delete_usertypes
    @ID int = null
    AS
    begin
		IF (@ID IS NOT NULL)	--Only delete user if there is a userID provided
   	    	BEGIN
			DELETE FROM Users WHERE ID=@ID; 		--Might still crash if there is no user with this userid
			END
    END
    GO

--Add Usertype
CREATE PROC usp_insert_usertype
	@ID INT = null,
	@Name VARCHAR(50)
    as
    begin
        insert into UserType ([Name])
        values (@Name);
    end
    GO

---------------Reservations ---------------------
--ADD reservation for objectID: using a cardID and a ReservationDate.
CREATE PROC usp_add_reservationsby
	@ReservationDate DATE,
	@ObjectID		INT,
	@Card			INT
    AS

    BEGIN
        INSERT INTO ReservationsBy (ReservationDate, ObjectID, Card)
        VALUES (@ReservationDate, @ObjectID, @Card);
    END
    GO

--DELETE Reservation for user
CREATE PROC usp_delete_reservationsby
    @ID int = null
    AS
    begin
		IF (@ID IS NOT NULL)	--Only delete user if there is a userID provided
   	    	BEGIN
			DELETE FROM ReservationsBy WHERE ID=@ID; 		--Might still crash if there is no user with this userid
			END
    END
    GO


--Slå ihop med nedan
--Edit Reservation: Change reservationdate
CREATE PROC usp_update_reservationsby
	@ID 			 	INT  	= null,
	@ReservationDate 	DATE	= null,
	@ObjectID 			INT 	= null,
	@Card 				INT 	= null, 
	@Fulfilled 			BIT

    AS
    BEGIN
		UPDATE ReservationsBy
		SET ReservationDate = @ReservationDate, 
			ObjectID 		= @OBjectID,
			Card 			= @Card,
			Fulfilled 		= @Fulfilled
		WHERE ID=@ID; 
    END
    GO

---------------Cards ---------------------
	--Delete the card for user

--funkar
CREATE PROC usp_delete_cards
    @ID int = null
    AS
    begin
		IF (@ID IS NOT NULL)	--Only delete user if there is a userID provided
   	    	BEGIN
			DELETE FROM Cards WHERE ID=@ID; 		--Might still crash if there is no user with this userid
			END
    END
    GO


CREATE PROC usp_add_cards
    @ID 			int 		=null,
	@ActivationDate DATETIME 	=null,
	@Activated 		BIT 		=null,	
	@Owner 		INT 		=null

    AS

    BEGIN
        INSERT INTO 
        Cards(Owner) VALUES (@Owner);
    END
    GO

---------------Search structures ---------------------
--Dynamic search of users
	--TODO : make it look for  --
	--				loandID LIKE %bla
	-- 				or card owner for CARD LIKE %bla543
CREATE PROC usp_findusers_users
    @PartialName VARCHAR(20) = '',
    @PartialLastName VARCHAR(20) = '',
    @PartialEmail VARCHAR(20) = ''

    AS
    BEGIN
		SELECT *
		FROM Users
		WHERE 
			Firstname LIKE '%' + @PartialName + '%' AND
			Firstname LIKE '%' + @PartialLastName + '%' AND
			[Email] LIKE '%' + @PartialEmail + '%';
    END
    GO

--------BELOW IS STILL UNDER CONSTRUCITON:
--Dynamic search of objects - multiple keywords (i.e conjunctive filter search)
CREATE PROC usp_findobject_LibraryObjects
	@PartialTitle VARCHAR(20) = '',
	@PartialDescription VARCHAR(20) = '',
	@PartialPublisher VARCHAR(20) = ''
	--@PartialISBN INT = 0, 					-----TODO; ADD MORE SEARCH OPTIONS
	--@DeweyDecimal INT = null,
	--@PriceRange INT = null,
	--@Category INT = null


    AS
    BEGIN
		SELECT *
		FROM LibraryObjects
		WHERE 
			Title LIKE '%' + @PartialTitle + '%' AND
			[Description] LIKE '%' + @PartialDescription + '%' AND
			[Publisher] LIKE '%' + @PartialPublisher + '%';
    END
    GO

--Dynamic search of objects - only one keyword
CREATE PROC usp_simplesearch_LibraryObjects
	@Partialword VARCHAR(20) = ''

    AS
    BEGIN
		SELECT *
		FROM LibraryObjects
		WHERE 
			Title LIKE '%' + @Partialword + '%' Or
			[Description] LIKE '%' + @Partialword + '%' OR
			[Publisher] LIKE '%' + @Partialword + '%';
    END
    GO

--Dynamic search of reservations
	-- CREATE PROC usp_findusers_ReservationsBy
	-- 	@dateStart DateTime =  DATEADD(year, -1,GETDATE()), --default intervall is +-1 year from now.
	-- 	@dateEnd DateTime, =  DATEADD(year, 1,GETDATE()),
		
	-- 	@commacategory VARCHAR(50), 					--such as '2 , 5 , 1'
	-- 	@PartialCardnumber VARCHAR(50), 				--such as '2363'
	-- 	@Fulfilled BIT =null, 							--such as 0
	--     AS
	--     BEGIN


	-- 		SELECT *
	-- 		FROM ReservationsBy
	-- 		WHERE 

	-- 			Reservation is within interval
				
	-- 			AND
	-- 			reservation has right category

	-- 			AND
	-- 			reservation is by cardnumber containing integers

	-- 			And
	-- 			if fulfilled is provided; Reservations filtered by fulfilled

	-- 			-----------
	-- 			AND
	-- 			(if fulfilled is null give 1)
				
	-- 			AND
	-- 			(if objectlist is not null check if category)
	-- 			IF (@ObjectIDList NOT NULL)
	-- 			BEGIN
	-- 			objectcategory IN STRING_SPLIT( @ObjectIDList, ',')
	--     		END
	--     END
	--     GO


	-- 		--use this...
	-- 		-- objectcategory IN STRING_SPLIT( 'hey,cool,guy', ',')  -- splittar upp den i tre nya rader.

	-- 		--SELECT DATEADD(YEAR, -1, DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) -3, 0))  --- Ger datum "1 år sen och 3 månader tillbaka"

	-- 		--SELECT dateadd(year,-1)