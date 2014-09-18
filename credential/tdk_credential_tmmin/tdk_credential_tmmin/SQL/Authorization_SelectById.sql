SELECT tAuth.[USERNAME] as "Username"
      ,(
		select isnull(tSystem.SYSTEM_ID, '') + ':' + isnull(tSystem.SYSTEM_NAME, '') + ':' + isnull(tSystem.SYSTEM_URL, '') + ':' + isnull(tSystem.SYSTEM_DESCRIPTION, '')
		from TB_M_SYSTEM tSystem
		where tSystem.SYSTEM_ID = tAuth.SYSTEM_ID
      ) as "_SystemId"
      ,tAuth.[ROLE_ID] as "RoleId"
	  ,tAuth.DIVISION_CODE as "DivisionCode"
  FROM [TB_M_USER_ASSIGNMENT] tAuth
  WHERE (tAuth.USERNAME = @Username)
		and (tAuth.ROLE_ID = @RoleId)
		and (tAuth.SYSTEM_ID = @SystemId)