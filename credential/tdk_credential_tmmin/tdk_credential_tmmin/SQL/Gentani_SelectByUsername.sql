select 
	GENTANI_KEY as "Key",
	USERNAME as "Username",
	PLANT_CODE as "PlantCode",
	SHOP_CODE as "ShopCode",
	LINE_GENTANI as "LineCode",
	SECTION_GENTANI as "SectionCode",
	(
		SELECT SECTION_NAME 
		FROM TB_M_SECTION_GENTANI
		WHERE SECTION_ID = SECTION_GENTANI
	) as "SectionDescription",
	(
		SELECT LINE_NAME 
		FROM TB_M_LINE_GENTANI
		WHERE LINE_ID = LINE_GENTANI
	) as "LineDescription",
	SHIFT as "Shift"
from TB_M_USER_GENTANI
where USERNAME = @Username