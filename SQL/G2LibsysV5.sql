----------------------------G2 SYSTEMS----------------------------------
		----------------------------------------------------------------
		-----SKAPAT AV HARIZ@KTH.se 2020  ------------------------------
		---- NEWTONS YRKESHÖGSKOLA		  ------------------------------
		---- PROJEKTARBETE TILL AGIL KURS ------------------------------
		----------------------------------------------------------------
------------------------------------------------------------------------

--KVAR ATT GÖRA---------------------------------------------------------
	--lätta saker
	-- Fixa syntax överallt
	-- Views for reports
		--WHich views? View for 

	-- Kolla default värden för alla Procedures
	-- Dummy Reservations
	-- Fix the search features

	--Svåra saker		
	-- Ta bort "Batch Resetter" och använd 
	-- "IF EXISTS...Replace" när tabeller 
	-- definieras istället
	-- History Tables och triggers till User/object/Reservations (Går inte att ta bort grejer annars eller följa utveckling)
	-- Find candidate keys
	-- Find depENDencies  , normalize more.

--QUESTIONS????---------------------------------------------------------
	--How to use the cardnumbers? Delete using cardnumbers?
	--

--LATEST EDITS:---------------------------------------------------------
	--DONE:

	-- Added some procedures for handling loans.
	-- Added a better Advanced search procedure, for libraryobjects (still needs work)
	-- Fixed SQL folder

