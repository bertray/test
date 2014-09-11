SELECT  LINE_CODE AS ParentId ,
        GROUP_CODE AS Id ,
        GROUP_NAME AS Name ,
        5 AS Type
FROM    dbo.TB_M_LINE_GROUP
WHERE   GROUP_CODE = @Code