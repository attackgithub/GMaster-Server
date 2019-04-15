CREATE PROCEDURE [dbo].[LogError_Create]
	@userId int = 0,
	@area varchar(32) = '',
	@url varchar(64) = '',
	@message varchar(512),
	@stacktrace varchar(512)
AS
	DECLARE @id int = NEXT VALUE FOR SequenceLogErrors
	INSERT INTO LogErrors (logId, datecreated, userId, area, [url], [message], stacktrace)
	VALUES (@id, GETDATE(), @userId, @area, @url, @message, @stacktrace)
