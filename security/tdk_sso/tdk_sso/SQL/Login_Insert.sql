INSERT INTO [TB_R_LOGIN]
           ([USERNAME]
		   ,[ID]
           ,[LOGIN_TIME]
		   ,[SESSION_TIMEOUT]
		   ,[LOCK_TIMEOUT]
		   ,[MAX_LOGIN])
     VALUES
           (@Username
		   ,@Id
           ,@LoginTime
		   ,@SessionTimeout
		   ,@LockTimeout
		   ,@MaxLogin)


