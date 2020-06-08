-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE DAO_BASE_Add_METHOD_QUERY_for_insert
	-- Add the parameters for the stored procedure here
@TABLE_NAME nvarchar(50),
@SECOND_COLUMN_NAME nvarchar(50),
@SECOND_COLUMN_VALUE nvarchar(max)	
AS
BEGIN
declare @sql nvarchar(max)
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--INSERT INTO {tableName} ({typeof(T).GetProperties()[1].Name}) VALUES ('{typeof(T).GetProperties()[1].GetValue(poco)}') SELECT SCOPE_IDENTITY()
	Set @sql = 'INSERT INTO '+@TABLE_NAME+' ('+@SECOND_COLUMN_NAME+') VALUES ('+@SECOND_COLUMN_VALUE+') SELECT SCOPE_IDENTITY()'
	Execute sp_executesql @sql
END
GO
