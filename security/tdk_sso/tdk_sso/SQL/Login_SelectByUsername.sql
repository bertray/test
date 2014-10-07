SELECT [USERNAME] as "Username"
	  ,[ID] as "Id"
      ,[LOGIN_TIME] as "LoginTime"
      ,[LOCKED] as "Locked"
      ,[LOCK_TIME] as "LockTime"
	  ,[UNLOCK_TIME] as "UnlockTime"
	  ,[SESSION_TIMEOUT] as "SessionTimeout"
	  ,[LOCK_TIMEOUT] as "LockTimeout"
	  ,[MAX_LOGIN] as "MaximumLogin"
	  ,[LAST_ACTIVE] as "LastActive"
  FROM [TB_R_LOGIN]
  WHERE USERNAME = @Username