------------------------------------------------------------------------
--SQL RESET -------------WARNING--------------WARNING-------------------
	--------------------------------------------------------------------
	----------- WARNING -----------WARNING------------------------------


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
--SQL DEFINITIONSFILE --------------------------------------------------

	DROP TABLE IF EXISTS dbo.Libraries	
	CREATE TABLE Libraries(
		ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL, 
		[Name] VARCHAR(50) NOT NULL DEFAULT 'UNNAMED',
		);
		--STANDARD INSERTS
			SET IDENTITY_INSERT [Libraries] on
				INSERT INTO Libraries (ID,Name) VALUES (1, 'StadsBibliotekAB');
				SET IDENTITY_INSERT [Libraries] OFF
				GO

	DROP TABLE IF EXISTS dbo.UserTypes	
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

	DROP TABLE IF EXISTS dbo.Users
	CREATE TABLE Users(
		ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY ,				  		--Candidatekey
		--Usernumber AS (ID+100) * 2867585817934888 % 8687931395129801, 			--Calculated with (mod p) . Inverse is 7654321234567890 . Always unique due to being Finite Field.

		[Email] VARCHAR(300) 	   	NOT NULL UNIQUE,
		[Password] VARCHAR(20) 		NOT NULL DEFAULT ROUND(RAND() * 100000, 0), 
		Firstname VARCHAR(20)  		NOT NULL DEFAULT 'UNNAMED',
		Lastname VARCHAR(20)   		NOT NULL DEFAULT 'UNNAMED',
		UserType INT 			   	NOT NULL FOREIGN KEY REFERENCES UserTypes(ID),
		LoggedIn bit 		   		NOT NULL DEFAULT 0 
		);
		GO
		--STANDARD INSERTS
			SET IDENTITY_INSERT 	Users on
				INSERT INTO 		Users (ID,[Email],[Password], Firstname, Lastname, UserType) 
						VALUES 
						(1, 'Admin@johan.com'	,123		,'johan'	,'schwartz'		,1),		--Does ID really have to be inserted? 
						(2, 'Petra@petra.com'	,123	  	,'Petra'	,'Mede'   	 	,2),
						(3, 'Joppan@johanna.com',123		,'Joan'		,'Sacrebleu'	,3);
				SET IDENTITY_INSERT Users OFF
				GO
	
	DROP TABLE IF EXISTS dbo.Authors	
	CREATE TABLE Authors(
		ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY ,				  							--Candidatekey
		Firstname VARCHAR(20)  NOT NULL DEFAULT 'UNNAMED',
		Lastname VARCHAR(20)   NOT NULL DEFAULT 'UNNAMED',
		ImageSRC VARCHAR(300),
		Biography VARCHAR(5000)
		);
		GO

		--STANDARD INSERTS
			SET IDENTITY_INSERT 	Authors on
				INSERT INTO 		Authors (ID,Firstname, Lastname, ImageSRC, Biography) 
						VALUES 
						(0, 'Okänt'	,'Okänt', 'http://ifmetall-alimak.se/onewebmedia/bild-saknas%20herr.gif', 'Ingen biografi hittades'									),				--Does ID really have to be inserted? 

						(1, 'JK - Joanne'	,'Rowling'	, 'https://upload.wikimedia.org/wikipedia/commons/thumb/5/5d/J._K._Rowling_2010.jpg/330px-J._K._Rowling_2010.jpg', 'Joanne Rowling, better known by her pen name J. K. Rowling, is a British author, film producer, television producer, screenwriter, and philanthropist. She is best known for writing the Harry Potter fantasy series, which has won multiple awards and sold more than 500 million copies, becoming the best-selling book series in history.'	), 
						(2, 'George R. R'	,'Martin'	, 'https://duckduckgo.com/i/e6ce7885.jpg', 'George Raymond Richard Martin, also known as GRRM, is an American novelist and short story writer in the fantasy, horror, and science fiction genres, screenwriter, and television producer. He is writing the series of epic fantasy novels A Song of Ice and Fire, which was adapted into the HBO series Game of Thrones'	),
						(3, 'JRR Tolkien'	,'Tolkien'	, 'https://upload.wikimedia.org/wikipedia/commons/b/b4/Tolkien_1916.jpg', 'John Ronald Reuel Tolkien was an English writer, poet, philologist, and academic. He was the author of the classic high fantasy works The Hobbit and The Lord of the Rings'						),
						(4, 'Jan'			,'Guillou'	, 'https://upload.wikimedia.org/wikipedia/commons/thumb/6/69/Jan_Guillou%2C_Bokm%C3%A4ssan_2013_5_%28crop%29.jpg/330px-Jan_Guillou%2C_Bokm%C3%A4ssan_2013_5_%28crop%29.jpg', 'Jan Oskar Sverre Lucien Henri Guillou is a French-Swedish author and journalist. Among his books are a series of spy fiction novels about a spy named Carl Hamilton, and a trilogy of historical fiction novels about a Knight Templar, Arn Magnusson'	);
				SET IDENTITY_INSERT Authors OFF
				GO
	
	DROP TABLE IF EXISTS dbo.Seminars	
	CREATE TABLE Seminars(
		ID 				INT IDENTITY(1,1) 	NOT NULL 	PRIMARY KEY 						,   --Candidatekey
		[Title] 		VARCHAR(200)  		NOT NULL 				DEFAULT 'UNNAMED'		,
		[Description] 	VARCHAR(500)   								DEFAULT 'UNNAMED'		,
		PlannedDate 	DATETIME 			NOT NULL 	DEFAULT SYSDATETIME()				,
		ImageSRC 		VARCHAR(300)														,
		Author			INT 				NOT NULL 	FOREIGN KEY REFERENCES Authors(ID)
		);
		--STANDARD INSERTS
			SET IDENTITY_INSERT 	Seminars on
				INSERT INTO 		Seminars (ID, [Title], [Description], PlannedDate, Author ) 
						VALUES 
						(1, 'Tjäna pengar med flumkurser'	,'Otroligt seminarie om modern utbildning!'	, '2020-06-10T15:00:00.000', 1),				--Does ID really have to be inserted? 
						(2, 'Starta Eget'					,'Den bästa seminarien om starta eget.'		, '2020-08-10T10:00:00.000', 1),
						(3, 'Zombiehantering 101'			,'En snabbkurs inför jordens undergång!'	, '2020-02-10T20:00:00.000', 1);
				SET IDENTITY_INSERT Seminars OFF
				GO


					--(6, 'Tjäna pengar med flumkurser!'			,'Otroligt seminarie om modern utbildning!' ,9999999423999,'StadsBiblioteketAB'	,19999	,350 ,5 ,1),

					--(8, 'Starta Eget'							,'Den bästa seminarien om starta eget.'		,9999999423999,'StadsBiblioteketAB'	,600	,350 ,4 ,1);

	DROP TABLE IF EXISTS dbo.Cards				
	CREATE TABLE Cards(
		ID 				INT IDENTITY(1,1) 	NOT NULL PRIMARY KEY,									  	--Candidatekey
		ActivationDate 	DATETIME 			NOT NULL DEFAULT SYSDATETIME(),
		Activated 		BIT 				NOT NULL DEFAULT 1,
		[Owner] 		INT 			   	NOT NULL FOREIGN KEY REFERENCES Users(ID) UNIQUE, 					--Many2One

 

		--Calculated Column
		ValidUntil AS	DATEADD(year, 1, ActivationDate),
		CardNumber AS FORMAT(ID * 2867585817934888 % 8687931395129801, '0000-0000-0000-0000') --Calculated with (mod p) . Inverse is 7654321234567890
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

	--Relationship USER-2-OBJECT ; M2M
	DROP TABLE IF EXISTS dbo.Categories
	CREATE TABLE Categories(
		ID				INT IDENTITY(1,1)	NOT NULL PRIMARY KEY,															--Candidatekey
		Name 			VARCHAR(50) 		NOT NULL 				DEFAULT 'UNNAMED' UNIQUE, 								--Candidatekey
		[Description] 	varchar(500) 								DEFAULT 'No Description',
		);
		--STANDARD INSERTS
		SET IDENTITY_INSERT 	Categories on
			INSERT INTO 		Categories (ID,[Name], Description) 
					VALUES 
					(1, 'Papers Bok','Alla typer av böcker'							),										--Does ID really have to be inserted? 
					(2, 'Ebok'	  	,'Eböcker som går att ladda ner.'				),
					(3, 'Ljudbok'	,'Ljudböcker som går att lyssna på'				),
					(4, 'Film'		,'Filmer som går att låna eller ladda ner'		);
			SET IDENTITY_INSERT Categories OFF
			GO
	
	DROP TABLE IF EXISTS dbo.DeweyDecimals
	CREATE TABLE DeweyDecimals(
		ID 				INT IDENTITY(1,1) 		NOT NULL PRIMARY KEY 	,
		DeweyINT 		INT 					NOT NULL UNIQUE 		,
		[Description] 	VARCHAR(300) 			NOT NULL 				,
		
		--Shows 004 correctly
		DeweyDecimal AS FORMAT(DeweyINT, '000')
		); 
		GO



	--
	--


	DROP TABLE IF EXISTS dbo.LibraryObjects
	CREATE TABLE LibraryObjects(
		ID 				INT 		IDENTITY(1,1) 				PRIMARY KEY,	--Candidatekey1
		Title 			VARCHAR(50) NOT NULL 	DEFAULT 'UNNAMED', 
		[Description] 	varchar(MAX) 			DEFAULT 'No Description',
		ISBN 			BIGINT 				 	DEFAULT 0,						
		Publisher		VARCHAR(100) 			DEFAULT 'UNNAMED',
		PurchasePrice 	FLOAT 					DEFAULT 300,
		Pages 			INT 					DEFAULT 0,

		Dewey			INT 	 			 					FOREIGN KEY REFERENCES DeweyDecimals(DeweyINT),
		Category 		INT 		NOT NULL	DEFAULT 1 		FOREIGN KEY REFERENCES Categories(ID) ,
		Author 			INT 		 			DEFAULT 1 		FOREIGN KEY REFERENCES Authors(ID) 		ON DELETE SET NULL,

		imagesrc VARCHAR(500), 
		--Hardcover 		INT 					DEFAULT 1 		FOREIGN KEY REFERENCES Covers(ID) 		ON DELETE SET NULL,
		--Genre 			INT 		 			DEFAULT 1 		FOREIGN KEY REFERENCES Genres(ID) 		ON DELETE SET NULL,
		-- META attributes -- Must be here due to the 1..1 -> 1..1 relationship. Cannot be in separate Table.
		-- Solution: Can be replaced with "history tables and history-triggers".
		[Library] 		INT 				 	DEFAULT 1 		FOREIGN KEY REFERENCES Libraries(ID) 	ON DELETE SET NULL,
		[AddedBy] 		INT 	 	NOT NULL 	DEFAULT 1 		FOREIGN KEY REFERENCES Users(ID),
		[LastEdited] 	DATETIME 	NOT NULL 	DEFAULT SYSDATETIME(),
		[DateAdded] 	DATETIME 	NOT NULL 	DEFAULT SYSDATETIME()
		--Constraints
		);
		GO

	--
	--Relationship Card-2-OBJECT ; M2M
	CREATE TABLE Loans(
		ID				INT IDENTITY(1,1)	NOT NULL PRIMARY KEY,
		ObjectID		INT 				NOT NULL FOREIGN KEY REFERENCES LibraryObjects(ID)  ON DELETE CASCADE,
		CardID			INT 				NOT NULL FOREIGN KEY REFERENCES Cards(ID)  			ON DELETE CASCADE,
		LoanDate		DATE				NOT NULL DEFAULT 				SYSDATETIME(),   

		--if the user has returned a book. 
		Returned BIT NOT NULL DEFAULT 0,
		--Computed columns
		--ExpirationDate
		--isLate
	 	CONSTRAINT UC_CardObjectCombo UNIQUE (CardID,ObjectID)
		);
		GO

	--Relationship User-2-Seminar ; M2M
	CREATE TABLE SeminarBookings(
		ID				INT IDENTITY(1,1)	NOT NULL PRIMARY KEY 												,
		BookingDate		DATE				NOT NULL DEFAULT				SYSDATETIME() 						,   
		SeminarID		INT 				NOT NULL FOREIGN KEY REFERENCES Seminars(ID)  	ON DELETE CASCADE	,
		UserID			INT 				NOT NULL FOREIGN KEY REFERENCES Users(ID)  		ON DELETE CASCADE	,

		--if the user has returned a book, or visited a seminar. Or in any other way fulfilled his deal of the contract. 
		AttENDed BIT NOT NULL DEFAULT 0
		
		--Computed columns
			--ExpirationDate
			--isLate

		);

	--Contains all the attachments , .pdfs etc..
	CREATE TABLE FilesTable(
		ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,				--Candidatekey
		FilePayload Binary NOT NULL UNIQUE						--Candidatekey	
		);
	--
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
--SQL PROCEDURES -------------------------------------------------------
---------------Users ---------------------------------------------------
	Create proc usp_getall_users
		as
		BEGIN
			select * from users;
		END
		GO

	--Add User
	Create proc usp_insert_users
		@ID int = null,
		@LoanID numeric = null,
		@Email varchar(300),
		@Password varchar(20),
		@Firstname varchar(20),
		@Lastname varchar(20),
		@UserType int,
		@LoggedIn bit,
		@NewID int Output
		as
		BEGIN
			insert into users (Email, [Password], Firstname, Lastname, UserType, LoggedIn)
			values (@Email, @Password, @Firstname, @Lastname, @UserType, @LoggedIn);
			select @NewID = SCOPE_IDENTITY();
		END
		GO

	--Remove user
	Create proc usp_remove_users
		@ID int
		as
		BEGIN
			delete from users where ID = @ID
		END
		GO

	--Update user . Dapper requires all attributes when handling entire objects.
	Create proc usp_update_users
		@ID int,
		@LoanID numeric = null,
		@Email varchar(300),
		@Password varchar(20),
		@Firstname varchar(20),
		@Lastname varchar(20),
		@UserType int,
		@LoggedIn bit
		as
		BEGIN
			Update users
			Set Email = @Email, 
				[Password] = @Password, 
				Firstname = @Firstname, 
				Lastname = @Lastname, 
				UserType = @UserType, 
				LoggedIn = @LoggedIn
			Where ID = @ID;
		END
		GO
	--
	Create proc usp_verifylogin_users
		@Email varchar(300),
		@Password varchar(20)
		as
		BEGIN
			SELECT * FROM Users WHERE
			CAST(Email as varbinary(100)) = CAST(@Email as varbinary(100))
			AND CAST([Password] as varbinary(100)) = CAST(@Password as varbinary(100))
			AND Email = @Email 
			AND [Password] = @Password
		END
		GO
	--
	Create proc usp_verifyemail_users
		@Email varchar(300)
		as
		BEGIN
			select count(1) from users where email = @email;
		END
		GO

	CREATE PROC usp_OwnsCard(
		@ID int
		)
		AS
		BEGIN
			SELECT *
			FROM Cards
			WHERE  Cards.Owner IN 
				(
				SELECT ID
				FROM Users
				WHERE ID=@ID
				);
		END
		GO
------------------------------------------------------------------------
---------------Usertypes -----------------------------------------------
	Create proc usp_getall_usertypes
		as
		BEGIN
			select * from usertypes;
		END
		GO

	--Delete Usertype
	CREATE PROC usp_delete_usertypes
		@ID int = null
		AS
		BEGIN
			IF (@ID IS NOT NULL)	--Only delete user if there is a userID provided
	   			BEGIN
				DELETE FROM Users WHERE ID=@ID; 		--Might still crash if there is no user with this userid
				END
		END
		GO
	--Add Usertype
	CREATE PROC usp_insert_usertypes
		@ID 	INT = null,
		@Name 	VARCHAR(50)
		AS
		BEGIN
			insert into UserType ([Name])
			values (@Name);
		END
		GO
	--
	CREATE PROC usp_update_usertypes
		@ID 	INT ,
		@Name 	VARCHAR(50)	= null
		AS
		BEGIN
			UPDATE 	UserTypes
			SET 	Name = @Name
			WHERE 	ID = @ID; 
		END
		GO
------------------------------------------------------------------------
---------------Loans ---------------------------------------------------
	--ADD Loans for objectID: using a cardID and a LoansDate.
	CREATE PROC usp_insert_loans
		@ID int 				 = null,
		@Returned 		BIT 	 = null,
		@LoanDate 		DATETIME = null,
		@ObjectID		INT,
		@CardID			INT
		AS

		BEGIN
			INSERT INTO Loans (LoanDate, ObjectID, CardID)
			VALUES (@LoanDate, @ObjectID, @CardID);
		END
		GO
	
	--DELETE Loans for user
	CREATE PROC usp_remove_loans
		@ID int = null
		AS
		BEGIN
			IF (@ID IS NOT NULL)	--Only delete user if there is a userID provided
	   			BEGIN
				DELETE FROM Loans WHERE ID=@ID; 		--Might still crash if there is no user with this userid
				END
		END
		GO
	
	--Edit Loans: Change Loansdate, returned value etc..
	CREATE PROC usp_update_loans
		@ID 			 	INT  	= null,
		@LoanDate		 	DATE	= null,
		@ObjectID 			INT 	= null,
		@CardID				INT 	= null, 
		@Returned 			BIT 	= null

		AS
		BEGIN
			UPDATE Loans
			SET LoanDate 		= @LoanDate, 
				ObjectID 		= @OBjectID,
				CardID 			= @CardID,
				Returned 		= @Returned
			WHERE ID=@ID; 
		END
		GO


	CREATE PROC usp_getloanobjects_users(@ID INT)
	AS
    BEGIN

        SELECT *
        FROM LibraryObjects
        WHERE ID IN
        (
            SELECT ObjectID
            FROM LOANS
            WHERE  Loans.CardID IN
            (
                SELECT Cards.ID
                FROM Cards
                WHERE Cards.Owner = @ID
            )
        )
    END
    GO

    Create PROC usp_getloans_users(
    @ID INT)
    AS
    BEGIN
        SELECt *
        FROM Loans
        WHERE CardID IN 
                ( 
                SELECT CardID
                FROM Cards as C
                WHERE Loans.CardID = C.ID and c.ID = @ID
                )

    END
    GO

------------------------------------------------------------------------
---------------Cards ---------------------------------------------------
	--Delete the card for user
	CREATE PROC usp_remove_cards
		@ID int = null
		AS
		BEGIN
			IF (@ID IS NOT NULL)	--Only delete user if there is a userID provided
	   			BEGIN
				DELETE FROM Cards WHERE ID=@ID; 		--Might still crash if there is no user with this userid
				END
		END
		GO

		--Just add a new card. Only userID required.
	--
	CREATE PROC usp_insert_cards
		@ID 			int 		=null,
		@ActivationDate DATETIME 	=null,
		@Activated 		BIT 		=null,	
		@Owner 			INT 		=null

		AS
		BEGIN
			INSERT INTO 
			Cards([Owner]) VALUES (@Owner);
		END
		GO

		--Disable a specific card. 
	--
	CREATE PROC usp_update_cards
		@ID 			int 		=null,
		@ActivationDate DATETIME 	=null,
		@Activated 		BIT 		=null,	
		@Owner 			INT 		=null,
		@ValidUntil		DateTime 	=null,
		@cardnumber 	Varchar(30) = null
		AS
		BEGIN
			UPDATE Cards
			SET ActivationDate 	=@ActivationDate,
				Activated 		=@Activated,	
				Owner 			=@Owner
			WHERE ID=@ID; 
		END
		GO

------------------------------------------------------------------------
---------------Authors--------------------------------------------------
	Create proc usp_getbyID_authors
		@ID int
		as
		BEGIN
			select * from authors where ID = @ID
		END
		GO	
------------------------------------------------------------------------
---------------LIBRARY OBJECTS -----------------------------------------
	Create proc usp_getall_libraryobjects
		as
		BEGIN
			select * from libraryobjects
		END
		GO
	Create proc usp_getbyID_libraryobjects
		@ID int
		as
		BEGIN
			select * from libraryobjects where ID = @ID
		END
		GO	
	Create proc usp_remove_libraryobjects
	    @ID INT
	    AS
	    BEGIN
	        DELETE FROM libraryobjects where ID = @ID
	    END
	    GO
	CREATE PROC usp_insert_libraryobjects
		@ID 			INT				= null,
		@Title			INT 			,
		@Description 	VARCHAR(500)	= null,
		@ISBN			BIGINT			= null,
		@Publisher		VARCHAR(100)	= null,
		@PurchasePrice	FLOAT			= null,
		@Pages			INT				= null,
		@Dewey			INT				= null,
		@Category		INT	      		,	
		@Author			INT				= null,
		@Imagesrc 		VARCHAR(500)	= null,
		@Library		INT				= null,
		@AddedBy		INT				= null,
		@LastEdited		DATETIME		= null,
		@DateAdded		DATETIME		= null
		AS
		BEGIN
			INSERT INTO 
			LibraryObjects(Title, [Description], ISBN, Publisher, PurchasePrice, Pages, Dewey, Category, Author, imagesrc, [Library], AddedBy, LastEdited, DateAdded) 
			VALUES 
			(@Title, @Description, @ISBN, @Publisher, @PurchasePrice, @Pages, @Dewey, @Category, @Author,@Imagesrc, @Library, @AddedBy, @LastEdited, @DateAdded);
		END
		GO
	CREATE PROC usp_update_libraryobjects
		@ID 			INT,
		@Title			INT,
		@Description 	Varchar(500)	= null,
		@ISBN			BIGINT			= null,
		@Publisher		VARCHAR(100)	= null,
		@PurchasePrice	FLOAT			= null,
		@Pages			INT				= null,
		@Dewey			INT				= null,
		@Category		INT,	
		@Author			INT				= null,
		@Imagesrc 		VARCHAR(500)	= null,
		@Library		INT				= null,
		@AddedBy		INT				= null,
		@LastEdited	DATETIME		= null,
		@DateAdded	DATETIME		= null

		AS
		BEGIN
		Update LibraryObjects 
		SET
			Title 			= @Title,
			[Description] 	= @Description,
			ISBN 			= @ISBN,
			Publisher 		= @Publisher,
			PurchasePrice 	= @PurchasePrice,
			Pages 			= @Pages,
			Dewey 			= @Dewey,
			Category 		= @Category,
			Author 			= @Author,
			imagesrc		= @Imagesrc,
			[Library] 		= @Library,
			AddedBy 		= @AddedBy,
			LastEdited 		= @LastEdited,
			DateAdded 		= @DateAdded
		WHERE ID=@ID; 
		END
		GO
------------------------------------------------------------------------
---------------Category ------------------------------------------------
	create proc usp_getall_category
		as
		BEGIN
			select * from Categories;
		END
		GO

------------------------------------------------------------------------
---------------Search/Data structures ----------------------------------
	--Dynamic search of users
		--TODO : make it look for  --
		--				loandID LIKE %bla
		-- 				or card owner for CARD LIKE %bla543

	--search with multiple parameters and filter
	CREATE PROC usp_filtersearch_users
		@PartialName VARCHAR(20) = '',
		@PartialLastName VARCHAR(20) = '',
		@PartialEmail VARCHAR(300) = ''

		AS
		BEGIN
			SELECT *
			FROM Users
			WHERE 
				Firstname LIKE '%' + @PartialName + '%' AND
				Lastname LIKE '%' + @PartialLastName + '%' AND
				[Email] LIKE '%' + @PartialEmail + '%';
		END
		GO


	--Simple search users
	CREATE PROC usp_simplesearch_users
		@search VARCHAR(20) = ''

		AS
		BEGIN
			SELECT *
			FROM Users
			WHERE 
				Firstname LIKE '%' + @search + '%' OR
				Lastname LIKE '%' + @search + '%' OR
				[Email] LIKE '%' + @search + '%';
		END
		GO

	-------- BELOW IS STILL UNDER CONSTRUCITON:
	--Dynamic search of objects - multiple keywords (i.e conjunctive filter search)

	IF OBJECT_ID('smart_filter_Search', 'P') IS NOT NULL
	    DROP PROCEDURE smart_filter_Search;
	    GO

	    --Dynamic Search directly to table
	CREATE PROC smart_filter_Search(
	    @Title          VARCHAR(MAX) = NULL , 
	    @Description    VARCHAR(MAX) = NULL, 
	    @ISBN           INT          = NULL, 
	    @Publisher      VARCHAR(MAX) = NULL, 
	    @Dewey          INT          = NULL, 
	    @Category       INT          = NULL, 
	    @Author         INT			 = NULL,
		@Library 		INT			 = NULL,
		@AddedBy 		INT			 = NULL,
		
		--All that that were added since this date
		@DateAdded DateTime			 = NULL,

		--All that were edited since this date
		@LastEdited Datetime		 = NULL
		)
	    AS
	    BEGIN

   		--To initiate the predicate with simple tautology.
	    DECLARE @tmpsql VARCHAR(MAX) = ' 1=1 '
	    
	    --Build up a string
	    IF @Title is NOT NULL
	        SET @tmpsql = @tmpsql   +  ' AND Title          LIKE + ''%'   +       @Title         + '%'''
	    IF @Description is NOT NULL
	        SET @tmpsql = @tmpsql   +  ' AND [Description]  LIKE + ''%'   +       @Description   + '%''' 
	    
	    IF @ISBN is NOT NULL
	        SET @tmpsql = @tmpsql   + ' AND ISBN              =  + '      + CAST( @ISBN as VARCHAR(MAX))
	    IF @Publisher is NOT NULL
	        SET @tmpsql = @tmpsql   + ' AND Publisher       LIKE + '      +       @Publisher     + '%'''       
	    
	    IF @Dewey is NOT NULL
	        SET @tmpsql = @tmpsql   + ' AND Dewey             =  + '      +  CAST(@Dewey   as VARCHAR(MAX))      
	    
		IF @Category is NOT NULL
	        SET @tmpsql =  @tmpsql  + ' AND Category          =  + '      +  CAST(@Category as VARCHAR(MAX))        
	    
	    IF @Author is NOT NULL
	        SET @tmpsql =  @tmpsql  + ' AND Author            =  + '      +  CAST(@Author as VARCHAR(MAX))             

	    IF @Library is NOT NULL
	        SET @tmpsql =  @tmpsql  + ' AND Library             =  + '     + CAST(@Library as VARCHAR(MAX))             

	    IF @AddedBy is NOT NULL
	        SET @tmpsql =  @tmpsql  + ' AND AddedBy            =  + '      +  CAST(@AddedBy as VARCHAR(MAX))             


	    IF @DateAdded is NOT NULL
	        SET @tmpsql = @tmpsql   + ' AND [DateAdded] BETWEEN '' ' + CAST(@DateAdded  as VARCHAR(MAX))  + ' '' AND  SYSDATETIME() ' 
	    
		IF @LastEdited is NOT NULL
	        SET @tmpsql = @tmpsql   + ' AND [LastEdited] BETWEEN '' ' + CAST(@LastEdited  as VARCHAR(MAX))  + ' '' AND  SYSDATETIME() ' 

	    DECLARE  @tmpsql2 VARCHAR(MAX);
	    SET @tmpsql2 = 'SELECT * FROM LibraryObjects WHERE' + @tmpsql;
	 

	    --Execute the string
	    EXEC(@tmpsql2)
		
		END
		GO

	--TODO , Search with partial ISBN too !!
	--Dynamic search of objects - only one keyword
	CREATE PROC usp_simplesearch_libraryobjects
		@search VARCHAR(20) = ''

		AS
		BEGIN
			SELECT *
			FROM LibraryObjects
			WHERE 
				Title 			LIKE '%' + @search + '%' Or
				[Description] 	LIKE '%' + @search + '%' OR
				[Publisher] 	LIKE '%' + @search + '%' ;
				--ISBN 			LIKE '%' + @PartialISBN + '%' ;   			  --FIX THIS
				--Author.fname 	LIKE '%' + @PartialAuthorfirstname 	+ '%' ;   --FIX THIS
				--Author.lname 	LIKE '%' + @PartialAuthorlastname 	+ '%' ;   --FIX THIS

		END
		GO

	--Dynamic search of Loans
		-- CREATE PROC usp_findusers_Loans
		-- 	@dateStart DateTime =  DATEADD(year, -1,GETDATE()), --default intervall is +-1 year from now.
		-- 	@dateEND DateTime, =  DATEADD(year, 1,GETDATE()),
			
		-- 	@commacategory VARCHAR(50), 					--such as '2 , 5 , 1'
		-- 	@PartialCardnumber VARCHAR(50), 				--such as '2363'
		-- 	@Fulfilled BIT =null, 							--such as 0
		--	 AS
		--	 BEGIN


		-- 		SELECT *
		-- 		FROM Loans
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
		--	 		END
		--	 END
		--	 GO


		-- 		--use this...
		-- 		-- objectcategory IN STRING_SPLIT( 'hey,cool,guy', ',')  -- splittar upp den i tre nya rader.

		-- 		--SELECT DATEADD(YEAR, -1, DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) -3, 0))  --- Ger datum "1 år sen och 3 månader tillbaka"

		-- 		--SELECT dateadd(year,-1)

