SELECT [COMPANY_ID] as "Code"
      ,[COMPANY_TYPE] as "Type"
      ,[COMPANY_ABBREVIATION] as "Abbreviation"
      ,[COMPANY_NAME] as "Name"
  FROM [TB_M_COMPANY]
  WHERE [COMPANY_ID] = @CompanyId