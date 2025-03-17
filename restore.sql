/*
This script creates a new database, user, and table, and inserts a row into the table.



PUT THIS OUT OF COMMENTS FOR DEPLOYMENT
CREATE DATABASE [CleanUpsDatabase];
GO
*/

/*
to connect with sqlcmd
sqlcmd -S localhost -d CleanUpsDatabase -U CleanUpsUser -P CleanUpsPassword
*/



USE [CleanUpsDatabase];


-- Drop the USER and LOGIN if they exist
IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'CleanUpsUser')
BEGIN
    ALTER ROLE [db_owner] DROP MEMBER [CleanUpsUser];
    DROP USER [CleanUpsUser];
END

IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'CleanUpsUser')
BEGIN
    CREATE LOGIN CleanUpsUser WITH PASSWORD = 'CleanUpsPassword';
END

CREATE USER CleanUpsUser FOR LOGIN CleanUpsUser;
ALTER ROLE db_owner ADD MEMBER CleanUpsUser;


-- drop UserTrashCollection table
DROP TABLE IF EXISTS [UserTrashCollection];
-- drop Event table
DROP TABLE IF EXISTS [Event];
-- drop User table
DROP TABLE IF EXISTS [User];



CREATE TABLE [User] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,         	-- unique identifier for the user
    [Name] NVARCHAR(MAX) NOT NULL,              	-- name of the user
    [Email] NVARCHAR(MAX) NOT NULL,             	-- email of the user
    [Password] NVARCHAR(MAX) NOT NULL,          	-- password of the user
    [Role] INT NOT NULL DEFAULT 0,              	-- 0: User, 1: Organizer
    [Created] DATETIME NOT NULL DEFAULT GETDATE(),  -- date and time the user was created
);


CREATE TABLE [Event] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,         	    -- unique identifier for the event
    [Description] NVARCHAR(MAX) NOT NULL,       	    -- description of the event
    [Date] DATETIME NOT NULL,                   	    -- date of attending the event
    [Location] GEOGRAPHY NOT NULL,              	    -- location of the event
    [Picture] VARBINARY(MAX) NULL,              	    -- This column is not used in this example
    [Attendees] INT NOT NULL DEFAULT 0,         	    -- number of attendees
    [Status] INT NOT NULL DEFAULT 0,            	    -- 0: Upcoming, 1: Completed, 3: Cancelled
    [Created] DATETIME NOT NULL DEFAULT GETDATE(),      -- date and time the event was created
    [FamilyFriendly] BIT NOT NULL DEFAULT 0,    	    -- 0: Not family friendly, 1: Family friendly
    [OrganizerId] INT NOT NULL,                 	    -- unique identifier for the organizer
    [CollectedTrash] DECIMAL(10, 2) NULL,       	    -- For example, in kilograms or any unit you prefer
    FOREIGN KEY ([OrganizerId]) REFERENCES [User]([Id]) -- foreign key reference to the User table
);

CREATE TABLE [UserTrashCollection] (
    [UserID] INT,
    [EventID] INT,
    [TrashAmount] DECIMAL(10, 2),  -- For example, in kilograms or any unit you prefer
    [CollectionDate] DATETIME DEFAULT GETDATE(),
    PRIMARY KEY ([UserID], [EventID]),
    FOREIGN KEY ([UserID]) REFERENCES [User]([Id]),  -- Assuming you have a User table
    FOREIGN KEY ([EventID]) REFERENCES [Event]([Id])  -- Assuming you have an Event table
);






-- INSERT INTO [User] ([Name], [Email], [Password], [Role]) VALUES ('User 1', '[email protected]', 'Password1', 0);
-- INSERT INTO [Event] ([Description], [Date], [Location], [Attendees], [Status], [FamilyFriendly], [OrganizerId]) VALUES ('Event 1', '2021-01-01', geography::Point(47.6097, -122.3331, 4326), 0, 0, 0, 1);
-- SELECT * FROM [Event];
-- DELETE [dbo].[CreateEvent];
DROP PROCEDURE [CreateEvent];

GO
CREATE PROCEDURE [CreateEvent] 
    @Description NVARCHAR(MAX),
    @Date DATETIME,
    @Location GEOGRAPHY,
    @FamilyFriendly BIT,
    @OrganizerId INT
AS
BEGIN
    -- Declare a variable to store the role of the organizer
    DECLARE @OrganizerRole INT;

    -- Check if the OrganizerId exists and has the role of 1 (Organizer)
    SELECT @OrganizerRole = [Role] 
    FROM [User] 
    WHERE [Id] = @OrganizerId;

    -- If the role is not 1 (Organizer), return an error message
    IF @OrganizerRole != 1
    BEGIN
        PRINT 'Error: The provided OrganizerId is not valid or is not an Organizer.';
        RETURN;
    END

    -- Insert into the Event table if the organizer is valid
    DECLARE @EventId INT;

    INSERT INTO [Event] ([Description], [Date], [Location], [FamilyFriendly], [OrganizerId])
    VALUES (@Description, @Date, @Location, @FamilyFriendly, @OrganizerId);

    -- Get the last inserted EventId
    SET @EventId = SCOPE_IDENTITY();
END;
GO
DECLARE @_Location GEOGRAPHY = geography::Point(47.6097, -122.3331, 4326);

INSERT INTO [User] ([Name], [Email], [Password], [Role])
VALUES 
    ('User 1', '[email protected]', 'Password1', 0),
    ('Organizer 1', '[email protected]', 'Password1', 1);

EXEC dbo.CreateEvent 
    @Description = 'Event 1',
    @Date = '2021-01-01',
    @Location = @_Location,  -- Correctly use the variable here
    @FamilyFriendly = 0,
    @OrganizerId = 1;

EXEC dbo.CreateEvent 
    @Description = 'Event 1',
    @Date = '2021-01-01',
    @Location = @_Location,  -- Correctly use the variable here
    @FamilyFriendly = 0,
    @OrganizerId = 2;

SELECT * FROM [Event];
GO
-- update attendees
DROP PROCEDURE IF EXISTS [UpdateAttendees];
GO
CREATE PROCEDURE [UpdateAttendees] 
    @EventId INT,
    @Attendees INT
AS
BEGIN
    -- Update the Attendees column in the Event table
    UPDATE [Event]
    SET [Attendees] = @Attendees
    WHERE [Id] = @EventId;
END;
GO
DROP PROCEDURE IF EXISTS [GetAllEvents];
GO
CREATE PROCEDURE [GetAllEvents]
AS
BEGIN
    -- Select all events from the Event table
    SELECT * FROM [Event];
END;
GO
-- update status
DROP PROCEDURE IF EXISTS [UpdateStatus];
GO
CREATE PROCEDURE [UpdateStatus] 
    @EventId INT,
    @Status INT
AS
BEGIN
    -- Update the Status column in the Event table
    UPDATE [Event]
    SET [Status] = @Status
    WHERE [Id] = @EventId;
END;
GO
-- update family friendly
DROP PROCEDURE IF EXISTS [UpdateFamilyFriendly];
GO
CREATE PROCEDURE [UpdateFamilyFriendly] 
    @EventId INT,
    @FamilyFriendly BIT
AS
BEGIN
    -- Update the FamilyFriendly column in the Event table
    UPDATE [Event]
    SET [FamilyFriendly] = @FamilyFriendly
    WHERE [Id] = @EventId;
END;
GO
