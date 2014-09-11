﻿IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[@TableName]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[@TableName](
	[ID] [bigint] identity(1,1) NOT NULL,
	[SESSION] [varchar](50) NOT NULL,
	[SEVERITY] [tinyint] NOT NULL,
	[DATE] [datetime] NOT NULL,
	[MESSAGE] [varchar] (max) NULL
	CONSTRAINT [PK_@TableName] PRIMARY KEY CLUSTERED (
		[ID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[@TableName]') AND name = N'IX_@TableName')
CREATE NONCLUSTERED INDEX [IX_@TableName] ON [dbo].[@TableName] 
(
	[SESSION] ASC,
	[SEVERITY] ASC,
	[DATE] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
