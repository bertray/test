﻿UPDATE TB_R_LOGIN 
SET LOCKED = 0, LOCK_TIME = null, UNLOCK_TIME = @UnlockTime
WHERE ID = @Id