INSERT INTO [TB_R_USER_SESSION]
           ([ID]
           ,[USERNAME]
           ,[LOGIN_TIME]
           ,[TIMEOUT]
           ,[LOCATION]
           ,[CLIENT_AGENT]
           ,[LOCK_TIMEOUT]
           ,[LOCKED])
     VALUES
           (@Id
           ,@Username
           ,@LoginTime
           ,@Timeout
           ,@Location
           ,@ClientAgent
           ,@LockTimeout
           ,@Locked)