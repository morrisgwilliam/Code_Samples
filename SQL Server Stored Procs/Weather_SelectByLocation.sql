USE [C65]
GO
/****** Object:  StoredProcedure [dbo].[Weather_Select]    Script Date: 3/10/2019 7:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Proc [dbo].[Weather_Select]
	@Date bigint
	,@Lat float
	,@Long float

	/*
	The function fo this proc is too select weather data in the DB that is within 20 miles of the location of the user and relevant to the day the data is being accessed for.

	Declare @date bigint = 1549501746415, @Lat float = 33.787914, @Long float = -117.853104
	Execute dbo.Weather_Select @date, @Lat, @Long

	*/



	as


	BEGIN

	Select		[Id]
			  ,[Summary]
			  ,[Icon]
			  ,[Temperature]
			  ,[PrecipProbability]
			  ,[Time]
      ,Miles = GEOGRAPHY::Point([Lat], [Long], 4326).STDistance(GEOGRAPHY::Point(@Lat, @Long, 4326))/1609.344
	  From dbo.Weather
	  WHERE Time >= @Date/1000 - 3599 AND 20 > GEOGRAPHY::Point([Lat], [Long], 4326).STDistance(GEOGRAPHY::Point(@Lat, @Long, 4326))/1609.344


	END