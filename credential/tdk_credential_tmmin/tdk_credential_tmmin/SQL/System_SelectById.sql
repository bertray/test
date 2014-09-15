SELECT [SYSTEM_ID] as "Id"
      ,[SYSTEM_NAME] as "Name"
      ,[SYSTEM_DESCRIPTION] as "Description"
      ,[SYSTEM_URL] as "Url"
  FROM [TB_M_SYSTEM]
  WHERE [SYSTEM_ID] = @SystemId