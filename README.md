# MyDatabaseBackupService

## Overview

`MyDatabaseBackupService` is a Windows Service that automatically performs database backups at specified intervals(Using Task Scheduler In My case). The service is designed to back up the `MySchool` database on a schedule, storing backup files with a timestamp for easy identification. The service can operate both as a Windows Service and a Console Application for flexible use.

The service performs full backups on the first run and differential backups thereafter. Backups are stored in a configurable folder (`Backups`), and logs of service operations are stored in a `Logs` folder.

---

## Features

- Automatic backup of the `MySchool` database at regular intervals (configurable).
- Full backups if no prior full backup is available; otherwise, differential backups are created.
- Support for both Windows Service and Console Application modes.
- Logging of all service activities (startup, backup operations, errors, etc.).
- Configurable paths for backups and logs.

---

## Project Structure

1. **Service Class (`MyBackupService`)**: The main service logic, including start and stop operations.
2. **Logging Class (`clsLog`)**: Handles logging of service events and errors.
3. **Data Access Class (`clsServiceDataAccess`)**: Connects to the database and executes the backup stored procedure.
4. **Global Configuration Class (`clsGlobal`)**: Manages global settings such as the connection string, backup folder, and procedure name.
5. **Installer Class (`ProjectInstaller`)**: Installs the service as a Windows Service.
6. **Stored Procedure (`sp_MySchool_Backup`)**: Defines the logic for creating database backups (full and differential).

 ---

## SQL Stored Procedure

### `sp_MySchool_Backup`

This stored procedure is used to create the backup of the `MySchool` database. The procedure checks if a full backup exists; if not, it performs a full backup. If a full backup is already present, it performs a differential backup.

```sql

create PROCEDURE sp_MySchool_Backup
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Define the dynamic backup path
        DECLARE @BackupPath NVARCHAR(255);
        SET @BackupPath = 'C:\MyServices\MyBackupService\Backups\MySchool_' + 
                          REPLACE(CONVERT(NVARCHAR(20), GETDATE(), 120), ':', '-') + '.bak';

        -- Check if a full backup has already been performed
        DECLARE @FullBackupExists INT;
        SET @FullBackupExists = (
            SELECT COUNT(*) 
            FROM msdb.dbo.backupset
            WHERE database_name = 'MySchool'
              AND type = 'D'  -- 'D' is for full backup
              AND backup_finish_date IS NOT NULL
        );

        IF @FullBackupExists = 0
        BEGIN
            -- Perform a full backup if no full backup exists
            PRINT 'Performing full backup...';
            BACKUP DATABASE MySchool
            TO DISK = @BackupPath
            WITH INIT;  -- INIT to overwrite if any file already exists
        END
        ELSE
        BEGIN
            -- Perform a differential backup if a full backup exists
            PRINT 'Performing differential backup...';
            BACKUP DATABASE MySchool
            TO DISK = @BackupPath
            WITH DIFFERENTIAL;
        END

        -- Informational message For MY SQL Server Debug
        PRINT 'Backup completed successfully. Backup path: ' + @BackupPath;
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR('Backup failed: %s', 16, 1, @ErrorMessage);
    END CATCH
END;
```
---

## Prerequisites

Before setting up the service, ensure the following:

- **SQL Server**: The service requires an SQL Server instance with the `MySchool` database.
- **.NET Framework 4.8.1**: The service is built using .NET Framework 4.8.1.
- **Directory Permissions**: The backup and log directories must be writable by the service.

---

## Installation

### 1. **Build the Service**

- Clone or download the project.
- Open the solution in **Visual Studio**.
- Build the solution.

### 2. **Configure Settings**

In the `App.config` file, configure the following settings:

- **ConnectionString**: Update the connection string to point to your SQL Server instance.
- **BackupFolder**: Specify the folder where the backups will be saved (must be accessible by the service).
- **LogFolder**: Specify the folder where logs will be saved.
- **DatabaseName**: Ensure this matches the name of the database you're backing up (e.g., `MySchool`).

### 3. **Install the Service**

To install the service:

1. Open **Command Prompt** as an Administrator.
2. Navigate to the folder where the service executable (`MyDatabaseBackupService.exe`) is located.
3. Run the following command:

    ```bash
    "C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" MyDatabaseBackupService.exe
    ```

This will install the service on your machine.

### 4. **Start the Service**

you can Start it from the command prompt As Administrator:

```bash
sc start MyBackupService
```

### 5. **Stop the Service**

you can stop it from the command prompt  As Administrator:

```bash
sc stop MyBackupService
```

### 6. **Delete the Service**

you can Delete it from the command prompt  As Administrator:

```bash
sc delete MyBackupService
```
