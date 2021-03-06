USE [C65]
GO
/****** Object:  StoredProcedure [dbo].[ExtActivityData_Insert]    Script Date: 3/10/2019 7:28:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[ExtActivityData_Insert]
	@UserId int
	,@Provider nchar(10)
	,@EndTime datetime2(7)
	,@StartTime datetime2(7)
	,@ActivityData dbo.ExtActivityData_v3 READONLY

/*

Insert Activity Data from a third party provider such as FitBit. This proc uses a user defined table type to perform an Insert Into/Select Statement

*/


AS

BEGIN


INSERT INTO [dbo].[ExtActivityData]
           ([UserId]
           ,[StartTime]
           ,[Steps]
           ,[Provider])
SELECT @UserId
       ,ad.StartTime
       ,ad.Steps
       ,@Provider
FROM @ActivityData ad


END