------------------------------------------------------------------------
--SQL DATA CONSTANTS ---------------------------------------------------
-------DEWEY DECIMAL INSERTS-----------
			--SET IDENTITY_INSERT [Usertypes] on
				INSERT INTO DeweyDecimals(DeweyINT ,[Description]) 
				VALUES 

				(100, 'filosofi och psykologi'),
				(101, 'Theory of Philosophy'),
				(102, 'Diverse filosofi'),
				(103, 'Filosofiska ordböcker och uppslagsverk'),
				(104, 'används inte längre [tidigare Essays]'),
				(105, 'seriella publikationer av filosofi'),
				(106, 'Organisationer och hantering av filosofi'),
				(107, 'Utbildning, forskning och relaterade filosofiska ämnen'),
				(108, 'Typer av personers behandling av filosofi'),
				(109, 'Historisk och insamlad persons behandling av filosofi'),
				(110, 'Metaphysics'),
				(112, 'Används inte längre [tidigare metod]'),
				(113, 'kosmologi (naturens filosofi)'),
				(114, 'Space'),
				(115, 'Tid'),
				(117, 'Structure'),
				(120, 'epistemologi, orsakssamband och mänskligheten'),
				(121, 'kunskapsteori'),
				(122, 'Orsakssamband'),
				(123, 'Determinism och indeterminism'),
				(125, 'används inte längre [tidigare Infinity]'),
				(126, 'jaget'),
				(127, 'Det omedvetna och det undermedvetna'),
				(129, 'ursprung och öde för enskilda själar'),
				(130, 'Parapsychology and occultism'),
				(131, 'parapsykologiska och ockulta metoder'),
				(132, 'Används inte längre [tidigare Mental derangements]'),
				(133, 'specifika ämnen inom parapsykologi och ockultism'),
				(134, 'Används inte längre [tidigare mesmerism och klärvojans]'),
				(135, 'Drömmar och mysterier'),
				(136, 'Används inte längre [tidigare Mentala egenskaper]'),
				(138, 'fysionomi'),
				(140, 'specifika filosofiska skolor'),
				(141, 'idealism och relaterade system'),
				(142, 'Kritisk filosofi'),
				(143, 'Bergsonism och intuitionism'),
				(144, 'humanism och relaterade system'),
				(145, 'sensationalism'),
				(146, 'Naturalism och relaterade system'),
				(147, 'panteism och relaterade system'),
				(148, 'liberalism, eklektism och traditionism'),
				(149, 'Andra filosofiska system'),
				(150, 'psykologi'),
				(151, 'Används inte längre [tidigare Intellekt]'),
				(152, 'Perception, rörelse, känslor och drivningar'),
				(153, 'mentala processer och intelligens'),
				(154, 'undermedvetna och förändrade tillstånd'),
				(155, 'Differential- och utvecklingspsykologi'),
				(156, 'Jämförande psykologi'),
				(157, 'Används inte längre [tidigare känslor]'),
				(158, 'Tillämpad psykologi'),
				(159, 'används inte längre [tidigare Will]'),
				(161, 'Induktion'),
				(162, 'Avdrag'),
				(165, 'misslyckanden och felkällor'),
				(166, 'Syllogismer'),
				(167, 'hypoteser'),
				(168, 'Argument och övertalning'),
				(170, 'etik'),
				(171, 'Etiska system'),
				(172, 'Politisk etik'),
				(173, 'Etik för familjerelationer'),
				(174, 'Yrkesetik'),
				(175, 'Etik för rekreation och fritid'),
				(176, 'Etik för kön och reproduktion'),
				(177, 'Etik för sociala relationer'),
				(178, 'konsumtionsetik'),
				(179, 'Andra etiska normer'),
				(180, 'forntida, medeltida och östlig filosofi'),
				(181, 'östlig filosofi'),
				(182, 'pre-socratic grekiska filosofier'),
				(183, 'Socratic och relaterade filosofier'),
				(184, 'Platonisk filosofi'),
				(185, 'Aristoteliansk filosofi'),
				(186, 'Skeptiska och Neoplatoniska filosofier'),
				(187, 'Epikurisk filosofi'),
				(188, 'stoisk filosofi'),
				(189, 'Medeltida västerländsk filosofi'),
				(190, 'modern västerländsk filosofi'),
				(191, 'Modern Western filosofi i USA och Kanada'),
				(192, 'Modern Western filosofi på de brittiska öarna'),
				(193, 'modern och västerländsk filosofi om Tyskland och Österrike'),
				(194, 'Modern Western Philosophy of France'),
				(195, 'Modern Western Philosophy of Italy'),
				(196, 'modern och västerländsk filosofi om Spanien och Portugal'),
				(197, 'modern västerländsk filosofi om fd Sovjetunionen'),
				(198, 'Modern Western filosofi om Skandinavien'),
				(199, 'Modern Western filosofi i andra geografiska områden'),
				(201, 'Religiös mytologi, allmänna klasser av religion, interreligiösa relationer och attityder, social teologi'),
				(202, 'Doktriner'),
				(203, 'Allmän tillbedjan och andra metoder'),
				(204, 'Religiös upplevelse, liv, praktik'),
				(205, 'Religiös etik'),
				(206, 'ledare och organisation'),
				(207, 'beskickningar och religionsundervisning'),
				(208, 'Källor'),
				(209, 'Sektioner och reformrörelser'),
				(211, 'Guds begrepp'),
				(212, 'Existence, attribut of God'),
				(215, 'Vetenskap & religion'),
				(216, 'Används inte längre [tidigare ondska]'),
				(217, 'Inte längre använt [tidigare bön]'),
				(219, 'Används inte längre [tidigare Analogier]'),
				(221, 'Gamla testamentet'),
				(222, 'Historiska böcker från Gamla testamentet'),
				(223, 'Poetic books of Old Testament'),
				(224, 'Profetiska böcker från Gamla testamentet'),
				(225, 'Nya testamentet'),
				(226, 'Evangelier och handlingar'),
				(227, 'epistlar'),
				(228, 'Uppenbarelse (Apocalypse)'),
				(229, 'Apocrypha & pseudepigrapha'),
				(232, 'Jesus Kristus och hans familj'),
				(234, 'Frälsning (Soteriologi) & nåd'),
				(235, 'Andliga varelser'),
				(236, 'Eskatologi'),
				(237, 'Används inte längre [tidigare framtidsstat]'),
				(238, 'trosbekännelser och katekismer'),
				(239, 'Apologetics & polemics'),
				(243, 'Evangelistiska skrifter för individer'),
				(244, 'Används inte längre [tidigare religiös fiktion]'),
				(245, 'Används inte längre [tidigare Hymnology]'),
				(246, 'Användning av konst i kristENDomen'),
				(247, 'kyrkliga möbler och artiklar'),
				(248, 'kristen erfarenhet, praxis, liv'),
				(249, 'kristna observationer i familjelivet'),
				(251, 'Predikande (Homiletik)'),
				(252, 'Texter till predikningar'),
				(253, 'Pastoral office (Pastoral theology)'),
				(254, 'Parish government & administration'),
				(255, 'Religiösa församlingar och order'),
				(256, 'Inte längre använt [tidigare religiösa samhällen]'),
				(257, 'Inte längre använt [tidigare parochiala skolor, bibliotek etc.]'),
				(258, 'Används inte längre [tidigare parokial medicin]'),
				(259, 'den lokala kyrkans aktiviteter'),
				(261, 'Socialteologi'),
				(262, 'Ecclesiology'),
				(263, 'Times, religiösa observationer'),
				(264, 'offentligt dyrkan'),
				(265, 'Sakramenter, andra ritualer och handlingar'),
				(266, 'beskickningar'),
				(267, 'Föreningar för religiöst arbete'),
				(268, 'Religionsundervisning'),
				(269, 'Andlig förnyelse'),
				(271, 'Religiösa ordningar i kyrkans historia'),
				(272, 'Förföljelser i kyrkans historia'),
				(273, 'Kätterier i kyrkans historia'),
				(274, 'Christian kyrka i Europa'),
				(275, 'Christian kyrka i Asien'),
				(276, 'Christian kyrka i Afrika'),
				(277, 'Christian kyrka i Nordamerika'),
				(278, 'Christian kyrka i Sydamerika'),
				(279, 'Christian kyrka i andra områden'),
				(281, 'Tidiga kyrkor och östra kyrkor'),
				(282, 'Romersk-katolska kyrkan'),
				(283, 'anglikanska kyrkor'),
				(284, 'protestanter av kontinentalt ursprung'),
				(285, 'presbyterian, reformerad, kongregationell'),
				(286, 'baptist, Kristi lärjungar, adventist'),
				(287, 'Metodist & besläktade kyrkor'),
				(288, 'Används inte längre [tidigare Unitarian]'),
				(289, 'Andra valörer och sekter'),
				(292, 'klassisk (grekisk och romersk) religion'),
				(293, 'germansk religion'),
				(294, 'religioner av indiskt ursprung'),
				(295, 'Zoroastrianism (Mazdaism, Parseeism)'),
				(296, 'judENDomen'),
				(297, 'Islam och österländsk Tro'),
				(298, 'Används inte längre [tidigare mormonism]'),
				(299, 'Andra religioner'),
				(300, 'Samhällsvetenskapen'),
				(301, 'sociologi & antropologi'),
				(302, 'Social interaktion'),
				(303, 'Sociala processer'),
				(304, 'Faktorer som påverkar socialt beteENDe'),
				(305, 'sociala grupper'),
				(306, 'Kultur & institutioner'),
				(307, 'gemenskaperna'),
				(308, 'Används inte längre [tidigare polygrafi]'),
				(309, 'Används inte längre [tidigare sociologihistoria]'),
				(311, 'Används inte längre [tidigare teori och metoder]'),
				(312, 'Används inte längre [tidigare befolkningen]'),
				(313, 'Används inte längre [tidigare specialämnen]'),
				(314, 'Allmän statistik över Europa'),
				(315, 'Allmän statistik över Asien'),
				(316, 'Allmän statistik över Afrika'),
				(317, 'Allmän statistik över Nordamerika'),
				(318, 'Allmän statistik över Sydamerika'),
				(319, 'Allmän statistik över andra delar av världen'),
				(321, 'Systems of Government & States'),
				(322, 'Statens förhållande till organiserade grupper'),
				(323, 'medborgerliga och politiska rättigheter'),
				(324, 'Den politiska processen'),
				(325, 'Internationell migration och kolonisering'),
				(326, 'Slaveri och frigörelse'),
				(327, 'Internationella förbindelser'),
				(328, 'lagstiftningsprocessen'),
				(331, 'Arbetsekonomi'),
				(332, 'Finansiell ekonomi'),
				(333, 'markekonomi'),
				(334, 'kooperativ'),
				(335, 'Socialism och relaterade system'),
				(337, 'Internationell ekonomi'),
				(339, 'makroekonomi och relaterade ämnen'),
				(342, 'konstitutionell och administrativ rätt'),
				(343, 'militär, skatt, handel, industrilag'),
				(344, 'social, arbetskraft, välfärd och relaterad lag'),
				(347, 'Civilförfarande och domstolar'),
				(348, 'lag (stadgar), förordningar, ärENDen'),
				(349, 'lag i specifika jurisdiktioner och områden'),
				(351, 'av centralregeringar'),
				(352, 'av lokala myndigheter'),
				(353, 'av amerikanska federala och statliga regeringar'),
				(354, 'av specifika centralregeringar'),
				(355, 'Militärvetenskap'),
				(356, 'Fotstyrkor och krigföring'),
				(357, 'monterade styrkor och krigföring'),
				(358, 'Övriga specialstyrkor och tjänster'),
				(359, 'Sea (Naval) styrkor och krigföring'),
				(361, 'Allmänna sociala problem'),
				(362, 'sociala välfärdsproblem och tjänster'),
				(363, 'Andra sociala problem och tjänster'),
				(365, 'straff- och relaterade institutioner'),
				(366, 'Association'),
				(367, 'Allmänna klubbar'),
				(368, 'Insurance'),
				(369, 'Diverse typer av föreningar'),
				(371, 'skolhantering; specialundervisning'),
				(373, 'gymnasieutbildning'),
				(375, 'Läroplaner'),
				(376, 'Används inte längre [tidigare utbildning av kvinnor]'),
				(377, 'Används inte längre [tidigare etisk utbildning]'),
				(379, 'statlig reglering, kontroll, stöd'),
				(381, 'Intern handel (inrikeshandel)'),
				(383, 'postkommunikation'),
				(384, 'Kommunikation; Telekommunikation'),
				(385, 'järnvägstransporter'),
				(386, 'inre vattenvägar och färjetrafik'),
				(387, 'Vatten, luft, rymdtransport'),
				(388, 'Transport; Marktransport'),
				(389, 'Metrologi & standardisering'),
				(391, 'kostym och personligt utseENDe'),
				(392, 'Tullar för livscykel och hushållsliv'),
				(393, 'dödstullar'),
				(394, 'Allmänna tullar'),
				(395, 'Etikett (Manners)'),
				(396, 'Används inte längre [tidigare kvinnors ställning och behandling]'),
				(397, 'Används inte längre [tidigare utstationerade studier]'),
				(399, 'Customs of war & diplomacy'),
				(401, 'Filosofi & teori'),
				(403, 'ordböcker och uppslagsverk'),
				(404, 'Specialämnen'),
				(406, 'Organisationer och ledning'),
				(407, 'Utbildning, forskning, relaterade ämnen'),
				(408, 'När det gäller typer av personer'),
				(409, 'Geografisk och personbehandling'),
				(411, 'Skrivsystem'),
				(413, 'ordböcker'),
				(415, 'struktursystem (grammatik)'),
				(416, 'Inte längre använt [tidigare Prosody (lingvistik)]'),
				(417, 'Dialektologi & historisk lingvistik'),
				(418, 'Standardanvändning; Tillämpad lingvistik'),
				(419, 'muntligt språk inte talat eller skriftligt'),
				(421, 'engelska skrivsystem och fonologi'),
				(422, 'engelsk etymologi'),
				(423, 'engelska ordböcker'),
				(424, 'Inte längre använt [tidigare engelska ordböcker]'),
				(425, 'engelsk grammatik'),
				(426, 'Används inte längre [tidigare engelska prosodier]'),
				(427, 'engelska variationer'),
				(428, 'Standard engelska användning'),
				(431, 'tyska skrivsystem och fonologi'),
				(433, 'tyska ordböcker'),
				(437, 'tyska språkvariationer'),
				(438, 'tysk standardanvändning'),
				(439, 'andra germanska språk'),
				(441, 'franska skrivsystem och fonologi'),
				(443, 'franska ordböcker'),
				(447, 'franska språkvariationer'),
				(448, 'fransk standardanvändning'),
				(451, 'italiensk skrivningssystem och fonologi'),
				(453, 'italienska ordböcker'),
				(457, 'italienska språkvariationer'),
				(458, 'italiensk standardanvändning'),
				(461, 'spanska skrivsystem och fonologi'),
				(462, 'spansk etymologi'),
				(463, 'spanska ordböcker'),
				(465, 'spansk grammatik'),
				(467, 'spanska variationer'),
				(468, 'Standard spansk användning'),
				(469, 'portugisisk'),
				(471, 'Klassisk latinskrift & fonologi'),
				(472, 'Klassisk latin etymologi & fonologi'),
				(473, 'klassiska latinska ordböcker'),
				(475, 'klassisk latinsk grammatik'),
				(477, 'gammal, postklassisk, vulgär latin'),
				(478, 'klassisk latinsk användning'),
				(479, 'andra kursiv'),
				(481, 'Klassisk grekisk skrift och fonologi'),
				(482, 'Klassisk grekisk etymologi'),
				(483, 'klassiska grekiska ordböcker'),
				(485, 'klassisk grekisk grammatik'),
				(486, 'Ej tilldelad eller inte längre används'),
				(487, 'Preklassical & postclassical Greek'),
				(488, 'klassisk grekisk användning'),
				(489, 'andra helleniska språk'),
				(491, 'östindo-europeiska och keltiska språk'),
				(492, 'afroasiatiska språk; semitiska'),
				(493, 'icke-semitiska afroasiatiska språk'),
				(494, 'Uralä Altaic, Paleosiberian, Dravidian'),
				(495, 'Språk i Öst- och Sydostasien'),
				(496, 'afrikanska språk'),
				(497, 'nordamerikanska modersmål'),
				(498, 'sydamerikanska modersmål'),
				(499, 'Diverse språk'),
				(500, 'Naturvetenskap och matematik'),
				(502, 'Miscellany'),
				(505, 'seriella publikationer'),
				(507, 'Utbildning, forskning, relaterade ämnen'),
				(508, 'Naturhistoria'),
				(509, 'Historisk, geografisk, personbehandling'),
				(511, 'Allmänna principer'),
				(515, 'analys'),
				(518, 'numerisk analys'),
				(519, 'Probabilities & tillämpad matematik'),
				(521, 'himmelmekanik'),
				(522, 'Tekniker, utrustning, material'),
				(523, 'specifika himmelkroppar och fenomen'),
				(525, 'Jorden (astronomisk geografi)'),
				(527, 'Celestial navigation'),
				(528, 'efemerider'),
				(531, 'Klassisk mekanik; Fast mekanik'),
				(532, 'Fluidmekanik; Flytande mekanik'),
				(533, 'Gasmekanik'),
				(534, 'Ljud & relaterade vibrationer'),
				(537, 'Elektricitet och elektronik'),
				(538, 'Magnetism'),
				(539, 'Modern fysik'),
				(541, 'Fysisk och teoretisk kemi'),
				(543, 'Analytisk kemi'),
				(544, 'Kvalitativ analys'),
				(545, 'kvantitativ analys'),
				(546, 'oorganisk kemi'),
				(547, 'Organisk kemi'),
				(548, 'Kristallografi'),
				(554, 'Earth Sciences of Europe'),
				(555, 'Earth Sciences of Asia'),
				(556, 'Earth Sciences of Africa'),
				(557, 'Earth Sciences of North America'),
				(558, 'Earth Sciences of South America'),
				(559, 'Jordvetenskap inom andra områden'),
				(562, 'fossila ryggradslösa djur'),
				(563, 'fossil primitiv phyla'),
				(564, 'Fossil Mollusca & Molluscoidea'),
				(565, 'Andra fossila ryggradslösa djur'),
				(566, 'Fossil Vertebrata (Fossil Craniata)'),
				(567, 'fossila kallblodiga ryggradsdjur'),
				(568, 'Fossil Aves (Fossil fåglar)'),
				(569, 'Fossil Mammalia'),
				(571, 'Physiology'),
				(572, 'Biochemistry'),
				(573, 'djurens fysiologiska system'),
				(575, 'fysiologiska växtsystem'),
				(576, 'Genetik och evolution'),
				(578, 'Naturhistoria av organismer'),
				(579, 'mikroorganismer, svampar, alger'),
				(582, 'Växter noterade för specifika vegetativa egenskaper och blommor'),
				(583, 'Dicotyledones'),
				(584, 'Monocotyledones'),
				(585, 'Gymnospermae (Pinophyta)'),
				(586, 'Cryptogamia (Seedless plants)'),
				(587, 'Pteridophyta (Vaskulära kryptogamer)'),
				(589, 'skogsbruk'),
				(592, 'Ryggradslösa djur'),
				(594, 'Mollusca & Molluscoidea'),
				(595, 'Andra ryggradslösa djur'),
				(596, 'Vertebrata (Craniata, Vertebrates)'),
				(597, 'kallblodiga ryggradsdjur, fiskar'),
				(598, 'Aves (Fåglar)'),
				(599, 'Mammalia (däggdjur)'),
				(608, 'uppfinning och patent'),
				(609, 'Historisk, geografisk, personbehandling'),
				(611, 'Human anatomy, cytology, histology'),
				(612, 'Human fysiologi'),
				(613, 'Personlig hälsa och säkerhet'),
				(614, 'förekomst och förebyggande av sjukdomar'),
				(615, 'farmakologi och terapeutik'),
				(616, 'sjukdomar'),
				(617, 'Kirurgi och relaterade medicinska specialiteter'),
				(618, 'gynekologi och andra medicinska specialiteter'),
				(621, 'Tillämpad fysik'),
				(622, 'gruvdrift och relaterad verksamhet'),
				(625, 'konstruktion av järnvägar, vägar'),
				(628, 'Sanitär- och kommunteknik'),
				(629, 'Övriga verkstadsgrenar'),
				(631, 'Tekniker, utrustning, material'),
				(632, 'Växtskador, sjukdomar, skadedjur'),
				(633, 'fält- och plantager'),
				(634, 'fruktträdgårdar, frukt, skogsbruk'),
				(635, 'trädgårdsgrödor (trädgårdsodling)'),
				(636, 'djuruppfödning'),
				(637, 'Bearbetning av mejeriprodukter och relaterade produkter'),
				(638, 'Insektkultur'),
				(639, 'Jakt, fiske, bevarande'),
				(642, 'måltider & bordservice'),
				(643, 'bostäder och hushållsutrustning'),
				(644, 'Hushållsverktyg'),
				(645, 'hushållsmöbler'),
				(646, 'sömnad, kläder, personligt boENDe'),
				(647, 'förvaltning av offentliga hushåll'),
				(648, 'städning'),
				(649, 'Barnuppfödning och sjukvård i hemmet'),
				(651, 'kontortjänster'),
				(652, 'Processer för skriftlig kommunikation'),
				(653, 'Shorthand'),
				(659, 'Reklam & PR'),
				(661, 'Industriell kemikalieteknik'),
				(662, 'Sprängämnen, bränsleteknologi'),
				(665, 'industriella oljor, fetter, växer, gaser'),
				(666, 'keramik & allierade tekniker'),
				(667, 'rengöring, färg, relaterad teknik'),
				(668, 'Teknologi för andra ekologiska produkter'),
				(671, 'Metallbearbetning och metallprodukter'),
				(672, 'järn, stål, andra järnlegeringar'),
				(673, 'icke-järnmetaller'),
				(674, 'timmerbearbetning, träprodukter, kork'),
				(675, 'Läder- och pälsbearbetning'),
				(676, 'massa & pappersteknologi'),
				(677, 'textilier'),
				(678, 'Elastomerer och elastomerprodukter'),
				(679, 'Andra produkter av specifika material'),
				(681, 'Precisionsinstrument och andra enheter'),
				(682, 'Smedarbeten (smed)'),
				(683, 'Hushållshårdvara och hushållsapparater'),
				(684, 'Inredning & hemverkstäder'),
				(685, 'läder, päls, relaterade produkter'),
				(686, 'Tryck & relaterade aktiviteter'),
				(688, 'Andra slutprodukter och förpackningar'),
				(691, 'Byggnadsmaterial'),
				(692, 'hjälpkonstruktionsmetoder'),
				(693, 'specifika material och ändamål'),
				(694, 'Träkonstruktion; Snickeri'),
				(696, 'Utilities'),
				(698, 'detaljbehandling'),
				(700, 'The Arts; fin & dekorativ konst'),
				(701, 'Filosofi & teori'),
				(703, 'ordböcker och uppslagsverk'),
				(705, 'seriella publikationer'),
				(708, 'Gallerier, museer, privata samlingar'),
				(709, 'Historisk, områden, personbehandling'),
				(711, 'Area Planning (Civic art)'),
				(712, 'Landskapsarkitektur'),
				(713, 'Landskapsarkitektur för trafikvägar'),
				(714, 'vattenfunktioner'),
				(715, 'Woody planter'),
				(716, 'örtartade växter'),
				(717, 'Structures'),
				(718, 'Landskapsdesign av kyrkogårdar'),
				(719, 'Naturliga landskap'),
				(721, 'Arkitektonisk struktur'),
				(725, 'Offentliga strukturer'),
				(726, 'Byggnader för religiösa ändamål'),
				(727, 'Byggnader för utbildning och forskning'),
				(728, 'bostäder och relaterade byggnader'),
				(729, 'Design & dekoration'),
				(731, 'Processer, former, skulpturämnen'),
				(732, 'Skulptur till ca 500'),
				(733, 'grekisk, etruskisk, romersk skulptur'),
				(734, 'Skulptur från ca 500 till 1399'),
				(735, 'Skulptur från 1400'),
				(736, 'Carving & Carvings'),
				(737, 'Numismatics & sigillography'),
				(738, 'Keramisk konst'),
				(741, 'Ritning & ritningar'),
				(742, 'perspektiv (grafiskt)'),
				(743, 'Ritning & teckningar efter ämne'),
				(745, 'dekorativ konst'),
				(746, 'Textilkonst'),
				(748, 'glas'),
				(749, 'Möbler & tillbehör'),
				(751, 'Tekniker, utrustning, formulär'),
				(753, 'symbolism, allegori, mytologi, legEND'),
				(754, 'genrmålningar'),
				(755, 'Religion & religiös symbolik'),
				(757, 'mänskliga figurer och deras delar'),
				(758, 'Andra ämnen'),
				(759, 'Geografiska, historiska, områden, personbehandling'),
				(761, 'lättnadsprocesser (blocktryck)'),
				(763, 'litografiska (planografiska) processer'),
				(764, 'kromolitografi och serigrafi'),
				(766, 'Mezzotinting & relaterade processer'),
				(769, 'tryck'),
				(771, 'Tekniker, utrustning, material'),
				(772, 'Metalliska saltprocesser'),
				(773, 'tryckpigmentprocesser'),
				(778, 'Fält och fotografier'),
				(779, 'Fotografier'),
				(781, 'Allmänna principer och musikaliska former'),
				(782, 'Vokalmusik'),
				(783, 'Musik för enstaka röster; Rösten'),
				(784, 'Instrument & Instrumental ensembles'),
				(785, 'kammarmusik'),
				(786, 'Tangentbord och andra instrument'),
				(787, 'Stränginstrument (Chordophones)'),
				(788, 'Blåsinstrument (aerofoner)'),
				(791, 'Offentliga föreställningar'),
				(792, 'scenpresentationer'),
				(793, 'inomhusspel och nöjen'),
				(794, 'inomhus skicklighetsspel'),
				(795, 'hasardspel'),
				(796, 'Atletisk & utomhussport & spel'),
				(797, 'Vatten- och luftsport'),
				(798, 'hästsport och djurracing'),
				(799, 'Fiske, jakt, skytte'),
				(801, 'Filosofi & teori'),
				(805, 'seriella publikationer'),
				(806, 'Organisationer'),
				(808, 'Retorik & litteratursamlingar'),
				(809, 'litteraturhistoria och kritik'),
				(811, 'Dikt'),
				(814, 'Essays'),
				(815, 'Tal'),
				(816, 'brev'),
				(817, 'Satire & humor'),
				(818, 'Diverse skrifter'),
				(821, 'engelsk poesi'),
				(822, 'engelskt drama'),
				(823, 'engelsk fiktion'),
				(824, 'engelska uppsatser'),
				(825, 'engelska tal'),
				(826, 'engelska bokstäver'),
				(827, 'engelsk satire & humor'),
				(828, 'engelska diverse skrifter'),
				(829, 'Old English (Anglo-Saxon)'),
				(834, 'tyska uppsatser'),
				(835, 'tyska tal'),
				(836, 'tyska bokstäver'),
				(837, 'tysk satir & humor'),
				(838, 'tyska diverse skrifter'),
				(839, 'andra germanska litteraturer'),
				(844, 'franska uppsatser'),
				(845, 'franska anföranden'),
				(846, 'franska bokstäver'),
				(847, 'fransk satir och humor'),
				(848, 'franska diverse skrifter'),
				(849, 'oksitanska och katalanska litteraturer'),
				(854, 'italienska uppsatser'),
				(855, 'italienska anföranden'),
				(856, 'italienska bokstäver'),
				(857, 'italiensk satir och humor'),
				(858, 'italienska diverse skrifter'),
				(861, 'spansk poesi'),
				(862, 'spanska drama'),
				(863, 'spansk fiktion'),
				(864, 'spanska uppsatser'),
				(865, 'spanska tal'),
				(866, 'spanska bokstäver'),
				(867, 'spansk satire & humor'),
				(868, 'spanska diverse skrifter'),
				(869, 'portugisisk litteratur'),
				(875, 'latinska tal'),
				(876, 'latinska bokstäver'),
				(877, 'Latin satire & humor'),
				(878, 'latinska diverse skrifter'),
				(879, 'Litteraturer av andra kursiv'),
				(881, 'Klassisk grekisk poesi'),
				(882, 'klassiskt grekiskt drama'),
				(883, 'Klassisk grekisk episk poesi & fiktion'),
				(884, 'Klassisk grekisk lyrisk poesi'),
				(885, 'klassiska grekiska tal'),
				(886, 'klassiska grekiska bokstäver'),
				(887, 'Klassisk grekisk satir & humor'),
				(888, 'Klassiska grekiska diverse skrifter'),
				(891, 'East Indo-European & Celtic'),
				(892, 'Afro-asiatic literatures Semitic'),
				(893, 'icke-semitiska afroasiatiska litteraturer'),
				(894, 'Uralä Altaic, Paleosiberian, Dravidian'),
				(895, 'Litteraturer i Öst- och Sydostasien'),
				(896, 'afrikanska litteraturer'),
				(897, 'nordamerikanska infödda litteraturer'),
				(898, 'sydamerikanska infödda litteraturer'),
				(899, 'Andra litteraturer'),
				(900, 'Historia & geografi'),
				(902, 'otilldelad'),
				(904, 'Samlade konton över händelser'),
				(909, 'Världshistoria'),
				(911, 'Historisk geografi'),
				(912, 'Grafiska representationer av jorden'),
				(915, 'Asien'),
				(918, 'Sydamerika'),
				(919, 'Övriga områden'),
				(929, 'släktforskning, namn, insignier'),
				(931, 'Historia om forntida världen; Kina'),
				(932, 'Historia om den antika världen; Egypten'),
				(933, 'Historia om forntida världen; Palestina'),
				(934, 'Historia om forntida världen; Indien'),
				(935, 'Historia om forntida världen; Mesopotamia och Iranian Plateau'),
				(936, 'Historia om forntida världen; Europa norr och väster om Italien'),
				(937, 'Historia om forntida världen; Italien och angränsande territorier'),
				(938, 'Historia om den antika världen; Grekland'),
				(939, 'Historia om den antika världen; Andra delar av den antika världen'),
				(941, 'Europas allmänna historia; Brittiska öarna'),
				(942, 'Europas allmänna historia; England och Wales'),
				(943, 'Europas allmänna historia; Centraleuropa; Tyskland'),
				(944, 'Europas allmänna historia; Frankrike och Monaco'),
				(945, 'Europas allmänna historia; italienska halvön och angränsande öar'),
				(946, 'Europas allmänna historia; Iberiska halvön och angränsande öar'),
				(947, 'Europas allmänna historia; Östeuropa; Ryssland'),
				(948, 'Europas allmänna historia; Nordeuropa; Skandinavien'),
				(949, 'Europas allmänna historia; Övriga delar av Europa'),
				(951, 'Asiens allmänna historia; Kina och angränsande områden'),
				(952, 'Asiens allmänna historia; Japan'),
				(953, 'Asiens allmänna historia; Arabiska halvön och angränsande områden'),
				(954, 'Asiens allmänna historia; Sydasien; Indien'),
				(955, 'Asiens allmänna historia; Iran'),
				(956, 'Asiens allmänna historia; Mellanöstern (Nära öst)'),
				(957, 'Asiens allmänna historia; Sibirien (Asiatiska Ryssland)'),
				(958, 'Asiens allmänna historia; Centralasien'),
				(959, 'Asiens allmänna historia; Sydostasien'),
				(961, 'Afrikas allmänna historia; Tunisien och Libyen'),
				(962, 'Afrikas allmänna historia; Egypten och Sudan'),
				(963, 'Afrikas allmänna historia; Etiopien'),
				(964, 'Afrikas allmänna historia; Marocko och Kanarieöarna'),
				(965, 'Afrikas allmänna historia; Algeriet'),
				(966, 'Afrikas allmänna historia; Västafrika & offshoreöar'),
				(967, 'Afrikas allmänna historia; Centralafrika & offshoreöar'),
				(968, 'Afrika: s allmänna historia; Södra Afrika'),
				(969, 'Afrikas allmänna historia; öar i södra Indiska oceanen'),
				(971, 'Nordamerikas allmänna historia; Kanada'),
				(972, 'Nordamerikas allmänna historia; Mellanamerika; Mexiko'),
				(973, 'Nordamerikas allmänna historia; Förenta staterna'),
				(974, 'Nordamerikas allmänna historia; Nordöstra USA'),
				(975, 'Nordamerikas allmänna historia; Sydöstra USA'),
				(976, 'Nordamerikas allmänna historia; södra centrala Förenta staterna'),
				(977, 'Nordamerikas allmänna historia; Nordamerikanska USA'),
				(978, 'Nordamerikas allmänna historia; västra USA'),
				(979, 'Nordamerikas allmänna historia; Great Basin & Pacific Slope'),
				(981, 'Sydamerikas allmänna historia; Brasilien'),
				(982, 'Sydamerikas allmänna historia; Argentina'),
				(983, 'Sydamerikas allmänna historia; Chile'),
				(984, 'Sydamerikas allmänna historia; Bolivia'),
				(985, 'Sydamerikas allmänna historia; Peru'),
				(986, 'Sydamerikas allmänna historia; Colombia och Ecuador'),
				(987, 'Sydamerikas allmänna historia; Venezuela'),
				(988, 'Sydamerikas allmänna historia; Guyana'),
				(989, 'Sydamerikas allmänna historia; Paraguay och Uruguay'),
				(993, 'Allmän historia för andra områden; Nya Zeeland'),
				(994, 'Allmän historia för andra områden; Australien'),
				(995, 'Allmän historia för andra områden; Melanesien; Nya Guinea'),
				(996, 'Allmän historia för andra områden; Övriga delar av Stillahavs-Polynesien'),
				(997, 'Övriga områdes allmänna historia; öarna i Atlanten'),
				(998, 'Övriga områdes allmänna historia; Arktiska öar och Antarktis'),
				(999, 'utomjordiska världar'),
				(079, 'Tidningar i andra geografiska områden'),
				(096, 'Böcker kända för illustrationer'),
				(033, 'på andra germanska språk'),
				(026, 'Bibliotek för specifika ämnen'),
				(078, 'Tidningar i Skandinavien'),
				(063, 'Organisationer i Centraleuropa; i Tyskland'),
				(921, 'Reserverad som valfri plats för biografier, som är hyllor alfabetiskt efter ämnets efternamn.'),
				(922, 'Reserverad som valfri plats för biografier, som är hyllor alfabetiskt efter ämnets efternamn.'),
				(923, 'Reserverad som valfri plats för biografier, som är hyllor alfabetiskt efter ämnets efternamn.'),
				(924, 'Reserverad som valfri plats för biografier, som är hyllor alfabetiskt efter ämnets efternamn.'),
				(925, 'Reserverat som valfri plats för biografier, som är hyllor alfabetiskt efter ämnets efternamn.'),
				(926, 'Reserverad som valfri plats för biografier, som är hyllor alfabetiskt efter ämnets efternamn.'),
				(927, 'Reserverad som valfri plats för biografier, som är hyllor alfabetiskt efter ämnets efternamn.'),
				(928, 'Reserverad som valfri plats för biografier, som är hyllor alfabetiskt efter ämnets efternamn.'),
				(091, 'manuskript'),
				(050, 'Allmänna seriella publikationer'),
				(021, 'Biblioteksrelationer'),
				(057, 'serier på slaviska språk'),
				(083, 'Samlingar på andra germanska språk'),
				(058, 'Serials på skandinaviska språk'),
				(075, 'Tidningar i Italien och angränsande öar'),
				(059, 'Serier på andra språk'),
				(034, 'Encyclopedier på franska, oksitanska och katalanska'),
				(014, 'Bibliografier av anonyma och pseudonyma verk'),
				(077, 'Tidningar i Östeuropa; i Ryssland'),
				(006, 'Särskilda datormetoder'),
				(064, 'Organisationer i Frankrike och Monaco'),
				(084, 'Samlingar på franska, oksitanska och katalanska'),
				(070, 'Nyhetsmedier, journalistik och publicering'),
				(024, 'Används inte längre, tidigare föreskrifter för läsare'),
				(094, 'Tryckta böcker'),
				(072, 'Tidningar på Brittiska öarna; i England'),
				(040, '040-049 alla otilldelade, tidigare samlade uppsatser efter språk'),
				(003, 'System'),
				(023, 'Personal management'),
				(036, 'Encyclopedias på spanska och portugisiska'),
				(016, 'Bibliografier av verk om specifika ämnen'),
				(052, 'serier på engelska'),
				(081, 'Samlingar på amerikansk engelska'),
				(061, 'Organisationer i Nordamerika'),
				(031, 'Encyclopedias på amerikanska engelska'),
				(011, 'Bibliografier'),
				(086, 'Samlingar på spanska och portugisiska'),
				(004, 'Databehandling och datavetenskap'),
				(005, 'Datorprogrammering, program och data'),
				(071, 'Tidningar i Nordamerika'),
				(029, 'Inte längre använt, tidigare litterära metoder'),
				(000, 'datavetenskap, kunskap och allmänna verk'),
				(098, 'Förbjudna verk, förfalskningar och smalor'),
				(028, 'Läsning och användning av andra informationsmedier'),
				(076, 'På den iberiska halvön och angränsande öar'),
				(099, 'Böcker som är anmärkningsvärda för format'),
				(051, 'Serials på amerikansk engelska'),
				(090, 'Manuskript och sällsynta böcker'),
				(044, 'Unassigned'),
				(027, 'Allmänna bibliotek'),
				(082, 'Samlingar på engelska'),
				(097, 'Böcker kända för ägande eller ursprung'),
				(062, 'Organisationer på Brittiska öarna; i England'),
				(020, 'Bibliotek & informationsvetenskap'),
				(012, 'bibliografier av individer'),
				(032, 'encyklopedier på engelska'),
				(056, 'serier på spanska och portugisiska'),
				(095, 'Böcker kända för bindningar'),
				(025, 'bibliotekets verksamhet'),
				(085, 'på italienska, rumänska och relaterade språk'),
				(030, 'Allmänna uppslagsverk'),
				(065, 'Organisationer i Italien och angränsande öar'),
				(017, 'Allmänna kataloger'),
				(037, 'Encyclopedier på slaviska språk'),
				(074, 'Tidningar i Frankrike och Monaco'),
				(080, 'Allmänna samlingar'),
				(015, 'bibliografier av verk från specifika platser'),
				(035, 'Encyclopedier på italienska, rumänska och relaterade språk'),
				(092, 'Blockböcker'),
				(067, 'Organisationer i Östeuropa; i Ryssland'),
				(087, 'Samlingar på slaviska språk'),
				(022, 'Administration av fysisk växt'),
				(053, 'serier på andra germanska språk'),
				(060, 'Allmänna organisationer & museologi'),
				(069, 'Museumsvetenskap'),
				(002, 'Boken (dvs. Meta-skrifter om böcker)'),
				(088, 'Samlingar på skandinaviska språk'),
				(068, 'Organisationer i andra geografiska områden'),
				(089, 'Samlingar på andra språk'),
				(018, 'Kataloger arrangerade av författare, datum etc.'),
				(038, 'encyklopedier på skandinaviska språk'),
				(073, 'Tidningar i Centraleuropa; i Tyskland'),
				(019, 'Ordbokskataloger'),
				(039, 'encyklopedier på andra språk'),
				(054, 'serier på franska, oksitanska och katalanska');
				--SET IDENTITY_INSERT [Usertypes] OFF
				GO
