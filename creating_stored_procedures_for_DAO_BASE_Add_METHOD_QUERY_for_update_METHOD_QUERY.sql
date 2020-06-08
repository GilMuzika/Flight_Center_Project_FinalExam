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
CREATE PROCEDURE DAO_BASE_Add_METHOD_QUERY_for_update
	-- Add the parameters for the stored procedure here
@TABLE_NAME nvarchar(50),
@SUBSEQUENT_COLUMN_NAME nvarchar(50),
@SUBSEQUENT_COLUMN_VALUE nvarchar(max),
@FIRST_COLUMN_NAME nvarchar(50),
@FIRST_COLUMN_VALUE nvarchar(50)
AS
BEGIN
declare @sql nvarchar(max)
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--Set @sql = 'UPDATE "' + @TABLE_NAME + '" SET ' + @SUBSEQUENT_COLUMN_NAME + ' = "' + @SUBSEQUENT_COLUMN_VALUE + '" WHERE ' + @FIRST_COLUMN_NAME + ' = "' + @FIRST_COLUMN_VALUE
	SEt @sql = 'UPDATE '+ @TABLE_NAME +' SET '+@SUBSEQUENT_COLUMN_NAME+' = '+ QUOTENAME(@SUBSEQUENT_COLUMN_VALUE) +' WHERE '+@FIRST_COLUMN_NAME+' = '+@FIRST_COLUMN_VALUE
	Execute sp_executesql @sql
END
GO
