﻿SELECT  PLANT_CODE AS Id,
        PLANT_NAME  AS Name
FROM    dbo.TB_M_PLANT
WHERE PLANT_CODE = @PlantCode
	AND COMPANY_CODE = @CompanyCode