UPDATE @TableName 
SET [MESSAGE] = LTRIM([MESSAGE]) + @Message
WHERE ([SESSION] = @SessionName) AND ([ID] = @Id)

