SELECT [ROLE_ID] as "RoleId"
      ,[FUNCTION_ID] as "FunctionId"
      ,[FEATURE_ID] as "FeatureId"
  FROM [TB_M_ROLE_FEATURE]
  WHERE ([ROLE_ID] = @RoleId)
		and ([FUNCTION_ID] = @FunctionId)
		and ([FEATURE_ID] = @FeatureId)