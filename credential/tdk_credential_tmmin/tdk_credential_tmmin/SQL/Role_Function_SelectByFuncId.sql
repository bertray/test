SELECT  b.FUNCTION_ID AS "Id" ,
        b.FUNCTION_NAME AS "Name" ,
        b.FUNCTION_DESCRIPTION AS "Description"
FROM    TB_M_ROLE_FUNCTION a
        INNER JOIN TB_M_FUNCTION b ON a.FUNCTION_ID = b.FUNCTION_ID
		WHERE (a.ROLE_ID = @RoleId) and (a.function_id = @FunctionId)