--creating stored prosedure "DAO_BASE_Add_METHOD_QUERY_for_update" begins here
Create proc dbo.DAO_BASE_Add_METHOD_QUERY_for_update
@TABLE_NAME nvarchar(50),
@SUBSEQUENT_COLUMN_NAME nvarchar(50),
@SUBSEQUENT_COLUMN_VALUE nvarchar(50),
@FIRST_COLUMN_NAME nvarchar(50),
@FIRST_COLUMN_VALUE nvarchar(50)

As
Begin
 Declare @sql nvarchar(max)
 Set @sql = 'UPDATE ' + @TABLE_NAME + ' SET ' + @SUBSEQUENT_COLUMN_NAME + ' = "' + @SUBSEQUENT_COLUMN_VALUE + '" WHERE ' + @FIRST_COLUMN_NAME + ' = "' + @FIRST_COLUMN_VALUE
 Execute sp_executesql @sql
End
--creating stored prosedure "DAO_BASE_Add_METHOD_QUERY_for_update" ends here