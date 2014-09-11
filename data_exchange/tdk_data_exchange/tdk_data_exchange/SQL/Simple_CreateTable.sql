IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[tb_r_data]') AND type in (N'U'))

BEGIN
CREATE TABLE [tb_r_data](
	[Bus] [varchar](50) NOT NULL,
	[Id] [varchar](50) NOT NULL,
	[Data] [varchar](max) NOT NULL,
 CONSTRAINT [PK_tb_r_data] PRIMARY KEY CLUSTERED 
(
	[Bus] ASC, [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END