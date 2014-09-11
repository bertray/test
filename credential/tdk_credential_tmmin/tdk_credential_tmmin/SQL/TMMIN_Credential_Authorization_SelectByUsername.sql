SELECT tAuth.[USERNAME] as "Username"
      ,(
		select tSystem.SYSTEM_ID + ':' + tSystem.SYSTEM_NAME + ':' + tSystem.SYSTEM_URL + ':' + tSystem.SYSTEM_DESCRIPTION
		from TB_M_SYSTEM tSystem
		where tSystem.SYSTEM_ID = tAuth.SYSTEM_ID
      ) as "_SystemId"
      ,tAuth.[ROLE_ID] as "RoleId"
	  ,tAuth.DIVISION_CODE as "DivisionCode"
  FROM [TB_M_USER_ASSIGNMENT] tAuth
  WHERE [USERNAME] = @Username