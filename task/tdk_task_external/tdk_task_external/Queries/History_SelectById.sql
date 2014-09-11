SELECT [ID] as "Id"
	  ,[REGISTRY_ID] as "RegistryId"
      ,[NAME] as "Name"
      ,[DESCRIPTION] as "Description"
      ,[SUBMITTER] as "SubmitterUsername"
      ,[FUNCTION_NAME] as "FunctionName"
      ,[PARAMETER] as "Parameter"
      ,[TYPE] as Type
      ,[STATUS] as "Status"
	  ,[COMMAND] as "Command"
	  ,[PROGRESS] as "Progress"
	  ,[START_TIME] as "ExecutionStartTime"
	  ,[FINISH_TIME] as "ExecutionFinishTime"
  FROM [TB_R_BACKGROUND_TASK_HISTORY]
  WHERE [ID] = @Id