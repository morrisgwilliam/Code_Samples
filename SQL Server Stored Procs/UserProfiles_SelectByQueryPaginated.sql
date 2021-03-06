USE [C65]
GO
/****** Object:  StoredProcedure [dbo].[UserProfiles_SearchPaginated]    Script Date: 3/10/2019 7:23:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Proc [dbo].[UserProfiles_SearchPaginated]
	@PageIndex int
	,@PageSize int
	,@Query nvarchar(100) = null

	

AS
/*

Declare 
		@PageIndex int = 0
		,@PageSize int = 2
		,@Query nvarchar(100) = 'James'

Execute dbo.UserProfiles_SearchPaginated
	@PageIndex
	,@PageSize
	,@Query


*/
BEGIN

	SELECT [Id]
		  ,[PhotoUrl]
		  ,[AddressId]
		  ,[FirstName]
		  ,[LastName]
		  ,[UserId]
		  ,[DateCreated]
		  ,[DateModified]
	  FROM [dbo].[UserProfiles]
	  WHERE [FirstName] LIKE (CASE WHEN @Query IS NUll THEN '%' ELSE '%'+@Query+'%' END)
		OR [LastName] LIKE (CASE WHEN @Query IS NUll THEN '%' ELSE '%'+@Query+'%' END)
	  ORDER BY Id DESC
	  OFFSET @PageSize * @PageIndex ROWS
	  FETCH NEXT @PageSize ROWS ONLY

	  SELECT COUNT(Id)
	  FROM dbo.UserProfiles
	  WHERE [FirstName] LIKE (CASE WHEN @Query IS NUll THEN '%' ELSE '%'+@Query+'%' END)
		OR [LastName] LIKE (CASE WHEN @Query IS NUll THEN '%' ELSE '%'+@Query+'%' END)

END


