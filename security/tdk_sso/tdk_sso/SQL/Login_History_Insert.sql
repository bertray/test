INSERT INTO [TB_R_LOGIN_HISTORY]
           ([USERNAME]
           ,[LOGIN_TIME]
           ,[LOGOUT_TIME]
		   ,[SESSION_TIMEOUT]
		   ,[LOCK_TIMEOUT])
     VALUES
           (@Username
           ,@LoginTime
           ,@LogoutTime
		   ,@SessionTimeout
		   ,@LockTimeout)


