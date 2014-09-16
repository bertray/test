UPDATE [TB_M_ROLE]
   SET [SYSTEM_ID] = @SystemId
      ,[ROLE_NAME] = @Name
      ,[ROLE_DESCRIPTION] = @Description
      ,[AREA_ID] = @AreaId
      ,[SESSION_TIME_OUT] = @SessionTimeout
      ,[CHANGED_BY] = @ChangedBy
      ,[CHANGED_DATE] = @ChangedDate
 WHERE [ROLE_ID] = @Id