﻿SELECT  DIVISION_CODE AS ParentId,
		DEPARTMENT_CODE as Id,
		DEPARTMENT_NAME AS Name,
		2 AS Type
FROM    TB_M_DIVISION_DEPARTMENT
WHERE   DEPARTMENT_CODE = @Code