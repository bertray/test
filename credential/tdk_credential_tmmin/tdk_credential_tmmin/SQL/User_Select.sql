﻿SELECT tUser.[USERNAME] as "Username"
	  ,tUser.[COMPANY_CODE] as "_CompanyId"
      ,tUser.[PASSWORD] as "Password"
      ,tUser.[PASSWORD_EXPIRATION_DATE] as "PasswordExpirationDate"
      ,tUser.[ACCOUNT_VALIDITY_DATE] as "AccountValidityDate"
      ,tUser.[FIRST_NAME] as "FirstName"
      ,tUser.[LAST_NAME] as "LastName"
      ,tUser.[FULL_NAME] as "FullName"
      ,tUser.[PHONE_NO] as "_PhoneNumber"
      ,tUser.[EXT_NO] as "_ExtensionPhoneNumber"
      ,tUser.[MOBILE_NO] as "_MobilePhoneNumber"
      ,tUser.[EMAIL] as "_Email"
      ,tUser.[CLASS_ID] as "_ClassId"
      ,tUser.[JOB_FUNCTION] as "JobFunction"
      ,tUser.[CC_CODE] as "_CostCenterCode"
      ,tUser.[LOCATION_ID] as "_LocationId"
      ,tUser.[MAX_LOGON] as MaximumConcurrentLogin
      ,tUser.[DEFAULT_SYSTEM] as "_DefaultSystemId"
      ,tUser.[PRODUCTION_FLAG] as "InProduction"
      ,tUser.[ACTIVE_DIRECTORY_FLAG] as "InActiveDirectory"
	  ,tUser.[REG_NO] as "Id"
	  ,tUser.[REG_NO] as "RegistrationNumber"
	  ,tUser.[SHIFT] as "Shift"
	  ,tUser.[ACTIVE_FLAG] as "IsActive"
  FROM [TB_M_USER] tUser
  ORDER BY tUser.USERNAME  