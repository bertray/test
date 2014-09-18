SELECT [FUNCTION_KEY]
      ,[FUNCTION_ID] as "Id"
      ,[SYSTEM_ID]
      ,[MODULE_ID]
      ,[FUNCTION_NAME] as "Name"
      ,[FUNCTION_DESCRIPTION] as "Description"
      ,[CREATED_BY]
      ,[CREATED_DATE]
      ,[CHANGED_BY]
      ,[CHANGED_DATE]
  FROM [TB_M_FUNCTION]
  WHERE ([FUNCTION_ID] = @FunctionId) and ([SYSTEM_ID] = @SystemId)

