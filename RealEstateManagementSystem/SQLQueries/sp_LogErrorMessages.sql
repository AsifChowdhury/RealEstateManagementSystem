--Created
USE [CentralDB]
GO
/****** Object:  StoredProcedure [dbo].[sp_LogErrorMessages]    Script Date: 14/Jun/2016 12:02:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_LogErrorMessages]
	@appName varchar(50), @message varchar(max), @exceptionType varchar(250), @stackTrace varchar(max), @source varchar(max), @targetSites varchar(max),
	@userId varchar(50), @workstationIP varchar(50)
AS
	INSERT INTO ErrorLog
		(AppName, [Message], ExceptionType, StackTrace, [Source], TargetSite, UserId, WorkstationIP)
	VALUES
		(@appName, @message, @exceptionType, @stackTrace, @source, @targetSites, @userId, @workstationIP)