-------LIBRARY OBJECT INSERTS----------

	--SAMPLE INSERTS
	SET IDENTITY_INSERT 	LibraryObjects on
		INSERT INTO 		LibraryObjects (ID, Title, [Description], ISBN, Publisher, PurchasePrice, Category, Author, imagesrc, Dewey) 
				VALUES 
				(1, 'Harry Potter och De vises sten'			,'Plötsligt händer det märkliga ting i den lilla staden! Mystiska stjärnskott på himlen och svärmar av ugglor mitt på dagen, katter som läser kartor och underliga människor som står i gathörnen och viskar. De viskar om en viss Harry Potter ... Föräldralöse Harry Potter bor hos sina elaka styvföräldrar och deras vidrige son. En helt ny värld öppnar sig för Harry när det visar sig att han egentligen är en trollkarl och börjar Hogwarths Skola för Häxkonster och Trolldom, en värld full av magi och spännande äventyr!'	,7599999888999,'Rabén Sjögren'	 ,350 ,1 ,1, 'https://s1.adlibris.com/images/54131168/harry-potter-och-de-vises-sten.jpg', 100),		--Does ID really have to be inserted? 
				(2, 'Harry Potter och Hemligheternas kammare '	,'Sommarlovet är äntligen över! Harry Potter har längtat tillbaka till sitt andra år på Hogwarts skola för häxkonster och trolldom. Men hur ska han stå ut med den omåttligt stroppige professor Lockman? Vad döljer Hagrids förflutna? Och vem är egentligen Missnöjda Myrtle? De verkliga problemen börjar när någon, eller något, förstenar den ena Hogwartseleven efter den andra. Är det Harrys fiende, Draco Malfoy, som ligger bakom? Eller är det den som alla på Hogwarts misstänker - Harry Potter själv?'	,799919449999,'Rabén Sjögren'	 ,350 ,1 ,1, 'https://s1.adlibris.com/images/54131167/harry-potter-och-hemligheternas-kammare.jpg', 100),		
				(3, 'Harry Potter och de vises sten'			,'Sexton år efter utgivningen av den första Harry Potter-boken i Sverige kommer nu en alldeles ny, genomillustrerad utgåva. Harry Potters fantastiska värld fångas mästerligt i Jim Kays makalösa bilder. Låt dig sugas in i en magisk upplevelse! Böckerna om Harry Potter har sålt i hundratals miljoner exemplar världen över. Den föräldralöse pojken som visar sig vara en trollkarl tog världen med storm, och lämnade ingen oberörd. Böckerna har blivit klassiker, och fortsätter att förtrolla nya generationer läsare.'	,7593399119999,'Rabén Sjögren'	 ,350 ,1 ,1, 'https://s1.adlibris.com/images/52391138/harry-potter-och-dodsrelikerna.jpg', 100),		
				(4, 'Sagan om Ringen'							,'"En ring att styra dem, en ring att se dem, en ring att fånga dem och till mörkret ge dem, i Mordor, i skuggornas land'	,9229229999999,'Penguin Classics'		,350 ,1 ,3, 'https://s2.adlibris.com/images/43135545/ringens-brodraskap.jpg', 100),
				(5, 'Sagan om de två tornen'					,'"En ring att styra dem, en ring att se dem, en ring att fånga dem och till mörkret ge dem, i Mordor, i skuggornas land'	,9329228888899,'Penguin Classics'		,350 ,1 ,3, 'https://s1.adlibris.com/images/42903698/de-tva-tornen.jpg', 100),
				(6, 'Konungens återkomst'						,'"En ring att styra dem, en ring att se dem, en ring att fånga dem och till mörkret ge dem, i Mordor, i skuggornas land'	,9529228333899,'Penguin Classics'		,350 ,1 ,3, 'https://s1.adlibris.com/images/43138299/konungens-aterkomst.jpg', 100),
				(7, 'Hobbiten - Ljudbok'						,'Den lille hobbiten Bilbo Secker dras av trollkarlen Gandalf grå med på äventyr tillsammans med tretton dvärgar, ledda av den sturske Thorin Ekensköld. De ska röva bort en stor guldskatt som vaktas av den eldsprutande draken Smaug. På vägen stöter de på ruskiga troll och vättar, men också hjälpsamma varelser som alver, jätteörnar och den store Beorn. Bilbo träffar också den slemmige Gollum som håller till i en mörk grotta uppe i bergen. Gollum utmanar Bilbo på en tävling och blir där av med sin magiska osynlighetsring. En ring som kommer att spela en viktig roll i berättelsen.'				,9174322223999,'Coola Förlaget'			,350 ,3 ,3, 'https://s1.adlibris.com/images/42934038/hobbiten-eller-bort-och-hem-igen.jpg', 100),
				(8, 'Game of Thrones Ljudbok'					,'Äventyr och hjältedåd, vänskap, kärlek och förräderi i en magisk medeltida värld. Robert Baratheon och Eddard Stark befriade en gång i tiden de sju konungarikena från den galne draklorden, och det blev Robert som besteg järntronen medan Eddard drog sig tillbaka till sitt Vinterhed. Efter en lång tid av fred samlar sig kung Roberts fiender både i Norden och i Södern. Hans närmaste rådgivare har dött under mystiska omständigheter och till och med hans egen drottning smider i hemlighet onda planer. Kung Robert ber Eddard Stark om hjälp och Eddard och hans familj dras obönhörligt in i maktspelet kring järntronen. Intrigerna leder till ett regelrätt krig mellan olika ätter och deras lorder. Men det största hotet mot kung Robert är draklordens två barn, en son och en dotter som nu har vuxit upp. De lever i exil och har fått en fristad hos hövdingen över Dothrakiens grässlätter. Med hjälp av honom och hans fyrtiotusen krigare ska åter en ättling av drakens blod härska över de sju konungarikena. Men kampen om järntronen har bara börjat … Den första boken i Sagan om is och eld.',9279229423999,'Amazon'	,75 ,2 ,2, 'https://s2.adlibris.com/images/625724/game-of-thrones---kampen-om-jarntronen.jpg', 100),
				(9, 'Den andra dödssynden Ljudbok'				,'Det är dags för en ny generation Lauritzen att ta över. Fastighetsbranschen lockar dem och enorma vinster väntar. Eller avgrunden. Samtidigt ställs Eric Letangs advokat­byrå inför helt andra påfrestningar. 1980-talet är decenniet som kommer att skörda rader av oskyldiga offer i rättssalarna. De vildsint egoistiska stämningarna är på väg att bli det socialdemokratiska folkhemmets svanesång. Allt kan hända. Och det händer. Den andra dödssynden är den nionde delen i Jan Guillous romansvit Det stora århundradet, en berättelse om mänsklighetens största, grymmaste och blodigaste århundrade. '			,993333343999,'Evas Förlag'			,350 ,3 ,4, 'https://s1.adlibris.com/images/55340502/den-andra-dodssynden.jpg', 100),
				(10, 'The Hulk'									,'Bruce Banner, a scientist on the run from the U.S. Government, must find a cure for the monster he turns into whenever he loses his temper.'			,9996229994999,'WarnerBrothers'			,350 ,4 ,1, 'https://m.media-amazon.com/images/M/MV5BMTUyNzk3MjA1OF5BMl5BanBnXkFtZTcwMTE1Njg2MQ@@._V1_SY1000_CR0,0,674,1000_AL_.jpg', 100);
		SET IDENTITY_INSERT LibraryObjects OFF
		GO

------------------------------------------------------------------------
