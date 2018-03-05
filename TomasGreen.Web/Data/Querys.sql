Select * from Warehouses

SELECT [a].[ID], [a].[Name], CONVERT(VARCHAR(11), [aw].[QtyPackagesOnhand]), CONVERT(VARCHAR(100), [aw].[QtyExtraOnhand])
FROM [ArticleWarehouseBalances] AS [aw]
INNER JOIN [Articles] AS [a] ON [aw].[ArticleID] = [a].[ID]
INNER JOIN [Warehouses] AS [w] ON [aw].[WarehouseID] = [w].[ID]
WHERE [aw].[WarehouseID] = 3
ORDER BY [a].[Name]