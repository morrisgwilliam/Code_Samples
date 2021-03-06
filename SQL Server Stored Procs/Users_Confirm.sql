USE [C65]
GO
/****** Object:  StoredProcedure [dbo].[Users_Confirm_V3]    Script Date: 3/10/2019 7:32:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Proc [dbo].[Users_Confirm_V3]
	@Token nvarchar(100)

AS
/*

Confirm a user account after receiving an email with a Token(GUID). After the user clicks on the email link their status will be updated to IsConfirmed = 1.

Declare 
	@Token nvarchar(100) = 'A71C5941-A94A-451D-94E5-487A039823F8',
	@UserId int

	SET @UserId = 
			(SELECT [UserId]
			FROM dbo.Tokens
			WHERE Token = @Token)

Execute dbo.Users_Confirm_V3
	@Token

*/
BEGIN

IF EXISTS (SELECT [UserId] FROM dbo.Tokens WHERE Token = @Token)

	BEGIN
		Declare @UserId int;

		SET @UserId = 
			(SELECT [UserId]
			FROM dbo.Tokens
			WHERE Token = @Token)
		
		DELETE 
		FROM dbo.Tokens 
		WHERE @UserId = UserId

		UPDATE [dbo].[Users]
		SET [IsConfirmed] = 1
		WHERE [Id] = @UserId

		SELECT [EmailAddress]
		FROM  [dbo].[Users]
		Where [Id] = @UserId

	END 
	
END


