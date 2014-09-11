SELECT  SECTION_CODE AS ParentId ,
        LINE_CODE AS Id,
        LINE_NAME AS Name,
        4 AS Type 
FROM    dbo.TB_M_SECTION_LINE
WHERE   LINE_CODE = @Code