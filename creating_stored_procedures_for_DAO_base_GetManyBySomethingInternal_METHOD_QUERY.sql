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