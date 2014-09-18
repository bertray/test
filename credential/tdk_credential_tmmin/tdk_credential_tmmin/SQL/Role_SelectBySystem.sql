SELECT (
		select isnull(tSystem.SYSTEM_ID,'') + ':' + isnull(tSystem.SYSTEM_NAME,'') + ':' + isnull(tSystem.SYSTEM_URL,'') + ':' + isnull(tSystem.SYSTEM_DESCRIPTION,'')
		from TB_M_SYSTEM tSystem
		where tSystem.SYSTEM_ID = tRole.SYSTEM_ID
      ) as "_SystemId"
      ,tRole.[ROLE_ID] as "Id"
      ,tRole.[ROLE_NAME] as "Name"
      ,tRole.[ROLE_DESCRIPTION] as "Description"
      ,(
		select tArea.AREA_ID + ':' + tArea.AREA_NAME
		from TB_M_AREA tArea
		where tArea.AREA_ID = tRole.AREA_ID
      ) as "_AreaId"
      ,tRole.[SESSION_TIME_OUT] as "SessionTimeout"
  FROM [TB_M_ROLE] tRole
  WHERE (tRole.SYSTEM_ID = @SystemId)