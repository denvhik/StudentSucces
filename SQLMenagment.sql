CREATE ROLE [Teacher];
CREATE ROLE [Administrator];
CREATE ROLE [Student];
GO


GRANT SELECT ON [dbo].[Student] TO [Teacher];
GRANT SELECT ON [dbo].[Hobbie] To [Teacher];
GRANT SELECT ON [dbo].[StudentHobby] To [Teacher];
GRANT SELECT ON [dbo].[StudentGroup] TO [Teacher];
GRANT SELECT, INSERT, UPDATE, DELETE ON [dbo].[Book] TO [Teacher];
GRANT SELECT, INSERT, UPDATE, DELETE ON [dbo].[StudentSubject] TO [Teacher];

GRANT SELECT ON [dbo].[Hobbie] TO [student];
GRANT SELECT ON [dbo].[Student] TO [Student];
GRANT SELECT ON [dbo].[StudentHobby] To [Student];
GRANT SELECT ON [dbo].[StudentGroup] TO [Student];
GRANT SELECT ON [dbo].[StudentsDormitory] To [Student];
GRANT SELECT ON [dbo].[StudentSubject] To [Student]
GRANT SELECT, UPDATE ON [dbo].[StudentHobby] TO [Student];


GRANT SELECT, INSERT, UPDATE, DELETE ON [dbo].[Student] TO [Administrator];
GRANT SELECT, INSERT, UPDATE, DELETE ON [dbo].[Groups] TO [Administrator];
GRANT SELECT, INSERT, UPDATE, DELETE ON [dbo]. [Teacher] To [Administrator];
GRANT SELECT, INSERT, UPDATE, DELETE ON [dbo].[Subject] To [Administrator];

GO
CREATE TRIGGER [tr_Student_StudentAudiTime]
ON [dbo].[Student]
AFTER INSERT, UPDATE
AS
BEGIN
    UPDATE s
    SET s.CreatedBy =  (CAST(SUBSTRING(CAST(SUSER_SID() AS varbinary(85)), 1, 16) AS uniqueidentifier)),
        s.CreatedDateTime = GETDATE(),
        s.ModifiedBy =  (CAST(SUBSTRING(CAST(SUSER_SID() AS varbinary(85)), 1, 16) AS uniqueidentifier)),
        s.ModifiedDateTime = GETDATE()
    FROM dbo.Student s
    JOIN inserted i ON s.StudentID = i.StudentID;
END;
GO
DROP TRIGGER [tr_Student_StudentAudiTime];

CREATE OR ALTER TRIGGER [tr_Student_StudentGroupAudit]
ON
	[dbo]. [StudentGroup]
AFTER INSERT,UPDATE
AS
BEGIN
 UPDATE [s]
SET
	[s].[CreatedBy] =  (CAST(SUBSTRING(CAST(SUSER_SID() AS varbinary(85)), 1, 16) AS uniqueidentifier)),
	[s].[CreatedDateTime] = GETDATE(),
	[s].[ModifiedBy] =  (CAST(SUBSTRING(CAST(SUSER_SID() AS varbinary(85)), 1, 16) AS uniqueidentifier)),
	[s].[ModifiedDateTime] = GETDATE ()
FROM
	[dbo].[StudentGroup] [s]
JOIN
	[inserted] [i] ON [s].[StudentID] = [i].[StudentID];
END;

ALTER TABLE [dbo].[Hobbie]
ADD CONSTRAINT [Hobbie_CreatedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER,CONVERT(BINARY(16),SUSER_SID())) FOR [CreatedBy],
	CONSTRAINT [Hobbie_CreatedDateTime] DEFAULT GETDATE() FOR [CreatedDateTime],
	CONSTRAINT [Hobbie_ModifiedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER,CONVERT(BINARY(16),SUSER_SID())) FOR [ModifiedBy],
	CONSTRAINT [Hobbie_ModifiedDateTime] DEFAULT GETDATE() FOR [ModifiedDateTime];


ALTER TABLE [dbo].[Subject]
ADD CONSTRAINT [Subject_CreatedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER,CONVERT(BINARY(16),SUSER_SID())) FOR [CreatedBy],
	CONSTRAINT [Subject_CreatedDateTime] DEFAULT GETDATE() FOR [CreatedDateTime],
	CONSTRAINT [Subject_ModifiedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER,CONVERT(BINARY(16),SUSER_SID())) FOR [ModifiedBy],
	CONSTRAINT [Subject_ModifiedDateTime] DEFAULT GETDATE() FOR [ModifiedDateTime];

IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'TeacherLogin')
BEGIN
    CREATE LOGIN [TeacherLogin] WITH PASSWORD = 'Teacher'; 
END

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'TeacherUser' AND type = 'S')
BEGIN
    CREATE USER [TeacherUser] FOR LOGIN [TeacherLogin];
END


IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'StudentLogin')
BEGIN
    CREATE LOGIN [StudentLogin] WITH PASSWORD = 'Student'; 
END

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'StudentUser' AND type = 'S')
BEGIN
    CREATE USER [StudentUser] FOR LOGIN [StudentLogin];
END


IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'AdministratorLogin')
BEGIN
    CREATE LOGIN [AdministratorLogin] WITH PASSWORD = 'Admin'; 
END

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'AdministratorUser' AND type = 'S')
BEGIN
    CREATE USER [AdministratorUser] FOR LOGIN [AdministratorLogin];
END

ALTER ROLE [Administrator] ADD MEMBER [AdministratorLogin];
ALTER ROLE [Teacher] ADD MEMBER [TeacherLogin];
ALTER ROLE [Student] ADD MEMBER [StudentLogin];

