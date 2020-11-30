ALTER DATABASE testsdb SET ENABLE_BROKER  WITH ROLLBACK IMMEDIATE;
ALTER DATABASE testsdb SET TRUSTWORTHY ON  WITH ROLLBACK IMMEDIATE;


--DROP QUEUE ProductUpates; 

--DROP SERVICE ProductUpatesNotifications

Create QUEUE TestUpates; 

Create SERVICE TestUpatesNotifications
  ON QUEUE TestUpates  
([http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification]);  