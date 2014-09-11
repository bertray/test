SELECT [ID] as "Id"
	  ,[SESSION] as "Session"
      ,[SEVERITY] as "Severity"
      ,[DATE] as "Date"
      ,[MESSAGE] as "Message"
FROM [@TableName]
WHERE [SESSION] = @SessionName
ORDER BY [DATE] desc

