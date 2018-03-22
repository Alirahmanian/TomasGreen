use TomasGreen
Select OBJECT_NAME(parent_object_Id) from sys.foreign_keys where object_name(referenced_object_id) = 'ArticleCategories'
select * from Articles where ArticleCategoryID = 3
