BEGIN TRAN

IF EXISTS (
        SELECT type_desc, type
        FROM sys.procedures WITH(NOLOCK)
        WHERE NAME = 'DAO_BASE_GetAllTableNames_METHOD_QUERY'
            AND type = 'P'
      )
     DROP PROCEDURE dbo.DAO_BASE_GetAllTableNames_METHOD_QUERY
GO
--creating stored prosedure "DAO_BASE_GetAllTableNames_METHOD_QUERY" begins here
Create proc dbo.DAO_BASE_GetAllTableNames_METHOD_QUERY	
@TABLE_NAME nvarchar(50),
@RELEVANT_COLUMN_NAME nvarchar(50),
@COLUMN_IDENTIFIER nvarchar(50)	
As
Begin
 Declare @sql nvarchar(max)
 Set @sql = 'SELECT * FROM' + QUOTENAME(@TABLE_NAME) + 'WHERE' + QUOTENAME(@RELEVANT_COLUMN_NAME) + '=' + QUOTENAME(@COLUMN_IDENTIFIER)
 Execute sp_executesql @sql
End
--creating stored prosedure "DAO_BASE_GetAllTableNames_METHOD_QUERY" ends here

GO
-- BEGIN DO NOT REMOVE THIS CODE (it commits or rolls back the stored procedure drop) 
IF EXISTS (
       SELECT 1
       FROM sys.procedures WITH(NOLOCK)
       WHERE NAME = 'DatasetDeleteCleanup'
           AND type = 'P'
     )
COMMIT TRAN
ELSE
ROLLBACK TRAN
-- END DO NOT REMOVE THIS CODE

--//////////////////////////////////////////////////////////////////////////////////

BEGIN TRAN

IF EXISTS (
        SELECT type_desc, type
        FROM sys.procedures WITH(NOLOCK)
        WHERE NAME = 'DAO_BASE_GetManyBySomethingInternal_METHOD_QUERY'
            AND type = 'P'
      )
     DROP PROCEDURE dbo.DAO_BASE_GetManyBySomethingInternal_METHOD_QUERY
GO
--creating stored prosedure "DAO_BASE_GetManyBySomethingInternal_METHOD_QUERY" begins here
Create proc dbo.DAO_BASE_GetManyBySomethingInternal_METHOD_QUERY
@TABLE_NAME nvarchar(50),
@RELEVANT_COLUMN_NAME nvarchar(50),
@COLUMN_IDENTIFIER nvarchar(50)	
As
Begin
 Declare @sql nvarchar(max)
 Set @sql = 'SELECT * FROM' + QUOTENAME(@TABLE_NAME) + 'WHERE' + QUOTENAME(@RELEVANT_COLUMN_NAME) + '=' + QUOTENAME(@COLUMN_IDENTIFIER)
 Execute sp_executesql @sql
End
--creating stored prosedure "DAO_BASE_GetManyBySomethingInternal_METHOD_QUERY" ends here

GO
-- BEGIN DO NOT REMOVE THIS CODE (it commits or rolls back the stored procedure drop) 
IF EXISTS (
       SELECT 1
       FROM sys.procedures WITH(NOLOCK)
       WHERE NAME = 'DatasetDeleteCleanup'
           AND type = 'P'
     )
COMMIT TRAN
ELSE
ROLLBACK TRAN
-- END DO NOT REMOVE THIS CODE
