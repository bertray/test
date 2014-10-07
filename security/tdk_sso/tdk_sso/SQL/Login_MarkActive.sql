UPDATE [TB_R_LOGIN]
   SET [LAST_ACTIVE] = getdate()
 WHERE [ID] = @Id


