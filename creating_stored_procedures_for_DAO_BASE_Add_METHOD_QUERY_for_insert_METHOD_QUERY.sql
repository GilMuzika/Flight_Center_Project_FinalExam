--creating stored prosedure "DAO_BASE_Add_METHOD_QUERY_for_insert" begins here
Create proc dbo.DAO_BASE_Add_METHOD_QUERY_for_insert
@TABLE_NAME nvarchar(50),
@SECOND_COLUMN_NAME nvarchar(50),
@SECOND_COLUMN_VALUE nvarchar(50)	
As
Begin
 Declare @sql nvarchar(max)
 --INSERT INTO {tableName} ({typeof(T).GetProperties()[1].Name}) VALUES ('{typeof(T).GetProperties()[1].GetValue(poco)}') SELECT SCOPE_IDENTITY()
 Set @sql = 'INSERT INTO ' + @TABLE_NAME + ' (' + @SECOND_COLUMN_NAME + ') VALUES ("' + @SECOND_COLUMN_VALUE + '") SELECT SCOPE_IDENTITY()' 
 Execute sp_executesql @sql
End
--creating stored prosedure "DAO_BASE_Add_METHOD_QUERY_for_insert" ends here