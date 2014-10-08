SELECT [ID] as Id
      ,[USERNAME] as Username
      ,[LOGIN_TIME] as LoginTime
      ,[LOCKED] as Locked
      ,[LOCK_TIME] as LockTime
      ,[UNLOCK_TIME] as UnlockTime
      ,[SESSION_TIMEOUT] as SessionTimeout
      ,[LOCK_TIMEOUT] as LockTimeout
      ,[MAX_LOGIN] as MaximumLogin
      ,[LAST_ACTIVE] as LastActive
      ,[HOSTNAME] as Hostname
      ,[HOST_IP] as HostIP
      ,[BROWSER] as Browser
      ,[BROWSER_VERSION] as BrowserVersion
      ,[IS_MOBILE] as IsMobile
  FROM [TB_R_LOGIN]
  WHERE (USERNAME = @Username) and
		(HOSTNAME = @Hostname) and
		(HOST_IP = @HostIP) and
		(BROWSER = @Browser) and
		(BROWSER_VERSION = @BrowserVersion) and
		(IS_MOBILE = @IsMobile)