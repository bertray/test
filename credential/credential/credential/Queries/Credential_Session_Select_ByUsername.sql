SELECT [ID] as "Id"
      ,[USERNAME] as "Username"
      ,[LOGIN_TIME] as "LoginTime"
      ,[LOGOUT_TIME] as "LogoutTime"
      ,[TIMEOUT] as "Timeout"
      ,[LOCATION] as "Location"
      ,[CLIENT_AGENT] as "ClientAgent"
      ,[LOCK_TIMEOUT] as "LockTimeout"
      ,[LOCKED] as "Locked"
      ,[LOCK_TIME] as "LockTime"
      ,[UNLOCK_TIME] as "UnlockTime"
FROM [TB_R_USER_SESSION]
WHERE USERNAME = @Username
ORDER BY ID