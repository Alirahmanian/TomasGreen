use TomasGreen;
--Select OBJECT_NAME(parent_object_Id) from sys.foreign_keys where object_name(referenced_object_id) = 'ArticleCategories'

--select * from Articles where ArticleCategoryID = 3

--SELECT 
--    ccu.table_name AS SourceTable
--    ,ccu.constraint_name AS SourceConstraint
--    ,ccu.column_name AS SourceColumn
--    ,kcu.table_name AS TargetTable
--    ,kcu.column_name AS TargetColumn
--FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
--    INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc
--        ON ccu.CONSTRAINT_NAME = rc.CONSTRAINT_NAME 
--    INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu 
--        ON kcu.CONSTRAINT_NAME = rc.UNIQUE_CONSTRAINT_NAME  
--ORDER BY ccu.table_name
update Currencies set Archive = 1
