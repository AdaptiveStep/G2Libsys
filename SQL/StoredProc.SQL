----------- Users ----------
    GO

Create proc usp_verifylogin_users
	@Email varchar(100),
	@Password varchar(20)
	as
	begin
		SELECT * FROM Users WHERE
		CAST(Email as varbinary(100)) = CAST(@Email as varbinary(100))
		AND CAST([Password] as varbinary(100)) = CAST(@Password as varbinary(100))
		AND Email = @Email 
		AND [Password] = @Password
	end
    GO

Create proc usp_verifyemail_users
    @Email varchar(100)
    as
    begin
        select count(1) from users where email = @email;
    end
    GO

Alter proc usp_insert_users
    @ID int = null,
    @LoanID numeric = null,
    @Email varchar(100),
    @Password varchar(20),
    @Firstname varchar(20),
    @Lastname varchar(20),
    @UserType int,
    @LoggedIn bit,
    @NewID int Output
    as
    begin
        insert into users (Email, [Password], Firstname, Lastname, UserType, LoggedIn)
        values (@Email, @Password, @Firstname, @Lastname, @UserType, @LoggedIn);
        select @NewID = SCOPE_IDENTITY();
    end
    GO

Create proc usp_update_users
    @ID int,
    @LoanID numeric = null,
    @Email varchar(100),
    @Password varchar(20),
    @Firstname varchar(20),
    @Lastname varchar(20),
    @UserType int,
    @LoggedIn bit
    as
    begin
		Update users
		Set Email = @Email, 
			[Password] = @Password, 
			Firstname = @Firstname, 
			Lastname = @Lastname, 
			UserType = @UserType, 
			LoggedIn = @LoggedIn
		Where ID = @ID;
    end
    GO

Create proc usp_remove_users
    @ID int
    as
    begin
		delete from users where ID = @ID
    end
    GO

Create proc usp_getall_users
    as
    begin
        select * from users;
    end
    GO

create proc usp_getall_category
    as
    begin
        select * from Categories;
    end
    GO

create proc usp_getall_libraryobjects
    as
    begin
        select * from LibraryObjects;
    end
    GO

Create proc usp_remove_libraryobjects
    @ID int
    as
    begin
		delete from libraryobjects where ID = @ID
    end
    GO
-------------------------------------------------------------

ALTER PROC [dbo].[usp_insert_libraryobjects]
		@ID 			INT				= null,
		@Title			VARCHAR(100)	,
		@Description 	VARCHAR(500)	= null,
		@ISBN			BIGINT			= null,
		@Publisher		VARCHAR(100)	= null,
		@PurchasePrice	FLOAT			= null,
		@Pages			INT				= null,
		@Dewey			INT				= null,
		@Category		INT	      		,	
		@Author			INT				= null,
		
		@Library		INT				= null,
		@AddedBy		INT				,
		@LastEdited		DATETIME		= null,
		@DateAdded		DATETIME		= null,
		@NewID			INT	OUTPUT
		AS
		BEGIN
			INSERT INTO 
			LibraryObjects(Title, [Description], ISBN, Publisher, PurchasePrice, Pages, Dewey, Category, Author, [Library], AddedBy, LastEdited, DateAdded) 
			VALUES 
			(@Title, @Description, @ISBN, @Publisher, @PurchasePrice, @Pages, @Dewey, @Category, @Author, @Library, @AddedBy, @LastEdited, @DateAdded);
			SELECT @NewID = SCOPE_IDENTITY();
		END