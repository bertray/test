SELECT [CLASS_ID] as "Code"
      ,[CLASS_NAME] as "Name"
	  ,[COMPANY_ID]
  FROM [TB_M_CLASS]
  WHERE ([CLASS_ID] = @ClassId)