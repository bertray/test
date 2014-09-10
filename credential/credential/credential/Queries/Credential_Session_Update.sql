UPDATE [TB_R_USER_SESSION]
   SET [LOGIN_TIME] = @LoginTime
      ,[LOGOUT_TIME] = @LogoutTime
      ,[TIMEOUT] = @Timeout
      ,[LOCATION] = @Location
      ,[CLIENT_AGENT] = @ClientAgent
      ,[LOCK_TIMEOUT] = @LockTimeout
      ,[LOCKED] = @Locked
      ,[LOCK_TIME] = @LockTime
      ,[UNLOCK_TIME] = @UnlockTime
 WHERE [ID] = @Id


