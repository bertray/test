INSERT INTO @TableName
           ([SESSION]
		   ,[SEVERITY]
		   ,[DATE]
           ,[MESSAGE])
VALUES (@Session, @Severity, @Date, @Message)

