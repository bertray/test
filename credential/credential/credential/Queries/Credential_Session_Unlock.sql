﻿UPDATE [TB_R_USER_SESSION]
SET LOCK_TIME = null, LOCKED = 0, UNLOCK_TIME = @UnlockTime
WHERE ID = @Id