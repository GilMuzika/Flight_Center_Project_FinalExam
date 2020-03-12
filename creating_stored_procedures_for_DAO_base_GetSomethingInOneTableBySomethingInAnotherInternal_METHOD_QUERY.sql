--creating stored prosedure "DAO_BASE_GetSomethingInOneTableBySomethingInAnotherInternal_METHOD_QUERY" begins here
Create proc dbo.DAO_BASE_GetSomethingInOneTableBySomethingInAnotherInternal_METHOD_QUERY
@TABLE_NAME nvarchar(50),
@ANOTHER_TABLE_NAME nvarchar(50),
@LEFT_SIDE_OF_ONSTATEMENT nvarchar(50),
@RIGHT_SIDE_OF_ONSTATEMENT nvarchar(50),
@RELEVANT_COLUMN_NAME_IN_ANOTHER_TABLE nvarchar(50),
@COLUMN_IDENTIFIER nvarchar(50)
As
Begin
 Declare @sql nvarchar(max)
 Set @sql = 'SELECT' + QUOTENAME(@TABLE_NAME) + '.* FROM' + QUOTENAME(@TABLE_NAME) + 'JOIN' + QUOTENAME(@ANOTHER_TABLE_NAME) + 'ON' + QUOTENAME(@TABLE_NAME) + '.' + QUOTENAME(@LEFT_SIDE_OF_ONSTATEMENT) + ' = ' + QUOTENAME(@ANOTHER_TABLE_NAME) + '.' + QUOTENAME(@RIGHT_SIDE_OF_ONSTATEMENT) + ' WHERE ' + QUOTENAME(@ANOTHER_TABLE_NAME) + '.' + QUOTENAME(@RELEVANT_COLUMN_NAME_IN_ANOTHER_TABLE) + ' = ' + QUOTENAME(@COLUMN_IDENTIFIER) 
 Execute sp_executesql @sql
End
--creating stored prosedure "DAO_BASE_GetSomethingInOneTableBySomethingInAnotherInternal_METHOD_QUERY" ends here