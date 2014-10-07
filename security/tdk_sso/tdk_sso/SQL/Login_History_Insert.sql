INSERT INTO [TB_R_LOGIN_HISTORY]
           ([ID]
           ,[USERNAME]
           ,[LOGIN_TIME]
           ,[SESSION_TIMEOUT]
           ,[LOCK_TIMEOUT]
           ,[MAX_LOGIN]
           ,[HOSTNAME]
           ,[HOST_IP]
           ,[BROWSER]
           ,[BROWSER_VERSION]
           ,[IS_MOBILE])
     VALUES
           (@Id
           ,@Username
           ,@LoginTime
           ,@SessionTimeout
           ,@LockTimeout
           ,@MaxLogin
           ,@Hostname
           ,@HostIP
           ,@Browser
           ,@BrowserVersion
           ,@IsMobile)


