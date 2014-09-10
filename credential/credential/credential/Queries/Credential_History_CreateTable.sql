﻿IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_USER_SESSION_HISTORY]') AND type in (N'U'))
BEGIN
CREATE TABLE [TB_R_USER_SESSION_HISTORY](
	[ID] [varchar](50) NOT NULL,
	[USERNAME] [varchar](100) NOT NULL,
	[LOGIN_TIME] [datetime] NOT NULL,
	[LOGOUT_TIME] [datetime] NULL,
	[TIMEOUT] [int] NULL,
	[LOCATION] [varchar](50) NOT NULL,
	[CLIENT_AGENT] [varchar](200) NOT NULL,
	[LOCK_TIMEOUT] [int] NOT NULL,
	[LOCKED] [bit] NOT NULL,
	[LOCK_TIME] [datetime] NULL,
	[UNLOCK_TIME] [datetime] NULL,
 CONSTRAINT [PK_TB_R_USER_SESSION_HISTORY] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END;

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[TB_R_USER_SESSION_HISTORY]') AND name = N'IX_TB_R_USER_SESSION_HISTORY')
CREATE NONCLUSTERED INDEX [IX_TB_R_USER_SESSION_HISTORY] ON [TB_R_USER_SESSION_HISTORY] 
(
	[ID] ASC,
	[USERNAME] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
