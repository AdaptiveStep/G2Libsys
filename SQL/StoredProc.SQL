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

Create proc usp_insert_users
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
-------------------------------------------------------------