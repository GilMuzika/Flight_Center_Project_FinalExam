--creating stored prosedure "DAO_BASE_GetAll_METHOD_QUERY" begins here
Create proc dbo.DAO_BASE_GetAll_METHOD_QUERY
@TABLE_NAME nvarchar(50)
As
Begin
 Declare @sql nvarchar(max)
 Set @sql = 'select * from ' + QUOTENAME(@TABLE_NAME)
 Execute sp_executesql @sql
End
--creating stored prosedure "DAO_BASE_GetAll_METHOD_QUERY" ends here
